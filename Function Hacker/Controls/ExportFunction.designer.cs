namespace FunctionHacker.Controls
{
    partial class ExportFunction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportFunction));
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioCodeCPlusPlus = new System.Windows.Forms.RadioButton();
            this.radioCodeDelphi = new System.Windows.Forms.RadioButton();
            this.radioCodeVbNet = new System.Windows.Forms.RadioButton();
            this.radioCodeCSharp = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioExportCallAndCallback = new System.Windows.Forms.RadioButton();
            this.radioExportCall = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextCode = new System.Windows.Forms.RichTextBox();
            this.txtCSharpCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioCodeCPlusPlus);
            this.panel1.Controls.Add(this.radioCodeDelphi);
            this.panel1.Controls.Add(this.radioCodeVbNet);
            this.panel1.Controls.Add(this.radioCodeCSharp);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Location = new System.Drawing.Point(3, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 122);
            this.panel1.TabIndex = 0;
            // 
            // radioCodeCPlusPlus
            // 
            this.radioCodeCPlusPlus.AutoSize = true;
            this.radioCodeCPlusPlus.Enabled = false;
            this.radioCodeCPlusPlus.Location = new System.Drawing.Point(17, 91);
            this.radioCodeCPlusPlus.Name = "radioCodeCPlusPlus";
            this.radioCodeCPlusPlus.Size = new System.Drawing.Size(44, 17);
            this.radioCodeCPlusPlus.TabIndex = 75;
            this.radioCodeCPlusPlus.Text = "C++";
            this.radioCodeCPlusPlus.UseVisualStyleBackColor = true;
            // 
            // radioCodeDelphi
            // 
            this.radioCodeDelphi.AutoSize = true;
            this.radioCodeDelphi.Enabled = false;
            this.radioCodeDelphi.Location = new System.Drawing.Point(17, 68);
            this.radioCodeDelphi.Name = "radioCodeDelphi";
            this.radioCodeDelphi.Size = new System.Drawing.Size(55, 17);
            this.radioCodeDelphi.TabIndex = 74;
            this.radioCodeDelphi.Text = "Delphi";
            this.radioCodeDelphi.UseVisualStyleBackColor = true;
            // 
            // radioCodeVbNet
            // 
            this.radioCodeVbNet.AutoSize = true;
            this.radioCodeVbNet.Enabled = false;
            this.radioCodeVbNet.Location = new System.Drawing.Point(17, 45);
            this.radioCodeVbNet.Name = "radioCodeVbNet";
            this.radioCodeVbNet.Size = new System.Drawing.Size(67, 17);
            this.radioCodeVbNet.TabIndex = 73;
            this.radioCodeVbNet.Text = "VB .NET";
            this.radioCodeVbNet.UseVisualStyleBackColor = true;
            // 
            // radioCodeCSharp
            // 
            this.radioCodeCSharp.AutoSize = true;
            this.radioCodeCSharp.Checked = true;
            this.radioCodeCSharp.Location = new System.Drawing.Point(17, 22);
            this.radioCodeCSharp.Name = "radioCodeCSharp";
            this.radioCodeCSharp.Size = new System.Drawing.Size(66, 17);
            this.radioCodeCSharp.TabIndex = 72;
            this.radioCodeCSharp.TabStop = true;
            this.radioCodeCSharp.Text = "c# .NET";
            this.radioCodeCSharp.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(116, 13);
            this.label15.TabIndex = 71;
            this.label15.Text = "Export Code Language";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.radioExportCallAndCallback);
            this.panel2.Controls.Add(this.radioExportCall);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(3, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(169, 73);
            this.panel2.TabIndex = 1;
            // 
            // radioExportCallAndCallback
            // 
            this.radioExportCallAndCallback.AutoSize = true;
            this.radioExportCallAndCallback.Enabled = false;
            this.radioExportCallAndCallback.Location = new System.Drawing.Point(17, 42);
            this.radioExportCallAndCallback.Name = "radioExportCallAndCallback";
            this.radioExportCallAndCallback.Size = new System.Drawing.Size(140, 17);
            this.radioExportCallAndCallback.TabIndex = 3;
            this.radioExportCallAndCallback.Text = "Export Call and Callback";
            this.radioExportCallAndCallback.UseVisualStyleBackColor = true;
            // 
            // radioExportCall
            // 
            this.radioExportCall.AutoSize = true;
            this.radioExportCall.Checked = true;
            this.radioExportCall.Location = new System.Drawing.Point(17, 21);
            this.radioExportCall.Name = "radioExportCall";
            this.radioExportCall.Size = new System.Drawing.Size(75, 17);
            this.radioExportCall.TabIndex = 2;
            this.radioExportCall.TabStop = true;
            this.radioExportCall.Text = "Export Call";
            this.radioExportCall.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 71;
            this.label1.Text = "Export Options";
            // 
            // richTextCode
            // 
            this.richTextCode.AcceptsTab = true;
            this.richTextCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextCode.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextCode.Location = new System.Drawing.Point(178, -1);
            this.richTextCode.Name = "richTextCode";
            this.richTextCode.ReadOnly = true;
            this.richTextCode.Size = new System.Drawing.Size(591, 389);
            this.richTextCode.TabIndex = 2;
            this.richTextCode.Text = resources.GetString("richTextCode.Text");
            this.richTextCode.WordWrap = false;
            this.richTextCode.TextChanged += new System.EventHandler(this.richTextCode_TextChanged);
            // 
            // txtCSharpCode
            // 
            this.txtCSharpCode.Location = new System.Drawing.Point(89, 246);
            this.txtCSharpCode.Multiline = true;
            this.txtCSharpCode.Name = "txtCSharpCode";
            this.txtCSharpCode.Size = new System.Drawing.Size(72, 35);
            this.txtCSharpCode.TabIndex = 3;
            this.txtCSharpCode.Text = resources.GetString("txtCSharpCode.Text");
            this.txtCSharpCode.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 249);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "invisible\r\nc# code box";
            this.label2.Visible = false;
            // 
            // ExportFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCSharpCode);
            this.Controls.Add(this.richTextCode);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ExportFunction";
            this.Size = new System.Drawing.Size(769, 388);
            this.Load += new System.EventHandler(this.ExportFunction_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioExportCallAndCallback;
        private System.Windows.Forms.RadioButton radioExportCall;
        private System.Windows.Forms.RadioButton radioCodeCSharp;
        private System.Windows.Forms.RadioButton radioCodeCPlusPlus;
        private System.Windows.Forms.RadioButton radioCodeDelphi;
        private System.Windows.Forms.RadioButton radioCodeVbNet;
        private System.Windows.Forms.RichTextBox richTextCode;
        private System.Windows.Forms.TextBox txtCSharpCode;
        private System.Windows.Forms.Label label2;

    }
}
