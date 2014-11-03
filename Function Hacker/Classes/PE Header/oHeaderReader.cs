using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FunctionHacker.Classes
{
  public class PEOptHeader
  {
    public bool _is64;
    PEOptHeader32 _opt32;
    PEOptHeader64 _opt64;


    // Wrapper for PEOptHeader32 and PEOptHeader64
    public UInt32 signature //decimal number 267.
    { get { return (_is64 ? _opt64.Magic : _opt32.signature); } }
    public byte MajorLinkerVersion
    { get { return (_is64 ? _opt64.MajorLinkerVersion : _opt32.MajorLinkerVersion); } }
    public byte MinorLinkerVersion
    { get { return (_is64 ? _opt64.MinorLinkerVersion : _opt32.MinorLinkerVersion); } }
    public UInt32 SizeOfCode
    { get { return (_is64 ? _opt64.SizeOfCode : _opt32.SizeOfCode); } }
    public UInt32 SizeOfInitializedData
    { get { return (_is64 ? _opt64.SizeOfInitializedData : _opt32.SizeOfInitializedData); } }
    public UInt32 SizeOfUninitializedData
    { get { return (_is64 ? _opt64.SizeOfUninitializedData : _opt32.SizeOfUninitializedData); } }
    public UInt32 AddressOfEntryPoint //The RVA of the code entry point
    { get { return (_is64 ? _opt64.AddressOfEntryPoint : _opt32.AddressOfEntryPoint); } }
    public UInt32 BaseOfCode
    { get { return (_is64 ? _opt64.BaseOfCode : _opt32.BaseOfCode); } }
    public UInt64 BaseOfData
    { get { return (_is64 ? _opt64.ImageBase : _opt32.BaseOfData); } }
    public UInt64 ImageBase
    { get { return (_is64 ? _opt64.ImageBase : _opt32.ImageBase); } }
    public UInt32 SectionAlignment
    { get { return (_is64 ? _opt64.SectionAlignment : _opt32.SectionAlignment); } }
    public UInt32 FileAlignment
    { get { return (_is64 ? _opt64.FileAlignment : _opt32.FileAlignment); } }
    public UInt16 MajorOSVersion
    { get { return (_is64 ? _opt64.MajorOperatingSystemVersion : _opt32.MajorOSVersion); } }
    public UInt16 MinorOSVersion
    { get { return (_is64 ? _opt64.MinorOperatingSystemVersion : _opt32.MinorOSVersion); } }
    public UInt16 MajorImageVersion
    { get { return (_is64 ? _opt64.MajorImageVersion : _opt32.MajorImageVersion); } }
    public UInt16 MinorImageVersion
    { get { return (_is64 ? _opt64.MinorImageVersion : _opt32.MinorImageVersion); } }
    public UInt16 MajorSubsystemVersion
    { get { return (_is64 ? _opt64.MajorSubsystemVersion : _opt32.MajorSubsystemVersion); } }
    public UInt16 MinorSubsystemVersion
    { get { return (_is64 ? _opt64.MinorSubsystemVersion : _opt32.MinorSubsystemVersion); } }
    public UInt32 Reserved
    { get { return (_is64 ? _opt64.Win32VersionValue : _opt32.Reserved); } }
    public UInt32 SizeOfImage
    { get { return (_is64 ? _opt64.SizeOfImage : _opt32.SizeOfImage); } }
    public UInt32 SizeOfHeaders
    { get { return (_is64 ? _opt64.SizeOfHeaders : _opt32.SizeOfHeaders); } }
    public UInt32 Checksum
    { get { return (_is64 ? _opt64.CheckSum : _opt32.Checksum); } }
    public UInt16 Subsystem
    { get { return (_is64 ? _opt64.Subsystem : _opt32.Subsystem); } }
    public UInt16 DLLCharacteristics
    { get { return (_is64 ? _opt64.DllCharacteristics : _opt32.DLLCharacteristics); } }
    public UInt64 SizeOfStackReserve
    { get { return (_is64 ? _opt64.SizeOfStackReserve : _opt32.SizeOfStackReserve); } }
    public UInt64 SizeOfStackCommit
    { get { return (_is64 ? _opt64.SizeOfStackCommit : _opt32.SizeOfStackCommit); } }
    public UInt64 SizeOfHeapReserve
    { get { return (_is64 ? _opt64.SizeOfHeapReserve : _opt32.SizeOfHeapReserve); } }
    public UInt64 SizeOfHeapCommit
    { get { return (_is64 ? _opt64.SizeOfHeapCommit : _opt32.SizeOfHeapCommit); } }
    public UInt32 LoaderFlags
    { get { return (_is64 ? _opt64.LoaderFlags : _opt32.LoaderFlags); } }
    public UInt32 NumberOfRvaAndSizes
    { get { return (_is64 ? _opt64.NumberOfRvaAndSizes : _opt32.NumberOfRvaAndSizes); } }

    public data_directory DataDirectory1_export
    { get { return (_is64 ? _opt64.DataDirectory1_export : _opt32.DataDirectory1_export); } }
    public data_directory DataDirectory2_import
    { get { return (_is64 ? _opt64.DataDirectory2_import : _opt32.DataDirectory2_import); } }
    public data_directory DataDirectory3
    { get { return (_is64 ? _opt64.DataDirectory3 : _opt32.DataDirectory3); } }
    public data_directory DataDirectory4
    { get { return (_is64 ? _opt64.DataDirectory4 : _opt32.DataDirectory4); } }
    public data_directory DataDirectory5
    { get { return (_is64 ? _opt64.DataDirectory5 : _opt32.DataDirectory5); } }
    public data_directory DataDirectory6
    { get { return (_is64 ? _opt64.DataDirectory6 : _opt32.DataDirectory6); } }
    public data_directory DataDirectory7
    { get { return (_is64 ? _opt64.DataDirectory7 : _opt32.DataDirectory7); } }
    public data_directory DataDirectory8
    { get { return (_is64 ? _opt64.DataDirectory8 : _opt32.DataDirectory8); } }
    public data_directory DataDirectory9
    { get { return (_is64 ? _opt64.DataDirectory9 : _opt32.DataDirectory9); } }
    public data_directory DataDirectory10
    { get { return (_is64 ? _opt64.DataDirectory10 : _opt32.DataDirectory10); } }
    public data_directory DataDirectory11
    { get { return (_is64 ? _opt64.DataDirectory11 : _opt32.DataDirectory11); } }
    public data_directory DataDirectory12
    { get { return (_is64 ? _opt64.DataDirectory12 : _opt32.DataDirectory12); } }
    public data_directory DataDirectory13
    { get { return (_is64 ? _opt64.DataDirectory13 : _opt32.DataDirectory13); } }
    public data_directory DataDirectory14
    { get { return (_is64 ? _opt64.DataDirectory14 : _opt32.DataDirectory14); } }
    public data_directory DataDirectory15
    { get { return (_is64 ? _opt64.DataDirectory15 : _opt32.DataDirectory15); } }
    public data_directory DataDirectory16
    { get { return (_is64 ? _opt64.DataDirectory16 : _opt32.DataDirectory16); } }

    public PEOptHeader(ref byte[] data, int index, int size)
    {
      if (size == Marshal.SizeOf(typeof(PEOptHeader64)))
      {
        // Parse 64 bit version
        _is64 = true;
        _opt64 = (PEOptHeader64)oMemoryFunctions.RawDataToObject(ref data, typeof(PEOptHeader64));
      }
      else if (size == Marshal.SizeOf(typeof(PEOptHeader32)))
      {
        // Parse 32 bit version
        _is64 = false;
        _opt32 = (PEOptHeader32)oMemoryFunctions.RawDataToObject(ref data, typeof(PEOptHeader32));
      }
      else
      {
        // ERROR
        throw new Exception("ERROR: Invalid pe optional header size of " + size.ToString() + ". Expected " + Marshal.SizeOf(typeof(PEOptHeader64)).ToString() + " or " + Marshal.SizeOf(typeof(PEOptHeader32)).ToString() + ".");
      }
    }
  }

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
      if (address >= start && address < start + size)
        return false;
      return true;
    }
  }

  public class HeaderReader
  {
    public uint codeLength;
    public uint codeStartAddress;
    public COFFHeader coffHeader;
    public IMAGE_DOS_HEADER dosHeader;
    public Hashtable exports;
    public List<IMPORT_FUNCTION> importTable;
    public PEOptHeader optHeader;
    public bool success;
    public List<ADDRESS_RANGE> invalidCodeAddresses;
    public List<section> sections;

    public UInt64 BaseAddress;
    public UInt64 Size;

    /// <summary>
    /// Check all invalid code ranges to see if this pointer is invalid for a code region.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public bool isValidCodePointer(uint address)
    {
      foreach (ADDRESS_RANGE range in invalidCodeAddresses)
        if (!range.isValid(address))
          return false;
      return true;
    }

    public static bool IsWin64(System.Diagnostics.Process process)
    {
      UInt64 address = (UInt64)process.MainModule.BaseAddress;

      // Read in the Image Dos Header
      byte[] headerData = oMemoryFunctions.ReadMemory(process, (ulong)address, (uint)Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)));
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
      }

      // Read in the PE token
      byte[] PE_Token = oMemoryFunctions.ReadMemory(process, (ulong)PE_header, 4);
      if (!(PE_Token[0] == 'P' & PE_Token[1] == 'E' & PE_Token[2] == 0 & PE_Token[3] == 0))
      {
        // Problem, we are not pointing at a correct PE header. Abort.
        throw new Exception("Failed to read PE header from block " + address.ToString("X") + " with PE header located at " + PE_header.ToString() + ".");
      }

      // Input the COFFHeader
      byte[] coffHeader_rawData = oMemoryFunctions.ReadMemory(process, (ulong)(PE_header + 4), (uint)Marshal.SizeOf(typeof(COFFHeader)));
      COFFHeader coffHeader = (COFFHeader)oMemoryFunctions.RawDataToObject(ref coffHeader_rawData, typeof(COFFHeader));

      return ((MACHINE)coffHeader.Machine) == MACHINE.IMAGE_FILE_MACHINE_IA64 || ((MACHINE)coffHeader.Machine) == MACHINE.IMAGE_FILE_MACHINE_AMD64;
    }

    /// <summary>
    /// This checks whether the specified heap is
    /// a valid pe header.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="address"></param>
    /// <returns></returns>
    public static bool isPeHeader(System.Diagnostics.Process process, UInt64 address)
    {
      // Read in the Image Dos Header
      byte[] headerData = oMemoryFunctions.ReadMemory(process, (ulong)address,
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
        byte[] PE_Token = oMemoryFunctions.ReadMemory(process, (ulong)PE_header, 4);
        if (!(PE_Token[0] == 'P' & PE_Token[1] == 'E' & PE_Token[2] == 0 & PE_Token[3] == 0))
        {
          // Problem, we are not pointing at a correct PE header. Abort.
          return false;
        }

        // Input the COFFHeader
        byte[] coffHeader_rawData = oMemoryFunctions.ReadMemory(process, (ulong)(PE_header + 4), (uint)Marshal.SizeOf(typeof(COFFHeader)));
        COFFHeader coffHeader = (COFFHeader)oMemoryFunctions.RawDataToObject(ref coffHeader_rawData, typeof(COFFHeader));

        // Read in the PEOptHeader if it exists
        if (coffHeader.SizeOfOptionalHeader != 0 && coffHeader.SizeOfOptionalHeader < 0x1000)
        {
          // Read in the optHeader
          byte[] optHeader_rawData = oMemoryFunctions.ReadMemory(process, (ulong)(PE_header + 4 + (uint)Marshal.SizeOf(typeof(COFFHeader))), coffHeader.SizeOfOptionalHeader);
          PEOptHeader optHeader = new PEOptHeader(ref optHeader_rawData, 0, optHeader_rawData.Length);

          // Confirm that it loaded correctly
          if ((optHeader.signature & 0xffff) != 0x10b && (optHeader.signature & 0xffff) != 0x20b)
          {
            return false;
          }

          if (optHeader.SizeOfCode == 0)
            return false;
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
    public HeaderReader(System.Diagnostics.Process process, UInt64 address)
    {
      this.BaseAddress = address;
      codeStartAddress = 0;
      codeLength = 0;
      invalidCodeAddresses = new List<ADDRESS_RANGE>(10);
      sections = new List<section>(10);
      exports = new Hashtable(10);

      // Read in the Image Dos Header
      importTable = new List<IMPORT_FUNCTION>(0);
      byte[] headerData = oMemoryFunctions.ReadMemory(process, (ulong)address, (uint)Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)));
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
      byte[] PE_Token = oMemoryFunctions.ReadMemory(process, (ulong)PE_header, 4);
      if (!(PE_Token[0] == 'P' & PE_Token[1] == 'E' & PE_Token[2] == 0 & PE_Token[3] == 0))
      {
        // Problem, we are not pointing at a correct PE header. Abort.
        Console.WriteLine("Failed to read PE header from block " + address.ToString("X") + " with PE header located at " + PE_header.ToString() + ".");
        return;
      }

      // Input the COFFHeader
      byte[] coffHeader_rawData = oMemoryFunctions.ReadMemory(process, (ulong)(PE_header + 4), (uint)Marshal.SizeOf(typeof(COFFHeader)));
      coffHeader = (COFFHeader)oMemoryFunctions.RawDataToObject(ref coffHeader_rawData, typeof(COFFHeader));

      // Read in the PEOptHeader if it exists
      if (coffHeader.SizeOfOptionalHeader != 0 && coffHeader.SizeOfOptionalHeader < 0x1000)
      {
        // Read in the optHeader
        byte[] optHeader_rawData = oMemoryFunctions.ReadMemory(process, (ulong)(PE_header + 4 + (uint)Marshal.SizeOf(typeof(COFFHeader))), coffHeader.SizeOfOptionalHeader);
        optHeader = new PEOptHeader(ref optHeader_rawData, 0, coffHeader.SizeOfOptionalHeader);

        // Confirm that it loaded correctly
        if ((optHeader.signature & 0xffff) != 0x10b && (optHeader.signature & 0xffff) != 0x20b)
        {
          Console.WriteLine("Failed to read optHeader; Expected signature does not match read in signature of " + optHeader.signature.ToString() + ".");
          return;
        }

        if (optHeader._is64)
          return; // Do not process 64bit headers

        // Load the sections
        UInt64 sectionAddress = PE_header + 4 + (UInt64)Marshal.SizeOf(typeof(COFFHeader)) + coffHeader.SizeOfOptionalHeader;
        sections = new List<section>(coffHeader.NumberOfSections);
        for (int i = 0; i < coffHeader.NumberOfSections; i++)
        {
          sections.Add(new section(process, sectionAddress));
          sectionAddress += (uint)Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER));
        }

        // Add all the directories as invalid code ranges
        try
        {
          if (optHeader.DataDirectory1_export.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory1_export.VirtualAddress),
                    optHeader.DataDirectory1_export.Size));
          if (optHeader.DataDirectory2_import.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory2_import.VirtualAddress),
                    optHeader.DataDirectory2_import.Size));
          if (optHeader.DataDirectory3.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory3.VirtualAddress),
                    optHeader.DataDirectory3.Size));
          if (optHeader.DataDirectory4.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory4.VirtualAddress),
                    optHeader.DataDirectory4.Size));
          if (optHeader.DataDirectory5.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory5.VirtualAddress),
                    optHeader.DataDirectory5.Size));
          if (optHeader.DataDirectory6.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory6.VirtualAddress),
                    optHeader.DataDirectory6.Size));
          if (optHeader.DataDirectory7.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory7.VirtualAddress),
                    optHeader.DataDirectory7.Size));
          if (optHeader.DataDirectory8.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory8.VirtualAddress),
                    optHeader.DataDirectory8.Size));
          if (optHeader.DataDirectory9.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory9.VirtualAddress),
                    optHeader.DataDirectory9.Size));
          if (optHeader.DataDirectory10.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory10.VirtualAddress),
                    optHeader.DataDirectory10.Size));
          if (optHeader.DataDirectory11.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory11.VirtualAddress),
                    optHeader.DataDirectory11.Size));
          if (optHeader.DataDirectory12.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory12.VirtualAddress),
                    optHeader.DataDirectory12.Size));
          if (optHeader.DataDirectory13.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory13.VirtualAddress),
                    optHeader.DataDirectory13.Size));
          if (optHeader.DataDirectory14.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory14.VirtualAddress),
                    optHeader.DataDirectory14.Size));
          if (optHeader.DataDirectory15.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory15.VirtualAddress),
                    optHeader.DataDirectory15.Size));
          if (optHeader.DataDirectory16.Size > 0)
            invalidCodeAddresses.Add(
                new ADDRESS_RANGE(
                    oMemoryFunctions.AddressAdd(address, optHeader.DataDirectory16.VirtualAddress),
                    optHeader.DataDirectory16.Size));
        }
        catch
        {
          // Ignore exceptions, this is not cretical.
        }

        // Extract the names of the functions and corresponding table entries
        Int64 importTableAddress = (Int64)optHeader.DataDirectory2_import.VirtualAddress + (Int64)address;
        loadImportTable(process, (uint)importTableAddress, optHeader.DataDirectory2_import.Size, (uint)address);

        // Extract the names of the functions and corresponding table entries
        Int64 exportTableAddress = (Int64)optHeader.DataDirectory1_export.VirtualAddress + (Int64)address;

        // Read in the first _IMAGE_EXPORT_DIRECTORY structure
        byte[] export_directory_rawData = oMemoryFunctions.ReadMemory(process, (ulong)exportTableAddress, (uint)Marshal.SizeOf(typeof(_IMAGE_EXPORT_DIRECTORY)));
        _IMAGE_EXPORT_DIRECTORY export_directory = (_IMAGE_EXPORT_DIRECTORY)oMemoryFunctions.RawDataToObject(ref export_directory_rawData, typeof(_IMAGE_EXPORT_DIRECTORY));
        exports = new Hashtable(20);

        UInt64 functionNameArray = (UInt64)export_directory.AddressOfNames + address;
        string exportName = oMemoryFunctions.ReadString(process, (UInt64)export_directory.Name + address, oMemoryFunctions.STRING_TYPE.ascii);
        UInt64 functionOrdinalArray = (UInt64)export_directory.AddressOfNameOrdinal + address;
        UInt64 functionAddressArray = (UInt64)export_directory.AddressOfFunctions + address;
        for (int i = 0; i < export_directory.NumberOfNames; i++)
        {
          int ordinal_relative = (int)oMemoryFunctions.ReadMemoryUShort(process, functionOrdinalArray + (UInt64)i * 2);
          int ordinal = ordinal_relative + (int)export_directory.Base;

          if (ordinal_relative < export_directory.NumberOfFunctions)
          {
            // Continue with importing this function
            string name = "";
            if (i < export_directory.NumberOfNames)
              name = oMemoryFunctions.ReadString(process, (UInt64)oMemoryFunctions.ReadMemoryDword(process, functionNameArray + (UInt64)i * 4) + address, oMemoryFunctions.STRING_TYPE.ascii);
            else
              name = "oridinal" + ordinal.ToString();



            // Lookup the function rva now
            try
            {
              UInt64 offset = (UInt64)oMemoryFunctions.ReadMemoryDword(process, functionAddressArray + (UInt64)ordinal_relative * 4);

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

                      if (!exports.ContainsKey(new_export.Address))
                      {
                        exports.Add(new_export.Address, new_export);
                      }
                      break;
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
    }

    private void loadImportTable(Process process, uint importTableAddress, uint length, uint baseAddress)
    {
      // Read in the first import descriptor structure
      byte[] image_import_descriptor_rawData = oMemoryFunctions.ReadMemory(process, importTableAddress, (uint)Marshal.SizeOf(typeof(image_import_descriptor)));
      image_import_descriptor descriptor = (image_import_descriptor)oMemoryFunctions.RawDataToObject(ref image_import_descriptor_rawData, typeof(image_import_descriptor));
      List<image_import_by_name> imports = new List<image_import_by_name>(0);
      while (descriptor.FirstThunk != 0 || descriptor.OriginalFirstThunk != 0 || descriptor.Name != 0)
      {
        // Process this descriptor
        imports.Add(new image_import_by_name(process, descriptor, (ulong)baseAddress));

        // Read next descriptor
        image_import_descriptor_rawData = oMemoryFunctions.ReadMemory(process, oMemoryFunctions.AddressAdd(importTableAddress, (ulong)(imports.Count * Marshal.SizeOf(typeof(image_import_descriptor)))), (uint)Marshal.SizeOf(typeof(image_import_descriptor)));
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
            newImport.memoryAddress = (uint)dllDescriptor.ImportTableAddresses[i];
            newImport.targetAddress = oMemoryFunctions.ReadMemoryDword(process,
                                                                       (ulong)newImport.memoryAddress);
            importTable.Add(newImport);
          }
          catch
          {
            // Not a critical error
          }
        }
      }
    }


    public static bool isPeHeader(System.Diagnostics.Process process, ulong p, MEMORY_PROTECT memoryProtection)
    {
      // We don't want to set off a page guard exception!
      if (memoryProtection.ToString().ToUpper().Contains("GUARD"))
        return false;
      else
        return isPeHeader(process, p);
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

  public struct IMPORT_FUNCTION
  {
    public UInt64 memoryAddress;
    public UInt64 targetAddress;
    public string name;
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

  public enum MACHINE
  {
    IMAGE_FILE_MACHINE_I386 = 0x014c,
    IMAGE_FILE_MACHINE_AMD64 = 0x8664,
    IMAGE_FILE_MACHINE_IA64 = 0x0200,
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct PEOptHeader32
  {
    public UInt16 signature; //decimal number 267.
    public byte MajorLinkerVersion;
    public byte MinorLinkerVersion;
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

  [StructLayout(LayoutKind.Explicit)]
  public struct PEOptHeader64
  {
    [FieldOffset(0)]
    public UInt32 Magic;

    [FieldOffset(2)]
    public byte MajorLinkerVersion;

    [FieldOffset(3)]
    public byte MinorLinkerVersion;

    [FieldOffset(4)]
    public UInt32 SizeOfCode;

    [FieldOffset(8)]
    public UInt32 SizeOfInitializedData;

    [FieldOffset(12)]
    public UInt32 SizeOfUninitializedData;

    [FieldOffset(16)]
    public UInt32 AddressOfEntryPoint;

    [FieldOffset(20)]
    public UInt32 BaseOfCode;

    [FieldOffset(24)]
    public UInt64 ImageBase;

    [FieldOffset(32)]
    public UInt32 SectionAlignment;

    [FieldOffset(36)]
    public UInt32 FileAlignment;

    [FieldOffset(40)]
    public UInt16 MajorOperatingSystemVersion;

    [FieldOffset(42)]
    public UInt16 MinorOperatingSystemVersion;

    [FieldOffset(44)]
    public UInt16 MajorImageVersion;

    [FieldOffset(46)]
    public UInt16 MinorImageVersion;

    [FieldOffset(48)]
    public UInt16 MajorSubsystemVersion;

    [FieldOffset(50)]
    public UInt16 MinorSubsystemVersion;

    [FieldOffset(52)]
    public UInt32 Win32VersionValue;

    [FieldOffset(56)]
    public UInt32 SizeOfImage;

    [FieldOffset(60)]
    public UInt32 SizeOfHeaders;

    [FieldOffset(64)]
    public UInt32 CheckSum;

    [FieldOffset(68)]
    public UInt16 Subsystem;

    [FieldOffset(70)]
    public UInt16 DllCharacteristics;

    [FieldOffset(72)]
    public UInt64 SizeOfStackReserve;

    [FieldOffset(80)]
    public UInt64 SizeOfStackCommit;

    [FieldOffset(88)]
    public UInt64 SizeOfHeapReserve;

    [FieldOffset(96)]
    public UInt64 SizeOfHeapCommit;

    [FieldOffset(104)]
    public UInt32 LoaderFlags;

    [FieldOffset(108)]
    public UInt32 NumberOfRvaAndSizes;

    [FieldOffset(112)]
    public data_directory DataDirectory1_export;

    [FieldOffset(120)]
    public data_directory DataDirectory2_import;

    [FieldOffset(128)]
    public data_directory DataDirectory3;

    [FieldOffset(136)]
    public data_directory DataDirectory4;

    [FieldOffset(144)]
    public data_directory DataDirectory5;

    [FieldOffset(152)]
    public data_directory DataDirectory6;

    [FieldOffset(160)]
    public data_directory DataDirectory7;

    [FieldOffset(168)]
    public data_directory DataDirectory8;

    [FieldOffset(176)]
    public data_directory DataDirectory9;

    [FieldOffset(184)]
    public data_directory DataDirectory10;

    [FieldOffset(192)]
    public data_directory DataDirectory11;

    [FieldOffset(200)]
    public data_directory DataDirectory12;

    [FieldOffset(208)]
    public data_directory DataDirectory13;

    [FieldOffset(216)]
    public data_directory DataDirectory14;

    [FieldOffset(224)]
    public data_directory DataDirectory15;

    [FieldOffset(232)]
    public data_directory DataDirectory16;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct data_directory
  {
    public UInt32 VirtualAddress;
    public UInt32 Size;
  }

  public struct image_import_descriptor
  {
    public int OriginalFirstThunk;
    public int TimeDateStamp;
    public int ForwarderChain;
    public int Name;
    public int FirstThunk;
  }

  public struct _IMAGE_EXPORT_DIRECTORY
  {
    public UInt32 Characteristics;
    public UInt32 TimeDateStamp;
    public UInt16 MajorVersion;
    public UInt16 MinorVersion;
    public UInt32 Name;
    public UInt32 Base;
    public UInt32 NumberOfFunctions;
    public UInt32 NumberOfNames;
    public UInt32 AddressOfFunctions;
    public UInt32 AddressOfNames;
    public UInt32 AddressOfNameOrdinal;
  }

  [StructLayout(LayoutKind.Explicit)]
  public struct IMAGE_SECTION_HEADER
  {
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public char[] Name;

    [FieldOffset(8)]
    public UInt32 VirtualSize;

    [FieldOffset(12)]
    public UInt32 VirtualAddress;

    [FieldOffset(16)]
    public UInt32 SizeOfRawData;

    [FieldOffset(20)]
    public UInt32 PointerToRawData;

    [FieldOffset(24)]
    public UInt32 PointerToRelocations;

    [FieldOffset(28)]
    public UInt32 PointerToLinenumbers;

    [FieldOffset(32)]
    public UInt16 NumberOfRelocations;

    [FieldOffset(34)]
    public UInt16 NumberOfLinenumbers;

    [FieldOffset(36)]
    public DataSectionFlags Characteristics;

    public string Section
    {
      get { return new string(Name); }
    }
  }

  [Flags]
  public enum DataSectionFlags : uint
  {
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    TypeReg = 0x00000000,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    TypeDsect = 0x00000001,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    TypeNoLoad = 0x00000002,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    TypeGroup = 0x00000004,
    /// <summary>
    /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
    /// </summary>
    TypeNoPadded = 0x00000008,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    TypeCopy = 0x00000010,
    /// <summary>
    /// The section contains executable code.
    /// </summary>
    ContentCode = 0x00000020,
    /// <summary>
    /// The section contains initialized data.
    /// </summary>
    ContentInitializedData = 0x00000040,
    /// <summary>
    /// The section contains uninitialized data.
    /// </summary>
    ContentUninitializedData = 0x00000080,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    LinkOther = 0x00000100,
    /// <summary>
    /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
    /// </summary>
    LinkInfo = 0x00000200,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    TypeOver = 0x00000400,
    /// <summary>
    /// The section will not become part of the image. This is valid only for object files.
    /// </summary>
    LinkRemove = 0x00000800,
    /// <summary>
    /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
    /// </summary>
    LinkComDat = 0x00001000,
    /// <summary>
    /// Reset speculative exceptions handling bits in the TLB entries for this section.
    /// </summary>
    NoDeferSpecExceptions = 0x00004000,
    /// <summary>
    /// The section contains data referenced through the global pointer (GP).
    /// </summary>
    RelativeGP = 0x00008000,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    MemPurgeable = 0x00020000,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    Memory16Bit = 0x00020000,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    MemoryLocked = 0x00040000,
    /// <summary>
    /// Reserved for future use.
    /// </summary>
    MemoryPreload = 0x00080000,
    /// <summary>
    /// Align data on a 1-byte boundary. Valid only for object files.
    /// </summary>
    Align1Bytes = 0x00100000,
    /// <summary>
    /// Align data on a 2-byte boundary. Valid only for object files.
    /// </summary>
    Align2Bytes = 0x00200000,
    /// <summary>
    /// Align data on a 4-byte boundary. Valid only for object files.
    /// </summary>
    Align4Bytes = 0x00300000,
    /// <summary>
    /// Align data on an 8-byte boundary. Valid only for object files.
    /// </summary>
    Align8Bytes = 0x00400000,
    /// <summary>
    /// Align data on a 16-byte boundary. Valid only for object files.
    /// </summary>
    Align16Bytes = 0x00500000,
    /// <summary>
    /// Align data on a 32-byte boundary. Valid only for object files.
    /// </summary>
    Align32Bytes = 0x00600000,
    /// <summary>
    /// Align data on a 64-byte boundary. Valid only for object files.
    /// </summary>
    Align64Bytes = 0x00700000,
    /// <summary>
    /// Align data on a 128-byte boundary. Valid only for object files.
    /// </summary>
    Align128Bytes = 0x00800000,
    /// <summary>
    /// Align data on a 256-byte boundary. Valid only for object files.
    /// </summary>
    Align256Bytes = 0x00900000,
    /// <summary>
    /// Align data on a 512-byte boundary. Valid only for object files.
    /// </summary>
    Align512Bytes = 0x00A00000,
    /// <summary>
    /// Align data on a 1024-byte boundary. Valid only for object files.
    /// </summary>
    Align1024Bytes = 0x00B00000,
    /// <summary>
    /// Align data on a 2048-byte boundary. Valid only for object files.
    /// </summary>
    Align2048Bytes = 0x00C00000,
    /// <summary>
    /// Align data on a 4096-byte boundary. Valid only for object files.
    /// </summary>
    Align4096Bytes = 0x00D00000,
    /// <summary>
    /// Align data on an 8192-byte boundary. Valid only for object files.
    /// </summary>
    Align8192Bytes = 0x00E00000,
    /// <summary>
    /// The section contains extended relocations.
    /// </summary>
    LinkExtendedRelocationOverflow = 0x01000000,
    /// <summary>
    /// The section can be discarded as needed.
    /// </summary>
    MemoryDiscardable = 0x02000000,
    /// <summary>
    /// The section cannot be cached.
    /// </summary>
    MemoryNotCached = 0x04000000,
    /// <summary>
    /// The section is not pageable.
    /// </summary>
    MemoryNotPaged = 0x08000000,
    /// <summary>
    /// The section can be shared in memory.
    /// </summary>
    MemoryShared = 0x10000000,
    /// <summary>
    /// The section can be executed as code.
    /// </summary>
    MemoryExecute = 0x20000000,
    /// <summary>
    /// The section can be read.
    /// </summary>
    MemoryRead = 0x40000000,
    /// <summary>
    /// The section can be written to.
    /// </summary>
    MemoryWrite = 0x80000000
  }

  public class export
  {
    public UInt64 Address;
    public string Name;
    public int Ordinal;

    public export(UInt64 address, string name, int ordinal)
    {
      this.Address = address;
      this.Ordinal = ordinal;
      if (name.Trim() == "")
        this.Name = "Ordinal " + ordinal;
      else
        this.Name = name;
    }
  }

  public class section
  {
    public IMAGE_SECTION_HEADER SectionHeader;

    public section(System.Diagnostics.Process process, UInt64 address)
    {
      // Parse the section struct
      byte[] headerData = oMemoryFunctions.ReadMemory(process, (ulong)address, (uint)Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER)));
      SectionHeader = (IMAGE_SECTION_HEADER)oMemoryFunctions.RawDataToObject(ref headerData, typeof(IMAGE_SECTION_HEADER));
    }

    public string Name
    {
      get { return SectionHeader.Section; }
    }

    public DataSectionFlags Characteristics
    {
      get { return SectionHeader.Characteristics; }
    }

    public bool Contains(UInt64 rva)
    {
      return rva >= SectionHeader.VirtualAddress && rva - SectionHeader.VirtualAddress < SectionHeader.VirtualSize;
    }

    public bool IsExecutable()
    {
      return (((uint)SectionHeader.Characteristics) & (uint)DataSectionFlags.MemoryExecute) > 0;
    }
  }

  public class image_import_by_name
  {
    public List<uint> AddressOfData;
    public List<string> Names;
    public List<UInt64> ImportTableAddresses;

    public image_import_by_name(System.Diagnostics.Process process, image_import_descriptor descriptor, UInt64 baseAddress)
    {
      AddressOfData = new List<uint>(0);
      ImportTableAddresses = new List<UInt64>(0);
      Names = new List<string>(0);

      // Read the AddressOfData array in
      if (descriptor.OriginalFirstThunk != 0 && descriptor.FirstThunk != 0)
      {
        // Read in the descriptor table with names
        int i = 0;
        uint newAddress = oMemoryFunctions.ReadMemoryDword(process,
                                                           (ulong)descriptor.OriginalFirstThunk +
                                                           (ulong)baseAddress + (ulong)i * 4);
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
                      oMemoryFunctions.ReadMemoryUShort(process, (UInt64)newAddress + baseAddress).
                          ToString("X"));
          }
          else if (newAddress > 0)
          {
            // Export by name
            Names.Add(oMemoryFunctions.ReadMemoryString(process, (UInt64)newAddress + baseAddress + 2, 256));
          }
          else
          {
            // There is no name or ordinal given, probably obfuscated?
            Names.Add("obfuscated?");
          }

          // Add the pe table address corresponding to this function
          ImportTableAddresses.Add((UInt64)descriptor.FirstThunk + (UInt64)baseAddress + (UInt64)i * 4);

          // Read in the next value
          i++;
          newAddress = oMemoryFunctions.ReadMemoryDword(process,
                                                        (ulong)descriptor.OriginalFirstThunk +
                                                        (ulong)baseAddress + (ulong)i * 4);
        }
      }
      else
      {
        // Read in the descriptor table without names
        int i = 0;
        ulong addressThunk = (ulong)(descriptor.OriginalFirstThunk != 0
                                  ? descriptor.OriginalFirstThunk
                                  : descriptor.FirstThunk);
        if (addressThunk == 0)
          return; // We have no imports that we can read in.

        uint newAddress = oMemoryFunctions.ReadMemoryDword(process,
                                                           addressThunk +
                                                           (ulong)baseAddress + (ulong)i * 4);
        while (newAddress != 0)
        {
          // Add this new address
          AddressOfData.Add(newAddress & 0x7fffffff);
          Names.Add("");
          ImportTableAddresses.Add((UInt64)addressThunk + (UInt64)baseAddress + (UInt64)i * 4);


          // Read in the next value
          i++;
          newAddress = oMemoryFunctions.ReadMemoryDword(process,
                                                         addressThunk +
                                                         (ulong)baseAddress + (ulong)i * 4);
        }
      }
    }
  }
}