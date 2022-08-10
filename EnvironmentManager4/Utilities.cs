﻿using Newtonsoft.Json;
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
            if (Environment.CurrentDirectory == @"C:\Users\steve.rodriguez\source\repos\EnvironmentManager4\EnvironmentManager4\bin\Debug")
                return true;
            else
                return false;
        }

        public static string GetFile(string fileName)
        {
            if (DevEnvironment())
                return String.Format(@"{0}\Files\{1}", @"C:\Program Files (x86)\EnvMgr", fileName);
            else
                return String.Format(@"{0}\Files\{1}", Environment.CurrentDirectory, fileName);
        }

        public static string GetFolder(string folderName)
        {
            if (DevEnvironment())
                return String.Format(@"{0}\{1}", @"C:\Program Files (x86)\EnvMgr", folderName);
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
            string logFile = Utilities.GetFile("Log.txt");
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
            ExceptionForm form = new ExceptionForm();
            ExceptionForm.exception = e;
            form.Show();
        }
    }
}
