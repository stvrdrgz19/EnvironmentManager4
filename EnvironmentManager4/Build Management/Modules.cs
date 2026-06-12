using EnvironmentManager4.src.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace EnvironmentManager4
{
    public class Modules
    {
        public string ModuleName { get; set; }
        public string ModulePath { get; set; }
        public List<ModuleFileContents> ModuleContents { get; set; }

        public static string[] RetrieveDLLs(string modulePath, string buildPath, string product, string installer, string version)
        {
            List<string> dllList = new List<string>();
            switch (product)
            {
                case ProductsOld.SalesPad:
                    switch (version)
                    {
                        case "x64":
                        case "x86":
                            dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(16, Path.GetFileNameWithoutExtension(file).Substring(16).IndexOf(TrimVersion(buildPath, ProductsOld.SalesPad).ToString())).TrimEnd('.')));
                            break;
                        case "Pre":
                            dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(16, Path.GetFileNameWithoutExtension(file).Substring(16).IndexOf(TrimVersion(buildPath, ProductsOld.SalesPad).ToString())).TrimEnd('.')));
                            break;
                    }
                    break;
                case ProductsOld.DataCollection:
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(24, Path.GetFileName(file).Substring(24).IndexOf(".dll"))));
                    break;
                case ProductsOld.ShipCenter:
                    //dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Substring(20, Path.GetFileNameWithoutExtension(file).Substring(20).IndexOf(TrimVersion(buildPath, ProductsOld.ShipCenter).ToString())).TrimEnd('.')));
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file).Split('.')[2]));
                    break;
                case ProductsOld.GPWeb:
                    dllList.AddRange(Directory.GetFiles(modulePath).Select(file => Path.GetFileNameWithoutExtension(file)));
                    break;
                case ProductsOld.WebAPI:
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
                case ProductsOld.SalesPad:
                    charCount = 3;
                    break;
                case ProductsOld.ShipCenter:
                    charCount = 4;
                    break;
            }
            string spVersion = path.Substring(path.LastIndexOf('\\') + 1);
            if (pre)
                return spVersion;
            if (spVersion.Count(character => character == '.') == charCount)
                spVersion = spVersion.Substring(0, spVersion.LastIndexOf('.'));
            return spVersion;
        }

        public static string GetVersionNum(string installer)
        {
            string version = Path.GetFileNameWithoutExtension(installer).Substring(24);
            return version;
        }

        public static string TrimVersionAndExtension(string dll, string product)
        {
            if (product == ProductsOld.SalesPad)
            {
                string dllWithoutExt = dll.Substring(0, dll.LastIndexOf('.'));
                return dllWithoutExt.Substring(0, dllWithoutExt.LastIndexOf('.'));
            }
            else
                return dll.Substring(0, dll.LastIndexOf('.'));
        }

        public static void GetDLLs(string product, string installerPath, string version, string startTime, List<string> custDlls = null, List<string> extDlls = null)
        {
            string extPath = "";
            string custPath = "";
            string moduleStart = "";
            switch (product)
            {
                case ProductsOld.SalesPad:
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
                case ProductsOld.WebAPI:
                    extPath = String.Format(@"{0}\ExtModules\", installerPath);
                    custPath = String.Format(@"{0}\CustomModules\", installerPath);
                    moduleStart = "SalesPad.GP.RESTv3.";
                    break;
                case ProductsOld.DataCollection:
                    custPath = String.Format(@"{0}\CustomModules\", installerPath);
                    moduleStart = "SalesPad.DataCollection.";
                    break;
                case ProductsOld.SalesPadMobile:
                    break;
                case ProductsOld.ShipCenter:
                    custPath = String.Format(@"{0}\Custom\", installerPath);
                    moduleStart = "SalesPad.ShipCenter.";
                    break;
                case ProductsOld.GPWeb:
                    custPath = String.Format(@"{0}\Plugins\", installerPath);
                    break;
            }

            if (custDlls.Count > 0)
                CopyDllsFromBuild(custDlls, product, moduleStart, installerPath, custPath, startTime, "Custom DLL", version);

            if (extDlls.Count > 0)
                CopyDllsFromBuild(extDlls, product, moduleStart, installerPath, extPath, startTime, "Extended DLL", version);
        }

        public static void CopyDllsFromBuild(List<string> dllList, string product, string moduleStart, string installerPath, string custExtPath, string startTime, string type, string version = null)
        {
            string dllName = "";
            foreach (string dll in dllList.Distinct())
            {
                switch (product)
                {
                    case ProductsOld.SalesPad:
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
                    case ProductsOld.WebAPI:
                        dllName = String.Format("");
                        break;
                    case ProductsOld.DataCollection:
                        dllName = String.Format("{0}{1}.dll", moduleStart, dll);
                        break;
                    case ProductsOld.SalesPadMobile:
                        break;
                    case ProductsOld.ShipCenter:
                        dllName = String.Format("{0}{1}.{2}.Zip", moduleStart, dll, Modules.TrimVersion(installerPath, product));
                        break;
                    case ProductsOld.GPWeb:
                        dllName = String.Format("");
                        break;
                }
                string copyTo = "";
                string copyFrom = String.Format("{0}{1}", custExtPath, dllName);
                copyTo = String.Format(@"{0}\{1}", DirectoryUtilities.GetFolder("Dlls"), dllName);
                try
                {
                    File.Copy(copyFrom, copyTo, true);

                }
                catch (Exception e)
                {
                    ErrorHandling.DisplayExceptionMessage(e);
                    ErrorHandling.LogException(e);
                    return;
                }
                DllModel dllModel = new DllModel(SqliteDataAccess.GetLastParentId(), dllName, type, version, startTime);
                try
                {
                    SqliteDataAccess.SaveDlls(dllModel);
                }
                catch (Exception e)
                {
                    ErrorHandling.DisplayExceptionMessage(e);
                    ErrorHandling.LogException(e);
                }
            }
        }

        public static void UnzipDLLFiles()
        {
            string[] toExtract = Directory.GetFiles(DirectoryUtilities.GetFolder("Dlls"));
            foreach (string dll in toExtract)
            {
                string dllName = Path.GetFileNameWithoutExtension(dll);
                string dllTempFolder = DirectoryUtilities.GetFolder("Dlls") + @"\" + dllName;
                Directory.CreateDirectory(dllTempFolder);
                using (ZipArchive zip = ZipFile.Open(dll, ZipArchiveMode.Read))
                {
                    try
                    {
                        zip.ExtractToDirectory(dllTempFolder);
                    }
                    catch (Exception e)
                    {
                        ErrorHandling.DisplayExceptionMessage(e);
                        ErrorHandling.LogException(e);
                    }
                    zip.Dispose();
                }
                File.Delete(dll);
            }
        }

        public static void CopyDllsFromDirectoriesToInstalledBuild(string installPath)
        {
            foreach (string dir in Directory.GetDirectories(DirectoryUtilities.GetFolder("Dlls")))
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
                        ErrorHandling.DisplayExceptionMessage(e);
                        ErrorHandling.LogException(e);
                    }
                }
                Directory.Delete(dir, true);
            }
        }

        public static void CopyDllsToInstalledBuild(string installPath)
        {
            foreach (string file in Directory.GetFiles(DirectoryUtilities.GetFolder("Dlls")))
            {
                string fileName = Path.GetFileName(file);
                try
                {
                    File.Copy(file, String.Format(@"{0}\{1}", installPath, fileName), true);
                    File.Delete(file);
                }
                catch (Exception e)
                {
                    ErrorHandling.DisplayExceptionMessage(e);
                    ErrorHandling.LogException(e);
                }
            }
        }
    }

    public class ModuleFileContents
    {
        public string ModuleName { get; set; }
        public string FileName { get; set; }
    }
}
