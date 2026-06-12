using EnvironmentManager4.src.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core
{
    internal class InstalledBuilds
    {
        public string Path { get; set; }
        public string ModifiedDate { get; set; }

        public List<InstalledBuilds> RetrieveBuilds(string product, string version)
        {
            switch (product)
            {
                case Products.SalesPad:
                    if (version == Versions.X64)
                    {
                        return new List<InstalledBuilds>
                        {
                            new InstalledBuilds { Path = @"C:\Program Files\SalesPad\7.5.0", ModifiedDate = "2026-01-15" },
                            new InstalledBuilds { Path = @"C:\Program Files\SalesPad\7.4.2", ModifiedDate = "2025-12-10" },
                            new InstalledBuilds { Path = @"C:\Program Files\SalesPad\7.4.0", ModifiedDate = "2025-11-20" }
                        };
                    }
                    else if (version == Versions.X86 || version == Versions.Pre)
                    {
                        return new List<InstalledBuilds>
                        {
                            new InstalledBuilds { Path = @"C:\Program Files (x86)\SalesPad\7.3.5", ModifiedDate = "2025-10-05" },
                            new InstalledBuilds { Path = @"C:\Program Files (x86)\SalesPad\7.3.0", ModifiedDate = "2025-09-12" }
                        };
                    }
                    return new List<InstalledBuilds>();
                case Products.InventoryControl:
                    return new List<InstalledBuilds>
                    {
                        new InstalledBuilds { Path = @"C:\Program Files (x86)\InventoryControl\3.1.0", ModifiedDate = "2025-11-15" },
                        new InstalledBuilds { Path = @"C:\Program Files (x86)\InventoryControl\3.0.8", ModifiedDate = "2025-10-30" }
                    };
                case Products.SalesPadMobile:
                    return new List<InstalledBuilds>
                    {
                        new InstalledBuilds { Path = @"C:\Program Files (x86)\SalesPadMobile\2.6.8", ModifiedDate = "2025-10-20" },
                        new InstalledBuilds { Path = @"C:\Program Files (x86)\SalesPadMobile\2.6.5", ModifiedDate = "2025-09-25" }
                    };
                case Products.ShipCenter:
                    if (version == Versions.X64)
                    {
                        return new List<InstalledBuilds>
                        {
                            new InstalledBuilds { Path = @"C:\Program Files\ShipCenter\5.1.2", ModifiedDate = "2026-02-05" },
                            new InstalledBuilds { Path = @"C:\Program Files\ShipCenter\5.1.0", ModifiedDate = "2026-01-12" },
                            new InstalledBuilds { Path = @"C:\Program Files\ShipCenter\5.0.8", ModifiedDate = "2025-12-05" }
                        };
                    }
                    else if (version == Versions.X86 || version == Versions.Pre)
                    {
                        return new List<InstalledBuilds>
                        {
                            new InstalledBuilds { Path = @"C:\Program Files (x86)\ShipCenter\5.0.5", ModifiedDate = "2025-11-10" },
                            new InstalledBuilds { Path = @"C:\Program Files (x86)\ShipCenter\5.0.0", ModifiedDate = "2025-10-15" }
                        };
                    }
                    return new List<InstalledBuilds>();
            }
            return new List<InstalledBuilds>();
        }
    }
}
