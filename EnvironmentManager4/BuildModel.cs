using System;
using System.Collections.Generic;
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
        public string Entry_Date { get; set; }
        public string Product { get; set; }
        public string InstallPath { get; set; }

        public BuildModel(string Path, string Version, string Entry_Date, string Product, string InstallPath)
        {
            this.Path = Path;
            this.Version = Version;
            this.Entry_Date = Entry_Date;
            this.Product = Product;
            this.InstallPath = InstallPath;
        }
    }
}
