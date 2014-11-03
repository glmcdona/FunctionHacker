using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BufferOverflowProtection;

namespace FunctionHacker.Classes
{
    

    public class oCircularBufferReader
    {
        private uint start; // Address of the start of the buffer.
        private uint length; // Length of the buffer.
        private uint offsetAddress; // Address to the memory location holding the current offset information.
        private uint lastOffset; // Last offset when we last read in data.
        private DateTime lastRecordTime;

        public oBufferTimeData data;

        private bool record;
        private int refreshTime;
        private Thread recordingThread = null;
        

       

        /// <summary>
        /// Start a new circular buffer reader class. This is for recording data from another process, where the data is being
        /// stored in a circular buffer format.
        /// </summary>
        /// <param name="start">Base address of the circular buffer.</param>
        /// <param name="length">Length of the circular buffer.</param>
        /// <param name="offsetAddress">Address of where the current offset within the circular buffer is stored.</param>
        public oCircularBufferReader(uint start, uint length, uint offsetAddress)
        {
            this.start = start;
            this.length = length;
            this.offsetAddress = offsetAddress;
            this.lastOffset = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, offsetAddress);
            record = false;
        }

        ~oCircularBufferReader()
        {
            // Terminate the thread, if it exists.
            try
            {
                record = false;
            }catch
            {
                // Do nothing, we don't care if it has an exception.
            }
            
        }

        /// <summary>
        /// Gets the size of the data recording between (endTime-duration to endTime)
        /// </summary>
        /// <param name="endTime">End time in seconds.</param>
        /// <param name="duration">Duration in seconds.</param>
        /// <returns>Size in bytes of the data.</returns>
        public List<oSingleData> getDataRange(double endTime, double duration)
        {
            List<oSingleData> result;
            if (recordingThread != null && recordingThread.IsAlive)
            {
                // Create a lock on the data
                lock (data)
                {
                    // Now request the data
                    result = data.getDataRange(endTime, duration);
                }
            }
            else
            {
                // Request the data
                result = data.getDataRange(endTime, duration);
            }

            return result;
        }


        /// <summary>
        /// Gets the size of the data recording between (endTime-duration to endTime)
        /// </summary>
        /// <param name="endTime">End time in seconds.</param>
        /// <param name="duration">Duration in seconds.</param>
        /// <returns>Size in bytes of the data.</returns>
        public int getDataSize(double endTime, double duration)
        {
            // Lock the data
            if (data == null)
                return 0;

            if (recordingThread != null)
            {
                // Create a lock on the data
                lock (data)
                {
                    // Get the data size
                    return data.getDataSize(endTime, duration);
                }
            }else
            {
                // Get the data size
                return data.getDataSize(endTime, duration);
            }

            
        }
        

        /// <summary>
        /// Gets all the data.
        /// </summary>
        /// <returns></returns>
        public List<oSingleData> getData()
        {
            if( data == null )
                return new List<oSingleData>(0);

            if (recordingThread != null)
            {
                // Create a lock on the data
                lock (data)
                {
                    // Now request the data
                    return data.getData();
                }
            }

            // Now request the data
            return data.getData();
        }

        /// <summary>
        /// Starts recording data from the circular buffer. It automatically starts a new thread
        /// and reads in data at an interval of 'refreshRate' milliseconds.
        /// </summary>
        /// <param name="refreshTime">Refresh rate in milliseconds. ie. 100 for ten times a second.</param>
        public void startRecording(int refreshTime)
        {
            // Update the parameters
            if (data != null)
            {
                lock (data)
                    data = new oBufferTimeData();
            }
            else
            {
                data = new oBufferTimeData();
            }

            lastOffset = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, offsetAddress);
            record = true;
            this.refreshTime = refreshTime;
            lastRecordTime = DateTime.Now;

            // Create the data recording thread
            if (recordingThread != null && recordingThread.IsAlive)
                recordingThread.Abort();
            recordingThread = new Thread(new ThreadStart(update));
            recordingThread.Start();
        }


        /// <summary>
        /// Stop recording data.
        /// </summary>
        /// <returns>Array of data recorded.</returns>
        public oBufferTimeData stopRecording()
        {
            // Stop the data recording
            record = false;
            return data;
        }



        /// <summary>
        /// Update the data buffer with the specified refresh rate.
        /// </summary>
        /// <param name="refreshRate"></param>
        private void update()
        {
            try
            {
                while (record && Application.OpenForms.Count > 0 && oProcess.processStillRunning())
                {
                    
                    // Read any new data in the circular buffer
                    uint newOffset = oMemoryFunctions.ReadMemoryDword(oProcess.activeProcess, offsetAddress);

                    if (newOffset != lastOffset)
                    {
                        // We have new data to read in

                        if (newOffset < lastOffset)
                        {
                            // Data wrapped around, lets just input one part of the data while leaving the wrapped data for the next input cycle.
                            newOffset = length - 1;
                        }

                        // Sleep this thread for a little bit. This positioning for the refresh time delay is chosen to make sure
                        // that the target process has time to finish filling out the structures we are about to read in.
                        Thread.Sleep(refreshTime);

                        // Read in data
                        byte[] newData = oMemoryFunctions.ReadMemory(oProcess.activeProcess, start + lastOffset,
                                                                     newOffset - lastOffset + 1);

                        // Add the new data to the time buffer array
                        lock( data )
                        {
                            record = data.addData(newData, lastRecordTime);
                        }

                        lastOffset = newOffset >= length - 1 ? 0 : newOffset;
                            // Deal with wrap-arounds of the circular buffer
                    }
                    else
                    {
                        // Sleep this thread for a little bit
                        Thread.Sleep(refreshTime);

                        data.addData(new byte[0], lastRecordTime);
                    }

                    lastRecordTime = DateTime.Now;

                    // Update the table of valid read pointers
                    // TODO: enable this again later once the event system is in place.
                    if (oProcess.disassemblyMode.aggressiveDereferencing == true)
                        oFunctionMaster.updateValidReadPointerTable();
                }
            }catch(ThreadAbortException e)
            {
                // Cleanup
                lastRecordTime = DateTime.Now;
            }
        }
    }
}
