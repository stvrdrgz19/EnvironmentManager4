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
        public DeleteBuilds()
        {
            InitializeComponent();
        }

        /*
        SalesPad GP
        DataCollection
        SalesPad Mobile
        ShipCenter
        Customer Portal Web
        Customer Portal API
        */

        private void UpdateBuildList(string product, string version)
        {
            InstalledBuilds.Items.Clear();
            List<string> installedBuilds = new List<string>();
            installedBuilds.AddRange(Utilities.InstalledBuilds(product, version));
            foreach (string build in installedBuilds)
            {
                InstalledBuilds.Items.Add(Path.GetDirectoryName(build));
            }
        }

        private void DeleteBuilds_Load(object sender, EventArgs e)
        {
            //
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //https://stackoverflow.com/questions/934241/how-do-i-select-all-items-in-a-listbox-on-checkbox-checked
            for (int i = 0; i < InstalledBuilds.Items.Count; i++)
            {
                InstalledBuilds.SetSelected(i, true);
            }
            return;
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            InstalledBuilds.ClearSelected();
            return;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete the selected builds? This action cannot be undone.";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                foreach (string ListBoxItem in InstalledBuilds.SelectedItems)
                {
                    Directory.Delete(ListBoxItem, true);
                }
                UpdateBuildList(cbProducts.Text, cbVersion.Text);
            }
            return;
        }

        private void cbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product = cbProducts.Text;
            if (product != "SalesPad GP")
            {
                cbVersion.Text = "x86";
                cbVersion.Enabled = false;
            }
            else if (product == "SalesPad GP")
            {
                if (cbVersion.Enabled == false)
                {
                    cbVersion.Enabled = true;
                }
            }
            UpdateBuildList(cbProducts.Text, cbVersion.Text);
            return;
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBuildList(cbProducts.Text, cbVersion.Text);
            return;
        }
    }
}
