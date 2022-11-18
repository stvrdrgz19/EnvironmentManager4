using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class ListViewModel
    {
        public List<SQLServiceStatus> ServiceList { get; set; }
        public bool HasResized { get; set; }
    }

    public class SQLServiceStatus
    {
        public string ServiceName { get; set; }
        public string ServiceStatus { get; set; }
    }
}
