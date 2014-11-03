namespace FunctionHacker.Controls
{
	partial class userSettings
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(userSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.circularBufferSize = new System.Windows.Forms.NumericUpDown();
            this.numBytesDereference = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.timerSave = new System.Windows.Forms.Timer(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.buttonSignaturePath = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonSigDumpPath = new System.Windows.Forms.Button();
            this.textBoxSigDumpPath = new System.Windows.Forms.TextBox();
            this.textBoxSignaturePath = new System.Windows.Forms.TextBox();
            this.radioButtonAllDlls = new System.Windows.Forms.RadioButton();
            this.radioButtonNonWindowsDlls = new System.Windows.Forms.RadioButton();
            this.radioButtonExeOnly = new System.Windows.Forms.RadioButton();
            this.numericUpDownMaxCalls = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.circularBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBytesDereference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxCalls)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Refresh rate for the timeline visualization:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Default module selection:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(22, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(342, 48);
            this.label3.TabIndex = 0;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "milliseconds.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(113, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "max calls.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(113, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "bytes.";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(22, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(342, 31);
            this.label7.TabIndex = 7;
            this.label7.Text = "Number of bytes to dereference when a valid pointer is detected as an argument.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(107, 277);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "mb.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 259);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(235, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Circular buffer size for storing call recording data.";
            // 
            // circularBufferSize
            // 
            this.circularBufferSize.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FunctionHacker.Properties.Settings.Default, "circularBufferSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.circularBufferSize.Location = new System.Drawing.Point(43, 275);
            this.circularBufferSize.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.circularBufferSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.circularBufferSize.Name = "circularBufferSize";
            this.circularBufferSize.Size = new System.Drawing.Size(58, 20);
            this.circularBufferSize.TabIndex = 12;
            this.circularBufferSize.Value = global::FunctionHacker.Properties.Settings.Default.CircularBufferSize;
            this.circularBufferSize.ValueChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // numBytesDereference
            // 
            this.numBytesDereference.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FunctionHacker.Properties.Settings.Default, "numDereferencedBytes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numBytesDereference.Location = new System.Drawing.Point(43, 192);
            this.numBytesDereference.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numBytesDereference.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numBytesDereference.Name = "numBytesDereference";
            this.numBytesDereference.Size = new System.Drawing.Size(58, 20);
            this.numBytesDereference.TabIndex = 10;
            this.numBytesDereference.Value = global::FunctionHacker.Properties.Settings.Default.NumDereferencedBytes;
            this.numBytesDereference.ValueChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // numericUpDownRefreshRate
            // 
            this.numericUpDownRefreshRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FunctionHacker.Properties.Settings.Default, "TimelineRefreshRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownRefreshRate.Location = new System.Drawing.Point(43, 236);
            this.numericUpDownRefreshRate.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownRefreshRate.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownRefreshRate.Name = "numericUpDownRefreshRate";
            this.numericUpDownRefreshRate.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownRefreshRate.TabIndex = 1;
            this.numericUpDownRefreshRate.Value = global::FunctionHacker.Properties.Settings.Default.TimelineRefreshRate;
            this.numericUpDownRefreshRate.ValueChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // timerSave
            // 
            this.timerSave.Interval = 500;
            this.timerSave.Tick += new System.EventHandler(this.timerSave_Tick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 298);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(151, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Path to IDA signature directory";
            // 
            // buttonSignaturePath
            // 
            this.buttonSignaturePath.Location = new System.Drawing.Point(311, 313);
            this.buttonSignaturePath.Name = "buttonSignaturePath";
            this.buttonSignaturePath.Size = new System.Drawing.Size(29, 23);
            this.buttonSignaturePath.TabIndex = 15;
            this.buttonSignaturePath.Text = "...";
            this.buttonSignaturePath.UseVisualStyleBackColor = true;
            this.buttonSignaturePath.Click += new System.EventHandler(this.buttonSignaturePath_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 337);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(214, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Path to sigdump.exe tool (IDA Flirt package)";
            // 
            // buttonSigDumpPath
            // 
            this.buttonSigDumpPath.Location = new System.Drawing.Point(311, 352);
            this.buttonSigDumpPath.Name = "buttonSigDumpPath";
            this.buttonSigDumpPath.Size = new System.Drawing.Size(29, 23);
            this.buttonSigDumpPath.TabIndex = 15;
            this.buttonSigDumpPath.Text = "...";
            this.buttonSigDumpPath.UseVisualStyleBackColor = true;
            this.buttonSigDumpPath.Click += new System.EventHandler(this.buttonSigDumpPath_Click);
            // 
            // textBoxSigDumpPath
            // 
            this.textBoxSigDumpPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FunctionHacker.Properties.Settings.Default, "DumpSigPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSigDumpPath.Location = new System.Drawing.Point(43, 353);
            this.textBoxSigDumpPath.Name = "textBoxSigDumpPath";
            this.textBoxSigDumpPath.Size = new System.Drawing.Size(258, 20);
            this.textBoxSigDumpPath.TabIndex = 14;
            this.textBoxSigDumpPath.Text = global::FunctionHacker.Properties.Settings.Default.DumpSigPath;
            this.textBoxSigDumpPath.TextChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // textBoxSignaturePath
            // 
            this.textBoxSignaturePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FunctionHacker.Properties.Settings.Default, "SignaturePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSignaturePath.Location = new System.Drawing.Point(43, 314);
            this.textBoxSignaturePath.Name = "textBoxSignaturePath";
            this.textBoxSignaturePath.Size = new System.Drawing.Size(258, 20);
            this.textBoxSignaturePath.TabIndex = 14;
            this.textBoxSignaturePath.Text = global::FunctionHacker.Properties.Settings.Default.SignaturePath;
            this.textBoxSignaturePath.TextChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // radioButtonAllDlls
            // 
            this.radioButtonAllDlls.AutoSize = true;
            this.radioButtonAllDlls.CausesValidation = false;
            this.radioButtonAllDlls.Checked = global::FunctionHacker.Properties.Settings.Default.RBAllDll;
            this.radioButtonAllDlls.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FunctionHacker.Properties.Settings.Default, "RBAllDll", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButtonAllDlls.Location = new System.Drawing.Point(43, 70);
            this.radioButtonAllDlls.Name = "radioButtonAllDlls";
            this.radioButtonAllDlls.Size = new System.Drawing.Size(88, 17);
            this.radioButtonAllDlls.TabIndex = 4;
            this.radioButtonAllDlls.Text = "Select all dll\'s";
            this.radioButtonAllDlls.UseVisualStyleBackColor = true;
            this.radioButtonAllDlls.CheckedChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // radioButtonNonWindowsDlls
            // 
            this.radioButtonNonWindowsDlls.AutoSize = true;
            this.radioButtonNonWindowsDlls.CausesValidation = false;
            this.radioButtonNonWindowsDlls.Checked = global::FunctionHacker.Properties.Settings.Default.RBNonWindowsDll;
            this.radioButtonNonWindowsDlls.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FunctionHacker.Properties.Settings.Default, "RBNonWindowsDll", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButtonNonWindowsDlls.Location = new System.Drawing.Point(43, 53);
            this.radioButtonNonWindowsDlls.Name = "radioButtonNonWindowsDlls";
            this.radioButtonNonWindowsDlls.Size = new System.Drawing.Size(307, 17);
            this.radioButtonNonWindowsDlls.TabIndex = 4;
            this.radioButtonNonWindowsDlls.Text = "Select dll\'s which belong to .exe but are not part of windows";
            this.radioButtonNonWindowsDlls.UseVisualStyleBackColor = true;
            this.radioButtonNonWindowsDlls.CheckedChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // radioButtonExeOnly
            // 
            this.radioButtonExeOnly.AutoSize = true;
            this.radioButtonExeOnly.CausesValidation = false;
            this.radioButtonExeOnly.Checked = global::FunctionHacker.Properties.Settings.Default.RBExeOnly;
            this.radioButtonExeOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FunctionHacker.Properties.Settings.Default, "RBExeOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButtonExeOnly.Location = new System.Drawing.Point(43, 35);
            this.radioButtonExeOnly.Name = "radioButtonExeOnly";
            this.radioButtonExeOnly.Size = new System.Drawing.Size(172, 17);
            this.radioButtonExeOnly.TabIndex = 4;
            this.radioButtonExeOnly.TabStop = true;
            this.radioButtonExeOnly.Text = "Select only main module (*.exe)";
            this.radioButtonExeOnly.UseVisualStyleBackColor = true;
            this.radioButtonExeOnly.CheckedChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // numericUpDownMaxCalls
            // 
            this.numericUpDownMaxCalls.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::FunctionHacker.Properties.Settings.Default, "MaxRecordedCalls", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownMaxCalls.Location = new System.Drawing.Point(43, 140);
            this.numericUpDownMaxCalls.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownMaxCalls.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMaxCalls.Name = "numericUpDownMaxCalls";
            this.numericUpDownMaxCalls.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownMaxCalls.TabIndex = 1;
            this.numericUpDownMaxCalls.Value = global::FunctionHacker.Properties.Settings.Default.MaxRecordedCalls;
            this.numericUpDownMaxCalls.ValueChanged += new System.EventHandler(this.customChangedEvent);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.exe";
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // userSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSigDumpPath);
            this.Controls.Add(this.buttonSignaturePath);
            this.Controls.Add(this.textBoxSigDumpPath);
            this.Controls.Add(this.textBoxSignaturePath);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.circularBufferSize);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numBytesDereference);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButtonAllDlls);
            this.Controls.Add(this.radioButtonNonWindowsDlls);
            this.Controls.Add(this.radioButtonExeOnly);
            this.Controls.Add(this.numericUpDownMaxCalls);
            this.Controls.Add(this.numericUpDownRefreshRate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "userSettings";
            this.Size = new System.Drawing.Size(708, 433);
            ((System.ComponentModel.ISupportInitialize)(this.circularBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBytesDereference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxCalls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownRefreshRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonExeOnly;
        private System.Windows.Forms.RadioButton radioButtonNonWindowsDlls;
        private System.Windows.Forms.RadioButton radioButtonAllDlls;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxCalls;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numBytesDereference;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown circularBufferSize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timerSave;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxSignaturePath;
        private System.Windows.Forms.Button buttonSignaturePath;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxSigDumpPath;
        private System.Windows.Forms.Button buttonSigDumpPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}
