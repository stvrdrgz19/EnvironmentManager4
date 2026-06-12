using EnvironmentManager4.src.Core.Models;
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
            //SettingsModel settings = SettingsUtilities.GetSettings();
            ProductInfo pi = new ProductInfo();
            switch (product)
            {
                case Products.SalesPad:
                //case ProductsOld.SalesPad:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\";
                    pi.ModuleNaming = "SalesPad.Module.";
                    pi.ProductExecutables = new List<string> { "SalesPad.exe" };
                    pi.DirectoryFilters = new List<string> { "SalesPad.Desktop" };
                        //settings.BuildManagement.SalesPadx86Directory.Substring(settings.BuildManagement.SalesPadx86Directory.LastIndexOf('\\')+1),
                        //settings.BuildManagement.SalesPadx64Directory.Substring(settings.BuildManagement.SalesPadx64Directory.LastIndexOf('\\')+1)};
                    switch (version)
                    {
                        case "x64":
                            pi.InstallDirectory = @"C:\Program Files";
                            break;
                        case "x86":
                        case "Pre":
                        case null:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.InventoryControl:
                //case ProductsOld.DataCollection:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\DataCollection\";
                    pi.ModuleNaming = "SalesPad.DataCollection.";
                    pi.ProductExecutables = new List<string> { "DataCollection Extended Warehouse.exe",
                        "SalesPad Inventory Manager Extended Warehouse.exe",
                        "SalesPad Inventory Manager.exe" };
                    pi.DirectoryFilters = new List<string> { "DataCollection",
                        "SalesPad Inventory Manager" };
                        //settings.BuildManagement.DataCollectionDirectory.Substring(settings.BuildManagement.DataCollectionDirectory.LastIndexOf('\\')+1)};
                    switch (install)
                    {
                        case true:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            //pi.InstallDirectory = settings.BuildManagement.DataCollectionDirectory;
                            break;
                        case false:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.SalesPadMobile:
                //case ProductsOld.SalesPadMobile:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Ares\Mobile-Server\";
                    pi.ProductExecutables = new List<string> { "SalesPad.GP.Mobile.Server.exe" };
                    pi.DirectoryFilters = new List<string> { "SalesPad.GP.Mobile.Server" };
                        //settings.BuildManagement.SalesPadMobileDirectory.Substring(settings.BuildManagement.SalesPadMobileDirectory.LastIndexOf('\\')+1)};
                    switch (install)
                    {
                        case true:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            //pi.InstallDirectory = settings.BuildManagement.SalesPadMobileDirectory;
                            break;
                        case false:
                            pi.InstallDirectory = @"C:\Program Files (x86)";
                            break;
                    }
                    break;
                case Products.ShipCenter:
                //case ProductsOld.ShipCenter:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\ShipCenter\";
                    pi.ModuleNaming = "SalesPad.ShipCenter.";
                    pi.ProductExecutables = new List<string> { "SalesPad.ShipCenter.exe" };
                    pi.DirectoryFilters = new List<string> { "ShipCenter" };
                        //settings.BuildManagement.ShipCenterx86Directory.Substring(settings.BuildManagement.ShipCenterx86Directory.LastIndexOf('\\')+1),
                        //settings.BuildManagement.ShipCenterx64Directory.Substring(settings.BuildManagement.ShipCenterx64Directory.LastIndexOf('\\')+1)};
                    switch (install)
                    {
                        case true:
                            switch (version)
                            {
                                case "x64":
                                    pi.InstallDirectory = @"C:\Program Files (x86)";
                                    //pi.InstallDirectory = settings.BuildManagement.ShipCenterx64Directory;
                                    break;
                                case "x86":
                                case "Pre":
                                case null:
                                    pi.InstallDirectory = @"C:\Program Files (x86)";
                                    //pi.InstallDirectory = settings.BuildManagement.ShipCenterx86Directory;
                                    break;
                            }
                            break;
                        case false:
                            switch (version)
                            {
                                case "x64":
                                    pi.InstallDirectory = @"C:\Program Files";
                                    break;
                                case "x86":
                                case "Pre":
                                case null:
                                    pi.InstallDirectory = @"C:\Program Files (x86)";
                                    break;
                            }
                            break;
                    }
                    break;
                case ProductsOld.GPWeb:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\Web-Portal\GP";
                    //pi.InstallDirectory = settings.BuildManagement.GPWebDirectory;
                    break;
                case ProductsOld.WebAPI:
                    pi.FileserverDirectory = @"\\sp-fileserv-01\Shares\Builds\SalesPad.WebApi";
                    pi.ModuleNaming = "SalesPad.GP.RESTv3.";
                    break;
            }
            return pi;
        }

        public static List<string> DefaultInstallPaths()
        {
            //get settings
            AppSettings settings = new AppSettings();

            //build a list of default install paths
            List<string> paths = new List<string>()
            {
                settings.BMSalesPadx86,
                settings.BMSalesPadx64,
                settings.BMDataCollection,
                settings.BMSalesPadMobile,
                settings.BMShipCenterx86,
                settings.BMShipCenterx64,
                settings.GPWeb,
                settings.WebAPI
            };

            return paths;
        }
    }
}
