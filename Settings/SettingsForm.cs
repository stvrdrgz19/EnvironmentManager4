﻿using ErrorHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using Utilities;

namespace Settings
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static SettingsModel startingSettings = new SettingsModel();
        public static List<Connection> connectionsInMemory = new List<Connection>();
        public static int s_Version;
        public static bool hidden;

        public void LoadSettings(SettingsModel settings)
        {
            try
            {
                cbConnections.Items.Clear();

                s_Version = settings.Version;
                tbdatabaseBackupDirectory.Text = settings.DbManagement.DatabaseBackupDirectory;
                cbConnections.Text = settings.DbManagement.Connection;

                List<Connection> connectionsLists = new List<Connection>();
                connectionsLists.AddRange(settings.DbManagement.ConnectionsList);

                //Need to check if connectionsInMemory already has a connection being loaded.
                List<string> connectionNames = new List<string>();
                foreach (Connection connectionInMemory in connectionsInMemory)
                    connectionNames.Add(connectionInMemory.ConnectionName);
                foreach (Connection connectionList in settings.DbManagement.ConnectionsList)
                    if (!connectionNames.Contains(connectionList.ConnectionName))
                        connectionsInMemory.Add(connectionList);

                foreach (Connection conn in connectionsLists)
                    cbConnections.Items.Add(conn.ConnectionName);

                tbSQLServerUN.Text = settings.DbManagement.SQLServerUserName;
                tbSQLServerPW.Text = Utils.ToInsecureString(Utils.DecryptString(settings.DbManagement.SQLServerPassword));
                checkResetDatabase.Checked = settings.DbManagement.ResetDatabaseAfterRestore;
                if (cbDBToReset.Items.Contains(settings.DbManagement.DBToRestore))
                    cbDBToReset.SelectedIndex = cbDBToReset.FindStringExact(settings.DbManagement.DBToRestore);
                else
                    cbDBToReset.Text = settings.DbManagement.DBToRestore;

                if (!settings.DbManagement.ResetDatabaseAfterRestore)
                    cbDBToReset.Enabled = false;

                //================================================[ BUILD MANAGEMENT SETTINGS ]================================================
                tbSalesPadx86Directory.Text = settings.BuildManagement.SalesPadx86Directory;
                tbSalesPadx64Directory.Text = settings.BuildManagement.SalesPadx64Directory;
                tbDataCollectionDirectory.Text = settings.BuildManagement.DataCollectionDirectory;
                tbSalesPadMobileDirectory.Text = settings.BuildManagement.SalesPadMobileDirectory;
                tbShipCenterDirectory.Text = settings.BuildManagement.ShipCenterDirectory;
                tbGPWebDirectory.Text = settings.BuildManagement.GPWebDirectory;
                tbWebAPIDirectory.Text = settings.BuildManagement.WebAPIDirectory;

                //=====================================================[ OTHER SETTINGS ]======================================================
                cbMode.Text = settings.Other.Mode;
                cbDefaultProductVersion.Text = settings.Other.DefaultVersion;
                checkShowAlwaysOnTop.Checked = settings.Other.ShowAlwaysOnTop;
                checkShowVPNIP.Checked = settings.Other.ShowVPNIP;
                checkShowWiFiIP.Checked = settings.Other.ShowIP;
                checkEnableWaterBot.Checked = settings.Other.EnableWaterBot;
                checkEnableInstallToasts.Checked = settings.Other.EnableInstallToasts;

                if (Environment.MachineName != "STEVERODRIGUEZ")
                    labelSettingsVersion.Visible = false;
                else
                    labelSettingsVersion.Text = String.Format("Settings Version: {0}", settings.Version);

                SetStartingValues();
            }
            catch (Exception e)
            {
                ErrorHandle.LogException(e);
                ErrorHandle.DisplayExceptionMessage(e);
            }
        }

        public SettingsModel GetSettingsValues()
        {
            var dbManagement = new DBManagement
            {
                DatabaseBackupDirectory = tbdatabaseBackupDirectory.Text,
                Connection = cbConnections.Text,
                ConnectionsList = connectionsInMemory,
                SQLServerUserName = tbSQLServerUN.Text,
                SQLServerPassword = Utils.EncryptString(Utils.ToSecureString(tbSQLServerPW.Text)),
                ResetDatabaseAfterRestore = checkResetDatabase.Checked,
                DBToRestore = cbDBToReset.Text
            };

            var buildManagement = new BuildManagement
            {
                SalesPadx86Directory = tbSalesPadx86Directory.Text,
                SalesPadx64Directory = tbSalesPadx64Directory.Text,
                DataCollectionDirectory = tbDataCollectionDirectory.Text,
                SalesPadMobileDirectory = tbSalesPadMobileDirectory.Text,
                ShipCenterDirectory = tbShipCenterDirectory.Text,
                GPWebDirectory = tbGPWebDirectory.Text,
                WebAPIDirectory = tbWebAPIDirectory.Text
            };

            var other = new Other
            {
                Mode = cbMode.Text,
                DefaultVersion = cbDefaultProductVersion.Text,
                ShowAlwaysOnTop = checkShowAlwaysOnTop.Checked,
                ShowVPNIP = checkShowVPNIP.Checked,
                ShowIP = checkShowWiFiIP.Checked,
                EnableWaterBot = checkEnableWaterBot.Checked,
                EnableInstallToasts = checkEnableInstallToasts.Checked
            };

            var settings = new SettingsModel
            {
                Version = s_Version,
                DbManagement = dbManagement,
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
            bool unsavedChanges = false;

            //DBMANAGEMENT
            DBManagement db1 = currentSettings.DbManagement;
            DBManagement db2 = startingSettings.DbManagement;
            var dbManagementVariances = db1.Compare(db2);
            var dbManagementProperties = dbManagementVariances.Aggregate(string.Empty, (a, next) => $"{ a }\r\n\t{ next.PropertyName }: { next.valA } | { next.valB }");
            if (dbManagementProperties.Count() != 0)
                unsavedChanges = true;

            //BUILDMANAGEMENT
            BuildManagement bm1 = currentSettings.BuildManagement;
            BuildManagement bm2 = startingSettings.BuildManagement;
            var buildManagementVariances = bm1.Compare(bm2);
            var buildManagementProperties = buildManagementVariances.Aggregate(string.Empty, (a, next) => $"{ a }\r\n\t{ next.PropertyName }: { next.valA } | { next.valB }");
            if (buildManagementProperties.Count() != 0)
                unsavedChanges = true;

            //OTHER
            Other other1 = currentSettings.Other;
            Other other2 = startingSettings.Other;
            var otherVariances = other1.Compare(other2);
            var otherProperties = otherVariances.Aggregate(string.Empty, (a, next) => $"{ a }\r\n\t{ next.PropertyName }: { next.valA } | { next.valB }");
            if (otherProperties.Count() != 0)
                unsavedChanges = true;

            return unsavedChanges;
        }

        private void SaveSettings(SettingsModel settings)
        {
            try
            {
                if (UnsavedChanges(settings))
                {
                    SetStartingValues();
                    string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    File.WriteAllText(Utils.GetFile("Settings.json"), json);
                }
            }
            catch (Exception e)
            {
                ErrorHandle.LogException(e);
                ErrorHandle.DisplayExceptionMessage(e);
                return;
            }
        }

        public static string GetDirectory(string selectedPath = @"C:\")
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = selectedPath;
                return folderBrowser.ShowDialog() == DialogResult.OK ? folderBrowser.SelectedPath : selectedPath;
            }
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

        public void PopulateDatabaseList()
        {
            cbDBToReset.Items.AddRange(SQL.GetCompanyDatabases().ToArray());
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            hidden = true;
            SettingsModel settings = SettingsUtilities.GetSettings();
            SettingsUtilities.UpdateSettingsFile(settings);
            if (!String.IsNullOrWhiteSpace(settings.DbManagement.Connection))
                PopulateDatabaseList();
            LoadSettings(settings);
            SetStartingValues();
            ToggleModeExecute();
        }

        private void btnSelectBackupDirectory_Click(object sender, EventArgs e)
        {
            tbdatabaseBackupDirectory.Text = GetDirectory(tbdatabaseBackupDirectory.Text);
            return;
        }

        private void btnSelectSPx86Directory_Click(object sender, EventArgs e)
        {
            tbSalesPadx86Directory.Text = GetDirectory(tbSalesPadx86Directory.Text);
            return;
        }

        private void btnSelectx64SPDirectory_Click(object sender, EventArgs e)
        {
            tbSalesPadx64Directory.Text = GetDirectory(tbSalesPadx64Directory.Text);
            return;
        }

        private void btnSelectDatacollectionDirectory_Click(object sender, EventArgs e)
        {
            tbDataCollectionDirectory.Text = GetDirectory(tbDataCollectionDirectory.Text);
            return;
        }

        private void btnSelectSalesPadMobileDirectory_Click(object sender, EventArgs e)
        {
            tbSalesPadMobileDirectory.Text = GetDirectory(tbSalesPadMobileDirectory.Text);
            return;
        }

        private void btnSelectShipCenterDirectory_Click(object sender, EventArgs e)
        {
            tbShipCenterDirectory.Text = GetDirectory(tbShipCenterDirectory.Text);
            return;
        }

        private void btnSelectGPWebDirectory_Click(object sender, EventArgs e)
        {
            tbGPWebDirectory.Text = GetDirectory(tbGPWebDirectory.Text);
            return;
        }

        private void btnSelectWebAPIDirectory_Click(object sender, EventArgs e)
        {
            tbWebAPIDirectory.Text = GetDirectory(tbWebAPIDirectory.Text);
            return;
        }

        private void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveSettings(GetSettingsValues());
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
            Connection conn = new Connection();
            conn.ConnectionName = cbConnections.Text;
            conn.ConnectionUN = tbSQLServerUN.Text;
            conn.ConnectionPW = Utils.EncryptString(Utils.ToSecureString(tbSQLServerPW.Text));

            //Add the new connection to the list to be saved
            connectionsInMemory.Add(conn);

            //Add the new connection name to the combobox
            cbConnections.Items.Add(cbConnections.Text);
            return;
        }

        private void cbConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedConnection = cbConnections.Text;
            foreach (Connection conn in connectionsInMemory)
            {
                if (selectedConnection == conn.ConnectionName)
                {
                    tbSQLServerUN.Text = conn.ConnectionUN;
                    tbSQLServerPW.Text = Utils.ToInsecureString(Utils.DecryptString(conn.ConnectionPW));
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
            foreach (Connection connection in connectionsInMemory)
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
            var json = File.ReadAllText(Utils.GetFile("Settings.json"));
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
                btnToggleVisibility.Image = Properties.Resource1.eyeopen;
                tbSQLServerPW.UseSystemPasswordChar = false;
                hidden = false;
                return;
            }
            else
            {
                btnToggleVisibility.Image = Properties.Resource1.eyeclosed;
                tbSQLServerPW.UseSystemPasswordChar = true;
                hidden = true;
                return;
            }
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            SettingsModel settings = GetSettingsValues();
            if (UnsavedChanges(settings))
            {
                string unsavedMessage = "There are unsaved changes. Would you like to save these changes?";
                string unsavedCaption = "UNSAVED CHANGES";
                MessageBoxButtons unsavedButtons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon unsavedIcon = MessageBoxIcon.Question;
                DialogResult unsavedResult;

                unsavedResult = MessageBox.Show(unsavedMessage, unsavedCaption, unsavedButtons, unsavedIcon);
                if (unsavedResult == DialogResult.Yes)
                    SaveSettings(settings);
                if (unsavedResult == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void checkResetDatabase_CheckedChanged(object sender, EventArgs e)
        {
            cbDBToReset.Enabled = checkResetDatabase.Checked ? true : false;
        }
    }
}
