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

        public string LaunchAfterInstall
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue("Launch After Install");
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue("Launch After Install", value);
            }
        }

        public string OpenInstallFolder
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue("Open Install Folder");
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue("Open Install Folder", value);
            }
        }

        public string RunDatabaseUpdate
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue("Run Database Update");
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue("Run Database Update", value);
            }
        }

        public string ResetDatabaseVersion
        {
            get
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                return (string)_key.GetValue("Reset Database Version");
            }
            set
            {
                RegistryKey _key = RegUtilities.GetInstallSubRegKey(_product);
                _key.SetValue("Reset Database Version", value);
            }
        }
    }
}
