using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.Database_Management
{
    public partial class DBUtils
    {
        public enum DBManagementType
        {
            Create,
            Restore,
            Overwrite
        }
    }
}
