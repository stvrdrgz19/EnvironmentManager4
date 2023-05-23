using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class DBManagement
    {
        public string DatabaseBackupDirectory { get; set; }
        public string Connection { get; set; }
        public List<Connection> ConnectionsList { get; set; }
        public string SQLServerUserName { get; set; }
        public string SQLServerPassword { get; set; }
        public bool ResetDatabaseAfterRestore { get; set; }
        public string DBToRestore { get; set; }
    }

    public class Connection
    {
        public string ConnectionName { get; set; }
        public string ConnectionUN { get; set; }
        public string ConnectionPW { get; set; }
    }
}
