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
    class oDereferenceHook:oDetailedHook
    {
        protected uint copySize;

        public oDereferenceHook(Process process, uint address, uint lengthParams, uint bufferSize, string originalBytes, uint copySize)
            : base(process, address, lengthParams, bufferSize, originalBytes)
        {
            this.dataSize += copySize;
            this.copySize = copySize;
            this.preEndBlock = preEndBlock +
                                                        // Dereference the parameter
                                                        // Check if the deleted location start XXand endXX are valid addresses
                                                        "push edi " +
                                                        "push esi " +
                                                        "cmp ecx, 00000000 " +
                                                        "je <oabel2> " +
                                                        //"mov edi, ecx " +
                                                        //"verr di " +
                                                        //"jnz <oabel2> " +
                                                        //"CC" +
                                                        // Perform the copy of <freeCopySize> bytes
                                                        "mov esi, ecx " + // source address
                                                        "mov edi, eax " + // destination address
                                                        "mov ecx, <copySize> " + // number of byte to copy
                                                        "rep movs byte [edi], byte [esi] " +
                                                        "fclex " +
                                                        "<label2:>" +
                                                        "pop esi " +
                                                        "pop edi "
                                                        ;
        }

        public oDereferenceHook(Process process, string dllName, string functionName, uint lengthParams, uint bufferSize, int sizeOriginalBytes, uint copySize)
            : base(process, dllName, functionName, lengthParams, bufferSize, sizeOriginalBytes)
        {
            this.dataSize += copySize;
            this.copySize = copySize;
            this.preEndBlock = preEndBlock +
                // Dereference the parameter
                // Check if the deleted location start XXand endXX are valid addresses
                                                        "push ebp " +
                                                        "push edi " +
                                                        "push esi " +

                                                        // Move the address to derefernce into ecx
                                                        "mov ecx, eax " +
                                                        "sub ecx, 10000000 " + // to arg0
                                                        "mov ecx, [ecx] " +
                                                        "mov ebp, ecx " + //
                                                        "shr ebp, 10 " +
                                                        "push eax " +
                                                        "push ecx " +
                                                        "push <copySize> " + // Set size parameter
                                                        "push ecx " + // Set address parameter
                                                        "call <offset.DLL.kernel32.IsBadReadPtr> " +
                                                        "pop ecx " +
                                                        "cmp eax, 00000000 " +
                                                        "pop eax " +
                                                        "jne <oabel3> " +

                                                        // Perform the copy of <freeCopySize> bytes
                                                        "mov esi, ecx " + // source address
                                                        "mov edi, eax " + // destination address
                                                        "mov ecx, <copySize> " + // number of byte to copy
                                                        "rep movs byte [edi], byte [esi] " +
                                                        "<label3:>" +

                                                        "pop esi " +
                                                        "pop edi " +
                                                        "pop ebp "
                                                        ;
        }

        public oDereferenceHook(Process process, string dllName, string functionName, uint lengthParams, uint bufferSize, string originalBytes, uint copySize)
            :base(process, dllName, functionName, lengthParams, bufferSize, originalBytes)
        {
            this.dataSize += copySize;
            this.copySize = copySize;
            this.preEndBlock = preEndBlock +
                                                        // Dereference the parameter
                                                        // Check if the deleted location start XXand endXX are valid addresses
                                                        "push ebp " +
                                                        "push edi " +
                                                        "push esi " +
                                                        
                                                        // Move the address to derefernce into ecx
                                                        "mov ecx, eax " +
                                                        "sub ecx, 10000000 " + // to arg0
                                                        "mov ecx, [ecx] " +
                                                        "mov ebp, ecx " + //
                                                        "shr ebp, 10 " +
                                                        "push eax " +
                                                        "push ecx " +
                                                        "push <copySize> " + // Set size parameter
                                                        "push ecx " + // Set address parameter
                                                        "call <offset.DLL.kernel32.IsBadReadPtr> " +
                                                        "pop ecx " +
                                                        "cmp eax, 00000000 " +
                                                        "pop eax " +
                                                        "jne <oabel3> " +
                                                        
                                                        // Perform the copy of <freeCopySize> bytes
                                                        "mov esi, ecx " + // source address
                                                        "mov edi, eax " + // destination address
                                                        "mov ecx, <copySize> " + // number of byte to copy
                                                        "rep movs byte [edi], byte [esi] " +
                                                        "<label3:>" +
                                                        
                                                        "pop esi " +
                                                        "pop edi " +
                                                        "pop ebp "
                                                        ;
        }

        protected override Array rawDataToStructArray(ref byte[] rawData)
        {
            if (rawData == null)
                return new DEREFERENCED_HOOK_CALL[0];

            // Create our result array
            uint size = (this.lengthParams + 3 * 4 + this.copySize);
            DEREFERENCED_HOOK_CALL[] result = new DEREFERENCED_HOOK_CALL[rawData.Length / size];
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

                // Copy the array
                result[i].dereference = new byte[this.copySize];
                Array.ConstrainedCopy(rawData, (int)(i * size + 8 + this.lengthParams),result[i].dereference, 0,(int) this.copySize);

                result[i].returnValue = oMemoryFunctions.byteArrayToUint(rawData, (int)(i * size + this.copySize + 8 + this.lengthParams));
            }


            return result;
        }

        protected override uint createVariables()
        {
            oAssemblyGenerator.addCommonAddress((uint)copySize, "copySize");

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
            this.storageAddressEnd = storageAddressStart + bufferSize - (this.lengthParams + 8 + this.copySize);
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
    }

    public struct DEREFERENCED_HOOK_CALL
    {
        public uint caller;
        public uint[] arguments;
        public uint sequenceNumber;
        public byte[] dereference;
        public uint returnValue;
    }
}
