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
        private ListViewColumnSorter lvwColumnSorter;

        public Form1()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvInstalledSQLServers.ListViewItemSorter = lvwColumnSorter;
            form = this;
        }

        //public const string dbDescLine1 = "===============================================================================";
        //public const string dbDescLine2 = "=================== SELECTED DATABASE HAS NO DESCRIPTION ==================";
        //public static string dbDescDefault = String.Format("{0}\n{0}\n{0}\n{0}\n{0}\n{1}\n{0}\n{0}\n{0}\n{0}\n{0}", dbDescLine1, dbDescLine2);
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

        public static void EnableWaitCursor(bool enable)
        {
            if (form != null)
                form.WaitCursor(enable);
        }

        private void WaitCursor(bool enable)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(WaitCursor), new object[] { enable });
                return;
            }
            if (enable)
            {
                this.Cursor = Cursors.WaitCursor;
                tbDBDesc.Cursor = Cursors.WaitCursor;
                tbSPVPNIPAddress.Cursor = Cursors.WaitCursor;
                tbWiFiIPAddress.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
                tbDBDesc.Cursor = Cursors.Default;
                tbSPVPNIPAddress.Cursor = Cursors.Default;
                tbWiFiIPAddress.Cursor = Cursors.Default;
            }
        }

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
        
        public void SettingsReload(bool settingsChange = false)
        {
            cbDatabaseList.Text = "Select a Database Backup";
            DatabaseManagement.LoadDatabaseList(cbDatabaseList, tbDBDesc);
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

            cbProductList.SelectedIndex = cbProductList.FindStringExact(Products.SalesPad);
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
                CoreModules.UpdateCoreModulesFile();
                Configurations.UpdateConfigurationsFile();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureEnvironment(Environment.MachineName);
            SettingsReload(true);
            GPManagement.LoadGPInsatlls(lbGPVersionsInstalled);
            LoadWifiIP();
            LoadVPNIP();
            ServiceManagement.PopulateSQLServerList(lvInstalledSQLServers);
            cbSPGPVersion.Enabled = false;
            LoadProductList();
            if (!Utilities.IsProgramUpToDate())
            {
                UpdatePrompt update = new UpdatePrompt();
                UpdatePrompt.OpenFromStartup = true;
                update.ShowDialog();
            }
            RegUtilities.GenerateRegistryEntries();
            RegUtilities.CheckForUpdates();
            this.lvInstalledSQLServers.ColumnClick += new ColumnClickEventHandler(ColumnClick);
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
            GPManagement.LoadGPInsatlls(lbGPVersionsInstalled);
        }

        private void btnLaunchSelectedGP_Click(object sender, EventArgs e)
        {
            GPManagement.LaunchGP(lbGPVersionsInstalled.Text);
            return;
        }

        private void btnLaunchGPUtils_Click(object sender, EventArgs e)
        {
            GPManagement.LaunchGPUtilities(lbGPVersionsInstalled.Text);
            return;
        }

        private void btnInstallGP_Click(object sender, EventArgs e)
        {
            //InstallGP();
            return;
        }

        private void labelSQLVersions_Click(object sender, EventArgs e)
        {
            ServiceManagement.PopulateSQLServerList(lvInstalledSQLServers);
            return;
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count == 0)
                return;

            ServiceManagement.EnableSQLControls(false, btnStartService, btnStopService, btnRestartService, btnInstallService);
            ServiceManagement.UpdateServices("Start", lvInstalledSQLServers);
            ServiceManagement.EnableSQLControls(true, btnStartService, btnStopService, btnRestartService, btnInstallService);
            return;
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count == 0)
                return;

            ServiceManagement.EnableSQLControls(false, btnStartService, btnStopService, btnRestartService, btnInstallService);
            ServiceManagement.UpdateServices("Stop", lvInstalledSQLServers);
            ServiceManagement.EnableSQLControls(true, btnStartService, btnStopService, btnRestartService, btnInstallService);
            return;
        }

        private void btnInstallService_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnStopAllServices_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count == 0)
                return;

            ServiceManagement.EnableSQLControls(false, btnStartService, btnStopService, btnRestartService, btnInstallService);
            ServiceManagement.UpdateServices("Restart", lvInstalledSQLServers);
            ServiceManagement.EnableSQLControls(true, btnStartService, btnStopService, btnRestartService, btnInstallService);
            return;
        }

        private void btnDBBackupFolder_Click(object sender, EventArgs e)
        {
            DatabaseManagement.LaunchDBBackupFolder();
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
            DatabaseManagement.LoadDatabaseDescription(cbDatabaseList, tbDBDesc);
        }

        private void generateSettingsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SettingsUtilities.GenerateSettingsFile();
            return;
        }

        private void generateCoreModulesFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CoreModules.GenerateCoreModulesFile();
            CoreModules.UpdateCoreModulesFile();
            return;
        }

        private void cbProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProduct = cbProductList.Text;
            if (selectedProduct == Products.SalesPad)
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
            DatabaseManagement.LoadDatabaseDescription(cbDatabaseList, tbDBDesc);
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
            this.lvInstalledSQLServers.Sort();
        }
    }
}
