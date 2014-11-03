using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BufferOverflowProtection;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Font = System.Drawing.Font;

namespace FunctionHacker.Classes.Visualization
{
    

    public struct DISPLAY_RANGE
    {
        public int xMin;
        public int xMax;
        public double tMin;
        public double tMax;
        public double tMinDataset;
        public double tMaxDataset;
    }

    public class oVisPlayBar : Panel
    {
        Device device = null;
        private int updateRate;
        private oFunctionList baseFunctionList;
        private List<oTimeseriesPlot> timeseriesPlots;
        private Thread drawingThread = null;
        private bool initialized = false;
        private bool redraw = false;
        private bool mouseLeft = false;
        private bool mouseRight = false;
        private oVisMain mainVisualization = null;
        Microsoft.DirectX.Direct3D.Font drawingFont;
        private int mouseX = 0;
        private int mouseY = 0;

        // Zoom variables
        private double zoom = 1.0;
        private double zoomCentre = 0.5;
        private double lastZoom = 1.0;
        

        // Cursor data
        public double cursorTime = -1000; // The currently selected time
        public double cursorWidth = 0.02; // 20ms default width
        public int cursorRow = 0;
        public double selectStart = -10; // The time of the selection start
        public double selectEnd = -10; // The time of the selection end
        public int selectedRow = 0; // The main function list

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (drawingFont != null)
                {
                    drawingFont.Dispose();
                    drawingFont = null;
                }
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }
                base.Dispose(true);
            }
        }

        // Update speed can be changed in runtime
        public int UpdateRate
        {
            get { return updateRate; }
            set { updateRate = value; }
        }

        public oVisPlayBar()
        {
            initialize(10);
        }

        public oVisPlayBar(int updateRate)
        {
            initialize(updateRate);
        }

        /// <summary>
        /// Add a new time-series from the specified function list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        public void addTimeSeriesFromFunctionList(oFunctionList list, string name)
        {
            // Create a new time series
            timeseriesPlots.Add(new oTimeseriesPlot(list, name, timeseriesPlots.Count, timeseriesPlots.Count + 1, drawingFont, mainVisualization, device));

            // Invalidate all the plots
            foreach (oTimeseriesPlot plot in timeseriesPlots)
                plot.setNumRows(timeseriesPlots.Count);
        }


        /// <summary>
        /// This removes all the extra timeseries from the dataset
        /// </summary>
        public void removeAllExtraTimeSeries()
        {
            if( timeseriesPlots.Count > 1 )
            {
                // Remove all the extra timeseries
                timeseriesPlots.RemoveRange(1,timeseriesPlots.Count-1);
                setSelectionRow(0);
                setCursorRow(0);

                // Invalidate all the plots
                foreach (oTimeseriesPlot plot in timeseriesPlots)
                    plot.setNumRows(timeseriesPlots.Count);
            }
        }

        /// <summary>
        /// Removes the specified timeseries.
        /// Cannot remove a time series with index of 0.
        /// </summary>
        /// <param name="index"></param>
        public void removeTimeSeries(int index)
        {
            if (timeseriesPlots.Count > 1)
            {
                // Change the selected row if required
                if (index == selectedRow)
                    setSelectionRow(0);
                if (index == cursorRow)
                    setCursorRow(0);

                // Remove the specified timeseries
                timeseriesPlots.Remove(timeseriesPlots[index]);

                // Invalidate all the plots, and update their row numbers
                for (int i = 0; i < timeseriesPlots.Count; i++)
                {
                    timeseriesPlots[i].setRow(i);
                    timeseriesPlots[i].setNumRows(timeseriesPlots.Count);
                }
            }
        }

        private void initialize(int rate)
        {
            this.updateRate = rate;

            // Create the event handling hooks
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.eventMouseMove);
            this.MouseLeave += new System.EventHandler(this.eventMouseLeave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.eventMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.eventMouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.eventMouseWheel);
        }

        public void setMainVisualization(oVisMain panel)
        {
            this.mainVisualization = panel;
        }

        private bool initializeGraphics()
        {
            try
            {
                // Initialize the DirectX drawing
                initialized = true;
                PresentParameters presentParams = new PresentParameters();
                presentParams.Windowed = true;
                presentParams.SwapEffect = SwapEffect.Discard;
                presentParams.DeviceWindowHandle = this.Handle;

                device = new Device(0,DeviceType.Hardware,
                                    this,
                                    CreateFlags.SoftwareVertexProcessing,
                                    presentParams);

                // Set initial device parameters
                device.RenderState.CullMode = Cull.None;

                // Initialize the font
                Font systemFont = new System.Drawing.Font("Arial", 8f, FontStyle.Regular);
                drawingFont = new Microsoft.DirectX.Direct3D.Font(device, systemFont);

                // Set the timeseries device details
                foreach (oTimeseriesPlot plot in timeseriesPlots)
                {
                    plot.setDevice(device, drawingFont);
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing. We needed to override this function so that the panel does not try to draw a background.
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // Update the graphics device
            initialized = false;
        }

        /// <summary>
        /// Sets the currently active function list.
        /// </summary>
        /// <param name="data">The function list currently viewable.</param>
        public void setData(oFunctionList newList)
        {
            // Update the data
            baseFunctionList = newList;
            
            // Update all the subplots with the data
            generateSubplotFunctionLists();

            // Set the cursor to the end of the data
            DISPLAY_RANGE range = getDisplayRange();
            double time = range.tMaxDataset;
            setCursor(time, cursorWidth, 0);

            // Render any changes immediately
            Render();
        }

        /// <summary>
        /// This takes the baseFunctionList and creates all the functionLists used by
        /// each subplot.
        /// </summary>
        public void generateSubplotFunctionLists()
        {
            if (baseFunctionList != null)
            {
                timeseriesPlots = new List<oTimeseriesPlot>(2);
                timeseriesPlots.Add(new oTimeseriesPlot(baseFunctionList, "all", 0, 1, drawingFont,
                                                         mainVisualization, device));
                setCursorRow(0);
                setSelectionRow(0);
            }else
            {
                timeseriesPlots = new List<oTimeseriesPlot>(2);
            }

        }


        /// <summary>
        /// Determines whether the specified row is in the selected region.
        /// </summary>
        /// <param name="row">Row in the call list table.</param>
        /// <returns></returns>
        public bool isCallSelected(int row)
        {
            if (baseFunctionList == null || baseFunctionList.dataVis == null)
                return false;

            if( selectedRow >=0 && selectedRow < timeseriesPlots.Count )
            {
                // Get the call in question
                oSingleData rowData = baseFunctionList.dataVis.getData(row);

                if (timeseriesPlots[selectedRow].isCallSelected(rowData, selectStart, selectEnd))
                    return true;
            }

            // No timeseries claimed this call was selected.
            return false;
        }


        /// <summary>
        /// Counts the number of times the specified function has been called in the selection.
        /// </summary>
        /// <param name="row">Row in the call list table.</param>
        /// <returns></returns>
        public int countCallSelected(uint address)
        {
            if (baseFunctionList == null)
                return 0;

            if (timeseriesPlots!= null && timeseriesPlots.Count > 0)
            {
                // Fix the selected row if required
                if (selectedRow < 0 || selectedRow >= timeseriesPlots.Count)
                    selectedRow = 0;

                // Ask the selected plot how many calls are selected
                return timeseriesPlots[selectedRow].countCallSelected(address, selectStart, selectEnd);
            }
            return 0;
        }
        

        /// <summary>
        /// Updates the cursor position and width. This also tells the main visualization to update.
        /// </summary>
        /// <param name="time">Time of cursor in seconds.</param>
        /// <param name="width">Width of cursor in seconds.</param>
        private void setCursor(double time, double width)
        {
            if( baseFunctionList == null )
                return;

            this.cursorWidth = width;
            this.cursorTime = time;
            foreach (oTimeseriesPlot plot in timeseriesPlots)
            {
                plot.invalidateCursor();
            }

            // Redraw
            redraw = true;
        }

        /// <summary>
        /// Updates the cursor position and width. This also tells the main visualization to update.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="width"></param>
        /// <param name="row"></param>
        private void setCursor(double time, double width, int row)
        {
            // Set the cursor row if required
            if( row != cursorRow )
                setCursorRow(row);

            // Set the cursor
            setCursor(time, width);
        }

        /// <summary>
        /// Sets the width of the visualization display.
        /// </summary>
        /// <param name="cursorWidth">Time in seconds.</param>
        public void setCursorWidth(double cursorWidth)
        {
            this.setCursor(cursorTime, cursorWidth);
        }

        /// <summary>
        /// Sets the selected region
        /// </summary>
        /// <param name="selectEnd"></param>
        public void setSelect(double selectStart, double selectEnd)
        {
            if (baseFunctionList == null)
                return;

            // Update the selected vertices
            this.selectEnd = selectEnd;
            this.selectStart = selectStart;

            // Invalidate the selection in the timeseries
            foreach (oTimeseriesPlot plot in timeseriesPlots)
            {
                plot.invalidateSelection();
            }

            // Invalidate the selected call count
            baseFunctionList.invalidateSelectedCount();

            // Redraw
            redraw = true;
            this.Render();
        }

        /// <summary>
        /// Sets the selected region and row.
        /// </summary>
        /// <param name="selectStart"></param>
        /// <param name="selectEnd"></param>
        /// <param name="row"></param>
        public void setSelect(double selectStart, double selectEnd, int row)
        {
            // Set the selection row if required
            if( row != selectedRow )
                setSelectionRow(row);

            // Set the selection
            setSelect(selectStart, selectEnd);
        }

        /// <summary>
        /// Sets the selected rebion
        /// </summary>
        /// <param name="selectEnd"></param>
        public void setSelectEnd(double selectEnd)
        {
            setSelect(selectStart, selectEnd);
        }

        /// <summary>
        /// Sets the selected rebion
        /// </summary>
        /// <param name="selectEnd"></param>
        public void setSelectStart(double selectStart)
        {
            setSelect(selectStart, selectEnd);
        }

        /// <summary>
        /// Handles mouse move events on the play bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventMouseMove(object sender, MouseEventArgs e)
        {
            // Get focus to get all on mouse events
            if (!Focused && CanFocus)
                Focus();

            // Set our mouse x and y
            mouseX = e.X;
            mouseY = e.Y;

            // Update the mouse highlight
            foreach (oTimeseriesPlot plot in timeseriesPlots)
            {
                plot.invalidateMouse();
            }

            // Set the cursor to this location
            DISPLAY_RANGE range = getDisplayRange();
            double time = (((double)e.X) / ((double)range.xMax - range.xMin)) * (range.tMax - range.tMin) + range.tMin;
            setCursor(time, cursorWidth, getRowFromY(e.Y));

            // Handle mouse buttons
            if( mouseRight )
            {
                // Set the right side of the select to here
                if (time > selectStart)
                {
                    setSelectEnd(time);
                }else
                {
                    setSelectEnd(selectStart);
                }
            }
            /*
            else if ( (Control.ModifierKeys & Keys.Shift) == Keys.Shift )
            {
                // We are changing the size of the tail
                double newWidth = cursorTime - time;
                if( newWidth >= 0 )
                {
                    this.setCursorWidth(newWidth);
                }else
                {
                    // Change the position of the cursor
                    this.setCursor(time, cursorWidth);
                }
            }
             */
            
            // Redraw, this will be a double redraw in some cases :(
            redraw = true;
            Render();
        }

        private void eventMouseDown(object sender, MouseEventArgs e)
        {
            if (baseFunctionList == null)
                return;

            DISPLAY_RANGE range = getDisplayRange();
            double time = (((double) e.X)/((double) range.xMax - range.xMin))*(range.tMax - range.tMin) + range.tMin;

            // Update the mouse states
            if( e.Button == MouseButtons.Left )
            {
                mouseRight = false;
                mouseLeft = true;

                // Set the cursor to this location
                setCursor(time, cursorWidth, getRowFromY(e.Y));
            }else if( e.Button == MouseButtons.Right )
            {
                mouseRight = true;
                mouseLeft = false;

                // Set the region start and end to this location
                setSelect(time, time, getRowFromY(e.Y));
            }
        }

        private void eventMouseUp(object sender, MouseEventArgs e)
        {
            // Update the mouse button states
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                mouseRight = false;
                mouseLeft = false;
            }
        }

        /// <summary>
        /// Gets the timeseries row associated with the specified y offset value.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private int getRowFromY(int y)
        {
            float spacing = (this.Height / (float)timeseriesPlots.Count);
            return (int) (y/spacing);
        }

        /// <summary>
        /// Because the mouse wheel event is sent to the main form, this event is forwarded from formMain.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void eventMouseWheel(object sender, MouseEventArgs e)
        {
            // Update the mouse button states
            if (baseFunctionList != null && e.Delta != 0)
            {
                // Update the zoom
                zoom = zoom * Math.Pow(0.9, ((double)e.Delta) / 120.0);
                if (zoom > 1.0)
                    zoom = 1.0;

                // Update the zoom centre such that the t corresponding to the mouse location stays at the same x
                DISPLAY_RANGE range = getDisplayRange();
                double timeMouse = (((double)e.X) / ((double)range.xMax - range.xMin)) * (range.tMax - range.tMin) +
                              range.tMin;
                double leftTimeNew = timeMouse - (timeMouse - range.tMin) * Math.Pow(0.9, ((double)e.Delta) / 120.0);
                double rightTimeNew = (range.tMax - timeMouse) * Math.Pow(0.9, ((double)e.Delta) / 120.0) + timeMouse;

                zoomCentre = (rightTimeNew + leftTimeNew) / 2;

                if (zoom != lastZoom)
                {
                    // Tell all the timeseries to update their log-plot vertices
                    foreach (oTimeseriesPlot plot in timeseriesPlots)
                    {
                        plot.invalidateLogPlot();
                        plot.invalidateAxis();
                        plot.invalidateCursor();
                        plot.invalidateSelection();
                    }

                    // Redraw
                    redraw = true;
                    Render();
                }
                    
                lastZoom = zoom;
            }
        }

        private void eventMouseLeave(object sender, EventArgs e)
        {
            // Destroy the mouse vertex buffer
            mouseX = -1000; // Off the display
            foreach (oTimeseriesPlot plot in timeseriesPlots)
                plot.invalidateMouse();

            // Redraw
            redraw = true;
            Render();

            // Update the mouse button states
            mouseLeft = false;
            mouseRight = false;
        }


        /// <summary>
        /// Redraws the play bar.
        /// </summary>
        private void DrawLoop()
        {
            while (!this.IsDisposed && Application.OpenForms.Count > 0) //aditionally check that FH don't stay in this loop forever
            {
                // Wait the specified time
                Thread.Sleep(updateRate);
                redraw = true;

                // Send a draw message to the panel
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Redraw only if the drawing thread told us to draw. This way
            // it will not try to draw a couple hundred times while trying
            // to resize.
            if (redraw || drawingThread == null)
            {
                if ((device == null || !initialized))
                {
                    if (device != null)
                        device.Dispose();

                    // Initialize the graphics
                    if (initializeGraphics() && drawingThread == null)
                    {
                        // Create the drawing thread
                        drawingThread = new Thread(new ThreadStart(DrawLoop));
                        drawingThread.Start();
                    }

                    if (device == null)
                        return;
                }

                // Reload the data
                redraw = false;
                Render();
            }
        }

        private void Render()
        {
            // Render the playbar
            
            if (device != null)
            {
                lock (this)
                {
                    try
                    {
                        device.Clear(ClearFlags.Target, System.Drawing.Color.Black, 1.0f, 0);
                        device.BeginScene();

                        // Draw the main play bar sections
                        for (int i = 0; i < timeseriesPlots.Count; i++)
                        {
                            timeseriesPlots[i].render(cursorTime, cursorWidth, selectStart, selectEnd, getDisplayRange(),
                                                      mouseX, mouseY, this.Height, i == selectedRow, i == cursorRow,
                                                      i == 0);
                        }

                        device.EndScene();
                        device.Present();

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

        /// <summary>
        /// Set the specified row which contains the cursor.
        /// </summary>
        /// <param name="row"></param>
        public void setCursorRow(int row)
        {
            // Set the row for the cursor
            cursorRow = row;

            // Invalidate the cursors in all the plots
            foreach( oTimeseriesPlot plot in timeseriesPlots )
                plot.invalidateCursor();
        }

        /// <summary>
        /// Set the specified row which contains the selected area.
        /// </summary>
        /// <param name="row"></param>
        public void setSelectionRow(int row)
        {
            // Set the row for the cursor
            selectedRow = row;

            // Invalidate the selections in all the plots
            foreach (oTimeseriesPlot plot in timeseriesPlots)
                plot.invalidateSelection();
        }

        private DISPLAY_RANGE getDisplayRange()
        {
            // This function gets the display range of the play bar and takes into account zoom.
            DISPLAY_RANGE result = new DISPLAY_RANGE();
            if (baseFunctionList == null)
                return result;

            result.xMin = 0;
            result.xMax = this.Width - 1;

            if (zoom == 1.0 || baseFunctionList.dataVis == null)
            {
                // No zoom, fill the whole screen
                if (baseFunctionList.dataVis != null)
                {
                    result.tMin = baseFunctionList.dataVis.timeStart;
                    result.tMax = baseFunctionList.dataVis.timeEnd;
                }else if( oFunctionMaster.reader_visualization != null && oFunctionMaster.reader_visualization.data != null )
                {
                    result.tMin = 0;
                    result.tMax = oFunctionMaster.reader_visualization.data.timeEnd;
                }else
                {
                    result.tMin = 0;
                    result.tMax = 10;
                }
                result.tMinDataset = result.tMin;
                result.tMaxDataset = result.tMax;
            }
            else
            {
                // Zoomed in, lets calculate the variables
                result.tMinDataset = baseFunctionList.dataVis.timeStart;
                result.tMaxDataset = baseFunctionList.dataVis.timeEnd;
                result.tMin = (zoomCentre - ((result.tMaxDataset - result.tMinDataset) / 2) * zoom);
                result.tMax = (zoomCentre + ( (result.tMaxDataset - result.tMinDataset) / 2) * zoom);

                // Check if these exceed the range on either side
                if (result.tMin < result.tMinDataset)
                {
                    // Shift the range to the right
                    result.tMax -= result.tMin - result.tMinDataset;
                    result.tMin = result.tMinDataset;
                }
                else if (result.tMax > result.tMaxDataset)
                {
                    // Shift the range to the left
                    result.tMin -= result.tMax - result.tMaxDataset;
                    result.tMax = result.tMaxDataset;
                }

            }

            return result;
        }

        /// <summary>
        /// Get the function list that is currently selected.
        /// </summary>
        /// <returns>The selected function list.</returns>
        public oFunctionList getSelectedFunctionList()
        {
            if( selectedRow < timeseriesPlots.Count() && selectedRow >= 0 )
            {
                // The selected row is valid
                return timeseriesPlots[selectedRow].getFunctionList();
            }else if(timeseriesPlots.Count() > 0)
            {
                // Return the first list in the dataset
                return timeseriesPlots[0].getFunctionList();
            }else
            {
                // Return the base function list.
                return baseFunctionList;
            }
        }

        public int getNumPlots()
        {
            if (timeseriesPlots == null)
                return 0;
            return timeseriesPlots.Count;
        }

        public void resetZoom()
        {
            if( baseFunctionList != null )
            {
                // Update the zoom
                this.zoom = 1.0;

                // Update the graphics
                if (zoom != lastZoom)
                {
                    // Tell all the timeseries to update their log-plot vertices
                    foreach (oTimeseriesPlot plot in timeseriesPlots)
                    {
                        plot.invalidateLogPlot();
                        plot.invalidateAxis();
                        plot.invalidateCursor();
                        plot.invalidateSelection();
                    }

                    // Redraw
                    redraw = true;
                    Render();
                }

                lastZoom = zoom;
            }
        }
    }
}
