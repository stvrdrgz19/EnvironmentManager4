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
        }

        public static string product;
        public static string version;

        public static string[] LoadDllList(string path)
        {
            string fileName = "";
            switch (product)
            {
                case "SalesPad GP":
                case "DataCollection":
                    fileName = "CoreModules";
                    break;
                case "SalesPad Mobile":
                    fileName = "SPMCoreModules";
                    break;
                case "ShipCenter":
                    fileName = "SCCoreModules";
                    break;
            }
            CoreModules coreModules = JsonConvert.DeserializeObject<CoreModules>(File.ReadAllText(String.Format(@"{0}\Files\{1}.json", Environment.CurrentDirectory, fileName)));
            string[] dllList = Directory.GetFiles(path, "SalesPad.Module.*.dll").Select(file => Path.GetFileName(file)).ToArray();
            return dllList.Except(coreModules.DLLName).ToArray();
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
            string selectedBuild = lvInstalledBuilds.SelectedItems[0].Text;
            if (!String.IsNullOrWhiteSpace(selectedBuild))
            {
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
            {
                return;
            }
            List<string> selectedDLLList = new List<string>();
            StringBuilder builder = new StringBuilder();

            foreach (string dll in SelectedBuildDLLs.SelectedItems)
            {
                selectedDLLList.Add(dll);
            }
            foreach (string str in selectedDLLList)
            {
                builder.Append(str.ToString()).AppendLine();
            }
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
    }
}
