using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class ConfigModel
    {
        //public List<Configurations> Configurations { get; set; }
        public string configurationName { get; set; }
        public List<string> extendedDLLs { get; set; }
        public List<string> customDLLs { get; set; }
    }

    //public class Configurations
    //{
    //    public List<ExtAndCustom> SalesPad { get; set; }
    //    public List<JustCustom> DataCollection { get; set; }
    //    public List<JustCustom> ShipCenter {get;set;}
    //    public List<JustCustom> GPWeb {get;set;}
    //    public List<JustCustom> WebAPI {get;set;}
    //}

    //public class ExtAndCustom
    //{
    //    public string ConfigurationName { get; set; }
    //    public List<string> Extended { get; set; }
    //    public List<string> Custom { get; set; }
    //}

    //public class JustCustom
    //{
    //    public string ConfigurationName { get; set; }
    //    public List<string> Custom { get; set; }
    //}
}
