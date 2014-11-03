using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FunctionHacker.Classes.Disassembly
{
    public enum TARGET_ASM_MODE
    {
        x86,
        x64
    }

    public enum OPCODE
    {

    }

    public struct INSTRUCTION
    {
        OPCODE opcode;
        string xrm;
        string sib;
        string diso;
        string imm;
        string arg2;
    }

    public class preparsedCode
    {
        List<string> compiledLines;
        List<string> originalLines;
        Hashtable labelToLine;
        Hashtable lineToOffset;

        public preparsedCode(List<string> lines)
        {
            // Initialize
            this.originalLines = lines;
            this.compiledLines = new List<string>(lines);
            labelToLine = new Hashtable(10);
            lineToOffset = new Hashtable(300);

            // Remove comments from the lines
            removeComments(ref lines);

            // Loop through the lines, removing the labels and filling out the hashtables
            for (int i = 0; i < compiledLines.Count(); i++)
            {
                if (compiledLines[i].Contains(':'))
                {
                    // This contains a label. Add this reference.
                    string label = compiledLines[i].Substring(0, compiledLines[i].IndexOf(':'));
                    labelToLine.Add(label, i);
                    lines[i] = "";
                }
            }
        }

        private void compile(TARGET_ASM_MODE targetMode, Hashtable variables, UInt64 baseAddress)
        {
            // Now we have a base address and variable list, so lets compile the code.

            // Loop through the lines, compiling each line
            for (int i = 0; i < compiledLines.Count(); i++)
            {

            }
        }

        private INSTRUCTION split_command(string line)
        {
            return new INSTRUCTION();
        }

        private void removeComments(ref List<string> lines)
        {
            // Parse out the comments from each line
            for (int i = 0; i < lines.Count(); i++)
            {
                // Find comment start
                int index = lines[i].IndexOf("//");
                if (index >= 0)
                {
                    // Remove comment
                    lines[i] = lines[i].Substring(0, index);
                }
            }
        }
    }

    // The script environment provides the compilation of assembly text into
    // either 32 bit or 64 bit binary. It also allows for advanced features such as
    // variable definitions, labels, offset calculation, and library calling.
    public class script
    {
        static string test = "mov eax, 0x10\n" +
                             "mov [eax], 0x90" +
                             "before:\n" +
                             "call before\n";

        public byte[] compile(string code)
        {
            //string[] lines = preparseLabels(code.Split(new char[] { '\n' }));


            byte[] result = new byte[0];
            return result;
        }
    }
}
