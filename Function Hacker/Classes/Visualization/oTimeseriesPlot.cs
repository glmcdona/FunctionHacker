using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Font = Microsoft.DirectX.Direct3D.Font;

namespace FunctionHacker.Classes.Visualization
{
    public class oTimeseriesPlot
    {
        // Set the default plot width.
        static double timeWidth = 0.001; // 1 ms width on the plot points


        private string name = "unnamed";
        private int row;
        private int numRows;
        private oFunctionList functionList;
        private Device device;
        private int lastCallCount;
        private double lastTime;

        // Validation keys
        private bool validCursor = false;
        private bool validSelection = false;
        private bool validAxis = false;
        private bool validLogPlot = false;
        private bool validMouse = false;
        

        // Mouse move vertices
        private CustomVertex.TransformedColored[] verticesMouse = null;
        private VertexBuffer vertexBufferMouse = null;
        private int mouseNumCalls = 0;
        private Point mouseTextPos;

        // Font to use for drawing
        private Microsoft.DirectX.Direct3D.Font directxFont;

        // Cursor vertices
        private CustomVertex.TransformedColored[] verticesCursor = null;
        private VertexBuffer vertexBufferCursor = null;

        // Selection vertices
        private CustomVertex.TransformedColored[] verticesSelectionBackground = null;
        private VertexBuffer vertexBufferSelectionBackground = null;

        // Axis grid vertices
        private CustomVertex.TransformedColored[] verticesAxis = null;
        private VertexBuffer vertexBufferAxis = null;

        // Log plot vertices
        private CustomVertex.TransformedColored[] verticesLogPlot = null;
        private VertexBuffer vertexBufferLogPlot = null;

        // Linked main visualization
        private oVisMain mainVisualization = null;

        /// <summary>
        /// Creates a timeseries plot from a function list.
        /// </summary>
        /// <param name="functionList"></param>
        /// <param name="name"></param>
        /// <param name="row"></param>
        /// <param name="numRows"></param>
        /// <param name="mainVisualization"></param>
        public oTimeseriesPlot(oFunctionList functionList, string name, int row, int numRows, Microsoft.DirectX.Direct3D.Font directxFont, oVisMain mainVisualization, Device device )
        {
            this.functionList = functionList;
            this.name = name;
            this.row = row;
            this.numRows = numRows;
            this.directxFont = directxFont;
            this.mainVisualization = mainVisualization;
            this.device = device;
        }


        /// <summary>
        /// Sets the number of rows that this plot shares the device with. We use 1/numRows share of the device.
        /// </summary>
        /// <param name="numRows"></param>
        public void setNumRows(int numRows)
        {
            this.numRows = numRows;
            
            // Everything is no longer valid.
            this.validAxis = false;
            this.validCursor = false;
            this.validLogPlot = false;
            this.validSelection = false;
            this.validMouse = false;
        }


        /// <summary>
        /// Determines if call number 'row' is selected.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="selectStart"></param>
        /// <param name="selectEnd"></param>
        /// <returns></returns>
        public bool isCallSelected(oSingleData rowData, double selectStart, double selectEnd)
        {
            // Get this row and column
            if (functionList != null && functionList.dataVis != null)
            {
                // Check if this function list contains the specified rowData
                if (functionList.dataVis.contains(rowData) )
                {
                    return rowData.time <= selectEnd && rowData.time >= selectStart;
                }
            }
            return false;
        }

        /// <summary>
        /// Counts the number of times the function has been called in the selected region.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="selectStart"></param>
        /// <param name="selectEnd"></param>
        /// <returns></returns>
        public int countCallSelected(uint address, double selectStart, double selectEnd )
        {
            if ( functionList != null && functionList.dataVis != null)
            {
                return functionList.dataVis.getCallCount(address, selectEnd, selectEnd - selectStart);
            }
            return 0;
        }

        /// <summary>
        /// Paint this timeseries
        /// </summary>
        /// <param name="device"></param>
        public void render(double cursorTime, double cursorWidth, double selectStart, double selectEnd, DISPLAY_RANGE range, int mouseX, int mouseY, float height, bool isSelected, bool hasCursor, bool drawAxis)
        {
            try
            {
                // Determine the y-offset
                float yOffset = (height/(float)numRows) * (float) row;

                // Update the vertices as necessary
                updateLogPlotVertices(range, height / (float)numRows, yOffset, cursorTime, cursorWidth);
                updateCursorVertices(cursorTime, cursorWidth, range, height / (float)numRows, yOffset, hasCursor);
                if( drawAxis )
                    updateAxisVertices(range, height / (float)numRows, yOffset);
                updateMouseVertices(mouseX, mouseY, cursorWidth, range, height / (float)numRows, yOffset);
                updateSelectionVertices(selectStart, selectEnd, range, height / (float)numRows, yOffset, isSelected);

                // Draw the main play bar sections
                RenderSelectionBackground();
                if (drawAxis)
                    RenderAxis();
                RenderMouse();
                RenderFrequency();
                RenderCursor();
                RenderName(height / (float)numRows, yOffset);

            }
            catch (Exception ex)
            {
            }
        }

        private void RenderName(float height, float yOffset)
        {
            // Draw the name of the funciton list
            directxFont.DrawText(null, name, new Point(10, (int) (yOffset + height*0.3f)), Color.Yellow);
        }

        public void invalidateAll()
        {
            // Invalidate all vertex buffers.
            invalidateCursor();
            invalidateAxis();
            invalidateLogPlot();
            invalidateMouse();
            invalidateSelection();
        }

        public void invalidateCursor()
        {
            // Cursor is no longer valid.
            this.validCursor = false;
        }

        public void invalidateAxis()
        {
            // Axis is no longer valid.
            this.validAxis = false;
        }

        public void invalidateLogPlot()
        {
            // Logplot is no longer valid.
            this.validLogPlot = false;
        }

        public void invalidateSelection()
        {
            // Selection is no longer valid.
            this.validSelection = false;
        }

        public void invalidateMouse()
        {
            // Selection is no longer valid.
            this.validMouse = false;
        }

        /// <summary>
        /// Sets the currently active function list.
        /// </summary>
        /// <param name="data">The function list currently viewable.</param>
        public void setData(oFunctionList newList)
        {
            // Update the data
            functionList = newList;
            
            // Invalidate everything
            validAxis = false;
            validCursor = false;
            validLogPlot = false;
            validSelection = false;
            validMouse = false;
        }

        public void setDevice(Device device, Font drawingFont)
        {
            // Update the device
            this.device = device;
            this.directxFont = drawingFont;

            // Invalidate everything
            validAxis = false;
            validCursor = false;
            validLogPlot = false;
            validSelection = false;
            validMouse = false;
        }

        private void RenderFrequency()
        {
            if (vertexBufferLogPlot != null)
            {
                // Draw the function call log-plot
                device.SetStreamSource(0, vertexBufferLogPlot, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.LineStrip, 0, verticesLogPlot.Length);
            }
        }

        private void RenderSelectionBackground()
        {
            if (vertexBufferSelectionBackground != null)
            {
                // Draw the selection start and end bars
                device.SetStreamSource(0, vertexBufferSelectionBackground, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, verticesSelectionBackground.Length / 3);
            }
        }

        private void RenderMouse()
        {
            if (vertexBufferMouse != null && verticesMouse.Length > 0)
            {
                // Draw the selection start and end bars
                device.SetStreamSource(0, vertexBufferMouse, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.LineList, 0, verticesMouse.Length / 2);

                // Draw the text to indicate the number of calls at this location
                directxFont.DrawText(null, string.Format("Calls : {0}", mouseNumCalls), mouseTextPos, Color.Yellow);
            }
        }

        private void RenderCursor()
        {
            // No longer drawing a cursor
            return;
            if (vertexBufferCursor != null && verticesCursor.Length > 0)
            {
                // Draw the selection start and end bars
                device.SetStreamSource(0, vertexBufferCursor, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.LineList, 0, verticesCursor.Length / 2);
            }
        }

        private void RenderAxis()
        {
            if (vertexBufferAxis != null && verticesAxis.Length > 0)
            {
                // Draw the axis markers
                device.SetStreamSource(0, vertexBufferAxis, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.LineList, 0, verticesAxis.Length / 2);
            }
        }


        private void updateAxisVertices(DISPLAY_RANGE range, float height, float yOffset)
        {
            if (device == null || functionList == null)
                return;

            if (!validAxis)
            {
                // Update the axis grid vertices
                System.Single z = 0.5f;
                System.Single yMin = yOffset;
                System.Single yMax = height + yOffset;

                // Pick the timescale
                float timecale = (range.tMax - range.tMin > 15 ? 1 : 10 );

                // Calculate the number of vertices
                // 2 vertices per line. 1 main major marker per second, 9 minor markers inbetween.
                int numMarkers = (int)(timecale * (range.tMax - range.tMin));
                verticesAxis = new CustomVertex.TransformedColored[2*numMarkers];

                // Loop through creating the markers
                float firstMarkerTime = ((float)((int)(timecale * range.tMin))) / timecale;
                for (int i = 0; i < numMarkers; i++)
                {
                    Single time = i * (1 / timecale) + firstMarkerTime;
                    
                    // Create this axis line
                    if ((Single) ((int) time) == time)
                    {
                        // Major marker
                        verticesAxis[2*i] =
                            new CustomVertex.TransformedColored(
                                (float)
                                ((int)
                                 ((time - range.tMin)/(range.tMax - range.tMin)*
                                  (range.xMax - range.xMin) + range.xMin)),
                                (float) ((int) yMin), z, 1.0f, Color.FromArgb(150, 255, 150).ToArgb());
                        verticesAxis[2*i + 1] =
                            new CustomVertex.TransformedColored(
                                (float)
                                ((int)
                                 ((time - range.tMin)/(range.tMax - range.tMin)*
                                  (range.xMax - range.xMin) + range.xMin)),
                                (float) ((int) ((yMax-yMin)/2+yMin)), z, 1.0f, Color.FromArgb(150, 255, 150).ToArgb());
                    }
                    else
                    {
                        // Minor Marker
                        verticesAxis[2*i] =
                            new CustomVertex.TransformedColored(
                                (float)
                                ((int)
                                 ((time - range.tMin)/(range.tMax - range.tMin)*
                                  (range.xMax - range.xMin) + range.xMin)),
                                (float) ((int) yMin), z, 1.0f, Color.FromArgb(100, 170, 100).ToArgb());
                        verticesAxis[2*i + 1] =
                            new CustomVertex.TransformedColored(
                                (float)
                                ((int)
                                 ((time - range.tMin)/(range.tMax - range.tMin)*
                                  (range.xMax - range.xMin) + range.xMin)),
                                (float)((int)((yMax - yMin) / 4 + yMin)), z, 1.0f, Color.FromArgb(100, 170, 100).ToArgb());
                    }
                }

                // Write the vertex drawing buffers
                if (verticesAxis.Length > 0)
                {
                    if (vertexBufferAxis != null)
                        vertexBufferAxis.Dispose();
                    vertexBufferAxis = new VertexBuffer(typeof (CustomVertex.TransformedColored),
                                                        verticesAxis.Length,
                                                        device,
                                                        0,
                                                        CustomVertex.TransformedColored.Format,
                                                        Pool.Default);
                    GraphicsStream stm = vertexBufferAxis.Lock(0, 0, 0);
                    stm.Seek(0, SeekOrigin.Begin);
                    stm.Write(verticesAxis);
                    vertexBufferAxis.Unlock();
                }

                validAxis = true;
            }
        }

        private void updateSelectionVertices(double selectStart, double selectEnd, DISPLAY_RANGE range, float height, float yOffset, bool isSelected)
        {
            if (device == null || functionList == null)
                return;

            if (!validSelection)
            {
                int colour;
                if (isSelected)
                {
                    colour = Color.FromArgb(80, 80, 80).ToArgb(); // A bit lighter gray
                }else
                {
                    colour = Color.FromArgb(40, 40, 40).ToArgb(); // Very dark gray
                }

                // Update the selection vertices
                verticesSelectionBackground = new CustomVertex.TransformedColored[3*2]; // Two triangles for a box.

                // General parameters
                System.Single z = 0.5f;
                System.Single yMin = yOffset;
                System.Single yMax = height + yOffset;

                int i = 0;

                // Draw the background
                verticesSelectionBackground[i++] =
                    new CustomVertex.TransformedColored(
                        (float)
                        (((selectStart - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) + range.xMin),
                        yMin, z, 1.0f, colour);

                verticesSelectionBackground[i++] =
                    new CustomVertex.TransformedColored(
                        (float)
                        (((selectEnd - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) + range.xMin),
                        yMin, z, 1.0f, colour);

                verticesSelectionBackground[i++] =
                    new CustomVertex.TransformedColored(
                        (float)
                        (((selectEnd - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) + range.xMin),
                        yMax, z, 1.0f, colour);

                verticesSelectionBackground[i++] =
                    new CustomVertex.TransformedColored(
                        (float)
                        (((selectStart - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) + range.xMin),
                        yMin, z, 1.0f, colour);

                verticesSelectionBackground[i++] =
                    new CustomVertex.TransformedColored(
                        (float)
                        (((selectStart - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) + range.xMin),
                        yMax, z, 1.0f, colour);

                verticesSelectionBackground[i++] =
                    new CustomVertex.TransformedColored(
                        (float)
                        (((selectEnd - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) + range.xMin),
                        yMax, z, 1.0f, colour);

                if (vertexBufferSelectionBackground != null)
                    vertexBufferSelectionBackground.Dispose();
                vertexBufferSelectionBackground = new VertexBuffer(typeof (CustomVertex.TransformedColored),
                                                                   verticesSelectionBackground.Length,
                                                                   device,
                                                                   0,
                                                                   CustomVertex.TransformedColored.Format,
                                                                   Pool.Default);

                GraphicsStream stm = vertexBufferSelectionBackground.Lock(0, 0, 0);
                stm.Seek(0, SeekOrigin.Begin);
                stm.Write(verticesSelectionBackground);
                vertexBufferSelectionBackground.Unlock();

                validSelection = true;
            }
        }

        private void updateLogPlotVertices(DISPLAY_RANGE range, float height, float yOffset, double cursorTime, double cursorWidth)
        {
            // Check that we have a valid device
            if (device != null && functionList != null)
            {
                // Load the data
                List<oSingleData> rawData = this.functionList.getData();

                if (rawData != null && rawData.Count > 0)
                {
                    if (!validLogPlot || lastCallCount != rawData.Count || vertexBufferLogPlot == null || vertexBufferLogPlot.Disposed || range.tMax != lastTime)
                    {
                        // Update the main visualizataion to show the new data

                        // Gather the data
                        List<oSingleData> data = functionList.getDataRange(range.tMax, cursorWidth*2);;
                        //List<oSingleData> data = functionList.getDataRange(range.tMax, range.tMax - lastTime); ;

                        // Now tell the main visualization to redraw
                        mainVisualization.update(data);

                        // Calculate the viewing range information
                        validAxis = false;
                        lastTime = range.tMax;
                        System.Single z = 0.5f;
                        System.Single yMin = 1 + yOffset;
                        System.Single yMax = height - 1 + yOffset;

                        // Create a value array to go with the vertexes
                        double[] functionCount = new double[(int)((range.tMaxDataset - range.tMinDataset) / timeWidth + 0.5)];
                        double[] times = new double[functionCount.Length];
                        for (int i = 0; i < functionCount.Count(); i++)
                        {
                            functionCount[i] = 0;
                            times[i] = 0;
                        }

                        // Loop through the time, calculating the call count associated with each time frame
                        double maxFunctionCount = 0;
                        int index = 0;
                        for (int i = 0; i < functionCount.Count(); i++)
                        {
                            // Process this time step
                            int tmpCount = this.functionList.getDataSize(i * timeWidth + range.tMinDataset, timeWidth);

                            // This time index record is associated with this vertex
                            double count = Math.Log10(tmpCount + 1);

                            double nextCount = 0.0;
                            if (i + 2 < functionCount.Count())
                            {
                                // Calculate the next count
                                nextCount = Math.Log10(this.functionList.getDataSize((i + 1) * timeWidth + range.tMinDataset, timeWidth) + 1);
                            }

                            if (count < 0)
                                count = 0;
                            if (nextCount < 0)
                                nextCount = 0;


                            if ((index == 0 || functionCount[index - 1] != count) || (nextCount != count) || index == functionCount.Length - 1)
                            {
                                // Create a new vertex
                                functionCount[index] = count;

                                // Set the time for this call
                                times[index] = i * timeWidth + range.tMinDataset;

                                if (functionCount[index] > maxFunctionCount)
                                    maxFunctionCount = functionCount[index];

                                index++;
                            }
                        }

                        // Generate the log-graph verticesLogPlot
                        if (maxFunctionCount <= 1)
                        {
                            maxFunctionCount = 1;
                        }

                        // Initialize the vertex array
                        verticesLogPlot = new CustomVertex.TransformedColored[index + 2];

                        // Generate the log-scale frequency plot verticesLogPlot
                        System.Single x;
                        System.Single y;
                        for (int i = 0; i < index; i++)
                        {
                            // Generate this vertex
                            x = (float)((((double)times[i] - range.tMin) / (range.tMax - range.tMin)) * (range.xMax - range.xMin) + range.xMin);
                            y = (float)((yMin - yMax) * (functionCount[i] / maxFunctionCount) + yMax);

                            verticesLogPlot[i] = new CustomVertex.TransformedColored((float)((int)(x)), (float)((int)y), z, 1.0f, Color.FromArgb(255, 255, 255).ToArgb());
                        }

                        // Create the last two vertices
                        x = (float)range.xMax;
                        y = (float)yMax;

                        verticesLogPlot[index] = new CustomVertex.TransformedColored((float)((int)(x)), (float)((int)y), z, 1.0f, Color.FromArgb(255, 255, 255).ToArgb());
                        verticesLogPlot[index + 1] = new CustomVertex.TransformedColored((float)((int)(x)), (float)(10000000000), z, 1.0f, Color.FromArgb(0, 0, 0).ToArgb());


                        // Setup the vertex buffer
                        if (vertexBufferLogPlot != null)
                            vertexBufferLogPlot.Dispose();
                        vertexBufferLogPlot = new VertexBuffer(typeof(CustomVertex.TransformedColored),
                                                             verticesLogPlot.Length,
                                                             device,
                                                             0,
                                                             CustomVertex.TransformedColored.Format,
                                                             Pool.Default);
                        lastCallCount = rawData.Count;

                        try
                        {
                            GraphicsStream stm = vertexBufferLogPlot.Lock(0, 0, 0);
                            stm.Seek(0, SeekOrigin.Begin);
                            stm.Write(verticesLogPlot);
                            vertexBufferLogPlot.Unlock();
                        }
                        catch (Exception ex)
                        {
                            // Do nothing
                        }
                    }

                    validLogPlot = true;
                }
                else
                {
                    // An empty dataset
                    vertexBufferLogPlot = null;
                }
            }
        }

        private void updateMouseVertices(int x, int y, double cursorWidth, DISPLAY_RANGE range, float height, float yOffset)
        {
            if (device == null || functionList == null)
                return;

            if (!validMouse)
            {
                System.Single yMin = yOffset;
                System.Single yMax = height + yOffset;

                // Check if the cursor is in this row
                bool inRow = false;
                if (y >= (int)yMin && y <= (int)yMax)
                    inRow = true;

                // Set the mouse text values
                mouseTextPos = new Point(x + 2, (int) yMin + 3);
                mouseNumCalls =
                    functionList.getDataSize(
                        ((double) (x - range.xMin)/(double) (range.xMax - range.xMin))*(range.tMax - range.tMin) +
                        range.tMin, cursorWidth);


                if (inRow)
                {
                    // Initialize the vertices
                    verticesMouse = new CustomVertex.TransformedColored[4];
                    for (int n = 0; n < verticesMouse.Length; n++)
                        verticesMouse[n] = new CustomVertex.TransformedColored(0.0f, 0.0f, 0.0f, 1.0f,
                                                                               Color.FromArgb(0, 255, 0).
                                                                                   ToArgb());

                    // Set the vertice positions
                    int i = 0;
                    verticesMouse[i].X = x + 2;
                    verticesMouse[i].Y = yMin;
                    verticesMouse[i++].Z = 0.4f;
                    verticesMouse[i].X = x + 2;
                    verticesMouse[i].Y = yMax;
                    verticesMouse[i++].Z = 0.4f;

                    verticesMouse[i].X = x - 2;
                    verticesMouse[i].Y = yMin;
                    verticesMouse[i++].Z = 0.4f;
                    verticesMouse[i].X = x - 2;
                    verticesMouse[i].Y = yMax;
                    verticesMouse[i++].Z = 0.4f;
                }else
                {
                    // Create the vertex buffer for the current mouse location

                    // Initialize the vertices
                    verticesMouse = new CustomVertex.TransformedColored[2];
                    for (int n = 0; n < verticesMouse.Length; n++)
                        verticesMouse[n] = new CustomVertex.TransformedColored(0.0f, 0.0f, 0.0f, 1.0f,
                                                                               Color.FromArgb(0, 100, 0).
                                                                                   ToArgb());

                    // Set the vertice positions
                    int i = 0;
                    verticesMouse[i].X = x;
                    verticesMouse[i].Y = yMin;
                    verticesMouse[i++].Z = 0.4f;

                    verticesMouse[i].X = x;
                    verticesMouse[i].Y = yMax;
                    verticesMouse[i++].Z = 0.4f;
                }

                // Write the vertex drawing buffers
                if (vertexBufferMouse != null)
                    vertexBufferMouse.Dispose();
                vertexBufferMouse = new VertexBuffer(typeof (CustomVertex.TransformedColored),
                                                     verticesMouse.Length,
                                                     device,
                                                     0,
                                                     CustomVertex.TransformedColored.Format,
                                                     Pool.Default);
                GraphicsStream stm = vertexBufferMouse.Lock(0, 0, 0);
                stm.Seek(0, SeekOrigin.Begin);
                stm.Write(verticesMouse);
                vertexBufferMouse.Unlock();

                validMouse = true;
            }
        }

        private void updateCursorVertices(double cursorTime, double cursorWidth, DISPLAY_RANGE range, float height, float yOffset, bool hasCursor)
        {
            if (device == null || functionList == null)
                return;

            if (!validCursor)
            {
                System.Single yMin = 2 + yOffset;
                System.Single yMax = height - 2 + yOffset;

                // If the cursor is in this row, tell the main visualization to redraw with our cursor data.
                if (hasCursor)
                {
                    // Gather the data
                    List<oSingleData> data;
                    if (functionList != null)
                        data = functionList.getDataRange(cursorTime, cursorWidth);
                    else
                        data = new List<oSingleData>(0);

                    // Now tell the main visualization to redraw
                    mainVisualization.update(data);


                    // Create the vertex buffer for the current mouse location
                    if (verticesCursor == null)
                    {
                        // Initialize the vertices
                        verticesCursor = new CustomVertex.TransformedColored[2*5];
                        for (int n = 0; n < verticesCursor.Length; n++)
                        {
                            verticesCursor[n] = new CustomVertex.TransformedColored(0.0f, 0.0f, 0.4f, 1.0f,
                                                                                    Color.FromArgb(255, 255, 0).ToArgb());
                        }
                    }

                    // Set the vertice positions
                    int i = 0;
                    float xEnd = (float) ((cursorTime - range.tMin)/(range.tMax - range.tMin))*(range.xMax - range.xMin) +
                                 range.xMin;
                    float xStart = (float) ((cursorTime - cursorWidth - range.tMin)/(range.tMax - range.tMin))*
                                   (range.xMax - range.xMin) + range.xMin;
                    verticesCursor[i].X = xEnd - 3;
                    verticesCursor[i++].Y = yMin;
                    verticesCursor[i].X = xEnd + 3;
                    verticesCursor[i++].Y = yMin;

                    verticesCursor[i].X = xEnd - 3;
                    verticesCursor[i++].Y = yMax;
                    verticesCursor[i].X = xEnd + 3;
                    verticesCursor[i++].Y = yMax;

                    verticesCursor[i].X = xEnd;
                    verticesCursor[i++].Y = yMin;
                    verticesCursor[i].X = xEnd;
                    verticesCursor[i++].Y = yMax;

                    verticesCursor[i].X = xStart;
                    verticesCursor[i++].Y = (yMax - yMin) / 2 + yMin;
                    verticesCursor[i].X = xEnd;
                    verticesCursor[i++].Y = (yMax - yMin) / 2 + yMin;

                    verticesCursor[i].X = xStart;
                    verticesCursor[i++].Y = (yMax - yMin) * 0.4f + yMin;
                    verticesCursor[i].X = xStart;
                    verticesCursor[i++].Y = (yMax - yMin) * 0.6f + yMin;

                    // Write the vertex drawing buffers
                    if (vertexBufferCursor != null)
                        vertexBufferCursor.Dispose();
                    vertexBufferCursor = new VertexBuffer(typeof (CustomVertex.TransformedColored),
                                                          verticesCursor.Length,
                                                          device,
                                                          0,
                                                          CustomVertex.TransformedColored.Format,
                                                          Pool.Default);
                    GraphicsStream stm = vertexBufferCursor.Lock(0, 0, 0);
                    stm.Seek(0, SeekOrigin.Begin);
                    stm.Write(verticesCursor);
                    vertexBufferCursor.Unlock();
                }
                else
                {
                    // Cursor is not in this row
                    vertexBufferCursor = null;
                }

                validCursor = true;
            }
        }

        /// <summary>
        /// Cleans up the mouse buffer vertices.
        /// </summary>
        private void mouseLeave()
        {
            if (vertexBufferMouse != null)
            {
                vertexBufferMouse.Dispose();
                vertexBufferMouse = null;
            }
        }

        /// <summary>
        /// Gets the function list associated with this timeseries plot.
        /// </summary>
        /// <returns></returns>
        public oFunctionList getFunctionList()
        {
            return functionList;
        }

        public void setRow(int row)
        {
            this.row = row;
            this.invalidateAll();
        }
    }
}
