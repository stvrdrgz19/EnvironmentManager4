using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            List<Builds> builds = Builds.GetInstalledBuilds(this.Product, this.Version);
            foreach (Builds build in builds)
            {
                if (this.InstallPath == build.InstallPath)
                    Process.Start(String.Format(@"{0}\{1}",
                        this.InstallPath,
                        build.Exe));
            }
        }

        public void LaunchInstalledFolder()
        {
            Process.Start(this.InstallPath);
        }
    }

    public class ProductInfo
    {
        public string InstallDirectory { get; set; }
        public string FileserverDirectory { get; set; }
        public List<string> ProductExecutables { get; set; }
        public List<string> DirectoryFilters { get; set; }

        public static ProductInfo GetProductInfo(string product, string version, bool install = false)
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            ProductInfo pi = new ProductInfo();
            switch (product)
            {
                case Products.SalesPad:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\";
                    pi.ProductExecutables = new List<string> { "SalesPad.exe" };
                    pi.DirectoryFilters = new List<string> { "SalesPad.Desktop",
                        settings.BuildManagement.SalesPadx86Directory.Substring(settings.BuildManagement.SalesPadx86Directory.LastIndexOf('\\')+1),
                        settings.BuildManagement.SalesPadx64Directory.Substring(settings.BuildManagement.SalesPadx64Directory.LastIndexOf('\\')+1)};
                    switch (version)
                    {
                        case "x64":
                            switch (install)
                            {
                                case true:
                                    pi.InstallDirectory = settings.BuildManagement.SalesPadx64Directory;
                                    break;
                                case false:
                                    pi.InstallDirectory = @"C:\Program Files";
                                    break;
                            }
                            break;
                        case "x86":
                        case "Pre":
                            switch (install)
                            {
                                case true:
                                    pi.InstallDirectory = settings.BuildManagement.SalesPadx86Directory;
                                    break;
                                case false:
                                    pi.InstallDirectory = @"C:\Program Files (x86)";
                                    break;
                            }
                            break;
                    }
                    break;
                case Products.DataCollection:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\DataCollection\";
                    pi.ProductExecutables = new List<string> { "DataCollection Extended Warehouse.exe",
                        "SalesPad Inventory Manager Extended Warehouse.exe",
                        "SalesPad Inventory Manager.exe" };
                    pi.DirectoryFilters = new List<string> { "DataCollection",
                        "SalesPad Inventory Manager",
                        settings.BuildManagement.DataCollectionDirectory.Substring(settings.BuildManagement.DataCollectionDirectory.LastIndexOf('\\')+1)};
                    switch (install)
                    {
                        case true:
                            pi.InstallDirectory = settings.BuildManagement.DataCollectionDirectory;
                            break;
                        case false:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.SalesPadMobile:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\Mobile-Server\";
                    pi.ProductExecutables = new List<string> { "SalesPad.GP.Mobile.Server.exe" };
                    pi.DirectoryFilters = new List<string> { "SalesPad.GP.Mobile.Server",
                        settings.BuildManagement.SalesPadMobileDirectory.Substring(settings.BuildManagement.SalesPadMobileDirectory.LastIndexOf('\\')+1)};
                    switch (install)
                    {
                        case true:
                            pi.InstallDirectory = settings.BuildManagement.SalesPadMobileDirectory;
                            break;
                        case false:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.ShipCenter:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\ShipCenter\";
                    pi.ProductExecutables = new List<string> { "SalesPad.ShipCenter.exe" };
                    pi.DirectoryFilters = new List<string> { "ShipCenter",
                        settings.BuildManagement.ShipCenterDirectory.Substring(settings.BuildManagement.ShipCenterDirectory.LastIndexOf('\\')+1)};
                    switch (install)
                    {
                        case true:
                            pi.InstallDirectory = settings.BuildManagement.ShipCenterDirectory;
                            break;
                        case false:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.GPWeb:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Web-Portal\GP";
                    break;
                case Products.WebAPI:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\SalesPad.WebApi";
                    break;
            }
            return pi;
        }
    }

    public class Builds
    {
        public string InstallPath { get; set; }
        public string Exe { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Builds(string path, string exe, DateTime modifiedDate)
        {
            this.InstallPath = path;
            this.Exe = exe;
            this.ModifiedDate = modifiedDate;
        }

        public static List<Builds> GetInstalledBuilds(string product, string version)
        {
            List<Builds> builds = new List<Builds>();
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            foreach (string exe in pi.ProductExecutables)
            {
                List<string> paths = new List<string>();
                foreach (string filter in pi.DirectoryFilters)
                {
                    string[] dirs = Directory.GetDirectories(pi.InstallDirectory, String.Format("{0}*", filter));
                    foreach (string dir in dirs)
                    {
                        paths.AddRange(Directory.GetFiles(dir,
                            String.Format("*{0}", exe),
                            SearchOption.AllDirectories));
                    }

                    foreach (string path in paths)
                    {
                        bool contains = builds.Any(p => p.InstallPath == Path.GetDirectoryName(path));
                        if (!contains)
                            builds.Add(new Builds(Path.GetDirectoryName(path),
                                exe,
                                Directory.GetLastWriteTime(Path.GetDirectoryName(path))));
                    }
                }
            }
            return builds;
        }

        public static void PopulateBuildLists(ListView lv, string product, string version)
        {
            lv.Items.Clear();
            List<Builds> builds = GetInstalledBuilds(product, version);
            builds.Sort(delegate (Builds x, Builds y)
            {
                return x.ModifiedDate.CompareTo(y.ModifiedDate);
            });
            builds.Reverse();

            foreach (Builds build in builds)
            {
                ListViewItem item = new ListViewItem(build.InstallPath);
                item.SubItems.Add(build.ModifiedDate.ToString());
                lv.Items.Add(item);
            }
            Utilities.ResizeUpdateableListViewColumnWidth(lv, 7, 0, 500);
        }
    }
}
