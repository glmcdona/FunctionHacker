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
using FunctionHacker.Classes;

namespace Program_Execution_Flow_Hacker_and_Visualizer.Memory_Classes
{
    public static class oModuleLookup
    {
        private static uint[] moduleBaseList = null;
        private static string[] moduleNameList = null;
        
        public static void generateLookupTable(Process process)
        {
            // This function generates the sorted lookup table for the specified process
            moduleBaseList = new uint[process.Modules.Count];
            moduleNameList = new string[process.Modules.Count];
            int bestMatchIndex = -1;
            uint bestMatchAddress = uint.MaxValue;
            uint lastAddress = 0;
            for( int destIndex = 0; destIndex < process.Modules.Count; destIndex++ )
            {
                // Search for the next smallest base address module
                bestMatchAddress = uint.MaxValue;
                for( int i = 0; i < process.Modules.Count; i++ )
                {
                    if ((uint)process.Modules[i].BaseAddress > lastAddress && (uint)process.Modules[i].BaseAddress < bestMatchAddress)
                    {
                        bestMatchIndex = i;
                        bestMatchAddress = (uint)process.Modules[i].BaseAddress;
                    }
                }
                lastAddress = bestMatchAddress;

                // Add this base address module
                moduleBaseList[destIndex] = bestMatchAddress;
                moduleNameList[destIndex] = process.Modules[bestMatchIndex].ModuleName;
            }
        }

        public static string[] getModuleNameList()
        {
            return moduleNameList;
        }

        public static string getModuleFromIndex(byte index)
        {
            if (index == byte.MaxValue)
                return "not a module";
            else
                return moduleNameList[index];
        }

        public static int getNumModules()
        {
            return moduleBaseList.Length;
        }

        public static byte getModuleIndex(uint address)
        {
            // Find the module to go with this address

            if (moduleBaseList == null || moduleNameList == null)
                return byte.MaxValue;

            // Search the list
            int index = 0;
            while (index < moduleNameList.Length && moduleBaseList[index] < address)
                index++;

            if (index >= moduleNameList.Length)
                return (byte)( moduleNameList.Length - 1);
            else
                return (byte)(index - 1);
        }

        public static string getModuleName(uint address)
        {
            // Find the module to go with this address

            if( moduleBaseList == null || moduleNameList == null )
                return "not initialized";

            // Search the list
            int index = 0;
            while (index < moduleNameList.Length && moduleBaseList[index] < address)
                index++;

            if (index >= moduleNameList.Length)
                return moduleNameList[moduleNameList.Length - 1];
            else if (index == 0)
                return "no module?";
            else
                return moduleNameList[index-1];
        }

        public static bool validExecuteAddress(uint address)
        {
            bool result = false;

            // First make sure this address makes sense
            if (address > 0x0000000000 && address < 0xffffffff)
            {
                // Make sure the destination has access writes execute
                MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();
                try
                {
                    int size = VirtualQueryEx(oProcess.activeProcess.Handle, (IntPtr)address, ref mbi, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                    if (mbi.Protect == MEMORY_PROTECT.PAGE_EXECUTE ||
                        mbi.Protect == MEMORY_PROTECT.PAGE_EXECUTE_READ ||
                        mbi.Protect == MEMORY_PROTECT.PAGE_EXECUTE_READWRITE ||
                        mbi.Protect == MEMORY_PROTECT.PAGE_EXECUTE_WRITECOPY)
                    {
                        // This function call is to a execute page.
                        result = true;
                    }
                }
                catch
                {
                }
            }
            return result;
        }

        [DllImport("Kernel32.dll")]
        private static extern Int32 VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress,
                                                   ref MEMORY_BASIC_INFORMATION buffer, Int32 dwLength);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct SYSTEM_INFO
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
        internal enum MEMORY_STATE
        {
            COMMIT = 0x1000,
            FREE = 0x10000,
            RESERVE = 0x2000
        }

        [Flags]
        internal enum MEMORY_TYPE
        {
            IMAGE = 0x1000000,
            MAPPED = 0x40000,
            PRIVATE = 0x20000
        }

        [Flags]
        internal enum MEMORY_PROTECT
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
        internal struct MEMORY_BASIC_INFORMATION
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
        internal struct LUID
        {
            public Int32 LowPart;
            public Int32 HighPart;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TOKEN_PRIVILEGES
        {
            public Int32 PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public Int32 Attributes;
        }
    }
}
