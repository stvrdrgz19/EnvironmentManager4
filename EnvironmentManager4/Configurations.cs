using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class Configurations
    {
        public string Product { get; set; }
        public string ConfigurationName { get; set; }
        public List<string> ExtendedModules { get; set; }
        public List<string> CustomModules { get; set; }

        public Configurations(string Product, string ConfigurationName, List<string> ExtendedModules, List<String> CustomModules)
        {
            this.Product = Product;
            this.ConfigurationName = ConfigurationName;
            this.ExtendedModules = ExtendedModules;
            this.CustomModules = CustomModules;
        }

        public static List<Configurations> GetConfigurations()
        {
            string configurationsFile = Utilities.GetFile("Configurations.json");
            if (!File.Exists(configurationsFile))
                GenerateDefaultConfigurationsFile();
            return JsonConvert.DeserializeObject<List<Configurations>>(File.ReadAllText(Utilities.GetFile("Configurations.json")));
        }

        public static void GenerateDefaultConfigurationsFile()
        {
            Configurations ediConfiguration = new Configurations("SalesPad GP",
                "EDI",
                new List<string> { "SalesPadEDI" },
                new List<string>());
            Configurations aaConfiguration = new Configurations("SalesPad GP",
                "AA",
                new List<string> { "AutomationAgent", "AutomationAgentService" },
                new List<string>());

            List<Configurations> configurationList = new List<Configurations> { ediConfiguration, aaConfiguration };

            string json = JsonConvert.SerializeObject(configurationList, Formatting.Indented);
            File.WriteAllText(Utilities.GetFile("Configurations.json"), json);
        }

        public static void GenerateConfigurationsFile(List<Configurations> configurations, bool append = true)
        {
            if (append)
                if (GetConfigurations().Count() != 0)
                    configurations.AddRange(GetConfigurations());

            string json = JsonConvert.SerializeObject(configurations.Distinct(), Formatting.Indented);
            File.WriteAllText(Utilities.GetFile("Configurations.json"), json);
        }

        public static List<Configurations> GetConfigurationsByProduct(string product)
        {
            List<Configurations> productConfigurations = new List<Configurations>();
            List<Configurations> configurations = GetConfigurations();
            foreach (Configurations configuration in configurations)
            {
                if (configuration.Product == product)
                    productConfigurations.Add(configuration);
            }
            return productConfigurations;
        }

        public static void DeleteConfiguration(Configurations configuration)
        {
            List<Configurations> configurations = GetConfigurations();
            int i = 0;

            foreach (Configurations config in configurations.ToList())
            {
                if (config.Product == configuration.Product && config.ConfigurationName == configuration.ConfigurationName)
                {
                    configurations.RemoveAt(i);
                    break;
                }
                else
                {
                    i++;
                }
            }

            GenerateConfigurationsFile(configurations, false);
        }

        public static void SaveConfiguration(Configurations configuration)
        {
            List<Configurations> configurations = GetConfigurations();
            foreach (Configurations config in configurations.ToList())
            {
                if (configuration.Product == config.Product && configuration.ConfigurationName == config.ConfigurationName)
                {
                    string message = String.Format("A Configuration named '{0}' already exists for the '{1}' product."
                        ,configuration.ConfigurationName
                        ,configuration.Product);
                    string caption = "DUPLICATE";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Exclamation;

                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }
                configurations.Add(configuration);
            }
            GenerateConfigurationsFile(configurations, false);
        }
    }
}
