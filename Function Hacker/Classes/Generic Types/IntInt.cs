using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionHacker.Classes.Generic_Types
{
    public class IntInt
    {
        public int int1;
        public int int2;

        public IntInt(int int1, int int2)
        {
            this.int1 = int1;
            this.int2 = int2;
        }

        public class IntIntComparer : IComparer<IntInt>
        {
            int IComparer<IntInt>.Compare(IntInt a, IntInt b)
            {
                return a.int1.CompareTo(b.int1);
            }
        }
    }
}
