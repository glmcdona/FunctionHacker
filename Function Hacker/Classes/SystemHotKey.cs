using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FunctionHacker.Classes
{
    /// <summary>
    /// Seting global system hotkey
    /// </summary>
    internal class SystemHotKey : IMessageFilter
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Key modifiers
        /// </summary>
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        private const int WM_HOTKEY = 0x0312;
        private const int id = 100;


        private IntPtr _handle;

        /// <summary>
        /// Gets or sets the handle.
        /// </summary>
        /// <value>
        /// The handle.
        /// </value>
        public IntPtr Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        private event EventHandler HotKeyPressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemHotKey"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifier">The modifier.</param>
        /// <param name="hotKeyPressed">The hot key pressed.</param>
        public SystemHotKey(Keys key, KeyModifiers modifier, EventHandler hotKeyPressed)
        {
            HotKeyPressed = hotKeyPressed;
            RegisterHotKey(key, modifier);
            Application.AddMessageFilter(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SystemHotKey"/> is reclaimed by garbage collection.
        /// </summary>
        ~SystemHotKey()
        {
            Application.RemoveMessageFilter(this);
            UnregisterHotKey(_handle, id);
        }

        private void RegisterHotKey(Keys key, KeyModifiers modifier)
        {
            if (key == Keys.None)
                return;

            bool isKeyRegisterd = RegisterHotKey(_handle, id, modifier, key);
            if (!isKeyRegisterd)
                throw new ApplicationException("Hotkey allready in use");
        }

        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        /// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
        /// </returns>
        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    HotKeyPressed(this, new EventArgs());
                    return true;
            }
            return false;
        }
    }
}