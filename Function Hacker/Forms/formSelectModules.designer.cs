namespace FunctionHacker.Forms
{
    partial class formSelectModules
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.listModules = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.checkAggressiveDereferencing = new System.Windows.Forms.CheckBox();
            this.checkForceAllIntermodular = new System.Windows.Forms.CheckBox();
            this.checkOnlyIntermodular = new System.Windows.Forms.CheckBox();
            this.checkInjectCallbacks = new System.Windows.Forms.CheckBox();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(746, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(846, 394);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // listModules
            // 
            this.listModules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listModules.CheckBoxes = true;
            this.listModules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2});
            this.listModules.Location = new System.Drawing.Point(0, 0);
            this.listModules.Name = "listModules";
            this.listModules.Size = new System.Drawing.Size(941, 389);
            this.listModules.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listModules.TabIndex = 5;
            this.listModules.UseCompatibleStateImageBehavior = false;
            this.listModules.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Module Name";
            this.columnHeader1.Width = 151;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Base Address";
            this.columnHeader3.Width = 129;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Module Path";
            this.columnHeader2.Width = 408;
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckAll.Location = new System.Drawing.Point(576, 395);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(79, 22);
            this.btnUncheckAll.TabIndex = 7;
            this.btnUncheckAll.Text = "(uncheck all)";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckAll.Location = new System.Drawing.Point(661, 395);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(79, 22);
            this.btnCheckAll.TabIndex = 6;
            this.btnCheckAll.Text = "(check all)";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.Info;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(719, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Disassembly Options";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.Snow;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.checkAggressiveDereferencing);
            this.panel5.Controls.Add(this.checkForceAllIntermodular);
            this.panel5.Controls.Add(this.checkOnlyIntermodular);
            this.panel5.Controls.Add(this.checkInjectCallbacks);
            this.panel5.Location = new System.Drawing.Point(719, 314);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(222, 75);
            this.panel5.TabIndex = 70;
            // 
            // checkAggressiveDereferencing
            // 
            this.checkAggressiveDereferencing.AutoSize = true;
            this.checkAggressiveDereferencing.Location = new System.Drawing.Point(3, 55);
            this.checkAggressiveDereferencing.Name = "checkAggressiveDereferencing";
            this.checkAggressiveDereferencing.Size = new System.Drawing.Size(193, 17);
            this.checkAggressiveDereferencing.TabIndex = 74;
            this.checkAggressiveDereferencing.Tag = "";
            this.checkAggressiveDereferencing.Text = "Aggressive argument dereferencing";
            this.checkAggressiveDereferencing.UseVisualStyleBackColor = true;
            this.checkAggressiveDereferencing.CheckedChanged += new System.EventHandler(this.checkAggressiveDereferencing_CheckedChanged);
            // 
            // checkForceAllIntermodular
            // 
            this.checkForceAllIntermodular.AutoSize = true;
            this.checkForceAllIntermodular.Location = new System.Drawing.Point(3, 38);
            this.checkForceAllIntermodular.Name = "checkForceAllIntermodular";
            this.checkForceAllIntermodular.Size = new System.Drawing.Size(212, 17);
            this.checkForceAllIntermodular.TabIndex = 73;
            this.checkForceAllIntermodular.Tag = "If checked, the recording instrumentation is inserted at the start of the functio" +
                "n - as opposed to redirecting the call in transit.";
            this.checkForceAllIntermodular.Text = "Force recording of all inter-modular calls";
            this.checkForceAllIntermodular.UseVisualStyleBackColor = true;
            // 
            // checkOnlyIntermodular
            // 
            this.checkOnlyIntermodular.AutoSize = true;
            this.checkOnlyIntermodular.Location = new System.Drawing.Point(3, 21);
            this.checkOnlyIntermodular.Name = "checkOnlyIntermodular";
            this.checkOnlyIntermodular.Size = new System.Drawing.Size(167, 17);
            this.checkOnlyIntermodular.TabIndex = 71;
            this.checkOnlyIntermodular.Text = "Only record inter-modular calls";
            this.checkOnlyIntermodular.UseVisualStyleBackColor = true;
            this.checkOnlyIntermodular.CheckedChanged += new System.EventHandler(this.checkOnlyIntermodularCheckedChanged);
            // 
            // checkInjectCallbacks
            // 
            this.checkInjectCallbacks.AutoSize = true;
            this.checkInjectCallbacks.Checked = true;
            this.checkInjectCallbacks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkInjectCallbacks.Location = new System.Drawing.Point(3, 3);
            this.checkInjectCallbacks.Name = "checkInjectCallbacks";
            this.checkInjectCallbacks.Size = new System.Drawing.Size(140, 17);
            this.checkInjectCallbacks.TabIndex = 0;
            this.checkInjectCallbacks.Text = "Overwrite object vtables";
            this.checkInjectCallbacks.UseVisualStyleBackColor = true;
            // 
            // formSelectModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 419);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.listModules);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "formSelectModules";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Modules to Analyze";
            this.Load += new System.EventHandler(this.formSelectModules_Load);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListView listModules;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox checkOnlyIntermodular;
        private System.Windows.Forms.CheckBox checkInjectCallbacks;
        private System.Windows.Forms.CheckBox checkForceAllIntermodular;
        private System.Windows.Forms.CheckBox checkAggressiveDereferencing;
    }
}