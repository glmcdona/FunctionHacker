using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BufferOverflowProtection;

namespace OLD
{
    public struct ADDRESS_RANGE
    {
        public uint start;
        public uint size;
        public ADDRESS_RANGE(uint address, uint size)
        {
            this.start = address;
            this.size = size;
        }

        public bool isValid(uint address)
        {
            if( address >= start && address < start + size )
                return false;
            return true;
        }
    }

    public class oHeaderReader
    {
        public uint codeLength;
        public uint codeStartAddress;
        public COFFHeader coffHeader;
        public IMAGE_DOS_HEADER dosHeader;
        public List<EXPORT_FUNCTION> exportTable;
        public List<IMPORT_FUNCTION> importTable;
        public PEOptHeader optHeader;
        public bool success;
        public List<ADDRESS_RANGE> invalidCodeAddresses;
        public List<IMAGE_SECTION_HEADER_MOD> sections;

        /// <summary>
        /// Check all invalid code ranges to see if this pointer is invalid for a code region.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool isValidCodePointer(uint address)
        {
            foreach(ADDRESS_RANGE range in invalidCodeAddresses)
                if( !range.isValid(address) )
                    return false;
            return true;
        }


        /// <summary>
        /// This checks whether the specified heap is
        /// a valid pe header.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool isPeHeader(Process process, UInt64 address)
        {
            // Read in the Image Dos Header
            byte[] headerData = oMemoryFunctions.readMemory(process, address,
                                                            (uint)Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)));
            IMAGE_DOS_HEADER dosHeader = (IMAGE_DOS_HEADER)oMemoryFunctions.RawDataToObject(ref headerData, typeof(IMAGE_DOS_HEADER));

            // Load the PE Address
            UInt64 PE_header = 0;
            if (dosHeader.e_magic == 0x5A4D)
            {
                // Load the PE header address
                PE_header = dosHeader.e_lfanew + address;
            }
            else
            {
                PE_header = address;
                return false;
            }

            if (PE_header <= 0x7FFFFFFF)
            {
                // Read in the PE token
                byte[] PE_Token = oMemoryFunctions.readMemory(process, PE_header, 4);
                if (!(PE_Token[0] == 'P' & PE_Token[1] == 'E' & PE_Token[2] == 0 & PE_Token[3] == 0))
                {
                    // Problem, we are not pointing at a correct PE header. Abort.
                    return false;
                }

                // Input the COFFHeader
                byte[] coffHeader_rawData = oMemoryFunctions.readMemory(process, PE_header + 4, (uint)Marshal.SizeOf(typeof(COFFHeader)));
                COFFHeader coffHeader = (COFFHeader)oMemoryFunctions.RawDataToObject(ref coffHeader_rawData, typeof(COFFHeader));

                // Read in the PEOptHeader if it exists
                if (coffHeader.SizeOfOptionalHeader != 0)
                {
                    if (coffHeader.SizeOfOptionalHeader != (ushort)Marshal.SizeOf(typeof(PEOptHeader)))
                    {
                        // Problem!
                        return false;
                    }
                    else
                    {
                        // Read in the optHeader
                        byte[] optHeader_rawData = oMemoryFunctions.readMemory(process, PE_header + 4 + (uint)Marshal.SizeOf(typeof(COFFHeader)), (uint)Marshal.SizeOf(typeof(PEOptHeader)));
                        PEOptHeader optHeader = (PEOptHeader)oMemoryFunctions.RawDataToObject(ref optHeader_rawData, typeof(PEOptHeader));

                        // Confirm that it loaded correctly
                        if (optHeader.signature != 267)
                        {
                            return false;
                        }

                        if( optHeader.SizeOfCode == 0 )
                            return false;
                    }
                }
                else
                {
                    // No COFFHeader found
                    return false;
                }


                return true;
            }
            else
            {
                return false;

            }
        }

        /// <summary>
        /// This function loads the header and PE header at the
        /// specified address in memory.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        public oHeaderReader(Process process, UInt64 address)
        {
            codeStartAddress = 0;
            codeLength = 0;
            invalidCodeAddresses = new List<ADDRESS_RANGE>(0);

            // Read in the Image Dos Header
            importTable = new List<IMPORT_FUNCTION>(0);
            byte[] headerData = oMemoryFunctions.readMemory(process, address, (uint)Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)));
            dosHeader = (IMAGE_DOS_HEADER)oMemoryFunctions.RawDataToObject(ref headerData, typeof(IMAGE_DOS_HEADER));

            // Load the PE Address
            UInt64 PE_header = 0;
            if (dosHeader.e_magic == 0x5A4D)
            {
                // Load the PE header address
                PE_header = dosHeader.e_lfanew + address;
            }
            else
            {
                PE_header = address;
            }

            // Read in the PE token
            byte[] PE_Token = oMemoryFunctions.readMemory(process, PE_header, 4);
            if (!(PE_Token[0] == 'P' & PE_Token[1] == 'E' & PE_Token[2] == 0 & PE_Token[3] == 0))
            {
                // Problem, we are not pointing at a correct PE header. Abort.
                oConsole.printMessage("Failed to read PE header from block " + address.ToString("X") + " with PE header located at " + PE_header.ToString() + ".");
                return;
            }

            // Input the COFFHeader
            byte[] coffHeader_rawData = oMemoryFunctions.readMemory(process, PE_header + 4, (uint)Marshal.SizeOf(typeof(COFFHeader)));
            coffHeader = (COFFHeader)oMemoryFunctions.RawDataToObject(ref coffHeader_rawData, typeof(COFFHeader));

            // Read in the PEOptHeader if it exists
            if (coffHeader.SizeOfOptionalHeader != 0)
            {
                if (coffHeader.SizeOfOptionalHeader != (ushort)Marshal.SizeOf(typeof(PEOptHeader)))
                {
                    // Problem!
                    oConsole.printMessage("Failed to read COFFHeader as a result of size mismatch. Size of expected COFFHeader, " + ((ushort)Marshal.SizeOf(typeof(PEOptHeader))).ToString() + ", does not equal size of SizeOfOptionalHeader, " + coffHeader.SizeOfOptionalHeader.ToString() + ".");
                    return;
                }
                else
                {
                    // Read in the optHeader
                    byte[] optHeader_rawData = oMemoryFunctions.readMemory(process, PE_header + 4 + (uint)Marshal.SizeOf(typeof(COFFHeader)), (uint)Marshal.SizeOf(typeof(PEOptHeader)));
                    optHeader = (PEOptHeader)oMemoryFunctions.RawDataToObject(ref optHeader_rawData, typeof(PEOptHeader));

                    // Confirm that it loaded correctly
                    if (optHeader.signature != 267)
                    {
                        oConsole.printMessage("Failed to read optHeader; Expected signature of 267 does not match read in signature of " + optHeader.signature.ToString() + ".");
                        return;
                    }
                }
            }
            else
            {
                // No COFFHeader found
                oConsole.printMessage("Warning, no COFFHeader found for address " + address.ToString("X") + ".");
                return;
            }
            

            // Add all the directories as invalid code ranges
            try
            {
                if (optHeader.DataDirectory1_export.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory1_export.VirtualAddress),
                            optHeader.DataDirectory1_export.Size));
                if (optHeader.DataDirectory2_import.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory2_import.VirtualAddress),
                            optHeader.DataDirectory2_import.Size));
                if (optHeader.DataDirectory3.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory3.VirtualAddress),
                            optHeader.DataDirectory3.Size));
                if (optHeader.DataDirectory4.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory4.VirtualAddress),
                            optHeader.DataDirectory4.Size));
                if (optHeader.DataDirectory5.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory5.VirtualAddress),
                            optHeader.DataDirectory5.Size));
                if (optHeader.DataDirectory6.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory6.VirtualAddress),
                            optHeader.DataDirectory6.Size));
                if (optHeader.DataDirectory7.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory7.VirtualAddress),
                            optHeader.DataDirectory7.Size));
                if (optHeader.DataDirectory8.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory8.VirtualAddress),
                            optHeader.DataDirectory8.Size));
                if (optHeader.DataDirectory9.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory9.VirtualAddress),
                            optHeader.DataDirectory9.Size));
                if (optHeader.DataDirectory10.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory10.VirtualAddress),
                            optHeader.DataDirectory10.Size));
                if (optHeader.DataDirectory11.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory11.VirtualAddress),
                            optHeader.DataDirectory11.Size));
                if (optHeader.DataDirectory12.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory12.VirtualAddress),
                            optHeader.DataDirectory12.Size));
                if (optHeader.DataDirectory13.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory13.VirtualAddress),
                            optHeader.DataDirectory13.Size));
                if (optHeader.DataDirectory14.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory14.VirtualAddress),
                            optHeader.DataDirectory14.Size));
                if (optHeader.DataDirectory15.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory15.VirtualAddress),
                            optHeader.DataDirectory15.Size));
                if (optHeader.DataDirectory16.Size > 0)
                    invalidCodeAddresses.Add(
                        new ADDRESS_RANGE(
                            oMemoryFunctions.addressAdd(address, optHeader.DataDirectory16.VirtualAddress),
                            optHeader.DataDirectory16.Size));
            }catch
            {
                // Ignore exceptions, this is not cretical.
            }

            // Extract the names of the functions and corresponding table entries
            uint importTableAddress = oMemoryFunctions.addressAdd(optHeader.DataDirectory2_import.VirtualAddress,(uint) address);
            uint exportTableAddress = oMemoryFunctions.addressAdd(optHeader.DataDirectory1_export.VirtualAddress, (uint) address);

            // Load the sections
            UInt64 sectionAddress = PE_header + 4 + (UInt64)Marshal.SizeOf(typeof(COFFHeader)) + coffHeader.SizeOfOptionalHeader;
            List<section> sections = new List<section>(coffHeader.NumberOfSections);
            for (int i = 0; i < coffHeader.NumberOfSections; i++)
            {
                sections.Add(new section( process, sectionAddress ));
                sectionAddress += (uint)Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER));
            }  

            // Load the import directory
            loadImportTable(process, importTableAddress, optHeader.DataDirectory2_import.Size, (uint)address);

            // Load the export directory
            loadExportTable(process, exportTableAddress, optHeader.DataDirectory1_export.Size, (uint) address, optHeader, sections);

            // Load the section structures
            int numSections = coffHeader.NumberOfSections;
            sections = new List<IMAGE_SECTION_HEADER_MOD>(numSections);
            for( uint i = 0; i < numSections; i++ )
            {
                ulong sectionBase = PE_header + 4 + (ulong)Marshal.SizeOf(typeof(COFFHeader)) + (uint)Marshal.SizeOf(typeof(PEOptHeader)) + i * (uint)Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER));
                byte[] sectionData = oMemoryFunctions.readMemory(process, sectionBase, (uint)Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER)));
                IMAGE_SECTION_HEADER section = (IMAGE_SECTION_HEADER)oMemoryFunctions.RawDataToObject(ref sectionData, typeof(IMAGE_SECTION_HEADER));

                // Convert the raw section to a more friendly section type
                sections.Add(new IMAGE_SECTION_HEADER_MOD(section));
            }
        }

        private void loadImportTable(Process process, uint importTableAddress, uint length, uint baseAddress)
        {
            // Read in the first import descriptor structure
            byte[] image_import_descriptor_rawData = oMemoryFunctions.readMemory(process, importTableAddress, (uint)Marshal.SizeOf(typeof(image_import_descriptor)));
            image_import_descriptor descriptor = (image_import_descriptor)oMemoryFunctions.RawDataToObject(ref image_import_descriptor_rawData, typeof(image_import_descriptor));
            List<image_import_by_name> imports = new List<image_import_by_name>(0);
            while (descriptor.FirstThunk != 0 || descriptor.OriginalFirstThunk != 0 || descriptor.Name != 0)
            {
                // Process this descriptor
                imports.Add(new image_import_by_name(process, descriptor, (int)baseAddress));

                // Read next descriptor
                image_import_descriptor_rawData = oMemoryFunctions.readMemory(process, oMemoryFunctions.addressAdd(importTableAddress, (ulong)(imports.Count * Marshal.SizeOf(typeof(image_import_descriptor)))), (uint)Marshal.SizeOf(typeof(image_import_descriptor)));
                descriptor = (image_import_descriptor)oMemoryFunctions.RawDataToObject(ref image_import_descriptor_rawData, typeof(image_import_descriptor));
            }

            // Now lets process the array of descriptors
            foreach (image_import_by_name dllDescriptor in imports)
            {
                for (int i = 0; i < dllDescriptor.ImportTableAddresses.Count; i++)
                {
                    // Process this function import
                    try
                    {
                        IMPORT_FUNCTION newImport = new IMPORT_FUNCTION();
                        newImport.name = dllDescriptor.Names[i];
                        newImport.memoryAddress = (uint) dllDescriptor.ImportTableAddresses[i];
                        newImport.targetAddress = oMemoryFunctions.readMemoryDword(process,
                                                                                   (ulong) newImport.memoryAddress);
                        importTable.Add(newImport);
                    }catch
                    {
                        // Not a critical error
                    }
                }
            }
        }

        private void loadExportTable(Process process, uint exportTableAddress, uint length, uint baseAddress, PEOptHeader optHeader, List<section> sections)
        {
          // Read in the first _IMAGE_EXPORT_DIRECTORY structure
          byte[] export_directory_rawData = MemoryFunctions.ReadMemory(process, exportTableAddress, (uint)Marshal.SizeOf(typeof(_IMAGE_EXPORT_DIRECTORY)));
          _IMAGE_EXPORT_DIRECTORY export_directory = (_IMAGE_EXPORT_DIRECTORY)MemoryFunctions.RawDataToObject(ref export_directory_rawData, typeof(_IMAGE_EXPORT_DIRECTORY));
          exports = new Hashtable(20);

          UInt64 functionNameArray = (UInt64)export_directory.AddressOfNames + address;
          string exportName = MemoryFunctions.ReadString(process, (UInt64)export_directory.Name + address, MemoryFunctions.STRING_TYPE.ascii);
          UInt64 functionOrdinalArray = (UInt64)export_directory.AddressOfNameOrdinal + address;
          UInt64 functionAddressArray = (UInt64)export_directory.AddressOfFunctions + address;
          for (int i = 0; i < export_directory.NumberOfNames; i++)
          {
              int ordinal_relative = (int)MemoryFunctions.ReadMemoryUShort(process, functionOrdinalArray + (UInt64)i * 2);
              int ordinal = ordinal_relative + (int)export_directory.Base;

              if (ordinal_relative < export_directory.NumberOfFunctions)
              {
                  // Continue with importing this function
                  string name = "";
                  if (i < export_directory.NumberOfNames)
                      name = MemoryFunctions.ReadString(process, (UInt64)MemoryFunctions.ReadMemoryDword(process, functionNameArray + (UInt64)i * 4) + address, MemoryFunctions.STRING_TYPE.ascii);
                  else
                      name = "oridinal" + ordinal.ToString();
                  
                  // Lookup the function rva now
                  try
                  {
                      UInt64 offset = (UInt64)MemoryFunctions.ReadMemoryDword(process, functionAddressArray + (UInt64)ordinal_relative * 4);
                      
                      // Check to see if this is a forwarded export
                      if (offset < optHeader.DataDirectory1_export.VirtualAddress ||
                          offset > optHeader.DataDirectory1_export.VirtualAddress + optHeader.DataDirectory1_export.Size)
                      {
                          // Lookup privilege of heap to confirm it requests execute privileges. We only want to list exported functions, not variables.
                          foreach (section exp in sections)
                          {
                              if (exp.Contains(offset))
                              {
                                  if (exp.IsExecutable())
                                  {
                                      // Add this exported function
                                      export new_export = new export(offset + address,
                                                          name,
                                                          ordinal);

                                      exports.Add(name.ToLower(), new_export);
                                  }
                                  break;
                              }
                          }
                      }
                      else
                      {
                          // Forwarded export. Ignore it.
                      }
                  }
                  catch (Exception e)
                  {
                      Console.WriteLine("Warning, failed to parse PE header export directory entry for name '" + name + "'.");
                  }
              }
          }
      }
      else
      {
          // No COFFHeader found
          Console.WriteLine("Warning, no COFFHeader found for address " + address.ToString("X") + ".");
          return;
      }

          /*
            // Read in the first import descriptor structure
            byte[] image_import_descriptor_rawData = oMemoryFunctions.readMemory(process, exportTableAddress, (uint)Marshal.SizeOf(typeof(IMAGE_EXPORT_DIRECTORY)));
            IMAGE_EXPORT_DIRECTORY descriptor = (IMAGE_EXPORT_DIRECTORY)oMemoryFunctions.RawDataToObject(ref image_import_descriptor_rawData, typeof(IMAGE_EXPORT_DIRECTORY));

            if (descriptor.NumberOfFunctions < 10000)
            {
                // Add all the exported functions without their names first
                exportTable = new List<EXPORT_FUNCTION>((int)descriptor.NumberOfFunctions);

                // Read in the AddressOfFunctions array
                List<uint> functionArray = oMemoryFunctions.readMemoryDwordArray(process, oMemoryFunctions.addressAdd(descriptor.AddressOfFunctions, baseAddress),
                                                                                (int)descriptor.NumberOfFunctions);
                if (functionArray.Count == descriptor.NumberOfFunctions)
                {
                    // Add these exports without names
                    for (int i = 0; i < (int)descriptor.NumberOfFunctions; i++)
                    {
                        // Add this export function
                        exportTable.Add(new EXPORT_FUNCTION((uint)((descriptor.AddressOfFunctions + baseAddress + i * 4) & 0x7fffffff), oMemoryFunctions.addressAdd(functionArray[i], baseAddress),
                                                            "ordinal 0x" + (descriptor.Base + i).ToString("X")));
                    }

                    // Read in the AddressofNames and AddressOfNameOrdinals tables
                    List<uint> nameArray = oMemoryFunctions.readMemoryDwordArray(process, oMemoryFunctions.addressAdd(descriptor.AddressOfNames, baseAddress), (int)descriptor.NumberOfNames);
                    List<UInt16> ordinalArray = oMemoryFunctions.readMemoryWordArray(process, oMemoryFunctions.addressAdd(descriptor.AddressOfNameOrdinals, baseAddress), (int)descriptor.NumberOfFunctions);
                    if (nameArray.Count <= ordinalArray.Count)
                    {
                        // Lookup the names of the functions that are exported
                        for (int i = 0; i < (int)descriptor.NumberOfNames; i++)
                        {
                            try
                            {
                                // Check that this is a valid string-to-export mapping
                                int indexFunctionTable = (int)ordinalArray[i];
                                if (indexFunctionTable >= 0 && indexFunctionTable < exportTable.Count)
                                {
                                    // Load this export name
                                    string name = oMemoryFunctions.readMemoryString(process,
                                                                                    oMemoryFunctions.addressAdd(
                                                                                        nameArray[i], baseAddress), 256);

                                    if (name.Length > 0)
                                    {
                                        // Set this export name
                                        exportTable[indexFunctionTable].setName(name);
                                    }

                                }
                            }
                            catch
                            {
                                // Not a critical error
                            }
                        }
                    }
                }
            }
            else
            {
                exportTable = new List<EXPORT_FUNCTION>(0);
            }
           * */
        }

        public static bool isPeHeader(Process process, ulong p, MEMORY_PROTECT memoryProtection)
        {
            // We don't want to set off a page guard exception!
            if (memoryProtection.ToString().ToUpper().Contains("GUARD"))
                return false;
            else
                return isPeHeader(process, p);
        }
    }


    public class section
    {
        private IMAGE_SECTION_HEADER _sectionHeader;

        public section(System.Diagnostics.Process process, UInt64 address)
        {
            // Parse the section struct
            byte[] headerData = MemoryFunctions.ReadMemory(process, (IntPtr)address, (uint)Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER)));
            _sectionHeader = (IMAGE_SECTION_HEADER)MemoryFunctions.RawDataToObject(ref headerData, typeof(IMAGE_SECTION_HEADER));
        }

        public string Section
        {
            get { return _sectionHeader.Section; }
        }

        public DataSectionFlags Characteristics
        {
            get { return _sectionHeader.Characteristics; }
        }

        public bool Contains(UInt64 rva)
        {
            return rva >= _sectionHeader.VirtualAddress && rva - _sectionHeader.VirtualAddress < _sectionHeader.VirtualSize;
        }

        public bool IsExecutable()
        {
            return ( ((uint) _sectionHeader.Characteristics) & (uint) DataSectionFlags.MemoryExecute ) > 0;
        }
    }

    

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_EXPORT_DIRECTORY
    {
        public UInt32 Characteristics;
        public UInt32 TimeDateStamp;
        public UInt16 MajorVersion;
        public UInt16 MinorVersion;
        public UInt32 Name;
        public UInt32 Base;
        public UInt32 NumberOfFunctions;
        public UInt32 NumberOfNames;
        public UInt32 AddressOfFunctions;     // RVA from base of image
        public UInt32 AddressOfNames;     // RVA from base of image
        public UInt32 AddressOfNameOrdinals;  // RVA from base of image
    }

    public class IMPORT_FUNCTION
    {
        public uint memoryAddress;
        public uint targetAddress;
        public string name;
    }

    public class EXPORT_FUNCTION
    {
        public uint source;
        public uint address;
        public string name;
        public EXPORT_FUNCTION(uint source, uint address, string name)
        {
            this.address = address;
            this.name = name;
            this.source = source;
        }

        public void setName(string name)
        {
            this.name = name;
        }
    }

    public struct IMAGE_DOS_HEADER
    {
        // DOS .EXE header
        public UInt16 e_magic; // Magic number, should be value to value to 0x54AD
        public UInt16 e_cblp; // Bytes on last page of file
        public UInt16 e_cp; // Pages in file
        public UInt16 e_crlc; // Relocations
        public UInt16 e_cparhdr; // Size of header in paragraphs
        public UInt16 e_minalloc; // Minimum extra paragraphs needed
        public UInt16 e_maxalloc; // Maximum extra paragraphs needed
        public UInt16 e_ss; // Initial (relative) SS value
        public UInt16 e_sp; // Initial SP value
        public UInt16 e_csum; // Checksum
        public UInt16 e_ip; // Initial IP value
        public UInt16 e_cs; // Initial (relative) CS value
        public UInt16 e_lfarlc; // File address of relocation table
        public UInt16 e_ovno; // Overlay number
        public UInt16 e_res_1; // Reserved words
        public UInt16 e_res_2; // Reserved words
        public UInt16 e_res_3; // Reserved words
        public UInt16 e_res_4; // Reserved words
        public UInt16 e_oemid; // OEM identifier (for e_oeminfo)
        public UInt16 e_oeminfo; // OEM information; e_oemid specific
        public UInt16 e_res2_1; // Reserved words
        public UInt16 e_res2_2; // Reserved words
        public UInt16 e_res2_3; // Reserved words
        public UInt16 e_res2_4; // Reserved words
        public UInt16 e_res2_5; // Reserved words
        public UInt16 e_res2_6; // Reserved words
        public UInt16 e_res2_7; // Reserved words
        public UInt16 e_res2_8; // Reserved words
        public UInt16 e_res2_9; // Reserved words
        public UInt16 e_res2_10; // Reserved words
        public UInt32 e_lfanew; // Offset of PE header
    }

    public struct COFFHeader
    {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }

    public struct PEOptHeader
    {
        public UInt16 signature; //decimal number 267.
        public char MajorLinkerVersion;
        public char MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint; //The RVA of the code entry point
        public UInt32 BaseOfCode;
        public UInt32 BaseOfData;
        public UInt32 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOSVersion;
        public UInt16 MinorOSVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Reserved;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 Checksum;
        public UInt16 Subsystem;
        public UInt16 DLLCharacteristics;
        public UInt32 SizeOfStackReserve;
        public UInt32 SizeOfStackCommit;
        public UInt32 SizeOfHeapReserve;
        public UInt32 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;

        public data_directory DataDirectory1_export;
                              //Can have any number of elements, matching the number in NumberOfRvaAndSizes.

        public data_directory DataDirectory2_import;
        public data_directory DataDirectory3;
        public data_directory DataDirectory4;
        public data_directory DataDirectory5;
        public data_directory DataDirectory6;
        public data_directory DataDirectory7;
        public data_directory DataDirectory8;
        public data_directory DataDirectory9;
        public data_directory DataDirectory10;
        public data_directory DataDirectory11;
        public data_directory DataDirectory12;
        public data_directory DataDirectory13;
        public data_directory DataDirectory14;
        public data_directory DataDirectory15;
        public data_directory DataDirectory16;
    }

    public struct data_directory
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMAGE_SECTION_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Name;
        public uint PhysicalAddress;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLinenumbers;
        public short NumberOfRelocations;
        public short NumberOfLinenumbers;
        public uint Characteristics;
    }

    public struct IMAGE_SECTION_HEADER_MOD
    {
        public string Name;
        public uint PhysicalAddress;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLinenumbers;
        public short NumberOfRelocations;
        public short NumberOfLinenumbers;
        public uint Characteristics;

        public IMAGE_SECTION_HEADER_MOD(IMAGE_SECTION_HEADER section)
        {
            this.Name = oMemoryFunctions.byteArrayToString(section.Name);
            this.PhysicalAddress = section.PhysicalAddress;
            this.VirtualAddress = section.VirtualAddress;
            this.SizeOfRawData = section.SizeOfRawData;
            this.PointerToRawData = section.PointerToRawData;
            this.PointerToRelocations = section.PointerToRelocations;
            this.PointerToLinenumbers = section.PointerToLinenumbers;
            this.NumberOfRelocations = section.NumberOfRelocations;
            this.NumberOfLinenumbers = section.NumberOfLinenumbers;
            this.Characteristics = section.Characteristics;
        }
    }

    public struct image_import_descriptor
    {
        public int OriginalFirstThunk;
        public int TimeDateStamp;
        public int ForwarderChain;
        public int Name;
        public int FirstThunk;
    }



    public class image_import_by_name {
        public List<uint> AddressOfData;
        public List<string> Names;
        public List<int> ImportTableAddresses;

        public image_import_by_name(Process process, image_import_descriptor descriptor, int baseAddress)
        {
            AddressOfData = new List<uint>(0);
            ImportTableAddresses = new List<int>(0);
            Names = new List<string>(0);

            // Read the AddressOfData array in
            if (descriptor.OriginalFirstThunk != 0 && descriptor.FirstThunk != 0)
            {
                // Read in the descriptor table with names
                int i = 0;
                uint newAddress = oMemoryFunctions.readMemoryDword(process,
                                                                   ((ulong) descriptor.OriginalFirstThunk +
                                                                   (ulong) baseAddress + (ulong) i*4) & 0x7fffffff);
                while (newAddress != 0)
                {
                    // Add this new address
                    AddressOfData.Add(newAddress & 0x7fffffff);

                    // Add the string corresponding to this new function
                    if ((newAddress & 0x80000000) > 0)
                    {
                        // Export by ordinal
                        newAddress = newAddress & 0x7fffffff;
                        Names.Add("ordinal 0x" +
                                  oMemoryFunctions.readMemoryUShort(process, (ulong)(newAddress + baseAddress) & 0x7fffffff).
                                      ToString("X"));
                    }
                    else if (newAddress > 0)
                    {
                        // Export by name
                        Names.Add(oMemoryFunctions.readMemoryString(process, (ulong)(newAddress + baseAddress + 2) & 0x7fffffff, 256));
                    }
                    else
                    {
                        // There is no name or ordinal given, probably obfuscated?
                        Names.Add("obfuscated?");
                    }

                    // Add the pe table address corresponding to this function
                    ImportTableAddresses.Add((descriptor.FirstThunk + baseAddress + i*4) & 0x7fffffff);

                    // Read in the next value
                    i++;
                    newAddress = oMemoryFunctions.readMemoryDword(process,
                                                                  ((ulong) descriptor.OriginalFirstThunk +
                                                                  (ulong) baseAddress + (ulong) i*4) & 0x7fffffff);
                }
            }else
            {
                // Read in the descriptor table without names
                int i = 0;
                ulong addressThunk = (ulong) (descriptor.OriginalFirstThunk != 0
                                          ? descriptor.OriginalFirstThunk
                                          : descriptor.FirstThunk);
                if( addressThunk == 0 )
                    return; // We have no imports that we can read in.

                uint newAddress = oMemoryFunctions.readMemoryDword(process,
                                                                   (addressThunk +
                                                                   (ulong)baseAddress + (ulong)i * 4) & 0x7fffffff);
                while (newAddress != 0)
                {
                    // Add this new address
                    AddressOfData.Add(newAddress & 0x7fffffff);
                    Names.Add("");
                    ImportTableAddresses.Add( ( (int)addressThunk + baseAddress + i * 4 ) & 0x7fffffff);
                    

                    // Read in the next value
                    i++;
                    newAddress = oMemoryFunctions.readMemoryDword(process,
                                                                   (addressThunk +
                                                                   (ulong)baseAddress + (ulong)i * 4) & 0x7fffffff);
                }
            }
        }
    }
}