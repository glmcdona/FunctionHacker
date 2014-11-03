using System;
using System.Windows.Forms;
using Be.Windows.Forms;
using FunctionHacker.Classes;

namespace FunctionHacker.Controls
{
    public partial class MemoryViewer : UserControl
    {
        private readonly ulong memoryAddress;
        private readonly uint blockLength;
        private byte[] memoryBlock;
        private bool inRead;

        public MemoryViewer(ulong address, uint length)
        {

            try
            {
                memoryAddress = address;
                blockLength = length;
                InitializeComponent();
                InitialiseBuffer();
                DynamicByteProvider dynamicByteProvider = new DynamicByteProvider(memoryBlock);
                hexBox.ByteProvider = dynamicByteProvider;
                BuildStringRepresentation(16);
            }
            catch
            {
                //textBoxMemory.Text = @"Can not read memory on adress: 0x" + address.ToString("X8");
            }
        }

        private void BuildStringRepresentation(int lengtPerLine)
        {
            hexBox.BytesPerLine = lengtPerLine;
        }

        private void InitialiseBuffer()
        {
            inRead = false;
            if (inRead) return;
            inRead = true;
            if (oProcess.activeProcess != null)
            {
                ulong realMemoryAddress = (ulong)oProcess.activeProcess.MainModule.BaseAddress + memoryAddress;
                memoryBlock = oMemoryFunctions.ReadMemory(oProcess.activeProcess, realMemoryAddress, blockLength);
            }
            inRead = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            BuildStringRepresentation((int) numericUpDown1.Value);
        }

        private void buttonRefresh(object sender, EventArgs e)
        {
            InitialiseBuffer();
            long currentPos = (hexBox.CurrentLine - 1) * hexBox.BytesPerLine + hexBox.CurrentPositionInLine - 1;
            long selectionStart = hexBox.SelectionStart;
            long selectionLength = hexBox.SelectionLength;
            hexBox.SuspendLayout();
            hexBox.ByteProvider = new DynamicByteProvider(memoryBlock);
            if (selectionStart == 0)
                hexBox.Select(currentPos, 1);
            else
            {
                hexBox.SelectionStart = selectionStart;
                hexBox.SelectionLength = selectionLength;
            }
            hexBox.ResumeLayout();
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            byte[] ascii = System.Text.Encoding.ASCII.GetBytes(textBox2.Text);
            if(ascii.Length > 0)
                hexBox.Find(ascii, 0);
        }
    }
}
