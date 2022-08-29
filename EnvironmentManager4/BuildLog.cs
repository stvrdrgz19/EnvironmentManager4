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

        List<BuildModel> builds = new List<BuildModel>();
        List<DllModel> dlls = new List<DllModel>();

        private void LoadBuildLog()
        {
            lvBuilds.Items.Clear();
            lvDlls.Items.Clear();
            builds = SqliteDataAccess.LoadBuilds();
            foreach (var build in builds)
            {
                ListViewItem item1 = new ListViewItem(build.Path);
                item1.SubItems.Add(build.Version);
                item1.SubItems.Add(build.EntryDate);
                item1.SubItems.Add(build.Product);
                lvBuilds.Items.Add(item1);
            }
            Utilities.ResizeListViewColumnWidth(lvBuilds, 9, 0);
            this.lvBuilds.Items[0].Focused = true;
            this.lvBuilds.Items[0].Selected = true;
        }

        private void BuildLog_Load(object sender, EventArgs e)
        {
            LoadBuildLog();
            return;
        }

        private void lvBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvDlls.Items.Clear();
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
            Utilities.ResizeListViewColumnWidth(lvDlls, 9, 1);
            return;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBuildLog();
            return;
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            string product = lvBuilds.SelectedItems[0].SubItems[3].Text;
            string selectedBuild = lvBuilds.SelectedItems[0].Text;
            string executable = "";
            switch (product)
            {
                case "SalesPad GP":
                    executable = @"\SalesPad.exe";
                    break;
                case "DataCollection":
                    executable = @"\DataCollection Extended Warehouse.exe";
                    break;
                case "Inventory Manager":
                    executable = @"\SalesPad Inventory Manager Extended Warehouse.exe";
                    break;
                case "SalesPad Mobile":
                    executable = @"\SalesPad.GP.Mobile.Server.exe";
                    break;
                case "ShipCenter":
                    executable = @"\SalesPad.ShipCenter.exe";
                    break;
            }

            string productPath = String.Format(@"{0}{1}", selectedBuild, executable);

            if (product == "SalesPad GP" || product == "SalesPad Mobile" || product == "ShipCenter")
            {
                if (File.Exists(productPath))
                {
                    try
                    {
                        Process.Start(productPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format("There was an error launching {0}, the error is as follows:\n\n{1}", productPath, ex.Message));
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("The selected build is no longer installed:\n\n{0}", productPath));
                }
                return;
            }

            if (product == "DataCollection" || product == "Inventory Manager")
            {
                //SalesPad Inventory Manager Extended Warehouse.exe
            }
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
                    case "SalesPad GP":
                        product = "Desktop: ";
                        break;
                    case "DataCollection":
                        product = "Console: ";
                        break;
                    case "SalesPad Mobile":
                        product = "Console: ";
                        break;
                    case "ShipCenter":
                        product = "ShipCenter: ";
                        break;
                }
                selectedBuild = product + lvBuilds.SelectedItems[0].Text;
            }
            if (copyDll)
            {
                foreach (ListViewItem item in lvDlls.Items)
                {
                    selectedBuild += "\n" + item.SubItems[1].Text + ": " + item.Text;
                }
            }
            Clipboard.SetText(selectedBuild);
        }

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.buildLog = null;
        }
    }
}
