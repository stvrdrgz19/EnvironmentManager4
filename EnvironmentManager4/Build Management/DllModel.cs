using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class DllModel
    {
        public int Id { get; set; }
        public int Parent_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public string EntryDate { get; set; }

        public DllModel(int Parent_Id, string Name, string Type, string Version, string EntryDate)
        {
            this.Parent_Id = Parent_Id;
            this.Name = Name;
            this.Type = Type;
            this.Version = Version;
            this.EntryDate = EntryDate;
        }

        public DllModel(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }
}
