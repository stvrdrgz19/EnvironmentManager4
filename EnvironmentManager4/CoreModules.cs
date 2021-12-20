using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class CoreModules
    {
        public List<string> DLLName { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", string.Join(",", DLLName.ToArray()));
        }
    }
}
