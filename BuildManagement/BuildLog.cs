using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace BuildManagement
{
    public partial class BuildLog : Form
    {
        public BuildLog()
        {
            InitializeComponent();
        }

        public static List<ListViewProperties> lvp = new List<ListViewProperties>();
        public static List<ListViewProperties> lvpDlls = new List<ListViewProperties>();
        private int sortBuilds = -1;
        private int sortDLLs = -1;

        private void LoadBuildLog(List<ListViewProperties> lvp)
        {
            lvBuilds.Items.Clear();
            lvDlls.Items.Clear();
            ListViewProperties.UpdateListViewProperties(lvp);
            List<BuildModel> builds = SqliteDataAccess.LoadBuilds();
            foreach (var build in builds)
            {
                ListViewItem item1 = new ListViewItem(build.Path);
                item1.SubItems.Add(build.Version);
                item1.SubItems.Add(build.EntryDate);
                item1.SubItems.Add(build.Product);
                lvBuilds.Items.Add(item1);
            }
            Utils.ResizeListViewColumnWidthForScrollBar(lvBuilds, 9, 0);
            this.lvBuilds.Items[0].Focused = true;
            this.lvBuilds.Items[0].Selected = true;
        }

        private void BuildLog_Load(object sender, EventArgs e)
        {
            lvp = ListViewProperties.RetrieveListViewProperties(lvBuilds);
            lvpDlls = ListViewProperties.RetrieveListViewProperties(lvDlls);
            LoadBuildLog(lvp);
            this.lvBuilds.ColumnClick += new ColumnClickEventHandler(lvBuilds_ColumnClick);
            this.lvDlls.ColumnClick += new ColumnClickEventHandler(lvDlls_ColumnClick);
            return;
        }

        private void lvBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvDlls.Items.Clear();
            ListViewProperties.UpdateListViewProperties(lvpDlls);
            ListView.SelectedListViewItemCollection build = this.lvBuilds.SelectedItems;
            foreach (ListViewItem item in build)
            {
                ProductInfo pi = ProductInfo.GetProductInfo(item.SubItems[3].Text, item.SubItems[1].Text);
                List<DllModel> dlls = SqliteDataAccess.LoadDlls(SqliteDataAccess.GetParentId(item.SubItems[2].Text));
                if (dlls == null)
                    return;

                foreach (var dll in dlls)
                {
                    ListViewItem item1 = new ListViewItem(dll.Name.Replace(pi.ModuleNaming, ""));
                    item1.SubItems.Add(dll.Type);
                    lvDlls.Items.Add(item1);
                }
            }

            Utils.ResizeListViewColumnWidthForScrollBar(lvDlls, 9, 1);
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
                return;

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
                foreach (ListViewItem item in lvDlls.Items)
                    selectedBuild += "\n" + item.SubItems[1].Text + ": " + Modules.TrimVersionAndExtension(item.Text, lvBuilds.SelectedItems[0].SubItems[3].Text);

            Clipboard.SetText(selectedBuild);
        }

        private void lvBuilds_ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortBuilds)
            {
                // Set the sort column to the new column.  
                sortBuilds = e.Column;
                // Set the sort order to ascending by default.  
                lvBuilds.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.  
                if (lvBuilds.Sorting == SortOrder.Ascending)
                    lvBuilds.Sorting = SortOrder.Descending;
                else
                    lvBuilds.Sorting = SortOrder.Ascending;
            }
            // Call the sort method to manually sort.  
            lvBuilds.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer  
            // object.  
            this.lvBuilds.ListViewItemSorter = new ListViewItemComparer(e.Column, lvBuilds.Sorting);
        }

        private void lvDlls_ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortDLLs)
            {
                // Set the sort column to the new column.  
                sortDLLs = e.Column;
                // Set the sort order to ascending by default.  
                lvDlls.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.  
                if (lvDlls.Sorting == SortOrder.Ascending)
                    lvDlls.Sorting = SortOrder.Descending;
                else
                    lvDlls.Sorting = SortOrder.Ascending;
            }
            // Call the sort method to manually sort.  
            lvDlls.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer  
            // object.  
            this.lvDlls.ListViewItemSorter = new ListViewItemComparer(e.Column, lvDlls.Sorting);
        }
    }
}
