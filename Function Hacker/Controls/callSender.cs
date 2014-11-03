using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Classes;

namespace FunctionHacker.Controls
{
    public partial class callSender : UserControl
    {
        

        private oFunctionList recordedData = null;
        private uint address = 0;
        private string code = "";
        public oFunction function = null;

        public callSender(oFunctionList recordedData)
        {
            this.recordedData = recordedData;
            
            InitializeComponent();

            if( recordedData != null )
            {
                // Set the call list count
                this.dataGridCalls.RowCount = recordedData.getCallCount();

                // Make sure only 1 function is in the function list.
                if( recordedData.getCount() == 1 )
                {
                    // Set this function address
                    this.address = recordedData.functions[0].address;

                    // Set this function
                    this.function = recordedData.functions[0];

                    // Initialize the arguments data grid view control                
                    this.dataGridArguments.Rows.Add(new string[] { "ecx", "0x0" });
                    this.dataGridArguments.Rows.Add(new string[] { "edx", "0x0" });
                    this.dataGridArguments.Rows.Add(new string[] { "eax", "0x0" });
                    for (int i = 0; i < recordedData.functions[0].getNumParams(); i++)
                    {
                        // Add this argument
                        this.dataGridArguments.Rows.Add(new string[] {"ebp+0x" + (8+i*4).ToString("X"), "0x0"});
                    }

                    // Load the initial arguments from the most recent function call
                    if (recordedData.getCallCount() > 0)
                    {
                        // Set the register inputs
                        this.dataGridArguments.Rows[0].Cells[1].Value = "0x" + recordedData.getData()[recordedData.getCallCount() - 1].ecx.ToString("X");
                        this.dataGridArguments.Rows[1].Cells[1].Value = "0x" + recordedData.getData()[recordedData.getCallCount() - 1].edx.ToString("X");
                        this.dataGridArguments.Rows[2].Cells[1].Value = "0x" + recordedData.getData()[recordedData.getCallCount() - 1].eax.ToString("X");

                        // Set the stack inputs
                        for (int i = 0; i < recordedData.functions[0].getNumParams(); i++)
                        {
                            // Set this argument
                            this.dataGridArguments.Rows[i+3].Cells[1].Value = "0x" + recordedData.getData()[recordedData.getCallCount() - 1].arguments[i].ToString("X");
                        }
                    }

                }
            }


            // Initialize the code text
            textCallAssembly.Text = generateCode();
        }

        private string generateCode()
        {
            if (function == null)
                return "";

            // Generate the code to send the specified function call
            string completeCode = "";

            // Get the argument values
            int index = 0;
            string ecx = "";
            string edx = "";
            string eax = "";
            List<string> stackArgs = new List<string>((int)function.getNumParams());

            // Read in ecx
            if (index < dataGridArguments.Rows.Count)
            {
                if (!interpretArgString((string)dataGridArguments.Rows[index].Cells[1].Value, out ecx))
                    dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                else
                    dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.White;
            }
            index++;

            // Read in edx
            if (index < dataGridArguments.Rows.Count)
            {
                if (!interpretArgString((string)dataGridArguments.Rows[index].Cells[1].Value, out edx))
                    dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                else
                    dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.White;
            }
            index++;

            // Read in eax
            if (index < dataGridArguments.Rows.Count)
            {
                if (!interpretArgString((string)dataGridArguments.Rows[index].Cells[1].Value, out eax))
                    dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                else
                    dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.White;
            }
            index++;

            // Read in the stack arguments
            for (int i = 0; i < function.getNumParams(); i++)
            {
                if (index < dataGridArguments.Rows.Count)
                {
                    string result = "";
                    if (!interpretArgString((string)dataGridArguments.Rows[index].Cells[1].Value, out result))
                        dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                    else
                        dataGridArguments.Rows[index].DefaultCellStyle.BackColor = Color.White;
                    stackArgs.Add(result);
                }
                index++;
            }

            // Generate the code
            return function.generateThreadCallCode(ecx, edx, eax, stackArgs);
        }


        private bool interpretArgString(string value, out string resultText)
        {
            if (value == null)
            {
                resultText = oMemoryFunctions.IntToDwordString(0);
                return false;
            }

            // Check for and remove hex indicators
            int tmpResult;
            if (value.EndsWith("h") || value.StartsWith("0x"))
            {
                // Hex number
                value = value.Replace("h", "").Replace("0x", "");

                // Try to interpret the number as hex
                if (int.TryParse(value, NumberStyles.HexNumber, null, out tmpResult))
                {
                    // Create the assembly
                    resultText = oMemoryFunctions.IntToDwordString((uint)tmpResult);
                    return true;
                }

                // Failed to parse. Use a value of 0x0.
                resultText = oMemoryFunctions.IntToDwordString(0);
                return false;
            }

            // Check if the argument contains an ascii string pointer
            if (value.Contains("'"))
            {
                // Check if it is a valid ascii string
                string[] split = value.Split(new char[] {'\''});
                if( split.Length == 3 )
                {
                    // Matching quotes
                    resultText = "'" + split[1] + "'";

                    // Replace escape sequences
                    resultText.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");

                    return true;
                }

                // Invalid character, use a value of 0x0.
                resultText = oMemoryFunctions.IntToDwordString(0);
                return false;
            }

            // Check if the argument contains an unicode string pointer
            if (value.Contains("\""))
            {
                // Check if it is a valid unicode string
                string[] split = value.Split(new char[] { '\"' });
                if (split.Length == 3)
                {
                    // Matching quotes
                    resultText = "\"" + split[1] + "\"";

                    // Replace escape sequences
                    resultText.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");

                    return true;
                }

                // Invalid character, use a value of 0x0.
                resultText = oMemoryFunctions.IntToDwordString(0);
                return false;
            }

            // Check if the argument contains an binary array pointer
            if (value.Contains("[") && value.Contains("]"))
            {
                // Check if it is a valid unicode string
                string[] split = value.Split(new char[] { '[', ']' });
                if (split.Length == 3)
                {
                    // Matching quotes, process the binary
                    string hex = split[1].Replace(" ", "");
                    if( (hex.Count() % 2) == 0)
                    {
                        // Even number of characters, good
                        if( oMemoryFunctions.IsValidHex(hex) )
                        {
                            // Valid hex characters
                            resultText = "[" + split[1] + "]";
                            return true;
                        }
                    }
                }

                // Invalid character, use a value of 0x0.
                resultText = oMemoryFunctions.IntToDwordString(0);
                return false;
            }

            // Integer number

            // Try to interpret the number as a base 10 integer
            if( int.TryParse(value, out
                tmpResult) )
            {
                resultText = oMemoryFunctions.IntToDwordString((uint)tmpResult);
                return true;
            }
            // Failed to parse, use a value of 0x0
            resultText = oMemoryFunctions.IntToDwordString(0);
            return false;
        }


        private void dataGridArguments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            textCallAssembly.Text = generateCode();
        }


        private void callSender_Load(object sender, EventArgs e)
        {

        }

        private void dataGridCalls_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Request this cell value fromt he current function list
            if (recordedData != null)
            {
                // Request the call info at row and column
                e.Value = recordedData.getCallListCell(e.RowIndex, e.ColumnIndex, dataGridCalls.Columns[e.ColumnIndex].Width, dataGridCalls.Columns[e.ColumnIndex].DefaultCellStyle.Font);
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Parse the delay interval
            double interval = 0;
            if (double.TryParse(comboDelay.Text, out interval) && interval >= 0 && interval <= 1000)
            {
                comboDelay.BackColor = Color.White;

                if (interval > 0)
                {
                    // Create a timer
                    System.Timers.Timer aTimer = new System.Timers.Timer();
                    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    aTimer.Interval = interval * 1000;
                    aTimer.Enabled = true;
                }
                else
                {
                    // Send the call
                    sendCall();
                }
            }
            else
            {
                comboDelay.BackColor = Color.Red;
            }
        }

        private void sendCall()
        {
            // This builds the code, injects it, and creates a thread

            // Verify the process is still running
            if (!oProcess.processStillRunning())
            {
                MessageBox.Show("ERROR: Process is no longer running.");
                return;
            }

            // Compile the code
            string codeProcessed = oAssemblyGenerator.processLabels(oAssemblyGenerator.replaceCommands(code.Replace("\r", " ").Replace("\n", " ")));

            // Allocate space for the code
            IntPtr address = oMemoryFunctions.VirtualAllocEx(oProcess.activeProcess.Handle, (IntPtr)0, codeProcessed.Length,
                                            (uint)(MEMORY_STATE.COMMIT),
                                            (uint)MEMORY_PROTECT.PAGE_EXECUTE_READWRITE);

            byte[] injection = oAssemblyGenerator.buildInjection((ulong)address, codeProcessed, oProcess.activeProcess, (uint)this.address, 0);

            // Inject the code
            oMemoryFunctions.WriteMemoryByteArray(oProcess.activeProcess, (ulong)address, injection);

            // Create the remote thread
            oMemoryFunctions.CreateThread(oProcess.activeProcess, (ulong)address);

            // Beep
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // Send the call
            sendCall();
            ((System.Timers.Timer) sender).Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textCallAssembly_TextChanged(object sender, EventArgs e)
        {
            code = textCallAssembly.Text;
        }

        private void dataGridCalls_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridCalls.AutoResizeRow(e.RowIndex);
        }
    }
}
