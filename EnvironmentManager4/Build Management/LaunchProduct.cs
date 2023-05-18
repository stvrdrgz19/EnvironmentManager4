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
        public LaunchProduct()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static string product;
        public static string version;
        public int sortColumn = -1;

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

        public static void RemoveDLLsFromInstallPropertiesFile(List<DLLFileModel> dllsFromFile, List<string> dllsToRemove, InstallProperties ip, string path, string type)
        {
            if (dllsFromFile.Count > 0)
                for (int i = dllsFromFile.Count() - 1; i > -1; i--)
                {
                    if (dllsToRemove.Contains(dllsFromFile[i].CoreDLL))
                    {
                        dllsToRemove.RemoveAt(dllsToRemove.IndexOf(dllsFromFile[i].CoreDLL));
                        foreach (string file in dllsFromFile[i].Files)
                            File.Delete(String.Format(@"{0}\{1}", path, file));

                        switch (type)
                        {
                            case "custom":
                                ip.CustomDLLs.RemoveAt(i);
                                break;
                            case "extended":
                                ip.ExtendedDLLs.RemoveAt(i);
                                break;
                        }
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

                RemoveDLLsFromInstallPropertiesFile(customDLLs, selectedDLLs, newIP, path, "custom");
                RemoveDLLsFromInstallPropertiesFile(extendedDLLs, selectedDLLs, newIP, path, "extended");

                foreach (string file in selectedDLLs)
                    File.Delete(String.Format(@"{0}\{1}", path, file));

                if (startingIP != newIP)
                    newIP.WritePropertiesFile();
            }
            else
                foreach (string dll in selectedDLLs)
                    File.Delete(String.Format(@"{0}\{1}", path, dll));
        }

        private void LaunchProduct_Load(object sender, EventArgs e)
        {
            this.Text = String.Format(@"Launch {0} {1}", product, version);
            Builds.PopulateBuildLists(lvInstalledBuilds, product, version);
            if (product != Products.SalesPad)
            {
                checkRunDatabaseUpdate.Enabled = false;
                checkRunDatabaseUpdate.Checked = false;
            }
            this.lvInstalledBuilds.ColumnClick += new ColumnClickEventHandler(ColumnClick);
            return;
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            if (lvInstalledBuilds.SelectedItems.Count != 0)
            {
                string selectedBuild = lvInstalledBuilds.SelectedItems[0].Text;
                List<Builds> builds = Builds.GetInstalledBuilds(product, version);
                this.Close();
                foreach (Builds build in builds)
                {
                    if (selectedBuild == build.InstallPath)
                    {
                        if (checkRunDatabaseUpdate.Checked)
                        {
                            Toasts.Toast("Running Datbase Update"
                                ,"The database update for the selected build is bring ran, this may take a few minutes. The build will be launched once complete."
                                ,1);
                            DatabaseManagement.RunSalesPadDatabaseUpdate(selectedBuild);
                        }
                        
                        Process.Start(String.Format(@"{0}\{1}",
                            selectedBuild,
                            build.Exe));
                    }
                }
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

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_Launch = null;
        }
    }
}
