using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class Modules
    {
        public static string[] RetrieveDLLs(string modulePath, string buildPath, string product, string installer)
        {
            List<string> dllList = new List<string>();
            switch (product)
            {
                case "SalesPad GP":
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(16, Path.GetFileNameWithoutExtension(file).Substring(16).IndexOf(TrimVersion(buildPath, "SalesPad").ToString())).TrimEnd('.')));
                    break;
                case "DataCollection":
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(24, Path.GetFileName(file).Substring(24).IndexOf(".dll"))));
                    break;
                case "ShipCenter":
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(20, Path.GetFileNameWithoutExtension(file).Substring(20).IndexOf(TrimVersion(buildPath, "ShipCenter").ToString())).TrimEnd('.')));
                    break;
                case "Customer Portal Web":
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file)));
                    break;
                case "Customer Portal API":
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file.Replace(GetVersionNum(installer), "")).Substring(19)));
                    break;
            }
            return dllList.ToArray();
        }

        public static string TrimVersion(string path, string product)
        {
            int charCount = 0;
            switch (product)
            {
                case "SalesPad GP":
                    charCount = 3;
                    break;
                case "ShipCenter":
                    charCount = 4;
                    break;
            }
            string spVersion = path.Substring(path.LastIndexOf('\\') + 1);
            if (spVersion.Count(character => character == '.') == charCount)
            {
                spVersion = spVersion.Substring(0, spVersion.LastIndexOf('.'));
            }
            return spVersion;
        }

        public static string GetVersionNum(string installer)
        {
            string version = Path.GetFileNameWithoutExtension(installer).Substring(24);
            return version;
        }
    }
}
