using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildManagement
{
    public class Products
    {
        public const string SalesPad = "SalesPad";
        public const string DataCollection = "Inventory Manager";
        public const string SalesPadMobile = "SalesPad Mobile";
        public const string ShipCenter = "ShipCenter";
        public const string WebAPI = "Customer Portal API";
        public const string GPWeb = "Customer Portal Web";

        public static List<string> ListOfProducts()
        {
            List<string> productsList = new List<string>();
            productsList.Add(SalesPad);
            productsList.Add(DataCollection);
            productsList.Add(SalesPadMobile);
            productsList.Add(ShipCenter);
            //productsList.Add(WebAPI);
            //productsList.Add(GPWeb);
            return productsList;
        }
    }
}
