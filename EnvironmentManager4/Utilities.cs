using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class Utilities
    {

        //Setup File Video - https://www.youtube.com/watch?v=tiHBwAp_Kz4&t=179s

        public static string GetSettingsFile()
        {
            //return Environment.CurrentDirectory + @"\Files\Settings.json";
            return @"C:\Program Files (x86)\EnvMgr\Files\Settings.json";
        }

        public static string GetInstallerFolder()
        {
            //return Environment.CurrentDirectory + @"\Installers";
            return @"C:\Program Files (x86)\EnvMgr\Installers";
        }

        public static string GetDLLsFolder()
        {
            return Environment.CurrentDirectory + @"\Dlls";
            ////return @"C:\Program Files (x86)\EnvMgr\Dlls";
        }

        public static string GetConfigurationsFile()
        {
            return @"C:\Users\steve.rodriguez\Downloads\Configurations4.json";
            //return @"C:\Users\steve.rodriguez\Downloads\Configurations2.json";
        }

        public static string RetrieveExe(string product, bool filter = false)
        {
            string executable = "";
            switch (product)
            {
                case "SalesPad GP":
                    executable = "SalesPad.exe";
                    break;
                case "DataCollection":
                    executable = "SalesPad Inventory Manager Extended Warehouse.exe";
                    //executable = "DataCollection Extended Warehouse.exe";
                    break;
                case "Inventory Manager":
                    executable = "SalesPad Inventory Manager Extended Warehouse.exe";
                    break;
                case "SalesPad Mobile":
                    executable = "SalesPad.GP.Mobile.Server.exe";
                    break;
                case "ShipCenter":
                    executable = "SalesPad.ShipCenter.exe";
                    break;
            }
            if (filter)
            {
                executable = String.Format("*{0}", executable);
            }
            return executable;
        }

        public static string GetWiFiIPAddress()
        {
            string local = "";
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface network in networkInterfaces)
            {
                IPInterfaceProperties properties = network.GetIPProperties();
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (network.Name == "Wi-Fi")
                    {
                        return address.Address.ToString();
                    }
                    local = "NOT CONNECTED";
                }
            }
            return local;
        }

        public static string GetSalesPadVPNIPAddress()
        {
            string ipAddress = "";
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface network in networkInterfaces)
            {
                IPInterfaceProperties properties = network.GetIPProperties();
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (network.Name == "SalesPad VPN")
                    {
                        return address.Address.ToString();
                    }
                    ipAddress = "NOT CONNECTED";
                }
            }
            return ipAddress;
        }

        public static void CreateDefaultSettingsFile()
        {
            List<string> dbList = new List<string>();

            var dbManagement = new DbManagement
            {
                DatabaseBackupDirectory = "",
                SQLServer = "",
                Databases = dbList,
                LockedIn = false
            };

            var buildManagement = new BuildManagement
            {
                SalesPadx86Directory = @"C:\Program Files (x86)\SalesPad.Desktop",
                SalesPadx64Directory = @"C:\Program Files\SalesPad.Desktop",
                DataCollectionDirectory = @"C:\Program Files (x86)\DataCollection",
                SalesPadMobileDirectory = @"C:\Program Files (x86)\SalesPad.GP.Mobile.Server",
                ShipCenterDirectory = @"C:\Program Files (x86)\ShipCenter",
                GPWebDirectory = @"C:\inetpub\wwwroot\SalesPadWebPortal",
                WebAPIDirectory = @"C:\inetpub\wwwroot\SalesPadWebAPI"
            };

            var other = new Other
            {
                Mode = "Standard"
            };

            var settings = new SettingsModel
            {
                DbManagement = dbManagement,
                BuildManagement = buildManagement,
                Other = other
            };

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON |*.json";
            saveFileDialog.Title = "Save Settings File";
            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                    string message = "The file was successfully saved.";
                    string caption = "SUCCESS";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Exclamation;

                    MessageBox.Show(message, caption, buttons, icon);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error saving the file to {0}, error is as follows:\n\n{1}\n\n{2}", saveFileDialog.FileName, e.Message, e.ToString()));
                }
                return;
            }
        }

        public static void CreateCoreModulesFile()
        {
            List<string> coreModulesList = new List<string>
            {
                "SalesPad.Module.App.dll",
                "SalesPad.Module.ARTransactionEntry.dll",
                "SalesPad.Module.AvaTax.dll",
                "SalesPad.Module.Ccp.dll",
                "SalesPad.Module.CRM.dll",
                "SalesPad.Module.Dashboard.dll",
                "SalesPad.Module.DistributionBOM.dll",
                "SalesPad.Module.DocumentManagement.dll",
                "SalesPad.Module.EquipmentManagement.dll",
                "SalesPad.Module.FedExServiceManager.dll",
                "SalesPad.Module.GP2010.dll",
                "SalesPad.Module.GP2010SP2.dll",
                "SalesPad.Module.GP2013.dll",
                "SalesPad.Module.GP2013R2.dll",
                "SalesPad.Module.GP2015.dll",
                "SalesPad.Module.Inventory.dll",
                "SalesPad.Module.NodusPayFabric.dll",
                "SalesPad.Module.Printing.dll",
                "SalesPad.Module.Purchasing.dll",
                "SalesPad.Module.QuickReports.dll",
                "SalesPad.Module.Reporting.dll",
                "SalesPad.Module.ReturnsManagement.dll",
                "SalesPad.Module.Sales.dll",
                "SalesPad.Module.SalesEntryQuickPick.dll",
                "SalesPad.Module.SignaturePad.dll",
                "SalesPad.Module.SquareIntegration.dll"
            };

            var coreModules = new CoreModules
            {
                DLLName = coreModulesList
            };

            string json = JsonConvert.SerializeObject(coreModules, Formatting.Indented);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON |*.json";
            saveFileDialog.Title = "Save Core Modules File";
            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                    string message = "The file was successfully saved.";
                    string caption = "SUCCESS";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Exclamation;

                    MessageBox.Show(message, caption, buttons, icon);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error saving the file to {0}, error is as follows:\n\n{1}\n\n{2}", saveFileDialog.FileName, e.Message, e.ToString()));
                }
                return;
            }
        }

        public static void GenerateConfigs()
        {
            var ediExt = new List<string>
            {
                "SalesPadEDI"
            };
            var ediCust = new List<string>();
            var ediConfig = new ExtAndCustom
            {
                ConfigurationName = "EDI",
                Extended = ediExt,
                Custom = ediCust
            };

            var aaExt = new List<string>
            {
                "AutomationAgent",
                "AutomationAgentService"
            };
            var aaCust = new List<string>();
            var aaConfig = new ExtAndCustom
            {
                ConfigurationName = "AA",
                Extended = aaExt,
                Custom = aaCust
            };

            var intExt = new List<string>
            {
                "AutomationAgent",
                "AutomationAgentService",
                "Integration",
                "Integration.Magento2",
                "Integration.Shopify"
            };
            var intCust = new List<string>();
            var intConfig = new ExtAndCustom
            {
                ConfigurationName = "Integrations",
                Extended = intExt,
                Custom = intCust
            };

            var spCore = new List<ExtAndCustom>
            {
                ediConfig,
                aaConfig,
                intConfig
            };

            var dcCore = new List<JustCustom>();
            var scCore = new List<JustCustom>();
            var webCore = new List<JustCustom>();
            var apiCore = new List<JustCustom>();

            var configurations = new Configurations
            {
                SalesPad = spCore,
                DataCollection = dcCore,
                ShipCenter = scCore,
                GPWeb = webCore,
                WebAPI = apiCore
            };

            string json = JsonConvert.SerializeObject(configurations, Formatting.Indented);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON |*.json";
            saveFileDialog.Title = "Save Core Modules File";
            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                    string message = "The file was successfully saved.";
                    string caption = "SUCCESS";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Exclamation;

                    MessageBox.Show(message, caption, buttons, icon);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error saving the file to {0}, error is as follows:\n\n{1}\n\n{2}", saveFileDialog.FileName, e.Message, e.ToString()));
                }
                return;
            }
        }

        public static void GenerateConfigsWithNulls()
        {
            var ediExt = new List<string>
            {
                "SalesPadEDI"
            };
            var ediConfig = new ExtAndCustom
            {
                ConfigurationName = "EDI",
                Extended = ediExt,
            };

            var aaExt = new List<string>
            {
                "AutomationAgent",
                "AutomationAgentService"
            };
            var aaConfig = new ExtAndCustom
            {
                ConfigurationName = "AA",
                Extended = aaExt,
            };

            var intExt = new List<string>
            {
                "AutomationAgent",
                "AutomationAgentService",
                "Integration",
                "Integration.Magento2",
                "Integration.Shopify"
            };
            var intConfig = new ExtAndCustom
            {
                ConfigurationName = "Integrations",
                Extended = intExt,
            };

            var spCore = new List<ExtAndCustom>
            {
                ediConfig,
                aaConfig,
                intConfig
            };

            var dcCore = new List<JustCustom>();
            var scCore = new List<JustCustom>();
            var webCore = new List<JustCustom>();
            var apiCore = new List<JustCustom>();

            var configurations = new Configurations
            {
                SalesPad = spCore,
                DataCollection = dcCore,
                ShipCenter = scCore,
                GPWeb = webCore,
                WebAPI = apiCore
            };

            string json = JsonConvert.SerializeObject(configurations, Formatting.Indented);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON |*.json";
            saveFileDialog.Title = "Save Core Modules File";
            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                    string message = "The file was successfully saved.";
                    string caption = "SUCCESS";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Exclamation;

                    MessageBox.Show(message, caption, buttons, icon);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error saving the file to {0}, error is as follows:\n\n{1}\n\n{2}", saveFileDialog.FileName, e.Message, e.ToString()));
                }
                return;
            }
        }
    }
}
