using Newtonsoft.Json;
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
        private ListViewColumnSorter lvwColumnSorter;
        public LaunchProduct()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvInstalledBuilds.ListViewItemSorter = lvwColumnSorter;
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static string product;
        public static string version;

        public static string[] LoadDllList(string path)
        {
            string[] coreModules = CoreModules.GetCoreModulesByProduct(product);
            string filter = "";
            switch(product)
            {
                case Products.SalesPad:
                    filter = "SalesPad.Module.";
                    break;
                case Products.DataCollection:
                    filter = "SalesPad.DataCollection.";
                    break;
                case Products.ShipCenter:
                    filter = "SalesPad.ShipCenter.";
                    break;
                case Products.SalesPadMobile:
                    filter = "NOMODULES";
                    break;
            }
            string[] dllList = Directory.GetFiles(path, String.Format("{0}*.dll", filter)).Select(file => Path.GetFileName(file)).ToArray();
            return dllList.Except(coreModules).ToArray();
        }

        public static void RemoveTheThings(List<DLLFileModel> dllsFromFile, List<string> selectedDLLs, InstallProperties ip, string path)
        {
            if (dllsFromFile.Count > 0)
                for (int i = dllsFromFile.Count() - 1; i > -1; i--)
                {
                    if (selectedDLLs.Contains(dllsFromFile[i].CoreDLL))
                    {
                        selectedDLLs.RemoveAt(selectedDLLs.IndexOf(dllsFromFile[i].CoreDLL));
                        foreach (string file in dllsFromFile[i].Files)
                            File.Delete(String.Format(@"{0}\{1}", path, file));
                        ip.CustomDLLs.RemoveAt(i);
                    }
                }
        }

        private void DeleteDLLFiles(string path, List<string> selectedDLLs)
        {
            InstallProperties startingIP = InstallProperties.RetrieveInstallProperties(path);
            InstallProperties newIP = InstallProperties.RetrieveInstallProperties(path);

            if (InstallProperties.DoesInstallHaveProperties(path))
            {
                List<DLLFileModel> customDLLs = InstallProperties.RetrieveInstalledDLLsFromProperties(path, true);
                List<DLLFileModel> extendedDLLs = InstallProperties.RetrieveInstalledDLLsFromProperties(path, false);

                RemoveTheThings(customDLLs, selectedDLLs, newIP, path);
                RemoveTheThings(extendedDLLs, selectedDLLs, newIP, path);

                foreach (string file in selectedDLLs)
                    File.Delete(String.Format(@"{0}\{1}", path, file));

                if (startingIP != newIP)
                    InstallProperties.WritePropertiesFile(newIP);
            }
            else
                foreach (string dll in selectedDLLs)
                    File.Delete(String.Format(@"{0}\{1}", path, dll));
        }

        private void LaunchProduct_Load(object sender, EventArgs e)
        {
            this.Text = String.Format(@"Launch {0} {1}", product, version);
            Builds.PopulateBuildLists(lvInstalledBuilds, product, version);
            this.lvInstalledBuilds.ColumnClick += new ColumnClickEventHandler(ColumnClick);
            return;
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            if (lvInstalledBuilds.SelectedItems.Count != 0)
            {
                string selectedBuild = lvInstalledBuilds.SelectedItems[0].Text;
                List<Builds> builds = Builds.GetInstalledBuilds(product, version);
                foreach (Builds build in builds)
                {
                    if (selectedBuild == build.InstallPath)
                        Process.Start(String.Format(@"{0}\{1}",
                            selectedBuild,
                            build.Exe));
                }
                this.Close();
            }
            return;
        }

        private void CopyLabels_Click(object sender, EventArgs e)
        {
            if (SelectedBuildDLLs.SelectedItems.Count == 0)
                return;

            StringBuilder builder = new StringBuilder();

            foreach (string dll in SelectedBuildDLLs.SelectedItems)
                builder.Append(dll.ToString()).AppendLine();

            try
            {
                Clipboard.SetText(builder.ToString());
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please select DLLs to copy.");
                return;
            }

            string copyMessage = "The following dll's have been copied to the clipboard: \n\n" + builder.ToString();
            string copyCaption = "COPIED";
            MessageBoxButtons copyButton = MessageBoxButtons.OK;
            MessageBoxIcon copyIcon = MessageBoxIcon.Exclamation;

            MessageBox.Show(copyMessage, copyCaption, copyButton, copyIcon);
            return;
        }

        private void RemoveDLLs_Click(object sender, EventArgs e)
        {
            string selectedBuild = lvInstalledBuilds.SelectedItems[0].Text;
            if (SelectedBuildDLLs.SelectedItems.Count == 0)
                return;

            StringBuilder builder = new StringBuilder();

            foreach (string dll in SelectedBuildDLLs.SelectedItems)
                builder.Append(dll.ToString()).AppendLine();

            string message = String.Format("Are you sure you want to delete the following dll(s)? This is irreversible and could cause issues with the selected build.\n\n{0}", builder.ToString());
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                List<string> selectedDLLs = new List<string>();
                foreach (string dll in SelectedBuildDLLs.SelectedItems)
                    selectedDLLs.Add(dll);
                DeleteDLLFiles(lvInstalledBuilds.SelectedItems[0].Text, selectedDLLs);

                SelectedBuildDLLs.Items.Clear();
                SelectedBuildDLLs.Items.AddRange(LoadDllList(lvInstalledBuilds.SelectedItems[0].Text));
            }
            return;
        }

        private void lvInstalledBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvInstalledBuilds.SelectedItems.Count == 0)
                return;
            SelectedBuildDLLs.Items.Clear();
            SelectedBuildDLLs.Items.AddRange(LoadDllList(lvInstalledBuilds.SelectedItems[0].Text));
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

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_Launch = null;
        }
    }
}
