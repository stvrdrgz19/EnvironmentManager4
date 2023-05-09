using System;
using System.IO;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class DeleteBuilds : Form
    {
        private int sortColumn = -1;

        public DeleteBuilds()
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

        private void DeleteBuilds_Load(object sender, EventArgs e)
        {
            this.lvInstalledBuilds.ColumnClick += new ColumnClickEventHandler(ColumnClick);
            LoadProductList(cbProducts);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvInstalledBuilds.Items)
                item.Selected = true;
            return;
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvInstalledBuilds.Items)
                item.Selected = false;
            return;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int count = lvInstalledBuilds.SelectedItems.Count;
            if (count > 0)
            {
                string message = "Are you sure you want to delete the selected build(s)? This action cannot be undone.";
                string caption = "CONFIRM";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    foreach (ListViewItem item in lvInstalledBuilds.SelectedItems)
                        Directory.Delete(item.Text, true);
                    Builds.PopulateBuildLists(lvInstalledBuilds, cbProducts.Text, cbVersion.Text);
                }
            }
            return;
        }

        private void cbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product = cbProducts.Text;
            if (product != Products.SalesPad)
            {
                cbVersion.Text = "x86";
                cbVersion.Enabled = false;
            }
            else if (product == Products.SalesPad)
            {
                if (cbVersion.Enabled == false)
                    cbVersion.Enabled = true;
            }
            Builds.PopulateBuildLists(lvInstalledBuilds, product, cbVersion.Text);
            return;
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Builds.PopulateBuildLists(lvInstalledBuilds, cbProducts.Text, cbVersion.Text);
            return;
        }

        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.  
                sortColumn = e.Column;
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

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.s_DeleteBuilds = null;
        }
    }
}
