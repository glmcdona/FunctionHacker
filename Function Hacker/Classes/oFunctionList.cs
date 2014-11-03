using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using FunctionHacker.Classes.Visualization;
using FunctionHacker.Controls;
using FunctionHacker.Forms;

namespace FunctionHacker.Classes
{
    public enum SORT_PARAMETER
    {
        ADDRESS,
        CALL_COUNT,
        DESCRIPTION,
        MODULE
    }

    /// <summary>
    /// One of these classes exists for each function list tab on the main form. They each hold separate function lists 
    /// and separate measured data.
    /// </summary>
    public class oFunctionList
    {
        public List<oFunction> functions; // Array of oFunction, this can be a subset of the whole function list.
        public oBufferTimeData dataVis;
        public bool recording;
        public FunctionListViewer parentControl;
        
        public oFunctionList(List<oFunction> functions)
        {
            if( functions != null )
            {
                this.functions = functions;

                // Sort the list
                if( functions.Count > 0)
                    functions.Sort(new FunctionCompareAddress());
            }
        }

        /// <summary>
        /// Shallow clones this object.
        /// </summary>
        /// <returns></returns>
        public oFunctionList Clone()
        {
            oFunctionList result = (oFunctionList)MemberwiseClone();
            if (result.dataVis != null)
                result.dataVis = result.dataVis.Clone();
            if (result.functions != null)
                result.functions = new List<oFunction>(result.functions);
            return result;
        }


        public void updateFunctionList()
        {
            // We need to update the functions that are involved with this dataset.
            if (dataVis != null)
            {
                List<uint> calledFunctions = dataVis.getFunctionList();

                // Extract the new function calls
                functions = oFunctionMaster.getFunctionListFromAddressList(calledFunctions);
            }
        }

        /// <summary>
        /// Clips this data to calls made between the specified time range.
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void clipCalls_timeRange(double startTime, double endTime)
        {
            if (dataVis != null)
            {
                // Update the data
                dataVis = new oBufferTimeData(dataVis.getDataRange(endTime, endTime - startTime), DateTime.Now,
                                                   startTime, endTime);
            }
        }

        /// <summary>
        /// This clips the calls to match the specified input argument range.
        /// </summary>
        /// <param name="args"></param>
        public void clipCalls_argument(RANGE_PARSE args)
        {
            if (dataVis != null)
                dataVis = dataVis.filterCalls_ArgumentValue(args);
        }

        /// <summary>
        /// Filters calls by their type.
        /// </summary>
        /// <param name="type">1 for inter-modular, 2 for intra-modular.</param>
        public void clipCalls_type(int type)
        {
            if (dataVis != null)
                dataVis = dataVis.filterCall_Type(type);
        }

        /// <summary>
        /// Clip to calls within the specified range of parameters.
        /// </summary>
        /// <param name="argMinCount"></param>
        /// <param name="argMaxCount"></param>
        public void clipCalls_argumentCount(int argMinCount, int argMaxCount)
        {
            if( dataVis != null )
                dataVis = dataVis.clipCalls_argumentCount(argMinCount, argMaxCount);
        }

        /// <summary>
        /// This clips this dataset to functions who have been called between minCalls and maxCalls inclusively.
        /// </summary>
        /// <param name="minCalls">Minimum number of calls.</param>
        /// <param name="maxCalls">Maximum number of calls.</param>
        public void clipFunctions_CallCount(int minCalls, int maxCalls)
        {
            if( dataVis == null )
                return;

            // Filter the data
            if( minCalls == 0 )
            {
                // Include functions in dataVis and functions with 0 calls
                List<uint> calledFunctionsBefore = dataVis.getFunctionList();
                dataVis = dataVis.filterFunctions_CallCountRange(minCalls, maxCalls);

                // We need to update the functions that are involved with this dataset.
                List<uint> calledFunctionsAfter = dataVis.getFunctionList();

                // Remove all the functions that are in calledFunctionsBefore but not in calledFunctionsAfter
                functions = new List<oFunction>(functions);
                foreach(uint address in calledFunctionsBefore)
                {
                    if( !calledFunctionsAfter.Contains(address) )
                    {
                        // Remove this function
                        int index = functions.BinarySearch(new oFunction(0, address, oFunction.CALL_TYPE.FIXED_OFFSET,""), new FunctionCompareAddress());
                        if( index >= 0 )
                            functions.RemoveAt(index);
                    }
                }

            }else
            {
                dataVis = dataVis.filterFunctions_CallCountRange(minCalls, maxCalls);

                // Include only functions in dataVis
                updateFunctionList();
            }
        }

        public void clipFunctions_address(List<uint> functionAddresses)
        {
            // Clip the function list
            FunctionSelectFromAddressArray selecter = new FunctionSelectFromAddressArray(functionAddresses);
            functions = functions.FindAll(selecter.isInList);
        }

        public void clipFunctions_selected(List<int> rowIndices)
        {
            // Clip the function list to the selected rows
                    
            // Generate the list of functions
            List<uint> functionAddresses = new List<uint>(rowIndices.Count);
            List<oFunction> newFunctionList = new List<oFunction>(rowIndices.Count);
            for (int i = 0; i < rowIndices.Count; i++)
            {
                if (rowIndices[i] >= 0 && rowIndices[i] < functions.Count)
                {
                    functionAddresses.Add(functions[rowIndices[i]].address);
                    newFunctionList.Add((oFunction)oFunctionMaster.destinationToFunction[functions[rowIndices[i]].address]);
                }
            }
            this.functions = newFunctionList;

            // Sort the functions list
            this.functions.Sort(new FunctionCompareAddress());

            if (dataVis != null)
            {
                // Now lets filter out the calls to these function addresses
                dataVis = dataVis.clipCalls_addressesDest(functionAddresses);
            }
        }

        public void clipCalls_addressDest(RANGE_PARSE_4BYTE range)
        {
            if( dataVis != null )
                dataVis = dataVis.clipCalls_addressDest(range);
        }

        public void clipCalls_addressStack(RANGE_PARSE_4BYTE range)
        {
            if (dataVis != null)
                dataVis = dataVis.clipCalls_addressStack(range);
        }


        public void clipCalls_addressSource(RANGE_PARSE_4BYTE range)
        {
            if (dataVis != null)
                dataVis = dataVis.clipCalls_addressSource(range);
        }

        /// <summary>
        /// Clips the calls to only calls which received a string as an input.
        /// </summary>
        public void clipCalls_hasStringArgument()
        {
            if (dataVis != null)
                dataVis = dataVis.clipCalls_hasStringArgument();
        }

        /// <summary>
        /// Clips the calls to calls which had a string input that is a partial match to text.
        /// </summary>
        /// <param name="text">String to partial match the string argument to.</param>
        public void clipCalls_hasStringArgumentMatch(string text, bool caseSensitive)
        {
            if (dataVis != null)
                dataVis = dataVis.clipCalls_hasStringArgumentMatch(text, caseSensitive);
        }


        /// <summary>
        /// Clips the calls to calls which had a string input that is a partial match to text.
        /// </summary>
        /// <param name="text">String to partial match the string argument to.</param>
        public void clipCalls_hasBinaryArgumentMatch(byte[] data,bool onlyStartWith)
        {
            if (dataVis != null)
                dataVis = dataVis.clipCalls_hasBinaryArgumentMatch(data, onlyStartWith);
        }

        /// <summary>
        /// Begins recording function call data. The visualization is updated while recording.
        /// </summary>
        public bool startRecording()
        {
            if( functions == null )
                return false;

            // Update the list state
            recording = true;

            // Clear the recorded data
            dataVis = null;

            // First record the visualization circular buffer information
            oFunctionMaster.reader_visualization.startRecording(10); // Update 100 times a second

            // Tell the functions in this list that we are now recording
            foreach( oFunction function in functions )
            {
                function.startRecording();
            }
            return true;
        }

        public void stopRecording()
        {
            // Update the list state
            recording = false;

            // Tell the functions in this list that we are no longer recording
            foreach (oFunction function in functions)
            {
                function.stopRecording();
            }

            // Read in the final data
            dataVis = oFunctionMaster.reader_visualization.stopRecording();
        }

        /// <summary>
        /// Gets either the currently recording visualization data, or the recorded visualization data.
        /// </summary>
        /// <returns></returns>
        public List<oSingleData> getData()
        {
            if (recording)
            {
                return oFunctionMaster.reader_visualization != null ? oFunctionMaster.reader_visualization.getData() : new List<oSingleData>(0);
            }
            return dataVis != null ? dataVis.getData() : new List<oSingleData>(0);
        }

        /// <summary>
        /// Gets the length of the data at the specified time and duration.
        /// </summary>
        /// <returns>Number of bytes.</returns>
        public int getDataSize(double endTime, double duration)
        {
            if (recording)
            {
                return oFunctionMaster.reader_visualization != null ? oFunctionMaster.reader_visualization.getDataSize(endTime, duration) : 0;
            }
            return dataVis != null ? dataVis.getDataSize(endTime, duration) : 0;
        }

        /// <summary>
        /// Gets the length of the data at the specified time and duration.
        /// </summary>
        /// <returns>Number of bytes.</returns>
        public List<oSingleData> getDataRange(double endTime, double duration)
        {
            if (recording)
            {
                return oFunctionMaster.reader_visualization != null ? oFunctionMaster.reader_visualization.getDataRange(endTime, duration) : new List<oSingleData>(0);
            }
            return dataVis != null ? dataVis.getDataRange(endTime, duration) : new List<oSingleData>(0);
        }

        /// <summary>
        /// Returns the number of functions in the list.
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            return (functions != null ? functions.Count: 0 );
        }

        /// <summary>
        /// Returns the number of calls in the list.
        /// </summary>
        /// <returns></returns>
        public int getCallCount()
        {
            return (dataVis != null ? dataVis.getCallCount() : 0);
        }


        /// <summary>
        /// Generates a list of unique modules that have been called by the data recording.
        /// </summary>
        /// <returns></returns>
        public List<HEAP_INFO> getModuleList()
        {
            if( dataVis == null )
            {
                return new List<HEAP_INFO>(0);
            }
            return dataVis.getModuleList();
        }

        /// <summary>
        /// Generates a list of unique stack heaps used by the calls.
        /// </summary>
        /// <returns></returns>
        public List<HEAP_INFO> getStackHeapList()
        {
            if (dataVis == null)
            {
                return new List<HEAP_INFO>(0);
            }

            // Update the process memory map if the process is still running
            if( oProcess.processStillRunning() )
                oProcess.generateMemoryMap();

            // Get the list of stack heaps
            return dataVis.getStackHeapList();
        }

        /// <summary>
        /// Returns the row and column text for a listgridview of functions.
        /// </summary>
        /// <param name="row">Cell row</param>
        /// <param name="column">Cell column</param>
        /// <returns></returns>
        public string getFunctionListCell(int row, int column, oVisPlayBar playBar)
        {
            // Get this row and column
            if(row<functions.Count && row >= 0 && functions[row] != null)
            {
                switch (column)
                {
                    case 0:
                        // Get the address description for this function
                        HEAP_INFO heap = oMemoryFunctions.LookupAddressInMap(oProcess.map, functions[row].address);

                        string name;

                        // Generate the string representation of this address
                        if (heap.associatedModule != null)
                        {
                            name = heap.associatedModule.ModuleName + " + 0x" +
                                          (functions[row].address - (uint)heap.associatedModule.BaseAddress).ToString("X");
                        }
                        else
                        {
                            name = "0x" + functions[row].address.ToString("X");
                        }

                        // Add the known name on a second line
                        if (functions[row].name != "")
                            name = functions[row].name + " - " + name;

                        if (functions[row].disabled)
                            name = "DISABLED " + name;

                        return name;
                    case 1:
                        // Address
                        return functions[row].address.ToString("X");
                    case 2:
                        // Call count
                        if (dataVis != null)
                        {
                            // Lets count the number of calls
                            int tmpCount =
                                dataVis.getCallCount(functions[row].address);

                            // If we are near the maximum number of calls, lets add a + sign. This is because this function
                            // stopped recorded further calls for performance reasons. (eg. a function being called millions
                            // of times a second uses up a lot of diskspace to record!)
                            string totalCount = tmpCount < Properties.Settings.Default.MaxRecordedCalls
                                                    ? tmpCount.ToString()
                                                    : tmpCount.ToString() + @"+";

                            // Get the selected region call count                           
                            string selectedRegionCount;
                            if (playBar != null)
                            {
                                selectedRegionCount = playBar.countCallSelected(functions[row].address).ToString();
                            }else
                            {
                                selectedRegionCount = "";
                            }

                            // Return the two results
                            return String.Concat(totalCount, "/", selectedRegionCount);
                        }
                        return "no data";
                    case 3:
                        // Number of arguments and calling convention
                        return String.Concat(functions[row].getNumParams().ToString(), " args");
                    default:
                        return "?";
                }
            }
            return "?";
        }


        /// <summary>
        /// Returns the row and column text for a listgridview of calls.
        /// </summary>
        /// <param name="row">Cell row</param>
        /// <param name="column">Cell column</param>
        /// <returns></returns>
        public Object getCallListCell(int row, int column, int columnWidth, Font font)
        {
            if (dataVis == null)
                return "";

            // Get this row and column
            if (row < dataVis.getCallCount() && row >= 0)
            {
                // Get the call in question
                oSingleData call = dataVis.getData(row);
                if (call == null)
                    return "";

                HEAP_INFO heap;
                string name;
                switch (column)
                {
                    case 0:
                        return row.ToString();
                    case 1:
                        // Source address
                        heap = oMemoryFunctions.LookupAddressInMap(oProcess.map, call.source);

                        // Generate the string representation of this address
                        if (heap.associatedModule != null)
                        {
                            name = heap.associatedModule.ModuleName + " + 0x" +
                                          (call.source - (uint)heap.associatedModule.BaseAddress).ToString("X") +
                                          " (0x" + call.source.ToString("X") + ")";
                        }
                        else
                        {
                            name = "0x" + call.source.ToString("X");
                        }
                        return name;

                    case 2:
                        // Destination address
                        heap = oMemoryFunctions.LookupAddressInMap(oProcess.map, call.destination);

                        // Generate the string representation of this address
                        if (heap.associatedModule != null)
                        {
                            name = heap.associatedModule.ModuleName + " + 0x" +
                                          (call.destination - (uint)heap.associatedModule.BaseAddress).ToString("X")
                                           + " (0x" + call.destination.ToString("X") + ")";
                        }
                        else
                        {
                            name = "0x" + call.destination.ToString("X");
                        }

                        // Add the function name if known
                        if (oFunctionMaster.destinationToFunction.ContainsKey(call.destination))
                        {
                            oFunction function = (oFunction) oFunctionMaster.destinationToFunction[call.destination];
                            if (function.name != "")
                                name = function.name + " - " + name;
                            if (function.disabled)
                                name = @"DISABLED - " + name;
                        }
                        return name;

                    case 3:
                        // Arguments
                        oFunction functionBase = (oFunction) oFunctionMaster.destinationToFunction[call.destination];
                        if (functionBase == null)
                            return "error, null associated function";

                        // Return the function data pair with the specified column width restriction
                        if( columnWidth > 0 )
                            return functionBase.getArgumentString(call, columnWidth, font).toString();
                        return functionBase.getArgumentString(call).toString();

                    default:
                        return "?";

                }
            }
            return "?";
        }

        public string getCallArgumentString(List<int> rows)
        {
            // Generates a merged argument string for the specified call rows.
            if( rows.Any() && dataVis != null )
            {
                // Sort the rows, arguments will be displayed in call order
                rows.Sort();
                // Check the first datapoint
                oSingleData datapoint = dataVis.getData(rows[0]);
                if (datapoint == null)
                    return string.Empty;
                if (oFunctionMaster.destinationToFunction.ContainsKey(datapoint.destination))
                {
                    // Loop through, printing the calls
                    string result = "";
                    for( int i = 0; i < rows.Count; i++ )
                    {
                        datapoint = dataVis.getData(rows[i]);
                        oFunction function = ((oFunction)oFunctionMaster.destinationToFunction[datapoint.destination]);
                        result = result + datapoint.source.ToString("X") + " -> " + datapoint.destination.ToString("X") + Environment.NewLine + function.getArgumentString(datapoint).toString() + Environment.NewLine + Environment.NewLine;
                    }

                    // Finished merging
                    return result.Trim();
                }
                return "ERROR: The function corresponding to address " + datapoint.destination.ToString("X") + " was not found.";
            }
            return "";
        }

        public void invalidateSelectedCount()
        {
            // Refresh the call count display
            if (parentControl != null)
                parentControl.InvalidateCallCountDataOnly();
        }

        /// <summary>
        /// This re-enables all the specified functions by moving the 'ret' statement to the top.
        /// </summary>
        /// <param name="rowIndices"></param>
        public void disableFunctions(List<int> rowIndices)
        {
            for(int i = 0; i < rowIndices.Count; i++)
            {
                if( i < functions.Count && i >= 0)
                    functions[rowIndices[i]].disable();
            }
        }

        /// <summary>
        /// This disables all the specified functions by restoring the previous starting code.
        /// </summary>
        /// <param name="rowIndices"></param>
        public void enableFunctions(List<int> rowIndices)
        {
            for (int i = 0; i < rowIndices.Count; i++)
            {
                if (i < functions.Count && i >= 0)
                    functions[rowIndices[i]].enable();
            }
        }

        /// <summary>
        /// This removes the functions corresponding to the indices. All calls are removed from
        /// the call recording to these functions as well.
        /// </summary>
        /// <param name="rowIndices"></param>
        public void removeFunctions(List<int> rowIndices)
        {
            // Generate the list of destination addresses
            List<uint> functionAddresses = new List<uint>(rowIndices.Count);
            for (int i = 0; i < rowIndices.Count; i++)
            {
                if (rowIndices[i] >= 0 && rowIndices[i] < functions.Count)
                {
                    functionAddresses.Add(functions[rowIndices[i]].address);
                }
            }

            if( dataVis != null )
            {
                // Filter out the calls
                // Now lets filter out the calls to these function addresses
                dataVis = dataVis.clipOutCalls_addresses(functionAddresses);
            }

            // Remove the functions from the list
            FunctionSelectFromAddressArray selecter = new FunctionSelectFromAddressArray(functionAddresses);
            functions = functions.FindAll(selecter.isNotInList);
        }

        /// <summary>
        /// Returns the function corresponding to 'row' in the function list.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public oFunction getFunction(int row)
        {
            // Lookup this function
            return functions[row];
        }

        /// <summary>
        /// Gets the function call at the specifed index.
        /// </summary>
        /// <param name="callIndex"></param>
        /// <returns></returns>
        public oSingleData getFunctionCall(int callIndex)
        {
            if( dataVis != null )
                return dataVis.getCallCount(callIndex);
            return null;
        }

        public oFunction getFunctionFromCallIndex(int rowIndex)
        {
            if (dataVis != null)
            {
                uint destAddress = dataVis.getData(rowIndex).destination;

                // Lookup the function
                return (oFunction)oFunctionMaster.destinationToFunction[destAddress];
            }
            return null;
        }
    }

    
}
