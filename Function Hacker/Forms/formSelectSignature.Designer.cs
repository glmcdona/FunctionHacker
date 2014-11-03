using System.Windows.Forms;

namespace FunctionHacker.Forms
{
    partial class formSignatures
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
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listSignatures = new System.Windows.Forms.ListView();
            this.SignatureName = new System.Windows.Forms.ColumnHeader();
            this.Path = new System.Windows.Forms.ColumnHeader();
            this.Count = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxOnlyWindows = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(482, 10);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(94, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(382, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // listSignatures
            // 
            this.listSignatures.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listSignatures.CheckBoxes = true;
            this.listSignatures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SignatureName,
            this.Path,
            this.Count});
            this.listSignatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSignatures.FullRowSelect = true;
            this.listSignatures.HideSelection = false;
            this.listSignatures.Location = new System.Drawing.Point(0, 0);
            this.listSignatures.Name = "listSignatures";
            this.listSignatures.Size = new System.Drawing.Size(588, 499);
            this.listSignatures.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listSignatures.TabIndex = 6;
            this.listSignatures.UseCompatibleStateImageBehavior = false;
            this.listSignatures.View = System.Windows.Forms.View.Details;
            // 
            // SignatureName
            // 
            this.SignatureName.Text = "Signature name";
            this.SignatureName.Width = 201;
            // 
            // Path
            // 
            this.Path.Text = "Signature Path";
            this.Path.Width = 272;
            // 
            // Count
            // 
            this.Count.Text = "Signatures";
            this.Count.Width = 109;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxOnlyWindows);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 499);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 45);
            this.panel1.TabIndex = 7;
            // 
            // checkBoxOnlyWindows
            // 
            this.checkBoxOnlyWindows.AutoSize = true;
            this.checkBoxOnlyWindows.CausesValidation = false;
            this.checkBoxOnlyWindows.Checked = true;
            this.checkBoxOnlyWindows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnlyWindows.Location = new System.Drawing.Point(12, 16);
            this.checkBoxOnlyWindows.Name = "checkBoxOnlyWindows";
            this.checkBoxOnlyWindows.Size = new System.Drawing.Size(170, 17);
            this.checkBoxOnlyWindows.TabIndex = 6;
            this.checkBoxOnlyWindows.Text = "Show only windows signatures";
            this.checkBoxOnlyWindows.UseVisualStyleBackColor = true;
            this.checkBoxOnlyWindows.CheckedChanged += new System.EventHandler(this.checkBoxOnlyWindows_CheckedChanged);
            // 
            // formSignatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 544);
            this.Controls.Add(this.listSignatures);
            this.Controls.Add(this.panel1);
            this.Name = "formSignatures";
            this.Text = "Select signature to apply";
            this.Load += new System.EventHandler(this.formSelectSignature_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView listSignatures;
        private System.Windows.Forms.ColumnHeader SignatureName;
        private System.Windows.Forms.ColumnHeader Count;
        private System.Windows.Forms.ColumnHeader Path;
        private System.Windows.Forms.Panel panel1;
        private CheckBox checkBoxOnlyWindows;

    }
}