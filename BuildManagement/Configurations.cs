using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace BuildManagement
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
            string configurationsFile = Utils.GetFile("Configurations.json");
            if (!File.Exists(configurationsFile))
                GenerateDefaultConfigurationsFile();
            return JsonConvert.DeserializeObject<List<Configurations>>(File.ReadAllText(configurationsFile));
        }

        /// <summary>
        /// This method can be used to update fields in the configurations file (eg. Product Name change)
        /// </summary>
        public static void UpdateConfigurationsFile()
        {
            List<Configurations> existingConfigurations = GetConfigurations();
            List<Configurations> newConfigurations = new List<Configurations>();
            foreach (Configurations configuration in existingConfigurations)
            {
                switch (configuration.Product)
                {
                    case "SalesPad GP":
                        configuration.Product = Products.SalesPad;
                        break;
                }
                newConfigurations.Add(configuration);
            }
            string json = JsonConvert.SerializeObject(newConfigurations.Distinct(), Formatting.Indented);
            File.WriteAllText(Utils.GetFile("Configurations.json"), json);
        }

        public static void GenerateDefaultConfigurationsFile()
        {
            Configurations ediConfiguration = new Configurations(Products.SalesPad,
                "EDI",
                new List<string> { "SalesPadEDI" },
                new List<string>());
            Configurations aaConfiguration = new Configurations(Products.SalesPad,
                "AA",
                new List<string> { "AutomationAgent", "AutomationAgentService" },
                new List<string>());

            List<Configurations> configurationList = new List<Configurations> { ediConfiguration, aaConfiguration };

            string json = JsonConvert.SerializeObject(configurationList, Formatting.Indented);
            File.WriteAllText(Utils.GetFile("Configurations.json"), json);
        }

        public static void GenerateConfigurationsFile(List<Configurations> configurations, bool append = true)
        {
            if (append)
                if (GetConfigurations().Count() != 0)
                    configurations.AddRange(GetConfigurations());

            string json = JsonConvert.SerializeObject(configurations.Distinct(), Formatting.Indented);
            File.WriteAllText(Utils.GetFile("Configurations.json"), json);
        }

        public static List<Configurations> GetConfigurationsByProduct(string product)
        {
            List<Configurations> productConfigurations = new List<Configurations>();
            List<Configurations> configurations = GetConfigurations();
            foreach (Configurations configuration in configurations)
                if (configuration.Product == product)
                    productConfigurations.Add(configuration);
            return productConfigurations;
        }

        public static void DeleteConfiguration(Configurations configurationToDelete)
        {
            List<Configurations> existingConfigurations = GetConfigurations();
            int i = 0;

            foreach (Configurations configuration in existingConfigurations.ToList())
            {
                if (configuration.Product == configurationToDelete.Product && configuration.ConfigurationName == configurationToDelete.ConfigurationName)
                {
                    existingConfigurations.RemoveAt(i);
                    break;
                }
                else
                {
                    i++;
                }
            }
            GenerateConfigurationsFile(existingConfigurations, false);
        }

        public static void SaveConfiguration(Configurations newConfiguration)
        {
            List<Configurations> existingConfigurations = GetConfigurations();
            foreach (Configurations existingConfiguration in existingConfigurations.ToList())
            {
                if (newConfiguration.Product == existingConfiguration.Product && newConfiguration.ConfigurationName == existingConfiguration.ConfigurationName)
                {
                    string message = String.Format("A Configuration named '{0}' already exists for the '{1}' product."
                        , newConfiguration.ConfigurationName
                        , newConfiguration.Product);
                    string caption = "DUPLICATE";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Exclamation;

                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }
                existingConfigurations.Add(newConfiguration);
            }
            GenerateConfigurationsFile(existingConfigurations, false);
        }
    }
}
