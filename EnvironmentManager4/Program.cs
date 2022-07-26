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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public class GetInstaller
    {
        public string Path { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }

        public GetInstaller(string Path, string Product, string Version)
        {
            this.Path = Path;
            this.Product = Product;
            this.Version = Version;
        }
    }

    public class Installer
    {
        public string BuildPath { get; set; }
        public string InstallerPath { get; set; }
        public string DefaultInstallPath { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }

        //public Installer(string BuildPath, string InstallerPath, string DefaultInstallPath, string Product, string Version)
        //{
        //    this.BuildPath = BuildPath;
        //    this.InstallerPath = InstallerPath;
        //    this.DefaultInstallPath = DefaultInstallPath;
        //    this.Product = Product;
        //    this.Version = Version;
        //}
    }
}
