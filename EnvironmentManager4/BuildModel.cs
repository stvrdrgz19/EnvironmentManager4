using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            Process.Start(String.Format(@"{0}\{1}", this.InstallPath, Utilities.RetrieveExe(this.Product, this.InstallPath)));
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
        public string ProductExecutable { get; set; }

        public static ProductInfo GetProductInfo(string product, string version = null, string dcPath = null)
        {
            SettingsModel settings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            ProductInfo pi = new ProductInfo();
            switch (product)
            {
                case "SalesPad GP":
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\";
                    pi.ProductExecutable = "SalesPad.exe";
                    switch (version)
                    {
                        case "x64":
                            pi.InstallDirectory = settings.BuildManagement.SalesPadx64Directory;
                            break;
                        case "x86":
                        case "Pre":
                            pi.InstallDirectory = settings.BuildManagement.SalesPadx86Directory;
                            break;
                    }
                    break;
                case "DataCollection":
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\DataCollection\";
                    pi.InstallDirectory = settings.BuildManagement.DataCollectionDirectory;
                    if (File.Exists(String.Format(@"{0}\{1}", dcPath, "DataCollection Extended Warehouse.exe")))
                    {
                        pi.ProductExecutable = "DataCollection Extended Warehouse.exe";
                    }
                    if (File.Exists(String.Format(@"{0}\{1}", dcPath, "SalesPad Inventory Manager Extended Warehouse.exe")))
                    {
                        pi.ProductExecutable = "SalesPad Inventory Manager Extended Warehouse.exe";
                    }
                    if (File.Exists(String.Format(@"{0}\{1}", dcPath, "SalesPad Inventory Manager.exe")))
                    {
                        pi.ProductExecutable = "SalesPad Inventory Manager.exe";
                    }
                    break;
                case "SalesPad Mobile":
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\Mobile-Server\";
                    pi.InstallDirectory = settings.BuildManagement.SalesPadMobileDirectory;
                    pi.ProductExecutable = "SalesPad.GP.Mobile.Server.exe";
                    break;
                case "ShipCenter":
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\ShipCenter\";
                    pi.InstallDirectory = settings.BuildManagement.ShipCenterDirectory;
                    pi.ProductExecutable = "SalesPad.ShipCenter.exe";
                    break;
                case "Customer Portal Web":
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Web-Portal\GP";
                    pi.InstallDirectory = settings.BuildManagement.GPWebDirectory;
                    break;
                case "Customer Portal API":
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\SalesPad.WebApi";
                    pi.InstallDirectory = settings.BuildManagement.WebAPIDirectory;
                    break;
            }
            return pi;
        }
    }
}
