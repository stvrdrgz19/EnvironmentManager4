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
        private static Form1 s_form = null;
        private delegate void EnableDelegate(bool enable);
        //https://www.py4u.net/discuss/717463
        //https://www.codegrepper.com/code-examples/csharp/c%23+edit+form+controls+from+another+class
        private ListViewColumnSorter _lvwColumnSorter;

        public Form1()
        {
            InitializeComponent();
            _lvwColumnSorter = new ListViewColumnSorter();
            this.lvInstalledSQLServers.ListViewItemSorter = _lvwColumnSorter;
            s_form = this;
        }

        public static string s_NewDBBackupName = "test1";
        public static LaunchProduct s_Launch;
        public static UpdateDatabaseDescription s_Udd;
        public static Install s_InstallBuild;
        public static BuildLog s_BuildLog;
        public static Settings s_SettingsFormOpen;
        public static ListAndButtonForm s_ListAndButtonForm;
        public static DeleteBuilds s_DeleteBuilds;
        public static DatabaseActivityLog s_DbLog;
        public static Notes s_Notes;
        public static About s_AboutForm;
        public static NewDatabaseBackup s_NewBackup;
        public static NewDatabaseBackup s_OverwriteBackup;
        public static List<ListViewProperties> s_LvProperties = new List<ListViewProperties>();

        public static void EnableWaitCursor(bool enable)
        {
            if (s_form != null)
                s_form.WaitCursor(enable);
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
            if (s_form != null)
                s_form.EnableButton(enable);
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
                SettingsModel settings = SettingsUtilities.GetSettings();
                LoadFromSettings(settings);
            }
        }

        public static void EnableInstallButton(bool enable)
        {
            if (s_form != null)
                s_form.EnableInstall(enable);
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

        public static void EnableGPInstallButton(bool enable)
        {
            if (s_form != null)
                s_form.EnableGPInstall(enable);
        }

        private void EnableGPInstall(bool enable)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(EnableGPInstall), new object[] { enable });
                return;
            }
            btnInstallGP.Enabled = enable;
            if (enable)
                ReloadGPListNotStatic();
        }

        public void ReloadGPListNotStatic()
        {
            GPManagement.LoadGPInsatlls(lbGPVersionsInstalled);
        }

        public static void SetStaticBackup(bool enable)
        {
            if (s_form != null)
                s_form.SetSelectedBackup(enable);
        }

        public void SetSelectedBackup(bool enable)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(SetSelectedBackup), new object[] { enable });
                return;
            }
            SettingsModel settings = SettingsUtilities.GetSettings();
            LoadFromSettings(settings);
            cbDatabaseList.SelectedIndex = cbDatabaseList.FindStringExact(s_NewDBBackupName);
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

        private void LoadIPAddresses(SettingsModel settings)
        {
            if (settings.Other.ShowIP)
                LoadWifiIP();

            if (settings.Other.ShowVPNIP)
                LoadVPNIP();
        }

        private void LoadBuildVersionAndCheckForUpdates()
        {
            labelVersion.Text = String.Format("v{0}", Utilities.GetAppVersion());
            if (!Utilities.IsProgramUpToDate())
            {
                UpdatePrompt update = new UpdatePrompt();
                UpdatePrompt.OpenFromStartup = true;
                update.ShowDialog();
            }
        }

        private void SetGroupBoxForeColor(Color color)
        {
            foreach (Control gb in this.Controls)
                if (gb is GroupBox)
                    gb.ForeColor = color;
        }

        private void CheckForDevEnvironment()
        {
            if (Utilities.DevEnvironment())
                SetGroupBoxForeColor(Color.Red);
            else
                SetGroupBoxForeColor(Color.Blue);
        }

        public void LoadFromSettings(SettingsModel settings)
        {
            DatabaseManagement.LoadDatabaseList(cbDatabaseList, tbDBDesc);
            LoadProductList();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForDevEnvironment();
            SettingsModel settings = SettingsUtilities.GetSettings();
            LoadBuildVersionAndCheckForUpdates();
            LoadFromSettings(settings);
            LoadIPAddresses(settings);
            GPManagement.LoadGPInsatlls(lbGPVersionsInstalled);
            GPManagement.LoadAvailableGPs(cbGPListToInstall);
            s_LvProperties = ListViewProperties.RetrieveListViewProperties(lvInstalledSQLServers);
            ServiceManagement.PopulateSQLServerList(lvInstalledSQLServers, s_LvProperties);
            this.lvInstalledSQLServers.ColumnClick += new ColumnClickEventHandler(ColumnClick);
            return;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s_SettingsFormOpen == null)
            {
                s_SettingsFormOpen = new Settings();
                s_SettingsFormOpen.FormClosing += new FormClosingEventHandler(SettingsClose);
                s_SettingsFormOpen.Show();
            }
            else
                s_SettingsFormOpen.BringToFront();
            return;
        }

        private void SettingsClose(object sender, FormClosingEventArgs e)
        {
            s_SettingsFormOpen = null;
            SettingsModel settings = SettingsUtilities.GetSettings();
            LoadFromSettings(settings);
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
            string selectedGP = cbGPListToInstall.Text;
            List<string> installedGPs = new List<string>();
            foreach (string gp in lbGPVersionsInstalled.Items)
                installedGPs.Add(gp);

            if (installedGPs.Contains(selectedGP))
            {
                string message = String.Format("The selected gp '{0}' is already installed. Do you want to overwrite the existing installation with a fresh one?", selectedGP);
                string caption = "OVERWRITE?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.No)
                    return;
                else
                    GPManagement.DeleteGPInstall(String.Format("{0}{1}", GPManagement.gpInstallPath, selectedGP));
            }

            Thread installGP = new Thread(() => GPManagement.InstallGP(selectedGP));
            installGP.Start();
            return;
        }

        private void labelSQLVersions_Click(object sender, EventArgs e)
        {
            ServiceManagement.PopulateSQLServerList(lvInstalledSQLServers, s_LvProperties);
            return;
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count == 0)
                return;

            ServiceManagement.EnableSQLControls(false, btnStartService, btnStopService, btnRestartService, btnInstallService);
            ServiceManagement.UpdateServices("Start", lvInstalledSQLServers, s_LvProperties);
            ServiceManagement.EnableSQLControls(true, btnStartService, btnStopService, btnRestartService, btnInstallService);
            return;
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            if (lvInstalledSQLServers.SelectedItems.Count == 0)
                return;

            ServiceManagement.EnableSQLControls(false, btnStartService, btnStopService, btnRestartService, btnInstallService);
            ServiceManagement.UpdateServices("Stop", lvInstalledSQLServers, s_LvProperties);
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
            ServiceManagement.UpdateServices("Restart", lvInstalledSQLServers, s_LvProperties);
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
                    Thread restoreBackup = new Thread(() => DatabaseManagement.Restore(backupName));
                    restoreBackup.Start();
                }
            }
            return;
        }

        private void btnOverwriteDB_Click(object sender, EventArgs e)
        {
            if (s_OverwriteBackup == null)
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
                        s_OverwriteBackup = new NewDatabaseBackup();
                        NewDatabaseBackup.existingDatabaseName = backupName;
                        NewDatabaseBackup.existingDatabaseFile = backupZip;
                        NewDatabaseBackup.action = "OVERWRITE";
                        s_OverwriteBackup.Show();
                    }
                }
            }
            else
                s_OverwriteBackup.BringToFront();
            return;
        }

        private void btnNewDB_Click(object sender, EventArgs e)
        {
            if (s_NewBackup == null)
            {
                string message = "Are you sure you want to create a new Database Backup?";
                string caption = "CONFIRM";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    s_NewBackup = new NewDatabaseBackup();
                    NewDatabaseBackup.existingDatabaseName = null;
                    NewDatabaseBackup.existingDatabaseFile = null;
                    NewDatabaseBackup.action = "BACKUP";
                    s_NewBackup.Show();
                }
            }
            else
                s_NewBackup.BringToFront();
            return;
        }

        private void btnDeleteBackup_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            SettingsModel settings = SettingsUtilities.GetSettings();
            string backupZip = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, backupName);
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
                    DatabaseManagement.DeleteDatabaseBackup(backupName, backupZip, true, true);
                    LoadFromSettings(settings);
                }
            }
            return;
        }

        private void btnInstallProduct_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (s_BuildLog == null)
                {
                    if (Products.ListOfProducts().Contains(cbProductList.Text))
                    {
                        s_BuildLog = new BuildLog();
                        s_BuildLog.Show();
                    }
                }
                else
                    s_BuildLog.BringToFront();
                return;
            }
            if (s_InstallBuild == null)
            {
                string product = cbProductList.Text;
                string version = cbSPGPVersion.Text;
                if (!Products.ListOfProducts().Contains(product))
                {
                    MessageBox.Show("Please select a product from the list to continue.");
                    return;
                }
                if (!Utilities.versionList.Contains(version))
                {
                    MessageBox.Show("Please select a version from the list to continue.");
                    return;
                }

                s_InstallBuild = new Install();
                string path = Clipboard.GetText();
                string installerPath = Install.GetInstallerPath(path, product, version);

                //Install.install = Installer.GetInstallerFile(path, selectedProduct, selectedVersion);
                if (installerPath != "EXIT")
                {
                    s_InstallBuild.Product = product;
                    s_InstallBuild.Version = version;
                    s_InstallBuild.NetworkPath = Path.GetDirectoryName(installerPath);
                    s_InstallBuild.InstallerPath = installerPath;
                    s_InstallBuild.Show();
                }
                else
                    s_InstallBuild = null;
            }
            else
                s_InstallBuild.BringToFront();
            return;
        }

        private void btnLaunchProduct_Click(object sender, EventArgs e)
        {
            if (s_Launch == null)
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
                s_Launch = new LaunchProduct();
                LaunchProduct.product = selectedProduct;
                LaunchProduct.version = selectedVersion;
                s_Launch.Show();
            }
            else
                s_Launch.BringToFront();
            return;
        }

        private void btnBuildFolder_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (Environment.MachineName == "STEVERODRIGUEZ")
                {
                    string path = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\master\5.2.40.25\CustomModules\x64\SalesPad.Module.AgruIntegration.5.2.40.X64.Zip";
                    string extension = Path.GetExtension(path);
                    MessageBox.Show(extension);
                    //TestClass testClass = TestClass.tc;
                    //testClass.GetTestClassValues();
                    //TestForm tf = new TestForm();
                    //tf.Show();
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
            return;
        }

        private void resetDatabaseVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s_ListAndButtonForm == null)
            {
                s_ListAndButtonForm = new ListAndButtonForm();
                ListAndButtonForm.title = "Select Database";
                ListAndButtonForm.button = "Reset Database Version";
                s_ListAndButtonForm.FormClosing += new FormClosingEventHandler(ResetDBTextPromptClose);
                s_ListAndButtonForm.Show();
            }
            else
                s_ListAndButtonForm.BringToFront();
            return;
        }

        private void ResetDBTextPromptClose(object sender, FormClosingEventArgs e)
        {
            s_ListAndButtonForm = null;
            if (!String.IsNullOrWhiteSpace(ListAndButtonForm.output))
            {
                SettingsModel settingsModel = SettingsUtilities.GetSettings();
                DatabaseManagement.ResetDatabaseVersion(settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword)), ListAndButtonForm.output);
            }
            return;
        }

        private void databaseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s_DbLog == null)
            {
                s_DbLog = new DatabaseActivityLog();
                s_DbLog.Show();
            }
            else
                s_DbLog.BringToFront();
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
            if (s_Notes == null)
            {
                s_Notes = new Notes();
                s_Notes.Show();
            }
            else
                s_Notes.BringToFront();
            return;
        }

        private void directoryCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryCompare dc = new DirectoryCompare();
            dc.Show();
            return;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbDatabaseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatabaseManagement.LoadDatabaseDescription(cbDatabaseList, tbDBDesc);
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
            if (s_DeleteBuilds == null)
            {
                s_DeleteBuilds = new DeleteBuilds();
                s_DeleteBuilds.Show();
            }
            else
                s_DeleteBuilds.BringToFront();
            return;
        }

        private void btnOpenBuildFolder_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                //
                FileInfo fi = new FileInfo(@"C:\DatabaseBackups\GP2016\NEWSP.zip");
                long size = fi.Length;
                MessageBox.Show(String.Format("File size in bytes: {0}", size));
                return;
            }
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
            if (s_Udd == null)
            {
                SettingsModel settings = SettingsUtilities.GetSettings();
                if (cbDatabaseList.Text == "Select a Database"
                    || !File.Exists(String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, cbDatabaseList.Text)))
                    return;

                DatabaseManagement backupConfig = new DatabaseManagement();
                backupConfig.BackupName = cbDatabaseList.Text;
                backupConfig.BackupDescription = tbDBDesc.Text;
                s_Udd = new UpdateDatabaseDescription();
                UpdateDatabaseDescription.backupConfig = backupConfig;
                s_Udd.FormClosing += new FormClosingEventHandler(EditDescriptionClose);
                s_Udd.Show();
            }
            else
                s_Udd.BringToFront();
            return;
        }

        private void EditDescriptionClose(object sender, FormClosingEventArgs e)
        {
            s_Udd = null;
            DatabaseManagement.LoadDatabaseDescription(cbDatabaseList, tbDBDesc);
            return;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s_AboutForm == null)
            {
                s_AboutForm = new About();
                s_AboutForm.Show();
            }
            else
                s_AboutForm.BringToFront();
            return;
        }

        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            if (e.Column == _lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_lvwColumnSorter.Order == SortOrder.Ascending)
                    _lvwColumnSorter.Order = SortOrder.Descending;
                else
                    _lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _lvwColumnSorter.SortColumn = e.Column;
                _lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvInstalledSQLServers.Sort();
        }
    }
}
