using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Classes.Disassembly;

namespace FunctionHacker.Classes
{
    public struct ARGUMENT_STRING_COLLECTION
    {
        public List<string> names;
        public List<string> values;

        public string toString()
        {
            string result = "";

            for( int i = 0; i < names.Count && i < values.Count; i++)
            {
                result = result + names[i] + "=" + values[i] + Environment.NewLine;
            }

            return result.Trim();
        }

    }

    public class FunctionSelectAddress
    {
        private uint functionAddress;

        public FunctionSelectAddress(uint functionAddress)
        {
            this.functionAddress = functionAddress;
        }



        public bool isFunctionAddress(oFunction function)
        {
            if (function.address == functionAddress)
            {
                return true;
            }
            {
                return false;
            }

        }
    }

    public class FunctionCompareAddress : IComparer<oFunction>
    {
        int IComparer<oFunction>.Compare(oFunction a, oFunction b)
        {
            return a.address.CompareTo(b.address);
        }
    }

    public class FunctionSelectIntermodular
    {

        public FunctionSelectIntermodular()
        {

        }

        public bool isInterModular(oFunction call)
        {
            if (call.peCallers.Count > 0)
            {
                // Remove the intra-modular calls
                //call.normalCallers = new ArrayList(0);
                return true;
            }
            {
                return false;
            }

        }
    }

    public class FunctionSelectFromAddressArray
    {
        private List<uint> functions;
        public FunctionSelectFromAddressArray(List<uint> functions)
        {
            this.functions = functions;
        }

        public bool isInList(oFunction call)
        {
            return functions.Contains(call.address);
        }

        public bool isNotInList(oFunction call)
        {
            return !functions.Contains(call.address);
        }
    }

    public enum STACK_CLEANUP
    {
        BY_CALLER,
        BY_CALLED
    }

    public class oFunction
    {
        public uint address;
        public uint addressCount;
        public uint addressRecord;
        private uint numParams;
        public byte[] originalFirstBytes;
        public byte[] functionStartBytes;
        public string name;
        public bool disabled = false;
        public List<uint> normalCallers;
        public List<uint> peCallers;
        public List<uint> jumpTablePECallers;
        public List<uint> callbackTableCallers;
        public List<uint> peExports;
        public STACK_CLEANUP cleanupMethod;
        private List<oArgument> arguments;

        private bool cleanedUp = false;


        ~oFunction()
        {
            cleanup();
        }

        public void cleanup()
        {
            // Redirect the functions back to their original state
            
            // Clean up the callers
            try
            {
                if (!cleanedUp)
                {
                    foreach (uint callerAddress in normalCallers)
                    {
                        int offset = (int) address - (int) (callerAddress + 5);
                        oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, callerAddress + 1,
                                                          address - (callerAddress + 5));
                    }

                    foreach (uint callerAddress in peCallers)
                    {
                        oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, callerAddress, address);
                    }

                    cleanedUp = true;
                }
            }catch
            {
                // Ignore all exceptions during cleanup.
            }
        }

        public oFunction(uint source, uint destination, CALL_TYPE type, string name)
        {
            // Initialize
            this.address = destination;
            normalCallers = new List<uint>(0);
            peCallers = new List<uint>(0);
            peExports = new List<uint>(0);
            jumpTablePECallers = new List<uint>(0);
            callbackTableCallers = new List<uint>(0);
            this.name = "";

            // Set the stack arguments
            arguments = new List<oArgument>(3);
            arguments.Add(new oArgument("mov ecx, ", "ecx"));
            arguments.Add(new oArgument("mov edx, ", "edx"));
            arguments.Add(new oArgument("mov eax, ", "eax"));

            // Add the first caller
            addCaller(source, type, name);
        }

        public void setNumParams(uint numParams)
        {
            this.numParams = numParams;
            
            // Initialize these stack arguments

            // We need to remove the old stack arguments. (3 register arguments)
            while( arguments.Count > 3 )
                arguments.RemoveAt(arguments.Count-1);

            // Add the stack arguments
            for( int i = 0; i < numParams; i++)
            {
                // Add this stack argument
                arguments.Add(new oArgument("push ", "[ebp+"+ (i*4+8).ToString("X") + "]"));
            }
        }

        public uint getNumParams()
        {
            return numParams;
        }

        /// <summary>
        /// This generates the argument string based on the calling convention of this
        /// function. It truncates the arguments at around the specified pixel width of the column.
        /// </summary>
        /// <param name="callRecording"></param>
        /// <param name="columnWidth"></param>
        /// <returns></returns>
        public ARGUMENT_STRING_COLLECTION getArgumentString(oSingleData callRecording, int columnWidth, Font font)
        {
            // Get the argument string collection
            ARGUMENT_STRING_COLLECTION arguments = getArgumentString(callRecording);

            // Truncate the string values to approximately the right size
            for( int i = 0; i < arguments.values.Count; i++)
            {
                // Test the indexTooLong value
                string value = arguments.values[i];
                string name = arguments.names[i];
                Size size = TextRenderer.MeasureText(name + "=" + value, font);
                if( size.Width > columnWidth - 2 && value.Length > 5 )
                {
                    // Too long of a string

                    int indexTooLong = value.Length;
                    int indexTooShort = 0;

                    // Make our next guess, that will reduce the set in half
                    int guess = (indexTooLong - indexTooShort) / 2 + indexTooShort; // Rounding down is nice.

                    while (guess != indexTooShort)
                    {
                        // Test this guess length
                        if (TextRenderer.MeasureText(name + "=" + value.Substring(0, guess) + "...", font).Width > columnWidth - 2)
                            indexTooLong = guess;
                        else
                            indexTooShort = guess;

                        // Make the next guess
                        guess = (indexTooLong - indexTooShort)/2 + indexTooShort;
                    }

                    // We found the prefect length that is just shorter than the column width with the "..."
                    arguments.values[i] = value.Substring(0, guess) + "...";
                }
            }

            return arguments;
        }

        /// <summary>
        /// This generates the argument string based on the calling convention of this
        /// function.
        /// </summary>
        /// <param name="callRecording"></param>
        public ARGUMENT_STRING_COLLECTION getArgumentString(oSingleData callRecording)
        {
            ARGUMENT_STRING_COLLECTION result = new ARGUMENT_STRING_COLLECTION();
            result.names = new List<string>((int)numParams + 3);
            result.values = new List<string>((int)numParams + 3);
            if( arguments.Count < 3 ) // Must have all three registers at least.
                return result;

            int dereferenceIndex = callRecording.dereferences.Count()-1;
            string deref;

            // ECX, __fastcall argument 1, __thiscall 'this' pointer
            if (dereferenceIndex >= 0 &&
                callRecording.dereferences[dereferenceIndex].argumentIndex == 0 )
            {
                // ECX with dereference
                result.values.Add(arguments[0].getValueString(callRecording.ecx, callRecording.dereferences[dereferenceIndex]));
                result.names.Add(arguments[0].getName() );
                dereferenceIndex--;
            }
            else
            {
                // ECX no dereference
                result.values.Add(arguments[0].getValueString(callRecording.ecx));
                result.names.Add(arguments[0].getName());
            }

            // EDX, __fastcall argument 2
            if (dereferenceIndex >= 0 &&
                callRecording.dereferences[dereferenceIndex].argumentIndex == 1)
            {
                // EDX with dereference
                result.values.Add(arguments[1].getValueString(callRecording.edx, callRecording.dereferences[dereferenceIndex]));
                result.names.Add(arguments[1].getName());
                dereferenceIndex--;
            }
            else
            {
                // EDX no dereference
                result.values.Add(arguments[1].getValueString(callRecording.edx));
                result.names.Add(arguments[1].getName());
            }

            // EAX, __fastcall argument 3 on borland
            if (dereferenceIndex >= 0 &&
                callRecording.dereferences[dereferenceIndex].argumentIndex == 2)
            {
                // EAX with dereference
                result.values.Add(arguments[2].getValueString(callRecording.eax, callRecording.dereferences[dereferenceIndex]));
                result.names.Add(arguments[2].getName());
                dereferenceIndex--;
            }
            else
            {
                // EAX no dereference
                result.values.Add(arguments[2].getValueString(callRecording.eax));
                result.names.Add(arguments[2].getName());
            }
            
            // STACK ARGUMENTS

            // Create room for the variables
            for (int i = 0; i < callRecording.arguments.Count(); i++ )
            {
                result.values.Add("");
                result.names.Add("");
            }

            // Load the data into the variables
            for (int i = callRecording.arguments.Count() - 1; i >= 0; i--)
            {
                if (dereferenceIndex >= 0 &&
                    callRecording.dereferences[dereferenceIndex].argumentIndex == i + 4)
                {
                    // Stack argument with dereference
                    result.values[i + 3] = (arguments[i + 3].getValueString(callRecording.arguments[i], callRecording.dereferences[dereferenceIndex]));
                    result.names[i + 3] = arguments[i + 3].getName();
                    dereferenceIndex--;
                }
                else
                {
                    // Stack argument no dereference
                    result.values[i + 3] = arguments[i + 3].getValueString(callRecording.arguments[i]);
                    result.names[i + 3] = arguments[i + 3].getName();
                }
            }

            return result;
        }

        private int findArgumentRowInArgumentString(List<string> lines, string argument)
        {
            for( int i = 0; i < lines.Count(); i++ )
            {
                // Find the argument name
                string[] split = lines[i].Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Count() > 0)
                {
                    if( argument.CompareTo(split[0]) == 0 )
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Begins recording data by setting the record flag to 1 for this function.
        /// </summary>
        public void startRecording()
        {
            // Set the record flag to 1 to start recording.
            oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess,addressRecord,1);

            // Reset the call count for this function
            oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, addressCount, 0);
        }

        /// <summary>
        /// Stops recording data by setting the record flag to 0 for this function.
        /// </summary>
        public void stopRecording()
        {
            // Set the record flag to 0 to stop recording.
            oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, addressRecord, 0);
        }



        /// <summary>
        /// Adds the caller of the specified type.
        /// </summary>
        /// <param name="source">Address of calling function or address of pe table entry.</param>
        /// <param name="type">Whether it is a direct fixed offset call, or a pe table entry.</param>
        public void addCaller(uint source, CALL_TYPE type, string suggestedName)
        {
            switch (type)
            {
                case CALL_TYPE.FIXED_OFFSET:
                    normalCallers.Add(source);
                    break;
                case CALL_TYPE.PE_TABLE:
                    peCallers.Add(source);
                    break;
                case CALL_TYPE.JUMP_TABLE_PE:
                    jumpTablePECallers.Add(source);
                    break;
                case CALL_TYPE.CALLBACK_TABLE:
                    callbackTableCallers.Add(source);
                    break;
                case CALL_TYPE.PE_EXPORT:
                    peExports.Add(source);
                    break;
                default:
                    MessageBox.Show("ERROR: Invalid add caller type.");
                    break;
            }

            // Check if it's name has been assigned
            if (suggestedName != "")
            {
                // Decide whether to use this new name
                if (name == "")
                {
                    name = suggestedName;
                }
                else if (name.Contains("ordinal"))
                {
                    name = suggestedName; // "ordinal" is a weak name. Overwrite it with the new name.
                }
                else if (name.Contains("obfuscated"))
                {
                    name = suggestedName; // "obfuscated" is a weak name. Overwrite it with the new name.
                }
                else if (name.Contains("vtable"))
                {
                    name = suggestedName; // "vtable" is a weak name. Overwrite it with the new name.
                }
                else if (!name.Contains(suggestedName))
                {
                    name = name + "/" + suggestedName;
                }
            }
        }

        /// <summary>
        /// Estimates the number of arguments by looking for the ret statement
        /// corresponding to this function address.
        /// </summary>
        /// <param name="list">List of ret addresses.</param>
        /// <param name="ebpAddresses"></param>
        public void estimateNumParameters(oAsmRetList list, oEbpArgumentList ebpAddresses)
        {
            // Calculate the number of args popped
            uint functionEnd = 0;
            uint numArgs = list.guessNumArguments(address, out functionEnd) / 4;
            this.cleanupMethod = STACK_CLEANUP.BY_CALLED;

            if( numArgs == 0 )
            {
                // We have no stack-cleaned up arguments, so lets see if any arguments on the stack are referenced.
                // Calculate the number of arguments referenced based on EBP measurements.
                this.cleanupMethod = STACK_CLEANUP.BY_CALLER;
                numArgs = (uint) ebpAddresses.guessNumArguments(address, functionEnd);
            }

            // Set the number of parameters now
            setNumParams(numArgs);
        }

        /// <summary>
        /// This redircts all the pe table entries and
        /// callers to the specified target address.
        /// </summary>
        /// <param name="target">Target address</param>
        public void redirect(uint target)
        {
            /*if (peExports.Count > 0)
            {
                // We need to insert our redirection at the start of the function
                byte[] offset = oMemoryFunctions.dwordToByteArray(target - (address + 5));
                byte[] jmpInstr = new byte[] { 0xe9, offset[0], offset[1], offset[2], offset[3] };
                oMemoryFunctions.writeMemoryByteArray(oProcess.activeProcess, address, jmpInstr);
            }
            else*/
            {
                // Redirect the PE table entries
                foreach (uint call in peCallers)
                {
                    // Redirect this pe table entry
                    oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, call, target);
                }

                // Redirect the export table entries
                foreach (uint call in peExports)
                {
                    uint value = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, call);
                    value = target - (uint) (oMemoryFunctions.LookupAddressInMap(oProcess.map, call).heapAddress - 0x1000);

                    oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, call, value);
                }

                // Redirect the fixed offset call instructions
                foreach (uint call in normalCallers)
                {
                    // Redirect this call instruction
                    int offset = (int) target - (int) (call + 5);
                    oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, call + 1, (uint) offset);
                }

                // Redirect the callback table callers
                foreach (uint call in callbackTableCallers)
                {
                    // Redirect this call instruction after verifying that it is still pointing to here
                    uint value = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, call);
                    if (value == this.address)
                        oMemoryFunctions.WriteMemoryDword(oProcess.activeProcess, call, target);
                }

                // We don't need to redirect jump table calls because the PE table is already redirected
            }
        }


        /// <summary>
        /// Injects code which is used in common by all the function injections.
        /// </summary>
        /// <param name="currentAddress">The address at which to inject the code.</param>
        /// <returns>Size of injected code.</returns>
        static public int injectCommonCode(uint currentAddress)
        {
            // Generate the instrumentation
            byte[] data = oAssemblyGenerator.buildInjection(currentAddress, oAssemblyCode.mainRecordFunction,
                                                            oProcess.activeProcess, 0, 0);

            // Inject the instrumentation
            oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, currentAddress, data);

            return data.Length;
        }

        /// <summary>
        /// This function injects the instrumentation code into the specified
        /// current address.
        /// </summary>
        /// <param name="currentAddress">The current address in the allocated heap for code injection.</param>
        /// <returns>Length of injected code to be added to the currentAddress.</returns>
        public int injectInstrumentation(uint currentAddress)
        {
            /*if (peExports.Count > 0)
            {
                // We need to inject the endpoint-measuring code, this differs in that it needs to execute
                // the originally-overwritten code prior to returning to the function.
                functionStartBytes =
                    LengthDecoder.getMinFiveBytesCode(oMemoryFunctions.readMemory(oProcess.activeProcess, address, 16));

                // Build the instrumentation
                byte[] data = generateInstrumentationEndpoint(currentAddress, functionStartBytes);

                // Inject the instrumentation
                oMemoryFunctions.writeMemoryByteArray(oProcess.activeProcess, currentAddress, data);

                return data.Length;
            }
            else*/
            {
                
                // Generate the instrumentation
                byte[] data = generateInstrumentation(currentAddress);

                // Inject the instrumentation
                oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, currentAddress, data);

                return data.Length;
            }
        }

        /*private byte[] generateInstrumentationEndpoint(uint injectAddress, byte[] replacedCodeBytes)
        {
            // Build the byte-sequence code
            string replacedCode = "";
            for (int i = 0; i < replacedCodeBytes.Count(); i++)
            {
                string byteString = replacedCodeBytes[i].ToString("X");
                while (byteString.Length < 2)
                    byteString = "0" + byteString;

                replacedCode = replacedCode + byteString;
            }

            // Set the number of argument parameter
            oAssemblyGenerator.addCommonAddress(this.numParams, "numParameters");

            // Build the assembly
            return oAssemblyGenerator.buildInjection(injectAddress, oAssemblyGenerator.processLabels(oAssemblyCode.mainInjectionEndpoint.Replace("<replacedCode>",replacedCode)), oProcess.activeProcess, (uint) (this.address + replacedCodeBytes.Length), 0);
        }*/

        /// <summary>
        /// This generates the code injection associated with this 
        /// </summary>
        /// <param name="injectAddress"></param>
        /// <returns></returns>
        public byte[] generateInstrumentation(uint injectAddress)
        {
            // Set the flag to filter invalid call sources if we are an intermodular call
            if (peExports.Count > 0 || peCallers.Count > 0)
            {
                oAssemblyGenerator.addCommonAddress(1, "useInvalidSourceTable");
            }else
            {
                oAssemblyGenerator.addCommonAddress(0, "useInvalidSourceTable");
            }

            // Set the number of argument parameter
            oAssemblyGenerator.addCommonAddress(this.numParams, "numParameters");

            // Build the assembly
            return oAssemblyGenerator.buildInjection(injectAddress, oAssemblyGenerator.processLabels(oAssemblyCode.mainInjection), oProcess.activeProcess, this.address, 0);
        }

        public static string injectionPreHandler =
                                            // Locals setup
                                            "push ebp\r\n" +
                                            "mov ebp, esp\r\n" +

                                            // Inintialize the exception occurred value
                                            "call 00000000\r\n" +
                                            "pop edi\r\n" +
                                            "add edi, <offset.EXCEPTIONOCCURRED>\r\n" +
                                            "add edi, 07000000\r\n" + // offset correction
                                            "mov [edi], 00000000\r\n" +

                                            // Setup the exception handler address
                                            "call 00000000\r\n" +
                                            "pop eax\r\n" +
                                            "add eax, <offset.catch>\r\n" +
                                            "add eax, 06000000\r\n" +

                                            // Setup the exception handler address
                                            "call 00000000\r\n" +
                                            "pop edi\r\n" +
                                            "add edi, <offset.exceptionHandler>\r\n" +
                                            "add edi, 07000000\r\n";

                                        // Add it to the exception chain
                                        //"push ebp\r\n" +
                                        //"push 00000000\r\n" +
                                        //"push 00000000\r\n" +
                                        //"push eax\r\n" + // safe place
                                        //"push edi\r\n"; // handler
                                        //"push fs:[0]\r\n" + // next seh
                                        //"mov fs:[0], esp\r\n" +

                                        //"movdfl 00000000, eax "; // test exception
                                        

        public static string registerArguments =
                                        // Setup the register argument inputs
                                        "mov ecx, #ECX#\r\n" +
                                        "mov edx, #EDX#\r\n" +
                                        "mov eax, #EAX#";

        public static string mainCode = // Make the actual call
                                        "mov edi, <destination>\r\n" +
                                        "call edi\r\n";
                                        //"pop fs:[0]\r\n"; // Cleanup the exception handler
                                        //"add esp, 04000000\r\n";
        public static string mainCodePost =
                                        // Record the return value
                                        "movdfl <RETURNVALUE>, eax\r\n" +
                                        "pop ebp\r\n " +                                        
                                        "ret\r\n" +
                                        "\r\n" +
                                        ";RETURNVALUE:\r\n" +
                                        "(4)\r\n";
        public static string injectionPostHandler =
                                        // Record that the exception occurred
                                        ";exceptionHandler:\r\n" +
                                        "int3 " +
                                        "mov eax, 00000000 " +
                                        "ret " +
                                        //"mov eax, 00000000 " +

                                        

                                        "push ebp\r\n" +                                        
                                        "mov ebp, esp\r\n" +
                                        "push ebx push edi push esi\r\n" +
                                        //"pushad\r\n" +

                                        "push 00000000\r\n" +
                                        "push [ebp+08]\r\n" +
                                        "push <un23>\r\n" +
                                        "push [ebp+0c]\r\n" +
                                        "mov edi, <dll.ntdll.RtlUnwind>\r\n" +
                                        "call edi\r\n" +

                                        ";un23:\r\n" +
                                        "mov esi,[ebp+10]\r\n" + // get context record
                                        "mov edx,[ebp+0c]\r\n" +
                                        "mov [esi+c4], edx\r\n" +
                                        "mov eax, [edx+08]\r\n" +
                                        "mov [esi+b8], eax\r\n" +
                                        "mov eax, [edx+14]\r\n" +
                                        "mov [esi+b4], eax\r\n" +
                                        "mov eax, 00000000\r\n" +
                                        //"popad\r\n" +
                                        "pop esi pop edi pop ebx\r\n" +
                                        "mov esp, ebp\r\n" +
                                        "pop ebp\r\n" +
                                        "ret\r\n" +

                                        ";catch:\r\n" +
                                        "pop fs:[0]\r\n" +
                                        "mov eax, 01000000\r\n" +
                                        "movdfl <EXCEPTIONOCCURRED>, eax\r\n" +
                                        "mov esp, ebp\r\n" +
                                        "pop ebp\r\n" +
                                        

                                        // Exit this thread
                                        //"push 00000000\r\n" + // Exit code
                                        //"mov edi, <dll.kernel32.ExitThread>\r\n" +
                                        //"call edi\r\n" + // Terminate thread
                                        "ret\r\n" +

                                        // Dataspace for recording that an exception occurred
                                        ";EXCEPTIONOCCURRED:\r\n" +
                                        "(4)\r\n"; // Data for the exception handler

        public string generateThreadCallCode(string ecx, string edx, string eax, List<string> stackArguments)
        {
            // Generate the code to send the specified function call
            string completeCode = "";

            // Create appended data for pointers as necessary
            string appendedData = "";
            appendedData += generateAppendedData(ref ecx, "ecx");
            appendedData += generateAppendedData(ref edx, "edx");
            appendedData += generateAppendedData(ref eax, "eax");
            for (int i = 0; i < stackArguments.Count; i++ )
            {
                // Process this argument for pointers
                string stackArgument = stackArguments[i];
                appendedData += generateAppendedData(ref stackArgument, "arg" + i.ToString());
                stackArguments[i] = stackArgument;
            }

            // Setup the code start
            completeCode += injectionPreHandler + "\r\n"
                + registerArguments.Replace("#ECX#", ecx).Replace("#EDX#", edx).Replace("#EAX#", eax) + "\r\n";

            // Add the stack arguments
            for (int i = stackArguments.Count - 1; i >= 0; i-- )
            {
                // Add this stack argument
                completeCode += "push " + stackArguments[i] + "\r\n";
            }

            // Add the main code block
            completeCode = completeCode + mainCode + "\r\n";

            // Optionally add the stack cleanup as required
            if (cleanupMethod == STACK_CLEANUP.BY_CALLER && numParams > 0)
            {
                // Cleanup the pushed arguments
                completeCode = completeCode + "add esp, " +
                               oMemoryFunctions.ReverseString(oMemoryFunctions.MakeFixedLengthHexString((numParams * 4).ToString("X"), 8))
                               + "\r\n";
            }

            // Add the post main code block
            completeCode = completeCode + mainCodePost + "\r\n";

            // Add the exception handler post-code
            completeCode = completeCode + injectionPostHandler + "\r\n";

            // Add the appended data
            completeCode = completeCode + appendedData;

            return completeCode;
        }

        /// <summary>
        /// Checks to see if the input variable is a pointer to data that
        /// needs to be appended:
        ///  "" unicode string
        ///  '' ascii string
        ///  [] hex array
        /// </summary>
        /// <param name="argument">THis is the argument value. It will be changed to the address of the appended data if necessary.</param>
        /// <returns></returns>
        private string generateAppendedData(ref string argument, string argName)
        {
            // Process ascii strings
            string[] split = argument.Split(new char[] {'\''});
            if( split.Length == 3 )
            {
                // Valid ascii dereference, generate the appended data
                string result = ";" + argName + "_data: " + Environment.NewLine;
                for( int i = 0; i < split[1].Length; i++)
                {
                    string character = ((byte)split[1][i]).ToString("X");
                    while (character.Length < 2)
                        character = "0" + character;
                    result = result + character;
                }
                result = result + "00"; // Null terminated

                // Patch the argument to refer to this
                argument = "<" + argName + "_data>";
                return result;
            }

            // Process unicode strings
            split = argument.Split(new char[] { '\"' });
            if (split.Length == 3)
            {
                // Valid unicode dereference, generate the appended data
                string result = ";" + argName + "_data: " + Environment.NewLine;
                for (int i = 0; i < split[1].Length; i++)
                {
                    string character = ((byte)split[1][i]).ToString("X") + "00";
                    while (character.Length < 4)
                        character = "0" + character;
                    result = result + character;
                }
                result = result + "0000"; // Null terminated

                // Patch the argument to refer to this
                argument = "<" + argName + "_data>";
                return result;
            }

            // Process binary arrays
            split = argument.Split(new char[] { '[', ']' });
            if (split.Length == 3)
            {
                // Valid unicode dereference, generate the appended data
                string result = ";" + argName + "_data: " + Environment.NewLine;
                result = result + split[1];

                // Patch the argument to refer to this
                argument = "<" + argName + "_data>";
                return result;
            }

            // No appended data
            return "";
        }


        public enum CALL_TYPE
        {
            FIXED_OFFSET = 0, // Call off a fixed constant
            JUMP_TABLE_PE = 1, // Jump to a jump off a PE table.
            PE_TABLE = 2, // PE table entry
            CALLBACK_TABLE = 3, // Callback table entry
            PE_EXPORT = 4
        }

        /// <summary>
        /// Get signature of function based on first 64 bytes
        /// </summary>
        /// <returns></returns>
        public string getSignature()
        {
            List<byte> binarySignature = new List<byte>();
            if (originalFirstBytes != null)
            {
                binarySignature.AddRange(originalFirstBytes);
                binarySignature.AddRange(oMemoryFunctions.ReadMemory(oProcess.activeProcess, address + 4, 60)); 
            }
            else
                binarySignature.AddRange(oMemoryFunctions.ReadMemory(oProcess.activeProcess, address, 64));

            string signature = BitConverter.ToString(binarySignature.ToArray());
            signature = signature.Replace("-", "");
            return signature;
        }

        /// <summary>
        /// Disable this function.
        /// </summary>
        public void disable()
        {
            if (!disabled)
            {
                // Read in the first couple bytes
                originalFirstBytes = oMemoryFunctions.ReadMemory(oProcess.activeProcess, this.address, 4);

                // Determine the imm8 for the ret instruction
                ulong numBytes = (cleanupMethod == STACK_CLEANUP.BY_CALLED ? this.numParams*4 : 0);

                // Write the corresponding return statement
                // C2 1000 for return 0x10
                byte[] imm8 = oMemoryFunctions.DwordToByteArray(numBytes);
                byte[] retStatement = new byte[3];
                retStatement[0] = 0xC2;
                retStatement[1] = imm8[0];
                retStatement[2] = imm8[1];
                oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, address, retStatement);
                disabled = true;
            }
        }

        /// <summary>
        /// Enable this function.
        /// </summary>
        public void enable()
        {
            if (disabled)
            {
                // Read in the first couple bytes
                oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, address, originalFirstBytes);
                disabled = false;
            }
        }

        public List<oArgument> getArgumentList()
        {
            return this.arguments;
        }
    }
}
