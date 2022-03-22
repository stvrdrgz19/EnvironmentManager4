using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class Configuration
    {
        public static List<string> GetConfigurationsNames(string product)
        {
            List<string> configurations = new List<string>();
            string path = Utilities.GetConfigurationsFiles(product);
            foreach (string file in Directory.GetFiles(path))
            {
                configurations.Add(Path.GetFileNameWithoutExtension(file));
            }
            return configurations;
        }

        public static bool DoesConfigurationExist(string product, string configName)
        {
            string path = Utilities.GetConfigurationsFiles(product);
            if (File.Exists(String.Format(@"{0}\{1}.json", path, configName)))
            {
                return true;
            }
            return false;
        }

        public static void CreateConfiguration(string product, string configurationName, List<string> extendedDlls, List<string> customDlls)
        {
            var config = new ConfigModel
            {
                configurationName = configurationName,
                extendedDLLs = extendedDlls,
                customDLLs = customDlls
            };

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(String.Format(@"{0}\{1}\{2}.json", Utilities.GetConfigurationDirectory(), product, configurationName), json);
        }

        public static void DeleteConfiguration(string product, string configurationName)
        {
            try
            {
                File.Delete(String.Format(@"{0}\{1}\{2}.json", Utilities.GetConfigurationDirectory(), product, configurationName));
            }
            catch
            {
                MessageBox.Show(String.Format("There was an issue deleting the '{0}' configuration for the '{1}' product.", configurationName, product));
                return;
            }
        }
    }
}
