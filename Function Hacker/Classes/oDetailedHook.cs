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
    class oDetailedHook:oStandardHook
    {
        public static uint sequenceAddress = 0;
        
                                                        

        public oDetailedHook(Process process, uint address, uint lengthParams, uint bufferSize, string originalBytes)
            : base(process, address, lengthParams, bufferSize, originalBytes)
        {
            this.dataSize += 4;
            this.preEndBlock = preEndBlock +
                                                        // Increment the sequence number
                                                        "push ecx " +
                                                        "push eax " +
                                                        "mov eax, <sequenceAddress> " +
                                                        "mov ecx, 01000000 " +
                                                        "lock xadd [eax], ecx " +
                                                        // Save the sequence number to the data structure
                                                        "pop eax " +
                                                        "mov [eax], ecx " +
                                                        "pop ecx " +
                                                        "add eax, 04000000 "
                                                        ;
        }

        public oDetailedHook(Process process, string dllName, string functionName, uint lengthParams, uint bufferSize, int lengthOriginalBytes)
            : base(process, dllName, functionName, lengthParams, bufferSize, lengthOriginalBytes)
        {
            this.dataSize += 4;
            this.preEndBlock = preEndBlock +
                // Increment the sequence number
                                                        "push ecx " +
                                                        "push eax " +
                                                        "mov eax, <sequenceAddress> " +
                                                        "mov ecx, 01000000 " +
                                                        "lock xadd [eax], ecx " +
                // Save the sequence number to the data structure
                                                        "pop eax " +
                                                        "mov [eax], ecx " +
                                                        "pop ecx " +
                                                        "add eax, 04000000 "
                                                        ;
        }

        public oDetailedHook(Process process, string dllName, string functionName, uint lengthParams, uint bufferSize, string originalBytes)
            :base(process, dllName, functionName, lengthParams, bufferSize, originalBytes)
        {
            this.dataSize += 4;
            this.preEndBlock = preEndBlock +
                                                        // Increment the sequence number
                                                        "push ecx " +
                                                        "push eax " +
                                                        "mov eax, <sequenceAddress> " +
                                                        "mov ecx, 01000000 " +
                                                        "lock xadd [eax], ecx " +
                                                        // Save the sequence number to the data structure
                                                        "pop eax " +
                                                        "mov [eax], ecx " +
                                                        "pop ecx " +
                                                        "add eax, 04000000 "
                                                        ;
        }

        protected override uint createVariables()
        {
            //// First create the common variables and allocations

            // Call count
            IntPtr memory = (IntPtr)0;
            memory = VirtualAllocEx((IntPtr)process.Handle, (IntPtr)0, 8, (uint)(MEMORY_STATE.COMMIT), (uint)MEMORY_PROTECT.PAGE_EXECUTE_READWRITE);
            
            this.callCountAddress = (uint)memory;
            if (sequenceAddress == 0)
            {
                sequenceAddress = (uint)memory + 4;
            }
            oAssemblyGenerator.addCommonAddress((uint)callCountAddress, "callCount");
            oAssemblyGenerator.addCommonAddress((uint)sequenceAddress, "sequenceAddress");
            oMemoryFunctions.writeMemoryDword(process, (UInt64)callCountAddress, 0);
            oMemoryFunctions.writeMemoryDword(process, (UInt64)sequenceAddress, 0);

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

            return (uint)memory;
        }

        protected override Array rawDataToStructArray(ref byte[] rawData)
        {
            if (rawData == null)
                return new DETAILED_HOOK_CALL[0];

            // Create our result array
            uint size = (this.lengthParams + 3*4);
            DETAILED_HOOK_CALL[] result = new DETAILED_HOOK_CALL[rawData.Length / size];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].caller = oMemoryFunctions.byteArrayToUint(rawData, (int)(i * size));

                // Copy over the arguments
                result[i].arguments = new uint[this.lengthParams / 4];
                for (int n = 0; n < this.lengthParams / 4; n++)
                {
                    result[i].arguments[n] = oMemoryFunctions.byteArrayToUint(rawData, (int)(i * size + 4 + n * 4));
                }
                result[i].sequenceNumber = oMemoryFunctions.byteArrayToUint(rawData, (int)(i * size + 4 + this.lengthParams));
                result[i].returnValue = oMemoryFunctions.byteArrayToUint(rawData, (int)(i * size + 8 + this.lengthParams));
            }


            return result;
        }
    }

    public struct DETAILED_HOOK_CALL
    {
        public uint caller;
        public uint[] arguments;
        public uint sequenceNumber;
        public uint returnValue;
    }
}
