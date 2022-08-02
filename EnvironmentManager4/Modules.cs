using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class Modules
    {
        public static string[] RetrieveDLLs(string modulePath, string buildPath, string product, string installer, string version)
        {
            List<string> dllList = new List<string>();
            switch (product)
            {
                case "SalesPad GP":
                    switch (version)
                    {
                        case "x64":
                        case "x86":
                            dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(16, Path.GetFileNameWithoutExtension(file).Substring(16).IndexOf(TrimVersion(buildPath, "SalesPad GP").ToString())).TrimEnd('.')));
                            break;
                        case "Pre":
                            dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(16, Path.GetFileNameWithoutExtension(file).Substring(16).IndexOf(TrimVersion(buildPath, "SalesPad GP").ToString())).TrimEnd('.')));
                            break;
                    }
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

        public static string TrimVersion(string path, string product, bool pre = false)
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
            if (pre)
                return spVersion;
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

        public static void GetDLLs(string product, string installerPath, string version, string startTime, List<string> custDlls = null, List<string> extDlls = null)
        {
            string extPath = "";
            string custPath = "";
            string moduleStart = "";
            switch (product)
            {
                case "SalesPad GP":
                    switch (version)
                    {
                        case "x64":
                        case "x86":
                            extPath = String.Format(@"{0}\ExtModules\{1}\", installerPath, version);
                            custPath = String.Format(@"{0}\CustomModules\{1}\", installerPath, version);
                            break;
                        case "Pre":
                            extPath = String.Format(@"{0}\ExtModules\WithOutCardControl\", installerPath);
                            custPath = String.Format(@"{0}\CustomModules\WithOutCardControl\", installerPath);
                            break;
                    }
                    moduleStart = "SalesPad.Module.";
                    break;
                case "Customer Portal API":
                    extPath = String.Format(@"{0}\ExtModules", installerPath);
                    custPath = String.Format(@"{0}\CustomModules", installerPath);
                    moduleStart = "SalesPad.GP.RESTv3.";
                    break;
                case "DataCollection":
                    custPath = String.Format(@"{0}\CustomModules", installerPath);
                    moduleStart = "SalesPad.DataCollection.";
                    break;
                case "SalesPad Mobile":
                    break;
                case "ShipCenter":
                    custPath = String.Format(@"{0}\Custom", installerPath);
                    moduleStart = "SalesPad.ShipCenter.";
                    break;
                case "Customer Portal Web":
                    custPath = String.Format(@"{0}\Plugins", installerPath);
                    break;
            }

            if (custDlls.Count > 0)
            {
                CopyDllsFromBuild(custDlls, product, moduleStart, installerPath, custPath, startTime, "Custom DLL", version);
            }

            if (extDlls.Count > 0)
            {
                CopyDllsFromBuild(extDlls, product, moduleStart, installerPath, extPath, startTime, "Extended DLL", version);
            }
        }

        public static void CopyDllsFromBuild(List<string> dllList, string product, string moduleStart, string installerPath, string custExtPath, string startTime, string type, string version = null)
        {
            string dllName = "";
            foreach (string dll in dllList.Distinct())
            {
                switch (product)
                {
                    case "SalesPad GP":
                        switch (version)
                        {
                            case "x64":
                            case "x86":
                                dllName = String.Format("{0}{1}.{2}.{3}.Zip", moduleStart, dll, Modules.TrimVersion(installerPath, product), version.ToUpper());
                                break;
                            case "Pre":
                                dllName = String.Format("{0}{1}.{2}.Zip", moduleStart, dll, Modules.TrimVersion(installerPath, product, true));
                                break;
                        }
                        break;
                    case "Customer Portal API":
                        dllName = String.Format("");
                        break;
                    case "DataCollection":
                        dllName = String.Format("{0}{1}.dll", moduleStart, dll);
                        break;
                    case "SalesPad Mobile":
                        break;
                    case "ShipCenter":
                        dllName = String.Format("{0}{1}.{2}.Zip", moduleStart, dll, Modules.TrimVersion(installerPath, product));
                        break;
                    case "Customer Portal Web":
                        dllName = String.Format("");
                        break;
                }
                string copyFrom = String.Format("{0}{1}", custExtPath, dllName);
                string copyTo = String.Format(@"{0}\{1}{2}", Utilities.GetDLLsFolder(), moduleStart, dllName);
                try
                {
                    File.Copy(copyFrom, copyTo, true);

                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error copying '{0}' from {1} to {2}, error is as follows:\n\n{3}\n\n{4}", dll, copyFrom, copyTo, e.Message, e.ToString()));
                    return;
                }
                DllModel dllModel = new DllModel(SqliteDataAccess.GetLastParentId(), dllName, type, version, startTime);
                try
                {
                    SqliteDataAccess.SaveDlls(dllModel);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error logging the dll {0} to the sqlite database, error is as follows:\n\n{1}\n\n{2}", dllName, e.Message, e.ToString()));
                }
            }
        }

        public static void UnzipDLLFiles()
        {
            string[] toExtract = Directory.GetFiles(Utilities.GetDLLsFolder());
            foreach (string dll in toExtract)
            {
                string dllName = Path.GetFileNameWithoutExtension(dll);
                string dllTempFolder = Utilities.GetDLLsFolder() + @"\" + dllName;
                Directory.CreateDirectory(dllTempFolder);
                using (ZipArchive zip = ZipFile.Open(dll, ZipArchiveMode.Read))
                {
                    try
                    {
                        zip.ExtractToDirectory(dllTempFolder);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(String.Format("An error was encountered while trying to extract {0}. Exception message is as follows:\n\n{1}", dllName, e.Message));
                    }
                    zip.Dispose();
                }
                File.Delete(dll);
            }
        }

        public static void CopyDllsFromDirectoriesToInstalledBuild(string installPath)
        {
            foreach (string dir in Directory.GetDirectories(Utilities.GetDLLsFolder()))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    string fileName = Path.GetFileName(file);
                    try
                    {
                        File.Copy(file, String.Format(@"{0}\{1}", installPath, fileName), true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(String.Format("An error was encountered while trying to copy {0} to {1}. Exception message is as follows:\n\n{2}", fileName, installPath, e.Message));
                    }
                }
                Directory.Delete(dir, true);
            }
        }

        public static void CopyDllsToInstalledBuild(string installPath)
        {
            foreach (string file in Directory.GetFiles(Utilities.GetDLLsFolder()))
            {
                string fileName = Path.GetFileName(file);
                try
                {
                    File.Copy(file, String.Format(@"{0}\{1}", installPath, fileName), true);
                    File.Delete(file);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("An error was encountered while trying to copy {0} to {1}. Exception message is as follows:\n\n{2}", fileName, installPath, e.Message));
                }
            }
        }
    }
}
