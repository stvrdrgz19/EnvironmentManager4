using Dapper;
using EnvironmentManager4.src.Core.Models;
using EnvironmentManager4.src.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static SettingsModel startingSettings = new SettingsModel();
        public static List<ConnectionInfo> connectionsInMemory = new List<ConnectionInfo>();
        public static int s_Version;
        public static bool hidden;

        public SettingsModel GetSettingsValues()
        {
            var dbManagement = new DBManagement
            {
                DatabaseBackupDirectory = tbdatabaseBackupDirectory.Text,
                Connection = cbConnections.Text,
                ConnectionsList = connectionsInMemory,
                SQLServerUserName = tbSQLServerUN.Text,
                SQLServerPassword = UtilitiesOld.EncryptString(UtilitiesOld.ToSecureString(tbSQLServerPW.Text)),
            };

            var buildManagement = new BuildManagement
            {
                SalesPadx86Directory = tbSalesPadx86Directory.Text,
                SalesPadx64Directory = tbSalesPadx64Directory.Text,
                DataCollectionDirectory = tbDataCollectionDirectory.Text,
                SalesPadMobileDirectory = tbSalesPadMobileDirectory.Text,
                ShipCenterx86Directory = tbShipCenterDirectory.Text,
                ShipCenterx64Directory = tbShipCenterx64Directory.Text,
                GPWebDirectory = tbGPWebDirectory.Text,
                WebAPIDirectory = tbWebAPIDirectory.Text
            };

            var other = new Other
            {
                Mode = cbMode.Text,
                ShowAlwaysOnTop = checkShowAlwaysOnTop.Checked,
                ShowVPNIP = checkShowVPNIP.Checked,
                ShowIP = checkShowWiFiIP.Checked,
                EnableWaterBot = checkEnableWaterBot.Checked,
                EnableInstallToasts = checkEnableInstallToasts.Checked,
                PromptForUpdate = checkPromptForUpdate.Checked,
            };

            var settings = new SettingsModel
            {
                Version = s_Version,
                DBManagement = dbManagement,
                BuildManagement = buildManagement,
                Other = other
            };

            return settings;
        }

        private void SetStartingValues()
        {
            startingSettings = GetSettingsValues();
        }

        public static bool UnsavedChanges(SettingsModel currentSettings)
        {
            AppSettings appSettings = new AppSettings();
            SettingsModel savedSettings = appSettings.GetAllSettings();
            bool unsavedChanges = false;

            //DBMANAGEMENT
            DBManagement db1 = currentSettings.DBManagement;
            DBManagement db2 = savedSettings.DBManagement;
            var dbManagementVariances = db1.Compare(db2);
            var dbManagementProperties = dbManagementVariances.Aggregate(string.Empty, (a, next) => $"{ a }\r\n\t{ next.PropertyName }: { next.valA } | { next.valB }");
            if (dbManagementProperties.Count() != 0)
                unsavedChanges = true;

            //BUILDMANAGEMENT
            BuildManagement bm1 = currentSettings.BuildManagement;
            BuildManagement bm2 = savedSettings.BuildManagement;
            var buildManagementVariances = bm1.Compare(bm2);
            var buildManagementProperties = buildManagementVariances.Aggregate(string.Empty, (a, next) => $"{ a }\r\n\t{ next.PropertyName }: { next.valA } | { next.valB }");
            if (buildManagementProperties.Count() != 0)
                unsavedChanges = true;

            //OTHER
            Other other1 = currentSettings.Other;
            Other other2 = savedSettings.Other;
            var otherVariances = other1.Compare(other2);
            var otherProperties = otherVariances.Aggregate(string.Empty, (a, next) => $"{ a }\r\n\t{ next.PropertyName }: { next.valA } | { next.valB }");
            if (otherProperties.Count() != 0)
                unsavedChanges = true;

            return unsavedChanges;
        }

        public void ToggleMode(bool tf)
        {
            tbSalesPadx64Directory.Enabled = tf;
            tbDataCollectionDirectory.Enabled = tf;
            tbSalesPadMobileDirectory.Enabled = tf;
            tbShipCenterDirectory.Enabled = tf;
            tbGPWebDirectory.Enabled = tf;
            tbWebAPIDirectory.Enabled = tf;
            btnSelectx64SPDirectory.Enabled = tf;
            btnSelectDatacollectionDirectory.Enabled = tf;
            btnSelectSalesPadMobileDirectory.Enabled = tf;
            btnSelectShipCenterDirectory.Enabled = tf;
            btnSelectShipCenterx64Directory.Enabled = tf;
            btnSelectGPWebDirectory.Enabled = tf;
            btnSelectWebAPIDirectory.Enabled = tf;
        }

        public void ToggleModeExecute()
        {
            if (cbMode.Text == "Standard" || cbMode.Text == "Kyle")
                ToggleMode(true);
            if (cbMode.Text == "SmartBear")
                ToggleMode(false);
        }

        public bool DoesConnectionExist(string connectionName)
        {
            return cbConnections.Items.Contains(connectionName) ? true : false;
        }

        private void LoadSettings(AppSettings settings)
        {
            cbConnections.Items.Clear();

            s_Version = settings.Version;
            tbdatabaseBackupDirectory.Text = settings.DBBackupDirectory;
            cbConnections.Text = settings.DBConnection;

            // load connections
            List<ConnectionInfo> connections = ConnectionInfo.DecodeConnections(settings.SavedConnections);
            foreach (ConnectionInfo connection in connections)
                cbConnections.Items.Add(connection.ConnectionName);

            // save connections to memory
            connectionsInMemory = connections;

            tbSQLServerUN.Text = settings.DBUserName;
            tbSQLServerPW.Text = UtilitiesOld.ToInsecureString(UtilitiesOld.DecryptString(settings.DBPassword));

            //================================================[ BUILD MANAGEMENT SETTINGS ]================================================
            tbSalesPadx86Directory.Text = settings.BMSalesPadx86;
            tbSalesPadx64Directory.Text = settings.BMSalesPadx64;
            tbDataCollectionDirectory.Text = settings.BMDataCollection;
            tbSalesPadMobileDirectory.Text = settings.BMSalesPadMobile;
            tbShipCenterDirectory.Text = settings.BMShipCenterx86;
            tbShipCenterx64Directory.Text = settings.BMShipCenterx64;
            tbGPWebDirectory.Text = settings.GPWeb;
            tbWebAPIDirectory.Text = settings.WebAPI;

            //=====================================================[ OTHER SETTINGS ]======================================================
            cbMode.Text = settings.Mode;
            checkShowAlwaysOnTop.Checked = settings.OShowAlwaysOnTop;
            checkShowVPNIP.Checked = settings.OShowVPNIP;
            checkShowWiFiIP.Checked = settings.OShowWIFIIP;
            checkEnableWaterBot.Checked = settings.OEnableWaterBot;
            checkEnableInstallToasts.Checked = settings.OEnableInstallToasts;
            checkPromptForUpdate.Checked = settings.OPromptForUpgrade;

            if (Environment.MachineName != "SRODRIGUEZ")
                labelSettingsVersion.Visible = false;
            else
                labelSettingsVersion.Text = String.Format("Settings Version: {0}", settings.Version);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            hidden = true;
            AppSettings appSettings = new AppSettings();
            AppSettings.CheckForSettingsUpdate();
            LoadSettings(appSettings);
            SetStartingValues();
            ToggleModeExecute();
        }

        private void btnSelectBackupDirectory_Click(object sender, EventArgs e)
        {
            tbdatabaseBackupDirectory.Text = DirectoryUtilities.GetDirectory(tbdatabaseBackupDirectory.Text);
            return;
        }

        private void btnSelectSPx86Directory_Click(object sender, EventArgs e)
        {
            tbSalesPadx86Directory.Text = DirectoryUtilities.GetDirectory(tbSalesPadx86Directory.Text);
            return;
        }

        private void btnSelectx64SPDirectory_Click(object sender, EventArgs e)
        {
            tbSalesPadx64Directory.Text = DirectoryUtilities.GetDirectory(tbSalesPadx64Directory.Text);
            return;
        }

        private void btnSelectDatacollectionDirectory_Click(object sender, EventArgs e)
        {
            tbDataCollectionDirectory.Text = DirectoryUtilities.GetDirectory(tbDataCollectionDirectory.Text);
            return;
        }

        private void btnSelectSalesPadMobileDirectory_Click(object sender, EventArgs e)
        {
            tbSalesPadMobileDirectory.Text = DirectoryUtilities.GetDirectory(tbSalesPadMobileDirectory.Text);
            return;
        }

        private void btnSelectShipCenterDirectory_Click(object sender, EventArgs e)
        {
            tbShipCenterDirectory.Text = DirectoryUtilities.GetDirectory(tbShipCenterDirectory.Text);
            return;
        }

        private void btnSelectShipCenterx64Directory_Click(object sender, EventArgs e)
        {
            tbShipCenterx64Directory.Text = DirectoryUtilities.GetDirectory(tbShipCenterx64Directory.Text);
            return;
        }

        private void btnSelectGPWebDirectory_Click(object sender, EventArgs e)
        {
            tbGPWebDirectory.Text = DirectoryUtilities.GetDirectory(tbGPWebDirectory.Text);
            return;
        }

        private void btnSelectWebAPIDirectory_Click(object sender, EventArgs e)
        {
            tbWebAPIDirectory.Text = DirectoryUtilities.GetDirectory(tbWebAPIDirectory.Text);
            return;
        }

        private void btnSaveExit_Click(object sender, EventArgs e)
        {
            AppSettings.SaveSettings(GetSettingsValues());
            this.Close();
            return;
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleModeExecute();
            return;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //Check if the connection exists - stop if it does
            if (DoesConnectionExist(cbConnections.Text))
                return;

            //Place the current connection info into a ConnectionList class
            ConnectionInfo conn = new ConnectionInfo();
            conn.ConnectionName = cbConnections.Text;
            conn.ConnectionUN = tbSQLServerUN.Text;
            conn.ConnectionPW = UtilitiesOld.EncryptString(UtilitiesOld.ToSecureString(tbSQLServerPW.Text));

            //Add the new connection to the list to be saved
            connectionsInMemory.Add(conn);

            //Add the new connection name to the combobox
            cbConnections.Items.Add(cbConnections.Text);
            return;
        }

        private void cbConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedConnection = cbConnections.Text;
            foreach (ConnectionInfo conn in connectionsInMemory)
            {
                if (selectedConnection == conn.ConnectionName)
                {
                    tbSQLServerUN.Text = conn.ConnectionUN;
                    tbSQLServerPW.Text = UtilitiesOld.ToInsecureString(UtilitiesOld.DecryptString(conn.ConnectionPW));
                }
            }
            return;
        }

        private void btnDeleteConnection_Click(object sender, EventArgs e)
        {
            string selectedConnection = cbConnections.Text;
            //Check to make sure a connection is selected
            if (String.IsNullOrWhiteSpace(selectedConnection))
                return;

            //Add all existing connection names to a list to be checked
            List<string> connectionNames = new List<string>();
            foreach (ConnectionInfo connection in connectionsInMemory)
                connectionNames.Add(connection.ConnectionName);

            //Make sure the connection is actually a saved connection before attempting to delete
            if (!connectionNames.Contains(selectedConnection))
            {
                MessageBox.Show(String.Format("The selected connection '{0}' does not exist as a saved connection", selectedConnection));
                return;
            }

            string message = String.Format(@"Are you sure you want to delete the '{0}' connection?", selectedConnection);
            string caption = "DELETE";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons, icon);

            if (result == DialogResult.No)
                return;

            int count = connectionsInMemory.Count();
            var json = File.ReadAllText(DirectoryUtilities.GetFile("Settings.json"));
            var obj = JObject.Parse(json);

            for (int i = 0; i < count; i++)
            {
                var connectionName = (string)obj["DbManagement"]["ConnectionsList"][i]["ConnectionName"];
                if (connectionName == selectedConnection)
                {
                    connectionsInMemory.RemoveAt(i);
                    cbConnections.Items.RemoveAt(i);
                    cbConnections.Text = "";
                    tbSQLServerUN.Text = "";
                    tbSQLServerPW.Text = "";
                    break;
                }
            }
            return;
        }

        private void btnToggleVisibility_Click(object sender, EventArgs e)
        {
            if (hidden)
            {
                btnToggleVisibility.Image = Properties.Resources.eyeopen;
                tbSQLServerPW.UseSystemPasswordChar = false;
                hidden = false;
                return;
            }
            else
            {
                btnToggleVisibility.Image = Properties.Resources.eyeclosed;
                tbSQLServerPW.UseSystemPasswordChar = true;
                hidden = true;
                return;
            }
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            SettingsModel settings = GetSettingsValues();
            //if (UnsavedChanges(settings))
            //{
            //    string unsavedMessage = "There are unsaved changes. Would you like to save these changes?";
            //    string unsavedCaption = "UNSAVED CHANGES";
            //    MessageBoxButtons unsavedButtons = MessageBoxButtons.YesNoCancel;
            //    MessageBoxIcon unsavedIcon = MessageBoxIcon.Question;
            //    DialogResult unsavedResult;

            //    unsavedResult = MessageBox.Show(unsavedMessage, unsavedCaption, unsavedButtons, unsavedIcon);
            //    if (unsavedResult == DialogResult.Yes)
            //        AppSettings.SaveSettings(settings);
            //    if (unsavedResult == DialogResult.Cancel)
            //        e.Cancel = true;
            //}
        }
    }
}
