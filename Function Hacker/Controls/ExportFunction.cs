using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Classes;

namespace FunctionHacker.Controls
{
    public enum LANGUAGE
    {
        C_SHARP,
        C_PLUS_PLUS,
        DELPHI,
        VB_NET
    }

    public partial class ExportFunction : UserControl
    {
        private LANGUAGE language;
        private oFunction function;

        public ExportFunction(oFunction function)
        {
            this.function = function;
            InitializeComponent();
        }


        private void ExportFunction_Load(object sender, EventArgs e)
        {
            this.richTextCode.Text = replaceConstants(txtCSharpCode.Text);
            updateSyntaxHighlighting();
        }

        private string replaceConstants(string code)
        {
            // This replaces all the # # constants with their appropriate values.
            // List of constants to replace iin this code:
            // #MODULENAME#       - Module name containing the function
            // #FUNCTIONOFFSET#   - Offset of the function
            // #FUNCTIONNAME#     - Name of the function. eg. kernel32_4a029
            // #NUMSTACKARGS#     - Number of arguments passed on the stack
            // #MAINMODULENAME#   - The name of the main module of the process
            // #CALLCODE#         - The binary responsible for setting up the stack and registers in the other process.
            // #PROCESSNAME#      - The name of the process
            //
            // List of constants that the C#, delphi, vb, etc is expected to replace in the built ASM string, #CALLCODE#:
            // #FUNCTION#         - This is the address of the function. It varies according to the loading module base.
            // #_ECX__#           - With arugment value
            // #_EDX__#           - With arugment value
            // #_EAX__#           - With arugment value
            // #ARGN01#           - With arugment value
            // #ARGN02#           - With arugment value
            // #ARGN03#           - With arugment value
            // #ARGN04#           - With arugment value
            // ...


            // Initialize the result
            string result = code;
            HEAP_INFO heap = oMemoryFunctions.LookupAddressInMap(oProcess.map, function.address);

            if (heap.heapAddress != 0)
            {
                // Replace the process name
                result = result.Replace("#PROCESSNAME#", oProcess.activeProcess.ProcessName);

                // Replace the module name
                result = result.Replace("#MODULENAME#", heap.associatedModule.ModuleName);

                // Replace the function offset
                if (heap.associatedModule != null)
                    result = result.Replace("#FUNCTIONOFFSET#", "0x" + (function.address - (uint)heap.associatedModule.BaseAddress).ToString("X"));
                else
                    result = result.Replace("#FUNCTIONOFFSET#", "0x" + (function.address - heap.heapAddress + 0x1000).ToString("X"));

                // Replace the function name
                string name = "";
                if( heap.associatedModule != null )
                    name = heap.associatedModule.ModuleName + "_" + (function.address - (uint) heap.associatedModule.BaseAddress).ToString("X");
                else
                    name = heap.associatedModule.ModuleName + "_" + (function.address - heap.heapAddress + 0x1000).ToString("X");
                    
                name = name.Replace(".", "_");
                for(int i = 0; i < name.Length; i ++)
                {
                    if( (name[i] < 48 || name[i] > 57) && (name[i] < 65 || name[i] > 90) && (name[i] < 97 || name[i] > 122) && name[i] != 95 )
                    {
                        // Invalid character for a function name, remove it
                        name = name.Remove(i, 1);
                        i--;
                    }
                }
                // Cannot be 0 length and cannot start with a number
                if( name.Length <= 0 || (name[0] >= 48 && name[0] <= 57)  )
                    name = "f_" + name;
                result = result.Replace("#FUNCTIONNAME#", name);

                // Replace the number of stack arguments
                result = result.Replace("#NUMSTACKARGS#", function.getNumParams().ToString());

                // Generate and replace the call code
                List<string> stackArgNames = new List<string>((int)function.getNumParams());
                for (int i = 1; i <= function.getNumParams(); i++)
                {
                    string argNum = i.ToString();
                    while( argNum.Length < 2 )
                        argNum = "0" + argNum;
                    stackArgNames.Add("#ARGN" + argNum + "#");
                }
                string asmCode = oAssemblyGenerator.processLabels(oAssemblyGenerator.replaceCommands(
                        function.generateThreadCallCode("#_ECX__#", "#_EDX__#", "#_EAX__#", stackArgNames).Replace("\n"," ").Replace("\r","")));
                asmCode = oAssemblyGenerator.buildInjectionStringOnly(0, asmCode.Replace("<destination>","#FUNCTION#"), oProcess.activeProcess,
                                                                      function.address, 0);
                result = result.Replace("#CALLCODE#", asmCode);
            }

            return result;
        }



        // Syntax highlighting
        public readonly string[] blueHighlightsCSharp = { "return", "namespace", "extern", "IntPtr", "static", "using", "enum", "string", "int", "uint", "private", "public", "class", "if", "else", "this", "void", "Process", "foreach", "for", "while", "byte", "null" };

        private void updateSyntaxHighlighting()
        {
            // Save the current selection
            int startSelect = richTextCode.SelectionStart;
            int startSelectLength = richTextCode.SelectionLength;
            richTextCode.SuspendLayout();
            richTextCode.Visible = false;
            

            // Highlight all the text black
            richTextCode.Select(0, richTextCode.Text.Length);
            richTextCode.SelectionColor = Color.Black;

            // Select the keywords and comment to use
            string comment = "//";
            string[] keywords = new string[0];
            switch (language)
            {
                case LANGUAGE.C_SHARP:
                    comment = "//";
                    keywords = blueHighlightsCSharp;
                    break;
            }

            // Highlight keywords with blue highlighting
            int index = 0;
            foreach (string keyword in keywords)
            {
                index = 0;
                index = richTextCode.Find(keyword, index, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
                while (index >= 0)
                {
                    // Highlight this find result
                    richTextCode.Select(index, keyword.Length);
                    richTextCode.SelectionColor = Color.Blue;

                    // Find the next highlight
                    index = richTextCode.Find(keyword, index + 1, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
                }
            }

            // Highlight comments
            index = richTextCode.Find(comment, 0, RichTextBoxFinds.None);
            while (index >= 0)
            {
                // Find the end of this line
                int lineEnd = richTextCode.Text.IndexOf("\n", index);
                if (lineEnd < 0)
                    lineEnd = richTextCode.Text.Length;

                // Highlight this find result
                richTextCode.Select(index, lineEnd - index + 1);
                richTextCode.SelectionColor = Color.Green;

                // Find the next highlight
                index = richTextCode.Find(comment, index + 1, RichTextBoxFinds.None);
            }

            // Restore the selection
            richTextCode.SelectionStart = startSelect;
            richTextCode.SelectionLength = startSelectLength;
            richTextCode.Visible = true;
            richTextCode.ResumeLayout();
        }

        private void richTextCode_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
