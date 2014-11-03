using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using BufferOverflowProtection;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Font = Microsoft.DirectX.Direct3D.Font;

namespace FunctionHacker.Classes.Visualization
{
    public struct oVisFunction
    {
        public uint address;
        public int vertexIndex;

        public oVisFunction(uint address, int vertexIndex)
        {
            this.address = address;
            this.vertexIndex = vertexIndex;

            // Create the vertex
            oVisLookup.vertexFunctions[vertexIndex] = new CustomVertex.TransformedColored(0.0f,0.0f, 0.4f, 1.0f, Color.FromArgb(60, 60, 60).ToArgb());
            oVisLookup.functionLocationsHash.Add(address,vertexIndex);
        }

        public void setPos(float x, float y)
        {
            oVisLookup.vertexFunctions[vertexIndex].X = x;
            oVisLookup.vertexFunctions[vertexIndex].Y = y;
        }
    }

    public class oVisModule
    {
        public int x = 0;
        public int y = 0;
        public int width = 100;
        public int heightMax = 0;
        public int height = 0;
        public uint address = 0;
        public string name = "";
        public ArrayList functions = null;
        static private Microsoft.DirectX.Direct3D.Font directxFont = null;

        public void Clear()
        {
            // Reset this vis module
            functions = new ArrayList(0);
            height = 0;
            heightMax = 0;
            width = 100;
            x = 0;
            y = 0;
        }

        public oVisModule(string name, uint address)
        {
            this.name = name;
            this.address = address;
            functions = new ArrayList(0);
        }

        public void setPos(int x, int y, int heightMax, int width)
        {
            this.x = x;
            this.y = y;
            this.heightMax = heightMax;
            this.width = width;
            this.height = 12 + (2 * functions.Count) / (this.width / 2);

            // Reassign all the function coordinates
            for( int i = 0; i < functions.Count; i++)
            {
                // Set this function position
                ((oVisFunction)functions[i]).setPos( x + 15 + (i % (width / 2) ) * 2, y + 15 + ( i / (width / 2) ) * 2);
            }
        }

        public void render(ref Device device, ref Microsoft.DirectX.Direct3D.Font directxFont)
        {
            if (this.functions != null && this.functions.Count > 0)
            {
                // Render this module name and bounding box

                // Draw the module name
                directxFont.DrawText(null, name, x, y, Color.White);

                // TODO: Draw the module border
            }
        }


        /// <summary>
        /// Adds a new function.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Whether a module resize is required.</returns>
        public bool addNewFunction(uint address)
        {
            // Determine a vertex index for this function
            if (oVisLookup.vertexFunctionsCount + 1 >= oVisLookup.vertexFunctionsCount)
            {
                // Allocate more space
                Array.Resize(ref oVisLookup.vertexFunctions, oVisLookup.vertexFunctionsCount + 100);
            }
            
            // Add this function and vertex index
            functions.Add(new oVisFunction(address, oVisLookup.vertexFunctionsCount));

            // Set it's position
            ((oVisFunction)functions[functions.Count - 1]).setPos(x + 15 + ((functions.Count - 1) % (width / 2)) * 2, y + 15 + ((functions.Count - 1) / (width / 2)) * 2);
            oVisLookup.vertexFunctionsCount++;

            // Calculate the new pixel size of this module
            this.width = 100;
            this.height = 12 + (2*functions.Count)/(this.width/2);
            if( height > heightMax )
                return true;
            else
                return false;
        }
    }
    
    public class oVisModuleManager
    {
        private ArrayList moduleList = null;
        private ArrayList moduleBaseAddress = null; // Sorted module base list

        // Column information
        private int[] moduleColumns = null;
        private int[] columnHeights = null;
        private int numColumns = 0;
        private int columnWidth = 100;

        public oVisModuleManager(List<HEAP_INFO> map, int numColumns)
        {
            // Initialize the module base address list
            moduleBaseAddress = new ArrayList((int) map.Count);
            moduleList = new ArrayList((int) map.Count);
            
            foreach( HEAP_INFO heap in map )
            {
                if (heap.associatedModule != null && heap.extra.Contains("PE"))
                {
                    // Add this base address
                    moduleBaseAddress.Add((uint) heap.associatedModule.BaseAddress);

                    // Create this vis module
                    moduleList.Add(new oVisModule(heap.associatedModule.ModuleName,
                                                  (uint) heap.associatedModule.BaseAddress));
                }
            }

            // Initialize the modules
            moduleColumns = new int[moduleList.Count];
            for (int i = 0; i < moduleColumns.Length; i++)
                moduleColumns[i] = -1;

            // Initialize the columns
            this.numColumns = numColumns;
            columnHeights = new int[numColumns];
            for (int i = 0; i < numColumns; i++ )
                columnHeights[i] = 0;
        }

        public void Clear()
        {
            // Clears the data
            foreach (oVisModule module in moduleList)
                module.Clear();

            // Initialize the modules
            moduleColumns = new int[moduleList.Count];
            for (int i = 0; i < moduleColumns.Length; i++)
                moduleColumns[i] = -1;

            // Initialize the columns
            this.numColumns = numColumns;
            columnHeights = new int[numColumns];
            for (int i = 0; i < numColumns; i++)
                columnHeights[i] = 0;

        }

        public void render(ref Device device, ref Microsoft.DirectX.Direct3D.Font directxFont)
        {
            // Render each module
            foreach (oVisModule module in moduleList)
            {
                // Render this module
                if( module != null )
                    module.render(ref device, ref directxFont);
            }
        }

        /// <summary>
        /// Adds the new function to the hash tables and creates the vertex.
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Index in the vertex array for the function.</returns>
        public int addNewFunction(uint address)
        {
            // Validate the arrays
            if( moduleList == null || moduleBaseAddress == null )
                return -1;

            // Determine which module this function is in
            int moduleIndex = moduleBaseAddress.BinarySearch(address);
            if (moduleIndex < 0)
                moduleIndex = ~moduleIndex - 1;

            if( moduleIndex >= moduleBaseAddress.Count || moduleIndex < 0 )
                return -1; // Not a valid address?

            // Add the function
            if( ((oVisModule)moduleList[moduleIndex]).addNewFunction(address) )
            {
                // We need to reposition this module.

                // Check if this module is assigned a column
                if( moduleColumns[moduleIndex] == -1 )
                {
                    // This module has no column yet

                    // Add this module to the shortest column
                    int newColumnIndex = 0;
                    int i;
                    for( i = 0 ; i < numColumns; i++)
                    {
                        if( columnHeights[i] < columnHeights[newColumnIndex] )
                        {
                            newColumnIndex = i;
                        }
                    }

                    // Assign this module position to the end of column i
                    ((oVisModule)moduleList[moduleIndex]).setPos(10 + (columnWidth+10) * newColumnIndex, columnHeights[newColumnIndex],
                                                                  ((oVisModule) moduleList[moduleIndex]).heightMax + 20,
                                                                  this.columnWidth);

                    // Update the column size
                    moduleColumns[moduleIndex] = newColumnIndex;
                    this.columnHeights[newColumnIndex] += 20;
                }else
                {
                    // We need to increase the size of this module
                    
                    // First create a list of modules below this one, so we can shift them all down
                    for (int i = 0; i < moduleList.Count; i++)
                    {
                        if( moduleColumns[i] == moduleColumns[moduleIndex] && ((oVisModule) moduleList[i]).y > ((oVisModule) moduleList[moduleIndex]).y )
                        {
                            // Shift this module down
                            ((oVisModule)moduleList[i]).setPos(10 + (columnWidth + 10) * moduleColumns[i], ((oVisModule)moduleList[i]).y + 20,
                                                                  ((oVisModule)moduleList[i]).heightMax + 20,
                                                                  this.columnWidth);
                        }
                    }

                    // Increase the size of this module
                    ((oVisModule)moduleList[moduleIndex]).setPos(10 + (columnWidth + 10) * moduleColumns[moduleIndex], ((oVisModule)moduleList[moduleIndex]).y,
                                                                  ((oVisModule)moduleList[moduleIndex]).heightMax + 20,
                                                                  this.columnWidth);
                    this.columnHeights[moduleColumns[moduleIndex]] += 20;
                }
            }

            return oVisLookup.vertexFunctionsCount - 1;
        }
        

        public void updateLayout()
        {
            // Update the layout by positioning the modules and functions.
            
        }
    }

    public class oVisLookup
    {
        // Static because this is a list of all call sources, this should never be a subset.
        static private Hashtable directCallSourcesHash = null; // Call source address -> Function address
        static public Hashtable functionLocationsHash = null; // Function address -> vertice index
        static private ArrayList functionLocationsList = null; // Sorted function address list

        // This is the array of vertices. The table hashtables values give the corresponding function index in the function vertices list, if it exists.
        // If it isn't in the function vertices list yet, the hash tables give -1, meaning it needs to be added to the function vertice list.
        static public int vertexFunctionsCount = 0;
        static public CustomVertex.TransformedColored[] vertexFunctions = null;
        static public VertexBuffer vertexBufferFunctions = null;
        static public int vertexLinksCount = 0;
        static public CustomVertex.TransformedColored[] vertexLinks = null;
        static public VertexBuffer vertexBufferLinks = null;
        private static oVisModuleManager moduleManager = null;
        public static ArrayList executedVertexIndices = null;

        public void reset()
        {
            // Clear the dynamic data buffers
            vertexFunctions = null;
            vertexFunctionsCount = 0;
            vertexBufferFunctions = null;
            vertexLinks = null;
            vertexLinksCount = 0;
            vertexBufferLinks = null;
            if (executedVertexIndices != null)
                executedVertexIndices.Clear();
            if (functionLocationsHash != null)
                functionLocationsHash.Clear();
            if (moduleManager != null)
                moduleManager.Clear();
        }

        public void render(ref Device device, ref Microsoft.DirectX.Direct3D.Font directxFont)
        {
            // Render the module names and outlines
            if( moduleManager != null )
                moduleManager.render(ref device, ref directxFont);
            
            // Render the functions
            if (vertexBufferFunctions != null)
            {
                device.SetStreamSource(0, vertexBufferFunctions, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.PointList, 0, vertexFunctionsCount);
            }

            // Render the function links
            if (vertexBufferLinks != null && vertexLinksCount > 0)
            {
                device.SetStreamSource(0, vertexBufferLinks, 0);
                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawPrimitives(PrimitiveType.LineList, 0, vertexLinksCount/2);
            }
        }

        /// <summary>
        /// Sets the data for the current vertex buffers.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="device"></param>
        public void setData(List<oSingleData> data, ref Device device)
        {
            if (data == null || device == null)
                return;

            // Clear the link vertex arrays
            vertexBufferLinks = null;
            vertexLinks = null;

            // Clear the status of the previous executed functions
            foreach (int index in executedVertexIndices)
            {
                // Clear this vertex colouring
                vertexFunctions[index].Color = Color.FromArgb(60, 60, 60).ToArgb();
            }
            executedVertexIndices.Clear();

            // Clear the vertex function call links
            vertexLinks = new CustomVertex.TransformedColored[data.Count*2];
            vertexLinksCount = 0;

            if (data.Count > 0)
            {
                // Convert the data
                //Int32[] callArray = (Int32[]) oMemoryFunctions.RawDataToObject(ref data, typeof (Int32[]));
                //uint[] callArray = oMemoryFunctions.ByteArrayToUintArray(ref data);

                // Process the new function calls)
                for (int i = 0; i < data.Count; i++)
                {
                    // Process this function call

                    // Load the source function address
                    uint sourceFunctionAddress;
                    if (directCallSourcesHash.Contains((uint) data[i].source))
                    {
                        // We can perform a hash table lookup. This was probably a direct fixed offset call.
                        sourceFunctionAddress = (uint) directCallSourcesHash[(uint) data[i].source];
                    }
                    else
                    {
                        // We have to do a binary search lookup, this is more expensive computationally. This was probably a PE table call.
                        int n = functionLocationsList.BinarySearch((uint) data[i].source);
                        if (n < 0)
                            n = ~n - 1;
                        if (n < 0)
                            n = 0;
                        sourceFunctionAddress = (uint) functionLocationsList[n];
                    }

                    // Load the source vertex index, or create a new vertex if needed
                    int sourceVertexIndex;
                    if (functionLocationsHash.Contains(sourceFunctionAddress))
                    {
                        sourceVertexIndex = (int) functionLocationsHash[sourceFunctionAddress];
                    }
                    else
                    {
                        // We need to create the vertex for this funciton
                        sourceVertexIndex = moduleManager.addNewFunction(sourceFunctionAddress);
                    }

                    if (sourceVertexIndex >= 0)
                    {
                        // Highlight the source index function
                        vertexFunctions[sourceVertexIndex].Color = Color.FromArgb(255, 255, 50).ToArgb();
                        executedVertexIndices.Add(sourceVertexIndex);
                    }

                    // Load the destination vertex index, or create a new vertex if needed
                    int destinationVertexIndex;
                    if (functionLocationsHash.Contains((uint) ((oSingleData) data[i]).destination))
                    {
                        destinationVertexIndex = (int) functionLocationsHash[(uint) data[i].destination];
                    }
                    else
                    {
                        // We need to create the vertex for this funciton
                        destinationVertexIndex = moduleManager.addNewFunction((uint) data[i].destination);
                    }

                    if (destinationVertexIndex >= 0)
                    {
                        // Highlight the destination index function
                        vertexFunctions[destinationVertexIndex].Color = Color.FromArgb(255, 255, 50).ToArgb();
                        executedVertexIndices.Add(destinationVertexIndex);
                    }

                    // Draw the link line
                    if (destinationVertexIndex >= 0 && sourceVertexIndex >= 0)
                    {
                        vertexLinks[vertexLinksCount] =
                            new CustomVertex.TransformedColored(vertexFunctions[sourceVertexIndex].X,
                                                                vertexFunctions[sourceVertexIndex].Y + 1,
                                                                vertexFunctions[sourceVertexIndex].Z,
                                                                vertexFunctions[sourceVertexIndex].Rhw,
                                                                Color.FromArgb(150, 0, 0).ToArgb());
                        vertexLinks[vertexLinksCount + 1] =
                            new CustomVertex.TransformedColored(vertexFunctions[destinationVertexIndex].X,
                                                                vertexFunctions[destinationVertexIndex].Y + 1,
                                                                vertexFunctions[destinationVertexIndex].Z,
                                                                vertexFunctions[destinationVertexIndex].Rhw,
                                                                Color.FromArgb(0, 0, 150).ToArgb());
                        vertexLinksCount += 2;
                    }
                }
            }
            else
            {
                vertexLinksCount = 0;
            }

            // Write the function vertex buffers
            if (vertexFunctionsCount > 0)
            {
                CustomVertex.TransformedColored[] vertexFunctionsRange =
                    new CustomVertex.TransformedColored[vertexFunctionsCount];
                Array.Copy(vertexFunctions, vertexFunctionsRange, vertexFunctionsRange.Length);
                if (vertexBufferFunctions != null)
                    vertexBufferFunctions.Dispose();
                vertexBufferFunctions = new VertexBuffer(typeof (CustomVertex.TransformedColored),
                                                         vertexFunctionsCount,
                                                         device,
                                                         0,
                                                         CustomVertex.TransformedColored.Format,
                                                         Pool.Default);
                GraphicsStream stm = vertexBufferFunctions.Lock(0, 0, 0);
                stm.Seek(0, SeekOrigin.Begin);
                stm.Write(vertexFunctionsRange);
                vertexBufferFunctions.Unlock();
                stm.Dispose();
            }

            // Write the link line vertex buffers
            if (vertexLinksCount > 0)
            {
                CustomVertex.TransformedColored[] vertexLinksRange =
                    new CustomVertex.TransformedColored[vertexLinksCount];
                Array.Copy(vertexLinks, vertexLinksRange, vertexLinksRange.Length);
                if (vertexBufferLinks != null)
                    vertexBufferLinks.Dispose();
                vertexBufferLinks = new VertexBuffer(typeof (CustomVertex.TransformedColored),
                                                     vertexLinksCount,
                                                     device,
                                                     0,
                                                     CustomVertex.TransformedColored.Format,
                                                     Pool.Default);

                GraphicsStream stm = vertexBufferLinks.Lock(0, 0, 0);
                stm.Seek(0, SeekOrigin.Begin);
                stm.Write(vertexLinksRange);
                vertexBufferLinks.Unlock();
                stm.Dispose();
            }
            else
            {
                vertexBufferLinks = null;
            }
        }

        /// <summary>
        /// Initializes all the quick-lookup arrays required for high-performance lookups.
        /// </summary>
        /// <param name="functions">Array of oFunction classes. This should be the complete list of functions.</param>
        /// /// <param name="displayPanel">The display panel.</param>
        public void initialize(List<HEAP_INFO> map, List<oFunction> functions, oVisMain displayPanel)
        {
            // Clear the dynamic buffer data
            reset();

            // Create the module manager
            moduleManager = new oVisModuleManager(map,3);

            // Initialize the arrays
            directCallSourcesHash = new Hashtable();
            functionLocationsHash = new Hashtable();
            functionLocationsList = new ArrayList();
            vertexFunctions = new CustomVertex.TransformedColored[0];
            executedVertexIndices = new ArrayList(1000);

            // Create the lookup tables
            foreach (oFunction function in functions)
            {
                // Add this function
                //functionLocationsHash.Add(function.address, -1);
                functionLocationsList.Add(function.address);
            }

            // Sort the list
            functionLocationsList.Sort();

            // Add the direct call sources
            foreach (oFunction function in functions)
            {
                foreach (uint source in function.normalCallers)
                {
                    // Add this lookup entry

                    // Find the function corresponding to this function source
                    int index = functionLocationsList.BinarySearch(source);
                    if (index < 0)
                        index = ~index-1;
                    if (index < 0)
                        index = 0;

                    // Add this function address
                    directCallSourcesHash.Add(source, functionLocationsList[index]);
                }
            }
        }
    }
}
