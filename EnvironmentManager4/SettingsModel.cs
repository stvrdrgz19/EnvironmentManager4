using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class SettingsModel
    {
        public DbManagement DbManagement { get; set; }
        public BuildManagement BuildManagement { get; set; }
        public Other Other { get; set; }
    }

    public class DbManagement
    {
        public string DatabaseBackupDirectory { get; set; }
        public string Connection { get; set; }
        public List<Connection> ConnectionsList { get; set; }
        public string SQLServerUserName { get; set; }
        public string SQLServerPassword { get; set; }
        public List<string> Databases { get; set; }
        public bool Connected { get; set; }
    }

    public class BuildManagement
    {
        public string SalesPadx86Directory { get; set; }
        public string SalesPadx64Directory { get; set; }
        public string DataCollectionDirectory { get; set; }
        public string SalesPadMobileDirectory { get; set; }
        public string ShipCenterDirectory { get; set; }
        public string GPWebDirectory { get; set; }
        public string WebAPIDirectory { get; set; }
    }

    public class Other
    {
        public string Mode { get; set; }
        public string DefaultVersion { get; set; }
        public bool ShowAlwaysOnTop { get; set; }
        public bool ShowVPNIP { get; set; }
        public bool ShowIP { get; set; }
    }

    public class Connection
    {
        public string ConnectionName { get; set; }
        public string ConnectionUN { get; set; }
        public string ConnectionPW { get; set; }
    }
}
