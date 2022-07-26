using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class Install : Form
    {
        public Install()
        {
            InitializeComponent();
        }

        public static Installer install = new Installer();

        public Installer GetInstallerFile(GetInstaller getInstaller)
        {
            ProductInfo pi = ProductInfo.GetProductInfo(getInstaller.Product, getInstaller.Version);
            string initialDir = pi.FileserverDirectory;
            string defaultInstallPath = pi.InstallDirectory;
            string buildPath = "";
            string installerPath = "";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (getInstaller.Product == "SalesPad GP")
                {
                    openFileDialog.Filter = String.Format("Executable Files (*.exe)|*{0}.exe", getInstaller.Version);
                }
                else
                {
                    openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                }
                if (!Directory.Exists(getInstaller.Path))
                {
                    string newPath = String.Format(@"{0}{1}", "\\", getInstaller.Path);
                    if (Directory.Exists(newPath))
                    {
                        if (newPath.Contains(initialDir))
                            openFileDialog.InitialDirectory = newPath;
                        else
                            openFileDialog.InitialDirectory = initialDir;
                    }
                    else
                        openFileDialog.InitialDirectory = initialDir;
                }
                else
                {
                    if (getInstaller.Path.Contains(initialDir))
                        openFileDialog.InitialDirectory = getInstaller.Path;
                    else
                        openFileDialog.InitialDirectory = initialDir;
                }

                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Installing Product";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    installerPath = openFileDialog.FileName;
                    buildPath = Path.GetDirectoryName(installerPath);
                }
                else
                    installerPath = "EXIT";
            }

            Installer installer = new Installer();
            installer.BuildPath = buildPath;
            installer.InstallerPath = installerPath;
            installer.DefaultInstallPath = defaultInstallPath;
            installer.Product = getInstaller.Product;
            installer.Version = getInstaller.Version;
            return installer;
        }

        public void LoadInstallPath()
        {
            if (install.Product == "Customer Portal Web" || install.Product == "Customer Portal API")
            {
                return;
            }
            int charCount = 0;
            switch (install.Product)
            {
                case "SalesPad GP":
                    charCount = 43;
                    break;
                case "DataCollection":
                    charCount = 51;
                    break;
                case "SalesPad Mobile":
                    charCount = 50;
                    break;
                case "ShipCenter":
                    charCount = 42;
                    break;
            }
            tbInstallLocation.Text = GetInstallPath(install.DefaultInstallPath, install.BuildPath, charCount);
        }

        public static string GetInstallPath(string defaultPath, string installerPath, int charCount)
        {
            string pathFromInstaller = installerPath.Remove(0, charCount);
            //check if smartbear mode
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string newPath = "";
            switch (settingsModel.Other.Mode)
            {
                case "Standard":
                    newPath = String.Format(@"{0}\{1}", defaultPath, pathFromInstaller);
                    break;
                case "Kyle":
                    string[] pathSplit = pathFromInstaller.Split('\\');
                    if (pathSplit.Count() > 2)
                        newPath = String.Format(@"{0} {1}", defaultPath, pathFromInstaller.Substring(Utilities.GetNthIndex(pathFromInstaller, '\\', pathSplit.Count() - 2)));
                    else
                        newPath = String.Format(@"{0} {1}", defaultPath, pathFromInstaller);
                    break;
                case "SmartBear":
                    newPath = String.Format(@"{0} {1}", defaultPath, pathFromInstaller.Replace(@"\", " "));
                    break;
            }
            return newPath;
        }

        public void LoadModules(string product, string installerPath, string productVersion, string installer)
        {
            string extModulesPath = "";
            string custModulesPath = "";
            if (product == "DataCollection" || product == "ShipCenter" || product == "Customer Portal Web" || product == "SalesPad Mobile")
            {
                lbExtendedModules.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (product == "SalesPad Mobile")
            {
                lbCustomModules.Enabled = false;
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            switch (product)
            {
                case "SalesPad GP":
                    extModulesPath = String.Format(@"{0}\ExtModules\{1}", installerPath, productVersion);
                    custModulesPath = String.Format(@"{0}\CustomModules\{1}", installerPath, productVersion);
                    break;
                case "Customer Portal API":
                    extModulesPath = String.Format(@"{0}\ExtModules", installerPath);
                    custModulesPath = String.Format(@"{0}\CustomModules", installerPath);
                    break;
                case "DataCollection":
                    custModulesPath = String.Format(@"{0}\CustomModules", installerPath);
                    break;
                case "SalesPad Mobile":
                    break;
                case "ShipCenter":
                    custModulesPath = String.Format(@"{0}\Custom", installerPath);
                    break;
                case "Customer Portal Web":
                    custModulesPath = String.Format(@"{0}\Plugins", installerPath);
                    break;
            }
            if (!String.IsNullOrWhiteSpace(extModulesPath))
            {
                lbExtendedModules.Items.AddRange(Modules.RetrieveDLLs(extModulesPath, installerPath, product, installer));
            }
            lbCustomModules.Items.AddRange(Modules.RetrieveDLLs(custModulesPath, installerPath, product, installer));
        }

        public void InstallBuild(string installPath, List<string> extendedModules, List<string> customModules, bool launchAfterInstall, bool openInstallFolder, bool runDatabaseUpdate, bool resetDatabaseVersion)
        {
            //start the busy cursor
            this.Cursor = Cursors.WaitCursor;

            //Disable Install button on form1.
            Form1.EnableInstallButton(false);

            string startTime = DateTime.Now.ToString();

            //global variable installer = the installer file path
            //global variable installerpath = the path to the installer, excluding the file name
            string installerFileName = Path.GetFileName(install.InstallerPath);     //the actual file name without it's path
            string tempInstaller = String.Format(@"{0}\{1}", Utilities.GetInstallerFolder(), installerFileName);
            File.Copy(install.InstallerPath, tempInstaller, true);

            //SILENTLY INSTALL PRODUCT
            Process installProduct = new Process();
            installProduct.StartInfo.FileName = tempInstaller;
            installProduct.StartInfo.Arguments = @"/S /D=" + installPath;
            installProduct.Start();
            installProduct.WaitForExit();

            //WRITE BUILD INFORMATION TO INSTALLEDBUILD TABLE
            BuildModel build = new BuildModel(install.BuildPath, install.Version, startTime, install.Product, installPath);
            try
            {
                SqliteDataAccess.SaveBuild(build);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an error logging the build installation. Error is as follows:\n\n{0}\n\n{1}", e.Message, e.ToString()));
            }

            try
            {
                File.Delete(tempInstaller);
            }
            catch (Exception deleteError)
            {
                MessageBox.Show(String.Format("There was an error deleting the installer file at {0}, error is as follows:\n\n{1}\n\n{2}", tempInstaller, deleteError.Message, deleteError.ToString()));
            }

            //==========================================================================================================================================================================================
            //  DLL INSTALL START
            //==========================================================================================================================================================================================
            List<string> custDllToAdd = new List<string>();
            List<string> extDllToAdd = new List<string>();

            //  ADD ANY SELECTED DLLS TO THE LIST
            foreach (string dll in customModules)
            {
                custDllToAdd.Add(dll);
            }
            custDllToAdd.Sort();

            foreach (string dll in extendedModules)
            {
                extDllToAdd.Add(dll);
            }
            extDllToAdd.Sort();

            if (custDllToAdd.Count > 0 || extDllToAdd.Count > 0)
            {
                Modules.GetDLLs(install.Product, install.BuildPath, install.Version, startTime, custDllToAdd, extDllToAdd);
            }

            //  UNZIP DLLS IF THERE ARE ANY
            if (custDllToAdd.Count > 0 || extDllToAdd.Count > 0)
            {
                if (install.Product == "DataCollection")
                {
                    Modules.CopyDllsToInstalledBuild(installPath);
                }
                else
                {
                    Modules.UnzipDLLFiles();
                    Modules.CopyDllsFromDirectoriesToInstalledBuild(installPath);
                }
            }
            //==========================================================================================================================================================================================
            //  DLL INSTALL END
            //==========================================================================================================================================================================================

            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            if (resetDatabaseVersion)
            {
                DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)));
            }

            if (runDatabaseUpdate)
            {
                if (!resetDatabaseVersion)
                {
                    DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)));
                }
                Process dbUpdate = new Process();
                dbUpdate.StartInfo.FileName = installPath + @"\SalesPad.exe";
                dbUpdate.StartInfo.Arguments = @"/dbUpdate /userfields /conn=TWO";      //write sql to pull databases and prompt user for which database to update. Maybe prompt before installation
                dbUpdate.StartInfo.UseShellExecute = false;
                dbUpdate.Start();
                dbUpdate.WaitForExit();
            }
            this.Cursor = Cursors.Default;

            if (openInstallFolder)
            {
                build.LaunchInstalledFolder();
            }

            if (launchAfterInstall)
            {
                build.LaunchBuild();
            }
            Form1.EnableInstallButton(true);
        }

        private void LoadConfigurations(string product)
        {
            cbConfigurationList.Items.Clear();
            cbConfigurationList.Text = "Select a Configuration";
            cbConfigurationList.Items.Add("None");
            List<Configurations> configurations = Configurations.GetConfigurations();
            foreach (Configurations config in configurations)
            {
                if (config.Product == product)
                    cbConfigurationList.Items.Add(config.ConfigurationName);
            }
        }

        private List<string> SelectedModules(ListBox lb)
        {
            List<string> selectedModules = new List<string>();
            foreach (string dll in lb.SelectedItems)
            {
                selectedModules.Add(dll);
            }
            return selectedModules;
        }

        private void Install_Load(object sender, EventArgs e)
        {
            if (install.Product != "SalesPad GP")
            {
                checkResetDBVersion.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (install.Product == "SalesPad Mobile")
            {
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            this.Text = String.Format("Install {0}", install.Product);
            tbSelectedBuild.Text = install.BuildPath;
            LoadModules(install.Product, install.BuildPath, install.Version, install.InstallerPath);
            LoadInstallPath();
            LoadConfigurations(install.Product);
            return;
        }

        private void btnAddConfiguration_Click(object sender, EventArgs e)
        {
            List<string> extendedList = SelectedModules(lbExtendedModules);
            List<string> customList = SelectedModules(lbCustomModules);
            if (extendedList.Count == 0 && customList.Count == 0)
            {
                string eMessage = "To save a configuration there must be more than 0 extended or custom dlls selected.";
                string eCaption = "ERROR";
                MessageBoxButtons eButtons = MessageBoxButtons.OK;
                MessageBoxIcon eIcon = MessageBoxIcon.Error;

                MessageBox.Show(eMessage, eCaption, eButtons, eIcon);
                return;
            }
            string message = "Are you sure you want to create a new Configuration?";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);

            if (result == DialogResult.Yes)
            {
                TextPrompt addConfiguration = new TextPrompt();
                TextPrompt.title = "Add Configuration";
                TextPrompt.label = "Please enter the name of the new configuration:";
                TextPrompt.isConfiguration = true;
                TextPrompt.extended = extendedList;
                TextPrompt.custom = customList;
                TextPrompt.product = install.Product;
                addConfiguration.FormClosing += new FormClosingEventHandler(AddConfigurationClose);
                addConfiguration.Show();
            }
            return;
        }

        private void AddConfigurationClose(object sender, FormClosingEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextPrompt.output))
            {
                return;
            }
            LoadConfigurations(install.Product);
        }

        private void btnRemoveConfiguration_Click(object sender, EventArgs e)
        {
            if (cbConfigurationList.Text == "Select a Configuration" || cbConfigurationList.Text == "None")
                return;

            Configurations configurationToDelete = new Configurations();
            configurationToDelete.Product = install.Product;
            configurationToDelete.ConfigurationName = cbConfigurationList.Text;
            configurationToDelete.ExtendedModules = SelectedModules(lbExtendedModules);
            configurationToDelete.CustomModules = SelectedModules(lbCustomModules);

            string message = String.Format("Are you sure you want to delete the '{0}' configuration for the '{1}' product? This action cannot be undone."
                ,configurationToDelete.ConfigurationName
                ,configurationToDelete.Product);
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                Configurations.DeleteConfiguration(configurationToDelete);
                LoadConfigurations(install.Product);
            }
            return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            List<string> defaultPaths = new List<string>
            {
                settingsModel.BuildManagement.SalesPadx86Directory,
                settingsModel.BuildManagement.SalesPadx64Directory,
                settingsModel.BuildManagement.DataCollectionDirectory,
                settingsModel.BuildManagement.SalesPadMobileDirectory,
                settingsModel.BuildManagement.ShipCenterDirectory,
                settingsModel.BuildManagement.GPWebDirectory,
                settingsModel.BuildManagement.WebAPIDirectory
            };

            DirectoryInfo di = new DirectoryInfo(Utilities.GetInstallerFolder());
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            string installPath = tbInstallLocation.Text;
            if (defaultPaths.Contains(installPath))
            {
                MessageBox.Show("Please enter an install location different than the default:\n\n" + installPath);
                return;
            }

            if (Directory.Exists(installPath))
            {
                string existsMessage = String.Format("{0} is already installed in the specified location, do you want to overwrite this install?", install.Product);
                string existsCaption = "EXISTS";
                MessageBoxButtons existsButtons = MessageBoxButtons.YesNo;
                MessageBoxIcon existsIcon = MessageBoxIcon.Warning;
                DialogResult existsResult;

                existsResult = MessageBox.Show(existsMessage, existsCaption, existsButtons, existsIcon);
                if (existsResult == DialogResult.Yes)
                {
                    Directory.Delete(installPath, true);
                }
                else
                {
                    return;
                }
            }
            this.Close();

            List<string> selectedExtendedModules = new List<string>();
            foreach (string dll in lbExtendedModules.SelectedItems)
            {
                selectedExtendedModules.Add(dll);
            }
            List<string> selectedCustomModules = new List<string>();
            foreach (string dll in lbCustomModules.SelectedItems)
            {
                selectedCustomModules.Add(dll);
            }

            Thread installBuild = new Thread(() => InstallBuild(installPath, selectedExtendedModules, selectedCustomModules, checkLaunchAfterInstall.Checked, checkInstallFolder.Checked, checkRunDatabaseUpdate.Checked, checkResetDBVersion.Checked));
            installBuild.Start();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbConfigurationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConfigurationList.Text == "None")
            {
                lbExtendedModules.ClearSelected();
                lbCustomModules.ClearSelected();
            }

            string prod = install.Product;
            string configName = cbConfigurationList.Text;
            LoadConfiguration(prod, configName);
            return;
        }

        private void LoadConfiguration(string product, string configurationName)
        {
            lbExtendedModules.ClearSelected();
            lbCustomModules.ClearSelected();
            List<string> extendedModules = new List<string>();
            List<string> customModules = new List<string>();
            List<Configurations> configurations = Configurations.GetConfigurationsByProduct(product);
            foreach (Configurations configuration in configurations)
            {
                if (configurationName == configuration.ConfigurationName)
                {
                    extendedModules = configuration.ExtendedModules;
                    customModules = configuration.CustomModules;
                    break;
                }
            }

            if (extendedModules != null)
                LoadConfigurationModules(extendedModules, lbExtendedModules, "Extended");

            if (customModules != null)
                LoadConfigurationModules(customModules, lbCustomModules, "Custom");
        }

        private void LoadConfigurationModules(List<string> modules, ListBox listBox, string type)
        {
            foreach (string dll in modules)
            {
                int index = listBox.FindStringExact(dll);
                try
                {
                    listBox.SetSelected(index, true);
                }
                catch
                {
                    MessageBox.Show(String.Format(@"The dll '{0}' does not exist as an {1} Modules for this build.", dll, type));
                }
            }
        }
    }
}
