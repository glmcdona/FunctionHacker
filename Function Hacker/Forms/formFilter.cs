using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FunctionHacker.Classes;
using FunctionHacker.Classes.Tabs;
using FunctionHacker.Classes.Visualization;

namespace FunctionHacker.Forms
{
    public enum FILTER_TYPE
    {
        FUNCTION_LIST_FULL,
        FUNCTION_LIST_FILTERED,
        GENERAL
    }

    public partial class formFilter : Form
    {
        private oFunctionList functionList;
        public oFunctionList functionListOutput;
        private bool refreshOutput = false;
        private oTabManager tabManager;
        private oTabFunctionList tab;
        private List<List<int>> moduleAddressRanges;
        private oVisPlayBar playBar;
        private FILTER_TYPE filterType;

        public formFilter(oFunctionList functionList, oTabManager tabManager, oTabFunctionList tab, FILTER_TYPE filterType, oVisPlayBar playBar)
        {
            InitializeComponent();
            this.tabManager = tabManager;
            this.tab = tab;
            this.functionGrid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.functionGrid_CellValueNeeded);
            this.functionList = functionList;
            this.functionListOutput = functionList.Clone();
            updatePredictedOutput();
            timerRefreshOutput.Enabled = false;
            this.playBar = playBar;

            // Make changes to the interface as determined by the filterType
            this.filterType = filterType;
            switch(filterType)
            {
                case FILTER_TYPE.FUNCTION_LIST_FILTERED:
                    // Do nothing special
                    break;
                case FILTER_TYPE.FUNCTION_LIST_FULL:
                    // We cannot replace this list
                    this.buttonApplyReplace.Visible = false;
                    break;
                case FILTER_TYPE.GENERAL:
                    // We do not want to change any tabs, just provide functionListOutput as the result
                    buttonApplyReplace.Visible = false;
                    buttonApplyNew.Text = "Apply";

                    // We cannot clip data to selection
                    this.radioTimeRangeClip.Enabled = false;

                    break;
            }
        }

        


        private void formFilter_Load(object sender, EventArgs e)
        {
            // Populate the module list
            this.textCallCountMax.Text = Properties.Settings.Default.MaxRecordedCalls.ToString();
            if (oProcess.activeProcess != null && oProcess.processStillRunning())
            {
                moduleAddressRanges = new List<List<int>>(0);
                foreach (ProcessModule module in oProcess.activeProcess.Modules)
                {
                    comboModules.Items.Add(module.ModuleName + " 0x" + module.BaseAddress.ToString("X"));
                    moduleAddressRanges.Add(new List<int>(2));
                    moduleAddressRanges[moduleAddressRanges.Count - 1].Add((int) module.BaseAddress);
                    moduleAddressRanges[moduleAddressRanges.Count - 1].Add(module.ModuleMemorySize +
                                                                           (int) module.BaseAddress);
                }
            }
        }

        private void functionGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Request this cell value from the current function list
            if (functionListOutput != null)
            {
                // Request the row and column
                e.Value = functionListOutput.getFunctionListCell(e.RowIndex, e.ColumnIndex, playBar);
            }
        }

        private void updatePredictedOutput()
        {
            // Parse the input parameters
            int minCallCount = getCallCountMin();
            int maxCallCount = getCallCountMax();
            
            // Validate input arguments again
            if (minCallCount < 0 | maxCallCount < 0 && maxCallCount >= minCallCount )
                return;

            // Load the time range selection radial menu
            int timeSelection = (radioTimeRangeAll.Checked ? 1 : 0) + (radioTimeRangeClip.Checked ? 2 : 0);

            // Load the arg filter type selection radial menu
            int argFilterSelection = (radioArgAny.Checked ? 1 : 0) + (radioArg1Byte.Checked ? 2 : 0) +
                                     (radioArg2Byte.Checked ? 3 : 0) + (radioArg4Byte.Checked ? 4 : 0) +
                                     (radioArgFloat.Checked ? 5 : 0);

            // Load the arg string selection radial menu
            int argStringSelection = (radioStringAny.Checked ? 1 : 0) + (radioStringYes.Checked ? 2 : 0) +
                                     (radioStringMatch.Checked ? 3 : 0);

            // Load the arg binary selection radial menu
            int argBinarySelection = (radioBinaryAny.Checked ? 1 : 0) + (radioBinaryPartial.Checked ? 2 : 0) +
                                     (radioBinaryStart.Checked ? 3 : 0);

            // Load the argument count filter
            int argMinCount;
            int argMaxCount;
            if (!getArgRange(out argMinCount, out argMaxCount))
                return;

            // Validate the input of the arg filter selection
            RANGE_PARSE args = null;
            switch(argFilterSelection)
            {
                case 2:
                    args = new RANGE_PARSE_1BYTE(text1Byte);
                    break;
                case 3:
                    args = new RANGE_PARSE_2BYTE(text2Byte);
                    break;
                case 4:
                    args = new RANGE_PARSE_4BYTE(text4Byte);
                    break;
                case 5:
                    args = new RANGE_PARSE_FLOAT(textFloat);
                    break;
            }
            if (args != null && !args.valid)
                return;

            // Create a copy of the original function list
            functionListOutput = functionList.Clone();

            // Perform the data clipping if required
            if( timeSelection == 2 )
            {
                // Clip the data to the selected region

                // Firstly replace the function list output with the selected time series output
                functionListOutput = playBar.getSelectedFunctionList().Clone();

                // Now clip it to the selected range
                functionListOutput.clipCalls_timeRange(playBar.selectStart, playBar.selectEnd);
            }

            // Filter the data based on the source and destination if required
            if( checkAddressFunction.Checked )
            {
                // Filter based on the function address
                RANGE_PARSE_4BYTE range = new RANGE_PARSE_4BYTE(textAddressRangeFunction);
                if( range.valid )
                    functionListOutput.clipCalls_addressDest(range);
            }
            if( checkAddressSource.Checked )
            {
                // Filter based on the call source address
                RANGE_PARSE_4BYTE range = new RANGE_PARSE_4BYTE(textAddressRangeSource);
                if (range.valid)
                    functionListOutput.clipCalls_addressSource(range);
            }

            // Perform the argument count filtering
            functionListOutput.clipCalls_argumentCount(argMinCount,argMaxCount);
            
            // Perform the argument searching
            if( args != null )
                functionListOutput.clipCalls_argument(args);

            // Filter based on the string arguments
            switch(argStringSelection)
            {
                case 1:
                    // Any call
                    break;
                case 2:
                    // Only calls with strings
                    functionListOutput.clipCalls_hasStringArgument();
                    break;
                case 3:
                    // Calls with string match
                    functionListOutput.clipCalls_hasStringArgumentMatch(textStringMatch.Text, checkStringCaseSensitive.Checked);
                    break;
            }

            // Filter based on the binary dereference arguments

            // Validate the data
            byte[] binaryData;
            if (!readByteHexString(textBinaryMatch.Text, out binaryData))
            {
                // Invalid hex string
                textBinaryMatch.BackColor = Color.MediumVioletRed;
            }
            else
            {
                // Valid hex string
                textBinaryMatch.BackColor = Color.White;
            }

            switch (argBinarySelection)
            {
                case 1:
                    // Any call
                    break;
                case 2:
                    // Only calls with a partial binary match
                    functionListOutput.clipCalls_hasBinaryArgumentMatch(binaryData, false);
                    break;
                case 3:
                    // Only calls with a starts-with binary match
                    functionListOutput.clipCalls_hasBinaryArgumentMatch(binaryData, true);
                    break;
            }
            

            // Perform the call type filtering
            if( radioTypeInter.Checked || radioTypeIntra.Checked )
                functionListOutput.clipCalls_type((radioTypeInter.Checked ? 1 : 2));

            

            // Perform the call count filtering and clip the resulting functions
            functionListOutput.clipFunctions_CallCount(minCallCount, maxCallCount);

            // Update the displays
            functionGrid.Rows.Clear();
            functionGrid.RowCount = functionListOutput.getCount();
            functionGrid.Invalidate();
            this.Update();
            labelCallsBefore.Text = string.Concat(functionList.getCallCount().ToString(), " calls before filtering.");
            labelCallsAfter.Text = string.Concat(functionListOutput.getCallCount().ToString(), " calls after filtering.");

            labelFunctionsBefore.Text = string.Concat(functionList.getCount().ToString(), " functions before filtering.");
            labelFunctionsAfter.Text = string.Concat(functionListOutput.getCount().ToString(), " functions after filtering.");
        }

        private int getCallCountMin()
        {
            // Validate the number
            int callCountMin;
            if (int.TryParse(textCallCountMin.Text, out callCountMin) && callCountMin >= 0)
            {
                textCallCountMin.BackColor = Color.White;
                return callCountMin;
            }
            textCallCountMin.BackColor = Color.MediumVioletRed;
            return -1;
        }

        private int getCallCountMax()
        {
            // Validate the number
            int number;
            if (int.TryParse(textCallCountMax.Text, out number) && number >= 0)
            {
                textCallCountMax.BackColor = Color.White;
                return number;
            }
            textCallCountMax.BackColor = Color.MediumVioletRed;
            return -1;
        }

        private bool getArgRange(out int min, out int max)
        {
            if (!int.TryParse(textArgCountMin.Text, out min) || !int.TryParse(textArgCountMax.Text, out max) || min > max )
            {
                min = 0;
                max = 0;
                textArgCountMin.BackColor = Color.MediumVioletRed;
                textArgCountMax.BackColor = Color.MediumVioletRed;
                return false;
            }
            textArgCountMin.BackColor = Color.White;
            textArgCountMax.BackColor = Color.White;
            return true;
        }

        private void textCallCountMin_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = getCallCountMin() >= 0;
            resetTimer();
        }

        private void textCallCountMax_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = getCallCountMax() >= 0;
            resetTimer();
        }

        private void resetTimer()
        {
            timerRefreshOutput.Enabled = false;
            timerRefreshOutput.Enabled = true;
        }

        private void timerRefreshOutput_Tick(object sender, EventArgs e)
        {
            // This timer updates the output periodically, so that it doesn't update every time you type in
            // a new character into a textbox.
            if( refreshOutput )
            {
                // There is a change we need to refresh the output function list
                updatePredictedOutput();
                refreshOutput = false;
            }else
            {
                // Don't update the output
            }
            timerRefreshOutput.Enabled = false;
        }

        private void buttonApplyNew_Click(object sender, EventArgs e)
        {
            // Create a new tab with this list
            updatePredictedOutput();

            // Create a new tab it is not of type GENERAL
            if (filterType != FILTER_TYPE.GENERAL)
            {
                tabManager.addFunctionListTab("Function List: Unnamed", functionListOutput, true, true);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonApplyReplace_Click(object sender, EventArgs e)
        {
            // Replace the current tab list with this list
            updatePredictedOutput();
            tab.SetFunctionList(functionListOutput);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioTimeRangeAll_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioTimeRangeClip_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioTimeRangeSearchSelection_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void text1Byte_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            radioArg1Byte.Checked = true;
            resetTimer();
            new RANGE_PARSE_1BYTE(text1Byte);
        }

        private void text2Byte_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            radioArg2Byte.Checked = true;
            resetTimer();
            new RANGE_PARSE_2BYTE(text2Byte);
        }

        private void text4Byte_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            radioArg4Byte.Checked = true;
            resetTimer();
            new RANGE_PARSE_4BYTE(text4Byte);
        }

        private void textFloat_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            radioArgFloat.Checked = true;
            resetTimer();
            new RANGE_PARSE_FLOAT(textFloat);
        }

        private void radioArg1Byte_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioArg2Byte_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioArg4Byte_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioArgFloat_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioArgAny_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void checkArgCount_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void textArgCountMin_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void textArgCountMax_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioTypeBoth_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioTypeInter_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioTypeIntra_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void checkAddressModule_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void checkAddress_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void comboModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the address ranges of the text boxes
            if( comboModules.SelectedIndex >= 0 && comboModules.SelectedIndex < moduleAddressRanges.Count )
            {
                textAddressRangeFunction.Text = "0x" + moduleAddressRanges[comboModules.SelectedIndex][0].ToString("X") + ":" + "0x" + moduleAddressRanges[comboModules.SelectedIndex][1].ToString("X");
                textAddressRangeSource.Text = "0x" + moduleAddressRanges[comboModules.SelectedIndex][0].ToString("X") + ":" + "0x" + moduleAddressRanges[comboModules.SelectedIndex][1].ToString("X");
            }
        }

        private void textAddressRangeSource_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
            // Validate the data
            new RANGE_PARSE_4BYTE(textAddressRangeSource);
        }

        private void textAddressRangeFunction_TextChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
            // Validate the data
            new RANGE_PARSE_4BYTE(textAddressRangeFunction);
        }

        private void radioStringAny_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioStringYes_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void radioStringMatch_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }

        private void textStringMatch_TextChanged(object sender, EventArgs e)
        {
            radioStringMatch.Checked = true;
            refreshOutput = true;
            resetTimer();
        }

        private void checkStringCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            radioStringMatch.Checked = true;
            refreshOutput = true;
            resetTimer();
        }

        private void textBinaryMatch_TextChanged(object sender, EventArgs e)
        {
            if( radioBinaryAny.Checked )
                radioBinaryPartial.Checked = true;
            refreshOutput = true;
            resetTimer();
        }

        private void radioBinaryPartial_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }
        
        private bool readByteHexString(string dataString, out byte[] data)
        {
            // This attempts to read in the data as a byte hex string

            // Remove extra characters
            dataString = dataString.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper();

            // Check that there are an even number of characters
            if ((dataString.Length % 2) > 0)
            {
                data = new byte[0];
                return false;
            }
            data = new byte[dataString.Length/2];

            // Begin inputting the bytes
            for( int i = 0; i < dataString.Length; i += 2)
            {
                byte singleByte = 0;
                if( !byte.TryParse(dataString.Substring(i,2),NumberStyles.HexNumber, null, out singleByte) )
                {
                    // Not a valid hex string
                    data = new byte[0];
                    return false;
                }
                data[i/2] = singleByte;
            }

            // Successful
            return true;
        }

        private void radioBinaryStart_CheckedChanged(object sender, EventArgs e)
        {
            refreshOutput = true;
            resetTimer();
        }
    }



    public class RANGE_PARSE
    {
        public bool valid;
        public TextBox control;
        public bool isDouble;
        public object firstValue;
        public object secondValue;

        /// <summary>
        /// Interpret the text of this control, and set it's valid or invalid background colour.
        /// </summary>
        /// <param name="control"></param>
        public RANGE_PARSE(TextBox control)
        {
            // Initialize
            this.control = control;
            if (control.Text.Length == 0)
            {
                invalidate();
            }
        }

        public RANGE_PARSE(object firstValue, object secondValue)
        {
            this.firstValue = firstValue;
            this.secondValue = secondValue;
            valid = true;
        }

        public RANGE_PARSE(object value)
        {
            this.firstValue = value;
            this.secondValue = value;
            valid = true;
        }

        public void invalidate()
        {
            valid = false;
            if (control != null)
                control.BackColor = Color.MediumVioletRed;
        }

        public void validate()
        {
            valid = true;
            if (control != null)
                control.BackColor = Color.White;
        }
    }

    public class RANGE_PARSE_4BYTE : RANGE_PARSE
    {

        public RANGE_PARSE_4BYTE(uint firstValue, uint secondValue)
            : base(firstValue, secondValue)
        {
        }

        public RANGE_PARSE_4BYTE(uint value)
            : base(value)
        {

        }

        /// <summary>
        /// Interpret the text of this control, and set it's valid or invalid background colour.
        /// </summary>
        /// <param name="control"></param>
        public RANGE_PARSE_4BYTE(TextBox control)
            : base(control)
        {
            // Initialize
            this.control = control;
            if (control.Text.Length == 0)
            {
                invalidate();
            }
            else
            {
                // Validate the text

                // Split the text according to the dash delimiter
                string[] delimited = control.Text.Split(':');
                if (delimited.Length > 2 || delimited.Length < 1)
                {
                    invalidate();
                }
                else
                {
                    // Update the number of parameters
                    int numParams = delimited.Length == 1 ? 1 : 2;

                    // Validate the parameters
                    if (!interpretArgString(delimited[0], out firstValue))
                    {
                        invalidate();
                    }
                    else
                    {
                        if (numParams == 2 && !interpretArgString(delimited[1], out secondValue))
                        {
                            invalidate();
                        }
                        else
                        {
                            // Everything inputted properly
                            if (numParams == 1)
                                secondValue = firstValue;
                            if ((uint)secondValue >= (uint)firstValue)
                            {
                                validate();
                            }
                            else
                            {
                                invalidate();
                            }
                        }
                    }

                }
            }
        }

        private bool interpretArgString(string arg, out object result)
        {
            // Check for and remove hex indicators
            uint tmpResult;
            bool success;
            if (arg.Contains("h") || arg.Contains("0x"))
            {
                // Hex number
                arg = arg.Replace("h", "").Replace("0x", "");

                // Try to interpret the number as hex
                success = uint.TryParse(arg, NumberStyles.HexNumber, null, out tmpResult);
                result = (object)tmpResult;
                return success;
            }

            // Integer number

            // Try to interpret the number as an integer
            success = uint.TryParse(arg, out tmpResult);
            result = (object)tmpResult;
            return success;
        }
    }

    public class RANGE_PARSE_2BYTE : RANGE_PARSE
    {

        /// <summary>
        /// Interpret the text of this control, and set it's valid or invalid background colour.
        /// </summary>
        /// <param name="control"></param>
        public RANGE_PARSE_2BYTE(TextBox control)
            : base(control)
        {
            // Initialize
            this.control = control;
            if (control.Text.Length == 0)
            {
                invalidate();
            }
            else
            {
                // Validate the text

                // Split the text according to the dash delimiter
                string[] delimited = control.Text.Split(':');
                if (delimited.Length > 2 || delimited.Length < 1)
                {
                    invalidate();
                }
                else
                {
                    // Update the number of parameters
                    int numParams = delimited.Length == 1 ? 1 : 2;

                    // Validate the parameters
                    if (!interpretArgString(delimited[0], out firstValue))
                    {
                        invalidate();
                    }
                    else
                    {
                        if (numParams == 2 && !interpretArgString(delimited[1], out secondValue))
                        {
                            invalidate();
                        }
                        else
                        {
                            // Everything inputted properly!
                            if (numParams == 1)
                                secondValue = firstValue;
                            if ((short)secondValue >= (short)firstValue)
                            {
                                validate();
                            }
                            else
                            {
                                invalidate();
                            }
                        }
                    }

                }
            }
        }

        private bool interpretArgString(string arg, out object result)
        {
            // Check for and remove hex indicators
            short tmpResult;
            bool success;
            if (arg.Contains("h") || arg.Contains("0x"))
            {
                // Hex number
                arg = arg.Replace("h", "").Replace("0x", "");

                // Try to interpret the number as hex
                success = short.TryParse(arg, NumberStyles.HexNumber, null, out tmpResult);
                result = (object)tmpResult;
                return success;
            }

            // Integer number

            // Try to interpret the number as an integer
            success = short.TryParse(arg, out tmpResult);
            result = (object)tmpResult;
            return success;
        }
    }

    public class RANGE_PARSE_1BYTE : RANGE_PARSE
    {


        /// <summary>
        /// Interpret the text of this control, and set it's valid or invalid background colour.
        /// </summary>
        /// <param name="control"></param>
        public RANGE_PARSE_1BYTE(TextBox control)
            : base(control)
        {
            // Initialize
            this.control = control;
            if (control.Text.Length == 0)
            {
                invalidate();
            }
            else
            {
                // Validate the text

                // Split the text according to the dash delimiter
                string[] delimited = control.Text.Split(':');
                if (delimited.Length > 2 || delimited.Length < 1)
                {
                    invalidate();
                }
                else
                {
                    // Update the number of parameters
                    int numParams = delimited.Length == 1 ? 1 : 2;

                    // Validate the parameters
                    if (!interpretArgString(delimited[0], out firstValue))
                    {
                        invalidate();
                    }
                    else
                    {
                        if (numParams == 2 && !interpretArgString(delimited[1], out secondValue))
                        {
                            invalidate();
                        }
                        else
                        {
                            // Everything inputted properly!
                            if (numParams == 1)
                                secondValue = firstValue;
                            if ((byte)secondValue >= (byte)firstValue)
                            {
                                validate();
                            }
                            else
                            {
                                invalidate();
                            }
                        }
                    }

                }
            }
        }

        private bool interpretArgString(string arg, out object result)
        {
            // Check for and remove hex indicators
            byte tmpResult;
            bool success;
            if (arg.Contains("h") || arg.Contains("0x"))
            {
                // Hex number
                arg = arg.Replace("h", "").Replace("0x", "");

                // Try to interpret the number as hex
                success = byte.TryParse(arg, NumberStyles.HexNumber, null, out tmpResult);
                result = (object)tmpResult;
                return success;
            }

            // Check if the argument contains a character indicator
            if (arg.Contains("'") || arg.Contains("\""))
            {
                arg = arg.Replace("'", "").Replace("\"", "");
                if (arg.Length == 1)
                {
                    result = (object)(byte)arg.ToCharArray()[0];
                    return true;
                }
                result = null;
                return false;
            }

            // Integer number

            // Try to interpret the number as an integer
            success = byte.TryParse(arg, out tmpResult);
            result = (object)tmpResult;
            return success;
        }
    }


    public class RANGE_PARSE_FLOAT : RANGE_PARSE
    {

        /// <summary>
        /// Interpret the text of this control, and set it's valid or invalid background colour.
        /// </summary>
        /// <param name="control"></param>
        public RANGE_PARSE_FLOAT(TextBox control)
            : base(control)
        {
            // Initialize
            this.control = control;
            if (control.Text.Length == 0)
            {
                invalidate();
            }
            else
            {
                // Validate the text

                // Split the text according to the dash delimiter
                string[] delimited = control.Text.Split(':');
                if (delimited.Length > 2 || delimited.Length < 1)
                {
                    invalidate();
                }
                else
                {
                    // Update the number of parameters
                    int numParams = delimited.Length == 1 ? 1 : 2;

                    // Validate the parameters
                    if (!interpretArgString(delimited[0], out firstValue))
                    {
                        invalidate();
                    }
                    else
                    {
                        if (numParams == 2 && !interpretArgString(delimited[1], out secondValue))
                        {
                            invalidate();
                        }
                        else
                        {
                            // Everything inputted properly!
                            if (numParams == 1)
                            {
                                secondValue = firstValue;
                                firstValue = (double)firstValue - 0.0001 * (double)firstValue;
                                secondValue = (double)secondValue + 0.0001 * (double)secondValue;
                            }
                            if ((double)secondValue >= (double)firstValue)
                            {
                                validate();
                            }
                            else
                            {
                                invalidate();
                            }
                        }
                    }

                }
            }
        }

        private bool interpretArgString(string arg, out object result)
        {
            // Try to interpret the number as an integer
            double tmpResult;
            bool success = double.TryParse(arg, out tmpResult);
            result = (object)tmpResult;
            return success;
        }
    }
}
