using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    public class BuildManagement
    {
        public string SalesPadx86Directory { get; set; }
        public string SalesPadx64Directory { get; set; }
        public string DataCollectionDirectory { get; set; }
        public string SalesPadMobileDirectory { get; set; }
        public string ShipCenterx86Directory { get; set; }
        public string ShipCenterx64Directory { get; set; }
        public string GPWebDirectory { get; set; }
        public string WebAPIDirectory { get; set; }
    }
}
