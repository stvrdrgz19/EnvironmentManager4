using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BuildManagement
{
    /// <summary>
    /// This class is used to for reading/writing to the Builds table in the sqlite database
    /// </summary>
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
            List<Builds> builds = Builds.GetInstalledBuilds(this.Product, this.Version);
            foreach (Builds build in builds)
                if (this.InstallPath == build.InstallPath)
                    Process.Start(String.Format(@"{0}\{1}",
                        this.InstallPath,
                        build.Exe));
        }

        public void LaunchInstalledFolder()
        {
            Process.Start(this.InstallPath);
        }
    }
}
