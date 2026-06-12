using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    public static class Products
    {
        public const string SalesPad = "SalesPad";
        public const string InventoryControl = "Inventory Manager";
        public const string SalesPadMobile = "SalesPad Mobile";
        public const string ShipCenter = "ShipCenter";

        public static IEnumerable<string> All
        {
            get
            {
                yield return SalesPad;
                yield return InventoryControl;
                yield return SalesPadMobile;
                yield return ShipCenter;
            }
        }
    }
}
