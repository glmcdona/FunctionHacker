using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FunctionHacker.Forms;

namespace FunctionHacker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //To get more rights - correct attach to .net processes too
            //Process.EnterDebugMode();
            //Give us exception errors in english even if .net environment is localised
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US"); 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new formMain());
        }
    }
}
