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

        public string Product { get; set; }
        public string Version { get; set; }
        public string NetworkPath { get; set; }
        public string InstallerPath { get; set; }
        public string InstallLocation { get; set; }
        public List<string> ExtendedDLLs { get; set; }
        public List<string> CustomDLLs { get; set; }
        public bool LaunchAfterInstall { get; set; }
        public bool OpenInstallFolder { get; set; }
        public bool RunDatabaseUpdate { get; set; }
        public bool ResetDatabaseVersion { get; set; }

        public static string extModulesPath;
        public static string custModulesPath;

        public static string GetInstallerPath(string path, string product, string version)
        {
            string installerPath;
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            string initialDir = pi.FileserverDirectory;
            path = path.Replace("SP-FILESERV-01", "sp-fileserv-01");
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (product == Products.SalesPad)
                {
                    switch (version)
                    {
                        case "x64":
                        case "x86":
                            openFileDialog.Filter = String.Format("Executable Files (*.exe)|*{0}.exe", version);
                            break;
                        case "Pre":
                            openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                            break;
                    }
                }
                else if (product == Products.GPWeb)
                {
                    openFileDialog.Filter = "ZIP Folder (.zip)|*.zip";
                }
                else if (product == Products.WebAPI)
                {
                    openFileDialog.Filter = "Windows Installer Package (.msi)|*.msi";
                }
                else
                {
                    openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                }
                if (!Directory.Exists(path))
                {
                    string newPath = String.Format(@"{0}{1}", "\\", path);
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
                    if (path.Contains(initialDir))
                        openFileDialog.InitialDirectory = path;
                    else
                        openFileDialog.InitialDirectory = initialDir;
                }

                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = String.Format("Installing {0}", product);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    installerPath = openFileDialog.FileName;
                }
                else
                    installerPath = "EXIT";
            }
            return installerPath;
        }

        public void CalculateInstallPath()
        {
            SettingsModel settings = SettingsUtilities.GetSettings();

            int charCount = 0;
            string path = null;
            if (this.Product == Products.WebAPI)
            {
                tbInstallLocation.Text = @"C:\inetpub\wwwroot\SalesPadWebAPI";
                path = settings.BuildManagement.WebAPIDirectory;
            }
            else if (this.Product == Products.GPWeb)
            {
                tbInstallLocation.Text = @"C:\inetpub\wwwroot\SalesPadWebPortal";
                path = settings.BuildManagement.GPWebDirectory;
            }
            else
            {
                switch (this.Product)
                {
                    case Products.SalesPad:
                        charCount = 43;
                        switch (this.Version)
                        {
                            case "x64":
                                path = settings.BuildManagement.SalesPadx64Directory;
                                break;
                            case "x86":
                                path = settings.BuildManagement.SalesPadx86Directory;
                                break;
                        }
                        break;
                    case Products.DataCollection:
                        charCount = 51;
                        path = settings.BuildManagement.DataCollectionDirectory;
                        break;
                    case Products.SalesPadMobile:
                        charCount = 50;
                        path = settings.BuildManagement.SalesPadMobileDirectory;
                        break;
                    case Products.ShipCenter:
                        charCount = 42;
                        path = settings.BuildManagement.ShipCenterDirectory;
                        break;
                }
                tbInstallLocation.Text = GetInstallPath(path, this.NetworkPath, charCount);
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
            if (!String.IsNullOrWhiteSpace(extModulesPath))
                lbExtendedModules.Items.AddRange(Modules.RetrieveDLLs(extModulesPath, installerPath, product, installer, productVersion));
            if (!String.IsNullOrWhiteSpace(custModulesPath))
                lbCustomModules.Items.AddRange(Modules.RetrieveDLLs(custModulesPath, installerPath, product, installer, productVersion));
        }

        public void InstallGPWebBuild()
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
            string installerFileName = Path.GetFileName(this.InstallerPath);     //the actual file name without it's path
            string tempInstaller = String.Format(@"{0}\{1}", Utilities.GetFolder("Installers"), installerFileName);
            File.Copy(this.InstallerPath, tempInstaller, true);
        }

        public void InstallWebAPI()
        {
            this.Cursor = Cursors.WaitCursor;
            Form1.EnableInstallButton(false);
            Form1.EnableWaitCursor(true);
            string startTime = DateTime.Now.ToString();

            //remove files from insatll dir
            foreach (string file in Directory.GetFiles(this.InstallLocation))
                File.Delete(file);

            //remove folders from install dir
            foreach (string dir in Directory.GetDirectories(this.InstallLocation))
                Directory.Delete(dir, true);



            //copy zipped install file to Environment Manager/Installers
            //Unzip the install file
            //Copy the contents of the install file to the install path (excluding config.js)
        }

        public void InstallBuild()
        {
            //start the busy cursor
            this.Cursor = Cursors.WaitCursor;

            //Disable Install button on form1.
            Form1.EnableInstallButton(false);
            //Update cursor to use the waiting cursor
            Form1.EnableWaitCursor(true);

            string startTime = DateTime.Now.ToString();

            string installerFileName = Path.GetFileName(this.InstallerPath);     //the actual file name without it's path
            string tempInstaller = String.Format(@"{0}\{1}", Utilities.GetFolder("Installers"), installerFileName);
            File.Copy(this.InstallerPath, tempInstaller, true);

            //SILENTLY INSTALL PRODUCT
            Process installProduct = new Process();
            installProduct.StartInfo.FileName = tempInstaller;
            installProduct.StartInfo.Arguments = @"/S /D=" + this.InstallLocation;
            installProduct.Start();
            installProduct.WaitForExit();

            //WRITE BUILD INFORMATION TO INSTALLEDBUILD TABLE
            BuildModel build = new BuildModel(this.NetworkPath, this.Version, startTime, this.Product, this.InstallLocation);
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
            foreach (string dll in this.CustomDLLs)
            {
                custDllToAdd.Add(dll);
            }
            custDllToAdd.Sort();

            foreach (string dll in this.ExtendedDLLs)
            {
                extDllToAdd.Add(dll);
            }
            extDllToAdd.Sort();

            if (custDllToAdd.Count > 0 || extDllToAdd.Count > 0)
            {
                Modules.GetDLLs(this.Product, this.NetworkPath, this.Version, startTime, custDllToAdd, extDllToAdd);
            }

            //  UNZIP DLLS IF THERE ARE ANY
            if (custDllToAdd.Count > 0 || extDllToAdd.Count > 0)
            {
                if (this.Product == Products.DataCollection)
                {
                    Modules.CopyDllsToInstalledBuild(this.InstallLocation);
                }
                else
                {
                    Modules.UnzipDLLFiles();
                    Modules.CopyDllsFromDirectoriesToInstalledBuild(this.InstallLocation);
                }
            }
            //==========================================================================================================================================================================================
            //  DLL INSTALL END
            //==========================================================================================================================================================================================

            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (this.ResetDatabaseVersion)
                DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)), settingsModel.DbManagement.DBToRestore);

            if (this.RunDatabaseUpdate)
            {
                //if log exists, delete log
                ErrorHandling.DeleteLogFiles();

                if (!this.ResetDatabaseVersion)
                    DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)), settingsModel.DbManagement.DBToRestore);

                Process dbUpdate = new Process();
                dbUpdate.StartInfo.FileName = this.InstallLocation + @"\SalesPad.exe";
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

            //Write the install properties file to the install path.
            InstallProperties ip = new InstallProperties();
            ip.Product = this.Product;
            ip.Version = this.Version;
            ip.CustomDLLs = InstallProperties.GetDLLList(this.CustomDLLs, this.NetworkPath, true, this.Product, this.Version);
            ip.ExtendedDLLs = InstallProperties.GetDLLList(this.ExtendedDLLs, this.NetworkPath, false, this.Product, this.Version);
            ip.BuildPath = this.NetworkPath;
            ip.InstallPath = this.InstallLocation;
            ip.WritePropertiesFile();

            if (this.OpenInstallFolder)
                build.LaunchInstalledFolder();

            if (this.LaunchAfterInstall)
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

        private void ConfigureLoadForm()
        {
            RegUtilities.CheckForInstallRegistryEntries();
            RegistryEntries reg = new RegistryEntries();
            reg._product = this.Product;
            checkLaunchAfterInstall.Checked = bool.Parse(reg.LaunchAfterInstall);
            checkInstallFolder.Checked = bool.Parse(reg.OpenInstallFolder);
            checkRunDatabaseUpdate.Checked = bool.Parse(reg.RunDatabaseUpdate);
            checkResetDBVersion.Checked = bool.Parse(reg.ResetDatabaseVersion);
            string installerDirectory = this.NetworkPath;

            switch (this.Product)
            {
                case Products.SalesPad:
                    switch (this.Version)
                    {
                        case "x64":
                        case "x86":
                            extModulesPath = String.Format(@"{0}\ExtModules\{1}", installerDirectory, this.Version);
                            custModulesPath = String.Format(@"{0}\CustomModules\{1}", installerDirectory, this.Version);
                            break;
                        case "Pre":
                            extModulesPath = String.Format(@"{0}\ExtModules\WithOutCardControl", installerDirectory);
                            custModulesPath = String.Format(@"{0}\CustomModules\WithOutCardControl", installerDirectory);
                            break;
                    }
                    break;
                case Products.DataCollection:
                    lbExtendedModules.Enabled = false;
                    checkRunDatabaseUpdate.Enabled = false;
                    extModulesPath = null;
                    custModulesPath = String.Format(@"{0}\CustomModules", installerDirectory);
                    break;
                case Products.SalesPadMobile:
                    lbExtendedModules.Enabled = false;
                    lbCustomModules.Enabled = false;
                    checkRunDatabaseUpdate.Enabled = false;
                    cbConfigurationList.Enabled = false;
                    btnAddConfiguration.Enabled = false;
                    btnRemoveConfiguration.Enabled = false;
                    extModulesPath = null;
                    custModulesPath = null;
                    break;
                case Products.ShipCenter:
                    lbExtendedModules.Enabled = false;
                    checkRunDatabaseUpdate.Enabled = false;
                    extModulesPath = null;
                    custModulesPath = String.Format(@"{0}\Custom", installerDirectory);
                    break;
                case Products.WebAPI:
                    extModulesPath = String.Format(@"{0}\ExtModules", installerDirectory);
                    custModulesPath = String.Format(@"{0}\CustomModules", installerDirectory);
                    break;
                case Products.GPWeb:
                    lbExtendedModules.Enabled = false;
                    checkRunDatabaseUpdate.Enabled = false;
                    checkLaunchAfterInstall.Enabled = false;
                    checkResetDBVersion.Enabled = false;
                    extModulesPath = null;
                    custModulesPath = String.Format(@"{0}\plugins", installerDirectory);
                    break;
            }
        }

        private void SaveInstallOptions(bool launchAfterInstall, bool openInstallFolder, bool runDatabaseUpdate, bool resetDatabaseVersion)
        {
            RegistryEntries reg = new RegistryEntries();
            reg._product = this.Product;
            reg.LaunchAfterInstall = launchAfterInstall.ToString().ToLower();
            reg.OpenInstallFolder = openInstallFolder.ToString().ToLower();
            reg.RunDatabaseUpdate = runDatabaseUpdate.ToString().ToLower();
            reg.ResetDatabaseVersion = resetDatabaseVersion.ToString().ToLower();
        }

        private void Install_Load(object sender, EventArgs e)
        {
            ConfigureLoadForm();

            if (this.Product != Products.SalesPad)
            {
                checkResetDBVersion.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (this.Product == Products.SalesPadMobile)
            {
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            if (this.Product == Products.WebAPI || this.Product == Products.GPWeb)
            {
                tbInstallLocation.ReadOnly = true;
                checkLaunchAfterInstall.Enabled = false;
                checkLaunchAfterInstall.Checked = false;
            }
            this.Text = String.Format("Install {0}", this.Product);
            tbSelectedBuild.Text = this.NetworkPath;
            LoadModules(this.Product, this.NetworkPath, this.Version, this.InstallerPath);
            CalculateInstallPath();
            LoadConfigurations(this.Product);
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
                TextPrompt.product = this.Product;
                addConfiguration.FormClosing += new FormClosingEventHandler(AddConfigurationClose);
                addConfiguration.Show();
            }
            return;
        }

        private void AddConfigurationClose(object sender, FormClosingEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextPrompt.output))
                return;
            LoadConfigurations(this.Product);
        }

        private void btnRemoveConfiguration_Click(object sender, EventArgs e)
        {
            if (cbConfigurationList.Text == "Select a Configuration" || cbConfigurationList.Text == "None")
                return;

            Configurations configurationToDelete = new Configurations(this.Product,
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
                LoadConfigurations(this.Product);
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
                if (this.Product != Products.GPWeb || this.Product != Products.WebAPI)
                {
                    MessageBox.Show("Please enter an install location different than the default:\n\n" + installPath);
                    return;
                }
            }

            //prompt the user to delete existing installs if installing in a directory that already contains a build installation
            if (Directory.Exists(installPath))
            {
                if (this.Product != Products.GPWeb || this.Product != Products.WebAPI)
                {
                    string existsMessage = String.Format("{0} is already installed in the specified location, do you want to overwrite this install?", this.Product);
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
                        return;
                }
            }

            this.InstallLocation = installPath;
            this.ExtendedDLLs = SelectedModules(lbExtendedModules);
            this.CustomDLLs = SelectedModules(lbCustomModules);
            this.LaunchAfterInstall = checkLaunchAfterInstall.Checked;
            this.OpenInstallFolder = checkInstallFolder.Checked;
            this.RunDatabaseUpdate = checkRunDatabaseUpdate.Checked;
            this.ResetDatabaseVersion = checkResetDBVersion.Checked;

            //save the install options
            SaveInstallOptions(this.LaunchAfterInstall, this.OpenInstallFolder, this.RunDatabaseUpdate, this.ResetDatabaseVersion);

            this.Close();
            Thread installBuild = new Thread(() => InstallBuild());
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

            string prod = this.Product;
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
