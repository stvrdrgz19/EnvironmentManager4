using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class Utilities
    {

        //Setup File Video - https://www.youtube.com/watch?v=tiHBwAp_Kz4&t=179s

        public static List<string> productList = new List<string>
        {
            "SalesPad GP"
            ,"DataCollection"
            ,"SalesPad Mobile"
            ,"ShipCenter"
            ,"Customer Portal Web"
            ,"Customer Portal API"
        };

        public static string GetSettingsFile()
        {
            //return Environment.CurrentDirectory + @"\Files\Settings.json";
            return @"C:\Program Files (x86)\EnvMgr\Files\Settings.json";
            //return @"C:\Users\steve.rodriguez\Desktop\test\EnvMgr Settings\Settings.json";
        }

        public static string GetInstallerFolder()
        {
            return Environment.CurrentDirectory + @"\Installers";
            //return @"C:\Program Files (x86)\EnvMgr\Installers";
        }

        public static string GetDLLsFolder()
        {
            return Environment.CurrentDirectory + @"\Dlls";
            //return @"C:\Program Files (x86)\EnvMgr\Dlls";
        }

        public static string GetNotesFile()
        {
            return Environment.CurrentDirectory + @"\Files\Notes.txt";
            //return @"C:\Program Files (x86)\EnvMgr\Files\Notes.txt";
        }

        public static string GetConfigurationDirectory()
        {
            return String.Format(@"{0}\{1}", Environment.CurrentDirectory, @"\Files\Configurations");
            //return @"C:\Program Files (x86)\EnvMgr\Files\Configurations";
        }

        public static string GetConfigurationsFiles(string product)
        {
            string env = Environment.CurrentDirectory;
            //string env = @"C:\Program Files (x86)\EnvMgr";
            return String.Format(@"{0}\Files\Configurations\{1}", env, product);
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

        public static string TrimEndOfPath(string path)
        {
            //handle if the provided path ends in a backslash
            char lastChar = path[path.Length - 1];
            if (lastChar == '\\')
            {
                TrimEndOfPath(path);
            }
            return path.Substring(0, path.LastIndexOf('\\'));
        }

        public static string GetProductInstallPath(string product, string version)
        {
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string productPath = "";
            switch (product)
            {
                case "SalesPad GP":
                    switch (version)
                    {
                        case "x86":
                            switch (settingsModel.Other.Mode)
                            {
                                case "Standard":
                                    productPath = settingsModel.BuildManagement.SalesPadx86Directory;
                                    break;
                                case "SmartBear":
                                case "Kyle":
                                    productPath = TrimEndOfPath(settingsModel.BuildManagement.SalesPadx86Directory);
                                    break;
                            }
                            break;
                        case "x64":
                            productPath = settingsModel.BuildManagement.SalesPadx64Directory;
                            break;
                    }
                    break;
                case "DataCollection":
                    productPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "Inventory Manager":
                    productPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "SalesPad Mobile":
                    productPath = settingsModel.BuildManagement.SalesPadMobileDirectory;
                    break;
                case "ShipCenter":
                    productPath = settingsModel.BuildManagement.ShipCenterDirectory;
                    break;
            }
            return productPath;
        }

        public static List<string> InstalledBuilds(string product, string version)
        {
            List<string> installedBuilds = new List<string>();
            try
            {
                var buildList = Directory.GetFiles(GetProductInstallPath(product, version) + @"\", Utilities.RetrieveExe(product, true), SearchOption.AllDirectories);
                installedBuilds.AddRange(buildList);
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            return installedBuilds;
        }

        public static void CreateDefaultSettingsFile()
        {
            List<string> dbList = new List<string>();
            List<ConnectionList> connectionList = new List<ConnectionList>();

            var dbManagement = new DbManagement
            {
                DatabaseBackupDirectory = "",
                Connection = "",
                ConnectionsList = connectionList,
                SQLServerUserName = "",
                SQLServerPassword = "",
                Databases = dbList,
                Connected = false
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
            //var ediExt = new List<string>
            //{
            //    "SalesPadEDI"
            //};
            //var ediCust = new List<string>();
            //var ediConfig = new ExtAndCustom
            //{
            //    ConfigurationName = "EDI",
            //    Extended = ediExt,
            //    Custom = ediCust
            //};

            //var aaExt = new List<string>
            //{
            //    "AutomationAgent",
            //    "AutomationAgentService"
            //};
            //var aaCust = new List<string>();
            //var aaConfig = new ExtAndCustom
            //{
            //    ConfigurationName = "AA",
            //    Extended = aaExt,
            //    Custom = aaCust
            //};

            //var intExt = new List<string>
            //{
            //    "AutomationAgent",
            //    "AutomationAgentService",
            //    "Integration",
            //    "Integration.Magento2",
            //    "Integration.Shopify"
            //};
            //var intCust = new List<string>();
            //var intConfig = new ExtAndCustom
            //{
            //    ConfigurationName = "Integrations",
            //    Extended = intExt,
            //    Custom = intCust
            //};

            //var spCore = new List<ExtAndCustom>
            //{
            //    ediConfig,
            //    aaConfig,
            //    intConfig
            //};

            //var dcCore = new List<JustCustom>();
            //var scCore = new List<JustCustom>();
            //var webCore = new List<JustCustom>();
            //var apiCore = new List<JustCustom>();

            //var configurations = new Configurations
            //{
            //    SalesPad = spCore,
            //    DataCollection = dcCore,
            //    ShipCenter = scCore,
            //    GPWeb = webCore,
            //    WebAPI = apiCore
            //};

            //string json = JsonConvert.SerializeObject(configurations, Formatting.Indented);

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "JSON |*.json";
            //saveFileDialog.Title = "Save Core Modules File";
            //saveFileDialog.ShowDialog();

            //if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            //{
            //    try
            //    {
            //        File.WriteAllText(saveFileDialog.FileName, json);
            //        string message = "The file was successfully saved.";
            //        string caption = "SUCCESS";
            //        MessageBoxButtons buttons = MessageBoxButtons.OK;
            //        MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            //        MessageBox.Show(message, caption, buttons, icon);
            //    }
            //    catch (Exception e)
            //    {
            //        MessageBox.Show(String.Format("There was an error saving the file to {0}, error is as follows:\n\n{1}\n\n{2}", saveFileDialog.FileName, e.Message, e.ToString()));
            //    }
            //    return;
            //}
        }

        public static void GenerateConfigsWithNulls()
        {
            //    var ediExt = new List<string>
            //    {
            //        "SalesPadEDI"
            //    };
            //    var ediConfig = new ExtAndCustom
            //    {
            //        ConfigurationName = "EDI",
            //        Extended = ediExt,
            //    };

            //    var aaExt = new List<string>
            //    {
            //        "AutomationAgent",
            //        "AutomationAgentService"
            //    };
            //    var aaConfig = new ExtAndCustom
            //    {
            //        ConfigurationName = "AA",
            //        Extended = aaExt,
            //    };

            //    var intExt = new List<string>
            //    {
            //        "AutomationAgent",
            //        "AutomationAgentService",
            //        "Integration",
            //        "Integration.Magento2",
            //        "Integration.Shopify"
            //    };
            //    var intConfig = new ExtAndCustom
            //    {
            //        ConfigurationName = "Integrations",
            //        Extended = intExt,
            //    };

            //    var spCore = new List<ExtAndCustom>
            //    {
            //        ediConfig,
            //        aaConfig,
            //        intConfig
            //    };

            //    var dcCore = new List<JustCustom>();
            //    var scCore = new List<JustCustom>();
            //    var webCore = new List<JustCustom>();
            //    var apiCore = new List<JustCustom>();

            //    var configurations = new Configurations
            //    {
            //        SalesPad = spCore,
            //        DataCollection = dcCore,
            //        ShipCenter = scCore,
            //        GPWeb = webCore,
            //        WebAPI = apiCore
            //    };

            //    string json = JsonConvert.SerializeObject(configurations, Formatting.Indented);

            //    SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Filter = "JSON |*.json";
            //    saveFileDialog.Title = "Save Core Modules File";
            //    saveFileDialog.ShowDialog();

            //    if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            //    {
            //        try
            //        {
            //            File.WriteAllText(saveFileDialog.FileName, json);
            //            string message = "The file was successfully saved.";
            //            string caption = "SUCCESS";
            //            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            //            MessageBox.Show(message, caption, buttons, icon);
            //        }
            //        catch (Exception e)
            //        {
            //            MessageBox.Show(String.Format("There was an error saving the file to {0}, error is as follows:\n\n{1}\n\n{2}", saveFileDialog.FileName, e.Message, e.ToString()));
            //        }
            //        return;
            //    }
        }

        //=======================================================[ ENCRYPTION ]========================================================
        static byte[] entropy = Encoding.Unicode.GetBytes("SaLtY bOy 6970 ePiC");

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        public static void ResizeListviewColumnWidth(ListView lv, int rowCount, int indx, int minW, int maxW)
        {
            int count = lv.Items.Count;
            if (count > rowCount)
                lv.Columns[indx].Width = minW;
            else
                lv.Columns[indx].Width = maxW;
        }
    }
}
