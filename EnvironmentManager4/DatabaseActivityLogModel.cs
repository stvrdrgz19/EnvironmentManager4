using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class DatabaseActivityLogModel
    {
        public int Id { get; set; }
        public string Created_On { get; set; }
        public string Action { get; set; }
        public string Backup { get; set; }

        public DatabaseActivityLogModel(string Created_On, string Action, string Backup)
        {
            this.Created_On = Created_On;
            this.Action = Action;
            this.Backup = Backup;
        }
    }
}
