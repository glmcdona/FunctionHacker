using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BufferOverflowProtection;

namespace Adv_Memory_Monitor.Memory_Classes
{
    class oStandardHook
    {




        private static string copy6Arg = "FF 74 24 18 " + // push [esp+0x18]
                                    "FF 74 24 18 " + // push [esp+0x18]
                                    "FF 74 24 18 " + // push [esp+0x18]
                                    "FF 74 24 18 " + // push [esp+0x18]
                                    "FF 74 24 18 " + // push [esp+0x18]
                                    "FF 74 24 18 "; // push [esp+0x18]

        private static string copy5Arg = "FF 74 24 14 " + // push [esp+0x14]
                                    "FF 74 24 14 " + // push [esp+0x14]
                                    "FF 74 24 14 " + // push [esp+0x14]
                                    "FF 74 24 14 " + // push [esp+0x14]
                                    "FF 74 24 14 "; // push [esp+0x14]

        private static string copy4Arg = "FF 74 24 10 " + // push [esp+0x10]
                                    "FF 74 24 10 " + // push [esp+0x10]
                                    "FF 74 24 10 " + // push [esp+0x10]
                                    "FF 74 24 10 "; // push [esp+0x10]

        private static string copy3Arg = "FF 74 24 0C " + // push [esp+0xc]
                                    "FF 74 24 0C " + // push [esp+0xc]
                                    "FF 74 24 0C "; // push [esp+0xc]

        private static string copy2Arg = "FF 74 24 08 " + // push [esp+0x8]
                                    "FF 74 24 08 "; // push [esp+0x8]

        private static string copy1Arg = "FF 74 24 04 "; // push [esp+0x4]

        private static string copy0Arg = "push eax"; // dummy push, we need this space to store the data save location

        private static string middleBlock =                    // Create a copy of the function arguments
                                                        // copy3Arg

                                                        // Set the return address
                                                        "push <label1> " +

                                                        // Save the registers
                                                        "push ecx " +
                                                        "push eax " +
                                                        "push edx " +

                                                        // Load and update the information save location
                                                        "mov eax, <currentAddress> " +
                                                        "mov ecx, <dataSize> " +
                                                        "lock xadd [eax], ecx " +
                                                        "mov eax, ecx " +
                                                        

                                                        // Check if we are at the end of the circular buffer
                                                        "cmp eax, <endAddress> " +
                                                        "jl <oabel2> " +
                                                        // Loop the information save buffer
                                                        "mov eax, <startAddress> " +
                                                        "add eax, <dataSize> " +
                                                        "movdfl <currentAddress>, eax " +
                                                        
                                                        "<label2:> " +

                                                        // Update the call count
                                                        "mov edx, <callCount> " +
                                                        "lock inc dword [edx]" +
                                                        //"movdfr edx, <callCount> " +
                                                        //"add edx, 01000000 " +
                                                        //"movdfl <callCount>, edx " +

                                                        // Save the information save location to the stack
                                                        "mov ecx, esp " +
                                                        "add ecx, <sizeToArgStart> " +
                                                        "add ecx, 08000000 " +
                                                        "mov [ecx], eax " +

                                                        // Save the calling address
                                                        "mov ecx, esp " +
                                                        "add ecx, <sizeToArgStart> " +
                                                        "add ecx, 04000000 " +
                                                        "mov ecx, [ecx] " +
                                                        "mov [eax], ecx " +
                                                        "add eax, 04000000 " +

                                                        // Save the arguments
                                                        "mov ecx, esp " +
                                                        "add ecx, <sizeToArgStart> "
                                                        
                                                        // saveArg
                                                        ;


        private static string saveArg = "mov edx, [ecx] " +
                                    "mov [eax], edx " +
                                    "add eax, 04000000 " +
                                    "sub ecx, 04000000 ";

        protected string preEndBlock = "";


        protected static string endBlock =
                                                        // Unload the registers
                                                        "pop edx " +
                                                        "pop eax " +
                                                        "pop ecx " +

                                                        // Make the original allocation call
                                                        "<originalCode> " +
                                                        //"8BFF558BEC " + // ORIGINAL CODE: MOV EDI,EDI // PUSH EBP // MOV EBP,ESP
                                                        //"CC " +
                                                        "jmp <offset.allocAfterInject> " +
                                                        //"CC " +
                                                        "<label1:>" +


                                                        // Record the allocation address return value
                                                        "push ecx " +
                                                        "mov ecx, esp " +
                                                        "add ecx, 08000000 " +
                                                        "mov ecx, [ecx] " +
                                                        "add ecx, <dataSize> " +
                                                        "sub ecx, 04000000 " +
                                                        "mov [ecx], eax " +
                                                        "pop ecx "

                                                        // Return to the calling address
                                                        // ret
                                                        //"C2 0C 00" // retn 0xc
                                                        ;

        private static string ret6 = "C2 18 00"; // retn 6*4
        private static string ret5 = "C2 14 00"; // retn 5*4
        private static string ret4 = "C2 10 00"; // retn 4*4
        private static string ret3 = "C2 0C 00"; // retn 3*4
        private static string ret2 = "C2 08 00"; // retn 2*4
        private static string ret1 = "C2 04 00"; // retn 1*4
        private static string ret0 = "C2 04 00"; // retn 1*4

        protected Process process = null;
        protected uint address = 0;
        protected uint lengthParams = 0;
        protected uint callCountAddress = 0;
        protected uint storageAddressPtr = 0;
        protected uint storageAddressStart = 0;
        protected uint storageAddressEnd = 0;
        protected uint bufferSize = 0;
        protected uint dataSize = 0;

        protected uint storageAddressLast = 0;

        protected string originalBytes = "";



        public oStandardHook(Process process, uint address, uint lengthParams, uint bufferSize, string originalBytes)
        {
            this.process = process;
            this.address = address;
            this.lengthParams = lengthParams;
            this.bufferSize = bufferSize;
            this.dataSize = lengthParams + 8;
            this.originalBytes = originalBytes;
        }

        public oStandardHook(Process process, string dllName, string functionName, uint lengthParams, uint bufferSize, int sizeOriginalBytes)
        {
            this.process = process;
            this.lengthParams = lengthParams;
            this.bufferSize = bufferSize;
            this.dataSize = lengthParams + 8;

            // Load the address of the specified function
            this.address = (uint)BufferOverflowProtection.oMemoryFunctions.loadAddress(dllName, functionName, process);

            // Determine the original bytes
            byte[] data = oMemoryFunctions.readMemory(process, this.address, (uint) sizeOriginalBytes);
            this.originalBytes = oMemoryFunctions.toHex(data).Replace(" ","");
        }

        public oStandardHook(Process process, string dllName, string functionName, uint lengthParams, uint bufferSize, string originalBytes)
        {
            this.process = process;
            this.lengthParams = lengthParams;
            this.bufferSize = bufferSize;
            this.dataSize = lengthParams + 8;
            this.originalBytes = originalBytes;

            // Load the address of the specified function
            this.address = (uint) BufferOverflowProtection.oMemoryFunctions.loadAddress(dllName, functionName, process);
        }

        public void inject()
        {
            injectHook();
        }

        protected virtual void injectHook()
        {
            uint memory = createVariables();

            // Consolidate the assembly injection string
            string codeInjection = "";
            string preludeInjection = "";
            string interludeInjection = "";
            string proludeInjection = "";
            switch (this.lengthParams)
            {
                case 0:
                    preludeInjection = copy0Arg;
                    proludeInjection = ret0;
                    break;
                case 4:
                    preludeInjection = copy1Arg;
                    proludeInjection = ret1;
                    break;
                case 8:
                    preludeInjection = copy2Arg;
                    proludeInjection = ret2;
                    break;
                case 12:
                    preludeInjection = copy3Arg;
                    proludeInjection = ret3;
                    break;
                case 16:
                    preludeInjection = copy4Arg;
                    proludeInjection = ret4;
                    break;
                case 20:
                    preludeInjection = copy5Arg;
                    proludeInjection = ret5;
                    break;
                case 24:
                    preludeInjection = copy6Arg;
                    proludeInjection = ret6;
                    break;
            }
            for (int i = 0; i < this.lengthParams / 4; i++)
            {
                interludeInjection = interludeInjection + saveArg;
            }
            codeInjection = preludeInjection + middleBlock + interludeInjection + preEndBlock + endBlock + proludeInjection;
            codeInjection = oAssemblyGenerator.replaceCommands(codeInjection);


            // Make the code injection
            byte[] injection = oAssemblyGenerator.buildInjection((ulong)memory, codeInjection, process, 0, 0);
            oMemoryFunctions.writeMemoryByteArray(process, (ulong)memory, injection);

            // Redirect the code to the injection
            // 8BFF558BEC - These are the original 5 bytes we are overwriting with our call
            injection = new byte[] {
                0xE9, 0x00, 0x00, 0x00, 0x00 // call replacement function
            };
            injection[1] = (byte)(((int)memory - (this.address + 5)) & 0x000000ff);
            injection[2] = (byte)((((int)memory - (this.address + 5)) & 0x0000ff00) >> 8);
            injection[3] = (byte)((((int)memory - (this.address + 5)) & 0x00ff0000) >> 16);
            injection[4] = (byte)((((int)memory - (this.address + 5)) & 0xff000000) >> 24);

            if (originalBytes.Length/2 > 5)
            {
                Array.Resize(ref injection, originalBytes.Length/2);
                for (int i = 5; i < originalBytes.Length; i++)
                {
                    // Add a 'nop' instruction
                    injection[i] = 0x90;
                }
            }

            oMemoryFunctions.writeMemoryByteArray(process, this.address, injection);
            storageAddressLast = this.storageAddressStart;
        }

        protected virtual uint createVariables()
        {
            //// First create the common variables and allocations

            // Call count
            IntPtr memory = (IntPtr)0;
            memory = VirtualAllocEx((IntPtr)process.Handle, (IntPtr)0, 4, (uint)(MEMORY_STATE.COMMIT), (uint)MEMORY_PROTECT.PAGE_EXECUTE_READWRITE);
            oAssemblyGenerator.addCommonAddress((uint)memory, "callCount");
            this.callCountAddress = (uint)memory;
            oMemoryFunctions.writeMemoryDword(process, (UInt64)callCountAddress, 0);

            // Allocate the space for the code injection and circular buffer datastorage
            memory = VirtualAllocEx((IntPtr)process.Handle, (IntPtr)0, (int)this.bufferSize + 404, (uint)(MEMORY_STATE.COMMIT), (uint)MEMORY_PROTECT.PAGE_EXECUTE_READWRITE);
            this.storageAddressPtr = (uint)memory + 400;
            this.storageAddressStart = (uint)memory + 404;
            this.storageAddressEnd = storageAddressStart + bufferSize - (this.lengthParams + 8);
            oMemoryFunctions.writeMemoryDword(process, (UInt64)storageAddressPtr, storageAddressStart);

            // Assign the variables for the code injection
            oAssemblyGenerator.addCommonAddress((uint)storageAddressPtr, "currentAddress");
            oAssemblyGenerator.addCommonAddress((uint)storageAddressStart, "startAddress");
            oAssemblyGenerator.addCommonAddress((uint)storageAddressEnd, "endAddress");
            oAssemblyGenerator.addCommonAddress((uint)dataSize, "dataSize");
            oAssemblyGenerator.addCommonAddress((uint)this.lengthParams + 3 * 4, "sizeToArgStart");
            oAssemblyGenerator.addCommonAddress(this.originalBytes, "originalCode");
            oAssemblyGenerator.addCommonAddressNoRev((uint)this.address + 5, "allocAfterInject");

            return (uint) memory;
        }

        public virtual uint getCallCount()
        {
            return oMemoryFunctions.readMemoryDword(process, (ulong)this.callCountAddress);
        }

        public virtual void clearBuffer()
        {
            this.storageAddressLast = oMemoryFunctions.readMemoryDword(process, this.storageAddressPtr);
        }

        public virtual Array readBuffer()
        {
            // Read in the current storage location
            uint curAddress = oMemoryFunctions.readMemoryDword(process, this.storageAddressPtr);

            if (curAddress == this.storageAddressLast)
            {
                // No new data
                return null;
            }
            else if (curAddress > this.storageAddressLast)
            {
                // Circular buffer did not wrap around
                byte[] rawData = oMemoryFunctions.readMemory(process, (ulong)this.storageAddressLast, curAddress - this.storageAddressLast + 1);

                this.storageAddressLast = curAddress;

                // Convert the raw data to the structure
                return rawDataToStructArray(ref rawData);
            }
            else
            {
                // Circular buffer wrapped around
                byte[] rawDataUpper = oMemoryFunctions.readMemory(process, (ulong)this.storageAddressLast, this.storageAddressEnd - this.storageAddressLast + 1);
                int length = rawDataUpper.Length;
                byte[] rawDataLower = oMemoryFunctions.readMemory(process, this.storageAddressStart, curAddress - this.storageAddressStart + 1);
                Array.Resize(ref rawDataUpper, rawDataUpper.Length + rawDataLower.Length);
                rawDataLower.CopyTo(rawDataUpper, length);

                this.storageAddressLast = curAddress;

                // Convert the raw data to the structure
                return rawDataToStructArray(ref rawDataLower);
            }

            
        }

        protected virtual Array rawDataToStructArray(ref byte[] rawData)
        {
            if (rawData == null)
                return new STANDARD_HOOK_CALL[0];

            // Create our result array
            uint size = (this.lengthParams+8);
            STANDARD_HOOK_CALL[] result = new STANDARD_HOOK_CALL[rawData.Length / size];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].caller = oMemoryFunctions.byteArrayToUint(rawData, (int) (i * size) );

                // Copy over the arguments
                result[i].arguments = new uint[this.lengthParams / 4];
                for (int n = 0; n < this.lengthParams / 4; n++)
                {
                    result[i].arguments[n] = oMemoryFunctions.byteArrayToUint(rawData, (int) (i * size + 4 + n*4) );
                }

                result[i].returnValue = oMemoryFunctions.byteArrayToUint(rawData, (int)(i * size + 4 + this.lengthParams));
            }
            

            return result;
        }

        public virtual uint getCallCount_Reset()
        {
            return 0;
        }

        [DllImport("kernel32")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);
    }

    public struct STANDARD_HOOK_CALL{
        public uint caller;
        public uint[] arguments;
        public uint returnValue;
    }
}
