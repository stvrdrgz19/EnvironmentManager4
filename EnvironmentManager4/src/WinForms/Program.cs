using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Match .NET Framework's layout: forms were designed against its default font and DPI behavior,
            // and .NET 8's larger default (Segoe UI 9pt) makes AutoScaleMode.Font stretch every form
            // The .NET Framework build was DPI-unaware (Windows scaled it as a bitmap); GdiScaled keeps that layout with crisper text
            Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
            Application.SetDefaultFont(new System.Drawing.Font("Microsoft Sans Serif", 8.25F));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
