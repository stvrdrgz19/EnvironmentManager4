using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4.Build_Management
{
    public partial class InstallPropertiesMonitor : Form
    {
        private int sortBuildColumn = -1;
        private int sortPropertiesColumn = -1;

        public InstallPropertiesMonitor()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        private static void LoadProductList(ComboBox cbProducts)
        {
            cbProducts.Items.Clear();
            foreach (string product in Products.ListOfProducts())
            {
                cbProducts.Items.Add(product);
            }
            cbProducts.SelectedIndex = cbProducts.FindStringExact(Products.SalesPad);
        }

        private void InstallPropertiesMonitor_Load(object sender, EventArgs e)
        {
            LoadProductList(cbProducts);
            this.lvInstalledBuilds.ColumnClick += new ColumnClickEventHandler(lvInstalledBuilds_ColumnClick);
            this.lvInstallProperties.ColumnClick += new ColumnClickEventHandler(lvInstallProperties_ColumnClick);
            return;
        }

        private void cbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product = cbProducts.Text;
            cbVersion.Text = "x86";

            if (product != Products.SalesPad && product != Products.ShipCenter)
                cbVersion.Enabled = false;
            else
                cbVersion.Enabled = true;

            Builds.PopulateBuildLists(lvInstalledBuilds, product, cbVersion.Text);
            return;
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Builds.PopulateBuildLists(lvInstalledBuilds, cbProducts.Text, cbVersion.Text);
            return;
        }

        private void lvInstalledBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvInstalledBuilds.SelectedItems.Count == 0)
                return;

            string path = lvInstalledBuilds.SelectedItems[0].Text;
            try
            {
                InstallProperties ip = InstallProperties.RetrieveInstallProperties(path);
                tbProduct.Text = ip.Product;
                tbVersion.Text = ip.Version;
                tbBuildPath.Text = ip.BuildPath;
                tbInstallPath.Text = ip.InstallPath;

                lvInstallProperties.Items.Clear();

                List<DLLFileModel> CustomDLLs = ip.CustomDLLs;
                foreach (DLLFileModel dllModel in CustomDLLs)
                    //DataCollection dlls are written to the InstallProperties file differently
                    if (ip.Product == Products.DataCollection)
                    {
                        ListViewItem item = new ListViewItem(dllModel.CoreDLL);
                        item.SubItems.Add(dllModel.CoreDLL);
                        lvInstallProperties.Items.Add(item);
                    }
                    else
                    {
                        foreach (string file in dllModel.Files)
                        {
                            ListViewItem item = new ListViewItem(dllModel.CoreDLL);
                            item.SubItems.Add(file);
                            lvInstallProperties.Items.Add(item);
                        }
                    }

                List<DLLFileModel> ExtendedDLLs = ip.ExtendedDLLs;
                foreach (DLLFileModel dllModel in ExtendedDLLs)
                    foreach (string file in dllModel.Files)
                    {
                        ListViewItem item = new ListViewItem(dllModel.CoreDLL);
                        item.SubItems.Add(file);
                        lvInstallProperties.Items.Add(item);
                    }
                Utilities.ResizeUpdateableListViewColumnWidthForScrollBar(lvInstallProperties, 9, 1, 329);
            }
            catch
            {
                string message = "The selected build doesn't have an installProperties file.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(message, caption, buttons, icon);

                tbProduct.Text = "";
                tbVersion.Text = "";
                tbBuildPath.Text = "";
                tbInstallPath.Text = "";
                lvInstallProperties.Items.Clear();
            }
            return;
        }

        private void lvInstalledBuilds_ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortBuildColumn)
            {
                // Set the sort column to the new column.  
                sortBuildColumn = e.Column;
                // Set the sort order to ascending by default.  
                lvInstalledBuilds.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.  
                if (lvInstalledBuilds.Sorting == SortOrder.Ascending)
                    lvInstalledBuilds.Sorting = SortOrder.Descending;
                else
                    lvInstalledBuilds.Sorting = SortOrder.Ascending;
            }
            // Call the sort method to manually sort.  
            lvInstalledBuilds.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer  
            // object.  
            this.lvInstalledBuilds.ListViewItemSorter = new ListViewItemComparer(e.Column, lvInstalledBuilds.Sorting);
        }

        private void lvInstallProperties_ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortPropertiesColumn)
            {
                // Set the sort column to the new column.  
                sortPropertiesColumn = e.Column;
                // Set the sort order to ascending by default.  
                lvInstallProperties.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.  
                if (lvInstallProperties.Sorting == SortOrder.Ascending)
                    lvInstallProperties.Sorting = SortOrder.Descending;
                else
                    lvInstallProperties.Sorting = SortOrder.Ascending;
            }
            // Call the sort method to manually sort.  
            lvInstallProperties.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer  
            // object.  
            this.lvInstallProperties.ListViewItemSorter = new ListViewItemComparer(e.Column, lvInstallProperties.Sorting);
        }

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.s_InstallPropertiesMonitor = null;
        }
    }
}
