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
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static Installer install = new Installer();

        public Installer GetInstallerFile(GetInstaller getInstaller)
        {
            ProductInfo pi = ProductInfo.GetProductInfo(getInstaller.Product, getInstaller.Version, true);
            string initialDir = pi.FileserverDirectory;
            string defaultInstallPath = pi.InstallDirectory;
            string buildPath = "";
            string installerPath = "";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (getInstaller.Product == Products.SalesPad)
                {
                    switch (getInstaller.Version)
                    {
                        case "x64":
                        case "x86":
                            openFileDialog.Filter = String.Format("Executable Files (*.exe)|*{0}.exe", getInstaller.Version);
                            break;
                        case "Pre":
                            openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                            break;
                    }
                }
                else if (getInstaller.Product == Products.GPWeb)
                {
                    openFileDialog.Filter = "ZIP Folder (.zip)|*.zip";
                }
                else if (getInstaller.Product == Products.WebAPI)
                {
                    openFileDialog.Filter = "Windows Installer Package (.msi)|*.msi";
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
                openFileDialog.Title = String.Format("Installing {0}", getInstaller.Product);

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
            int charCount = 0;
            if (install.Product == Products.WebAPI)
                tbInstallLocation.Text = @"C:\inetpub\wwwroot\SalesPadWebAPI";
            else if (install.Product == Products.GPWeb)
                tbInstallLocation.Text = @"C:\inetpub\wwwroot\SalesPadWebPortal";
            else
            {
                switch (install.Product)
                {
                    case Products.SalesPad:
                        charCount = 43;
                        break;
                    case Products.DataCollection:
                        charCount = 51;
                        break;
                    case Products.SalesPadMobile:
                        charCount = 50;
                        break;
                    case Products.ShipCenter:
                        charCount = 42;
                        break;
                }
                tbInstallLocation.Text = GetInstallPath(install.DefaultInstallPath, install.BuildPath, charCount);
            }
        }

        public static string GetInstallPath(string defaultPath, string installerPath, int charCount)
        {
            string pathFromInstaller = installerPath.Remove(0, charCount);
            //check if smartbear mode
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
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
            if (product == Products.DataCollection || product == Products.ShipCenter || product == Products.GPWeb || product == Products.SalesPadMobile)
            {
                lbExtendedModules.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (product == Products.SalesPadMobile)
            {
                lbCustomModules.Enabled = false;
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            switch (product)
            {
                case Products.SalesPad:
                    switch (productVersion)
                    {
                        case "x64":
                        case "x86":
                            extModulesPath = String.Format(@"{0}\ExtModules\{1}", installerPath, productVersion);
                            custModulesPath = String.Format(@"{0}\CustomModules\{1}", installerPath, productVersion);
                            break;
                        case "Pre":
                            extModulesPath = String.Format(@"{0}\ExtModules\WithOutCardControl", installerPath);
                            custModulesPath = String.Format(@"{0}\CustomModules\WithOutCardControl", installerPath);
                            break;
                    }
                    break;
                case Products.WebAPI:
                    extModulesPath = String.Format(@"{0}\ExtModules", installerPath);
                    custModulesPath = String.Format(@"{0}\CustomModules", installerPath);
                    break;
                case Products.DataCollection:
                    custModulesPath = String.Format(@"{0}\CustomModules", installerPath);
                    break;
                case Products.SalesPadMobile:
                    break;
                case Products.ShipCenter:
                    custModulesPath = String.Format(@"{0}\Custom", installerPath);
                    break;
                case Products.GPWeb:
                    custModulesPath = String.Format(@"{0}\Plugins", installerPath);
                    break;
            }
            if (!String.IsNullOrWhiteSpace(extModulesPath))
            {
                lbExtendedModules.Items.AddRange(Modules.RetrieveDLLs(extModulesPath, installerPath, product, installer, productVersion));
            }
            lbCustomModules.Items.AddRange(Modules.RetrieveDLLs(custModulesPath, installerPath, product, installer, productVersion));
        }

        public void InstallGPWebBuild(string installPath)
        {
            //start the busy cursor
            this.Cursor = Cursors.WaitCursor;

            //Disable Install button on form1.
            Form1.EnableInstallButton(false);
            //Update cursor to use the waiting cursor
            Form1.EnableWaitCursor(true);

            string startTime = DateTime.Now.ToString();

            //global variable installer = the installer file path
            //global variable installerpath = the path to the installer, excluding the file name
            string installerFileName = Path.GetFileName(install.InstallerPath);     //the actual file name without it's path
            string tempInstaller = String.Format(@"{0}\{1}", Utilities.GetFolder("Installers"), installerFileName);
            File.Copy(install.InstallerPath, tempInstaller, true);
        }

        public void InstallBuild(string installPath, List<string> extendedModules, List<string> customModules, bool launchAfterInstall, bool openInstallFolder, bool runDatabaseUpdate, bool resetDatabaseVersion)
        {
            //start the busy cursor
            this.Cursor = Cursors.WaitCursor;

            //Disable Install button on form1.
            Form1.EnableInstallButton(false);
            //Update cursor to use the waiting cursor
            Form1.EnableWaitCursor(true);

            string startTime = DateTime.Now.ToString();

            //global variable installer = the installer file path
            //global variable installerpath = the path to the installer, excluding the file name
            string installerFileName = Path.GetFileName(install.InstallerPath);     //the actual file name without it's path
            string tempInstaller = String.Format(@"{0}\{1}", Utilities.GetFolder("Installers"), installerFileName);
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
                ErrorHandling.DisplayExceptionMessage(e);
            }

            try
            {
                File.Delete(tempInstaller);
            }
            catch (Exception deleteError)
            {
                ErrorHandling.LogException(deleteError);
                ErrorHandling.DisplayExceptionMessage(deleteError);
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
                if (install.Product == Products.DataCollection)
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

            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (resetDatabaseVersion)
                DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)), settingsModel.DbManagement.DBToRestore);

            if (runDatabaseUpdate)
            {
                //if log exists, delete log
                ErrorHandling.DeleteLogFiles();

                if (!resetDatabaseVersion)
                    DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)), settingsModel.DbManagement.DBToRestore);

                Process dbUpdate = new Process();
                dbUpdate.StartInfo.FileName = installPath + @"\SalesPad.exe";
                dbUpdate.StartInfo.Arguments = String.Format(@"/dbUpdate /userfields /conn={0}", settingsModel.DbManagement.DBToRestore);
                dbUpdate.StartInfo.UseShellExecute = false;
                try
                {
                    dbUpdate.Start();
                    dbUpdate.WaitForExit();
                    //check for pass/fail log
                    if (ErrorHandling.IsThereAFailLog())
                    {
                        ErrorHandling.DisplayDatabaseUpdateFailure();
                        ErrorHandling.LogDatabaseUpdateFailure();
                    }
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                }
            }
            this.Cursor = Cursors.Default;

            if (openInstallFolder)
                build.LaunchInstalledFolder();

            if (launchAfterInstall)
                build.LaunchBuild();

            Form1.EnableInstallButton(true);
            Form1.EnableWaitCursor(false);
        }

        private void LoadConfigurations(string product)
        {
            cbConfigurationList.Items.Clear();
            cbConfigurationList.Text = "Select a Configuration";
            cbConfigurationList.Items.Add("None");
            List<Configurations> configurations = Configurations.GetConfigurations();
            foreach (Configurations config in configurations)
                if (config.Product == product)
                    cbConfigurationList.Items.Add(config.ConfigurationName);
        }

        private List<string> SelectedModules(ListBox lb)
        {
            List<string> selectedModules = new List<string>();
            foreach (string dll in lb.SelectedItems)
                selectedModules.Add(dll);
            return selectedModules;
        }

        private void InstallOptionsOnLoad()
        {
            RegUtilities.CheckForInstallRegistryEntries();
            RegistryEntries reg = new RegistryEntries();
            reg._product = install.Product;
            if (reg.LaunchAfterInstall == "true")
                checkLaunchAfterInstall.Checked = true;
            else
                checkLaunchAfterInstall.Checked = false;

            if (reg.OpenInstallFolder == "true")
                checkInstallFolder.Checked = true;
            else
                checkInstallFolder.Checked = false;

            if (reg.RunDatabaseUpdate == "true")
                checkRunDatabaseUpdate.Checked = true;
            else
                checkRunDatabaseUpdate.Checked = false;

            if (reg.ResetDatabaseVersion == "true")
                checkResetDBVersion.Checked = true;
            else
                checkResetDBVersion.Checked = false;
        }

        private void SaveInstallOptions(bool launchAfterInstall, bool openInstallFolder, bool runDatabaseUpdate, bool resetDatabaseVersion)
        {
            RegistryEntries reg = new RegistryEntries();
            reg._product = install.Product;
            reg.LaunchAfterInstall = launchAfterInstall.ToString().ToLower();
            reg.OpenInstallFolder = openInstallFolder.ToString().ToLower();
            reg.RunDatabaseUpdate = runDatabaseUpdate.ToString().ToLower();
            reg.ResetDatabaseVersion = resetDatabaseVersion.ToString().ToLower();
        }

        private void Install_Load(object sender, EventArgs e)
        {
            InstallOptionsOnLoad();

            if (install.Product != Products.SalesPad)
            {
                checkResetDBVersion.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (install.Product == Products.SalesPadMobile)
            {
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            if (install.Product == Products.WebAPI || install.Product == Products.GPWeb)
            {
                tbInstallLocation.ReadOnly = true;
                checkLaunchAfterInstall.Enabled = false;
                checkLaunchAfterInstall.Checked = false;
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
                return;
            LoadConfigurations(install.Product);
        }

        private void btnRemoveConfiguration_Click(object sender, EventArgs e)
        {
            if (cbConfigurationList.Text == "Select a Configuration" || cbConfigurationList.Text == "None")
                return;

            Configurations configurationToDelete = new Configurations(install.Product,
                cbConfigurationList.Text,
                SelectedModules(lbExtendedModules),
                SelectedModules(lbCustomModules));

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
            //retrieve settings values from settings file
            SettingsModel settingsModel = SettingsUtilities.GetSettings();

            //get a list of default install paths to prevent the user from installing in the base install directory
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

            //clear out any saved installers from previous installations
            DirectoryInfo di = new DirectoryInfo(Utilities.GetFolder("Installers"));
            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            //check if the selected install path is a base install path - reject
            string installPath = tbInstallLocation.Text;
            if (defaultPaths.Contains(installPath))
            {
                MessageBox.Show("Please enter an install location different than the default:\n\n" + installPath);
                return;
            }

            //prompt the user to delete existing installs if installing in a directory that already contains a build installation
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
                    try
                    {
                        Directory.Delete(installPath, true);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandling.LogException(ex);
                        ErrorHandling.DisplayExceptionMessage(ex);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            //save the install options
            SaveInstallOptions(checkLaunchAfterInstall.Checked, checkInstallFolder.Checked, checkRunDatabaseUpdate.Checked, checkResetDBVersion.Checked);

            this.Close();

            //add any selected custom/extended modules to the install
            List<string> selectedExtendedModules = new List<string>();
            foreach (string dll in lbExtendedModules.SelectedItems)
                selectedExtendedModules.Add(dll);

            List<string> selectedCustomModules = new List<string>();
            foreach (string dll in lbCustomModules.SelectedItems)
                selectedCustomModules.Add(dll);

            //run the installation
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
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                }
            }
        }

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form1.s_InstallBuild = null;
        }
    }
}
