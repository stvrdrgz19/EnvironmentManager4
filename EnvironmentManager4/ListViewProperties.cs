using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class ListViewProperties
    {
        public ListView lv { get; set; }
        public int Width { get; set; }

        public static List<ListViewProperties> RetrieveListViewProperties(ListView lv)
        {
            List<ListViewProperties> lvProperties = new List<ListViewProperties>();

            foreach (ColumnHeader ch in lv.Columns)
            {
                ListViewProperties lvp = new ListViewProperties();
                lvp.lv = lv;
                lvp.Width = ch.Width;

                lvProperties.Add(lvp);
            }

            return lvProperties;
        }

        public static void UpdateListViewProperties(List<ListViewProperties> lvp)
        {
            int i = 0;
            foreach (ListViewProperties p in lvp)
            {
                p.lv.Columns[i].Width = p.Width;
                i++;
            }
        }
    }
}
