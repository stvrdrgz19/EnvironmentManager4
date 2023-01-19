﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace EnvironmentManager4
{
    public class InstallProperties
    {
        public string Product { get; set; }
        public string Version { get; set; }
        public List<DLLFileModel> CustomDLLs { get; set; }
        public List<DLLFileModel> ExtendedDLLs { get; set; }
        public string BuildPath { get; set; }
        public string InstallPath { get; set; }

        public static void WritePropertiesFile(InstallProperties ip)
        {
            string json = JsonConvert.SerializeObject(ip, Formatting.Indented);
            File.WriteAllText(String.Format(@"{0}\InstallProperties.envprop", ip.InstallPath), json);
        }

        public static List<DLLFileModel> GetDLLList(List<string> rawDllList, string buildPath, bool cust, string product, string version)
        {
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            List<DLLFileModel> dllList = new List<DLLFileModel>();
            string type = cust ? "CustomModules" : "ExtModules";
            foreach (string dll in rawDllList)
            {
                DLLFileModel dllConfig = new DLLFileModel();
                List<string> fileList = new List<string>();

                string path = "";
                switch (product)
                {
                    case Products.SalesPad:
                        path = String.Format(@"{0}\{1}\{2}", buildPath, type, version);
                        break;
                    case Products.DataCollection:
                        path = String.Format(@"{0}\{1}", buildPath, type);
                        break;
                    case Products.SalesPadMobile:
                        path = "";
                        break;
                    case Products.ShipCenter:
                        path = String.Format(@"{0}\Custom", buildPath);
                        break;
                    case Products.WebAPI:
                        path = String.Format(@"{0}\{1}", buildPath, type);
                        break;
                    case Products.GPWeb:
                        path = String.Format(@"{0}\plugins", buildPath);
                        break;
                }

                string[] files = Directory.GetFiles(path, String.Format("{0}{1}.*", pi.ModuleNaming, dll));

                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (extension == ".Zip")
                        using (ZipArchive archive = ZipFile.OpenRead(file))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                                fileList.Add(entry.Name);
                        }
                }

                dllConfig.CoreDLL = ConvertDLLNameToFile(dll, product, version);
                dllConfig.Files = fileList;
                dllList.Add(dllConfig);
            }
            return dllList;
        }

        public static string ConvertDLLNameToFile(string input, string product, string version)
        {
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            return String.Format("{0}{1}.dll", pi.ModuleNaming, input);
        }

        public static bool DoesInstallHaveProperties(string path)
        {
            return File.Exists(String.Format(@"{0}\InstallProperties.envprop", path)) ? true : false;
        }

        public static InstallProperties RetrieveInstallProperties(string path)
        {
            return JsonConvert.DeserializeObject<InstallProperties>(File.ReadAllText(String.Format(@"{0}\InstallProperties.envprop", path))); ;
        }

        public static List<DLLFileModel> RetrieveInstalledDLLsFromProperties(string path, bool custom)
        {
            InstallProperties ip = JsonConvert.DeserializeObject<InstallProperties>(File.ReadAllText(String.Format(@"{0}\InstallProperties.envprop", path)));
            return custom ? ip.CustomDLLs : ip.ExtendedDLLs;
        }
    }

    public class DLLFileModel
    {
        public string CoreDLL { get; set; }
        public List<string> Files { get; set; }
    }
}
