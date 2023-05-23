using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace BuildManagement
{
    public class Builds
    {
        public string InstallPath { get; set; }
        public string Exe { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Builds(string path, string exe, DateTime modifiedDate)
        {
            this.InstallPath = path;
            this.Exe = exe;
            this.ModifiedDate = modifiedDate;
        }

        public static List<Builds> GetInstalledBuilds(string product, string version)
        {
            List<Builds> builds = new List<Builds>();
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            foreach (string exe in pi.ProductExecutables)
            {
                List<string> paths = new List<string>();
                foreach (string filter in pi.DirectoryFilters)
                {
                    string[] dirs = Directory.GetDirectories(pi.InstallDirectory, String.Format("{0}*", filter));
                    foreach (string dir in dirs)
                    {
                        paths.AddRange(Directory.GetFiles(dir,
                            String.Format("*{0}", exe),
                            SearchOption.AllDirectories));
                    }

                    foreach (string path in paths)
                    {
                        bool contains = builds.Any(p => p.InstallPath == Path.GetDirectoryName(path));
                        if (!contains)
                            builds.Add(new Builds(Path.GetDirectoryName(path),
                                exe,
                                Directory.GetLastWriteTime(Path.GetDirectoryName(path))));
                    }
                }
            }
            return builds;
        }

        public static void PopulateBuildLists(ListView lv, string product, string version)
        {
            lv.Items.Clear();
            List<Builds> builds = GetInstalledBuilds(product, version);
            builds.Sort(delegate (Builds x, Builds y)
            {
                return x.ModifiedDate.CompareTo(y.ModifiedDate);
            });
            builds.Reverse();

            foreach (Builds build in builds)
            {
                ListViewItem item = new ListViewItem(build.InstallPath);
                item.SubItems.Add(build.ModifiedDate.ToString());
                lv.Items.Add(item);
            }
            Utils.ResizeUpdateableListViewColumnWidthForScrollBar(lv, 7, 0, 500);
        }
    }
}
