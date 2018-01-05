using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Graphic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                Application.Run(new FrmSchema(args[0]));
            }
            else
                Application.Run(new FrmSchema());
        }
    }
}