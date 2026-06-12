using EnvironmentManager4.src.Core;
using EnvironmentManager4.src.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4.src.WinForms.Controls
{
    public partial class BuildList : UserControl
    {
        public BuildList()
        {
            InitializeComponent();
        }
        public string SelectedProduct;
        public string SelectedVersion;

        public List<string> SelectedBuilds
        {
            get
            {
                return lvBuilds.SelectedItems
                    .Cast<ListViewItem>()
                    .Select(item => item.Text)   // column 0
                    .ToList();
            }
        }

        public event EventHandler BuildsSelectionChanged;

        private void OnBuildSelectionChanged(object sender, EventArgs e)
        {
            BuildsSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void PopulateBuildList(string product, string version)
        {
            if (String.IsNullOrWhiteSpace(product))
                product = SelectedProduct;
            if (String.IsNullOrWhiteSpace(version))
                version = SelectedVersion;
            lvBuilds.BeginUpdate();
            // clear the build list first
            lvBuilds.Items.Clear();
            // populate the build list accordingly
            List<Builds> builds = Builds.GetInstalledBuilds(product, version);
            foreach (Builds build in builds)
            {
                ListViewItem item = new ListViewItem(build.InstallPath);
                item.SubItems.Add(build.ModifiedDate.ToString());
                lvBuilds.Items.Add(item);
            }
            lvBuilds.EndUpdate();
        }
    }
}
