using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BufferOverflowProtection;
using FunctionHacker.Classes.Generic_Types;

namespace FunctionHacker.Classes
{
    public static class oMemoryFunctions
    {
        public static string ReverseString(string input)
        {
            // Reverses the input string
            string result = "";
            for (int i = 0; i < input.Length; i += 2)
                result = String.Concat(input[i], input[i + 1], result);
            return result;
        }

        /// <summary>
        /// Converts the number an 8-character hex representation, with byte-wise reversing.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string IntToDwordString(uint number)
        {
            // Convert it to an 4-byte string
            string result = number.ToString("X");
            while( result.Length < 8 )
                result = "0" + result;

            // Reverse the string
            return ReverseString(result);
        }

        /// <summary>
        /// Converts a uint to a signed float.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float IntToFloat(uint input)
        {
            // This is untested, I am not sure if I did this perfectly!
            int sign = (input & 0x80000000) == 1 ? -1 : 1;
            int exponent = ((int)(input & 0x7F800000)) >> 23;
            return (float) sign * ( 1.0f + ( (float) (input & 0x007fffff)) / ((float) (2^23)) ) * (float) (2^(exponent-127));
        }

        public static UInt64 LoadAddress(string dllName, string procedureName, Process process)
        {
            // Find the dll in the destination process
            ProcessModule dll = null;
            foreach (ProcessModule module in process.Modules)
            {
                try
                {
                    if (module.ModuleName.ToLower().Equals(dllName.ToLower()) || module.ModuleName.ToLower().Equals(dllName.ToLower() + ".dll"))
                    {
                        dll = module;
                        break;
                    }
                }
                catch { }
            }
            if (dll == null)
            {
                oConsole.printMessageShow("Failed to find dll named " + dllName + " while searching for procedure " + procedureName + ".");
                return 0;
            }

            // Import the dll into our application and find the function offset from the base address of the module
            int hwnd = LoadLibrary(dllName);
            UInt64 myProcedureAddress = (UInt64)GetProcAddress(hwnd, procedureName);

            // Find the dll base address in our process
            UInt64 myDllBase = 0;
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                try
                {
                    if (module.ModuleName.ToLower().Equals(dllName.ToLower()) || module.ModuleName.ToLower().Equals(dllName.ToLower() + ".dll"))
                    {
                        myDllBase = (UInt64)module.BaseAddress;
                        break;
                    }
                }
                catch { }
            }
            if (myDllBase == 0)
            {
                oConsole.printMessageShow("Failed to find dll named " + dllName + " while searching myself to obtain base address. Procedure " + procedureName + ".");
                FreeLibrary((IntPtr)hwnd);
                return 0;
            }

            // Calculate the address
            UInt64 offsetProcedure = myProcedureAddress - myDllBase;
            UInt64 procedureAddressTarget = offsetProcedure + (UInt64)dll.BaseAddress;

            // Remove the new copy of kernel32.dll
            FreeLibrary((IntPtr)hwnd);

            return procedureAddressTarget;
        }


        /*public static byte[] getMin5Bytes(Process process, uint addressStart)
        {
            // This function returns NULL if in the first 5 bytes it starts a call or jump statement.
            // This is to protect it from screwing up the code.

            // Create our disassembler
            xdis.Class1 disassembler = new xdis.Class1();

            // Read in the memory
            byte[] buffer = new byte[50];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)(addressStart), buffer, 50, ref numRead);

            unsafe
            {
                // Set the disassembler source data
                IntPtr tmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                disassembler.setData((byte*)tmpPtr, (uint)buffer.Length, (uint)(addressStart));
                uint address = 0;
                uint* pAddress = &address;

                // Create our string and hex pointers
                sbyte* ascii = (sbyte*)Marshal.UnsafeAddrOfPinnedArrayElement(new sbyte[255], 0);
                sbyte* hex = (sbyte*)Marshal.UnsafeAddrOfPinnedArrayElement(new sbyte[255], 0);

                // Read in the instructions until we have over 5 bytes of data
                uint offsetStart = 0;
                uint count = 0;
                uint lastAddress = addressStart;
                byte[] data = new byte[50];
                while (disassembler.getNextInstruction(pAddress, &ascii, &hex) != 0 && count < 5)
                {
                    // Record this location if it is a nop instruction
                    if ((ascii[0] == 'c' && ascii[1] == 'a' && ascii[2] == 'l' && ascii[3] == 'l' && ascii[4] == ' ') ||
                        (ascii[0] == 'j' && ascii[1] == 'm' && ascii[2] == 'p' && ascii[3] == ' '))
                        return null;

                    // Increment our total count
                    count += address - lastAddress;
                    lastAddress = address;
                }

                // Read in our result
                byte[] result = oMemoryFunctions.readMemory(process, addressStart, count);

                // Return
                return result;
            }
        }*/

        public static void WriteMemoryByteArray(Process process, UInt64 address, byte[] data)
        {
            // Write the array
            IntPtr numWritten = (IntPtr)0;
            WriteProcessMemory(process.Handle, (IntPtr)address, data, (UIntPtr)data.Length, out numWritten);

            // Check that all the data was read correctly
            if ((UInt32)numWritten != data.Length)
                oConsole.printMessage("Failed to write byte array to address " + address.ToString("X") + ". Wrote " + numWritten.ToString() + " of " + data.Length.ToString() + ".");
        }

        public static ProcessModule GetModuleFromAddress(Process process, ulong addressLoc)
        {
            // This function figures out which module the specified address lies in
            ulong resultBase = 0;
            ProcessModule resultModule = null;

            foreach (ProcessModule module in process.Modules)
            {
                if ((ulong)(module.BaseAddress) > resultBase && (ulong)(module.BaseAddress) <= addressLoc)
                {
                    resultBase = (ulong)(module.BaseAddress);
                    resultModule = module;
                }
            }
            return resultModule;
        }

        public enum STRING_TYPE
        {
          auto,
          ascii,
          unicode
        }

        public static bool[] isDisplayableAscii =/* 0x00 */new bool[]{false,false,false,false,   false,false,false,false,   false,false ,true,false,   false,true ,false,false,
	                                             /* 0x10 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0x20 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
	                                             /* 0x30 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
	                                             /* 0x40 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
	                                             /* 0x50 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
	                                             /* 0x60 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,
	                                             /* 0x70 */  true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,true ,   true ,true ,true ,false,
	                                             /* 0x80 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0x90 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0xA0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0xB0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0xC0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0xD0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0xE0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false,
	                                             /* 0xF0 */  false,false,false,false,   false,false,false,false,   false,false,false,false,   false,false,false,false};

        public static string ReadString(Process process, UInt64 address, STRING_TYPE type)
        {
          // Reads a string from the specified address.
          string result = "";
          bool moreString = true;
          int chunkSize = 0x4;
          while (moreString)
          {
            byte[] data;
            try
            {
              data = ReadMemory(process, (ulong)address, (uint)chunkSize);
            }
            catch (Exception ex)
            {
              data = new byte[0];
            }

            if (data.Length == 0)
              return result;
            int i = 0;
            while (i < chunkSize && i < data.Length && moreString)
            {
              if (isDisplayableAscii[data[i]])
                result = result + ((char)data[i]);
              else
                moreString = false;

              if (type == STRING_TYPE.auto && i + 1 < data.Length)
                type = (data[i + 1] == 0 ? STRING_TYPE.unicode : STRING_TYPE.ascii);
              else if (type == STRING_TYPE.auto)
                type = STRING_TYPE.ascii;

              if (type == STRING_TYPE.ascii)
                i++;
              else
                i += 2;
            }
            address += (ulong)i;

            // Increase chunksize
            chunkSize *= 2;
          }
          return result;
        }

        public static byte[] ReadMemory(Process process, UInt64 address, UInt32 length)
        {
            // Copy the bytes from this heap
            byte[] buffer = new byte[length];
            IntPtr numRead = (IntPtr)0;
            bool result = ReadProcessMemory(process.Handle, (IntPtr) address, buffer, (uint) length, ref numRead);

            // Check that all the data was read correctly
            if ((UInt32)numRead != length)
                oConsole.printMessage("Failed to read memory from address " + address.ToString("X") + ". Read " + numRead.ToString() + " of " + length.ToString() + ".");

            if (!result)
                oConsole.printMessage("GetLastError = " + GetLastError().ToString());

            return buffer;
        }

        public static uint AddressAdd(uint address1, uint address2)
        {
            return (address1 + address2) & 0x7fffffff;
        }

        public static uint AddressAdd(ulong address1, ulong address2)
        {
            return (uint) ((address1 + address2) & 0x7fffffff);
        }

        public static uint AddressAdd(uint address1, ulong address2)
        {
            return (uint)((address1 + address2) & 0x7fffffff);
        }

        public static uint AddressAdd(ulong address1, uint address2)
        {
            return (uint)((address1 + address2) & 0x7fffffff);
        }

        public static byte ReadMemoryByte(Process process, UInt64 address)
        {
            // Copy the bytes from this heap
            byte[] buffer = new byte[1];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)address, buffer, (uint)1, ref numRead);

            // Check that all the data was read correctly
            if ((int)numRead != 1)
                oConsole.printMessage("Failed to read BYTE from address " + address.ToString("X") + ". Read " + numRead.ToString() + " of 4.");

            return (byte)RawDataToObject(ref buffer, typeof(byte));
        }


        public static ushort ReadMemoryUShort(Process process, UInt64 address)
        {
            // Copy the bytes from this heap
            byte[] buffer = new byte[2];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)address, buffer, (uint)2, ref numRead);

            // Check that all the data was read correctly
            if ((int)numRead != 2)
                oConsole.printMessage("Failed to read SHORT from address " + address.ToString("X") + ". Read " + numRead.ToString() + " of 4.");

            return (ushort)RawDataToObject(ref buffer, typeof(ushort));
        }

        public static List<UInt32> ReadMemoryDwordArray(Process process, uint address, int numberOfDwords)
        {
            List<UInt32> result = new List<uint>(numberOfDwords);

            // Read in the data
            byte[] buffer = new byte[numberOfDwords*4];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)address, buffer, (uint)numberOfDwords * 4, ref numRead);

            // Convert the buffer to the uint32 array
            for( int i = 0; i < ((int)numRead)/4; i++)
            {
                result.Add( ByteArrayToUint(buffer,i*4) );
            }

            return result;
        }

        public static List<UInt16> ReadMemoryWordArray(Process process, uint address, int numberOfWords)
        {
            List<UInt16> result = new List<UInt16>(numberOfWords);

            // Read in the data
            byte[] buffer = new byte[numberOfWords * 2];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)address, buffer, (uint)numberOfWords * 2, ref numRead);

            // Convert the buffer to the uint32 array
            for (int i = 0; i < ((int)numRead) / 2; i++)
            {
                result.Add(ByteArrayToUshort(buffer, i * 2));
            }

            return result;
        }

        public static UInt32 ReadMemoryDword(Process process, UInt64 address)
        {
            // Copy the bytes from this heap
            byte[] buffer = new byte[4];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)address, buffer, (uint)4, ref numRead);

            // Check that all the data was read correctly
            if ((UInt32)numRead != 4)
                oConsole.printMessage("Failed to read DWORD from address " + address.ToString("X") + ". Read " + numRead.ToString() + " of 4.");

            return (UInt32)RawDataToObject(ref buffer, typeof(UInt32));
        }

        public static string ReadMemoryString(Process process, UInt64 address, int maxLength)
        {
            // Copy the bytes from this heap
            byte[] buffer = new byte[maxLength];
            IntPtr numRead = (IntPtr)0;
            ReadProcessMemory(process.Handle, (IntPtr)address, buffer, (uint)maxLength, ref numRead);

            // Check that all the data was read correctly
            if ((UInt32)numRead != maxLength)
                oConsole.printMessage("Failed to read string from address " + address.ToString("X") + ". Read " + numRead.ToString() + " of " + maxLength.ToString() + ".");

            // Find the first null in the data
            int length = 0;
            string result = "";
            while( length < maxLength && buffer[length] != 0  )
            {
                result += (char)buffer[length];
                length++;
            }

            return result;
        }

        public static void WriteMemoryDword(Process process, UInt64 address, UInt32 data)
        {
            // Copy the bytes from this heap
            byte[] buffer = new byte[4];
            IntPtr numWritten = (IntPtr)1;
            buffer[0] = (byte)(data & 0x000000ff);
            buffer[1] = (byte)((data & 0x0000ff00) >> 8);
            buffer[2] = (byte)((data & 0x00ff0000) >> 16);
            buffer[3] = (byte)((data & 0xff000000) >> 24);
            WriteProcessMemory(process.Handle, (IntPtr)address, buffer, (UIntPtr)4, out numWritten);

            // Check that all the data was read correctly
            if ((UInt32)numWritten != 4)
                oConsole.printMessage("Failed to write DWORD to address " + address.ToString("X") + ". Wrote " + numWritten.ToString() + " of 4. Attempting to write value of " + data.ToString() + ".");
        }

        /// <summary>
        /// Sets the memory protection. ie. read, write, execute
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="protection">The protection code: </param>
        public static bool SetMemoryProtection(Process process, uint address, uint length, MEMORY_PROTECT protection)
        {
            uint oldProtection = 0;
            bool result = VirtualProtectEx(process.Handle, (IntPtr)address, (UIntPtr)length, (uint)protection, out oldProtection);
            return result;
        }

        public static void ClearMemory(Process process, uint address, uint length)
        {
            byte[] blankData = new byte[length];
            WriteMemoryByteArray(process, (ulong)address, blankData);
        }

        public static void WriteString(Process process, UInt64 address, String data)
        {
            char[] bufferChar = data.ToCharArray();
            byte[] buffer = new byte[bufferChar.Length + 1];
            for (int i = 0; i < bufferChar.Length; i++)
                buffer[i] = (byte) bufferChar[i];
            buffer[buffer.Length - 1] = 0;

            IntPtr numWritten = (IntPtr)0;
            WriteProcessMemory(process.Handle, (IntPtr)address, buffer, (UIntPtr)4, out numWritten);

            // Check that all the data was read correctly
            if ((UInt32)numWritten != buffer.Length)
                oConsole.printMessage("Failed to write string to address " + address.ToString("X") + ". Wrote " + numWritten.ToString() + " of " + buffer.Length.ToString() + ". Attempting to write string of '" + data + "'.");
        }

        /// <summary>
        /// Converts the specified dword into an array of bytes
        /// </summary>
        /// <param name="dword"></param>
        /// <returns></returns>
        public static byte[] DwordToByteArray(ulong dword)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(dword & 0x000000ff);
            result[1] = (byte)((dword & 0x0000ff00) >> 8);
            result[2] = (byte)((dword & 0x00ff0000) >> 16);
            result[3] = (byte)((dword & 0xff000000) >> 24);
            return result;
        }

        public static string ByteArrayToString(byte[] name)
        {
            return System.Text.Encoding.UTF8.GetString(name).TrimEnd('\0');
        }

        /// <summary>
        /// Converts takes the first 4 bytes from the array into a uint
        /// </summary>
        /// <param name="dword"></param>
        /// <returns></returns>
        public static uint ByteArrayToUint(byte[] data, int index)
        {
            if (index >= 0 && data.Length - index >= 4)
                return (uint)(data[0 + index] + ( data[1 + index] << 8 ) + ( data[2 + index] << 16 ) + ( data[3 + index] << 24));
            else
                return 12345678;
        }
        public static uint ByteArrayToUint(List<byte> data, int index)
        {
            if (index >= 0 && data.Count - index >= 4)
                return (uint)(data[0 + index] + (data[1 + index] << 8) + (data[2 + index] << 16) + (data[3 + index] << 24));
            else
                return 12345678;
        }

        /// <summary>
        /// Converts takes the first 2 bytes from the array into a ushort
        /// </summary>
        /// <param name="dword"></param>
        /// <returns></returns>
        public static ushort ByteArrayToUshort(byte[] data, int index)
        {
            if (data.Length - index >= 2)
                return (ushort)(data[0 + index] + (data[1 + index] << 8));
            else
                return 0;
        }
        public static ushort ByteArrayToUshort(List<byte> data, int index)
        {
            if (data.Count - index >= 2)
                return (ushort)(data[0 + index] + (data[1 + index] << 8));
            else
                return 0;
        }

        /// <summary>
        /// This will return the heap containing the specified address.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static HEAP_INFO LookupAddressInMap(List<HEAP_INFO> map, uint address)
        {
            // Loop through the heaps
            foreach( HEAP_INFO heap in map )
            {
                // Check if our address falls into this heap
                if(address <= heap.heapAddress + heap.heapLength - 1)
                {
                    return heap;
                }
            }

            // Return an invalid heap
            return new HEAP_INFO(0,0,"","",null);
        }

        /// <summary>
        /// Generates an access lookup table of the process for valid read addresses.
        /// This is used by the code injection to determine if a pointer can be dereferenced.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static byte[] GenerateValidReadPointerTableFromMap(List<HEAP_INFO> map)
        {
            // Create the table
            byte[] lookupTable = new byte[0x80000]; // Arrays are automatically initialized to 0, which is good.

            // Loop through the heaps
            for (int index = 0; index < map.Count; index++ )
            {
                // Check if this is a valid read heap
                if (map[index].heapProtection.ToLower().Contains("read") && !map[index].heapProtection.ToLower().Contains("guard"))
                {
                    // This is a valid read heap, so lets set all the bits as required
                    int endRegion = (((int)map[index].heapAddress + (int)map[index].heapLength) >> 0xC);
                    for (int i = ((int)map[index].heapAddress >> 0xC); i < endRegion; i++)
                    {
                        lookupTable[i] = 1;
                    }
                }
            }

            return lookupTable;
        }

        /// <summary>
        /// Generates a table to be used by the code injection to see if the call source is in an excluded region. This table
        /// is used by exported function instrumentation to exclude the recording of calls originating from module that have
        /// not been selected..
        /// </summary>
        /// <param name="invalidHeaps">List of heaps to exclude. This should be the list of EXECUTE regions that were not selected in the heap selection menu and associated with a module.</param>
        /// <returns></returns>
        public static byte[] GenerateInvalidCallSourceTable(List<HEAP_INFO> invalidHeaps)
        {
            // Create the table
            byte[] lookupTable = new byte[0x80000]; // Arrays are automatically initialized to 0, which is good.

            // Loop through the heaps, we want to exlude 
            for (int index = 0; index < invalidHeaps.Count; index++)
            {
                int endRegion = (((int)invalidHeaps[index].heapAddress + (int)invalidHeaps[index].heapLength) >> 0xC);
                for (int i = ((int)invalidHeaps[index].heapAddress >> 0xC); i < endRegion; i++)
                {
                    lookupTable[i] = 1;
                }
            }

            return lookupTable;
        }


        /// <summary>
        /// Given a process, this function generates a memory map of the entire process.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static List<HEAP_INFO> GenerateMemoryMap(Process process)
        {
            try
            {
                // Initialize the return structure
                List<HEAP_INFO> result = new List<HEAP_INFO>(100);

                // Create a list of module base addresses
                List<IntInt> moduleBases = new List<IntInt>(process.Modules.Count);
                for( int i = 0; i < process.Modules.Count; i++ )
                {
                    moduleBases.Add( new IntInt((int) process.Modules[i].BaseAddress, i) );
                }
                IntInt.IntIntComparer comparer = new IntInt.IntIntComparer();
                moduleBases.Sort(comparer);


                // Walk the process heaps
                uint address = 0;
                uint addressLast = uint.MaxValue;
                MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();
                List<ulong> peHeaderBases = new List<ulong>(20);
                List<IntInt> addressToHeapinfoIndex = new List<IntInt>(100);
                while (address != addressLast && address < 0x7fffffff)
                {
                    // Load this heap information
                    uint blockSize = (uint)VirtualQueryEx(process.Handle, address, ref mbi, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                    

                    addressLast = address;
                    address = (uint)mbi.BaseAddress + (uint)mbi.RegionSize + 1;
                    
                    // If this is a PE header, lets mark it for processing later
                    bool isPeHeader = HeaderReader.isPeHeader(process, (ulong) mbi.BaseAddress, mbi.Protect);
                    ProcessModule associatedModule = null;
                    if( isPeHeader )
                    {
                        // Add this PE header base for later processing
                        peHeaderBases.Add((ulong)mbi.BaseAddress);

                        // Try to associate a module with this peHeader
                        int index = moduleBases.BinarySearch(new IntInt((int)mbi.BaseAddress, 0), comparer);
                        if (index < 0)
                            index = ~index - 1;
                        if (index >= 0 && index < moduleBases.Count)
                            associatedModule = process.Modules[moduleBases[index].int2];
                    }

                    // Add this heap information
                    HEAP_INFO heapInfo = new HEAP_INFO((ulong) mbi.BaseAddress, (ulong) mbi.RegionSize,
                                                       mbi.Protect.ToString(),
                                                       (isPeHeader ? "PE HEADER" : ""), associatedModule);
                    result.Add(heapInfo);
                    addressToHeapinfoIndex.Add(new IntInt((int)mbi.BaseAddress, result.Count - 1));
                }

                // Now add all the header information from the PE headers to the regions
                for (int i = 0; i < peHeaderBases.Count; i++)
                {
                    // Get this header
                    HeaderReader headerReader = new HeaderReader(process, peHeaderBases[i]);

                    // Find the associated module for this pe header
                    int moduleIndex = moduleBases.BinarySearch(new IntInt((int)peHeaderBases[i], 0), comparer);
                    ProcessModule associatedModule = null;
                    if (moduleIndex < 0)
                        moduleIndex = ~moduleIndex - 1;
                    if (moduleIndex >= 0 && moduleIndex < moduleBases.Count)
                        associatedModule = process.Modules[moduleBases[moduleIndex].int2];

                    // Record the highest address, because we know the section from the start of the PE until
                    // the highest section is all part of this module
                    int highestAddress = (int) peHeaderBases[i];

                    // Mark the execution entry point heap
                    int entry = (int) headerReader.optHeader.AddressOfEntryPoint;
                    // Associate this with a heap
                    int index = addressToHeapinfoIndex.BinarySearch(new IntInt(entry + (int)peHeaderBases[i], 0), comparer);
                    if (index < 0)
                        index = ~index - 1;
                    if (index >= 0 && index < addressToHeapinfoIndex.Count)
                    {
                        // We found the heap this section resides in, lets add this section and associate the module.
                        HEAP_INFO newInfo = result[index];
                        newInfo.associatedModule = associatedModule;

                        // Add the section information
                        newInfo.extra = (newInfo.extra + " entry_point").Trim();

                        // Change the heap info
                        result[index] = newInfo;

                        if ( (int) newInfo.heapAddress > highestAddress)
                            highestAddress = (int) newInfo.heapAddress;
                    }

                    // Add the section information to the heaps
                    foreach(section section in headerReader.sections )
                    {
                        // Associate this section with a heap
                        index = addressToHeapinfoIndex.BinarySearch(new IntInt((int)section.SectionHeader.VirtualAddress + (int)peHeaderBases[i], 0), comparer);
                        if (index < 0)
                            index = ~index - 1;
                        if (index >= 0 && index < addressToHeapinfoIndex.Count)
                        {
                            // We found the heap this section resides in, lets add this section and associate the module.
                            HEAP_INFO newInfo = result[index];
                            newInfo.associatedModule = associatedModule;

                            // Add the section information
                            newInfo.extra = section.Name + " " + newInfo.extra;

                            // Change the heap info
                            result[index] = newInfo;

                            if ((int)newInfo.heapAddress > highestAddress)
                                highestAddress = (int)newInfo.heapAddress;
                        }
                    }

                    // Now associate all heaps up until highestAddress with this module
                    int indexStart = addressToHeapinfoIndex.BinarySearch(new IntInt(entry + (int)peHeaderBases[i], 0), comparer);
                    if (indexStart < 0)
                        indexStart = ~indexStart - 1;
                    int indexEnd = addressToHeapinfoIndex.BinarySearch(new IntInt(highestAddress,0), comparer);
                    if (indexEnd < 0)
                        indexEnd = ~indexEnd - 1;
                    for( int n = indexStart; n <= indexEnd; n++ )
                    {
                        // Associate this heap
                        HEAP_INFO newInfo = result[n];
                        newInfo.associatedModule = associatedModule;
                        result[n] = newInfo;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                oConsole.printException(ex);
                return new List<HEAP_INFO>();
            }
        }

        /// <summary>
        /// Given a process, this function generates a basic memory map of the entire process. All the extra details are skipped.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static List<HEAP_INFO> GenerateMemoryMapFast(Process process)
        {
            try
            {
                // Initialize the return structure
                List<HEAP_INFO> result = new List<HEAP_INFO>(200);

                // Walk the process heaps
                uint address = 0;
                uint addressLast = uint.MaxValue;
                MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();
                while (address != addressLast && address < 0x7fffffff)
                {
                    // Load this heap information
                    uint blockSize = (uint)VirtualQueryEx(process.Handle, address, ref mbi, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                    addressLast = address;
                    address = (uint)mbi.BaseAddress + (uint)mbi.RegionSize + 1;

                    // Add this heap information
                    result.Add(new HEAP_INFO((ulong)mbi.BaseAddress, (ulong)mbi.RegionSize, mbi.Protect.ToString(), "", null));
                }
                return result;
            }
            catch (Exception ex)
            {
                oConsole.printException(ex);
                return new List<HEAP_INFO>();
            }
        }

        /// <summary>
        /// This converts a hex number string to a fixed length, ie 1A0 to 000001A0.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string MakeFixedLengthHexString(string hex, int length)
        {
            while( hex.Length < length )
                hex = "0" + hex;
            return hex;
        }

        public static string ToHex(byte[] hex)
        {
            // Convert the array into hex readable text
            string result = "";
            for (int index = 0; index < hex.Length; index++)
            {
                if (hex[index].ToString("X").Length == 1)
                    result = result + "0" + hex[index].ToString("X") + " ";
                else
                    result = result + hex[index].ToString("X") + " ";
            }
            result.TrimEnd(new char[] { ' ' });
            return result;
        }

        public static byte[] HexStringToByteArray(string text)
        {
            string tmpText = text.Replace(" ", "").ToLower();
            if ((tmpText.Length % 2) != 0 || tmpText.Length == 0)
                return null;

            // Parse the hex string
            byte[] result = new byte[tmpText.Length / 2];
            for (int i = 0; i < tmpText.Length - 1; i += 2)
            {
                if (!byte.TryParse(tmpText.Substring(i, 2), NumberStyles.HexNumber, null, out result[i / 2]))
                    return null;
            }
            return result;
        }

        public static byte[] HextToByteArray_ascii(string text)
        {
            // Parse the hex string
            byte[] result = new byte[text.Length];
            for (int i = 0; i < text.Length; i += 1)
            {
                result[i] = (byte)text[i];
            }
            return result;
        }

        public static byte[] TextToByteArray_unicode(string text)
        {
            // Parse the unicode ascii string
            byte[] result = new byte[text.Length * 2];
            for (int i = 0; i < text.Length * 2; i += 1)
            {
                if (i % 2 == 0)
                    result[i] = (byte)text[i / 2];
                else
                    result[i] = 0;
            }
            return result;
        }

        public static bool ByteArrayInByByteArray(byte[] largerArray, uint lengthLargerArray, byte[] contains)
        {
            if (largerArray == null || contains == null ) return false;
            if (largerArray.Length < lengthLargerArray) lengthLargerArray = (uint) largerArray.Length;

            // Searches the larger array for the byte array
            int n = 0;
            for (int i = 0; i < lengthLargerArray - contains.Length; i++)
            {
                // Check to see if this spot is a match
                n = 0;
                while (largerArray[i + n] == contains[n])
                {
                    if (n++ >= contains.Length - 1)
                        // We found our match
                        return true;
                }
            }
            return false;
        }

        public static string ToHex(byte[] hex, uint length)
        {
            if (length > hex.Length) length = (uint) hex.Length;
            // Convert the array into hex readable text
            string result = "";
            for (int index = 0; index < length; index++)
            {
                if (hex[index].ToString("X").Length == 1)
                    result = result + "0" + hex[index].ToString("X") + " ";
                else
                    result = result + hex[index].ToString("X") + " ";
            }
            result.TrimEnd(new char[] { ' ' });
            return result;
        }

        public static string ToAscii(byte[] hex)
        {
            // Convert the array into ascii readable text
            string result = "";
            for (int index = 0; index < hex.Length; index++)
            {
                if (hex[index] >= 32)
                    result += new string((char)hex[index], 1);
                else
                    result += "?";
            }
            result.TrimEnd(new char[] { ' ' });
            return result;
        }

        public static string ToAscii(byte[] hex, uint length)
        {
            if (length > hex.Length) length = (uint)hex.Length;
            // Convert the array into ascii readable text
            string result = "";
            for (int index = 0; index < length; index++)
            {
                if (hex[index] >= 32)
                    result += new string((char)hex[index], 1);
                else
                    result += "?";
            }
            result.TrimEnd(new char[] { ' ' });
            return result;
        }

        /// <summary>
        /// Creates a remote thread in the process starting at the specified address
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IntPtr CreateThread(Process process, ulong address)
        {
            uint threadIdentifier;
            IntPtr result = CreateRemoteThread(process.Handle, (IntPtr)null, 10000, (IntPtr)address, (IntPtr)null, 0, out threadIdentifier);
            if (result == (IntPtr)null)
                oConsole.printMessage("Failed to create remote thread at address 0x" + address.ToString("X"));
            return result;
        }

        public static uint[] ByteArrayToUintArray(ref byte[] data)
        {
            uint[] result = new uint[data.Length/4];
            for( int i = 0; i < data.Length; i+=4)
            {
                // Copy this element
                result[i/4] = BitConverter.ToUInt32(data, i);
            }
            return result;
        }

        /// <summary>
        /// Check if all of the values in hex are valid hex characters. Spaces should be removed before calling this function.
        /// </summary>
        /// <param name="hex">Hex characters to check</param>
        /// <returns>True if they are all valid hex characters.</returns>
        public static bool IsValidHex(string hex)
        {
            // Must be even number of characters
            if( (hex.Length % 2) != 0 )
                return false;

            // Check all the characters
            for( int i = 0; i < hex.Length; i++ )
            {
                if( !(( hex[i] <= 57 && hex[i] >= 48 )
                    || ( hex[i] <= 102 && hex[i] >= 97 )
                    || ( hex[i] <= 70 && hex[i] >= 65 )) )
                    return false;
            }
            return true;
        }

        // This function from http://www.matthew-long.com/2005/10/18/memory-pinning/
        public static object RawDataToObject(ref byte[] rawData, Type overlayType)
        {
            object result = null;

            GCHandle pinnedRawData = GCHandle.Alloc(rawData,
                GCHandleType.Pinned);
            try
            {

                // Get the address of the data array
                IntPtr pinnedRawDataPtr =
                    pinnedRawData.AddrOfPinnedObject();

                // overlay the data type on top of the raw data
                result = Marshal.PtrToStructure(
                    pinnedRawDataPtr,
                    overlayType);
            }
            finally
            {
                // must explicitly release
                pinnedRawData.Free();
            }

            return result;
        }

       

        /*public static FUNCTION_INFORMATION[] RawDataToFunctionInformationArray(ref byte[] rawData)
        {
            FUNCTION_INFORMATION[] result = null;

            GCHandle pinnedRawData = GCHandle.Alloc(rawData,
                GCHandleType.Pinned);
            try
            {

                // Get the address of the data array
                IntPtr pinnedRawDataPtr =
                    pinnedRawData.AddrOfPinnedObject();
                unsafe
                {
                    FUNCTION_INFORMATION* resultPtr = (FUNCTION_INFORMATION*)pinnedRawDataPtr;
                    
                    // Create our result array
                    result = new FUNCTION_INFORMATION[rawData.Length / FUNCTION_INFORMATION.SIZE];
                    for (int i = 0; i < result.Length; i++)
                        result[i] = resultPtr[i];
                }
            }
            finally
            {
                // must explicitly release
                pinnedRawData.Free();
            }

            return result;
        }*/

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          IntPtr lpStartAddress, // raw Pointer into remote process
          IntPtr lpParameter,
          uint dwCreationFlags,
          out uint lpThreadId
        );

        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, UInt32 size, ref IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32")]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
           UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("Kernel32.dll")]
        static extern void GetSystemInfo(ref SYSTEM_INFO systemInfo);
        // void GetSystemInfo( LPSYSTEM_INFO lpSystemInfo );

        [DllImport("Kernel32.dll")]
        static extern Int32 VirtualQueryEx(IntPtr hProcess, uint lpAddress, ref MEMORY_BASIC_INFORMATION buffer, Int32 dwLength);

        [DllImport("kernel32")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
           int dwSize, int dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, bool bInheritHandle, Int32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("Advapi32.dll")]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, Int32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public extern static int GetProcAddress(int hwnd, string procedureName);

        [DllImport("kernel32")]
        public extern static int LoadLibrary(string librayName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int GetLastError();
        
    }

    public struct HEAP_INFO
    {
        public ulong heapAddress;
        public ulong heapLength;
        public string heapProtection;
        public string extra;
        public ProcessModule associatedModule;

        public HEAP_INFO(ulong heapAddress, ulong heapLength, string heapProtection, string extra, ProcessModule associatedModule)
        {
            this.heapAddress = heapAddress;
            this.heapLength = heapLength;
            this.heapProtection = heapProtection;
            this.extra = extra;
            this.associatedModule = associatedModule;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SYSTEM_INFO
    {
        public Int32 dwOemId;
        public Int32 dwPageSize;
        public UInt32 lpMinimumApplicationAddress;
        public UInt32 lpMaximumApplicationAddress;
        public IntPtr dwActiveProcessorMask;
        public Int32 dwNumberOfProcessors;
        public Int32 dwProcessorType;
        public Int32 dwAllocationGranularity;
        public Int16 wProcessorLevel;
        public Int16 wProcessorRevision;
    }

    [Flags]
    public enum MEMORY_STATE : int
    {
        COMMIT = 0x1000,
        FREE = 0x10000,
        RESERVE = 0x2000
    }

    [Flags]
    public enum MEMORY_TYPE : int
    {
        IMAGE = 0x1000000,
        MAPPED = 0x40000,
        PRIVATE = 0x20000
    }

    [Flags]
    public enum MEMORY_PROTECT : int
    {
        PAGE_UNKNOWN = 0x0,
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READWRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_NOACCESS = 0x01,
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public MEMORY_PROTECT AllocationProtect;
        public UInt32 RegionSize;
        public MEMORY_STATE State;
        public MEMORY_PROTECT Protect;
        public MEMORY_TYPE Type;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LUID
    {
        public Int32 LowPart;
        public Int32 HighPart;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TOKEN_PRIVILEGES
    {
        public Int32 PrivilegeCount;
        public LUID_AND_ATTRIBUTES Privileges;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public Int32 Attributes;
    }
}
