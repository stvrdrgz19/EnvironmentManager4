using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class Variance
    {
        public string PropertyName { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }

    public static class Comparision
    {
        public static List<Variance> Compare<T>(this T val1, T val2)
        {
            var variances = new List<Variance>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name != "SQLServerPassword" && property.Name != "Databases")
                {
                    var v = new Variance
                    {
                        PropertyName = property.Name,
                        valA = property.GetValue(val1),
                        valB = property.GetValue(val2)
                    };
                    if (v.valA == null && v.valB == null)
                    {
                        continue;
                    }
                    if (
                        (v.valA == null && v.valB != null)
                        ||
                        (v.valA != null && v.valB == null)
                    )
                    {
                        variances.Add(v);
                        continue;
                    }
                    if (!v.valA.Equals(v.valB))
                    {
                        variances.Add(v);
                    }
                }
            }
            return variances;
        }
    }
}
