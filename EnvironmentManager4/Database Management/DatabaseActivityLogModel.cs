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
        public string TimeStamp { get; set; }
        public string Action { get; set; }
        public string Backup { get; set; }

        public DatabaseActivityLogModel(string TimeStamp, string Action, string Backup)
        {
            this.TimeStamp = TimeStamp;
            this.Action = Action;
            this.Backup = Backup;
        }
    }
}
