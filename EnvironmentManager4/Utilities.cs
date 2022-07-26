using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public static bool DevEnvironment()
        {
            if (Environment.CurrentDirectory == @"C:\Program Files (x86)\EnvMgr")
                return false;
            else
                return true;
        }

        public static string GetSettingsFile()
        {
            if (DevEnvironment())
                return @"C:\Program Files (x86)\EnvMgr\Files\Settings.json";
            else
                return Environment.CurrentDirectory + @"\Files\Settings.json";
        }

        public static string GetInstallerFolder()
        {
            if (DevEnvironment())
                return @"C:\Program Files (x86)\EnvMgr\Installers";
            else
                return Environment.CurrentDirectory + @"\Installers";
        }

        public static string GetDLLsFolder()
        {
            if (DevEnvironment())
                return @"C:\Program Files (x86)\EnvMgr\Dlls";
            else
                return Environment.CurrentDirectory + @"\Dlls";
        }

        public static string GetNotesFile()
        {
            if (DevEnvironment())
                return @"C:\Program Files (x86)\EnvMgr\Files\Notes.txt";
            else
                return Environment.CurrentDirectory + @"\Files\Notes.txt";
        }

        public static string GetConfigurationsFile()
        {
            if (DevEnvironment())
                return @"C:\Program Files (x86)\EnvMgr\Files\Configurations.json";
            else
                return Environment.CurrentDirectory + @"\Files\Configurations.json";
        }

        public static string RetrieveExe(string product, string installerPath = null, bool filter = false)
        {
            string executable = ProductInfo.GetProductInfo(product, null, installerPath).ProductExecutable;
            if (filter)
            {
                executable = String.Format("*{0}", executable);
            }
            return executable;
        }

        public static string GetIP(string networkName)
        {
            string ip = "";
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

                    if (network.Name == networkName)
                    {
                        return address.Address.ToString();
                    }
                    ip = "NOT CONNECTED";
                }
            }
            return ip;
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
            string mode = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile())).Other.Mode;
            string installDir = ProductInfo.GetProductInfo(product, version).InstallDirectory;
            if (mode != "Standard")
                return installDir.Substring(0, installDir.LastIndexOf('\\'));
            return installDir;
        }

        public static List<string> InstalledBuilds(string product, string version)
        {
            SettingsModel settings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            List<string> installedBuilds = new List<string>();
            try
            {
                if (settings.Other.Mode == "Standard")
                {
                    var buildList = Directory.GetFiles(GetProductInstallPath(product, version) + @"\", Utilities.RetrieveExe(product, "", true), SearchOption.AllDirectories);
                    installedBuilds.AddRange(buildList);
                }
                else
                {
                    var buildList = Directory.GetDirectories(GetProductInstallPath(product, version), "SalesPad.Desktop*");
                    foreach (string buildPath in buildList)
                    {
                        var file = Directory.GetFiles(buildPath, Utilities.RetrieveExe(product, "", true), SearchOption.AllDirectories);
                        foreach (string f in file)
                        {
                            installedBuilds.Add(f);
                        }
                    }
                }
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
            List<Connection> connectionList = new List<Connection>();

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
                Mode = "Standard",
                DefaultVersion = "x64",
                ShowAlwaysOnTop = true,
                ShowVPNIP = true,
                ShowIP = true
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

        public static void ResizeListviewColumnWidth(ListView lv, int rowCount, int indx, int minW, int maxW)
        {
            int count = lv.Items.Count;
            if (count > rowCount)
                lv.Columns[indx].Width = minW;
            else
                lv.Columns[indx].Width = maxW;
        }

        public static int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i + 1;
                    }
                }
            }
            return -1;
        }

        public static string GetDatabaseDescription(string backupName)
        {
            SettingsModel settings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(GetSettingsFile()));
            string zipPath = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, backupName);

            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry description = archive.GetEntry("Description.txt");
                    using (StreamReader reader = new StreamReader(description.Open()))
                        return reader.ReadToEnd();
                }
            }
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
    }
}
