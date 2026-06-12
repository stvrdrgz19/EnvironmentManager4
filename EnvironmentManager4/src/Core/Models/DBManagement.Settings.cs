using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    public class DBManagement
    {
        public string DatabaseBackupDirectory { get; set; }
        public string Connection { get; set; }
        public List<ConnectionInfo> ConnectionsList { get; set; }
        public string SQLServerUserName { get; set; }
        public string SQLServerPassword { get; set; }
    }
}
