namespace FunctionHacker.Controls
{
    partial class callSender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(callSender));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.textCallAssembly = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.comboDelay = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.dataGridArguments = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridCalls = new FunctionHacker.Controls.DataGridViewEx();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.callCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.args = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridArguments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCalls)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridCalls);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textCallAssembly);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(941, 534);
            this.splitContainer1.SplitterDistance = 670;
            this.splitContainer1.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 65;
            this.label1.Text = "RECORDED CALLS";
            // 
            // textCallAssembly
            // 
            this.textCallAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textCallAssembly.BackColor = System.Drawing.Color.LightGray;
            this.textCallAssembly.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textCallAssembly.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCallAssembly.Location = new System.Drawing.Point(2, 377);
            this.textCallAssembly.Multiline = true;
            this.textCallAssembly.Name = "textCallAssembly";
            this.textCallAssembly.ReadOnly = true;
            this.textCallAssembly.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textCallAssembly.Size = new System.Drawing.Size(262, 161);
            this.textCallAssembly.TabIndex = 70;
            this.textCallAssembly.WordWrap = false;
            this.textCallAssembly.TextChanged += new System.EventHandler(this.textCallAssembly_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Snow;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.comboDelay);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.buttonSend);
            this.panel1.Controls.Add(this.dataGridArguments);
            this.panel1.Location = new System.Drawing.Point(2, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 363);
            this.panel1.TabIndex = 69;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 77;
            this.label5.Text = "with delay of";
            // 
            // comboDelay
            // 
            this.comboDelay.FormattingEnabled = true;
            this.comboDelay.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60"});
            this.comboDelay.Location = new System.Drawing.Point(106, 303);
            this.comboDelay.Name = "comboDelay";
            this.comboDelay.Size = new System.Drawing.Size(50, 21);
            this.comboDelay.TabIndex = 76;
            this.comboDelay.Text = "0";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Ctrl-1",
            "Ctrl-2",
            "Ctrl-3",
            "Ctrl-4",
            "Ctrl-5",
            "Ctrl-6",
            "Ctrl-7",
            "Ctrl-8",
            "Ctrl-9",
            "Ctrl-0",
            "Ctrl-F1",
            "Ctrl-F2",
            "Ctrl-F3",
            "Ctrl-F4",
            "Ctrl-F5",
            "Ctrl-F6",
            "Ctrl-F7",
            "Ctrl-F8",
            "Ctrl-F9",
            "Ctrl-F10",
            "Ctrl-F11",
            "Ctrl-F12"});
            this.comboBox1.Location = new System.Drawing.Point(106, 325);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(113, 21);
            this.comboBox1.TabIndex = 75;
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(34, 328);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 74;
            this.label4.Text = "Shortcut Key:";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 306);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 73;
            this.label3.Text = "seconds";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(71, 280);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(101, 20);
            this.buttonSend.TabIndex = 70;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // dataGridArguments
            // 
            this.dataGridArguments.AllowUserToAddRows = false;
            this.dataGridArguments.AllowUserToDeleteRows = false;
            this.dataGridArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.value});
            this.dataGridArguments.Location = new System.Drawing.Point(5, 87);
            this.dataGridArguments.Name = "dataGridArguments";
            this.dataGridArguments.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridArguments.RowHeadersVisible = false;
            this.dataGridArguments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridArguments.Size = new System.Drawing.Size(252, 189);
            this.dataGridArguments.TabIndex = 69;
            this.dataGridArguments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridArguments_CellValueChanged);
            // 
            // name
            // 
            this.name.FillWeight = 75F;
            this.name.HeaderText = "Argument Location";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // value
            // 
            this.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(2, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 68;
            this.label2.Text = "SEND CALL";
            // 
            // dataGridCalls
            // 
            this.dataGridCalls.AllowUserToAddRows = false;
            this.dataGridCalls.AllowUserToDeleteRows = false;
            this.dataGridCalls.AllowUserToResizeRows = false;
            this.dataGridCalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridCalls.BackgroundColor = System.Drawing.Color.White;
            this.dataGridCalls.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridCalls.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridCalls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCalls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.description,
            this.callCount,
            this.arguments,
            this.args});
            this.dataGridCalls.Location = new System.Drawing.Point(0, 15);
            this.dataGridCalls.Name = "dataGridCalls";
            this.dataGridCalls.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridCalls.RowHeadersVisible = false;
            this.dataGridCalls.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dataGridCalls.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridCalls.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridCalls.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridCalls.Size = new System.Drawing.Size(670, 519);
            this.dataGridCalls.Suspended = false;
            this.dataGridCalls.TabIndex = 66;
            this.dataGridCalls.VirtualMode = true;
            this.dataGridCalls.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridCalls_RowPrePaint);
            this.dataGridCalls.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridCalls_CellValueNeeded);
            // 
            // description
            // 
            this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.description.HeaderText = "Call Number";
            this.description.Name = "description";
            this.description.Width = 50;
            // 
            // callCount
            // 
            this.callCount.HeaderText = "Source";
            this.callCount.Name = "callCount";
            this.callCount.Width = 130;
            // 
            // arguments
            // 
            this.arguments.HeaderText = "Destination";
            this.arguments.Name = "arguments";
            this.arguments.Width = 130;
            // 
            // args
            // 
            this.args.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.args.HeaderText = "Arguments";
            this.args.Name = "args";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Info;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(252, 83);
            this.label6.TabIndex = 78;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // callSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "callSender";
            this.Size = new System.Drawing.Size(945, 540);
            this.Load += new System.EventHandler(this.callSender_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridArguments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCalls)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textCallAssembly;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.DataGridView dataGridArguments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboDelay;
        private System.Windows.Forms.Label label5;
        private DataGridViewEx dataGridCalls;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn callCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn arguments;
        private System.Windows.Forms.DataGridViewTextBoxColumn args;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.Label label6;

    }
}
