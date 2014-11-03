using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BufferOverflowProtection;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Font = System.Drawing.Font;

namespace FunctionHacker.Classes.Visualization
{
    //public class oVisPlayBar
    public partial class oVisMain : System.Windows.Forms.Panel
    {
        Device device = null;
        private oVisPlayBar playBar; // This is used to determine which part of the data recording is being displayed.
        private oFunctionList functionList;
        private bool initialized = false;
        private oVisLookup visLookup = null;
        private Microsoft.DirectX.Direct3D.Font directxFont;
        private DateTime lastInitializeTime;

        // Data
        private bool invalidData = true;
        private List<oSingleData> curData = null;

        public oVisMain()
        {
            lastInitializeTime = DateTime.Now;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (directxFont != null)
                {
                    directxFont.Dispose();
                    directxFont = null;
                }
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }
                base.Dispose(true);
            }

        }

        public void setPlayBar(oVisPlayBar playBar)
        {
            this.playBar = playBar;
        }


        public void initializeFunctionList(List<HEAP_INFO> map, List<oFunction> functions)
        {
            if( visLookup == null )
            {
                visLookup = new oVisLookup();
            }
            visLookup.initialize(map, functions, this);
        }

        public void resetLookup()
        {
            if( visLookup != null )
                visLookup.reset();
        }

        private bool initializeGraphics()
        {
            try
            {
                lock(this)
                {
                    // Initialize the DirectX drawing
                    initialized = true;
                    lastInitializeTime = DateTime.Now;
                    PresentParameters presentParams = new PresentParameters();
                    presentParams.Windowed = true;
                    presentParams.SwapEffect = SwapEffect.Discard;
                    presentParams.DeviceWindowHandle = this.Handle;

                    device = new Device(0,
                                        DeviceType.Hardware,
                                        this,
                                        CreateFlags.SoftwareVertexProcessing,
                                        presentParams);

                    // Set initial device parameters
                    device.RenderState.FillMode = FillMode.WireFrame;
                    device.RenderState.CullMode = Cull.None;

                    // Initialize the font
                    Font systemFont = new System.Drawing.Font("Arial", 8f, FontStyle.Regular);
                    directxFont = new Microsoft.DirectX.Direct3D.Font(device, systemFont);
                }

                return true;
            }
            catch (Exception ex)
            {
                initialized = false;
                //oConsole.printException(ex);
                return false;
            }
        }

        public void update(List<oSingleData> data)
        {
            this.invalidData = true;
            curData = data;

            // Trigger a redraw
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            paint();
        }

        private void paint()
        {
            // Redraw only if the drawing thread told us to draw. This way
            // it will not try to draw a couple hundred times while trying
            // to resize.
            if (device == null || !initialized )
            {
                // Initialize the graphics
                initializeGraphics();


                if (device == null)
                    return;

                if( curData != null && visLookup != null )
                    visLookup.setData(curData, ref device);
            }

            // Redraw the scene
            Render();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // Update the graphics device
            initialized = false;

            // Redraw
            //this.Invalidate();
        }

        /// <summary>
        /// Sets the currently active function list.
        /// </summary>
        /// <param name="data">The function list currently viewable.</param>
        public void setData(oFunctionList data)
        {
            // Update the data
            functionList = data;
        }

        private void Render()
        {
            try
            {
                if (device != null)
                {
                    lock (this)
                    {
                        // Update the vertices
                        visLookup.setData(curData, ref device);

                        // Draw
                        device.Clear(ClearFlags.Target, System.Drawing.Color.Black, 1.0f, 0);
                        device.BeginScene();

                        // Draw the play bar
                        if (visLookup != null)
                        {
                            // Render the module names

                            // Render the function calls

                            // Render the functions
                            visLookup.render(ref device, ref directxFont);
                        }

                        device.EndScene();
                        device.Present();
                    }
                }
            }
            catch (DeviceLostException ex_lost)
            {
                // We need to recreate the device
                initialized = false;
                device = null;
            }
            catch (Exception e)
            {
                // Reset as well
                initialized = false;
                device = null;
            }
        }
    }
}
