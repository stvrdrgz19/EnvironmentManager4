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
using System.Reflection;
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
        //https://www.codegrepper.com/code-examples/csharp/c%23+edit+form+controls+from+another+class

        public Form1()
        {
            InitializeComponent();
            form = this;
        }

        public const string dbDescLine1 = "===============================================================================";
        public const string dbDescLine2 = "=================== SELECTED DATABASE HAS NO DESCRIPTION ==================";
        public static string dbDescDefault = String.Format("{0}\n{0}\n{0}\n{0}\n{0}\n{1}\n{0}\n{0}\n{0}\n{0}\n{0}", dbDescLine1, dbDescLine2);
        public const string gpPath = @"C:\Program Files (x86)\Microsoft Dynamics\";
        public static string newDBBackupName = "test1";
        public static LaunchProduct launch;
        public static UpdateDatabaseDescription udd;
        public static Install installBuild;
        public static BuildLog buildLog;
        public static Settings settingsFormOpen;
        public static ListAndButtonForm listAndButtonForm;
        public static DeleteBuilds deleteBuilds;
        public static DatabaseActivityLog dbLog;
        public static Notes notes;
        public static About aboutForm;
        public static NewDatabaseBackup newBackup;
        public static NewDatabaseBackup overwriteBackup;

        public static void EnableDBControls(bool enable)
        {
            if (form != null)
            {
                form.EnableButton(enable);
            }
            if (enable)
            {
                //form.Cursor = Cursors.Default;
            }
            if (!enable)
            {
                //form.Cursor = Cursors.WaitCursor;
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
        
        public void SettingsReload(bool settingsChange = false)
        {
            cbDatabaseList.Text = "Select a Database Backup";
            LoadDatabaseList();
            LoadDatabaseDescription(cbDatabaseList.Text);
            if (settingsChange)
            {
                DetermineMode();
            }
        }

        public void Reload(bool settingsChange = false)
        {
            if (settingsChange)
            {
                DetermineMode();
            }
        }

        public static void SetStaticBackup(bool enable)
        {
            if (form != null)
                form.SetSelectedBackup(enable);
        }

        public void SetSelectedBackup(bool enable)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(SetSelectedBackup), new object[] { enable });
                return;
            }
            SettingsReload();
            cbDatabaseList.SelectedIndex = cbDatabaseList.FindStringExact(newDBBackupName);
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
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (String.IsNullOrWhiteSpace(settingsModel.DbManagement.DatabaseBackupDirectory))
            {
                MessageBox.Show("There is no value in the Database Backup Directory Setting. Please set one in Settings.");
                return;
            }
            if (!Directory.Exists(settingsModel.DbManagement.DatabaseBackupDirectory))
            {
                MessageBox.Show(String.Format("The provided database backup directory '{0}' doesn't exist.", settingsModel.DbManagement.DatabaseBackupDirectory));
                return;
            }
            var databases = Directory.GetFiles(settingsModel.DbManagement.DatabaseBackupDirectory).Select(file => Path.GetFileNameWithoutExtension(file));
            cbDatabaseList.Items.AddRange(databases.ToArray());
        }

        public void LoadDatabaseDescription(string backup)
        {
            tbDBDesc.Clear();
            if (backup == "Select a Database Backup")
            {
                tbDBDesc.Text = dbDescDefault;
            }
            else
            {
                try
                {
                    tbDBDesc.Text = Utilities.GetDatabaseDescription(backup);
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
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
            List<string> services = SQLServices.InstalledSQLServerInstanceNames();
            services.AddRange(SQLServices.GetSalesPadServices());
            bool status;
            string serverStatus = "";
            foreach (string service in services)
            {
                if (SQLServices.IsSQLService(service))
                    status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(service));
                else
                    status = SQLServices.IsServiceRunning(service);
                ListViewItem item = new ListViewItem(service);
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
            Utilities.ResizeListViewColumnWidth(lvInstalledSQLServers, 6, 0);
        }

        public void DetermineMode()
        {
            LoadProductList();
            SettingsModel settings = SettingsUtilities.GetSettings();
            cbSPGPVersion.SelectedIndex = cbSPGPVersion.FindStringExact(settings.Other.DefaultVersion);
            cbAlwaysOnTop.Visible = settings.Other.ShowAlwaysOnTop;
            labelReloadVPNIPAddress.Visible = settings.Other.ShowVPNIP;
            tbSPVPNIPAddress.Visible = settings.Other.ShowVPNIP;
            labelReloadIPAddress.Visible = settings.Other.ShowIP;
            tbWiFiIPAddress.Visible = settings.Other.ShowIP;

            labelReloadVPNIPAddress.Location = new Point(89, 566);
            tbSPVPNIPAddress.Location = new Point(136, 563);

            if (settings.Other.ShowVPNIP || settings.Other.ShowIP)
                this.Size = new Size(536, 626);

            if (!settings.Other.ShowVPNIP && !settings.Other.ShowIP)
                this.Size = new Size(536, 605);

            if (settings.Other.ShowVPNIP && !settings.Other.ShowIP)
            {
                //starting VPNIPLabel POS   89, 566
                //starting VPNIPTextBox POS 136, 563
                //starting WifiIPLabel POS  339, 566
                //starting WifiTextBox POS  386, 563
                labelReloadVPNIPAddress.Location = new Point(339, 566);
                tbSPVPNIPAddress.Location = new Point(386, 563);
            }

            cbProductList.SelectedIndex = cbProductList.FindStringExact("SalesPad GP");
            switch (settings.Other.Mode)
            {
                case "Standard":
                    cbProductList.Enabled = true;
                    cbSPGPVersion.Enabled = true;
                    btnBuildFolder.Enabled = true;
                    break;
                case "Kyle":
                    cbProductList.Enabled = true;
                    cbSPGPVersion.Enabled = true;
                    btnBuildFolder.Enabled = false;
                    break;
                case "SmartBear":
                    cbSPGPVersion.SelectedIndex = cbSPGPVersion.FindStringExact("x86");
                    cbProductList.Enabled = false;
                    cbSPGPVersion.Enabled = false;
                    btnBuildFolder.Enabled = false;
                    break;
            }
        }

        private void LoadProductList()
        {
            cbProductList.Items.Clear();
            foreach (string product in Products.ListOfProducts())
            {
                cbProductList.Items.Add(product);
            }
            cbProductList.SelectedIndex = cbProductList.FindStringExact(Products.SalesPad);
        }

        private void LoadWifiIP()
        {
            tbWiFiIPAddress.Text = Utilities.GetIP("Wi-Fi");
        }

        private void LoadVPNIP()
        {
            tbSPVPNIPAddress.Text = Utilities.GetIP("SalesPad VPN");
        }

        private void ConfigureEnvironment(string machine)
        {
            labelVersion.Text = String.Format("v{0}", Utilities.GetAppVersion());
            if (machine != "STEVERODRIGUEZ")
            {
                notesToolStripMenuItem.Visible = false;
                directoryCompareToolStripMenuItem.Visible = false;
                trimSOLTickets.Visible = false;
                generateSettingsFileToolStripMenuItem.Visible = false;
                //generateCoreModulesFileToolStripMenuItem.Visible = false;
                //Utilities.UpdateEnvironment();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureEnvironment(Environment.MachineName);
            SettingsReload(true);
            LoadGPInstalls();
            LoadWifiIP();
            LoadVPNIP();
            LoadSQLServerListView();
            cbSPGPVersion.Enabled = false;
            LoadProductList();
            if (!Utilities.IsProgramUpToDate())
            {
                UpdatePrompt update = new UpdatePrompt();
                UpdatePrompt.OpenFromStartup = true;
                update.ShowDialog();
            }
            return;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingsFormOpen == null)
            {
                settingsFormOpen = new Settings();
                settingsFormOpen.FormClosing += new FormClosingEventHandler(SettingsClose);
                settingsFormOpen.Show();
            }
            else
                settingsFormOpen.BringToFront();
            return;
        }

        private void SettingsClose(object sender, FormClosingEventArgs e)
        {
            settingsFormOpen = null;
            SettingsReload(true);
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
                catch (Exception ex)
                {
                    ErrorHandling.LogException(ex);
                    ErrorHandling.DisplayExceptionMessage(ex);
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
                bool isSQLService = SQLServices.IsSQLService(selectedService);
                if (isSQLService)
                {
                    bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(selectedService));
                    if (!status)
                        SQLServices.StartSQLServer(SQLServices.FormatServiceName(selectedService));
                }
                else
                {
                    bool status = SQLServices.IsServiceRunning(selectedService);
                    if (!status)
                        SQLServices.StartSQLServer(selectedService);
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
                bool isSQLService = SQLServices.IsSQLService(selectedService);
                if (isSQLService)
                {
                    bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(selectedService));
                    if (status)
                        SQLServices.StopSQLServer(SQLServices.FormatServiceName(selectedService));
                }
                else
                {
                    bool status = SQLServices.IsServiceRunning(selectedService);
                    if (status)
                        SQLServices.StopSQLServer(selectedService);

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
            List<string> installedSQLServices = new List<string>();
            foreach (ListViewItem service in lvInstalledSQLServers.Items)
            {
                installedSQLServices.Add(service.Text);
            }
            if (installedSQLServices.Count != 0)
            {
                EnableSQLControls(false);
                foreach (string service in installedSQLServices)
                {
                    bool isSQLService = SQLServices.IsSQLService(service);
                    if (isSQLService)
                    {
                        bool status = SQLServices.IsServiceRunning(SQLServices.FormatServiceName(service));
                        if (status)
                            SQLServices.StopSQLServer(SQLServices.FormatServiceName(service));
                    }
                    else
                    {
                        bool status = SQLServices.IsServiceRunning(service);
                        if (status)
                            SQLServices.StopSQLServer(service);

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
                SettingsModel settingsModel = SettingsUtilities.GetSettings();
                Process.Start(settingsModel.DbManagement.DatabaseBackupDirectory);
            }
            return;
        }

        private void btnRestoreDB_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
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
            if (overwriteBackup == null)
            {
                string backupName = cbDatabaseList.Text;
                SettingsModel settingsModel = SettingsUtilities.GetSettings();
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
                        overwriteBackup = new NewDatabaseBackup();
                        NewDatabaseBackup.existingDatabaseName = backupName;
                        NewDatabaseBackup.existingDatabaseFile = backupZip;
                        NewDatabaseBackup.action = "OVERWRITE";
                        overwriteBackup.Show();
                    }
                }
            }
            else
                overwriteBackup.BringToFront();
            return;
        }

        private void btnNewDB_Click(object sender, EventArgs e)
        {
            if (newBackup == null)
            {
                string message = "Are you sure you want to create a new Database Backup?";
                string caption = "CONFIRM";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    newBackup = new NewDatabaseBackup();
                    NewDatabaseBackup.existingDatabaseName = null;
                    NewDatabaseBackup.existingDatabaseFile = null;
                    NewDatabaseBackup.action = "BACKUP";
                    newBackup.Show();
                }
            }
            else
                newBackup.BringToFront();
            return;
        }

        private void btnDeleteBackup_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
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
                    SettingsReload();
                }
            }
            return;
        }

        private void btnInstallProduct_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (buildLog == null)
                {
                    if (Products.ListOfProducts().Contains(cbProductList.Text))
                    {
                        buildLog = new BuildLog();
                        buildLog.Show();
                    }
                }
                else
                    buildLog.BringToFront();
                return;
            }
            if (installBuild == null)
            {
                string selectedProduct = cbProductList.Text;
                string selectedVersion = cbSPGPVersion.Text;
                if (!Products.ListOfProducts().Contains(selectedProduct))
                {
                    MessageBox.Show("Please select a product from the list to continue.");
                    return;
                }
                if (!Utilities.versionList.Contains(selectedVersion))
                {
                    MessageBox.Show("Please select a version from the list to continue.");
                    return;
                }

                installBuild = new Install();
                string path = Clipboard.GetText();
                GetInstaller getInstaller = new GetInstaller(path, selectedProduct, selectedVersion);
                Install.install = installBuild.GetInstallerFile(getInstaller);
                if (Install.install.InstallerPath != "EXIT")
                    installBuild.Show();
                else
                    installBuild = null;
            }
            else
                installBuild.BringToFront();
            return;
        }

        private void btnLaunchProduct_Click(object sender, EventArgs e)
        {
            if (launch == null)
            {
                string selectedProduct = cbProductList.Text;
                string selectedVersion = cbSPGPVersion.Text;

                if (!Products.ListOfProducts().Contains(selectedProduct))
                {
                    string message = "Please select a product from the list.";
                    string caption = "ERROR";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }

                if (!Utilities.versionList.Contains(selectedVersion))
                {
                    string message = "Please select a version from the list.";
                    string caption = "ERROR";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }

                if (Control.ModifierKeys == Keys.Shift)
                {
                    string lastInstalledPath = SqliteDataAccess.LastInstalledBuild(selectedProduct, selectedVersion);
                    if (String.IsNullOrWhiteSpace(lastInstalledPath))
                    {
                        MessageBox.Show(String.Format("There isn't a last recorded build for the selected product '{0}'", selectedProduct));
                        return;
                    }

                    string exe = "";

                    List<Builds> builds = Builds.GetInstalledBuilds(selectedProduct, selectedVersion);
                    foreach (Builds build in builds)
                    {
                        if (lastInstalledPath == build.InstallPath)
                            exe = (String.Format(@"{0}\{1}",
                                lastInstalledPath,
                                build.Exe));
                    }

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
                            Process.Start(exe);
                        }
                        catch (Exception ex)
                        {
                            ErrorHandling.LogException(ex);
                            ErrorHandling.DisplayExceptionMessage(ex);
                            return;
                        }
                    }
                    return;
                }
                //LaunchProduct launch = new LaunchProduct();
                launch = new LaunchProduct();
                LaunchProduct.product = selectedProduct;
                LaunchProduct.version = selectedVersion;
                launch.Show();
            }
            else
                launch.BringToFront();
            return;
        }

        private void btnBuildFolder_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                foreach (Control c in this.Controls)
                {
                    MessageBox.Show(c.Name);
                }
                return;
            }
            string product = cbProductList.Text;
            string version = cbSPGPVersion.Text;
            if (!Products.ListOfProducts().Contains(product))
            {
                string errorMessage = "Please select a Product.";
                string errorCaption = "ERROR";
                MessageBoxButtons errorButton = MessageBoxButtons.OK;
                MessageBoxIcon errorIcon = MessageBoxIcon.Error;

                MessageBox.Show(errorMessage, errorCaption, errorButton, errorIcon);
                return;
            }
            if (!Utilities.versionList.Contains(version))
            {
                string message = "Please select a version from the list.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
                return;
            }

            string buildPath = ProductInfo.GetProductInfo(product, version).InstallDirectory;
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
                ErrorHandling.LogException(ex);
                ErrorHandling.DisplayExceptionMessage(ex);
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
            LoadVPNIP();
            return;
        }

        private void labelReloadIPAddress_Click(object sender, EventArgs e)
        {
            LoadWifiIP();
            //string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //string assemblyVersionX = Assembly.LoadFile(@"C:\Users\steve.rodriguez\source\repos\EnvironmentManager4\EnvironmentManager4\Properties\AssemblyInfo.cs").GetName().Version.ToString();
            //string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            //string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            //MessageBox.Show(String.Format("{0}\r\n{1]\r\n{2}\r\n{3}", assemblyVersion, assemblyVersionX, fileVersion, productVersion));
            //MessageBox.Show(String.Format("{0}\r\n{1}\r\n{2}", assemblyVersion, fileVersion, productVersion));
            return;
        }

        private void resetDatabaseVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listAndButtonForm == null)
            {
                listAndButtonForm = new ListAndButtonForm();
                ListAndButtonForm.title = "Select Database";
                ListAndButtonForm.button = "Reset Database Version";
                listAndButtonForm.FormClosing += new FormClosingEventHandler(ResetDBTextPromptClose);
                listAndButtonForm.Show();
            }
            else
                listAndButtonForm.BringToFront();
            return;
        }

        private void ResetDBTextPromptClose(object sender, FormClosingEventArgs e)
        {
            listAndButtonForm = null;
            if (!String.IsNullOrWhiteSpace(ListAndButtonForm.output))
            {
                SettingsModel settingsModel = SettingsUtilities.GetSettings();
                DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)), ListAndButtonForm.output);
            }
            return;
        }

        private void databaseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbLog == null)
            {
                dbLog = new DatabaseActivityLog();
                dbLog.Show();
            }
            else
                dbLog.BringToFront();
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
            if (notes == null)
            {
                notes = new Notes();
                notes.Show();
            }
            else
                notes.BringToFront();
            return;
        }

        private void directoryCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Directory Compare is not currently hooked up to Environment Manager.");
            return;
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
            //SettingsUtilities.GenerateSettingsFile();
            return;
        }

        private void generateCoreModulesFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CoreModules.GenerateCoreModulesFile();
            return;
        }

        private void cbProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProduct = cbProductList.Text;
            if (selectedProduct == "SalesPad GP")
            {
                SettingsModel settings = SettingsUtilities.GetSettings();
                cbSPGPVersion.SelectedIndex = cbSPGPVersion.FindStringExact(settings.Other.DefaultVersion);
                cbSPGPVersion.Enabled = true;
            }
            else
            {
                cbSPGPVersion.SelectedIndex = cbSPGPVersion.FindStringExact("x86");
                cbSPGPVersion.Enabled = false;
            }
            return;
        }

        private void deleteBuildInstallsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (deleteBuilds == null)
            {
                deleteBuilds = new DeleteBuilds();
                deleteBuilds.Show();
            }
            else
                deleteBuilds.BringToFront();
            return;
        }

        private void btnOpenBuildFolder_Click(object sender, EventArgs e)
        {
            string product = cbProductList.Text;
            if (String.IsNullOrWhiteSpace(product) || product == "Select a Product")
                return;
            ProductInfo pi = ProductInfo.GetProductInfo(product, cbSPGPVersion.Text);
            try
            {
                Process.Start(pi.FileserverDirectory);
            }
            catch (Exception ex)
            {
                ErrorHandling.LogException(ex);
                ErrorHandling.DisplayExceptionMessage(ex);
            }
        }

        private void btnEditDescription_Click(object sender, EventArgs e)
        {
            if (udd == null)
            {
                SettingsModel settings = SettingsUtilities.GetSettings();
                if (cbDatabaseList.Text == "Select a Database"
                    || !File.Exists(String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, cbDatabaseList.Text)))
                    return;

                DatabaseManagement backupConfig = new DatabaseManagement();
                backupConfig.BackupName = cbDatabaseList.Text;
                backupConfig.BackupDescription = tbDBDesc.Text;
                udd = new UpdateDatabaseDescription();
                UpdateDatabaseDescription.backupConfig = backupConfig;
                udd.FormClosing += new FormClosingEventHandler(EditDescriptionClose);
                udd.Show();
            }
            else
                udd.BringToFront();
            return;
        }

        private void EditDescriptionClose(object sender, FormClosingEventArgs e)
        {
            udd = null;
            LoadDatabaseDescription(cbDatabaseList.Text);
            return;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null)
            {
                aboutForm = new About();
                aboutForm.Show();
            }
            else
                aboutForm.BringToFront();
            return;
        }
    }
}
