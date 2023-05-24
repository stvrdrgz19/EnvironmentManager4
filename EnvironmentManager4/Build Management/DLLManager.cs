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

namespace EnvironmentManager4.Build_Management
{
    public partial class DLLManager : Form
    {
        public static string stringToTrimOrAdd = "";

        public DLLManager()
        {
            InitializeComponent();
        }

        private void ReloadProductsList()
        {
            cbProducts.Items.Clear();
            cbProducts.Items.AddRange(Products.ListOfProducts().ToArray());
            cbProducts.SelectedIndex = cbProducts.FindStringExact(Products.SalesPad);
            cbVersion.SelectedIndex = cbVersion.FindStringExact("x86");
        }

        private void LoadBuildsPerProduct(string product, string version)
        {
            cbBuilds.Items.Clear();
            cbBuilds.Text = "";
            lvInstalledDLLs.Items.Clear();
            SettingsModel settings = SettingsUtilities.GetSettings();
            string pathToTrim = "";

            switch(product)
            {
                case Products.SalesPad:
                    switch (version)
                    {
                        case "x86":
                        case "Pre":
                            pathToTrim = settings.BuildManagement.SalesPadx86Directory;
                            break;
                        case "x64":
                            pathToTrim = settings.BuildManagement.SalesPadx64Directory;
                            break;
                    }
                    break;
                case Products.DataCollection:
                    pathToTrim = settings.BuildManagement.DataCollectionDirectory;
                    break;
                case Products.SalesPadMobile:
                    pathToTrim = settings.BuildManagement.SalesPadMobileDirectory;
                    break;
                case Products.ShipCenter:
                    pathToTrim = settings.BuildManagement.ShipCenterDirectory;
                    break;
            }

            stringToTrimOrAdd = String.Format(@"{0}\", pathToTrim);

            List<Builds> buildList = Builds.GetInstalledBuilds(product, version);
            foreach (Builds build in buildList)
                cbBuilds.Items.Add(build.InstallPath.Replace(stringToTrimOrAdd, ""));
        }

        private void LoadDLLs(string buildPath, string product, string version)
        {
            lvInstalledDLLs.Items.Clear();
            try
            {
                InstallProperties ip = InstallProperties.RetrieveInstallProperties(String.Format("{0}{1}", stringToTrimOrAdd, buildPath));
                List<DLLFileModel> CustomDLLs = ip.CustomDLLs;
                List<DLLFileModel> ExtendedDLLs = ip.ExtendedDLLs;
                RetrieveDLLsFromInstallPropertiesFile(CustomDLLs, "Custom");
                RetrieveDLLsFromInstallPropertiesFile(ExtendedDLLs, "Extended");
                Utilities.ResizeUpdateableListViewColumnWidthForScrollBar(lvInstalledDLLs, 11, 1, 118);
            }
            catch
            {
                CompareDLLsWithoutInstallPropertiesFile(buildPath, product, version);
            }
            Utilities.ResizeUpdateableListViewColumnWidthForScrollBar(lvInstalledDLLs, 11, 1, 118);
        }

        private void RetrieveDLLsFromInstallPropertiesFile(List<DLLFileModel> dllList, string type)
        {
            foreach (DLLFileModel dll in dllList)
            {
                ListViewItem item = new ListViewItem(dll.CoreDLL);
                item.SubItems.Add(type);
                lvInstalledDLLs.Items.Add(item);
            }
        }

        private void CompareDLLsWithoutInstallPropertiesFile(string buildPath, string product, string version)
        {
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            string[] coreModules = CoreModules.GetCoreModulesByProduct(product);

            string filter = String.Format("{0}*.dll", pi.ModuleNaming);
            string[] buildMods = Directory.GetFiles(String.Format("{0}{1}", stringToTrimOrAdd, buildPath), filter)
                .Select(filePath => Path.GetFileName(filePath))
                .ToList();

            string[] buildModules = Directory.GetFiles(String.Format("{0}{1}", stringToTrimOrAdd, buildPath), String.Format("{0}*", pi.ModuleNaming));
            foreach (string module in buildModules)
                module.Replace(String.Format(@"{0}{1}\", stringToTrimOrAdd, buildPath), "");
            var nonCoreModules = buildModules.Except(coreModules);

            foreach (string module in nonCoreModules)
            {
                ListViewItem item = new ListViewItem(module);
                item.SubItems.Add("Unknown");
                lvInstalledDLLs.Items.Add(item);
            }
        }

        private void DLLManager_Load(object sender, EventArgs e)
        {
            ReloadProductsList();
            LoadBuildsPerProduct(cbProducts.Text, cbVersion.Text);
            return;
        }

        private void cbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProducts.Text == Products.SalesPad)
                cbVersion.Enabled = true;
            else
            {
                cbVersion.Enabled = false;
                cbVersion.SelectedIndex = cbVersion.FindStringExact("x86");
            }
            LoadBuildsPerProduct(cbProducts.Text, cbVersion.Text);
            return;
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBuildsPerProduct(cbProducts.Text, cbVersion.Text);
            return;
        }

        private void cbBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDLLs(cbBuilds.Text, cbProducts.Text, cbVersion.Text);
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadBuildsPerProduct(cbProducts.Text, cbVersion.Text);
            return;
        }
    }
}
