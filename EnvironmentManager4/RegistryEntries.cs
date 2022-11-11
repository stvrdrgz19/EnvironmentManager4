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
        public string LaunchAfterInstall
        {
            get
            {
                RegistryKey key = RegUtilities.GetEnvMgrRegKey();
                return (string)key.GetValue("LaunchAfterInstall");
            }
            set
            {
                RegistryKey key = RegUtilities.GetEnvMgrRegKey();
                key.SetValue("LaunchAfterInstall", value);
            }
        }
    }
}
