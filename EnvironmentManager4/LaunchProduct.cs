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
        }

        public static string product;
        public static string version;
        public static string path;
        public static string[] LoadDllList(string path)
        {
            CoreModules coreModules = JsonConvert.DeserializeObject<CoreModules>(File.ReadAllText(Environment.CurrentDirectory + @"\Files\CoreModules.json"));
            string[] dllList = Directory.GetFiles(path, "SalesPad.Module.*.dll").Select(file => Path.GetFileName(file)).ToArray();
            return dllList.Except(coreModules.DLLName).ToArray();
        }

        private void RetrieveBuildPaths()
        {
            List<string> installedBuilds = new List<string>();
            installedBuilds.AddRange(Utilities.InstalledBuilds(product, version));
            foreach (string build in installedBuilds)
            {
                InstalledBuilds.Items.Add(Path.GetDirectoryName(build));
            }
        }

        private void LaunchProduct_Load(object sender, EventArgs e)
        {
            this.Text = String.Format(@"Launch {0} {1}", product, version);
            RetrieveBuildPaths();
            return;
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            string selectedBuild = InstalledBuilds.Text;
            if (!String.IsNullOrWhiteSpace(selectedBuild))
            {
                Process.Start(String.Format(@"{0}\{1}", selectedBuild, Utilities.RetrieveExe(product)));
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

        private void InstalledBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBuild = InstalledBuilds.Text;
            SelectedBuildDLLs.Items.Clear();
            if (!String.IsNullOrWhiteSpace(selectedBuild))
            {
                SelectedBuildDLLs.Items.AddRange(LoadDllList(selectedBuild));
            }
            return;
        }
    }
}
