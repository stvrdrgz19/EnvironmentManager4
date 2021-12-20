using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class ConfigurationsModel
    {
        public SalesPadGP SalesPadGP { get; set; }
        public DataCollection DataCollection { get; set; }
        public ShipCenter ShipCenter { get; set; }
        public CustomerPortalWeb CustomerPortalWeb { get; set; }
        public CustomerPortalAPI CustomerPortalAPI { get; set; }
    }

    public class SalesPadGP
    {
        public ConfigurationsSP ConfigurationsSP { get; set; }
    }

    public class DataCollection
    {
        public Configurations Configurations { get; set; }
    }

    public class ShipCenter
    {
        public Configurations Configurations { get; set; }
    }

    public class CustomerPortalWeb
    {
        public Configurations Configurations { get; set; }
    }

    public class CustomerPortalAPI
    {
        public Configurations Configurations { get; set; }
    }

    public class ConfigurationsSP
    {
        public string ConfigurationName { get; set; }
        public List<string> Extended { get; set; }
        public List<string> CustomSP { get; set; }
    }

    public class Configurations
    {
        public string ConfigurationName { get; set; }
        public List<string> Custom { get; set; }
    }
}
