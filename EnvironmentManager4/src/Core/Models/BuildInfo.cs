using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    internal class BuildInfo
    {
        public string FileServerDirectory { get; set; }
        public string InstallDirectory { get; set; }
        public List<string> ProductExecutables { get; set; }
        public List<string> DirectoryFilters { get; set; }
        public string ModulePrefix { get; set; }
    }
}
