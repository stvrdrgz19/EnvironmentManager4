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

        public static List<string> versionList = new List<string>
        {
            "x86",
            "x64",
            "Pre"
        };

        public static bool DevEnvironment()
        {
            if (Environment.CurrentDirectory == @"C:\Program Files (x86)\EnvMgr")
                return false;
            else
                return true;
        }

        public static string GetLogFile()
        {
            if (Environment.MachineName == "STEVERODRIGUEZ")
            {
                if (DevEnvironment())
                    return @"C:\Program Files (x86)\EnvMgr\Files\Log.txt";
                else
                    return Environment.CurrentDirectory + @"\Files\Log.txt";
            }
            else
                return Environment.CurrentDirectory + @"\Files\Log.txt";
        }

        public static void CheckForSettingsFile(string path)
        {
            if (!File.Exists(path))
            {
                CreateDefaultSettingsFile(path);
            }
        }

        public static string GetSettingsFile()
        {
            if (Environment.MachineName == "STEVERODRIGUEZ")
            {
                if (DevEnvironment())
                {
                    CheckForSettingsFile(@"C:\Program Files (x86)\EnvMgr\Files\Settings.json");
                    return @"C:\Program Files (x86)\EnvMgr\Files\Settings.json";
                }
                else
                {
                    CheckForSettingsFile(Environment.CurrentDirectory + @"\Files\Settings.json");
                    return Environment.CurrentDirectory + @"\Files\Settings.json";
                }
            }
            else
            {
                CheckForSettingsFile(Environment.CurrentDirectory + @"\Files\Settings.json");
                return Environment.CurrentDirectory + @"\Files\Settings.json";
            }
        }

        public static string GetInstallerFolder()
        {
            if (Environment.MachineName == "STEVERODRIGUEZ")
            {
                if (DevEnvironment())
                    return @"C:\Program Files (x86)\EnvMgr\Installers";
                else
                    return Environment.CurrentDirectory + @"\Installers";
            }
            else
                return Environment.CurrentDirectory + @"\Installers";
        }

        public static string GetDLLsFolder()
        {
            if (Environment.MachineName == "STEVERODRIGUEZ")
            {
                if (DevEnvironment())
                    return @"C:\Program Files (x86)\EnvMgr\Dlls";
                else
                    return Environment.CurrentDirectory + @"\Dlls";
            }
            else
                return Environment.CurrentDirectory + @"\Dlls";
        }

        public static string GetNotesFile()
        {
            if (Environment.MachineName == "STEVERODRIGUEZ")
            {
                if (DevEnvironment())
                    return @"C:\Program Files (x86)\EnvMgr\Files\Notes.txt";
                else
                    return Environment.CurrentDirectory + @"\Files\Notes.txt";
            }
            else
                return Environment.CurrentDirectory + @"\Files\Notes.txt";
        }

        public static string GetConfigurationsFile()
        {
            if (Environment.MachineName == "STEVERODRIGUEZ")
            {
                if (DevEnvironment())
                    return @"C:\Program Files (x86)\EnvMgr\Files\Configurations.json";
                else
                    return Environment.CurrentDirectory + @"\Files\Configurations.json";
            }
            else
                return Environment.CurrentDirectory + @"\Files\Configurations.json";
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

                    if (network.Name.Contains(networkName))
                    {
                        return address.Address.ToString();
                    }
                    ip = "NOT CONNECTED";
                }
            }
            return ip;
        }

        public static void CreateDefaultSettingsFile(string path)
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
            try
            {
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an issue creating the Settings file, error is as follows:\n\n{0}\n\n{1}",
                    e.Message,
                    e.ToString()));
            }

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "JSON |*.json";
            //saveFileDialog.Title = "Save Settings File";
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

        public static void ResizeListViewColumnWidth(ListView lv, int maxRowCount, int colIndx)
        {
            int count = lv.Items.Count;
            int maxW = lv.Columns[colIndx].Width;
            if (count > maxRowCount)
                lv.Columns[colIndx].Width = maxW-17;
        }

        public static void ResizeUpdateableListViewColumnWidth(ListView lv, int maxRowCount, int colIndx, int colDefaultWidth)
        {
            int count = lv.Items.Count;
            if (count > maxRowCount)
                lv.Columns[colIndx].Width = colDefaultWidth - 17;
            else
                lv.Columns[colIndx].Width = colDefaultWidth;
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

    public class Products
    {
        public string SalesPad { get; set; }
        public string DataCollection { get; set; }
        public string SalesPadMobile { get; set; }
        public string ShipCenter { get; set; }
        public string WebAPI { get; set; }
        public string GPWeb { get; set; }

        public static Products GetProductNames()
        {
            Products products = new Products();
            products.SalesPad = "SalesPad GP";
            products.DataCollection = "DataCollection";
            products.SalesPadMobile = "SalesPad Mobile";
            products.ShipCenter = "ShipCenter";
            products.WebAPI = "Customer Portal Web";
            products.GPWeb = "Customer Portal API";
            return products;
        }

        public static List<string> ListOfProducts()
        {
            List<string> productsList = new List<string>();
            Products products = GetProductNames();
            productsList.Add(products.SalesPad);
            productsList.Add(products.DataCollection);
            productsList.Add(products.SalesPadMobile);
            productsList.Add(products.ShipCenter);
            //productsList.Add(products.WebAPI);
            //productsList.Add(products.GPWeb);
            return productsList;
        }
    }

    public class ErrorHandling
    {
        public static void LogException(Exception e)
        {
            string logFile = Utilities.GetLogFile();
            DateTime logTime = DateTime.Now;
            if (!File.Exists(logFile))
            {
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Exception Message: {0}", e.Message));
                    sw.WriteLine(String.Format("Exception Type: {0}",e.GetType().ToString()));
                    sw.WriteLine(String.Format("Exception Source: {0}",e.Source));
                    sw.WriteLine(String.Format("Exception Target Site: {0}",e.TargetSite));
                    sw.WriteLine("");
                    sw.WriteLine("STACK TRACE");
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine("");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Exception Message: {0}", e.Message));
                    sw.WriteLine(String.Format("Exception Type: {0}",e.GetType().ToString()));
                    sw.WriteLine(String.Format("Exception Source: {0}", e.Source));
                    sw.WriteLine(String.Format("Exception Target Site: {0}", e.TargetSite));
                    sw.WriteLine("");
                    sw.WriteLine("STACK TRACE");
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine("");
                }
            }
        }

        public static void DisplayExceptionMessage(Exception e)
        {
            string exceptionMessage = String.Format("Exception Message: {0}\nException Type: {1}\nException Source: {2}\nException Traget Site: {3}\n\nSTACK TRACE\n{4}",
                e.Message,
                e.GetType().ToString(),
                e.Source,
                e.TargetSite,
                e.StackTrace);
            ExceptionForm form = new ExceptionForm();
            ExceptionForm.exceptionMessage = exceptionMessage;
            form.Show();
        }
    }
}
