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
                    foreach (string file in dllModel.Files)
                    {
                        ListViewItem item = new ListViewItem(dllModel.CoreDLL);
                        item.SubItems.Add(file);
                        lvInstallProperties.Items.Add(item);
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

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.s_InstallPropertiesMonitor = null;
        }
    }
}
