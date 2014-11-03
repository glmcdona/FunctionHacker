using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace BufferOverflowProtection
{
    static class oConsole
    {
        private static bool Debug = true;
        private static bool Log = false;
        public static RichTextBox consoleBox = null;
        public static string logFilePath = "log.txt";
        public static void printMessage(string text)
        {
            if (Log)
                File.AppendAllText(Application.StartupPath + "\\" + logFilePath, "\r\n" + text);

            if (!(consoleBox == null || consoleBox.IsDisposed))
            {
                consoleBox.FindForm().SuspendLayout();
                consoleBox.SuspendLayout();
                if (consoleBox.Text.Length == 0)
                    consoleBox.Text = text;
                else
                    consoleBox.AppendText("\r\n" + text);

                consoleBox.Select(consoleBox.Text.Length - 1, 1);
                consoleBox.ScrollToCaret();
                consoleBox.ResumeLayout();
                consoleBox.FindForm().ResumeLayout();
            }
        }

        public static void printMessageShow(string text)
        {
            MessageBox.Show(text);
            printMessage(text);
        }

        public static void printMessageDebug(string text)
        {
            if (!Debug) return;
            printMessage(text);
        }

        public static void printException(Exception ex)
        {
            printMessage(ex.ToString());
            //Beep(1000, 100);
            MessageBox.Show(ex.ToString());
        }

        [DllImport("Kernel32.dll")]
        public static extern bool Beep(UInt32 frequency, UInt32 duration);
    }
}
