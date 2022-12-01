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
    public partial class BuildLog : Form
    {
        public BuildLog()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static List<BuildModel> builds = new List<BuildModel>();
        public static List<DllModel> dlls = new List<DllModel>();
        public static List<ListViewProperties> lvp = new List<ListViewProperties>();
        public static List<ListViewProperties> lvpDlls = new List<ListViewProperties>();

        private void LoadBuildLog(List<ListViewProperties> lvp)
        {
            lvBuilds.Items.Clear();
            lvDlls.Items.Clear();
            ListViewProperties.UpdateListViewProperties(lvp);
            builds = SqliteDataAccess.LoadBuilds();
            foreach (var build in builds)
            {
                ListViewItem item1 = new ListViewItem(build.Path);
                item1.SubItems.Add(build.Version);
                item1.SubItems.Add(build.EntryDate);
                item1.SubItems.Add(build.Product);
                lvBuilds.Items.Add(item1);
            }
            Utilities.ResizeListViewColumnWidthForScrollBar(lvBuilds, 9, 0);
            this.lvBuilds.Items[0].Focused = true;
            this.lvBuilds.Items[0].Selected = true;
        }

        private void BuildLog_Load(object sender, EventArgs e)
        {
            lvp = ListViewProperties.RetrieveListViewProperties(lvBuilds);
            lvpDlls = ListViewProperties.RetrieveListViewProperties(lvDlls);
            LoadBuildLog(lvp);
            return;
        }

        private void lvBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvDlls.Items.Clear();
            ListViewProperties.UpdateListViewProperties(lvpDlls);
            string entryDate = "";
            ListView.SelectedListViewItemCollection build = this.lvBuilds.SelectedItems;
            foreach (ListViewItem item in build)
            {
                entryDate = item.SubItems[2].Text;
            }
            dlls = SqliteDataAccess.LoadDlls(SqliteDataAccess.GetParentId(entryDate));
            if (dlls == null)
            {
                return;
            }
            foreach (var dll in dlls)
            {
                ListViewItem item1 = new ListViewItem(dll.Name);
                item1.SubItems.Add(dll.Type);
                lvDlls.Items.Add(item1);
            }
            Utilities.ResizeListViewColumnWidthForScrollBar(lvDlls, 9, 1);
            return;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBuildLog(lvp);
            return;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (lvBuilds.SelectedItems.Count <= 0)
            {
                return;
            }

            bool copyProduct = cbProduct.Checked;
            bool copyDll = cbDlls.Checked;
            string product = lvBuilds.SelectedItems[0].SubItems[3].Text;
            string selectedBuild = lvBuilds.SelectedItems[0].Text;
            if (copyProduct)
            {
                switch (product)
                {
                    case Products.SalesPad:
                        product = "Desktop: ";
                        break;
                    case Products.DataCollection:
                        product = "Console: ";
                        break;
                    case Products.SalesPadMobile:
                        product = "Console: ";
                        break;
                    case Products.ShipCenter:
                        product = "ShipCenter: ";
                        break;
                }
                selectedBuild = product + lvBuilds.SelectedItems[0].Text;
            }
            if (copyDll)
            {
                foreach (ListViewItem item in lvDlls.Items)
                {
                    selectedBuild += "\n" + item.SubItems[1].Text + ": " + Modules.TrimVersionAndExtension(item.Text, lvBuilds.SelectedItems[0].SubItems[3].Text);
                }
            }
            Clipboard.SetText(selectedBuild);
        }

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.s_BuildLog = null;
        }
    }
}
