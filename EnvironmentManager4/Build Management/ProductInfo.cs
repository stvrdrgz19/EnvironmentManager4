using System.Collections.Generic;

namespace EnvironmentManager4
{
    public class ProductInfo
    {
        public string InstallDirectory { get; set; }
        public string FileserverDirectory { get; set; }
        public string ModuleNaming { get; set; }
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
                    pi.ModuleNaming = "SalesPad.Module.";
                    pi.ProductExecutables = new List<string> { "SalesPad.exe" };
                    pi.DirectoryFilters = new List<string> { "SalesPad.Desktop",
                        settings.BuildManagement.SalesPadx86Directory.Substring(settings.BuildManagement.SalesPadx86Directory.LastIndexOf('\\')+1),
                        settings.BuildManagement.SalesPadx64Directory.Substring(settings.BuildManagement.SalesPadx64Directory.LastIndexOf('\\')+1)};
                    switch (version)
                    {
                        case "x64":
                            pi.InstallDirectory = @"C:\Program Files";
                            break;
                        case "x86":
                        case "Pre":
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.DataCollection:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\DataCollection\";
                    pi.ModuleNaming = "SalesPad.DataCollection.";
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
                    pi.ModuleNaming = "SalesPad.ShipCenter.";
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
                    pi.ModuleNaming = "SalesPad.GP.RESTv3.";
                    break;
            }
            return pi;
        }
    }
}
