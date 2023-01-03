using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                //string path = String.Format(@"{0}\{1}\{2}\{3}.zip", buildPath, type, version, dll);
                string path = String.Format(@"{0}\{1}\{2}", buildPath, type, version);
                string[] files = Directory.GetFiles(path, String.Format("{0}{1}.*", pi.ModuleNaming, dll));

                foreach (string file in files)
                {
                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                            fileList.Add(entry.Name);
                    }
                }

                dllConfig.CoreDLL = dll;
                dllConfig.Files = fileList;
                dllList.Add(dllConfig);
            }
            return dllList;
        }
    }

    public class DLLFileModel
    {
        public string CoreDLL { get; set; }
        public List<string> Files { get; set; }
    }
}
