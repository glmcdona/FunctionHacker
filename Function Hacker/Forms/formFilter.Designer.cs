namespace FunctionHacker.Forms
{
    partial class formFilter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioTypeIntra = new System.Windows.Forms.RadioButton();
            this.radioTypeInter = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.radioTypeBoth = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioTimeRangeClip = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.radioTimeRangeAll = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textCallCountMax = new System.Windows.Forms.TextBox();
            this.textCallCountMin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textArgCountMax = new System.Windows.Forms.TextBox();
            this.textArgCountMin = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.text1Byte = new System.Windows.Forms.TextBox();
            this.radioArg1Byte = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.text2Byte = new System.Windows.Forms.TextBox();
            this.radioArg2Byte = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.textFloat = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.text4Byte = new System.Windows.Forms.TextBox();
            this.radioArgFloat = new System.Windows.Forms.RadioButton();
            this.radioArg4Byte = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.radioArgAny = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.checkStringCaseSensitive = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textStringMatch = new System.Windows.Forms.TextBox();
            this.radioStringMatch = new System.Windows.Forms.RadioButton();
            this.radioStringYes = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.radioStringAny = new System.Windows.Forms.RadioButton();
            this.buttonApplyReplace = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApplyNew = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.textAddressRangeFunction = new System.Windows.Forms.TextBox();
            this.textAddressRangeSource = new System.Windows.Forms.TextBox();
            this.checkAddressSource = new System.Windows.Forms.CheckBox();
            this.checkAddressFunction = new System.Windows.Forms.CheckBox();
            this.comboModules = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.labelCallsBefore = new System.Windows.Forms.Label();
            this.labelCallsAfter = new System.Windows.Forms.Label();
            this.labelFunctionsAfter = new System.Windows.Forms.Label();
            this.labelFunctionsBefore = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.functionGrid = new System.Windows.Forms.DataGridView();
            this.colFunction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExecutionCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCountSelection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerRefreshOutput = new System.Windows.Forms.Timer(this.components);
            this.panel9 = new System.Windows.Forms.Panel();
            this.radioBinaryStart = new System.Windows.Forms.RadioButton();
            this.textBinaryMatch = new System.Windows.Forms.TextBox();
            this.radioBinaryPartial = new System.Windows.Forms.RadioButton();
            this.label21 = new System.Windows.Forms.Label();
            this.radioBinaryAny = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.functionGrid)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(406, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 62;
            this.label1.Text = "RESULT PREVIEW";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Snow;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioTypeIntra);
            this.panel1.Controls.Add(this.radioTypeInter);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.radioTypeBoth);
            this.panel1.Location = new System.Drawing.Point(12, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(389, 26);
            this.panel1.TabIndex = 65;
            // 
            // radioTypeIntra
            // 
            this.radioTypeIntra.AutoSize = true;
            this.radioTypeIntra.Location = new System.Drawing.Point(261, 3);
            this.radioTypeIntra.Name = "radioTypeIntra";
            this.radioTypeIntra.Size = new System.Drawing.Size(86, 17);
            this.radioTypeIntra.TabIndex = 68;
            this.radioTypeIntra.Tag = "Calls within the modules. This means functions calls internal to the code (not be" +
                "tween different dlls).";
            this.radioTypeIntra.Text = "Intra-modular";
            this.radioTypeIntra.UseVisualStyleBackColor = true;
            this.radioTypeIntra.CheckedChanged += new System.EventHandler(this.radioTypeIntra_CheckedChanged);
            // 
            // radioTypeInter
            // 
            this.radioTypeInter.AutoSize = true;
            this.radioTypeInter.Location = new System.Drawing.Point(158, 3);
            this.radioTypeInter.Name = "radioTypeInter";
            this.radioTypeInter.Size = new System.Drawing.Size(86, 17);
            this.radioTypeInter.TabIndex = 67;
            this.radioTypeInter.Tag = "Calls between different modules, meaning calls to and between dll\'s.";
            this.radioTypeInter.Text = "Inter-modular";
            this.radioTypeInter.UseVisualStyleBackColor = true;
            this.radioTypeInter.CheckedChanged += new System.EventHandler(this.radioTypeInter_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 66;
            this.label3.Text = "Call Type:";
            // 
            // radioTypeBoth
            // 
            this.radioTypeBoth.AutoSize = true;
            this.radioTypeBoth.Checked = true;
            this.radioTypeBoth.Location = new System.Drawing.Point(94, 3);
            this.radioTypeBoth.Name = "radioTypeBoth";
            this.radioTypeBoth.Size = new System.Drawing.Size(47, 17);
            this.radioTypeBoth.TabIndex = 0;
            this.radioTypeBoth.TabStop = true;
            this.radioTypeBoth.Tag = "Will include both types of function calls in the result.";
            this.radioTypeBoth.Text = "Both";
            this.radioTypeBoth.UseVisualStyleBackColor = true;
            this.radioTypeBoth.CheckedChanged += new System.EventHandler(this.radioTypeBoth_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Snow;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.radioTimeRangeClip);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.radioTimeRangeAll);
            this.panel2.Location = new System.Drawing.Point(12, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(389, 26);
            this.panel2.TabIndex = 66;
            // 
            // radioTimeRangeClip
            // 
            this.radioTimeRangeClip.AutoSize = true;
            this.radioTimeRangeClip.Location = new System.Drawing.Point(158, 3);
            this.radioTimeRangeClip.Name = "radioTimeRangeClip";
            this.radioTimeRangeClip.Size = new System.Drawing.Size(153, 17);
            this.radioTimeRangeClip.TabIndex = 67;
            this.radioTimeRangeClip.Tag = "Clips the data to the selected region. Select a region by holding down right clic" +
                "k on the time-bar at the bottom of the main form.";
            this.radioTimeRangeClip.Text = "Clip data to selected region";
            this.radioTimeRangeClip.UseVisualStyleBackColor = true;
            this.radioTimeRangeClip.CheckedChanged += new System.EventHandler(this.radioTimeRangeClip_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Time Range:";
            // 
            // radioTimeRangeAll
            // 
            this.radioTimeRangeAll.AutoSize = true;
            this.radioTimeRangeAll.Checked = true;
            this.radioTimeRangeAll.Location = new System.Drawing.Point(94, 3);
            this.radioTimeRangeAll.Name = "radioTimeRangeAll";
            this.radioTimeRangeAll.Size = new System.Drawing.Size(36, 17);
            this.radioTimeRangeAll.TabIndex = 0;
            this.radioTimeRangeAll.TabStop = true;
            this.radioTimeRangeAll.Tag = "All the recorded data will be filtered  and the full time range will be returned." +
                "";
            this.radioTimeRangeAll.Text = "All";
            this.radioTimeRangeAll.UseVisualStyleBackColor = true;
            this.radioTimeRangeAll.CheckedChanged += new System.EventHandler(this.radioTimeRangeAll_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Snow;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.textCallCountMax);
            this.panel3.Controls.Add(this.textCallCountMin);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(12, 56);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(389, 26);
            this.panel3.TabIndex = 67;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(324, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 69;
            this.label6.Text = "times.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 68;
            this.label5.Text = "and";
            // 
            // textCallCountMax
            // 
            this.textCallCountMax.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCallCountMax.Location = new System.Drawing.Point(246, 3);
            this.textCallCountMax.Name = "textCallCountMax";
            this.textCallCountMax.Size = new System.Drawing.Size(72, 20);
            this.textCallCountMax.TabIndex = 69;
            this.textCallCountMax.Text = "100";
            this.textCallCountMax.TextChanged += new System.EventHandler(this.textCallCountMax_TextChanged);
            // 
            // textCallCountMin
            // 
            this.textCallCountMin.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCallCountMin.Location = new System.Drawing.Point(145, 3);
            this.textCallCountMin.Name = "textCallCountMin";
            this.textCallCountMin.Size = new System.Drawing.Size(73, 20);
            this.textCallCountMin.TabIndex = 68;
            this.textCallCountMin.Text = "1";
            this.textCallCountMin.TextChanged += new System.EventHandler(this.textCallCountMin_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "Functions called between";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Snow;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.textArgCountMax);
            this.panel4.Controls.Add(this.textArgCountMin);
            this.panel4.Location = new System.Drawing.Point(12, 81);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(389, 26);
            this.panel4.TabIndex = 68;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 70;
            this.label9.Text = "Functions with";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(227, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 69;
            this.label7.Text = "parameters.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 13);
            this.label8.TabIndex = 68;
            this.label8.Text = "to";
            // 
            // textArgCountMax
            // 
            this.textArgCountMax.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textArgCountMax.Location = new System.Drawing.Point(161, 3);
            this.textArgCountMax.Name = "textArgCountMax";
            this.textArgCountMax.Size = new System.Drawing.Size(59, 20);
            this.textArgCountMax.TabIndex = 69;
            this.textArgCountMax.Text = "20";
            this.textArgCountMax.TextChanged += new System.EventHandler(this.textArgCountMax_TextChanged);
            // 
            // textArgCountMin
            // 
            this.textArgCountMin.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textArgCountMin.Location = new System.Drawing.Point(85, 3);
            this.textArgCountMin.Name = "textArgCountMin";
            this.textArgCountMin.Size = new System.Drawing.Size(50, 20);
            this.textArgCountMin.TabIndex = 68;
            this.textArgCountMin.Text = "0";
            this.textArgCountMin.TextChanged += new System.EventHandler(this.textArgCountMin_TextChanged);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Snow;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label19);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Controls.Add(this.text1Byte);
            this.panel5.Controls.Add(this.radioArg1Byte);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.text2Byte);
            this.panel5.Controls.Add(this.radioArg2Byte);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.textFloat);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.text4Byte);
            this.panel5.Controls.Add(this.radioArgFloat);
            this.panel5.Controls.Add(this.radioArg4Byte);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.radioArgAny);
            this.panel5.Location = new System.Drawing.Point(12, 106);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(389, 129);
            this.panel5.TabIndex = 69;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(111, 19);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(226, 13);
            this.label19.TabIndex = 74;
            this.label19.Text = "You can enter ranges with a colon (eg.  10:15)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(210, 40);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 13);
            this.label16.TabIndex = 78;
            this.label16.Text = "(eg. 255 or 0xff or \'a\')";
            // 
            // text1Byte
            // 
            this.text1Byte.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text1Byte.Location = new System.Drawing.Point(103, 37);
            this.text1Byte.Name = "text1Byte";
            this.text1Byte.Size = new System.Drawing.Size(101, 20);
            this.text1Byte.TabIndex = 77;
            this.text1Byte.Text = "0";
            this.text1Byte.TextChanged += new System.EventHandler(this.text1Byte_TextChanged);
            // 
            // radioArg1Byte
            // 
            this.radioArg1Byte.AutoSize = true;
            this.radioArg1Byte.Location = new System.Drawing.Point(6, 38);
            this.radioArg1Byte.Name = "radioArg1Byte";
            this.radioArg1Byte.Size = new System.Drawing.Size(91, 17);
            this.radioArg1Byte.TabIndex = 76;
            this.radioArg1Byte.Tag = "All the recorded data will be filtered  and the full time range will be returned." +
                "";
            this.radioArg1Byte.Text = "1-Byte Integer";
            this.radioArg1Byte.UseVisualStyleBackColor = true;
            this.radioArg1Byte.CheckedChanged += new System.EventHandler(this.radioArg1Byte_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(210, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(99, 13);
            this.label14.TabIndex = 75;
            this.label14.Text = "(eg. 4351 or 0x10ff)";
            // 
            // text2Byte
            // 
            this.text2Byte.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text2Byte.Location = new System.Drawing.Point(103, 57);
            this.text2Byte.Name = "text2Byte";
            this.text2Byte.Size = new System.Drawing.Size(101, 20);
            this.text2Byte.TabIndex = 74;
            this.text2Byte.Text = "0";
            this.text2Byte.TextChanged += new System.EventHandler(this.text2Byte_TextChanged);
            // 
            // radioArg2Byte
            // 
            this.radioArg2Byte.AutoSize = true;
            this.radioArg2Byte.Location = new System.Drawing.Point(6, 58);
            this.radioArg2Byte.Name = "radioArg2Byte";
            this.radioArg2Byte.Size = new System.Drawing.Size(91, 17);
            this.radioArg2Byte.TabIndex = 73;
            this.radioArg2Byte.Tag = "All the recorded data will be filtered  and the full time range will be returned." +
                "";
            this.radioArg2Byte.Text = "2-Byte Integer";
            this.radioArg2Byte.UseVisualStyleBackColor = true;
            this.radioArg2Byte.CheckedChanged += new System.EventHandler(this.radioArg2Byte_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(210, 104);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 71;
            this.label12.Text = "(eg. 0.00215)";
            // 
            // textFloat
            // 
            this.textFloat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFloat.Location = new System.Drawing.Point(102, 101);
            this.textFloat.Name = "textFloat";
            this.textFloat.Size = new System.Drawing.Size(102, 20);
            this.textFloat.TabIndex = 71;
            this.textFloat.Text = "0.0";
            this.textFloat.TextChanged += new System.EventHandler(this.textFloat_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(210, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(129, 13);
            this.label11.TabIndex = 70;
            this.label11.Text = "(eg. 1708287 or 0x1a10ff)";
            // 
            // text4Byte
            // 
            this.text4Byte.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text4Byte.Location = new System.Drawing.Point(103, 79);
            this.text4Byte.Name = "text4Byte";
            this.text4Byte.Size = new System.Drawing.Size(101, 20);
            this.text4Byte.TabIndex = 70;
            this.text4Byte.Text = "0";
            this.text4Byte.TextChanged += new System.EventHandler(this.text4Byte_TextChanged);
            // 
            // radioArgFloat
            // 
            this.radioArgFloat.AutoSize = true;
            this.radioArgFloat.Location = new System.Drawing.Point(6, 102);
            this.radioArgFloat.Name = "radioArgFloat";
            this.radioArgFloat.Size = new System.Drawing.Size(97, 17);
            this.radioArgFloat.TabIndex = 72;
            this.radioArgFloat.Tag = "All the recorded data will be filtered  and the full time range will be returned." +
                "";
            this.radioArgFloat.Text = "Float or Double";
            this.radioArgFloat.UseVisualStyleBackColor = true;
            this.radioArgFloat.CheckedChanged += new System.EventHandler(this.radioArgFloat_CheckedChanged);
            // 
            // radioArg4Byte
            // 
            this.radioArg4Byte.AutoSize = true;
            this.radioArg4Byte.Location = new System.Drawing.Point(6, 80);
            this.radioArg4Byte.Name = "radioArg4Byte";
            this.radioArg4Byte.Size = new System.Drawing.Size(91, 17);
            this.radioArg4Byte.TabIndex = 70;
            this.radioArg4Byte.Tag = "All the recorded data will be filtered  and the full time range will be returned." +
                "";
            this.radioArg4Byte.Text = "4-Byte Integer";
            this.radioArg4Byte.UseVisualStyleBackColor = true;
            this.radioArg4Byte.CheckedChanged += new System.EventHandler(this.radioArg4Byte_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 70;
            this.label10.Text = "Calls with Argument:";
            // 
            // radioArgAny
            // 
            this.radioArgAny.AutoSize = true;
            this.radioArgAny.Checked = true;
            this.radioArgAny.Location = new System.Drawing.Point(6, 19);
            this.radioArgAny.Name = "radioArgAny";
            this.radioArgAny.Size = new System.Drawing.Size(43, 17);
            this.radioArgAny.TabIndex = 1;
            this.radioArgAny.TabStop = true;
            this.radioArgAny.Tag = "All the recorded data will be filtered  and the full time range will be returned." +
                "";
            this.radioArgAny.Text = "Any";
            this.radioArgAny.UseVisualStyleBackColor = true;
            this.radioArgAny.CheckedChanged += new System.EventHandler(this.radioArgAny_CheckedChanged);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Snow;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.checkStringCaseSensitive);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.textStringMatch);
            this.panel6.Controls.Add(this.radioStringMatch);
            this.panel6.Controls.Add(this.radioStringYes);
            this.panel6.Controls.Add(this.label15);
            this.panel6.Controls.Add(this.radioStringAny);
            this.panel6.Location = new System.Drawing.Point(12, 233);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(389, 108);
            this.panel6.TabIndex = 70;
            // 
            // checkStringCaseSensitive
            // 
            this.checkStringCaseSensitive.AutoSize = true;
            this.checkStringCaseSensitive.Location = new System.Drawing.Point(126, 86);
            this.checkStringCaseSensitive.Name = "checkStringCaseSensitive";
            this.checkStringCaseSensitive.Size = new System.Drawing.Size(140, 17);
            this.checkStringCaseSensitive.TabIndex = 76;
            this.checkStringCaseSensitive.Text = "Case sensitive matching";
            this.checkStringCaseSensitive.UseVisualStyleBackColor = true;
            this.checkStringCaseSensitive.CheckedChanged += new System.EventHandler(this.checkStringCaseSensitive_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(282, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 75;
            this.label13.Text = "(eg. hello world)";
            // 
            // textStringMatch
            // 
            this.textStringMatch.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textStringMatch.Location = new System.Drawing.Point(142, 62);
            this.textStringMatch.Name = "textStringMatch";
            this.textStringMatch.Size = new System.Drawing.Size(134, 20);
            this.textStringMatch.TabIndex = 74;
            this.textStringMatch.Text = "hello world";
            this.textStringMatch.TextChanged += new System.EventHandler(this.textStringMatch_TextChanged);
            // 
            // radioStringMatch
            // 
            this.radioStringMatch.AutoSize = true;
            this.radioStringMatch.Location = new System.Drawing.Point(6, 63);
            this.radioStringMatch.Name = "radioStringMatch";
            this.radioStringMatch.Size = new System.Drawing.Size(130, 17);
            this.radioStringMatch.TabIndex = 72;
            this.radioStringMatch.Tag = "Only calls which have have an argument pointing to a ascii or unicode string that" +
                " matches the text are included.";
            this.radioStringMatch.Text = "String partial match to:";
            this.radioStringMatch.UseVisualStyleBackColor = true;
            this.radioStringMatch.CheckedChanged += new System.EventHandler(this.radioStringMatch_CheckedChanged);
            // 
            // radioStringYes
            // 
            this.radioStringYes.AutoSize = true;
            this.radioStringYes.Location = new System.Drawing.Point(6, 41);
            this.radioStringYes.Name = "radioStringYes";
            this.radioStringYes.Size = new System.Drawing.Size(226, 17);
            this.radioStringYes.TabIndex = 70;
            this.radioStringYes.Tag = "Only calls which have at least one argument pointing to a unicode/ascii string wi" +
                "ll be included.";
            this.radioStringYes.Text = "Has at least 1 argument pointing to a string";
            this.radioStringYes.UseVisualStyleBackColor = true;
            this.radioStringYes.CheckedChanged += new System.EventHandler(this.radioStringYes_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(205, 13);
            this.label15.TabIndex = 70;
            this.label15.Text = "Calls with String Argument: (unicode/ascii)";
            // 
            // radioStringAny
            // 
            this.radioStringAny.AutoSize = true;
            this.radioStringAny.Checked = true;
            this.radioStringAny.Location = new System.Drawing.Point(6, 20);
            this.radioStringAny.Name = "radioStringAny";
            this.radioStringAny.Size = new System.Drawing.Size(43, 17);
            this.radioStringAny.TabIndex = 1;
            this.radioStringAny.TabStop = true;
            this.radioStringAny.Tag = "All calls will be included.";
            this.radioStringAny.Text = "Any";
            this.radioStringAny.UseVisualStyleBackColor = true;
            this.radioStringAny.CheckedChanged += new System.EventHandler(this.radioStringAny_CheckedChanged);
            // 
            // buttonApplyReplace
            // 
            this.buttonApplyReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyReplace.Location = new System.Drawing.Point(493, 547);
            this.buttonApplyReplace.Name = "buttonApplyReplace";
            this.buttonApplyReplace.Size = new System.Drawing.Size(166, 26);
            this.buttonApplyReplace.TabIndex = 71;
            this.buttonApplyReplace.Text = "Apply and Replace Current List";
            this.buttonApplyReplace.UseVisualStyleBackColor = true;
            this.buttonApplyReplace.Click += new System.EventHandler(this.buttonApplyReplace_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.Location = new System.Drawing.Point(12, 547);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(113, 26);
            this.buttonCancel.TabIndex = 72;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApplyNew
            // 
            this.buttonApplyNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyNew.Location = new System.Drawing.Point(665, 547);
            this.buttonApplyNew.Name = "buttonApplyNew";
            this.buttonApplyNew.Size = new System.Drawing.Size(166, 26);
            this.buttonApplyNew.TabIndex = 73;
            this.buttonApplyNew.Text = "Apply and Create New List";
            this.buttonApplyNew.UseVisualStyleBackColor = true;
            this.buttonApplyNew.Click += new System.EventHandler(this.buttonApplyNew_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Snow;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label18);
            this.panel7.Controls.Add(this.textAddressRangeFunction);
            this.panel7.Controls.Add(this.textAddressRangeSource);
            this.panel7.Controls.Add(this.checkAddressSource);
            this.panel7.Controls.Add(this.checkAddressFunction);
            this.panel7.Controls.Add(this.comboModules);
            this.panel7.Controls.Add(this.label17);
            this.panel7.Location = new System.Drawing.Point(12, 436);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(389, 105);
            this.panel7.TabIndex = 74;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(18, 78);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(169, 13);
            this.label18.TabIndex = 81;
            this.label18.Text = "Load address ranges from module:";
            // 
            // textAddressRangeFunction
            // 
            this.textAddressRangeFunction.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textAddressRangeFunction.Location = new System.Drawing.Point(191, 21);
            this.textAddressRangeFunction.Name = "textAddressRangeFunction";
            this.textAddressRangeFunction.Size = new System.Drawing.Size(188, 20);
            this.textAddressRangeFunction.TabIndex = 80;
            this.textAddressRangeFunction.Text = "0x00000000:0xffffffff";
            this.textAddressRangeFunction.TextChanged += new System.EventHandler(this.textAddressRangeFunction_TextChanged);
            // 
            // textAddressRangeSource
            // 
            this.textAddressRangeSource.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textAddressRangeSource.Location = new System.Drawing.Point(191, 46);
            this.textAddressRangeSource.Name = "textAddressRangeSource";
            this.textAddressRangeSource.Size = new System.Drawing.Size(188, 20);
            this.textAddressRangeSource.TabIndex = 79;
            this.textAddressRangeSource.Text = "0x00000000:0xffffffff";
            this.textAddressRangeSource.TextChanged += new System.EventHandler(this.textAddressRangeSource_TextChanged);
            // 
            // checkAddressSource
            // 
            this.checkAddressSource.AutoSize = true;
            this.checkAddressSource.Location = new System.Drawing.Point(6, 49);
            this.checkAddressSource.Name = "checkAddressSource";
            this.checkAddressSource.Size = new System.Drawing.Size(179, 17);
            this.checkAddressSource.TabIndex = 78;
            this.checkAddressSource.Text = "The call source address is within";
            this.checkAddressSource.UseVisualStyleBackColor = true;
            this.checkAddressSource.CheckedChanged += new System.EventHandler(this.checkAddress_CheckedChanged);
            // 
            // checkAddressFunction
            // 
            this.checkAddressFunction.AutoSize = true;
            this.checkAddressFunction.Location = new System.Drawing.Point(6, 23);
            this.checkAddressFunction.Name = "checkAddressFunction";
            this.checkAddressFunction.Size = new System.Drawing.Size(166, 17);
            this.checkAddressFunction.TabIndex = 76;
            this.checkAddressFunction.Text = "The function address is within";
            this.checkAddressFunction.UseVisualStyleBackColor = true;
            this.checkAddressFunction.CheckedChanged += new System.EventHandler(this.checkAddressModule_CheckedChanged);
            // 
            // comboModules
            // 
            this.comboModules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModules.FormattingEnabled = true;
            this.comboModules.Location = new System.Drawing.Point(190, 75);
            this.comboModules.Name = "comboModules";
            this.comboModules.Size = new System.Drawing.Size(188, 21);
            this.comboModules.TabIndex = 77;
            this.comboModules.SelectedIndexChanged += new System.EventHandler(this.comboModules_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(95, 13);
            this.label17.TabIndex = 70;
            this.label17.Text = "Calls with Address:";
            // 
            // labelCallsBefore
            // 
            this.labelCallsBefore.AutoSize = true;
            this.labelCallsBefore.Location = new System.Drawing.Point(15, 0);
            this.labelCallsBefore.Name = "labelCallsBefore";
            this.labelCallsBefore.Size = new System.Drawing.Size(106, 13);
            this.labelCallsBefore.TabIndex = 75;
            this.labelCallsBefore.Text = "0 calls before filtering";
            // 
            // labelCallsAfter
            // 
            this.labelCallsAfter.AutoSize = true;
            this.labelCallsAfter.Location = new System.Drawing.Point(15, 14);
            this.labelCallsAfter.Name = "labelCallsAfter";
            this.labelCallsAfter.Size = new System.Drawing.Size(97, 13);
            this.labelCallsAfter.TabIndex = 76;
            this.labelCallsAfter.Text = "0 calls after filtering";
            // 
            // labelFunctionsAfter
            // 
            this.labelFunctionsAfter.AutoSize = true;
            this.labelFunctionsAfter.Location = new System.Drawing.Point(204, 14);
            this.labelFunctionsAfter.Name = "labelFunctionsAfter";
            this.labelFunctionsAfter.Size = new System.Drawing.Size(97, 13);
            this.labelFunctionsAfter.TabIndex = 78;
            this.labelFunctionsAfter.Text = "0 calls after filtering";
            // 
            // labelFunctionsBefore
            // 
            this.labelFunctionsBefore.AutoSize = true;
            this.labelFunctionsBefore.Location = new System.Drawing.Point(204, 0);
            this.labelFunctionsBefore.Name = "labelFunctionsBefore";
            this.labelFunctionsBefore.Size = new System.Drawing.Size(106, 13);
            this.labelFunctionsBefore.TabIndex = 77;
            this.labelFunctionsBefore.Text = "0 calls before filtering";
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.BackColor = System.Drawing.Color.White;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.functionGrid);
            this.panel8.Controls.Add(this.labelFunctionsAfter);
            this.panel8.Controls.Add(this.labelFunctionsBefore);
            this.panel8.Controls.Add(this.labelCallsAfter);
            this.panel8.Controls.Add(this.labelCallsBefore);
            this.panel8.Location = new System.Drawing.Point(406, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(432, 525);
            this.panel8.TabIndex = 79;
            // 
            // functionGrid
            // 
            this.functionGrid.AllowUserToAddRows = false;
            this.functionGrid.AllowUserToResizeRows = false;
            this.functionGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.functionGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.functionGrid.BackgroundColor = System.Drawing.Color.White;
            this.functionGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.functionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.functionGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFunction,
            this.colAddress,
            this.colExecutionCount,
            this.colCountSelection});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.functionGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.functionGrid.Location = new System.Drawing.Point(6, 30);
            this.functionGrid.Name = "functionGrid";
            this.functionGrid.RowHeadersVisible = false;
            this.functionGrid.Size = new System.Drawing.Size(417, 489);
            this.functionGrid.TabIndex = 79;
            this.functionGrid.VirtualMode = true;
            // 
            // colFunction
            // 
            this.colFunction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFunction.HeaderText = "Description";
            this.colFunction.Name = "colFunction";
            this.colFunction.ReadOnly = true;
            this.colFunction.Width = 163;
            // 
            // colAddress
            // 
            this.colAddress.HeaderText = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.Width = 70;
            // 
            // colExecutionCount
            // 
            this.colExecutionCount.HeaderText = "Call Count: Total/Selected Region";
            this.colExecutionCount.Name = "colExecutionCount";
            this.colExecutionCount.ReadOnly = true;
            // 
            // colCountSelection
            // 
            this.colCountSelection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCountSelection.HeaderText = "Num Args";
            this.colCountSelection.Name = "colCountSelection";
            this.colCountSelection.ReadOnly = true;
            // 
            // timerRefreshOutput
            // 
            this.timerRefreshOutput.Enabled = true;
            this.timerRefreshOutput.Interval = 500;
            this.timerRefreshOutput.Tick += new System.EventHandler(this.timerRefreshOutput_Tick);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Snow;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.radioBinaryStart);
            this.panel9.Controls.Add(this.textBinaryMatch);
            this.panel9.Controls.Add(this.radioBinaryPartial);
            this.panel9.Controls.Add(this.label21);
            this.panel9.Controls.Add(this.radioBinaryAny);
            this.panel9.Location = new System.Drawing.Point(12, 340);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(389, 97);
            this.panel9.TabIndex = 80;
            // 
            // radioBinaryStart
            // 
            this.radioBinaryStart.AutoSize = true;
            this.radioBinaryStart.Location = new System.Drawing.Point(7, 65);
            this.radioBinaryStart.Name = "radioBinaryStart";
            this.radioBinaryStart.Size = new System.Drawing.Size(107, 17);
            this.radioBinaryStart.TabIndex = 76;
            this.radioBinaryStart.Tag = "Only calls which have have an argument pointing to data that is a starts with the" +
                " textbox hex are included.";
            this.radioBinaryStart.Text = "Binary starts with:";
            this.radioBinaryStart.UseVisualStyleBackColor = true;
            this.radioBinaryStart.CheckedChanged += new System.EventHandler(this.radioBinaryStart_CheckedChanged);
            // 
            // textBinaryMatch
            // 
            this.textBinaryMatch.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBinaryMatch.Location = new System.Drawing.Point(142, 30);
            this.textBinaryMatch.Multiline = true;
            this.textBinaryMatch.Name = "textBinaryMatch";
            this.textBinaryMatch.Size = new System.Drawing.Size(236, 62);
            this.textBinaryMatch.TabIndex = 74;
            this.textBinaryMatch.Text = "00 00 00 00 00 00 00 00 00 00";
            this.textBinaryMatch.TextChanged += new System.EventHandler(this.textBinaryMatch_TextChanged);
            // 
            // radioBinaryPartial
            // 
            this.radioBinaryPartial.AutoSize = true;
            this.radioBinaryPartial.Location = new System.Drawing.Point(6, 42);
            this.radioBinaryPartial.Name = "radioBinaryPartial";
            this.radioBinaryPartial.Size = new System.Drawing.Size(132, 17);
            this.radioBinaryPartial.TabIndex = 72;
            this.radioBinaryPartial.Tag = "Only calls which have have an argument pointing to a data that is a partial match" +
                " to the textbox hex are included.";
            this.radioBinaryPartial.Text = "Binary partial match to:";
            this.radioBinaryPartial.UseVisualStyleBackColor = true;
            this.radioBinaryPartial.CheckedChanged += new System.EventHandler(this.radioBinaryPartial_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(3, 4);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(148, 13);
            this.label21.TabIndex = 70;
            this.label21.Text = "Calls with Binary Dereference:";
            // 
            // radioBinaryAny
            // 
            this.radioBinaryAny.AutoSize = true;
            this.radioBinaryAny.Checked = true;
            this.radioBinaryAny.Location = new System.Drawing.Point(6, 20);
            this.radioBinaryAny.Name = "radioBinaryAny";
            this.radioBinaryAny.Size = new System.Drawing.Size(43, 17);
            this.radioBinaryAny.TabIndex = 1;
            this.radioBinaryAny.TabStop = true;
            this.radioBinaryAny.Tag = "All calls will be included.";
            this.radioBinaryAny.Text = "Any";
            this.radioBinaryAny.UseVisualStyleBackColor = true;
            // 
            // formFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 576);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.buttonApplyNew);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApplyReplace);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "formFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Data";
            this.Load += new System.EventHandler(this.formFilter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.functionGrid)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioTypeBoth;
        private System.Windows.Forms.RadioButton radioTypeIntra;
        private System.Windows.Forms.RadioButton radioTypeInter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioTimeRangeClip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioTimeRangeAll;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textCallCountMax;
        private System.Windows.Forms.TextBox textCallCountMin;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textArgCountMax;
        private System.Windows.Forms.TextBox textArgCountMin;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton radioArgAny;
        private System.Windows.Forms.RadioButton radioArgFloat;
        private System.Windows.Forms.RadioButton radioArg4Byte;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox text4Byte;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textFloat;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton radioStringYes;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RadioButton radioStringAny;
        private System.Windows.Forms.RadioButton radioStringMatch;
        private System.Windows.Forms.Button buttonApplyReplace;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApplyNew;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textAddressRangeSource;
        private System.Windows.Forms.CheckBox checkAddressSource;
        private System.Windows.Forms.ComboBox comboModules;
        private System.Windows.Forms.CheckBox checkAddressFunction;
        private System.Windows.Forms.Label labelCallsBefore;
        private System.Windows.Forms.Label labelCallsAfter;
        private System.Windows.Forms.Label labelFunctionsAfter;
        private System.Windows.Forms.Label labelFunctionsBefore;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.DataGridView functionGrid;
        private System.Windows.Forms.Timer timerRefreshOutput;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox text1Byte;
        private System.Windows.Forms.RadioButton radioArg1Byte;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox text2Byte;
        private System.Windows.Forms.RadioButton radioArg2Byte;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textAddressRangeFunction;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExecutionCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCountSelection;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textStringMatch;
        private System.Windows.Forms.CheckBox checkStringCaseSensitive;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TextBox textBinaryMatch;
        private System.Windows.Forms.RadioButton radioBinaryPartial;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.RadioButton radioBinaryAny;
        private System.Windows.Forms.RadioButton radioBinaryStart;

    }
}