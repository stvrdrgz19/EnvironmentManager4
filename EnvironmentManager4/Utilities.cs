using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
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
            if (Environment.CurrentDirectory == @"C:\Users\steve.rodriguez\source\repos\EnvironmentManager4\EnvironmentManager4\bin\Debug")
                return true;
            else
                return false;
        }

        public static string GetFile(string fileName)
        {
            if (DevEnvironment())
                return String.Format(@"{0}\Files\{1}", @"C:\Program Files (x86)\Environment Manager", fileName);
            else
                return String.Format(@"{0}\Files\{1}", Environment.CurrentDirectory, fileName);
        }

        public static string GetFolder(string folderName)
        {
            if (DevEnvironment())
                return String.Format(@"{0}\{1}", @"C:\Program Files (x86)\Environment Manager", folderName);
            else
                return String.Format(@"{0}\{1}", Environment.CurrentDirectory, folderName);
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

        public static void ResizeListViewColumnWidthForScrollBar(ListView lv, int maxRowCount, int colIndx)
        {
            int count = lv.Items.Count;
            int maxW = lv.Columns[colIndx].Width;
            if (count > maxRowCount)
            {
                lv.Columns[colIndx].Width = maxW - 17;
            }
        }

        public static void ResizeUpdateableListViewColumnWidthForScrollBar(ListView lv, int maxRowCount, int colIndx, int colDefaultWidth)
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
            SettingsModel settings = SettingsUtilities.GetSettings();
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

        public static string GetAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetLatestVersion()
        {
            var directory = @"\\sp-fileserv-01\Team QA\Tools\Environment Manager\Installers";
            string pattern = "*.msi";
            string version;
            try
            {
                var dirInfo = new DirectoryInfo(directory);
                var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

                version = file.ToString().Substring(21, file.ToString().Length - 25);
            }
            catch (Exception e)
            {
                string extraMessage = "Possibly not connected to the SalesPad Network or VPN.";
                ErrorHandling.DisplayExceptionMessage(e, extraMessage);
                ErrorHandling.LogException(e, extraMessage);
                version = "Unable to Connect";
            }
            return version;
        }

        public static bool IsProgramUpToDate()
        {
            string latestVersion = GetLatestVersion();
            if (GetAppVersion() == latestVersion || latestVersion == "Unable to Connect")
                return true;
            else
                return false;
        }

        public static string GetUpdateFile()
        {
            return @"\\sp-fileserv-01\Team QA\Tools\Environment Manager\Utility Scripts\GetLatestEnvironmentManagerAdmin.bat.lnk";
        }

        public static string GetProjectLink()
        {
            return "https://github.com/stvrdrgz19/EnvironmentManager4/projects/1";
        }

        public static string GetWikiLink()
        {
            return "https://github.com/stvrdrgz19/EnvironmentManager4/wiki";
        }

        public static string GetRepoLink()
        {
            return "https://github.com/stvrdrgz19/EnvironmentManager4";
        }

        public static string GetChangeLogLink()
        {
            return "https://github.com/stvrdrgz19/EnvironmentManager4/wiki/Change-Log";
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

        public static string[] GetFilesFromDirectoryByExtension(string path, string extension = "zip")
        {
            IEnumerable<string> results =
                from x in Directory.GetFiles(path)
                where Path.GetFileName(x).Contains(extension)
                select Path.GetFileNameWithoutExtension(x);

            return results.ToArray();
        }

        public static string[] GetFilesFromDirectoryByExtensions(string path, string[] extensions)
        {
            List<string> results = new List<string>();
            foreach (string extension in extensions)
            {
                results.AddRange(GetFilesFromDirectoryByExtension(path, extension));
            }
            return results.ToArray();
        }
    }

    public class Products
    {
        public const string SalesPad = "SalesPad";
        public const string DataCollection = "Inventory Manager";
        public const string SalesPadMobile = "SalesPad Mobile";
        public const string ShipCenter = "ShipCenter";
        public const string WebAPI = "Customer Portal API";
        public const string GPWeb = "Customer Portal Web";

        public static List<string> ListOfProducts()
        {
            List<string> productsList = new List<string>();
            productsList.Add(SalesPad);
            productsList.Add(DataCollection);
            productsList.Add(SalesPadMobile);
            productsList.Add(ShipCenter);
            //productsList.Add(WebAPI);
            //productsList.Add(GPWeb);
            return productsList;
        }
    }

    public class ErrorHandling
    {
        public static void LogException(Exception e, string extraMessage = null)
        {
            string logFile = Utilities.GetFile("Log.txt");
            DateTime logTime = DateTime.Now;
            if (!File.Exists(logFile))
            {
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine(String.Format("-({0})-------------------------------------------------", logTime));
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utilities.GetAppVersion()));
                    sw.WriteLine(String.Format("Exception Message: {0}", e.Message));
                    sw.WriteLine(String.Format("Exception Type: {0}",e.GetType().ToString()));
                    sw.WriteLine(String.Format("Exception Source: {0}",e.Source));
                    sw.WriteLine(String.Format("Exception Target Site: {0}",e.TargetSite));
                    sw.WriteLine("");
                    if (!String.IsNullOrEmpty(extraMessage))
                    {
                        sw.WriteLine(extraMessage);
                        sw.WriteLine("");
                    }
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
                    sw.WriteLine(String.Format("Environment Manager v{0}", Utilities.GetAppVersion()));
                    sw.WriteLine(String.Format("Exception Message: {0}", e.Message));
                    sw.WriteLine(String.Format("Exception Type: {0}",e.GetType().ToString()));
                    sw.WriteLine(String.Format("Exception Source: {0}", e.Source));
                    sw.WriteLine(String.Format("Exception Target Site: {0}", e.TargetSite));
                    sw.WriteLine("");
                    if (!String.IsNullOrEmpty(extraMessage))
                    {
                        sw.WriteLine(extraMessage);
                        sw.WriteLine("");
                    }
                    sw.WriteLine("STACK TRACE");
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine("");
                }
            }
        }

        public static void DisplayExceptionMessage(Exception e, string extraMessage = null)
        {
            ExceptionForm form = new ExceptionForm();
            ExceptionForm.exception = e;
            ExceptionForm.extraMessage = extraMessage;
            form.ShowDialog();
        }
    }

    public class RegUtilities
    {
        public static RegistryKey GetInstallSubRegKey(string product)
        {
            return Registry.CurrentUser.OpenSubKey(String.Format(@"Software\Environment Manager\Install\{0}", product), true);
        }

        public static void CheckForInstallRegistryEntries()
        {
            RegistryKey spKey = Registry.CurrentUser.OpenSubKey(String.Format(@"Software\Environment Manager\Install\{0}", Products.SalesPad));
            if (spKey == null)
                GenerateInstallOptionEntries();
        }

        public static void GenerateInstallOptionEntries()
        {
            foreach (string product in Products.ListOfProducts())
            {
                Registry.CurrentUser.CreateSubKey(String.Format(@"Software\Environment Manager\Install\{0}", product));
                RegistryKey key = GetInstallSubRegKey(product);
                key.SetValue(RegistryEntries._LaunchAfterInstall, "false");
                key.SetValue(RegistryEntries._OpenInstallFolder, "false");
                key.SetValue(RegistryEntries._RunDatabaseUpdate, "false");
                key.SetValue(RegistryEntries._ResetDatabaseVersion, "false");
                key.Close();
            }
        }
    }
}
