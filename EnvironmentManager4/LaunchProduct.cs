using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class LaunchProduct : Form
    {
        public LaunchProduct()
        {
            InitializeComponent();
        }

        public static string product;
        public static string version;
        public static string path;

        private void RetrieveBuildPaths()
        {
            string executable = "";
            switch (product)
            {
                case "SalesPad":
                    executable = @"*SalesPad.exe";
                    break;
            }
            var buildList = Directory.GetFiles(path + @"\", executable, SearchOption.AllDirectories);
            foreach (string build in buildList)
            {
                InstalledBuilds.Items.Add(Path.GetDirectoryName(build));
            }
        }

        private void LaunchProduct_Load(object sender, EventArgs e)
        {
            this.Text = String.Format(@"Launch {0} {1}", product, version);
            RetrieveBuildPaths();
            return;
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            string selectedBuild = InstalledBuilds.Text;
            if (!String.IsNullOrWhiteSpace(selectedBuild))
            {
                Process.Start(selectedBuild + @"\SalesPad.exe");
                this.Close();
            }
            return;
        }

        private void CopyLabels_Click(object sender, EventArgs e)
        {

        }

        private void RemoveDLLs_Click(object sender, EventArgs e)
        {

        }
    }
}
