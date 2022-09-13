using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class DeleteBuilds : Form
    {
        private ListViewColumnSorter lvwColumnSorter;
        public DeleteBuilds()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvInstalledBuilds.ListViewItemSorter = lvwColumnSorter;
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
            {
                item.Selected = true;
            }
            return;
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvInstalledBuilds.Items)
            {
                item.Selected = false;
            }
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
                    {
                        Directory.Delete(item.Text, true);
                    }
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
                {
                    cbVersion.Enabled = true;
                }
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
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvInstalledBuilds.Sort();
        }

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.deleteBuilds = null;
        }
    }
}
