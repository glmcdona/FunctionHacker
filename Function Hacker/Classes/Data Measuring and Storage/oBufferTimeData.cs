using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FunctionHacker.Forms;

namespace FunctionHacker.Classes
{
    public class oBufferTimeData
    {
        private List<oSingleData> data; // Array of type oSingleData. 
        private DateTime startTime;
        public double timeStart = 0;
        public double timeEnd = 0;
        private IComparer<oSingleData> dataTimeComparer;

        public oBufferTimeData()
        {
            // Initialize the buffer data parameters
            data = new List<oSingleData>(10000); // Initial capacity of 10,000 function calls.
            dataTimeComparer = (IComparer<oSingleData>)new SingleDataTimeComparer();
            startTime = DateTime.Now;
        }

        public oBufferTimeData(List<oSingleData> data, DateTime startTime, double timeStart, double timeEnd)
        {
            // Initialize the buffer data parameters
            this.data = data;
            this.startTime = startTime;
            this.timeStart = timeStart;
            this.timeEnd = timeEnd;
            dataTimeComparer = (IComparer<oSingleData>)new SingleDataTimeComparer();
        }

        /// <summary>
        /// Shallow clones the object.
        /// </summary>
        /// <returns></returns>
        public oBufferTimeData Clone()
        {
            return new oBufferTimeData(data, startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Gets a list of the addresses of the functions associated with these calls
        /// </summary>
        /// <returns></returns>
        public List<uint> getFunctionList()
        {
            // Bucket the data according to function address
            List<List<oSingleData>> bucket = bucketData(new SingleDataDestinationComparer());

            // Output the resulting function list
            List<uint> functionList = new List<uint>(bucket.Count);
            for( int i = 0; i < bucket.Count; i++)
                functionList.Add(bucket[i][0].destination);

            return functionList;
        }


        /// <summary>
        /// Generate a list of unique stack heaps that have been used by the callers.
        /// </summary>
        /// <returns></returns>
        public List<HEAP_INFO> getStackHeapList()
        {
            // Generate the list of heaps.
            Hashtable heapsHash = new Hashtable();

            // Perform a FindAll search to get the calls to fill out the hashtable.
            oSingleDataFillHashTableSelecter selecter = new oSingleDataFillHashTableSelecter(heapsHash);
            data.FindAll(selecter.fillOutHeapsStack);

            // Return the unique heaps found
            List<HEAP_INFO> heaps = new List<HEAP_INFO>(heapsHash.Count);
            foreach (HEAP_INFO heap in heapsHash.Values)
                heaps.Add(heap);

            return heaps;
        }

        /// <summary>
        /// Returns a list of the modules that have been called by the recorded functions.
        /// </summary>
        /// <returns></returns>
        public List<HEAP_INFO> getModuleList()
        {
            // Generate the list of heaps.
            Hashtable heapsHash = new Hashtable();

            // Perform a FindAll search to get the calls to fill out the hashtable.
            oSingleDataFillHashTableSelecter selecter = new oSingleDataFillHashTableSelecter(heapsHash);
            data.FindAll(selecter.fillOutHeapsDestination);
            
            // Return the unique heaps found
            List<HEAP_INFO> heaps = new List<HEAP_INFO>(heapsHash.Count);
            foreach(HEAP_INFO heap in heapsHash.Values)
                heaps.Add(heap);
            
            return heaps;
        }

        /// <summary>
        /// Returns a new dataset clipped to the specified range of addresses.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_addressDest(RANGE_PARSE_4BYTE range)
        {
            // Perform a FindAll search.
            oSingleDataAddressSelecter selecter = new oSingleDataAddressSelecter(range);
            return new oBufferTimeData(data.FindAll(selecter.isDestinationGood), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Returns a new dataset clipped to stack pointers with the specified range of addresses.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_addressStack(RANGE_PARSE_4BYTE range)
        {
            // Perform a FindAll search.
            oSingleDataAddressSelecter selecter = new oSingleDataAddressSelecter(range);
            return new oBufferTimeData(data.FindAll(selecter.isStackGood), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Returns a new dataset clipped to the specified range of addresses.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_addressSource(RANGE_PARSE_4BYTE range)
        {
            // Perform a FindAll search.
            oSingleDataAddressSelecter selecter = new oSingleDataAddressSelecter(range);
            return new oBufferTimeData(data.FindAll(selecter.isSourceGood), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Returns a new dataset clipped to calls satisfying the specified argument ranges.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public oBufferTimeData filterCalls_ArgumentValue(RANGE_PARSE args)
        {
            // Perform a FindAll search.
            oSingleDataArgumentRangeSelecter selecter = new oSingleDataArgumentRangeSelecter(args);
            return new oBufferTimeData(data.FindAll(selecter.isArgumentGood), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Returns a new dataset clipped to calls of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public oBufferTimeData filterCall_Type(int type)
        {
            // Perform a FindAll search.
            oSingleDataCallTypeSelecter selecter = new oSingleDataCallTypeSelecter(type, oProcess.map);
            return new oBufferTimeData(data.FindAll(selecter.isArgumentGood), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Returns a new dataset clipped to calls satisfying the specified argument count range.
        /// </summary>
        /// <param name="argMinCount"></param>
        /// <param name="argMaxCount"></param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_argumentCount(int argMinCount, int argMaxCount)
        {
            // Perform a FindAll search.
            oSingleDataArgumentCountRangeSelecter selecter = new oSingleDataArgumentCountRangeSelecter(argMinCount,argMaxCount);
            return new oBufferTimeData(data.FindAll(selecter.isArgumentGood), startTime, timeStart, timeEnd);
        }
        /// <summary>
        /// Clips the calls to only calls which received a string as an input.
        /// </summary>
        /// <returns></returns>
        public oBufferTimeData clipCalls_hasStringArgument()
        {
            // Perform a FindAll search.
            oSingleDataHasDereferenceSelecter selecter = new oSingleDataHasDereferenceSelecter();
            return new oBufferTimeData(data.FindAll(selecter.hasStringDereference), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Clips the calls to calls which had a string input that is a partial match to text.
        /// </summary>
        /// <param name="text">String to partial match the string argument to.</param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_hasStringArgumentMatch(string text, bool caseSensitive)
        {
            // Perform a FindAll search.
            oSingleDataHasDereferenceStringMatchSelecter selecter = new oSingleDataHasDereferenceStringMatchSelecter(text, caseSensitive);
            return new oBufferTimeData(data.FindAll(selecter.hasDereferenceStringMatch), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Check to see if a dereference partial matches the specified data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_hasBinaryArgumentMatch(byte[] binary, bool onlyStartsWith)
        {
            // Perform a FindAll search.
            oSingleDataHasDereferenceBinaryMatchSelecter selecter = new oSingleDataHasDereferenceBinaryMatchSelecter(binary, onlyStartsWith);
            return new oBufferTimeData(data.FindAll(selecter.hasDereferenceBinaryMatch), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// This removes calls to the specified functions from the recorded data.
        /// </summary>
        /// <param name="functionAddresses"></param>
        /// <returns></returns>
        public oBufferTimeData clipOutCalls_addresses(List<uint> functionAddresses)
        {
            // Perform a FindAll search.
            oSingleDataCallToAddressSelecter selecter = new oSingleDataCallToAddressSelecter(functionAddresses);
            return new oBufferTimeData(data.FindAll(selecter.isCallNotTo), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// This limits the data to calls to the specified functions.
        /// </summary>
        /// <param name="functionAddresses"></param>
        /// <returns></returns>
        public oBufferTimeData clipCalls_addressesDest(List<uint> functionAddresses)
        {
            // Perform a FindAll search.
            oSingleDataCallToAddressSelecter selecter = new oSingleDataCallToAddressSelecter(functionAddresses);
            return new oBufferTimeData(data.FindAll(selecter.isCallTo), startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// Returns a new oBufferTimeData containing only the functions with calls matching the condition.
        /// </summary>
        /// <param name="minCalls"></param>
        /// <param name="maxCalls"></param>
        /// <returns></returns>
        public oBufferTimeData filterFunctions_CallCountRange(int minCalls, int maxCalls)
        {
            // Bucket the data according to function address
            List<List<oSingleData>> bucket = bucketData(new SingleDataDestinationComparer());

            // Remove buckets not matching the specified size
            List<oSingleData> result = new List<oSingleData>(10000);
            for (int i = 0; i < bucket.Count; i++)
                if (bucket[i].Count <= maxCalls && bucket[i].Count >= minCalls)
                    result.AddRange(bucket[i]);

            // Return the new data structure
            return new oBufferTimeData(result, startTime, timeStart, timeEnd);
        }

        /// <summary>
        /// This function groups 'data' into buckets of identical oSingleData entries according to the specified comparer.
        /// </summary>
        /// <param name="bucketComparer">Comparer to use for bucket creation.</param>
        /// <returns>Grouped oSingleData entries deemed equal by the comparer. The result is sorted by the comparer as well.</returns>
        public List<List<oSingleData>> bucketData(IComparer<oSingleData> bucketComparer)
        {
            // Sort the data by the comparer
            if (data.Count > 0)
            {
                data.Sort(bucketComparer);

                // Now loop through creating the buckets
                List<List<oSingleData>> result = new List<List<oSingleData>>(1000);
                int bucketIndex = 0;
                result.Add(new List<oSingleData>(1));
                result[bucketIndex].Add(data[0]);
                for (int i = 1; i < data.Count; i++)
                {
                    // Add this to a bucket

                    if (bucketComparer.Compare(data[i - 1], data[i]) != 0)
                    {
                        // New bucket
                        bucketIndex++;
                        result.Add(new List<oSingleData>(10));
                    }

                    // Add the call to the bucket
                    result[bucketIndex].Add(data[i]);
                }
                data.Sort(this.dataTimeComparer);
                return result;
            }else
            {
                return new List<List<oSingleData>>(0);
            }
        }

        /// <summary>
        /// Returns the number of times the function has been called in the specified time range.
        /// </summary>
        /// <param name="function">The address of the function in question.</param>
        /// <param name="endTime">Time in seconds at the end time of the region.</param>
        /// <param name="duration">Duration in seconds of the region.</param>
        public int getCallCount(uint function, double endTime, double duration)
        {
            // Extract the data range
            List<oSingleData> dataToProcess = getDataRange(endTime, duration);

            // Perform the search
            oSingleDataSelecter selecter = new oSingleDataSelecter(function);
            return dataToProcess.FindAll(selecter.isFunctionAddress).Count;
        }

        /// <summary>
        /// Returns the number of calls in the dataset.
        /// </summary>
        public int getCallCount()
        {
            return data.Count;
        }

        /// <summary>
        /// Returns the number of times the function has been called in all the data.
        /// </summary>
        /// <param name="function">The address of the function in question.</param>
        public int getCallCount(uint function)
        {
            // Perform the search
            oSingleDataSelecter selecter = new oSingleDataSelecter(function);
            return data.FindAll(selecter.isFunctionAddress).Count;
        }

        /// <summary>
        /// Gets all the recorded data.
        /// </summary>
        /// <returns></returns>
        public List<oSingleData> getData()
        {
            if (data.Count > 0)
            {
                //if (!sortedTime)
                //{
                //    data.Sort(this.dataTimeComparer);
                //    sortedTime = true;
                //}
                return data;
            }
            return null;
        }

        /// <summary>
        /// Gets the data at a specified index.
        /// </summary>
        /// <returns></returns>
        public oSingleData getData(int index)
        {
            if (data.Count > 0 && index < data.Count && index >= 0)
            {
                return data[index];
            }
            return null;
        }


        /// <summary>
        /// Gets the number of recorded function calls.
        /// </summary>
        /// <returns></returns>
        public int getLength()
        {
            return data.Count;
        }

        /// <summary>
        /// Gets the size of the data recording between (endTime-duration) to (endTime)
        /// </summary>
        /// <param name="endTime">End time in seconds.</param>
        /// <param name="duration">Duration in seconds.</param>
        /// <returns>Array of calls corresponding to the specified time range.</returns>
        public List<oSingleData> getDataRange(double endTime, double duration)
        {
            // ASSUMPTION:  Data aquired at time T is actually aquired linearly between time (T) and (T of next data aquisition).
            //              This assumption is required so that the data can be analyzed to determine call order.
            
            double startTime = endTime - duration;

            //if (!sortedTime)
            //{
            //    data.Sort(this.dataTimeComparer);
            //    sortedTime = true;
            //}

            // Load the start time index
            int start = (int)data.BinarySearch(new oSingleData(startTime), dataTimeComparer);
            if (start < 0)
                start = ~start;
            if (start < 0)
                start = 0;

            // Load the end time index
            int end = (int)data.BinarySearch(new oSingleData(endTime), dataTimeComparer);
            if (end < 0)
                end = ~end - 1;

            // Extract the data section
            if (end >= start)
                return data.GetRange(start,end-start+1);
            return new List<oSingleData>(0);
        }

        
        

        /// <summary>
        /// Adds data to the structure with the specified time identifier.
        /// </summary>
        /// <param name="newData">New data to add to the structure.</param>
        public bool addData(byte[] newData, DateTime lastMeasurementTime)
        {
            // ASSUMPTION:  Data aquired at time T is actually aquired between time (T of the previous measurement) and (T).
            //              This assumption is required so that the data can be analyzed to determine call order.
            timeEnd = DateTime.Now.Subtract(startTime).TotalSeconds;
            if (newData == null || newData.Length == 0)
            {
                return true;
            }

            // Calculate the number of new function calls
            int numCalls = oSingleData.calculateNumCalls(newData);

            if (numCalls > 0)
            {
                // Load the last data time and current time
                double currentTime = DateTime.Now.Subtract(startTime).TotalSeconds;
                double lastTime = lastMeasurementTime.Subtract(startTime).TotalSeconds;

                // Allocate more space in the arrays if required
                if( data.Count + numCalls >= data.Capacity )
                {
                    // Create lots more space
                    data.Capacity += 1000000; // Room for 1 million calls
                }

                // Loop through while processing the data
                int index = 0;
                double timeStep = (currentTime - lastTime) / numCalls;
                for( int i = 0; i < numCalls; i++)
                {
                    data.Add(new oSingleData(newData, ref index, lastTime + timeStep * i));
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the size of the data recording between (endTime-duration to endTime)
        /// </summary>
        /// <param name="endTime">End time in seconds.</param>
        /// <param name="duration">Duration in seconds.</param>
        /// <returns>Size in number of recorded calls.</returns>
        public int getDataSize(double endTime, double duration)
        {
            double startTime = endTime - duration;

            //if (!sortedTime)
            //{
            //    data.Sort(this.dataTimeComparer);
            //    sortedTime = true;
            //}

            // Load the start time index
            int start = (int)data.BinarySearch(new oSingleData(startTime), dataTimeComparer);
            if (start < 0)
                start = ~start;
            if (start < 0)
                start = 0;

            // Load the end time index
            int end = (int)data.BinarySearch(new oSingleData(endTime), dataTimeComparer);
            if (end < 0)
                end = ~end - 1;

            if( end >= start )
                return end - start + 1;
            return 0;
        }


        public bool contains(oSingleData rowData)
        {
            // Check if this dataset contains rowData
            return this.data.Contains(rowData);
        }



        internal oSingleData getCallCount(int callIndex)
        {
            return data[callIndex];
        }
    }
}