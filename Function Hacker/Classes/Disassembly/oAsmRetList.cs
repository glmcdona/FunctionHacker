using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionHacker.Classes
{
    public class retAddress
    {
        public uint address;
        public uint numParams;

        public retAddress(uint address, uint numParams)
        {
            this.address = address;
            this.numParams = numParams;
        }

        public class COMPARER_ADDRESS : IComparer<retAddress>
        {
            int IComparer<retAddress>.Compare(retAddress a, retAddress b)
            {
                return a.address.CompareTo(b.address);
            }
        }
    }

    public class retAddressSelecter
    {
        private uint addressStart;
        private uint addressEnd;

        public retAddressSelecter(uint addressStart, uint addressEnd)
        {
            this.addressStart = addressStart;
            this.addressEnd = addressEnd;
        }



        public bool isWithinRange(retAddress ret)
        {
            if (ret.address <= addressEnd && ret.address >= addressStart)
            {
                return true;
            }
            return false;
        }
    }

    public class retAddressComparer : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            if (a is uint && b is uint)
                return ((uint) a).CompareTo((uint) b);
            if (b is uint)
                return ((retAddress)a).address.CompareTo((uint)b);
            if (a is uint)
                return ((uint)a).CompareTo(((retAddress)b).address);
            return ((retAddress)a).address.CompareTo(((retAddress)b).address);
        }
    }

    public class oAsmRetList
    {
        private List<retAddress> rets;
        private bool sorted = false;
        private retAddress.COMPARER_ADDRESS comparer;
        

        public oAsmRetList()
        {
            rets = new List<retAddress>(50000);
            comparer = new retAddress.COMPARER_ADDRESS();
        }

        /// <summary>
        /// Adds the ret instruction to the list.
        /// </summary>
        /// <param name="address">Address at which the ret instruction is located.</param>
        /// <param name="numParams">The number of parameters poped off the stack. ie ret8 would be 2 parameteres.</param>
        public void addRet(uint address,uint numParams)
        {
            // Insert this ret instruction
            rets.Add(new retAddress(address, numParams));
            sorted = false;
        }

        /// <summary>
        /// Looks up the ret statement likely associated with the
        /// function, and returns the guess for the number of function
        /// arguments.
        /// </summary>
        /// <param name="functionAddress">Address of the start of the function.</param>
        /// <returns>Guessed number of 4-byte parameters.</returns>
        public uint guessNumArguments(uint functionAddress, out uint functionEnd)
        {
            // Sort the array if required
            if( !sorted )
            {
                // Sort the list so that the binary search will work.
                rets.Sort(comparer);
                sorted = true;
            }

            // Perform a binary search algorithm, array is assumed to be already sorted properly
            // according to the order they called addRet()
            int index = rets.BinarySearch(new retAddress(functionAddress,0), comparer);
            
            // The result is probably negative of the index, because functionAddress was unlikely found.
            if( index < 0 )
            {
                index = ~index;
            }

            if (index >= rets.Count || index < 0)
            {
                // There is no ret address larger than the function address, take a guess of 0.
                functionEnd = functionAddress + 50;
                return 0;
            }else
            {
                // We just return the number of arguments based on this return address
                functionEnd = ((retAddress)rets[index]).address;
                return (uint) ((retAddress)rets[index]).numParams;
            }
        }

        public List<retAddress> getRets(uint addressStart, uint addressEnd)
        {
            retAddressSelecter selecter = new retAddressSelecter(addressStart, addressEnd);
            List<retAddress> result = rets.FindAll(selecter.isWithinRange);
            result.Sort(comparer);
            return result;
        }
    }
}
