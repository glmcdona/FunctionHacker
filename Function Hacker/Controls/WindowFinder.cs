using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

/* ****************************************************************************
 *  RuntimeObjectEditor
 * 
 * Copyright (c) 2005 Corneliu I. Tusnea
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from 
 * the use of this software.
 * Permission to use, copy, modify, distribute and sell this software for any 
 * purpose is hereby granted without fee, provided that the above copyright 
 * notice appear in all copies and that both that copyright notice and this 
 * permission notice appear in supporting documentation.
 * 
 * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
 * www.acorns.com.au
 * ****************************************************************************/

namespace FunctionHacker.Controls
{
    /// <summary>
    /// windowFinder - Control to help find other windows/controls.
    /// </summary>
    [DefaultEvent("ActiveWindowChanged")]
    public class WindowFinder : UserControl
    {
        public event EventHandler ActiveWindowChanged;
        public event EventHandler ActiveWindowSelected;

        #region PInvoke

        #region Consts

        private const uint RDW_INVALIDATE = 0x0001;
        private const uint RDW_INTERNALPAINT = 0x0002;
        private const uint RDW_ERASE = 0x0004;

        private const uint RDW_VALIDATE = 0x0008;
        private const uint RDW_NOINTERNALPAINT = 0x0010;
        private const uint RDW_NOERASE = 0x0020;

        private const uint RDW_NOCHILDREN = 0x0040;
        private const uint RDW_ALLCHILDREN = 0x0080;

        private const uint RDW_UPDATENOW = 0x0100;
        private const uint RDW_ERASENOW = 0x0200;

        private const uint RDW_FRAME = 0x0400;
        private const uint RDW_NOFRAME = 0x0800;

        #endregion

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT point);

        [DllImport("user32.dll")]
        private static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, POINT point);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, [Out] StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lpRect, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        #region RECT

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Height
            {
                get { return Bottom - Top; }
            }

            public int Width
            {
                get { return Right - Left; }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
            }


            public Point Location
            {
                get { return new Point(Left, Top); }
            }


            // Handy method for converting to a System.Drawing.Rectangle
            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }


            public static Rect FromRectangle(Rectangle rectangle)
            {
                return new Rect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }
        }

        #endregion

        #region POINT

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            private readonly int x;
            private readonly int y;

            private POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public POINT ToPoint()
            {
                return new POINT(x, y);
            }

            public static POINT FromPoint(Point pt)
            {
                return new POINT(pt.X, pt.Y);
            }

            public override bool Equals(object obj)
            {
                // I wish we could use the "as" operator
                // and check the type compatibility only
                // once here, just like with reference 
                // types. Maybe in the v2.0 :)
                if (!(obj is POINT))
                {
                    return false;
                }
                POINT point = (POINT) obj;
                if (point.x == x)
                {
                    return (point.y == y);
                }
                return false;
            }

            public override int GetHashCode()
            {
                // this is the Microsoft implementation for the
                // System.Drawing.Point's GetHashCode() method.
                return (x ^ y);
            }

            public override string ToString()
            {
                return string.Format("{{X={0}, Y={1}}}", x, y);
            }
        }

        #endregion

        #endregion

        #region WindowProperties

        public class WindowProperties : IDisposable
        {
            private static readonly Pen DrawPen = new Pen(Brushes.Red, 2);
            private IntPtr detectedWindow = IntPtr.Zero;

            public IntPtr DetectedWindow
            {
                get { return detectedWindow; }
            }

            public Control ActiveWindow
            {
                get
                {
                    if (detectedWindow != IntPtr.Zero)
                    {
                        return FromHandle(detectedWindow);
                    }
                    return null;
                }
            }

            public string Name
            {
                get
                {
                    if (ActiveWindow != null)
                        return ActiveWindow.Name;
                    return null;
                }
            }

            public string Text
            {
                get
                {
                    if (!IsValid)
                        return null;
                    if (IsManaged)
                        return ActiveWindow.Text;
                    StringBuilder builder = new StringBuilder();
                    GetWindowText(detectedWindow, builder, 255);
                    return builder.ToString();
                }
            }

            public string ClassName
            {
                get
                {
                    if (!IsValid)
                        return null;
                    StringBuilder builder = new StringBuilder();
                    GetClassName(detectedWindow, builder, 255);
                    return builder.ToString();
                }
            }

            public bool IsManagedByClassName
            {
                get
                {
                    string className = ClassName;
                    if (className != null && className.StartsWith("WindowsForms10"))
                    {
                        return true;
                    }
                    return false;
                }
            }

            public bool IsValid
            {
                get { return detectedWindow != IntPtr.Zero; }
            }

            public bool IsManaged
            {
                get { return ActiveWindow != null; }
            }

            internal void SetWindowHandle(IntPtr handle)
            {
                Refresh();
                detectedWindow = handle;
                Refresh();
                Highlight();
            }

            public void Refresh()
            {
                if (!IsValid)
                    return;
                IntPtr toUpdate = detectedWindow;
                IntPtr parentWindow = GetParent(toUpdate);
                if (parentWindow != IntPtr.Zero)
                {
                    toUpdate = parentWindow; // using parent
                }

                InvalidateRect(toUpdate, IntPtr.Zero, true);
                UpdateWindow(toUpdate);
                RedrawWindow(toUpdate, IntPtr.Zero, IntPtr.Zero,
                             RDW_FRAME | RDW_INVALIDATE | RDW_UPDATENOW | RDW_ERASENOW | RDW_ALLCHILDREN);

            }

            public void Highlight()
            {
                Rect windowRect;
                GetWindowRect(detectedWindow, out windowRect);

                GetParent(detectedWindow);
                IntPtr windowDc = GetWindowDC(detectedWindow);
                if (windowDc != IntPtr.Zero)
                {
                    Graphics graph = Graphics.FromHdc(windowDc, detectedWindow);
                    graph.DrawRectangle(DrawPen, 1, 1, windowRect.Width - 2, windowRect.Height - 2);
                    graph.Dispose();
                    ReleaseDC(detectedWindow, windowDc);
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                Refresh();
            }

            #endregion
        }

        #endregion

        private bool searching;
        private readonly WindowProperties window;

        public WindowFinder()
        {
            InitializeComponent();
            window = new WindowProperties();
            MouseDown += WindowFinder_MouseDown;
        }

        protected override void Dispose(bool disposing)
        {
            window.Dispose();
            base.Dispose(disposing);
        }

        #region Designer Generated

        #endregion

        #region DetectedWindowProperties

        public WindowProperties Window
        {
            get { return window; }
        }

        #endregion

        #region Start/Stop Search

        public void StartSearch()
        {
            searching = true;
            Cursor.Current = Cursors.Cross;
            Capture = true;

            MouseMove += WindowFinder_MouseMove;
            MouseUp += WindowFinder_MouseUp;
        }

        public void EndSearch()
        {
            MouseMove -= WindowFinder_MouseMove;
            Capture = false;
            searching = false;
            Cursor.Current = Cursors.Default;

            if (ActiveWindowSelected != null)
            {
                ActiveWindowSelected(this, EventArgs.Empty);
            }
        }

        #endregion

        private void WindowFinder_MouseDown(object sender, MouseEventArgs e)
        {
            if (!searching)
                StartSearch();
        }

        private void WindowFinder_MouseMove(object sender, MouseEventArgs e)
        {
            if (!searching)
                EndSearch();

            // Grab the window from the screen location of the mouse.
            POINT windowPoint = POINT.FromPoint(PointToScreen(new Point(e.X, e.Y)));
            IntPtr found = WindowFromPoint(windowPoint);

            // we have a valid window handle
            if (found != IntPtr.Zero)
            {
                // give it another try, it might be a child window (disabled, hidden .. something else)
                // offset the point to be a client point of the active window
                if (ScreenToClient(found, ref windowPoint))
                {
                    // check if there is some hidden/disabled child window at this point
                    IntPtr childWindow = ChildWindowFromPoint(found, windowPoint);
                    if (childWindow != IntPtr.Zero)
                    {
                        // great, we have the inner child
                        found = childWindow;
                    }
                }
            }

            // Is this the same window as the last detected one?
            if (found != window.DetectedWindow)
            {
                window.SetWindowHandle(found);
                Trace.WriteLine("FoundWindow:" + window.Name + ":" + window.Text + " Managed:" + window.IsManaged);
                InvokeActiveWindowChanged();
            }
        }

        private void InvokeActiveWindowChanged()
        {
            if (ActiveWindowChanged != null)
                ActiveWindowChanged(this, EventArgs.Empty);
        }

        private void WindowFinder_MouseUp(object sender, MouseEventArgs e)
        {
            EndSearch();
        }

        public object SelectedObject
        {
            get { return window.ActiveWindow; }
        }

        public IntPtr SelectedHandle
        {
            get { return window.DetectedWindow; }
            set
            {
                window.SetWindowHandle(value);
                InvokeActiveWindowChanged();
            }
        }

        public bool IsManagedByClassName
        {
            get { return window.IsManagedByClassName; }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WindowFinder
            // 
            this.BackgroundImage = global::FunctionHacker.Properties.Resources.scope;
            this.Name = "WindowFinder";
            this.Size = new System.Drawing.Size(32, 32);
            this.ResumeLayout(false);

        }

        public void RefreshWindow()
        {
            window.Refresh();
        }
    }
}