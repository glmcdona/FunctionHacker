using System.Reflection;
using System.Windows.Forms;

namespace FunctionHacker.Controls
{
    partial class FunctionListViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /*
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
         * */




        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionListViewer));
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
          this.toolStrip1 = new System.Windows.Forms.ToolStrip();
          this.toolButtonRecord = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
          this.toolButtonFilter = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
          this.toolButtonFilterComparison = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
          this.toolButtonRename = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
          this.toolButtonSave = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripLabelFastFilter = new System.Windows.Forms.ToolStripLabel();
          this.toolStripTextBoxFastFilter = new System.Windows.Forms.ToolStripTextBox();
          this.toolStripLabelNumberOfRecords = new System.Windows.Forms.ToolStripLabel();
          this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripButtonTakeFastFilter = new System.Windows.Forms.ToolStripButton();
          this.contextMenuFunctions = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.disableSelectedFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.enableSelectedFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.sendCallsToSelectedFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
          this.deleteSelectedFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.renameSelectedFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.renameSelectedFunctionsOrdinalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
          this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.addressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.nameToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
          this.addressesNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
          this.loadNamesFromMapFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.importNamesFromIDASignaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripExportFunction = new System.Windows.Forms.ToolStripMenuItem();
          this.contextMenuStripCalls = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.toolStripCallGoto = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripCallDisable = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripCallEnable = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripCallSend = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripCallResend = new System.Windows.Forms.ToolStripMenuItem();
          this.splitContainerFunctionsPlaybar = new System.Windows.Forms.SplitContainer();
          this.splitContainer1 = new System.Windows.Forms.SplitContainer();
          this.tabControlFunctionsCalls = new System.Windows.Forms.TabControl();
          this.tabPageFunctionList = new System.Windows.Forms.TabPage();
          this.dataGridFunctions = new FunctionHacker.Controls.DataGridViewEx();
          this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.callCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.arguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.tabPageCallList = new System.Windows.Forms.TabPage();
          this.splitContainer2 = new System.Windows.Forms.SplitContainer();
          this.dataGridCalls = new System.Windows.Forms.DataGridView();
          this.colCallNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.colSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.colDestination = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.colArguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridCallArguments = new FunctionHacker.Controls.DataGridViewCall();
          this.colArgumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.colArgumentType = new System.Windows.Forms.DataGridViewComboBoxColumn();
          this.colArgumentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.controlVisMain = new FunctionHacker.Classes.Visualization.oVisMain();
          this.buttonBreakdown = new System.Windows.Forms.Button();
          this.contextMenuPlaybar = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
          this.menuBreakdownModule = new System.Windows.Forms.ToolStripMenuItem();
          this.menuBreakdownThread = new System.Windows.Forms.ToolStripMenuItem();
          this.menuBreakdownCallType = new System.Windows.Forms.ToolStripMenuItem();
          this.menuBreakdownCallFrequency = new System.Windows.Forms.ToolStripMenuItem();
          this.binsToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
          this.binsToolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
          this.binsToolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
          this.binsToolStripMenuItem31 = new System.Windows.Forms.ToolStripMenuItem();
          this.binsToolStripMenuItem41 = new System.Windows.Forms.ToolStripMenuItem();
          this.binsToolStripMenuItem51 = new System.Windows.Forms.ToolStripMenuItem();
          this.addPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.moduleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
          this.itemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.menuAddPlotIntermodular = new System.Windows.Forms.ToolStripMenuItem();
          this.menuAddPlotIntramodular = new System.Windows.Forms.ToolStripMenuItem();
          this.menuAddPlotHasString = new System.Windows.Forms.ToolStripMenuItem();
          this.menuAddPlotFromFilter = new System.Windows.Forms.ToolStripMenuItem();
          this.menuRemoveAllPlots = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
          this.enableStartRecording = new System.Windows.Forms.Timer(this.components);
          this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.controlVisPlayBar = new FunctionHacker.Classes.Visualization.oVisPlayBar();
          this.toolStrip1.SuspendLayout();
          this.contextMenuFunctions.SuspendLayout();
          this.contextMenuStripCalls.SuspendLayout();
          this.splitContainerFunctionsPlaybar.Panel1.SuspendLayout();
          this.splitContainerFunctionsPlaybar.Panel2.SuspendLayout();
          this.splitContainerFunctionsPlaybar.SuspendLayout();
          this.splitContainer1.Panel1.SuspendLayout();
          this.splitContainer1.Panel2.SuspendLayout();
          this.splitContainer1.SuspendLayout();
          this.tabControlFunctionsCalls.SuspendLayout();
          this.tabPageFunctionList.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.dataGridFunctions)).BeginInit();
          this.tabPageCallList.SuspendLayout();
          this.splitContainer2.Panel1.SuspendLayout();
          this.splitContainer2.Panel2.SuspendLayout();
          this.splitContainer2.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.dataGridCalls)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.dataGridCallArguments)).BeginInit();
          this.contextMenuPlaybar.SuspendLayout();
          this.SuspendLayout();
          // 
          // toolStrip1
          // 
          this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonRecord,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.toolButtonFilter,
            this.toolStripSeparator3,
            this.toolButtonFilterComparison,
            this.toolStripSeparator4,
            this.toolStripSeparator5,
            this.toolButtonRename,
            this.toolStripSeparator6,
            this.toolButtonSave,
            this.toolStripSeparator12,
            this.toolStripSeparator11,
            this.toolStripLabelFastFilter,
            this.toolStripTextBoxFastFilter,
            this.toolStripLabelNumberOfRecords,
            this.toolStripSeparator14,
            this.toolStripSeparator13,
            this.toolStripButtonTakeFastFilter});
          this.toolStrip1.Location = new System.Drawing.Point(0, 0);
          this.toolStrip1.Name = "toolStrip1";
          this.toolStrip1.Size = new System.Drawing.Size(1062, 25);
          this.toolStrip1.TabIndex = 0;
          this.toolStrip1.Text = "toolStrip1";
          // 
          // toolButtonRecord
          // 
          this.toolButtonRecord.CheckOnClick = true;
          this.toolButtonRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolButtonRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolButtonRecord.Name = "toolButtonRecord";
          this.toolButtonRecord.Size = new System.Drawing.Size(92, 22);
          this.toolButtonRecord.Text = "Start Recording";
          this.toolButtonRecord.ToolTipText = "(CTRL+F5)";
          this.toolButtonRecord.Click += new System.EventHandler(this.toolButtonRecord_Click);
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
          // 
          // toolStripSeparator2
          // 
          this.toolStripSeparator2.Name = "toolStripSeparator2";
          this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
          // 
          // toolButtonFilter
          // 
          this.toolButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolButtonFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonFilter.Image")));
          this.toolButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolButtonFilter.Name = "toolButtonFilter";
          this.toolButtonFilter.Size = new System.Drawing.Size(37, 22);
          this.toolButtonFilter.Text = "Filter";
          this.toolButtonFilter.Click += new System.EventHandler(this.toolButtonFilter_Click);
          // 
          // toolStripSeparator3
          // 
          this.toolStripSeparator3.Name = "toolStripSeparator3";
          this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
          this.toolStripSeparator3.Visible = false;
          // 
          // toolButtonFilterComparison
          // 
          this.toolButtonFilterComparison.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolButtonFilterComparison.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonFilterComparison.Image")));
          this.toolButtonFilterComparison.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolButtonFilterComparison.Name = "toolButtonFilterComparison";
          this.toolButtonFilterComparison.Size = new System.Drawing.Size(121, 22);
          this.toolButtonFilterComparison.Text = "Filter by Comparison";
          this.toolButtonFilterComparison.Visible = false;
          this.toolButtonFilterComparison.Click += new System.EventHandler(this.toolButtonFilterComparison_Click);
          // 
          // toolStripSeparator4
          // 
          this.toolStripSeparator4.Name = "toolStripSeparator4";
          this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
          this.toolStripSeparator4.Visible = false;
          // 
          // toolStripSeparator5
          // 
          this.toolStripSeparator5.Name = "toolStripSeparator5";
          this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
          this.toolStripSeparator5.Visible = false;
          // 
          // toolButtonRename
          // 
          this.toolButtonRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolButtonRename.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonRename.Image")));
          this.toolButtonRename.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolButtonRename.Name = "toolButtonRename";
          this.toolButtonRename.Size = new System.Drawing.Size(54, 22);
          this.toolButtonRename.Text = "Rename";
          this.toolButtonRename.Visible = false;
          this.toolButtonRename.Click += new System.EventHandler(this.toolButtonRename_Click);
          // 
          // toolStripSeparator6
          // 
          this.toolStripSeparator6.Name = "toolStripSeparator6";
          this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
          this.toolStripSeparator6.Visible = false;
          // 
          // toolButtonSave
          // 
          this.toolButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonSave.Image")));
          this.toolButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolButtonSave.Name = "toolButtonSave";
          this.toolButtonSave.Size = new System.Drawing.Size(35, 22);
          this.toolButtonSave.Text = "Save";
          this.toolButtonSave.Visible = false;
          this.toolButtonSave.Click += new System.EventHandler(this.toolButtonSave_Click);
          // 
          // toolStripSeparator12
          // 
          this.toolStripSeparator12.Name = "toolStripSeparator12";
          this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
          // 
          // toolStripSeparator11
          // 
          this.toolStripSeparator11.Name = "toolStripSeparator11";
          this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
          // 
          // toolStripLabelFastFilter
          // 
          this.toolStripLabelFastFilter.BackColor = System.Drawing.SystemColors.Control;
          this.toolStripLabelFastFilter.ForeColor = System.Drawing.SystemColors.Highlight;
          this.toolStripLabelFastFilter.Name = "toolStripLabelFastFilter";
          this.toolStripLabelFastFilter.Size = new System.Drawing.Size(70, 22);
          this.toolStripLabelFastFilter.Text = "Quick Filter:";
          // 
          // toolStripTextBoxFastFilter
          // 
          this.toolStripTextBoxFastFilter.Name = "toolStripTextBoxFastFilter";
          this.toolStripTextBoxFastFilter.Size = new System.Drawing.Size(100, 25);
          this.toolStripTextBoxFastFilter.TextChanged += new System.EventHandler(this.ToolStripTextBoxFastFilterTextChanged);
          // 
          // toolStripLabelNumberOfRecords
          // 
          this.toolStripLabelNumberOfRecords.ForeColor = System.Drawing.SystemColors.Highlight;
          this.toolStripLabelNumberOfRecords.Name = "toolStripLabelNumberOfRecords";
          this.toolStripLabelNumberOfRecords.Size = new System.Drawing.Size(71, 22);
          this.toolStripLabelNumberOfRecords.Text = "Functions: 0";
          // 
          // toolStripSeparator14
          // 
          this.toolStripSeparator14.Name = "toolStripSeparator14";
          this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
          // 
          // toolStripSeparator13
          // 
          this.toolStripSeparator13.Name = "toolStripSeparator13";
          this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
          // 
          // toolStripButtonTakeFastFilter
          // 
          this.toolStripButtonTakeFastFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolStripButtonTakeFastFilter.ForeColor = System.Drawing.SystemColors.Highlight;
          this.toolStripButtonTakeFastFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTakeFastFilter.Image")));
          this.toolStripButtonTakeFastFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolStripButtonTakeFastFilter.Name = "toolStripButtonTakeFastFilter";
          this.toolStripButtonTakeFastFilter.Size = new System.Drawing.Size(150, 22);
          this.toolStripButtonTakeFastFilter.Text = "New Tab from Quick Filter";
          this.toolStripButtonTakeFastFilter.ToolTipText = "Take functions from fast filter into new monitor tab";
          this.toolStripButtonTakeFastFilter.Click += new System.EventHandler(this.toolStripButtonTakeFastFilter_Click);
          // 
          // contextMenuFunctions
          // 
          this.contextMenuFunctions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableSelectedFunctionsToolStripMenuItem,
            this.enableSelectedFunctionsToolStripMenuItem,
            this.sendCallsToSelectedFunctionToolStripMenuItem,
            this.toolStripSeparator7,
            this.toolStripMenuItem1,
            this.deleteSelectedFunctionsToolStripMenuItem,
            this.renameSelectedFunctionToolStripMenuItem,
            this.renameSelectedFunctionsOrdinalToolStripMenuItem,
            this.toolStripSeparator16,
            this.copyToClipboardToolStripMenuItem,
            this.toolStripSeparator8,
            this.loadNamesFromMapFileToolStripMenuItem,
            this.importNamesFromIDASignaturesToolStripMenuItem,
            this.toolStripExportFunction});
          this.contextMenuFunctions.Name = "contextMenuFunctions";
          this.contextMenuFunctions.Size = new System.Drawing.Size(385, 264);
          this.contextMenuFunctions.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuFunctions_Opening);
          // 
          // disableSelectedFunctionsToolStripMenuItem
          // 
          this.disableSelectedFunctionsToolStripMenuItem.Name = "disableSelectedFunctionsToolStripMenuItem";
          this.disableSelectedFunctionsToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.disableSelectedFunctionsToolStripMenuItem.Text = "Disable Selected Function(s)";
          this.disableSelectedFunctionsToolStripMenuItem.Click += new System.EventHandler(this.disableSelectedFunctionsToolStripMenuItem_Click);
          // 
          // enableSelectedFunctionsToolStripMenuItem
          // 
          this.enableSelectedFunctionsToolStripMenuItem.Name = "enableSelectedFunctionsToolStripMenuItem";
          this.enableSelectedFunctionsToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.enableSelectedFunctionsToolStripMenuItem.Text = "Enable Selected Function(s)";
          this.enableSelectedFunctionsToolStripMenuItem.Click += new System.EventHandler(this.enableSelectedFunctionsToolStripMenuItem_Click);
          // 
          // sendCallsToSelectedFunctionToolStripMenuItem
          // 
          this.sendCallsToSelectedFunctionToolStripMenuItem.Name = "sendCallsToSelectedFunctionToolStripMenuItem";
          this.sendCallsToSelectedFunctionToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.sendCallsToSelectedFunctionToolStripMenuItem.Text = "Send Calls to Selected Function";
          this.sendCallsToSelectedFunctionToolStripMenuItem.Click += new System.EventHandler(this.sendCallsToSelectedFunctionToolStripMenuItem_Click);
          // 
          // toolStripSeparator7
          // 
          this.toolStripSeparator7.Name = "toolStripSeparator7";
          this.toolStripSeparator7.Size = new System.Drawing.Size(381, 6);
          // 
          // toolStripMenuItem1
          // 
          this.toolStripMenuItem1.Name = "toolStripMenuItem1";
          this.toolStripMenuItem1.Size = new System.Drawing.Size(384, 22);
          this.toolStripMenuItem1.Text = "Create New Function List from Selected Function(s)";
          this.toolStripMenuItem1.Click += new System.EventHandler(this.newListSelectedFunctionsToolStripMenuItem_Click);
          // 
          // deleteSelectedFunctionsToolStripMenuItem
          // 
          this.deleteSelectedFunctionsToolStripMenuItem.Name = "deleteSelectedFunctionsToolStripMenuItem";
          this.deleteSelectedFunctionsToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.deleteSelectedFunctionsToolStripMenuItem.Text = "Delete Selected Function(s)";
          this.deleteSelectedFunctionsToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedFunctionsToolStripMenuItem_Click);
          // 
          // renameSelectedFunctionToolStripMenuItem
          // 
          this.renameSelectedFunctionToolStripMenuItem.Name = "renameSelectedFunctionToolStripMenuItem";
          this.renameSelectedFunctionToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.renameSelectedFunctionToolStripMenuItem.Text = "Rename Selected Function";
          this.renameSelectedFunctionToolStripMenuItem.Click += new System.EventHandler(this.renameSelectedFunctionToolStripMenuItem_Click);
          // 
          // renameSelectedFunctionsOrdinalToolStripMenuItem
          // 
          this.renameSelectedFunctionsOrdinalToolStripMenuItem.Name = "renameSelectedFunctionsOrdinalToolStripMenuItem";
          this.renameSelectedFunctionsOrdinalToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.renameSelectedFunctionsOrdinalToolStripMenuItem.Text = "Rename Selected Functions(s) - Ordinal ";
          this.renameSelectedFunctionsOrdinalToolStripMenuItem.Click += new System.EventHandler(this.renameSelectedFunctionsOrdinalToolStripMenuItem_Click);
          // 
          // toolStripSeparator16
          // 
          this.toolStripSeparator16.Name = "toolStripSeparator16";
          this.toolStripSeparator16.Size = new System.Drawing.Size(381, 6);
          // 
          // copyToClipboardToolStripMenuItem
          // 
          this.copyToClipboardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addressToolStripMenuItem,
            this.nameToolStripMenu,
            this.addressesNamesToolStripMenuItem});
          this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
          this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
          // 
          // addressToolStripMenuItem
          // 
          this.addressToolStripMenuItem.Name = "addressToolStripMenuItem";
          this.addressToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
          this.addressToolStripMenuItem.Text = "Address(es)";
          this.addressToolStripMenuItem.Click += new System.EventHandler(this.addressToolStripMenuItem_Click);
          // 
          // nameToolStripMenu
          // 
          this.nameToolStripMenu.Name = "nameToolStripMenu";
          this.nameToolStripMenu.Size = new System.Drawing.Size(194, 22);
          this.nameToolStripMenu.Text = "Name(s)";
          this.nameToolStripMenu.Click += new System.EventHandler(this.nameToolStripMenu_Click);
          // 
          // addressesNamesToolStripMenuItem
          // 
          this.addressesNamesToolStripMenuItem.Name = "addressesNamesToolStripMenuItem";
          this.addressesNamesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
          this.addressesNamesToolStripMenuItem.Text = "Address(es) + Name(s)";
          this.addressesNamesToolStripMenuItem.Click += new System.EventHandler(this.addressesNamesToolStripMenuItem_Click);
          // 
          // toolStripSeparator8
          // 
          this.toolStripSeparator8.Name = "toolStripSeparator8";
          this.toolStripSeparator8.Size = new System.Drawing.Size(381, 6);
          // 
          // loadNamesFromMapFileToolStripMenuItem
          // 
          this.loadNamesFromMapFileToolStripMenuItem.Name = "loadNamesFromMapFileToolStripMenuItem";
          this.loadNamesFromMapFileToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.loadNamesFromMapFileToolStripMenuItem.Text = "Import Names from .map file";
          this.loadNamesFromMapFileToolStripMenuItem.Click += new System.EventHandler(this.LoadNamesFromMapFileToolStripMenuItemClick);
          // 
          // importNamesFromIDASignaturesToolStripMenuItem
          // 
          this.importNamesFromIDASignaturesToolStripMenuItem.Name = "importNamesFromIDASignaturesToolStripMenuItem";
          this.importNamesFromIDASignaturesToolStripMenuItem.Size = new System.Drawing.Size(384, 22);
          this.importNamesFromIDASignaturesToolStripMenuItem.Text = "Import Names from IDA signatures for selected functions...";
          this.importNamesFromIDASignaturesToolStripMenuItem.Click += new System.EventHandler(this.importNamesFromIDASignaturesToolStripMenuItem_Click);
          // 
          // toolStripExportFunction
          // 
          this.toolStripExportFunction.Name = "toolStripExportFunction";
          this.toolStripExportFunction.Size = new System.Drawing.Size(384, 22);
          this.toolStripExportFunction.Text = "Export Selected Function to C#";
          this.toolStripExportFunction.Click += new System.EventHandler(this.toolStripExportFunction_Click);
          // 
          // contextMenuStripCalls
          // 
          this.contextMenuStripCalls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCallGoto,
            this.toolStripSeparator10,
            this.toolStripCallDisable,
            this.toolStripCallEnable,
            this.toolStripSeparator9,
            this.toolStripCallSend,
            this.toolStripCallResend});
          this.contextMenuStripCalls.Name = "contextMenuFunctions";
          this.contextMenuStripCalls.Size = new System.Drawing.Size(240, 126);
          this.contextMenuStripCalls.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripCalls_Opening);
          // 
          // toolStripCallGoto
          // 
          this.toolStripCallGoto.Name = "toolStripCallGoto";
          this.toolStripCallGoto.Size = new System.Drawing.Size(239, 22);
          this.toolStripCallGoto.Text = "Goto Function";
          this.toolStripCallGoto.Click += new System.EventHandler(this.toolStripCallGoto_Click);
          // 
          // toolStripSeparator10
          // 
          this.toolStripSeparator10.Name = "toolStripSeparator10";
          this.toolStripSeparator10.Size = new System.Drawing.Size(236, 6);
          // 
          // toolStripCallDisable
          // 
          this.toolStripCallDisable.Name = "toolStripCallDisable";
          this.toolStripCallDisable.Size = new System.Drawing.Size(239, 22);
          this.toolStripCallDisable.Text = "Disable Selected Function(s)";
          this.toolStripCallDisable.Click += new System.EventHandler(this.toolStripCallDisable_Click);
          // 
          // toolStripCallEnable
          // 
          this.toolStripCallEnable.Name = "toolStripCallEnable";
          this.toolStripCallEnable.Size = new System.Drawing.Size(239, 22);
          this.toolStripCallEnable.Text = "Enable Selected Function(s)";
          this.toolStripCallEnable.Click += new System.EventHandler(this.toolStripCallEnable_Click);
          // 
          // toolStripSeparator9
          // 
          this.toolStripSeparator9.Name = "toolStripSeparator9";
          this.toolStripSeparator9.Size = new System.Drawing.Size(236, 6);
          // 
          // toolStripCallSend
          // 
          this.toolStripCallSend.Name = "toolStripCallSend";
          this.toolStripCallSend.Size = new System.Drawing.Size(239, 22);
          this.toolStripCallSend.Text = "Send Calls to Selected Function";
          this.toolStripCallSend.Click += new System.EventHandler(this.toolStripCallSend_Click);
          // 
          // toolStripCallResend
          // 
          this.toolStripCallResend.Enabled = false;
          this.toolStripCallResend.Name = "toolStripCallResend";
          this.toolStripCallResend.Size = new System.Drawing.Size(239, 22);
          this.toolStripCallResend.Text = "Resend this Call";
          // 
          // splitContainerFunctionsPlaybar
          // 
          this.splitContainerFunctionsPlaybar.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainerFunctionsPlaybar.Location = new System.Drawing.Point(0, 25);
          this.splitContainerFunctionsPlaybar.Name = "splitContainerFunctionsPlaybar";
          this.splitContainerFunctionsPlaybar.Orientation = System.Windows.Forms.Orientation.Horizontal;
          // 
          // splitContainerFunctionsPlaybar.Panel1
          // 
          this.splitContainerFunctionsPlaybar.Panel1.Controls.Add(this.splitContainer1);
          // 
          // splitContainerFunctionsPlaybar.Panel2
          // 
          this.splitContainerFunctionsPlaybar.Panel2.BackColor = System.Drawing.Color.Black;
          this.splitContainerFunctionsPlaybar.Panel2.Controls.Add(this.buttonBreakdown);
          this.splitContainerFunctionsPlaybar.Size = new System.Drawing.Size(1062, 661);
          this.splitContainerFunctionsPlaybar.SplitterDistance = 606;
          this.splitContainerFunctionsPlaybar.TabIndex = 6;
          // 
          // splitContainer1
          // 
          this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.splitContainer1.Location = new System.Drawing.Point(3, 0);
          this.splitContainer1.Name = "splitContainer1";
          // 
          // splitContainer1.Panel1
          // 
          this.splitContainer1.Panel1.Controls.Add(this.tabControlFunctionsCalls);
          // 
          // splitContainer1.Panel2
          // 
          this.splitContainer1.Panel2.Controls.Add(this.controlVisMain);
          this.splitContainer1.Size = new System.Drawing.Size(1056, 606);
          this.splitContainer1.SplitterDistance = 715;
          this.splitContainer1.TabIndex = 6;
          // 
          // tabControlFunctionsCalls
          // 
          this.tabControlFunctionsCalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.tabControlFunctionsCalls.Controls.Add(this.tabPageFunctionList);
          this.tabControlFunctionsCalls.Controls.Add(this.tabPageCallList);
          this.tabControlFunctionsCalls.Location = new System.Drawing.Point(3, 0);
          this.tabControlFunctionsCalls.Name = "tabControlFunctionsCalls";
          this.tabControlFunctionsCalls.SelectedIndex = 0;
          this.tabControlFunctionsCalls.Size = new System.Drawing.Size(708, 605);
          this.tabControlFunctionsCalls.TabIndex = 5;
          this.tabControlFunctionsCalls.SelectedIndexChanged += new System.EventHandler(this.TabControlFunctionsCallsSelectedIndexChanged);
          // 
          // tabPageFunctionList
          // 
          this.tabPageFunctionList.Controls.Add(this.dataGridFunctions);
          this.tabPageFunctionList.Location = new System.Drawing.Point(4, 22);
          this.tabPageFunctionList.Name = "tabPageFunctionList";
          this.tabPageFunctionList.Padding = new System.Windows.Forms.Padding(3);
          this.tabPageFunctionList.Size = new System.Drawing.Size(700, 579);
          this.tabPageFunctionList.TabIndex = 0;
          this.tabPageFunctionList.Text = "Function List";
          this.tabPageFunctionList.UseVisualStyleBackColor = true;
          // 
          // dataGridFunctions
          // 
          this.dataGridFunctions.AllowUserToAddRows = false;
          this.dataGridFunctions.AllowUserToDeleteRows = false;
          this.dataGridFunctions.AllowUserToResizeRows = false;
          this.dataGridFunctions.BackgroundColor = System.Drawing.Color.White;
          this.dataGridFunctions.BorderStyle = System.Windows.Forms.BorderStyle.None;
          this.dataGridFunctions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
          this.dataGridFunctions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.dataGridFunctions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.description,
            this.address,
            this.callCount,
            this.arguments});
          this.dataGridFunctions.ContextMenuStrip = this.contextMenuFunctions;
          this.dataGridFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
          this.dataGridFunctions.Location = new System.Drawing.Point(3, 3);
          this.dataGridFunctions.Name = "dataGridFunctions";
          this.dataGridFunctions.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
          this.dataGridFunctions.RowHeadersVisible = false;
          this.dataGridFunctions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.dataGridFunctions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.dataGridFunctions.Size = new System.Drawing.Size(694, 573);
          this.dataGridFunctions.Suspended = false;
          this.dataGridFunctions.TabIndex = 0;
          this.dataGridFunctions.VirtualMode = true;
          this.dataGridFunctions.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridFunctions_CellBeginEdit);
          this.dataGridFunctions.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridFunctions_CellValueNeeded);
          this.dataGridFunctions.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridFunctionsRowsAdded);
          this.dataGridFunctions.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridFunctions_CellValuePushed);
          this.dataGridFunctions.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DataGridFunctionsRowsRemoved);
          // 
          // description
          // 
          this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
          this.description.HeaderText = "Description";
          this.description.Name = "description";
          // 
          // address
          // 
          this.address.HeaderText = "Address";
          this.address.Name = "address";
          // 
          // callCount
          // 
          this.callCount.HeaderText = "Call Count: Total/Selected Region";
          this.callCount.Name = "callCount";
          this.callCount.ReadOnly = true;
          // 
          // arguments
          // 
          this.arguments.HeaderText = "Number of Arguments";
          this.arguments.Name = "arguments";
          this.arguments.ReadOnly = true;
          // 
          // tabPageCallList
          // 
          this.tabPageCallList.Controls.Add(this.splitContainer2);
          this.tabPageCallList.Location = new System.Drawing.Point(4, 22);
          this.tabPageCallList.Name = "tabPageCallList";
          this.tabPageCallList.Padding = new System.Windows.Forms.Padding(3);
          this.tabPageCallList.Size = new System.Drawing.Size(700, 579);
          this.tabPageCallList.TabIndex = 1;
          this.tabPageCallList.Text = "Call List";
          this.tabPageCallList.UseVisualStyleBackColor = true;
          // 
          // splitContainer2
          // 
          this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer2.Location = new System.Drawing.Point(3, 3);
          this.splitContainer2.Name = "splitContainer2";
          this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
          // 
          // splitContainer2.Panel1
          // 
          this.splitContainer2.Panel1.Controls.Add(this.dataGridCalls);
          // 
          // splitContainer2.Panel2
          // 
          this.splitContainer2.Panel2.Controls.Add(this.dataGridCallArguments);
          this.splitContainer2.Size = new System.Drawing.Size(694, 573);
          this.splitContainer2.SplitterDistance = 426;
          this.splitContainer2.TabIndex = 3;
          // 
          // dataGridCalls
          // 
          this.dataGridCalls.AllowUserToAddRows = false;
          this.dataGridCalls.AllowUserToDeleteRows = false;
          this.dataGridCalls.AllowUserToResizeRows = false;
          this.dataGridCalls.BackgroundColor = System.Drawing.Color.White;
          this.dataGridCalls.BorderStyle = System.Windows.Forms.BorderStyle.None;
          this.dataGridCalls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.dataGridCalls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCallNumber,
            this.colSource,
            this.colDestination,
            this.colArguments});
          this.dataGridCalls.ContextMenuStrip = this.contextMenuStripCalls;
          dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
          dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
          dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.dataGridCalls.DefaultCellStyle = dataGridViewCellStyle1;
          this.dataGridCalls.Dock = System.Windows.Forms.DockStyle.Fill;
          this.dataGridCalls.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(200)))));
          this.dataGridCalls.Location = new System.Drawing.Point(0, 0);
          this.dataGridCalls.Name = "dataGridCalls";
          this.dataGridCalls.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
          this.dataGridCalls.RowHeadersVisible = false;
          this.dataGridCalls.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.dataGridCalls.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.dataGridCalls.RowTemplate.Height = 24;
          this.dataGridCalls.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.dataGridCalls.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.dataGridCalls.Size = new System.Drawing.Size(694, 426);
          this.dataGridCalls.TabIndex = 1;
          this.dataGridCalls.VirtualMode = true;
          this.dataGridCalls.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridCalls_RowPrepaint);
          this.dataGridCalls.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridCalls_CellValueNeeded);
          this.dataGridCalls.SelectionChanged += new System.EventHandler(this.dataGridCalls_SelectionChanged);
          this.dataGridCalls.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridCalls_CellContentClick);
          // 
          // colCallNumber
          // 
          this.colCallNumber.Frozen = true;
          this.colCallNumber.HeaderText = "Call Number";
          this.colCallNumber.Name = "colCallNumber";
          this.colCallNumber.Width = 50;
          // 
          // colSource
          // 
          this.colSource.Frozen = true;
          this.colSource.HeaderText = "Source";
          this.colSource.Name = "colSource";
          this.colSource.Width = 150;
          // 
          // colDestination
          // 
          this.colDestination.Frozen = true;
          this.colDestination.HeaderText = "Destination";
          this.colDestination.Name = "colDestination";
          this.colDestination.Width = 150;
          // 
          // colArguments
          // 
          this.colArguments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
          this.colArguments.HeaderText = "Arguments";
          this.colArguments.Name = "colArguments";
          // 
          // dataGridCallArguments
          // 
          this.dataGridCallArguments.AllowUserToAddRows = false;
          this.dataGridCallArguments.AllowUserToDeleteRows = false;
          this.dataGridCallArguments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
          this.dataGridCallArguments.BackgroundColor = System.Drawing.Color.White;
          this.dataGridCallArguments.BorderStyle = System.Windows.Forms.BorderStyle.None;
          this.dataGridCallArguments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
          this.dataGridCallArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.dataGridCallArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colArgumentName,
            this.colArgumentType,
            this.colArgumentValue});
          dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
          dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
          dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.dataGridCallArguments.DefaultCellStyle = dataGridViewCellStyle2;
          this.dataGridCallArguments.Dock = System.Windows.Forms.DockStyle.Fill;
          this.dataGridCallArguments.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
          this.dataGridCallArguments.Location = new System.Drawing.Point(0, 0);
          this.dataGridCallArguments.Name = "dataGridCallArguments";
          this.dataGridCallArguments.RowHeadersVisible = false;
          this.dataGridCallArguments.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.dataGridCallArguments.RowTemplate.Height = 24;
          this.dataGridCallArguments.Size = new System.Drawing.Size(694, 143);
          this.dataGridCallArguments.TabIndex = 0;
          // 
          // colArgumentName
          // 
          this.colArgumentName.HeaderText = "Argument Name";
          this.colArgumentName.Name = "colArgumentName";
          this.colArgumentName.Width = 150;
          // 
          // colArgumentType
          // 
          this.colArgumentType.HeaderText = "Type";
          this.colArgumentType.Name = "colArgumentType";
          this.colArgumentType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
          this.colArgumentType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
          this.colArgumentType.Width = 120;
          // 
          // colArgumentValue
          // 
          this.colArgumentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
          this.colArgumentValue.HeaderText = "Value";
          this.colArgumentValue.Name = "colArgumentValue";
          // 
          // controlVisMain
          // 
          this.controlVisMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.controlVisMain.BackColor = System.Drawing.Color.Black;
          this.controlVisMain.Location = new System.Drawing.Point(-2, 0);
          this.controlVisMain.Name = "controlVisMain";
          this.controlVisMain.Size = new System.Drawing.Size(338, 605);
          this.controlVisMain.TabIndex = 0;
          // 
          // buttonBreakdown
          // 
          this.buttonBreakdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.buttonBreakdown.BackColor = System.Drawing.Color.Yellow;
          this.buttonBreakdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          this.buttonBreakdown.Location = new System.Drawing.Point(0, 34);
          this.buttonBreakdown.Name = "buttonBreakdown";
          this.buttonBreakdown.Size = new System.Drawing.Size(21, 17);
          this.buttonBreakdown.TabIndex = 0;
          this.buttonBreakdown.Text = "^";
          this.buttonBreakdown.UseVisualStyleBackColor = false;
          this.buttonBreakdown.Click += new System.EventHandler(this.buttonBreakdown_Click);
          // 
          // contextMenuPlaybar
          // 
          this.contextMenuPlaybar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.addPToolStripMenuItem,
            this.menuRemoveAllPlots,
            this.toolStripSeparator15});
          this.contextMenuPlaybar.Name = "contextMenuPlaybar";
          this.contextMenuPlaybar.Size = new System.Drawing.Size(164, 76);
          this.contextMenuPlaybar.Text = "Breakdown";
          // 
          // toolStripMenuItem2
          // 
          this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBreakdownModule,
            this.menuBreakdownThread,
            this.menuBreakdownCallType,
            this.menuBreakdownCallFrequency});
          this.toolStripMenuItem2.Name = "toolStripMenuItem2";
          this.toolStripMenuItem2.Size = new System.Drawing.Size(163, 22);
          this.toolStripMenuItem2.Text = "Breakdown by";
          // 
          // menuBreakdownModule
          // 
          this.menuBreakdownModule.Name = "menuBreakdownModule";
          this.menuBreakdownModule.Size = new System.Drawing.Size(213, 22);
          this.menuBreakdownModule.Text = "Module";
          this.menuBreakdownModule.ToolTipText = "Creates a plot for each module.";
          this.menuBreakdownModule.Click += new System.EventHandler(this.menuBreakdownModule_Click);
          // 
          // menuBreakdownThread
          // 
          this.menuBreakdownThread.Name = "menuBreakdownThread";
          this.menuBreakdownThread.Size = new System.Drawing.Size(213, 22);
          this.menuBreakdownThread.Text = "Thread";
          this.menuBreakdownThread.ToolTipText = "Creates a plot for each thread.";
          this.menuBreakdownThread.Click += new System.EventHandler(this.menuBreakdownThread_Click);
          // 
          // menuBreakdownCallType
          // 
          this.menuBreakdownCallType.Name = "menuBreakdownCallType";
          this.menuBreakdownCallType.Size = new System.Drawing.Size(213, 22);
          this.menuBreakdownCallType.Text = "Intermodule/Intramodular";
          this.menuBreakdownCallType.ToolTipText = "Creates a plot intermodular calls and a plot for intramodular calls.";
          this.menuBreakdownCallType.Click += new System.EventHandler(this.menuBreakdownCallType_Click);
          // 
          // menuBreakdownCallFrequency
          // 
          this.menuBreakdownCallFrequency.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.binsToolStripMenuItem6,
            this.binsToolStripMenuItem11,
            this.binsToolStripMenuItem21,
            this.binsToolStripMenuItem31,
            this.binsToolStripMenuItem41,
            this.binsToolStripMenuItem51});
          this.menuBreakdownCallFrequency.Name = "menuBreakdownCallFrequency";
          this.menuBreakdownCallFrequency.Size = new System.Drawing.Size(213, 22);
          this.menuBreakdownCallFrequency.Text = "Call Frequency";
          this.menuBreakdownCallFrequency.ToolTipText = resources.GetString("menuBreakdownCallFrequency.ToolTipText");
          // 
          // binsToolStripMenuItem6
          // 
          this.binsToolStripMenuItem6.Name = "binsToolStripMenuItem6";
          this.binsToolStripMenuItem6.Size = new System.Drawing.Size(111, 22);
          this.binsToolStripMenuItem6.Text = "6 bins";
          this.binsToolStripMenuItem6.ToolTipText = "Breaks down the calls based on how many times the target function has been called" +
              ". eg. [0 to 5 calls], [6 to 10 calls], [11 to 15 calls], etc.";
          this.binsToolStripMenuItem6.Click += new System.EventHandler(this.binsToolStripMenuItem6_Click);
          // 
          // binsToolStripMenuItem11
          // 
          this.binsToolStripMenuItem11.Name = "binsToolStripMenuItem11";
          this.binsToolStripMenuItem11.Size = new System.Drawing.Size(111, 22);
          this.binsToolStripMenuItem11.Text = "11 bins";
          this.binsToolStripMenuItem11.ToolTipText = "Breaks down the calls based on how many times the target function has been called" +
              ". eg. [0 to 5 calls], [6 to 10 calls], [11 to 15 calls], etc.";
          this.binsToolStripMenuItem11.Click += new System.EventHandler(this.binsToolStripMenuItem11_Click);
          // 
          // binsToolStripMenuItem21
          // 
          this.binsToolStripMenuItem21.Name = "binsToolStripMenuItem21";
          this.binsToolStripMenuItem21.Size = new System.Drawing.Size(111, 22);
          this.binsToolStripMenuItem21.Text = "21 bins";
          this.binsToolStripMenuItem21.ToolTipText = "Breaks down the calls based on how many times the target function has been called" +
              ". eg. [0 to 5 calls], [6 to 10 calls], [11 to 15 calls], etc.";
          this.binsToolStripMenuItem21.Click += new System.EventHandler(this.binsToolStripMenuItem21_Click);
          // 
          // binsToolStripMenuItem31
          // 
          this.binsToolStripMenuItem31.Name = "binsToolStripMenuItem31";
          this.binsToolStripMenuItem31.Size = new System.Drawing.Size(111, 22);
          this.binsToolStripMenuItem31.Text = "31 bins";
          this.binsToolStripMenuItem31.ToolTipText = "Breaks down the calls based on how many times the target function has been called" +
              ". eg. [0 to 5 calls], [6 to 10 calls], [11 to 15 calls], etc.";
          this.binsToolStripMenuItem31.Click += new System.EventHandler(this.binsToolStripMenuItem31_Click);
          // 
          // binsToolStripMenuItem41
          // 
          this.binsToolStripMenuItem41.Name = "binsToolStripMenuItem41";
          this.binsToolStripMenuItem41.Size = new System.Drawing.Size(111, 22);
          this.binsToolStripMenuItem41.Text = "41 bins";
          this.binsToolStripMenuItem41.ToolTipText = "Breaks down the calls based on how many times the target function has been called" +
              ". eg. [0 to 5 calls], [6 to 10 calls], [11 to 15 calls], etc.";
          this.binsToolStripMenuItem41.Click += new System.EventHandler(this.binsToolStripMenuItem41_Click);
          // 
          // binsToolStripMenuItem51
          // 
          this.binsToolStripMenuItem51.Name = "binsToolStripMenuItem51";
          this.binsToolStripMenuItem51.Size = new System.Drawing.Size(111, 22);
          this.binsToolStripMenuItem51.Text = "51 bins";
          this.binsToolStripMenuItem51.ToolTipText = "Breaks down the calls based on how many times the target function has been called" +
              ". eg. [0 to 5 calls], [6 to 10 calls], [11 to 15 calls], etc.";
          this.binsToolStripMenuItem51.Click += new System.EventHandler(this.binsToolStripMenuItem51_Click);
          // 
          // addPToolStripMenuItem
          // 
          this.addPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moduleToolStripMenuItem1,
            this.menuAddPlotIntermodular,
            this.menuAddPlotIntramodular,
            this.menuAddPlotHasString,
            this.menuAddPlotFromFilter});
          this.addPToolStripMenuItem.Name = "addPToolStripMenuItem";
          this.addPToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
          this.addPToolStripMenuItem.Text = "Add Plot";
          // 
          // moduleToolStripMenuItem1
          // 
          this.moduleToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemToolStripMenuItem});
          this.moduleToolStripMenuItem1.Name = "moduleToolStripMenuItem1";
          this.moduleToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
          this.moduleToolStripMenuItem1.Text = "Destination Module";
          this.moduleToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.moduleToolStripMenuItem1_DropDownOpening);
          // 
          // itemToolStripMenuItem
          // 
          this.itemToolStripMenuItem.Name = "itemToolStripMenuItem";
          this.itemToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
          this.itemToolStripMenuItem.Text = "item";
          // 
          // menuAddPlotIntermodular
          // 
          this.menuAddPlotIntermodular.Name = "menuAddPlotIntermodular";
          this.menuAddPlotIntermodular.Size = new System.Drawing.Size(185, 22);
          this.menuAddPlotIntermodular.Tag = "";
          this.menuAddPlotIntermodular.Text = "Intermodular Calls";
          this.menuAddPlotIntermodular.ToolTipText = "Adds a plot for intermodular calls. These are calls between loaded libraries.";
          this.menuAddPlotIntermodular.Click += new System.EventHandler(this.menuAddPlotIntermodular_Click);
          // 
          // menuAddPlotIntramodular
          // 
          this.menuAddPlotIntramodular.Name = "menuAddPlotIntramodular";
          this.menuAddPlotIntramodular.Size = new System.Drawing.Size(185, 22);
          this.menuAddPlotIntramodular.Tag = "";
          this.menuAddPlotIntramodular.Text = "Intramodular Calls";
          this.menuAddPlotIntramodular.ToolTipText = "Adds a plot for intramodular calls. These are calls from a module to itself.";
          this.menuAddPlotIntramodular.Click += new System.EventHandler(this.menuAddPlotIntramodular_Click);
          // 
          // menuAddPlotHasString
          // 
          this.menuAddPlotHasString.Name = "menuAddPlotHasString";
          this.menuAddPlotHasString.Size = new System.Drawing.Size(185, 22);
          this.menuAddPlotHasString.Tag = "";
          this.menuAddPlotHasString.Text = "Has String Argument";
          this.menuAddPlotHasString.ToolTipText = "Adds a plot for calls that included a string argument.";
          this.menuAddPlotHasString.Click += new System.EventHandler(this.menuAddPlotHasString_Click);
          // 
          // menuAddPlotFromFilter
          // 
          this.menuAddPlotFromFilter.Name = "menuAddPlotFromFilter";
          this.menuAddPlotFromFilter.Size = new System.Drawing.Size(185, 22);
          this.menuAddPlotFromFilter.Tag = "";
          this.menuAddPlotFromFilter.Text = "Custom Filter";
          this.menuAddPlotFromFilter.ToolTipText = "Allows you to create a custom plot by using the Filter menu to select the options" +
              ".";
          this.menuAddPlotFromFilter.Click += new System.EventHandler(this.menuAddPlotFromFilter_Click);
          // 
          // menuRemoveAllPlots
          // 
          this.menuRemoveAllPlots.Name = "menuRemoveAllPlots";
          this.menuRemoveAllPlots.Size = new System.Drawing.Size(163, 22);
          this.menuRemoveAllPlots.Text = "Remove All Plots";
          this.menuRemoveAllPlots.Click += new System.EventHandler(this.menuRemoveAllPlots_Click);
          // 
          // toolStripSeparator15
          // 
          this.toolStripSeparator15.Name = "toolStripSeparator15";
          this.toolStripSeparator15.Size = new System.Drawing.Size(160, 6);
          // 
          // enableStartRecording
          // 
          this.enableStartRecording.Interval = 2000;
          this.enableStartRecording.Tick += new System.EventHandler(this.enableStartRecording_Tick);
          // 
          // dataGridViewTextBoxColumn1
          // 
          this.dataGridViewTextBoxColumn1.HeaderText = "Call Number";
          this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
          this.dataGridViewTextBoxColumn1.Width = 50;
          // 
          // dataGridViewTextBoxColumn2
          // 
          this.dataGridViewTextBoxColumn2.HeaderText = "Source";
          this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
          this.dataGridViewTextBoxColumn2.Width = 300;
          // 
          // dataGridViewTextBoxColumn3
          // 
          this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
          this.dataGridViewTextBoxColumn3.HeaderText = "Destination";
          this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
          // 
          // controlVisPlayBar
          // 
          this.controlVisPlayBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.controlVisPlayBar.Location = new System.Drawing.Point(2, 800);
          this.controlVisPlayBar.Name = "controlVisPlayBar";
          this.controlVisPlayBar.Size = new System.Drawing.Size(1055, 50);
          this.controlVisPlayBar.TabIndex = 2;
          this.controlVisPlayBar.UpdateRate = 10;
          // 
          // FunctionListViewer
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.Controls.Add(this.splitContainerFunctionsPlaybar);
          this.Controls.Add(this.toolStrip1);
          this.Name = "FunctionListViewer";
          this.Size = new System.Drawing.Size(1062, 686);
          this.toolStrip1.ResumeLayout(false);
          this.toolStrip1.PerformLayout();
          this.contextMenuFunctions.ResumeLayout(false);
          this.contextMenuStripCalls.ResumeLayout(false);
          this.splitContainerFunctionsPlaybar.Panel1.ResumeLayout(false);
          this.splitContainerFunctionsPlaybar.Panel2.ResumeLayout(false);
          this.splitContainerFunctionsPlaybar.ResumeLayout(false);
          this.splitContainer1.Panel1.ResumeLayout(false);
          this.splitContainer1.Panel2.ResumeLayout(false);
          this.splitContainer1.ResumeLayout(false);
          this.tabControlFunctionsCalls.ResumeLayout(false);
          this.tabPageFunctionList.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.dataGridFunctions)).EndInit();
          this.tabPageCallList.ResumeLayout(false);
          this.splitContainer2.Panel1.ResumeLayout(false);
          this.splitContainer2.Panel2.ResumeLayout(false);
          this.splitContainer2.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.dataGridCalls)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.dataGridCallArguments)).EndInit();
          this.contextMenuPlaybar.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolButtonRecord;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolButtonFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolButtonFilterComparison;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolButtonRename;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolButtonSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuFunctions;
        private System.Windows.Forms.ToolStripMenuItem disableSelectedFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableSelectedFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedFunctionsToolStripMenuItem;
        private FunctionHacker.Classes.Visualization.oVisPlayBar controlVisPlayBar;
        private ToolStripMenuItem sendCallsToSelectedFunctionToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ContextMenuStrip contextMenuStripCalls;
        private ToolStripMenuItem toolStripCallDisable;
        private ToolStripMenuItem toolStripCallEnable;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem toolStripCallSend;
        private ToolStripMenuItem toolStripCallResend;
        private ToolStripMenuItem toolStripCallGoto;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem renameSelectedFunctionToolStripMenuItem;
        private ToolStripMenuItem renameSelectedFunctionsOrdinalToolStripMenuItem;
        private ToolStripMenuItem loadNamesFromMapFileToolStripMenuItem;
        private ToolStripTextBox toolStripTextBoxFastFilter;
        private ToolStripLabel toolStripLabelFastFilter;
        private ToolStripLabel toolStripLabelNumberOfRecords;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripButton toolStripButtonTakeFastFilter;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private ToolStripMenuItem addressToolStripMenuItem;
        private ToolStripMenuItem nameToolStripMenu;
        private ToolStripMenuItem addressesNamesToolStripMenuItem;
        private SplitContainer splitContainerFunctionsPlaybar;
        private SplitContainer splitContainer1;
        private TabControl tabControlFunctionsCalls;
        private TabPage tabPageFunctionList;
        private DataGridViewEx dataGridFunctions;
        private TabPage tabPageCallList;
        private DataGridView dataGridCalls;
        private FunctionHacker.Classes.Visualization.oVisMain controlVisMain;
        private Button buttonBreakdown;
        private ContextMenuStrip contextMenuPlaybar;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem menuBreakdownModule;
        private ToolStripMenuItem menuBreakdownThread;
        private ToolStripMenuItem menuBreakdownCallType;
        private ToolStripMenuItem menuBreakdownCallFrequency;
        private ToolStripMenuItem addPToolStripMenuItem;
        private ToolStripMenuItem moduleToolStripMenuItem1;
        private ToolStripMenuItem menuAddPlotIntermodular;
        private ToolStripMenuItem menuAddPlotIntramodular;
        private ToolStripMenuItem menuAddPlotHasString;
        private ToolStripMenuItem menuAddPlotFromFilter;
        private ToolStripMenuItem menuRemoveAllPlots;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripMenuItem binsToolStripMenuItem6;
        private ToolStripMenuItem binsToolStripMenuItem11;
        private ToolStripMenuItem binsToolStripMenuItem21;
        private ToolStripMenuItem binsToolStripMenuItem31;
        private ToolStripMenuItem binsToolStripMenuItem41;
        private ToolStripMenuItem binsToolStripMenuItem51;
        private ToolStripSeparator toolStripSeparator16;
        private Timer enableStartRecording;
        private ToolStripMenuItem itemToolStripMenuItem;
        private DataGridViewTextBoxColumn description;
        private DataGridViewTextBoxColumn address;
        private DataGridViewTextBoxColumn callCount;
        private DataGridViewTextBoxColumn arguments;
        private SplitContainer splitContainer2;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem toolStripExportFunction;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn colCallNumber;
        private DataGridViewTextBoxColumn colSource;
        private DataGridViewTextBoxColumn colDestination;
        private DataGridViewTextBoxColumn colArguments;
        private DataGridViewCall dataGridCallArguments;
        private DataGridViewTextBoxColumn colArgumentName;
        private DataGridViewComboBoxColumn colArgumentType;
        private DataGridViewTextBoxColumn colArgumentValue;
        private ToolStripMenuItem importNamesFromIDASignaturesToolStripMenuItem;
    }
}
