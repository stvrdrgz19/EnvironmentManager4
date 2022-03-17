using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class Configuration
    {
        public static List<string> GetConfigurationNames(string product)
        {
            List<string> configurationList = new List<string>();

            //TARGET THE INSTALL PATH CONFIGURATION FILE
            var json = File.ReadAllText(@"C:\Users\steve.rodriguez\Downloads\Configurations4.json");
            var obj = JObject.Parse(json);
            int configCount = 0;

            while (true)
            {
                try
                {
                    var configName = (string)obj[product][configCount]["ConfigurationName"];
                    configurationList.Add(configName);
                    configCount++;
                }
                catch
                {
                    break;
                }
            }
            return configurationList;
        }

        public static bool DoesConfigurationExist(string product, string configurationName)
        {
            List<string> configurationList = GetConfigurationNames(product);
            if (configurationList.Contains(configurationName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetConfigurationIndexByName(string configurationName)
        {
            int indx = 0;

            //TARGET THE INSTALL PATH CONFIGURATION FILE
            var json = File.ReadAllText(@"C:\Users\steve.rodriguez\Downloads\Configurations4.json");
            var obj = JObject.Parse(json);

            while (true)
            {
                try
                {
                    var configName = (string)obj["SalesPad"][indx]["ConfigurationName"];
                    if (configName == configurationName)
                    {
                        return indx;
                    }
                    indx++;
                }
                catch
                {
                    break;
                }
            }
            return indx;
        }

        public static List<string> GetConfigurationDLLs(string product, string configurationName, string customOrExt)
        {
            int indx = GetConfigurationIndexByName(configurationName);
            List<string> configurationDLLList = new List<string>();

            //TARGET THE INSTALL PATH CONFIGURATION FILE
            var json = File.ReadAllText(@"C:\Users\steve.rodriguez\Downloads\Configurations4.json");
            var obj = JObject.Parse(json);
            var configDLL = (string)obj[product][indx][customOrExt];

            return configurationDLLList;
        }
    }
}
