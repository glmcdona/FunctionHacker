using System.Windows.Forms;
using FunctionHacker.Classes.Visualization;
using FunctionHacker.Controls;

namespace FunctionHacker.Classes.Tabs
{
    
    public class oTabFunctionList : oTab
    {
        private FunctionListViewer functionListControl;

        public oTabFunctionList(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip, string tabTitle, oFunctionList functionList, oVisMain visMain, oVisPlayBar visPlayBar)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            

            // Initialize the controls we need
            InitializeComponents();

            // Set the data source for the control
            functionListControl.FunctionList = functionList;
            
            // Set the function list parent tab
            if (functionList != null)
                functionList.parentControl = functionListControl;
        }


        private void InitializeComponents()
        {
            functionListControl = new FunctionListViewer(this);
            workingPage.Controls.Add(functionListControl);
            functionListControl.Location = new System.Drawing.Point(0, 0);
            functionListControl.Dock = DockStyle.Fill;
        }

        public override bool isProcessSpecific()
        {
            return true;
        }


        public override void deactivate()
        {
            base.deactivate();

            // Deactivate the main control
            functionListControl.deactivate();
        }

        public override void activate()
        {
            if( activated )
                return;
            // Perform common activate function
            base.activate();

            // Activate the main control
            functionListControl.Activate();
        }

        /// <summary>
        /// initialise function list
        /// </summary>
        /// <param name="functionList">The function list.</param>
        public void SetFunctionList(oFunctionList functionList)
        {
            functionListControl.FunctionList = functionList;
        }

        /// <summary>
        /// Call function from outside
        /// </summary>
        public void InvokeStartStopRecording()
        {
            functionListControl.StartStopRecording();
        }
    }

}
