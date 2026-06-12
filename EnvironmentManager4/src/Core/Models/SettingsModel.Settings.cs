using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    public class SettingsModel
    {
        public int Version { get; set; }
        public DBManagement DBManagement { get; set; }
        public BuildManagement BuildManagement { get; set; }
        public Other Other { get; set; }
    }
}
