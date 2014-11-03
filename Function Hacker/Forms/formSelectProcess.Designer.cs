namespace FunctionHacker.Forms
{
    partial class FormSelectProcess
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataProcessList = new System.Windows.Forms.DataGridView();
            this.processName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processPrivilege = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.buttonRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataProcessList)).BeginInit();
            this.SuspendLayout();
            // 
            // dataProcessList
            // 
            this.dataProcessList.AllowUserToAddRows = false;
            this.dataProcessList.AllowUserToDeleteRows = false;
            this.dataProcessList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataProcessList.BackgroundColor = System.Drawing.Color.White;
            this.dataProcessList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataProcessList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataProcessList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.processName,
            this.processID,
            this.processText,
            this.processPrivilege,
            this.processPath});
            this.dataProcessList.Location = new System.Drawing.Point(-1, 2);
            this.dataProcessList.MultiSelect = false;
            this.dataProcessList.Name = "dataProcessList";
            this.dataProcessList.ReadOnly = true;
            this.dataProcessList.RowHeadersVisible = false;
            this.dataProcessList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataProcessList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataProcessList.Size = new System.Drawing.Size(822, 442);
            this.dataProcessList.TabIndex = 3;
            this.dataProcessList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataProcessListCellDoubleClick1);
            this.dataProcessList.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataProcessListCellContentDoubleClick);
            this.dataProcessList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataProcessListCellContentClick);
            // 
            // processName
            // 
            this.processName.HeaderText = "Process";
            this.processName.Name = "processName";
            this.processName.ReadOnly = true;
            // 
            // processID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.processID.DefaultCellStyle = dataGridViewCellStyle2;
            this.processID.HeaderText = "PID";
            this.processID.Name = "processID";
            this.processID.ReadOnly = true;
            this.processID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // processText
            // 
            this.processText.HeaderText = "Window Text";
            this.processText.Name = "processText";
            this.processText.ReadOnly = true;
            // 
            // processPrivilege
            // 
            this.processPrivilege.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.processPrivilege.HeaderText = "Privilege";
            this.processPrivilege.Name = "processPrivilege";
            this.processPrivilege.ReadOnly = true;
            this.processPrivilege.Width = 72;
            // 
            // processPath
            // 
            this.processPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.processPath.HeaderText = "Path";
            this.processPath.Name = "processPath";
            this.processPath.ReadOnly = true;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 1000;
            this.refreshTimer.Tick += new System.EventHandler(this.RefreshTimerTick);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Location = new System.Drawing.Point(734, 450);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(87, 23);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FormSelectProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 478);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.dataProcessList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormSelectProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Process to Analyse";
            this.Load += new System.EventHandler(this.FormSelectProcessLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dataProcessList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataProcessList;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn processName;
        private System.Windows.Forms.DataGridViewTextBoxColumn processID;
        private System.Windows.Forms.DataGridViewTextBoxColumn processText;
        private System.Windows.Forms.DataGridViewTextBoxColumn processPrivilege;
        private System.Windows.Forms.DataGridViewTextBoxColumn processPath;
    }
}