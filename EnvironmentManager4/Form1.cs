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
    public partial class Form1 : Form
    {
        private static Form1 form = null;
        private delegate void EnableDelegate(bool enable);
        //https://www.py4u.net/discuss/717463

        public Form1()
        {
            InitializeComponent();
            form = this;
        }

        public const string dbDescLine1 = "===============================================================================";
        public const string dbDescLine2 = "=================== SELECTED DATABASE HAS NO DESCRIPTION ==================";
        public static string dbDescDefault = String.Format("{0}\n{0}\n{0}\n{0}\n{0}\n{1}\n{0}\n{0}\n{0}\n{0}\n{0}", dbDescLine1, dbDescLine2);
        public const string gpPath = @"C:\Program Files (x86)\Microsoft Dynamics\";
        public static List<string> productList = new List<string>
        {
            "SalesPad GP",
            "DataCollection",
            "SalesPad Mobile",
            "ShipCenter",
            "Customer Portal Web",
            "Customer Portal API"
        };
        public static List<string> versionList = new List<string>
        {
            "x86",
            "x64"
        };

        public static void EnableDBControls(bool enable)
        {
            if (form != null)
            {
                form.EnableButton(enable);
            }
        }

        private void EnableButton(bool enable)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(EnableButton), new object[] { enable });
                return;
            }
            btnRestoreDB.Enabled = enable;
            btnOverwriteDB.Enabled = enable;
            btnNewDB.Enabled = enable;
            btnDeleteBackup.Enabled = enable;
            cbDatabaseList.Enabled = enable;
            if (enable)
            {
                Reload();
            }
        }

        public static void EnableInstallButton(bool enable)
        {
            if (form != null)
            {
                form.EnableInstall(enable);
            }
        }

        private void EnableInstall(bool enable)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(EnableInstall), new object[] { enable });
                return;
            }
            btnInstallProduct.Enabled = enable;
        }

        public void Reload()
        {
            cbDatabaseList.Text = "Select a Database Backup";
            LoadDatabaseList();
            LoadDatabaseDescription(cbDatabaseList.Text);
        }

        private void EnableSQLControls(bool enable)
        {
            btnStartService.Enabled = enable;
            btnStopService.Enabled = enable;
            btnStopAllServices.Enabled = enable;
        }

        public void LoadDatabaseList()
        {
            cbDatabaseList.Items.Clear();
            cbDatabaseList.Text = "Select a Database Backup";
            LoadDatabaseDescription(cbDatabaseList.Text);
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            if (String.IsNullOrWhiteSpace(settingsModel.DbManagement.DatabaseBackupDirectory))
            {
                MessageBox.Show("There is no value in the Database Backup Directory Setting. Please set one in Settings.");
                return;
            }
            if (!Directory.Exists(settingsModel.DbManagement.DatabaseBackupDirectory))
            {
                MessageBox.Show(String.Format("The provided database backup directory '{0}' doesn't exist."), settingsModel.DbManagement.DatabaseBackupDirectory);
                return;
            }
            var databases = Directory.GetFiles(settingsModel.DbManagement.DatabaseBackupDirectory).Select(file => Path.GetFileNameWithoutExtension(file));
            cbDatabaseList.Items.AddRange(databases.ToArray());
        }

        public void LoadDatabaseDescription(string backup)
        {
            tbDBDesc.Clear();
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            if (backup == "Select a Database Backup")
            {
                tbDBDesc.Text = dbDescDefault;
            }
            else
            {
                string zipPath = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, backup);

                try
                {
                    using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                        {
                            ZipArchiveEntry descEntry = archive.GetEntry("Description.txt");
                            using (StreamReader reader = new StreamReader(descEntry.Open()))
                            {
                                tbDBDesc.Text = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("There was an error loading the description file for the '{0}' database backup file. The error is as follows:\n\n{1}", backup, e.Message));
                    tbDBDesc.Text = dbDescDefault;
                }
            }
        }

        public void LoadGPInstalls()
        {
            lbGPVersionsInstalled.Items.Clear();
            var gpFolderList = Directory.GetDirectories(gpPath).Select(folder => folder.Remove(0, gpPath.Length));
            lbGPVersionsInstalled.Items.AddRange(gpFolderList.ToArray());
        }

        public void LoadSQLServerListView()
        {
            lvInstalledSQLServers.Items.Clear();
            List<string> sqlServices = SQLServices.InstalledSQLServerInstanceNames();
            foreach (string sqlService in sqlServices)
            {
                bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(sqlService));
                string serverStatus = "";
                ListViewItem item = new ListViewItem(sqlService);
                switch (status)
                {
                    case true:
                        item.ForeColor = Color.Green;
                        item.Font = new Font(this.Font, FontStyle.Bold);
                        serverStatus = "RUNNING";
                        break;
                    case false:
                        item.ForeColor = Color.Gray;
                        item.Font = new Font(this.Font, FontStyle.Italic);
                        serverStatus = "NOT RUNNING";
                        break;
                }
                item.SubItems.Add(serverStatus);
                lvInstalledSQLServers.Items.Add(item);
            }
        }

        private void RemoveSalesPad(string x86Path, string x64Path)
        {
            if (!String.IsNullOrWhiteSpace(x86Path) && Directory.Exists(x86Path))
            {
                string[] foldersx86 = Directory.GetDirectories(x86Path);
                foreach (string dir in foldersx86)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex86)
                    {
                        MessageBox.Show(String.Format("The following build could not be deleted. It may be running.\n\n{0}\n\nThe error is as follows:{1}\n\n", dir, ex86));
                    }
                }
            }
            if (!String.IsNullOrWhiteSpace(x64Path) && Directory.Exists(x64Path))
            {
                string[] foldersx64 = Directory.GetDirectories(x64Path);
                foreach (string dir in foldersx64)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex64)
                    {
                        MessageBox.Show(String.Format("The following build could not be deleted. It may be running.\n\n{0}\n\nThe error is as follows:\n\n{1}", dir, ex64));
                    }
                }
            }
        }

        private void RemoveOtherProducts(string product)
        {
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string buildPath = "";
            switch (product)
            {
                case "DataCollection":
                    buildPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "SalesPad Mobile":
                    buildPath = settingsModel.BuildManagement.SalesPadMobileDirectory;
                    break;
                case "ShipCenter":
                    buildPath = settingsModel.BuildManagement.ShipCenterDirectory;
                    break;
                case "Customer Portal Web":
                    buildPath = settingsModel.BuildManagement.GPWebDirectory;
                    break;
                case "Customer Portal API":
                    buildPath = settingsModel.BuildManagement.WebAPIDirectory;
                    break;
            }

            string message = String.Format("Are you sure you want to delete all of your {0} builds?", product);
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                if (!String.IsNullOrWhiteSpace(buildPath) && Directory.Exists(buildPath))
                {
                    string[] folders = Directory.GetDirectories(buildPath);
                    foreach (string dir in folders)
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                        }
                        catch (Exception exPath)
                        {
                            MessageBox.Show(String.Format("The following build could not be deleted. It may be running.\n\n{0}\n\nThe error is as follows:\n\n{1}", dir, exPath));
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Environment.MachineName != "STEVERODRIGUEZ")
            {
                notesToolStripMenuItem.Visible = false;
                directoryCompareToolStripMenuItem.Visible = false;
                trimSOLTickets.Visible = false;
                generateSettingsFileToolStripMenuItem.Visible = false;
                generateCoreModulesFileToolStripMenuItem.Visible = false;
                generateConfigurationsFileToolStripMenuItem.Visible = false;
                generateConfigurationsFileWithNullsToolStripMenuItem.Visible = false;
            }
            Reload();
            LoadGPInstalls();
            tbWiFiIPAddress.Text = Utilities.GetWiFiIPAddress();
            tbSPVPNIPAddress.Text = Utilities.GetSalesPadVPNIPAddress();
            LoadSQLServerListView();
            return;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.FormClosing += new FormClosingEventHandler(SettingsClose);
            settings.Show();
            return;
        }

        private void SettingsClose(object sender, FormClosingEventArgs e)
        {
            Reload();
        }

        private void labelGPInstallationList_Click(object sender, EventArgs e)
        {
            LoadGPInstalls();
        }

        private void btnLaunchSelectedGP_Click(object sender, EventArgs e)
        {
            string selectedGPFolder = lbGPVersionsInstalled.Text;
            if (String.IsNullOrWhiteSpace(selectedGPFolder))
            {
                return;
            }
            if (Control.ModifierKeys == Keys.Shift)
            {
                try
                {
                    Process.Start(gpPath + selectedGPFolder);
                }
                catch
                {
                    MessageBox.Show("There was an error launching the installed GP folder.");
                }
                return;
            }
            string selectedGP = lbGPVersionsInstalled.Text;
            Process.Start(gpPath + selectedGP + "\\Dynamics.exe", "\"" + gpPath + selectedGP + "\\DYNAMICS.SET\"");
            return;
        }

        private void btnLaunchGPUtils_Click(object sender, EventArgs e)
        {
            string selectedGPFolder = lbGPVersionsInstalled.Text;
            if (String.IsNullOrWhiteSpace(selectedGPFolder))
            {
                return;
            }
            if (Control.ModifierKeys == Keys.Shift)
            {
                try
                {
                    Process.Start(@"C:\Program Files\Microsoft SQL Server\MSSQL13.SQLSERVER2016\MSSQL\Backup");
                }
                catch
                {
                    MessageBox.Show("There was an error launching the Dynamics Database backup folder.");
                }
                return;
            }
            string selectedGP = lbGPVersionsInstalled.Text;
            Process.Start(gpPath + selectedGP + "\\DynUtils.exe", "\"" + gpPath + selectedGP + "\\DYNUTILS.SET\"");
            return;
        }

        private void btnInstallGP_Click(object sender, EventArgs e)
        {
            //InstallGP();
            return;
        }

        private void labelSQLVersions_Click(object sender, EventArgs e)
        {
            LoadSQLServerListView();
            return;
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count > 0)
            {
                EnableSQLControls(false);
                string selectedService = lvInstalledSQLServers.SelectedItems[0].Text;
                bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(selectedService));
                if (!status)
                {
                    SQLServices.StartSQLServer(SQLServices.FormatServiceName(selectedService));
                }
                LoadSQLServerListView();
                EnableSQLControls(true);
            }
            return;
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count > 0)
            {
                EnableSQLControls(false);
                string selectedService = lvInstalledSQLServers.SelectedItems[0].Text;
                bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(selectedService));
                if (status)
                {
                    SQLServices.StopSQLServer(SQLServices.FormatServiceName(selectedService));
                }
                LoadSQLServerListView();
                EnableSQLControls(true);
            }
            return;
        }

        private void btnInstallService_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnStopAllServices_Click(object sender, EventArgs e)
        {
            List<string> installedSQLServices = SQLServices.InstalledSQLServerInstanceNames();
            if (installedSQLServices.Count != 0)
            {
                EnableSQLControls(false);
                foreach (string service in installedSQLServices)
                {
                    bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(service));
                    if (status)
                    {
                        SQLServices.StopSQLServer(SQLServices.FormatServiceName(service));
                    }
                }
                LoadSQLServerListView();
                EnableSQLControls(true);
            }
            return;
        }

        private void btnDBBackupFolder_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to open the database backup folder?";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
                Process.Start(settingsModel.DbManagement.DatabaseBackupDirectory);
            }
            return;
        }

        private void btnRestoreDB_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string backupZip = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, backupName);
            bool continueRestore = DatabaseManagement.PreDatabaseActionValidation(backupName, backupZip, "Restore");
            if (continueRestore)
            {
                string message = String.Format("Are you sure you want to restore the backup '{0}' over your current environment?", backupName);
                string caption = "CONFIRM";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    Thread restoreBackup = new Thread(() => DatabaseManagement.RestoreDatabase(backupName, backupZip));
                    restoreBackup.Start();
                }
            }
            return;
        }

        private void btnOverwriteDB_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string backupZip = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, backupName);
            bool continueOverwrite = DatabaseManagement.PreDatabaseActionValidation(backupName, backupZip, "Overwrite");
            if (continueOverwrite)
            {
                string message = String.Format(@"Are you sure you want to overwrite the selected backup '{0}'? This action cannot be undone.", backupName);
                string caption = "OVERWRITE?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    NewDatabaseBackup newBackup = new NewDatabaseBackup();
                    NewDatabaseBackup.existingDatabaseName = backupName;
                    NewDatabaseBackup.existingDatabaseFile = backupZip;
                    NewDatabaseBackup.action = "OVERWRITE";
                    newBackup.Show();
                }
            }
            return;
        }

        private void btnNewDB_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to create a new Database Backup?";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                NewDatabaseBackup newBackup = new NewDatabaseBackup();
                NewDatabaseBackup.action = "NEW";
                newBackup.Show();
            }
            return;
        }

        private void btnDeleteBackup_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string backupZip = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, backupName);
            bool continueDelete = DatabaseManagement.PreDatabaseActionValidation(backupName, backupZip, "Delete");
            if (continueDelete)
            {
                string message = String.Format(@"Are you sure you want to delete the selected backup '{0}'? This action cannot be undone.", backupName);
                string caption = "DELETE?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    DatabaseManagement.DeleteDatabase(backupName, backupZip, true, true);
                    Reload();
                }
            }
            return;
        }

        private void btnInstallProduct_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                BuildLog buildLog = new BuildLog();
                buildLog.Show();
                return;
            }
            string selectedProduct = cbProductList.Text;
            string selectedVersion = cbSPGPVersion.Text;
            if (!productList.Contains(selectedProduct))
            {
                MessageBox.Show("Please select a product from the list to continue.");
                return;
            }
            if (!versionList.Contains(selectedVersion))
            {
                MessageBox.Show("Please select a version from the list to continue.");
                return;
            }
            Install.InstallProduct(selectedProduct, selectedVersion);
            return;
        }

        private void btnLaunchProduct_Click(object sender, EventArgs e)
        {
            string selectedProduct = cbProductList.Text;
            string selectedVersion = cbSPGPVersion.Text;

            if (!productList.Contains(selectedProduct))
            {
                string message = "Please select a product from the list.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
                return;
            }

            if (!versionList.Contains(selectedVersion))
            {
                string message = "Please select a version from the list.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
                return;
            }

            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));

            if (Control.ModifierKeys == Keys.Shift)
            {
                string lastInstalledPath = SqliteDataAccess.LastInstalledBuild(selectedProduct);
                if (String.IsNullOrWhiteSpace(lastInstalledPath))
                {
                    MessageBox.Show(String.Format("There isn't a last recorded build for the selected product '{0}'", selectedProduct));
                    return;
                }
                lastInstalledPath += String.Format(@"\{0}", Utilities.RetrieveExe(selectedProduct));

                string message = String.Format("Are you sure you want to launch {0}?", lastInstalledPath);
                string caption = "CONFIRM";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Process.Start(lastInstalledPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format("There was an error launching the build listed below\n\n{0}\n\n{1}\n\n{2}", lastInstalledPath, ex.Message, ex.ToString()));
                        return;
                    }
                }
                return;
            }

            string productPath = "";
            switch (selectedProduct)
            {
                case "SalesPad GP":
                    switch (selectedVersion)
                    {
                        case "x86":
                            productPath = settingsModel.BuildManagement.SalesPadx86Directory;
                            break;
                        case "x64":
                            productPath = settingsModel.BuildManagement.SalesPadx64Directory;
                            break;
                    }
                    break;
                case "DataCollection":
                    productPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "Inventory Manager":
                    productPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "SalesPad Mobile":
                    productPath = settingsModel.BuildManagement.SalesPadMobileDirectory;
                    break;
                case "ShipCenter":
                    productPath = settingsModel.BuildManagement.ShipCenterDirectory;
                    break;
            }

            if (!Directory.Exists(productPath))
            {
                MessageBox.Show(String.Format("The Settings defined path for '{0}', '{1}' does not exist. There are either no builds to launch, or Settings needs reconfigured.", selectedProduct, productPath));
                return;
            }

            LaunchProduct launch = new LaunchProduct(); ;
            LaunchProduct.product = selectedProduct;
            LaunchProduct.version = selectedVersion;
            LaunchProduct.path = productPath;
            launch.Show();
            return;
        }

        private void btnDLLManager_Click(object sender, EventArgs e)
        {

        }

        private void btnBuildFolder_Click(object sender, EventArgs e)
        {
            string product = cbProductList.Text;
            string version = cbSPGPVersion.Text;
            string buildPath = "";
            if (!productList.Contains(product))
            {
                string errorMessage = "Please select a Product.";
                string errorCaption = "ERROR";
                MessageBoxButtons errorButton = MessageBoxButtons.OK;
                MessageBoxIcon errorIcon = MessageBoxIcon.Error;

                MessageBox.Show(errorMessage, errorCaption, errorButton, errorIcon);
                return;
            }
            if (!versionList.Contains(version))
            {
                string message = "Please select a version from the list.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
                return;
            }
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            switch (product)
            {
                case "SalesPad GP":
                    switch (version)
                    {
                        case "x86":
                            buildPath = settingsModel.BuildManagement.SalesPadx86Directory;
                            break;
                        case "x64":
                            buildPath = settingsModel.BuildManagement.SalesPadx64Directory;
                            break;
                    }
                    break;
                case "DataCollection":
                    buildPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "SalesPad Mobile":
                    buildPath = settingsModel.BuildManagement.SalesPadMobileDirectory;
                    break;
                case "ShipCenter":
                    buildPath = settingsModel.BuildManagement.ShipCenterDirectory;
                    break;
            }

            if (!Directory.Exists(buildPath))
            {
                MessageBox.Show(String.Format("The Settings defined path for '{0}', '{1}' does not exist. There are either no builds to launch, or Settings needs reconfigured.", product, buildPath));
                return;
            }

            try
            {
                Process.Start(buildPath);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("There was an error opening the build folder '{0}', error below:\n\n{1}\n\n{2}", buildPath, ex.Message, ex.ToString()));
                return;
            }
        }

        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAlwaysOnTop.Checked == true)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
            return;
        }

        private void labelReloadVPNIPAddress_Click(object sender, EventArgs e)
        {
            tbSPVPNIPAddress.Text = Utilities.GetSalesPadVPNIPAddress();
            return;
        }

        private void labelReloadIPAddress_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                //ConfigModel configModel = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(Utilities.GetConfigurationsFile()));
                //foreach (var config in configModel.Configurations)
                //{
                //    MessageBox.Show(config.ToString());
                //}
            }
            tbWiFiIPAddress.Text = Utilities.GetWiFiIPAddress();
            return;
        }

        private void resetDatabaseVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListAndButtonForm listAndButtonForm = new ListAndButtonForm();
            ListAndButtonForm.title = "Select Database";
            ListAndButtonForm.button = "Reset Database Version";
            listAndButtonForm.FormClosing += new FormClosingEventHandler(ResetDBTextPromptClose);
            listAndButtonForm.Show();
        }

        private void ResetDBTextPromptClose(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ListAndButtonForm.output))
            {
                DatabaseManagement.ResetDatabaseVersion(ListAndButtonForm.output);
            }
            return;
        }

        private void salesPadDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string x86Path = settingsModel.BuildManagement.SalesPadx86Directory;
            string x64Path = settingsModel.BuildManagement.SalesPadx64Directory;

            string message = "Are you sure you want to delete all of your SalesPad GP builds?";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                Thread removeSPGP = new Thread(() => RemoveSalesPad(x86Path, x64Path));
                removeSPGP.Start();
            }
            return;
        }

        private void dataCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOtherProducts("DataCollection");
            return;
        }

        private void mobileSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOtherProducts("SalesPad Mobile");
            return;
        }

        private void shipCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOtherProducts("ShipCenter");
            return;
        }

        private void databaseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to open the Database Log?";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                DatabaseActivityLog dbLog = new DatabaseActivityLog();
                dbLog.Show();
            }
            return;
        }

        private void killSalesPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("SalesPad"))
            {
                process.Kill();
            }
            return;
        }

        private void notesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
        }

        private void directoryCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
        }

        private void trimSOLTickets_Click(object sender, EventArgs e)
        {
            //
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbDatabaseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDatabaseDescription(cbDatabaseList.Text);
        }

        private void generateSettingsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.CreateDefaultSettingsFile();
            return;
        }

        private void generateCoreModulesFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.CreateCoreModulesFile();
            return;
        }

        private void generateConfigurationsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.GenerateConfigs();
            return;
        }

        private void generateConfigurationsFileWithNullsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.GenerateConfigsWithNulls();
            return;
        }

        private void cbProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProduct = cbProductList.Text;
            if (selectedProduct == "SalesPad GP")
            {
                cbSPGPVersion.Enabled = true;
            }
            else
            {
                cbSPGPVersion.Text = "x86";
                cbSPGPVersion.Enabled = false;
            }
            return;
        }
    }
}
