using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    public static class Versions
    {
        public const string X64 = "x64";
        public const string X86 = "x86";
        public const string Pre = "Pre";

        public static IEnumerable<string> All
        {
            get
            {
                yield return X64;
                yield return X86;
                yield return Pre;
            }
        }
    }
}
