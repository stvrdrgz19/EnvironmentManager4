using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class SettingsModel
    {
        public int Version { get; set; }
        public DBManagement DbManagement { get; set; }
        public BuildManagement BuildManagement { get; set; }
        public Other Other { get; set; }
    }
}
