using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class RegistryEntries
    {
        public string _product;
        public const string _LaunchAfterInstall = "Launch After Install";
        public const string _OpenInstallFolder = "Open Install Folder";
        public const string _RunDatabaseUpdate = "Run Database Update";
        public const string _ResetDatabaseVersion = "Reset Database Version";

        public string LaunchAfterInstall
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue(_LaunchAfterInstall);
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue(_LaunchAfterInstall, value);
            }
        }

        public string OpenInstallFolder
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue(_OpenInstallFolder);
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue(_OpenInstallFolder, value);
            }
        }

        public string RunDatabaseUpdate
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue(_RunDatabaseUpdate);
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue(_RunDatabaseUpdate, value);
            }
        }

        public string ResetDatabaseVersion
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue(_ResetDatabaseVersion);
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue(_ResetDatabaseVersion, value);
            }
        }
    }
}
