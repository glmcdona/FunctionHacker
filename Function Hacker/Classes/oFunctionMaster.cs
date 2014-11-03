using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Forms;
using Program_Execution_Flow_Hacker_and_Visualizer.Memory_Classes;

namespace FunctionHacker.Classes
{
    public static class oFunctionMaster
    {
        public static List<oFunction> functions = null;
        public static Hashtable destinationToFunction; // Objects are of type oFunction
        static public int numCalls;
        static public int numFunctions;
        private static uint injectedHeapAddress1 = 0;
        private static uint injectedHeapAddress2 = 0;
        private static uint injectedHeapSize1 = 0;
        private static uint injectedHeapSize2 = 0;
        private static uint table_addressValidReadTable = 0;
        private static uint table_addressInvalidSourceTable = 0;
        private static uint dereferenceSizeAddress = 0;

        // Circular buffer readers
        public static oCircularBufferReader reader_visualization;


        /// <summary>
        /// Updates the code injections with the latest settings. Number of bytes to dereference and number of calls to dereference.
        /// </summary>
        public static void applySettings()
        {
            // Update the code injects with the latest settings
            oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, dereferenceSizeAddress, (uint) Properties.Settings.Default.NumDereferencedBytes);
        }

        public static void reset()
        {
            // Clean up the functions
            if (functions != null)
            {
                foreach (oFunction function in functions)
                {
                    function.cleanup();
                }
            }

            //Thread.Sleep(10); // A failsafe to make sure we do not release the memory while recording.

            // Cleanup the injected heaps
            if (oProcess.activeProcess != null)
            {
                if (injectedHeapAddress1 != 0 && injectedHeapSize1 != 0)
                {
                    oMemoryFunctions.VirtualFreeEx(oProcess.activeProcess.Handle, (IntPtr) injectedHeapAddress1,
                                                   (int) injectedHeapSize1, 0x8000); // MEM_RELEASE
                }
                if (injectedHeapAddress2 != 0 && injectedHeapSize2 != 0)
                {
                    oMemoryFunctions.VirtualFreeEx(oProcess.activeProcess.Handle, (IntPtr) injectedHeapAddress2,
                                                   (int) injectedHeapSize2, 0x8000); // MEM_RELEASE
                }
            }

            functions = null;
            destinationToFunction = null;
            numCalls = 0;
            numFunctions = 0;
        }


        /// <summary>
        /// Returns the list of oFunctions corresponding to the list of function addresses.
        /// </summary>
        /// <param name="functionAddresses">List of function addresses.</param>
        /// <returns></returns>
        public static List<oFunction> getFunctionListFromAddressList(List<uint> functionAddresses)
        {
            if( functions == null || functionAddresses == null || destinationToFunction == null )
                return new List<oFunction>(0);

            // Convert formats.
            List<oFunction> result = new List<oFunction>(functionAddresses.Count);
            for( int i = 0; i < functionAddresses.Count; i++ )
                result.Add( (oFunction)destinationToFunction[functionAddresses[i]] );

            return result;
        }

        /// <summary>
        /// This function injects the instrumentation code and
        /// redirects the call instructions and pe tables.
        /// </summary>
        public static bool inject(uint bufferSize, uint maxCallCount, List<HEAP_INFO> invalidCallSourceHeaps, Form parent)
        {
            if( functions.Count > 0 )
            {
                // Create the progress bar
                formProgress progress = new formProgress(parent);
                progress.Show();
                progress.setMin(0);
                progress.setMax(functions.Count / 457);
                progress.setTitle("Injecting Function Instrumentation Code...");
                progress.setLabel1("Allocating buffer space...");
                progress.setLabel2("");

                // Create and setup the circular buffer recording buffer for the visualizations.
                uint visualizationAddress = (uint) oMemoryFunctions.VirtualAllocEx(oProcess.activeProcess.Handle,
                                                                                   (IntPtr) 0,
                                                                                   20000000 + 1000,
                                                                                   (uint) (MEMORY_STATE.COMMIT),
                                                                                   (uint) MEMORY_PROTECT.PAGE_READWRITE
                                                       );
                injectedHeapAddress1 = visualizationAddress;
                injectedHeapSize1 = 20000000 + 1000;


                oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, visualizationAddress+4, 0);
                oAssemblyGenerator.addCommonAddress(visualizationAddress+4, "VIS_AD_CIRCULAR_OFFSET");
                oAssemblyGenerator.addCommonAddress(bufferSize * 1000000 - 8 - 4, "VIS_CIRCULAR_SIZE");
                oAssemblyGenerator.addCommonAddress(visualizationAddress+16+4, "VIS_AD_CIRCULAR_BASE");
                oAssemblyGenerator.addCommonAddress(0, "mainRecordFunction");

                

                //MessageBox.Show((visualizationAddress + 16).ToString("X"));

                // Create the visualization buffer reader
                reader_visualization = new oCircularBufferReader(visualizationAddress + 16 + 4, bufferSize * 1000000 - 4, visualizationAddress + 4);

                // Set the global buffer parameters
                oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, visualizationAddress, maxCallCount);
                oAssemblyGenerator.addCommonAddress(visualizationAddress, "MAXCALLS");


                // Determine the size per function
                int sizePerFunc = ((oFunction)functions[0]).generateInstrumentation(10000).Length;

                // Allocate the memory heap for the funciton code injections
                uint currentAddress = (uint)oMemoryFunctions.VirtualAllocEx(oProcess.activeProcess.Handle,
                                                    (IntPtr) 0,
                                                    functions.Count * sizePerFunc + 1000 + 2*0x80000, // 1000 + 2*0x80000 bytes for the mainRecordFunction
                                                    (uint)(MEMORY_STATE.COMMIT),
                                                    (uint)MEMORY_PROTECT.PAGE_EXECUTE_READWRITE
                                                );
                injectedHeapAddress2 = currentAddress;
                injectedHeapSize2 = (uint) (functions.Count * sizePerFunc + 1000 + 2*0x80000);

                if (currentAddress == 0)
                {
                    oConsole.printMessageShow("ERROR: Could not allocate memory space in target process for code injections.");
                    progress.Dispose();
                    return false;
                }

                
                
                progress.setLabel1("Injecting common base functions." );

                // Inject the common function
                oAssemblyGenerator.addCommonAddress(currentAddress, "mainRecordFunction");
                uint size = (uint) oFunction.injectCommonCode(currentAddress);

                // Generate and write the valid read pointer table
                progress.setLabel1("Generating and injecting the read pointer dereference lookup table.");

                // Get the address of the table
                table_addressValidReadTable = UInt32.Parse(oMemoryFunctions.ReverseString(oAssemblyGenerator.evaluateCommonAddress("sLOOKUPVALIDPOINTER", 0, currentAddress)), NumberStyles.HexNumber);
                updateValidReadPointerTable();

                // Generate and write the excluded call source table
                progress.setLabel1("Generating and injecting the exclusion source lookup table.");

                // Get the address of the table
                table_addressInvalidSourceTable = UInt32.Parse(oMemoryFunctions.ReverseString(oAssemblyGenerator.evaluateCommonAddress("sLOOKUPINVALIDSOURCE", 0, currentAddress)), NumberStyles.HexNumber);
                updateInvalidCallSourceTable(invalidCallSourceHeaps);

                // Set the size to dereference option
                dereferenceSizeAddress = UInt32.Parse(oMemoryFunctions.ReverseString(oAssemblyGenerator.evaluateCommonAddress("sSTRINGDEREFERENCESIZE", 0, currentAddress)), NumberStyles.HexNumber);
                oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, dereferenceSizeAddress, (uint) Properties.Settings.Default.NumDereferencedBytes);

                currentAddress += size;

                // Perform the intecept function code injection
                int count = 0;
                progress.setLabel1("Injecting instrumentation functions and redirecting functions: " + count.ToString() + " of " + functions.Count.ToString());
                foreach (oFunction function in functions)
                {
                    // Inject the instrumentation functions
                    uint injectionAddress = currentAddress;
                    size = (uint) function.injectInstrumentation(currentAddress);

                    // Update the function parameters
                    function.addressCount = UInt32.Parse(oMemoryFunctions.ReverseString(oAssemblyGenerator.evaluateCommonAddress("sCOUNT", 0, currentAddress)), NumberStyles.HexNumber);
                    function.addressRecord = UInt32.Parse(oMemoryFunctions.ReverseString(oAssemblyGenerator.evaluateCommonAddress("sSAVEDATA", 0, currentAddress)), NumberStyles.HexNumber);

                    // Redirect the function calls
                    function.redirect(injectionAddress);
                    currentAddress += size;
                    
                    // Update the progress bar
                    count++;
                    if ((count % 457) == 0)
                    {
                        progress.setLabel1("Injecting instrumentation functions and redirecting functions: " + count.ToString() + " of " + functions.Count.ToString());
                        progress.increment();
                    }
                }

                progress.Dispose();
            }
            return true;
        }

        public static void updateValidReadPointerTable()
        {
            // Generate the table
            List<HEAP_INFO> heaps = oMemoryFunctions.GenerateMemoryMapFast(oProcess.activeProcess);
            byte[] table = oMemoryFunctions.GenerateValidReadPointerTableFromMap(heaps);

            // Write the table
            oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, table_addressValidReadTable, table);
        }

        public static void updateInvalidCallSourceTable(List<HEAP_INFO> invalidCallSourceHeaps)
        {
            // Generate the table
            byte[] table = oMemoryFunctions.GenerateInvalidCallSourceTable(invalidCallSourceHeaps);

            // Write the table
            oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, table_addressInvalidSourceTable, table);
        }


        public static void addCall(uint source, uint destination, oFunction.CALL_TYPE type, string name)
        {
            if (functions == null)
            {
                // Initialize
                functions = new List<oFunction>(10000);
                destinationToFunction = new Hashtable(20000);
            }

            // The source or destination cannot be in the kernel address space.
            if (destination > 0x80000000 || source > 0x80000000)
                return;

            // Check if this function destination is a jump table call
            if (type == oFunction.CALL_TYPE.FIXED_OFFSET || type == oFunction.CALL_TYPE.JUMP_TABLE_PE || type == oFunction.CALL_TYPE.CALLBACK_TABLE)
            {
                byte[] data = oMemoryFunctions.ReadMemory(oProcess.activeProcess, destination, 2);

                // Codes: E9 jump, EB short jump, FF 25 jump off fixed offset
                if (data.Length == 2 && (data[0] == 0xE9 || (data[0] == 0xFF && data[1] == 0x25)))
                {
                    // This is a jump table call, we need to extract the actual call destination
                    if (data[0] == 0xE9)
                    {
                        // Far jump
                        uint offset = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, destination + 1);
                        destination = destination + offset + 5;
                    }
                    else if (data[0] == 0xEB)
                    {
                        // Short jump
                        uint offset = (uint) oMemoryFunctions.ReadMemoryByte(oProcess.activeProcess, destination + 1);
                        destination = destination + offset + 2;
                    }
                    else if (data[0] == 0xFF && data[1] == 0x25)
                    {
                        // Read the address
                        uint address = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, destination + 2);
                        uint destinationNew = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, address);

                        if (destinationNew == 0)
                            return;
                        // This is a PE call jump table call, but the linked address has not been loaded yet.

                        // Standard PE jump table format
                        type = oFunction.CALL_TYPE.JUMP_TABLE_PE;
                        destination = destinationNew;
                    }

                    // Recurse this add call, incase we have a jump chain.
                    addCall(source, destination, type, name);
                    return;
                }
            }

            // Add the call to the system
            if (destinationToFunction.ContainsKey(destination))
            {
                // Function already exists, add this call source.
                ((oFunction)destinationToFunction[destination]).addCaller(source, type, name);

                numCalls++;
            }
            else
            {
                // Function does not exist, create it
                numCalls++;
                numFunctions++;

                // Add the call to the function list
                functions.Add(new oFunction(source, destination, type, name));

                // Add the function to the hash table
                destinationToFunction.Add(destination, functions[functions.Count - 1]);
            }
        }

        public static void addCall(uint source, uint destination, oFunction.CALL_TYPE type)
        {
            addCall(source, destination, type, ""); // add a call with no known name
        }

        /// <summary>
        /// Estimates the number of input parameters of each funciton.
        /// </summary>
        /// <param name="retList">Return instruction list.</param>
        public static void estimateFunctionParameters(oAsmRetList retList, oEbpArgumentList ebpAddresses, Form parent)
        {
            if( functions == null ) functions = new List<oFunction>();

            // Estimate the number of parameters for each function return
            int count = 0;
            formProgress progress = new formProgress(parent);
            progress.Show(parent);
            progress.setMin(0);
            progress.setMax(oFunctionMaster.numFunctions/273);
            progress.setTitle("Estimating Number Parameter Counts...");
            progress.setLabel1("Estimating Function Input Parameter Counts: " + count.ToString() + "0 of " + oFunctionMaster.numFunctions.ToString() );
            progress.setLabel2("");

            // Loop through each function, estimating the number of parameters
            List<oFunction> invalidFunctions = new List<oFunction>(0);
            foreach( oFunction function in functions )
            {
                // Estimate parameters
                function.estimateNumParameters(retList, ebpAddresses);

                // Check the number of parameters is valid
                if (function.getNumParams() > 50)
                {
                    // Invalid function
                    invalidFunctions.Add( function );
                }

                count++;
                if(count%273 == 0)
                {
                    progress.setLabel1("Estimating Function Input Parameter Counts: " + count.ToString() + "0 of " + oFunctionMaster.numFunctions.ToString());
                    progress.increment();
                }
            }

            // Remove the invalid functions
            foreach( oFunction function in invalidFunctions )
            {
                functions.Remove(function);
            }
            

            progress.Dispose();
        }

        public static void clipToInterModular()
        {
            // Clip the function list to inter-modular calls only.
            FunctionSelectIntermodular selecter = new FunctionSelectIntermodular();
            functions = functions.FindAll(selecter.isInterModular);
        }
    }
}
