using EnvironmentManager4.Build_Management;
using EnvironmentManager4.Database_Management;
using EnvironmentManager4.ErrorManagement;
using EnvironmentManager4.Service_Management;
using EnvironmentManager4.src.Core.Models;
using EnvironmentManager4.src.Core.Utilities;
using EnvironmentManager4.src.WinForms.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
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

        public Form1()
        {
            InitializeComponent();
            s_form = this;
        }

        //this is a placeholder value, used to set the selected database backup to the newly made one.
        public static string s_NewDBBackupName = "test1";

        //These are here to prevent opening duplicates of the different forms.
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
        public static InstallPropertiesMonitor s_InstallPropertiesMonitor;
        public static DatabaseManagement s_DatabaseManagement;
        public static ExceptionLog s_ExceptionLog;

        //This is in place to call/set for re-sizing the listview (lvInstalledSQLServers) depending on the number of rows - for the column chooser.
        public static List<ListViewProperties> s_LvProperties = new List<ListViewProperties>();

        //Set sort value for server listview (lvInstalledSQLServers).
        private int sortColumn = -1;

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
                AppSettings settings = new AppSettings();
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
            AppSettings settings = new AppSettings();
            LoadFromSettings(settings);
            cbDatabaseList.SelectedIndex = cbDatabaseList.FindStringExact(s_NewDBBackupName);
        }

        private void LoadWifiIP()
        {
            tbWiFiIPAddress.Text = UtilitiesOld.GetIP("Wi-Fi");
        }

        private void LoadVPNIP()
        {
            tbSPVPNIPAddress.Text = UtilitiesOld.GetIP("SalesPad VPN");
        }

        private void LoadIPAddresses(AppSettings settings)
        {
            if (settings.OShowWIFIIP)
                LoadWifiIP();

            if (settings.OShowVPNIP)
                LoadVPNIP();
        }

        private void SetGroupBoxForeColor(Color color)
        {
            foreach (Control gb in this.Controls)
                if (gb is GroupBox)
                    gb.ForeColor = color;
        }

        private void CheckForDevEnvironment()
        {
            if (DirectoryUtilities.DevEnvironment())
            {
                SetGroupBoxForeColor(Color.Red);
                tbDBDesc.BackColor = Color.MistyRose;
            }
            else
            {
                SetGroupBoxForeColor(Color.Blue);
                tbDBDesc.BackColor = Color.AliceBlue;
            }
        }

        private void LoadDatabaseList(AppSettings settings)
        {
            bool backupSelected = false;
            string startingBackupLabel = cbDatabaseList.Text;
            cbDatabaseList.Text = "Select a Database Backup";

            if (startingBackupLabel != "Select a Database Backup")
                backupSelected = true;

            cbDatabaseList.Items.Clear();
            if (String.IsNullOrWhiteSpace(settings.DBBackupDirectory))
            {
                MessageBox.Show("There is no value in the Database Backup Directory Setting. Please set one in Settings.");
                LoadDatabaseDescription();
                return;
            }
            if (!Directory.Exists(settings.DBBackupDirectory))
            {
                string projectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                MessageBox.Show(String.Format("The provided database backup DIR '{0}' doesn't exist. {1} will create this folder if you choose to create a database backup using this value.", settings.DBBackupDirectory, projectName));
                LoadDatabaseDescription();
                return;
            }
            cbDatabaseList.Items.AddRange(UtilitiesOld.GetFilesFromDirectoryByExtension(settings.DBBackupDirectory, "zip"));

            if (backupSelected)
                cbDatabaseList.SelectedIndex = cbDatabaseList.FindStringExact(startingBackupLabel);
            else
                cbDatabaseList.Text = "Select a Database Backup";
            LoadDatabaseDescription();
        }

        private void LoadDatabaseDescription()
        {
            DatabaseManagement databaseManagement = new DatabaseManagement();
            databaseManagement.backupName = cbDatabaseList.Text;
            if (databaseManagement.backupName == "Select a Database Backup")
                tbDBDesc.Text = DBUtils.defaultBackupDescription;
            else
                tbDBDesc.Text = databaseManagement.GetDatabaseDescription();
        }

        public void LoadFromSettings(AppSettings settings)
        {
            LoadDatabaseList(settings);
            cbAlwaysOnTop.Visible = settings.OShowAlwaysOnTop;
            labelReloadVPNIPAddress.Visible = settings.OShowVPNIP;
            tbSPVPNIPAddress.Visible = settings.OShowVPNIP;
            labelReloadIPAddress.Visible = settings.OShowWIFIIP;
            tbWiFiIPAddress.Visible = settings.OShowWIFIIP;

            labelReloadVPNIPAddress.Location = new Point(89, 566);
            tbSPVPNIPAddress.Location = new Point(136, 563);

            if (settings.OShowVPNIP || settings.OShowWIFIIP)
                this.Size = new Size(536, 626);

            if (!settings.OShowVPNIP && !settings.OShowWIFIIP)
                this.Size = new Size(536, 605);

            if (settings.OShowVPNIP && !settings.OShowWIFIIP)
            {
                labelReloadVPNIPAddress.Location = new Point(339, 566);
                tbSPVPNIPAddress.Location = new Point(386, 563);
            }
        }

        private void CheckIfConnectedToTheNetwork()
        {
            string connectionValue = AppVersionUtilities.GetLatestVersion();
            if (connectionValue == "Unable to Connect")
            {
                labelNotConnected.Visible = true;
                labelNotConnected.BackColor = System.Drawing.Color.Transparent;
            }
            else
                labelNotConnected.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // update app coloring to show the environment the app is running from
            CheckForDevEnvironment();

            // inform the user if connected to the Cavallo Network or not
            CheckIfConnectedToTheNetwork();

            // retrieve settings
            AppSettings settings = new AppSettings();
            AppSettings.CheckForSettingsUpdate();

            // display build version
            string version = AppVersionUtilities.GetAppVersion();
            labelVersion.Text = String.Format("v{0}", version);

            // check for updates
            AppVersionUtilities.CheckForUpdatesAndPrompt(settings);

            // enable waterbot if settings deem so
            if (settings.OEnableWaterBot)
                WaterBot.StartWaterBot();

            // update the UI based on settings values
            LoadFromSettings(settings);

            // retrieve and set IP Address values
            LoadIPAddresses(settings);

            // load installed GP versions
            GPManagement.LoadGPInsatlls(lbGPVersionsInstalled);

            // load available gp versions to "install"
            GPManagement.LoadAvailableGPs(cbGPListToInstall);

            // configure the installed sql server listview
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
            AppSettings settings = new AppSettings();
            LoadFromSettings(settings);
            ServiceManagement.PopulateSQLServerList(lvInstalledSQLServers, s_LvProperties);
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

            if (selectedGP == Constants.CouldNotConnect)
                return;

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

            // stop if no backup is selected
            if (backupName == "Select a Database Backup")
            {
                string message = "Please select a database backup to Restore.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(message, caption, buttons, icon);
                return;
            }

            // launch restore database backup form
            if (s_DatabaseManagement == null)
            {
                s_DatabaseManagement = new DatabaseManagement();
                s_DatabaseManagement.type = DBUtils.DBManagementType.Restore;
                s_DatabaseManagement.backupName = backupName;
                s_DatabaseManagement.Show();
            }
            else
                s_DatabaseManagement.BringToFront();
            return;
        }

        private void btnOverwriteDB_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;

            // stop if no backup is selected
            if (backupName == "Select a Database Backup")
            {
                string message = "Please select a database backup to Overwrite.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(message, caption, buttons, icon);
                return;
            }

            // launch overwrite database backup form
            if (s_DatabaseManagement == null)
            {
                s_DatabaseManagement = new DatabaseManagement
                {
                    type = DBUtils.DBManagementType.Overwrite,
                    backupName = backupName
                };
                s_DatabaseManagement.Show();
            }
            else
                s_DatabaseManagement.BringToFront();
            return;
        }

        private void btnNewDB_Click(object sender, EventArgs e)
        {
            // launch create database backup form
            if (s_DatabaseManagement == null)
            {
                s_DatabaseManagement = new DatabaseManagement
                {
                    type = DBUtils.DBManagementType.Create
                };
                s_DatabaseManagement.Show();
            }
            else
                s_DatabaseManagement.BringToFront();
            return;
        }

        private void btnDeleteBackup_Click(object sender, EventArgs e)
        {
            string backupName = cbDatabaseList.Text;
            AppSettings settings = new AppSettings();
            string databaseFile = String.Format(@"{0}\{1}.zip", settings.DBBackupDirectory, backupName);

            if (backupName == "Select a Database Backup")
            {
                MessageBox.Show("Please select a backup to delete.");
                return;
            }
            if (!File.Exists(databaseFile))
            {
                string promptMessage = String.Format("The selected backup '{0}' does not exist in the below path:\n\n{1}", backupName, databaseFile);
                string promptCaption = "ERROR";
                MessageBoxButtons promptButton = MessageBoxButtons.OK;
                MessageBoxIcon promptIcon = MessageBoxIcon.Error;

                MessageBox.Show(promptMessage, promptCaption, promptButton, promptIcon);
                return;
            }

            string message = String.Format(@"Are you sure you want to delete the selected backup '{0}'? This action cannot be undone.", backupName);
            string caption = "DELETE?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);
            if (result == DialogResult.Yes)
            {
                DatabaseManagement databaseManagement = new DatabaseManagement();
                databaseManagement.backupName = backupName;
                databaseManagement.DeleteDatabaseBackup(false, false);
                LoadFromSettings(settings);
            }
            return;
        }

        private void btnInstallProduct_Click(object sender, EventArgs e)
        {
            string product = productVersionLists1.SelectedProduct;
            string version = productVersionLists1.SelectedVersion;
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (s_BuildLog == null)
                {
                    if (ProductsOld.ListOfProducts().Contains(product))
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
                if (!ProductsOld.ListOfProducts().Contains(product))
                {
                    MessageBox.Show("Please select a product from the list to continue.");
                    return;
                }
                if (!UtilitiesOld.versionList.Contains(version))
                {
                    MessageBox.Show("Please select a version from the list to continue.");
                    return;
                }

                s_InstallBuild = new Install();
                string path = Clipboard.GetText();
                string installerPath = Install.GetInstallerPath(path, product, version);

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
                string product = productVersionLists1.SelectedProduct;
                string version = productVersionLists1.SelectedVersion;

                string[] products = Products.All.ToArray();

                if (!products.Contains(product))
                {
                    string message = "Please select a product from the list.";
                    string caption = "ERROR";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }

                if (product == Products.SalesPad || product == Products.ShipCenter)
                {
                    if (!Versions.All.Contains(version))
                    {
                        string message = "Please select a version from the list.";
                        string caption = "ERROR";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBoxIcon icon = MessageBoxIcon.Error;

                        MessageBox.Show(message, caption, buttons, icon);
                        return;
                    }
                }

                if (Control.ModifierKeys == Keys.Shift)
                {
                    string lastInstalledPath = SqliteDataAccess.LastInstalledBuild(product, version);
                    if (String.IsNullOrWhiteSpace(lastInstalledPath))
                    {
                        MessageBox.Show(String.Format("There isn't a last recorded build for the selected product '{0}'", product));
                        return;
                    }

                    string exe = "";

                    List<Builds> builds = Builds.GetInstalledBuilds(product, version);
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
                LaunchProduct.product = product;
                LaunchProduct.version = version;
                s_Launch.Show();
            }
            else
                s_Launch.BringToFront();
            return;
        }

        private void btnBuildFolder_Click(object sender, EventArgs e)
        {
            string product = productVersionLists1.SelectedProduct;
            string version = productVersionLists1.SelectedVersion;
            if (!ProductsOld.ListOfProducts().Contains(product))
            {
                string errorMessage = "Please select a Product.";
                string errorCaption = "ERROR";
                MessageBoxButtons errorButton = MessageBoxButtons.OK;
                MessageBoxIcon errorIcon = MessageBoxIcon.Error;

                MessageBox.Show(errorMessage, errorCaption, errorButton, errorIcon);
                return;
            }
            if (!UtilitiesOld.versionList.Contains(version))
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
                DatabaseManagement.ResetDatabaseVersion(ListAndButtonForm.output);
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
            LoadDatabaseDescription();
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
            string product = productVersionLists1.SelectedProduct;
            string version = productVersionLists1.SelectedVersion;
            if (Control.ModifierKeys == Keys.Shift)
            {
                AppSettings settings = new AppSettings();
                MessageBox.Show($"DB Dir: {settings.DBBackupDirectory}\n\nShow Always: {settings.OShowAlwaysOnTop}\n\nShow VPNIP: {settings.OShowVPNIP}");
                settings.DBBackupDirectory = @"C:\p\q";
                settings.OShowAlwaysOnTop = true;
                settings.OShowVPNIP = true;

                AppSettings settings2 = new AppSettings();
                MessageBox.Show($"DB Dir: {settings2.DBBackupDirectory}\n\nShow Always: {settings2.OShowAlwaysOnTop}\n\nShow VPNIP: {settings2.OShowVPNIP}");

                //TestForm tf = new TestForm();
                //tf.Show();
                return;
            }
            if (String.IsNullOrWhiteSpace(product) || product == "Select a Product")
                return;
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
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
            if (Control.ModifierKeys == Keys.Shift)
            {
                int settingsVersion = Properties.Settings.Default.Version;
                MessageBox.Show(settingsVersion.ToString());
                return;
            }
            if (s_Udd == null)
            {
                AppSettings settings = new AppSettings();
                if (cbDatabaseList.Text == "Select a Database"
                    || !File.Exists(String.Format(@"{0}\{1}.zip", settings.DBBackupDirectory, cbDatabaseList.Text)))
                    return;

                s_Udd = new UpdateDatabaseDescription();
                UpdateDatabaseDescription.BackupName = cbDatabaseList.Text;
                UpdateDatabaseDescription.BackupDescription = tbDBDesc.Text;
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
            LoadDatabaseDescription();
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
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.  
                sortColumn = e.Column;
                // Set the sort order to ascending by default.  
                lvInstalledSQLServers.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.  
                if (lvInstalledSQLServers.Sorting == SortOrder.Ascending)
                    lvInstalledSQLServers.Sorting = SortOrder.Descending;
                else
                    lvInstalledSQLServers.Sorting = SortOrder.Ascending;
            }
            // Call the sort method to manually sort.
            lvInstalledSQLServers.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.  
            this.lvInstalledSQLServers.ListViewItemSorter = new ListViewItemComparer(e.Column, lvInstalledSQLServers.Sorting);
        }

        private void installPropertiesMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s_InstallPropertiesMonitor == null)
            {
                s_InstallPropertiesMonitor = new InstallPropertiesMonitor();
                s_InstallPropertiesMonitor.Show();
            }
            else
                s_InstallPropertiesMonitor.BringToFront();
            return;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (s_ExceptionLog == null)
            {
                s_ExceptionLog = new ExceptionLog();
                s_ExceptionLog.Show();
            }
            else
                s_ExceptionLog.BringToFront();
            return;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppSettings settings = new AppSettings();
            LoadFromSettings(settings);
            return;
        }
    }
}
