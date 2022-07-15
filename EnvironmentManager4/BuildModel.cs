using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class BuildModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string EntryDate { get; set; }
        public string Product { get; set; }
        public string InstallPath { get; set; }

        public BuildModel(string Path, string Version, string EntryDate, string Product, string InstallPath)
        {
            this.Path = Path;
            this.Version = Version;
            this.EntryDate = EntryDate;
            this.Product = Product;
            this.InstallPath = InstallPath;
        }

        public void LaunchBuild()
        {
            Process.Start(String.Format(@"{0}\{1}", this.InstallPath, Utilities.RetrieveExe(this.Product)));
        }

        public void LaunchInstalledFolder()
        {
            Process.Start(this.InstallPath);
        }
    }
}
