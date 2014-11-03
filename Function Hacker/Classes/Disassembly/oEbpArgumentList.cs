using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionHacker.Classes
{
    public class ebpArgument
    {
        public uint address;
        public int numParams;

        public ebpArgument(uint address, int numParams)
        {
            this.address = address;
            this.numParams = numParams;
        }
    }

    public class ebpArgumentComparer : IComparer<ebpArgument>
    {
        int IComparer<ebpArgument>.Compare(ebpArgument a, ebpArgument b)
        {
            return a.address.CompareTo(b.address);
        }
    }

    public class oEbpArgumentList
        {
        private List<ebpArgument> ebpUsages;
        private bool sorted = false;
        private ebpArgumentComparer comparer;


        public oEbpArgumentList()
        {
            ebpUsages = new List<ebpArgument>(50000);
            comparer = new ebpArgumentComparer();
        }

        /// <summary>
        /// Adds the ret instruction to the list.
        /// </summary>
        /// <param name="address">Address at which the ret instruction is located.</param>
        /// <param name="numParams">The number of parameters poped off the stack. ie ret8 would be 2 parameteres.</param>
        public void addEbpReference(uint address, int numParams)
        {
            ebpUsages.Add( new ebpArgument(address, numParams) );
            sorted = false;
        }

        /// <summary>
        /// Looks up the ret statement likely associated with the
        /// function, and returns the guess for the number of function
        /// arguments.
        /// </summary>
        /// <param name="functionAddress">Address of the start of the function.</param>
        /// <returns>Guessed number of 4-byte parameters.</returns>
        public int guessNumArguments(uint functionAddress, uint functionEnd)
        {
            // Sort the array if required
            if( !sorted )
            {
                // Sort the list so that the binary search will work.
                ebpUsages.Sort(comparer);
                sorted = true;
            }

            // Find the start index
            int indexStart = ebpUsages.BinarySearch(new ebpArgument(functionAddress,0), comparer);
            if (indexStart < 0)
                indexStart = ~indexStart;

            // Find the end index
            int indexEnd = ebpUsages.BinarySearch(new ebpArgument(functionEnd, 0), comparer);
            if (indexEnd < 0)
                indexEnd = ~indexEnd;

            // Check which argument reference referred to the largest argument
            int maxArgs = -1;
            for (int i = indexStart; i < indexEnd; i++ ) // I purposely do NOT check the indexEnd, it is above our ret address.
            {
                if (ebpUsages[i].numParams > maxArgs)
                    maxArgs = ebpUsages[i].numParams;
            }

            if (maxArgs > 0)
                return maxArgs;
            return 0;
        }
    }
}
