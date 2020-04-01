using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace sweepnet
{
    static class Program
    {
        /* copied from https://stackoverflow.com/questions/49012233/winforms-4k-and-1080p-scaling-high-dpi */
        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]

        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    /* Putting it inside another try/catch as it might not be available on all systems */
                    SetProcessDpiAwareness(2); /* PerMonitorAware */
                }
                catch { }

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
