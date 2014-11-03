using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionHacker.Classes.Disassembly
{
    public class jmpInstruction
    {
        public uint address;
        public uint destination;

        public jmpInstruction(uint address, uint destination)
        {
            this.address = address;
            this.destination = destination;
        }
    }

    public class jmpInstructionComparer : IComparer<jmpInstruction>
    {
        int IComparer<jmpInstruction>.Compare(jmpInstruction a, jmpInstruction b)
        {
            return a.address.CompareTo(b.address);
        }
    }
}
