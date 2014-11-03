using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BufferOverflowProtection;
using FunctionHacker.Forms;

namespace FunctionHacker.Classes
{
    public class SingleDataTimeComparer : IComparer<oSingleData>
    {
        int IComparer<oSingleData>.Compare(oSingleData a, oSingleData b)
        {
            return a.time.CompareTo(b.time);
        }
    }

    public class SingleDataSourceComparer : IComparer<oSingleData>
    {
        int IComparer<oSingleData>.Compare(oSingleData a, oSingleData b)
        {
            return a.source.CompareTo(b.source);
        }
    }

    public class SingleDataDestinationComparer : IComparer<oSingleData>
    {
        int IComparer<oSingleData>.Compare(oSingleData a, oSingleData b)
        {
            return a.destination.CompareTo(b.destination);
        }
    }

    public class SingleDataModuleComparer : IComparer<oSingleData>
    {
        int IComparer<oSingleData>.Compare(oSingleData a, oSingleData b)
        {
            return oMemoryFunctions.LookupAddressInMap(oProcess.map, a.destination).heapAddress.CompareTo(
                    oMemoryFunctions.LookupAddressInMap(oProcess.map, b.destination).heapAddress
                );
        }
    }

    public class oSingleDataFillHashTableSelecter
    {
        private Hashtable selectedHeaps;

        public oSingleDataFillHashTableSelecter(Hashtable selectedHeaps)
        {
            this.selectedHeaps = selectedHeaps;
        }



        public bool fillOutHeapsDestination(oSingleData call)
        {
            // Check to see if this is already in the heaps list
            HEAP_INFO heap = oMemoryFunctions.LookupAddressInMap(oProcess.map, call.destination);
            if( selectedHeaps.Contains(heap.heapAddress) )
                return false;
            selectedHeaps.Add(heap.heapAddress,heap);
            return false;
        }


        public bool fillOutHeapsStack(oSingleData call)
        {
            // Check to see if this is already in the heaps list
            HEAP_INFO heap = oMemoryFunctions.LookupAddressInMap(oProcess.map, call.esp);
            if (selectedHeaps.Contains(heap.heapAddress))
                return false;
            selectedHeaps.Add(heap.heapAddress, heap);
            return false;
        }
    }

    public class oSingleDataSelecter
    {
        private uint functionAddress;

        public oSingleDataSelecter(uint functionAddress)
        {
            this.functionAddress = functionAddress;
        }



        public bool isFunctionAddress(oSingleData call)
        {
            if (call.destination == functionAddress)
            {
                return true;
            }
            {
                return false;
            }

        }
    }

    public class oSingleDataCallTypeSelecter
    {
        private int type;
        private List<HEAP_INFO> map;

        public oSingleDataCallTypeSelecter(int type, List<HEAP_INFO> map)
        {
            this.type = type;
            this.map = map;
        }

        public bool isArgumentGood(oSingleData call)
        {
            
            if( type == 1 )
                return oMemoryFunctions.LookupAddressInMap(map, (uint)call.source).heapAddress !=
                       oMemoryFunctions.LookupAddressInMap(map, (uint)call.destination).heapAddress;
            return oMemoryFunctions.LookupAddressInMap(map, (uint)call.source).heapAddress ==
                       oMemoryFunctions.LookupAddressInMap(map, (uint)call.destination).heapAddress;
        }
    }

    public class oSingleDataHasDereferenceSelecter
    {
        public bool hasDereference(oSingleData call)
        {
            return call.dereferences.Count() > 0;
        }

        public bool hasStringDereference(oSingleData call)
        {
            // Figure out if this dereference has a string dereference
            return call.hasStringDereference();
        }
    }

    public class oSingleDataHasDereferenceStringMatchSelecter
    {
        private string textMatch;
        private bool caseSensitive;

        public oSingleDataHasDereferenceStringMatchSelecter(string textMatch, bool caseSensitive)
        {
            this.textMatch = textMatch;
            this.caseSensitive = caseSensitive;
        }



        public bool hasDereferenceStringMatch(oSingleData call)
        {
            if( textMatch.Length == 0 )
                return true;

            for( int i = 0; i < call.dereferences.Count(); i++ )
            {
                // Ask the dereference if it is a match
                if( call.dereferences[i].isStringMatch(textMatch, caseSensitive) )
                    return true;
            }
            return false; // No derference match found
        }
    }

    public class oSingleDataHasDereferenceBinaryMatchSelecter
    {
        private byte[] data;
        private bool onlyStartWith;

        public oSingleDataHasDereferenceBinaryMatchSelecter(byte[] data, bool onlyStartWith)
        {
            this.data = data;
            this.onlyStartWith = onlyStartWith;
        }



        public bool hasDereferenceBinaryMatch(oSingleData call)
        {
            if (data.Length == 0)
                return true;

            for (int i = 0; i < call.dereferences.Count(); i++)
            {
                // Ask the dereference if it is a match
                if (call.dereferences[i].isBinaryMatch(data, onlyStartWith))
                    return true;
            }
            return false; // No derference match found
        }
    }

    public class oSingleDataArgumentCountRangeSelecter
    {
        private int minArgCount;
        private int maxArgCount;

        public oSingleDataArgumentCountRangeSelecter(int minArgCount, int maxArgCount)
        {
            this.minArgCount = minArgCount;
            this.maxArgCount = maxArgCount;
        }



        public bool isArgumentGood(oSingleData call)
        {
            return call.arguments.Count() <= maxArgCount && call.arguments.Count() >= minArgCount;
        }
    }

    public class oSingleDataCallToAddressSelecter
    {
        private List<uint> functions;

        public oSingleDataCallToAddressSelecter(List<uint> functions)
        {
            this.functions = functions;
        }

        public bool isCallNotTo(oSingleData call)
        {
            return !functions.Contains(call.destination);
        }

        public bool isCallTo(oSingleData call)
        {
            return functions.Contains(call.destination);
        }
    }


    public class oSingleDataAddressSelecter
    {
        private RANGE_PARSE addressRange;

        public oSingleDataAddressSelecter(RANGE_PARSE addressRange)
        {
            this.addressRange = addressRange;
        }


        public bool isDestinationGood(oSingleData call)
        {
            if (addressRange.firstValue is uint)
            {
                // See if one of the calls satisfies this short range
                return call.destination >= (uint)addressRange.firstValue && call.destination <= (uint)addressRange.secondValue;
            }
            return false;
        }

        public bool isSourceGood(oSingleData call)
        {
            if (addressRange.firstValue is uint)
            {
                // See if one of the calls satisfies this short range
                return call.source >= (uint)addressRange.firstValue && call.source <= (uint)addressRange.secondValue;
            }
            return false;
        }

        public bool isStackGood(oSingleData call)
        {
            if (addressRange.firstValue is uint)
            {
                // See if one of the calls satisfies this short range
                return call.esp >= (uint)addressRange.firstValue && call.esp <= (uint)addressRange.secondValue;
            }
            return false;
        }
    }

    public class oSingleDataArgumentRangeSelecter
    {
        private RANGE_PARSE argumentRange;

        public oSingleDataArgumentRangeSelecter(RANGE_PARSE argumentRange)
        {
            this.argumentRange = argumentRange;
        }


        public bool isArgumentGood(oSingleData call)
        {
            if (call.arguments.Length > 0)
            {
                if (argumentRange.firstValue is byte)
                {
                    // See if one of the calls satisfies this byte range
                    if ( (call.eax & 0xff) >= (byte)argumentRange.firstValue && (call.eax & 0xff) <= (byte)argumentRange.secondValue)
                        return true;
                    if ((call.ecx & 0xff) >= (byte)argumentRange.firstValue && (call.ecx & 0xff) <= (byte)argumentRange.secondValue)
                        return true;
                    if ((call.edx & 0xff) >= (byte)argumentRange.firstValue && (call.edx & 0xff) <= (byte)argumentRange.secondValue)
                        return true;
                    return call.arguments.Any(t => ((t & 0x000000ff) >= (byte) argumentRange.firstValue) && ((t & 0x000000ff) <= (byte) argumentRange.secondValue));
                }
                if(argumentRange.firstValue is short)
                {
                    // See if one of the calls satisfies this short range
                    if ((call.eax & 0xffff) >= (short)argumentRange.firstValue && (call.eax & 0xffff) <= (short)argumentRange.secondValue)
                        return true;
                    if ((call.ecx & 0xffff) >= (short)argumentRange.firstValue && (call.ecx & 0xffff) <= (short)argumentRange.secondValue)
                        return true;
                    if ((call.edx & 0xffff) >= (short)argumentRange.firstValue && (call.edx & 0xffff) <= (short)argumentRange.secondValue)
                        return true;
                    return call.arguments.Any(t => ((t & 0x0000ffff) >= (short) argumentRange.firstValue) && ((t & 0x0000ffff) <= (short) argumentRange.secondValue));
                }
                if (argumentRange.firstValue is int)
                {
                    // See if one of the calls satisfies this short range
                    if (call.eax >= (int)argumentRange.firstValue && call.eax <= (int)argumentRange.secondValue)
                        return true;
                    if (call.ecx >= (int)argumentRange.firstValue && call.ecx <= (int)argumentRange.secondValue)
                        return true;
                    if (call.edx >= (int)argumentRange.firstValue && call.edx <= (int)argumentRange.secondValue)
                        return true;
                    return call.arguments.Any(t => (t >= (int)argumentRange.firstValue) && (t <= (int)argumentRange.secondValue));
                }
                if (argumentRange.firstValue is uint)
                {
                    // See if one of the calls satisfies this short range
                    if (call.eax >= (uint)argumentRange.firstValue && call.eax <= (uint)argumentRange.secondValue)
                        return true;
                    if (call.ecx >= (uint)argumentRange.firstValue && call.ecx <= (uint)argumentRange.secondValue)
                        return true;
                    if (call.edx >= (uint)argumentRange.firstValue && call.edx <= (uint)argumentRange.secondValue)
                        return true;
                    return call.arguments.Any(t => (t >= (uint)argumentRange.firstValue) && (t <= (uint)argumentRange.secondValue));
                }
                if (argumentRange.firstValue is double)
                {
                    // See if one of the calls satisfies this float range
                    if (oMemoryFunctions.IntToFloat(call.eax) >= (float)(double)argumentRange.firstValue && oMemoryFunctions.IntToFloat(call.eax) <= (float)(double)argumentRange.secondValue)
                        return true;
                    if (oMemoryFunctions.IntToFloat(call.ecx) >= (float)(double)argumentRange.firstValue && oMemoryFunctions.IntToFloat(call.ecx) <= (float)(double)argumentRange.secondValue)
                        return true;
                    if (oMemoryFunctions.IntToFloat(call.edx) >= (float)(double)argumentRange.firstValue && oMemoryFunctions.IntToFloat(call.edx) <= (float)(double)argumentRange.secondValue)
                        return true;
                    return call.arguments.Any(t => (oMemoryFunctions.IntToFloat((uint)t) >= (float)(double)argumentRange.firstValue) && (oMemoryFunctions.IntToFloat((uint)t) <= (float) (double)argumentRange.secondValue));
                }
            }
            return false;
        }
    }

    public enum DEREF_DATA_TYPE
    {
        ASCII,
        UNICODE,
        ASCII_4BYTE_OFFSET,
        UNICODE_4BYTE_OFFSET,
        BINARY
    }

    public struct dereference
    {
        public uint dereferenceAddress;
        public uint argumentIndex;
        public byte[] data;
        public dereference(uint dereferenceAddress, uint argumentIndex, byte[] data)
        {
            this.dereferenceAddress = dereferenceAddress;
            this.argumentIndex = argumentIndex;
            this.data = data;
        }

        public dereference(byte[] data, int startIndex)
        {
            // Load the basic data
            this.dereferenceAddress = oMemoryFunctions.ByteArrayToUint(data, startIndex);
            this.argumentIndex = oMemoryFunctions.ByteArrayToUint(data, startIndex + 4);

            // Create a copy of the dereferenced data
            this.data = new byte[(int) Properties.Settings.Default.NumDereferencedBytes];
            Array.ConstrainedCopy(data, startIndex + 8, this.data, 0, (int) Properties.Settings.Default.NumDereferencedBytes);
        }

        public static int getSize()
        {
            return (int)Properties.Settings.Default.NumDereferencedBytes + 8;
        }

        public bool isBinaryMatch(byte[] binaryHexString,bool onlyStartWith)
        {
            if (onlyStartWith)
            {
                // See if this is a match
                int j = 0;
                for (j = 0; j < binaryHexString.Length; j++)
                {
                    if (binaryHexString[j] != data[j])
                        break;
                }
                if (j == binaryHexString.Length && binaryHexString[j - 1] == data[j - 1])
                    return true; // We found a match
            }
            else
            {
                // Check to see if this dereference contains the specified binary hex string
                for (int i = 0; i < data.Length - binaryHexString.Length + 1; i++)
                {
                    // See if this is a match
                    int j = 0;
                    for (j = 0; j < binaryHexString.Length; j++)
                    {
                        if (binaryHexString[j] != data[j + i])
                            break;
                    }
                    if (j == binaryHexString.Length && binaryHexString[j - 1] == data[j + i -1])
                        return true; // We found a match
                }
            }

            // Not a match
            return false;
        }

        public bool isStringMatch(string textMatch, bool caseSensitive)
        {
            // First check what type of dereference this is
            string derefAscii = toStringAscii();
            string derefUnicode = toStringUnicode();

            // Check if it is case sensitive match
            if (caseSensitive && (derefAscii.Contains(textMatch) || derefUnicode.Contains(textMatch)))
                return true;

            // Check if it is a case insensitive match
            string textMatchLower = textMatch.ToLower();
            if (!caseSensitive && (derefAscii.ToLower().Contains(textMatchLower) || derefUnicode.ToLower().Contains(textMatchLower)))
                return true;

            return false;
        }

        private string toStringAscii()
        {
            // Form the string based on the data type
            string result = "";
            int i = 0;

            // Print until a null or invalid character
            i = 0;
            while (i < data.Length)
            {
                if( data[i] >= 32 && data[i] <= 126)
                    result = result + (char)data[i];
                else
                    result = result + ".";
                i++;
            }
                    
            return result;
        }

        private string toStringUnicode()
        {
            // Form the string based on the data type
            string result = "";
            int i = 0;

            // Print until a null or invalid character
            i = 0;
            while (i < data.Length)
            {
                if (data[i] >= 32 && data[i] <= 126)
                    result = result + (char)data[i];
                else
                    result = result + ".";
                i+=2;
            }

            return result;
        }

        public bool isStringDereference()
        {
            return data[0] >= 0x20 && data[0] <= 0x7E && data[1] == 0 && data[2] >= 0x20 && data[2] <= 0x7E && data[3] == 0
                   || data[0] >= 0x20 && data[0] <= 0x7E && data[1] >= 0x20 && data[1] <= 0x7E && data[2] >= 0x20 && data[2] <= 0x7E;
        }
    }

    /// <summary>
    /// One of the classes is created for each recorded function call.
    /// </summary>
    public class oSingleData
    {
        public double time;
        public uint destination;
        public uint source;
        public uint esp;
        public uint ecx;
        public uint edx;
        public uint eax;
        public uint[] arguments;
        public dereference[] dereferences;
        public static int minSize = 32;

        /// <summary>
        /// This function quickly calculates the number of calls contained in the data.
        /// </summary>
        /// <param name="data">Raw data.</param>
        /// <returns>Number of calls.</returns>
        public static int calculateNumCalls(byte[] data)
        {
            int index = 0;
            int count = 0;
            int derefSize = dereference.getSize();
            while( index + minSize - 1 < data.Length )
            {
                int numArgs = ((int) oMemoryFunctions.ByteArrayToUint(data, index + 24));
                int numDeref = ((int)oMemoryFunctions.ByteArrayToUint(data, index + 28 + 4 * numArgs));
                index += minSize + 4 * numArgs + numDeref * derefSize;
                count++;
            }

            if (index > data.Length)
                count--;

            return count;
        }

        /// <summary>
        /// This is used to create an empty class for a specific time. This is used to perform a binary search with the time comparison.
        /// </summary>
        /// <param name="time"></param>
        public oSingleData(double time)
        {
            this.time = time;
        }

        /// <summary>
        /// Initializes this data element.
        /// </summary>
        /// <param name="data">The read-in data array.</param>
        /// <param name="index">The index in the data array in which to start reading from, this is incremented according to the size of the data measurement read in.</param>
        /// /// <param name="time">The time of the measurement with respect to the start time of the measurement.</param>
        public oSingleData(byte[] data, ref int index, double time)
        {
            if( index + minSize - 1 >= data.Length )
            {
                // Not enough data
                index = -1;
                return;
            }

            // Load this call information
            this.time = time;
            this.destination = oMemoryFunctions.ByteArrayToUint(data, index);
            this.source = oMemoryFunctions.ByteArrayToUint(data, index + 4);
            this.esp = oMemoryFunctions.ByteArrayToUint(data, index + 8);
            this.ecx = oMemoryFunctions.ByteArrayToUint(data, index + 12);
            this.edx = oMemoryFunctions.ByteArrayToUint(data, index + 16);
            this.eax = oMemoryFunctions.ByteArrayToUint(data, index + 20);
            
            
            // Load the arguments
            int numParameters = (int)oMemoryFunctions.ByteArrayToUint(data, index + 24);
            if (index + minSize + 4*numParameters > data.Length || numParameters > 0x100)
            {
                // Not enough data
                index = -1;
                return;
            }
            arguments = new uint[numParameters];
            for( int i = 0; i < arguments.Length; i++ )
            {
                arguments[i] = oMemoryFunctions.ByteArrayToUint(data, index + 28 + i * 4);
            }

            // Load the dereferences
            int derefSize = dereference.getSize();
            int numDereferences = (int)oMemoryFunctions.ByteArrayToUint(data, index + 28 + numParameters*4 );
            if (index + minSize + 4 * numParameters
                + numDereferences * derefSize > data.Length)
            {
                // Not enough data
                index = -1;
                return;
            }
            dereferences = new dereference[numDereferences];
            for (int i = 0; i < dereferences.Length; i++)
            {
                // Input this dereference structure
                dereferences[i] = new dereference(data, index + 32 + numParameters * 4 + i * derefSize);
            }

            // Update the index
            index += minSize + 4 * numParameters + numDereferences * derefSize;
        }

        // Check if this single data element has a string dereference
        public bool hasStringDereference()
        {
            for (int i = 0; i < dereferences.Count(); i++ )
            {
                if( dereferences[i].isStringDereference() )
                    return true;
            }
            return false;
        }
    }
}