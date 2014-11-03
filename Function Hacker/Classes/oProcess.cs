using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Classes.Disassembly;
using FunctionHacker.Classes.Tabs;
using FunctionHacker.Forms;

namespace FunctionHacker.Classes
{
  internal static class oProcess
  {
    public static List<ProcessModule> activeModules = new List<ProcessModule>(10);
    public static DISASSEMBLY_MODE disassemblyMode;
    public static Process activeProcess;
    public static List<HEAP_INFO> map;


    public static void generateMemoryMap()
    {
      map = oMemoryFunctions.GenerateMemoryMap(activeProcess);
    }

    /// <summary>
    /// This disassmebles all the modules
    /// </summary>
    public static void disassembleProcess(Form parent, List<HEAP_INFO> selectedHeaps)
    {
      // Create the progress bar
      formProgress progress = new formProgress(parent);

      progress.Show(parent);
      progress.setMin(0);
      progress.setMax(selectedHeaps.Count);
      progress.setTitle("Disassembling Modules...");
      progress.setLabel1("Generating Memory Map");
      progress.setLabel2("Total Discovered Internal Calls: 0");

      // Reset the function master
      oFunctionMaster.reset();

      // Create the return instruction list, for parameter count estimation usage later on
      oAsmRetList returnAddresses = new oAsmRetList();
      oEbpArgumentList ebpAddresses = new oEbpArgumentList();

      // Process the selected heaps
      ulong count = 0;
      HeaderReader headerReader = null;
      for (int i = 0; i < selectedHeaps.Count; i++)
      {
        HEAP_INFO heap = selectedHeaps[i];

        // Check if it is a PE Header
        if (heap.extra.Contains("PE"))
        {
          // Process the PE import table
          headerReader = new HeaderReader(activeProcess, heap.heapAddress);


          // Set the access rights to this heap so that we can edit the header
          if (headerReader.importTable.Count > 0)
          {
            HEAP_INFO tmpHeapInfo = oMemoryFunctions.LookupAddressInMap(map, headerReader.optHeader.DataDirectory13.VirtualAddress + (uint)heap.heapAddress);

            if (tmpHeapInfo.heapProtection.Contains("EXECUTE"))
            {
              oMemoryFunctions.SetMemoryProtection(activeProcess,
                                               (uint)tmpHeapInfo.heapAddress,
                                               (uint)tmpHeapInfo.heapLength,
                                               MEMORY_PROTECT.PAGE_EXECUTE_READWRITE);
            }
            else
            {
              oMemoryFunctions.SetMemoryProtection(activeProcess,
                                               (uint)tmpHeapInfo.heapAddress,
                                               (uint)tmpHeapInfo.heapLength,
                                               MEMORY_PROTECT.PAGE_READWRITE);
            }

          }


          // Add all the imported functions 
          foreach (IMPORT_FUNCTION importFunction in headerReader.importTable)
          {
            // Lookup the destination address
            HEAP_INFO destinationHeap = oMemoryFunctions.LookupAddressInMap(map,
                                                                            (uint)importFunction.targetAddress);

            // Add this function if it is valid
            if (destinationHeap.heapProtection.Contains("EXECUTE") && destinationHeap.heapLength > 0)
            {
              // Valid destination
              oFunctionMaster.addCall((uint)importFunction.memoryAddress, (uint)importFunction.targetAddress,
                                      oFunction.CALL_TYPE.PE_TABLE, importFunction.name);

              // Disasemble only one function and find return (needed for parametar count estimation)
              uint length =
                  (uint)
                  (destinationHeap.heapLength -
                   (importFunction.targetAddress - destinationHeap.heapAddress));
              if (length > 0x1000)
                length = 0x1000;
              byte[] block = oMemoryFunctions.ReadMemory(activeProcess, importFunction.targetAddress,
                                                         length);

              // Process the function only
              LengthDecoder.AddFirstRet(block, (int)importFunction.targetAddress, 0, ref returnAddresses);
            }
          }

          // Add all the exported functions
          if (headerReader.exports.Count > 0)
          {
            // Read in this heap
            HEAP_INFO tmpHeap;
            byte[] block = null;
            IEnumerator exportEnum = headerReader.exports.Values.GetEnumerator();
            if (exportEnum.MoveNext())
            {
              tmpHeap = oMemoryFunctions.LookupAddressInMap(map, (uint)((export)exportEnum.Current).Address);
              block = oMemoryFunctions.ReadMemory(activeProcess, tmpHeap.heapAddress, (uint)tmpHeap.heapLength);

              // We need to add all the exported functions from this library
              foreach (export exportFunction in headerReader.exports.Values)
              {
                if ((exportFunction.Address >= tmpHeap.heapAddress) && (exportFunction.Address <= tmpHeap.heapAddress + tmpHeap.heapLength))
                {
                  // Add this call
                  oFunctionMaster.addCall(0, (uint)exportFunction.Address,
                                          oFunction.CALL_TYPE.PE_EXPORT, exportFunction.Name);

                  // Disasemble only one function and find return (needed for parameter count estimation)

                  // Process the function only
                  LengthDecoder.AddFirstRet(block, (int)tmpHeap.heapAddress,
                                            (int)(exportFunction.Address - tmpHeap.heapAddress),
                                            ref returnAddresses);
                }
              }
            }
          }
        }

        // If this contains EXECUTE privileges, instrument this heap even if it is a PE header.
        // Instrument this heap if it has been selected, but not if it is a PE header region without EXECUTE privileges.
        if (!heap.extra.Contains("PE") || heap.heapProtection.Contains("EXECUTE"))
        {
          // Disassemble this heap as a code region
          List<jmpInstruction> jmpAddresses = new List<jmpInstruction>(10000);

          // Let's process this data region
          if (heap.associatedModule != null)
          {
            progress.setLabel1("Analyzing code region for module " +
                               heap.associatedModule.ModuleName + " heap at address 0x" +
                               heap.heapAddress.ToString("X"));
          }
          else
          {
            progress.setLabel1(
                "Analyzing code region associated with an unknown module at address 0x" +
                heap.heapAddress.ToString("X"));
          }

          // Read in the heap
          byte[] block = oMemoryFunctions.ReadMemory(activeProcess, heap.heapAddress, (uint)heap.heapLength);

          // Process the heap
          List<SIMPLE_INSTRUCTION> instructions = LengthDecoder.DisassembleBlockCallsOnly(block,
                                                                                          (int)heap.heapAddress,
                                                                                          ref returnAddresses,
                                                                                          ref ebpAddresses,
                                                                                          ref jmpAddresses);

          // Get a list of the return instruction addresses for this heap
          List<retAddress> retAddresses = returnAddresses.getRets((uint)heap.heapAddress, (uint)(heap.heapAddress + heap.heapLength));

          // Generate a list of call source-destination pairs.
          List<SOURCE_DEST_PAIR> callPairs = new List<SOURCE_DEST_PAIR>(1000);
          for (int n = 0; n < instructions.Count; n++)
          {
            HEAP_INFO destinationHeap = oMemoryFunctions.LookupAddressInMap(map,
                                                                            (uint)
                                                                            instructions[n].jmp_target);


            if (destinationHeap.heapLength > 0 &&
                destinationHeap.heapAddress == heap.heapAddress &&
                isValidCodePointer(activeProcess, instructions[n].address, (uint)instructions[n].jmp_target, headerReader) &&
                isValidCodeBlock(activeProcess, heap, instructions[n].address, jmpAddresses, retAddresses, 500) &&
                isValidCodeBlock(activeProcess, heap, instructions[n].jmp_target, jmpAddresses, retAddresses, 500))
            {
              // Add this call, it may be filtered out later.
              callPairs.Add(new SOURCE_DEST_PAIR((uint)instructions[n].address,
                                                 (uint)instructions[n].jmp_target,
                                                 oFunction.CALL_TYPE.FIXED_OFFSET,
                                                 null,
                                                 ""));
            }
          }


          // -----------------------------------
          // PROPER CALLBACK TABLE CONDITION
          // -----------------------------------
          // The start of the callback table is defined as:
          // 1. 4 dwords in a row that point to within this heap.
          // 2. Must point to at least 2 different addresses.
          // 3. All 4 pointers must point to valid code regions.
          //
          // The end of the callback table is defined as:
          // 1. first not heap-internal pointer
          // 2. or first heap-internal pointer that fails the
          //    valid function test.
          //
          bool inCallbackTable = false;
          for (int n = 0; n < block.Length - 4 * 4; n += 4)
          {
            // Check if this could be the start of a callback table.
            uint value1 = oMemoryFunctions.ByteArrayToUint(block, n);
            if (inCallbackTable)
            {
              // Check is this value should be added onto the callback table
              if (value1 >= heap.heapAddress && value1 <= heap.heapAddress + heap.heapLength &&
                  isValidCodePointer(activeProcess, ((uint)n) + (uint)heap.heapAddress, value1, headerReader) &&
                  isValidCodeBlock(activeProcess, heap, value1, jmpAddresses, retAddresses, 500))
              {
                // Another valid callback entry
                callPairs.Add(new SOURCE_DEST_PAIR(((uint)n) + (uint)heap.heapAddress,
                               value1,
                               oFunction.CALL_TYPE.CALLBACK_TABLE,
                               callPairs[callPairs.Count - 1],
                               "vtable"));
              }
              else
              {
                // End of callback table.
                inCallbackTable = false;
              }


            }
            else if (value1 >= heap.heapAddress && value1 <= heap.heapAddress + heap.heapLength)
            {
              // Check second pointer
              uint value2 = oMemoryFunctions.ByteArrayToUint(block, n + 4);
              if (value2 >= heap.heapAddress && value2 <= heap.heapAddress + heap.heapLength)
              {
                // Check third pointer
                uint value3 = oMemoryFunctions.ByteArrayToUint(block, n + 4 * 2);
                if (value3 >= heap.heapAddress && value3 <= heap.heapAddress + heap.heapLength)
                {
                  // Check fourth pointer
                  uint value4 = oMemoryFunctions.ByteArrayToUint(block, n + 4 * 3);
                  if (value4 >= heap.heapAddress && value4 <= heap.heapAddress + heap.heapLength)
                  {
                    // We found four internal pointers in a row. Check that condition #2 is met.
                    if (!((value1 == value2 && value1 == value3) ||
                        (value1 == value2 && value1 == value4) ||
                        (value2 == value3 && value2 == value4)))
                    {
                      // At least two of the values are different

                      // Check condition #3, all valid code poitners
                      if (isValidCodePointer(activeProcess, ((uint)n) + (uint)heap.heapAddress, value1, headerReader) &&
                          isValidCodeBlock(activeProcess, heap, value1, jmpAddresses, retAddresses, 500) &&
                          isValidCodePointer(activeProcess, ((uint)n + 4 * 1) + (uint)heap.heapAddress, value2, headerReader) &&
                          isValidCodeBlock(activeProcess, heap, value2, jmpAddresses, retAddresses, 500) &&
                          isValidCodePointer(activeProcess, ((uint)n + 4 * 2) + (uint)heap.heapAddress, value3, headerReader) &&
                          isValidCodeBlock(activeProcess, heap, value3, jmpAddresses, retAddresses, 500) &&
                          isValidCodePointer(activeProcess, ((uint)n + 4 * 3) + (uint)heap.heapAddress, value4, headerReader) &&
                          isValidCodeBlock(activeProcess, heap, value4, jmpAddresses, retAddresses, 500))
                      {
                        // All pointers are valid, start of callback table
                        inCallbackTable = true;

                        // Add these function calls
                        callPairs.Add(new SOURCE_DEST_PAIR(((uint)n) + (uint)heap.heapAddress,
                                       value1,
                                       oFunction.CALL_TYPE.CALLBACK_TABLE,
                                       null,
                                       "vtable"));
                        callPairs.Add(new SOURCE_DEST_PAIR(((uint)n + 4 * 1) + (uint)heap.heapAddress,
                                       value2,
                                       oFunction.CALL_TYPE.CALLBACK_TABLE,
                                       callPairs[callPairs.Count - 1],
                                       "vtable"));
                        callPairs.Add(new SOURCE_DEST_PAIR(((uint)n + 4 * 2) + (uint)heap.heapAddress,
                                       value3,
                                       oFunction.CALL_TYPE.CALLBACK_TABLE,
                                       callPairs[callPairs.Count - 1],
                                       "vtable"));
                        callPairs.Add(new SOURCE_DEST_PAIR(((uint)n + 4 * 3) + (uint)heap.heapAddress,
                                       value4,
                                       oFunction.CALL_TYPE.CALLBACK_TABLE,
                                       callPairs[callPairs.Count - 1],
                                       "vtable"));

                        // Move on to the next callback
                        n += 4 * 3; // because n+=4
                      }
                    }
                  }
                }
              }
            }
          }



          // -----------------------------------
          // PROPER SUBROUTINE ENTER FILTER CONDITION:
          // -----------------------------------
          // Condition Description:
          // - each subroutine is only called from the start of the subroutine, and no call
          //   can enter the subroutine in the middle of the function. I have yet to see a
          //   function that violates rule even under obfuscation (except rarely the skipping
          //   of hotpatching bytes).
          // 
          // Condition Implementation:
          // - only smallest call destination in range (retAddress[i-1], retAddress[i]] is valid
          //   for funcion i.
          // - a destination of min destionation + const, where  0 <= const <= 2 is allowed to support
          //   some obfuscation methods that skip the hotpatching useless bytes.
          //

          // Sort the array by the destination
          SOURCE_DEST_PAIR.COMPARER_DEST comparerDest = new SOURCE_DEST_PAIR.COMPARER_DEST();
          callPairs.Sort(comparerDest);

          // Loop through each return instruction
          for (int n = 0; n < retAddresses.Count; n++)
          {
            // Find all destination addresses associated with this function

            // Find the start destination pair index
            int indexStart = (n > 0 ? callPairs.BinarySearch(new SOURCE_DEST_PAIR(0, retAddresses[n - 1].address, oFunction.CALL_TYPE.FIXED_OFFSET, null, ""),
                                        comparerDest) : 0);
            if (indexStart < 0)
              indexStart = ~indexStart;

            // Find the end destination pair index
            int indexEnd = callPairs.BinarySearch(new SOURCE_DEST_PAIR(0, retAddresses[n].address, oFunction.CALL_TYPE.FIXED_OFFSET, null, ""),
                                                    comparerDest);
            if (indexEnd < 0)
              indexEnd = ~indexEnd - 1;

            // If we have more than one call to this procedure, loop through them and maybe filter them out
            if (indexEnd - indexStart + 1 > 1)
            {
              // Find the start of the subroutine
              uint minDestination = uint.MaxValue;
              for (int m = indexStart; m <= indexEnd; m++)
              {
                if (callPairs[m].dest < minDestination)
                  minDestination = callPairs[m].dest;
              }

              // Loop through these call destinations to make sure they are not entering in the middle of the subroutine
              for (int m = indexStart; m <= indexEnd; m++)
              {
                if (callPairs[m].dest > minDestination + 2)
                {
                  // This is entering the subrouting in the middle! Remove this call as invalid.
                  callPairs.RemoveAt(m);
                  m--;
                  indexEnd--;
                }
              }

            }
          }

          // Add the calls to the function list)
          for (int n = 0; n < callPairs.Count; n++)
          {
            // Add this function call
            if (disassemblyMode.recordIntramodular)
              oFunctionMaster.addCall(callPairs[n].source, callPairs[n].dest, callPairs[n].type, callPairs[n].name);
          }
          count = count + (ulong)instructions.Count;
        }

        progress.setLabel2("Total Discovered Internal Calls: " + count);

        progress.increment();
      }
      progress.Dispose();

      // Estimate number of parameters for each function
      oFunctionMaster.estimateFunctionParameters(returnAddresses, ebpAddresses, parent);
    }

    private static bool isValidCodeBlock(Process process, HEAP_INFO heap, uint address, List<jmpInstruction> jmps, List<retAddress> rets, uint maxLength)
    {
      // Convert this block to a linear block
      byte[] block = getLinearCodeToRet(process, heap, address, jmps, rets, maxLength);

      // Check if this is a valid code block
      //byte[] block = oMemoryFunctions.readMemory(process, address, addressEnd - address);
      int warnings = LengthDecoder.CountWarnings(block);

      if (warnings > 0)
        return false;

      return true;
    }

    private static byte[] getLinearCodeToRet(Process process, HEAP_INFO heap, uint address, List<jmpInstruction> jmps_in, List<retAddress> rets, uint maxLength)
    {
      // This gets the block of data starting at address, following all force jumps, and ending at the first return.

      byte[] result = new byte[0];

      bool cloned = false;
      List<jmpInstruction> jmps = jmps_in;
      uint curAddress = address;
      retAddress.COMPARER_ADDRESS retComparer = new retAddress.COMPARER_ADDRESS();
      jmpInstructionComparer jmpComparer = new jmpInstructionComparer();

      while (curAddress >= heap.heapAddress && curAddress < heap.heapAddress + heap.heapLength)
      {
        // Find the next return or jump - whichever comes first.

        // Find the next return
        int retIndex = rets.BinarySearch(new retAddress(curAddress, 0), retComparer);
        if (retIndex < 0)
          retIndex = ~retIndex;
        uint nextRet = uint.MaxValue;
        if (retIndex < rets.Count && retIndex >= 0)
          nextRet = rets[retIndex].address;

        // Find the next jump
        int jmpIndex = jmps.BinarySearch(new jmpInstruction(curAddress, 0), jmpComparer);
        if (jmpIndex < 0)
          jmpIndex = ~jmpIndex;
        uint nextJmp = uint.MaxValue;
        if (jmpIndex < jmps.Count && jmpIndex >= 0)
          nextJmp = jmps[jmpIndex].address;

        // Make sure we found either a next jump or return
        if (nextJmp != uint.MaxValue || nextRet != uint.MaxValue)
        {
          // Add this region to the result
          uint blockEnd = (nextJmp > nextRet ? nextRet : nextJmp) - 1;
          uint newLength = (uint)result.Length + (blockEnd - curAddress + 1);
          if (newLength > maxLength)
            newLength = maxLength;
          byte[] newResult = new byte[newLength];
          Array.ConstrainedCopy(result, 0, newResult, 0, result.Length);
          byte[] tmp = oMemoryFunctions.ReadMemory(process, curAddress, (uint)(newResult.Length - result.Length));
          Array.ConstrainedCopy(tmp, 0, newResult, result.Length, tmp.Length);
          result = newResult;

          // Check if we have gathered enough bytes
          if (result.Length >= maxLength)
            return result;

          // If this is a return we are done, jmp we need to update the current address
          if (nextJmp > nextRet)
          {
            // We hit the return statement next. We are done.
            return result;
          }
          else
          {
            // Update the current address to the jmp target
            curAddress = jmps[jmpIndex].destination;

            // Delete this jmp so that we do not follow it more than once
            if (!cloned)
            {
              jmps = new List<jmpInstruction>(jmps_in);
              cloned = true;
            }
            jmps.RemoveAt(jmpIndex);
          }
        }
        else
        {
          // No return or jump coming up, we are done.
          return result;
        }
      }

      // We jumped out of this heap. Stop analysis.
      return result;
    }

    private static bool isValidCodePointer(Process process, uint source, uint destination, HeaderReader headerReader)
    {
      if (headerReader != null)
      {
        // Check that the pointer is valid according to the header reader excluded regions
        if (!headerReader.isValidCodePointer(source))
          return false;
        if (!headerReader.isValidCodePointer(destination))
          return false;
      }
      return true;
    }

    public static bool inject(Form parent, List<HEAP_INFO> invalidCallSourceHeaps)
    {
      return oFunctionMaster.inject((uint)Properties.Settings.Default.CircularBufferSize, (uint)Properties.Settings.Default.MaxRecordedCalls, invalidCallSourceHeaps, parent);
    }


    public static void clearDataBuffers()
    {
      // Reset all the data buffers
    }

    public static void clearActiveProcess(oTabManager tabManager)
    {
      if (oProcess.activeProcess != null)
      {
        // Clean up the old function list tabs if required
        tabManager.cleanupProcessSpecificTabs();

        // Clean up the function master
        oFunctionMaster.reset();
      }

      activeModules = new List<ProcessModule>(0);
      activeProcess = null;
    }

    /// <summary>
    /// This sets how aggressive the disassembly process is.
    /// </summary>
    /// <param name="mode"></param>
    public static void setDisassemblyMode(DISASSEMBLY_MODE mode)
    {
      disassemblyMode = mode;
    }

    /// <summary>
    /// This function sets the active process from the process information array.
    /// </summary>
    /// <param name="itemDetails"></param>
    /// <returns></returns>
    public static bool setActiveProcess(string[] itemDetails)
    {
      activeModules = new List<ProcessModule>(10);
      // Search for this process
      try
      {
        // Result columns are: Process Name, Window Title,Privileges, Eecutable Path
        Process[] processList = Process.GetProcesses();

        for (int i = 0; i < processList.Length; i++)
        {
          // Load the current process
          Process curProcess = processList[i];

          // Determine if we have access to this process
          string filePath = "";
          int pid;
          bool security = false;
          try
          {
            filePath = curProcess.MainModule.FileName;
          }
          catch
          {
            security = true;
          }

          // Generate the list item for this process
          string securityString = (security ? "Higher Privilege Required" : "Good");
          if (itemDetails[0].CompareTo(curProcess.ProcessName) == 0 &&
              Int32.Parse(itemDetails[1]) == curProcess.Id &&
              itemDetails[2].CompareTo(curProcess.MainWindowTitle) == 0 &&
              itemDetails[3].CompareTo(securityString) == 0 &&
              itemDetails[4].CompareTo(filePath) == 0)
          {
            activeProcess = curProcess;
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        oConsole.printException(ex);
      }
      return false;
    }


    /// <summary>
    /// This function loads the process list.
    /// </summary>
    /// <returns></returns>
    public static string[,] getProcessList()
    {
      try
      {
        // Result columns are: Process Name, Window Title,Privileges, Eecutable Path
        Process[] processList = Process.GetProcesses();
        string[,] result = new string[processList.Length, 5];

        for (int i = 0; i < processList.Length; i++)
        {
          // Load the current process
          Process curProcess = processList[i];

          // Determine if we have access to this process
          string filePath = "";
          bool security = false;
          try
          {
            filePath = curProcess.MainModule.FileName;
          }
          catch
          {
            security = true;
          }
          //if ((int)curProcess.MainWindowHandle == 0)
          //    security = true;

          // Generate the list item for this process
          result[i, 0] = curProcess.ProcessName;
          result[i, 1] = curProcess.Id.ToString().PadLeft(10, ' ');
          result[i, 2] = curProcess.MainWindowTitle;
          result[i, 3] = (security ? "Higher Privilege Required" : "Good");
          result[i, 4] = filePath;
        }
        return result;
      }
      catch (Exception ex)
      {
        oConsole.printException(ex);
      }
      return null;
    }

    /// <summary>
    /// This function loads the module list from the active process.
    /// </summary>
    /// <returns></returns>
    public static string[,] getModuleList()
    {
      try
      {
        if (activeProcess != null)
        {
          // Result columns are: Module Name, Base Address, Module Path
          ProcessModuleCollection moduleList = activeProcess.Modules;
          string[,] result = new string[moduleList.Count, 3];

          for (int i = 0; i < moduleList.Count; i++)
          {
            // Generate the list item for this module
            result[i, 0] = moduleList[i].ModuleName;
            result[i, 1] = moduleList[i].BaseAddress.ToString("X") + "h";
            result[i, 2] = moduleList[i].FileName;
          }
          return result;
        }
        oConsole.printMessage("Failed to load module list because there is no active process.");
      }
      catch (Exception ex)
      {
        oConsole.printException(ex);
      }
      return null;
    }

    /// <summary>
    /// This function adds an active module to the activeModule array
    /// </summary>
    /// <param name="moduleDetails"></param>
    /// <returns></returns>
    public static bool addActiveModule(string[] moduleDetails)
    {
      if (activeProcess != null)
      {
        if (activeModules == null)
          activeModules = new List<ProcessModule>(10);

        // Search for this module
        try
        {
          ProcessModuleCollection moduleList = activeProcess.Modules;

          for (int i = 0; i < moduleList.Count; i++)
          {
            // Check to see if this is the correct module
            if (moduleDetails[0].CompareTo(moduleList[i].ModuleName) == 0 &&
                moduleDetails[1].CompareTo(moduleList[i].BaseAddress.ToString("X") + "h") == 0 &&
                moduleDetails[2].CompareTo(moduleList[i].FileName) == 0)
            {
              activeModules.Add(moduleList[i]);
              return true;
            }
          }
        }
        catch (Exception ex)
        {
          oConsole.printException(ex);
        }
      }
      return false;
    }

    /// <summary>
    /// Check if active process still running
    /// </summary>
    /// <returns></returns>
    public static bool processStillRunning()
    {
      return activeProcess != null && !activeProcess.HasExited;
      //return activeProcess != null && Process.GetProcesses().Any(clsProcess => clsProcess.ProcessName == activeProcess.ProcessName && clsProcess.Id == activeProcess.Id);
    }

    [DllImport("kernel32")]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType,
                                               uint flProtect);
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct FREE_DETAILS
  {
    public uint freeLoc;
    public uint sequenceNumber;
    public byte[] data;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ALLOC_DETAILS
  {
    public uint caller;
    public uint arg1;
    public uint arg2;
    public uint arg3;
    public uint sequenceNumber;
    public uint allocLoc;
    public static uint SIZE = 24;
  }


  public class SOURCE_DEST_PAIR
  {
    public uint dest;
    public uint source;
    public oFunction.CALL_TYPE type;
    public SOURCE_DEST_PAIR joinedPair;
    public string name;

    public SOURCE_DEST_PAIR(uint source, uint dest, oFunction.CALL_TYPE type, SOURCE_DEST_PAIR joinedPair, string name)
    {
      this.dest = dest;
      this.source = source;
      this.type = type;
      this.joinedPair = joinedPair;
      this.name = name;
    }

    public class COMPARER_DEST : IComparer<SOURCE_DEST_PAIR>
    {
      int IComparer<SOURCE_DEST_PAIR>.Compare(SOURCE_DEST_PAIR a, SOURCE_DEST_PAIR b)
      {
        return a.dest.CompareTo(b.dest);
      }
    }

    public class COMPARER_SOURCE : IComparer<SOURCE_DEST_PAIR>
    {
      int IComparer<SOURCE_DEST_PAIR>.Compare(SOURCE_DEST_PAIR a, SOURCE_DEST_PAIR b)
      {
        return a.source.CompareTo(b.source);
      }
    }
  }


}