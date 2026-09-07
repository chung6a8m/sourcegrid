using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
#if NET8_0_OR_GREATER
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
            Application.SetCompatibleTextRenderingDefault(false);

            GenericTest.Run();

            Application.Run(new MainForm());
        }
    }
}
