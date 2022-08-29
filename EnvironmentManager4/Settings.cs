using Dapper;
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
        public static List<Connection> connectionsInMemory = new List<Connection>();
        public static bool connectionModified = false;
        public static bool connected;
        public static bool hidden;

        public void LoadSettings(SettingsModel settingsModel)
        {
            try
            {
                cbConnections.Items.Clear();
                lbDatabases.Items.Clear();

                tbdatabaseBackupDirectory.Text = settingsModel.DbManagement.DatabaseBackupDirectory;
                cbConnections.Text = settingsModel.DbManagement.Connection;

                List<Connection> connectionsLists = new List<Connection>();
                connectionsLists.AddRange(settingsModel.DbManagement.ConnectionsList);

                //Need to check if connectionsInMemory already has a connection being loaded.
                List<string> connectionNames = new List<string>();
                foreach (Connection connectionInMemory in connectionsInMemory)
                    connectionNames.Add(connectionInMemory.ConnectionName);
                foreach (Connection connectionList in settingsModel.DbManagement.ConnectionsList)
                {
                    if (!connectionNames.Contains(connectionList.ConnectionName))
                        connectionsInMemory.Add(connectionList);
                }
                //connectionsInMemory.AddRange(settingsModel.DbManagement.ConnectionsList);

                foreach (Connection conn in connectionsLists)
                {
                    cbConnections.Items.Add(conn.ConnectionName);
                }

                tbSQLServerUN.Text = settingsModel.DbManagement.SQLServerUserName;
                tbSQLServerPW.Text = Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword));
                foreach (string item in settingsModel.DbManagement.Databases)
                {
                    lbDatabases.Items.Add(item);
                }

                Connect(settingsModel.DbManagement.Connected);

                //================================================[ BUILD MANAGEMENT SETTINGS ]================================================
                tbSalesPadx86Directory.Text = settingsModel.BuildManagement.SalesPadx86Directory;
                tbSalesPadx64Directory.Text = settingsModel.BuildManagement.SalesPadx64Directory;
                tbDataCollectionDirectory.Text = settingsModel.BuildManagement.DataCollectionDirectory;
                tbSalesPadMobileDirectory.Text = settingsModel.BuildManagement.SalesPadMobileDirectory;
                tbShipCenterDirectory.Text = settingsModel.BuildManagement.ShipCenterDirectory;
                tbGPWebDirectory.Text = settingsModel.BuildManagement.GPWebDirectory;
                tbWebAPIDirectory.Text = settingsModel.BuildManagement.WebAPIDirectory;

                //=====================================================[ OTHER SETTINGS ]======================================================
                cbMode.Text = settingsModel.Other.Mode;
                cbDefaultProductVersion.Text = settingsModel.Other.DefaultVersion;
                checkShowAlwaysOnTop.Checked = settingsModel.Other.ShowAlwaysOnTop;
                checkShowVPNIP.Checked = settingsModel.Other.ShowVPNIP;
                checkShowWiFiIP.Checked = settingsModel.Other.ShowIP;
                SetStartingValues();
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                //MessageBox.Show(String.Format("There was an error loading the Settings File. Error is as follows:\n\n{0}", e.Message));
            }
        }

        public SettingsModel GetSettingsValues()
        {
            List<string> dbList = new List<string>();
            foreach (string item in lbDatabases.Items)
            {
                dbList.Add(item);
            }

            var dbManagement = new DbManagement
            {
                DatabaseBackupDirectory = tbdatabaseBackupDirectory.Text,
                Connection = cbConnections.Text,
                ConnectionsList = connectionsInMemory,
                SQLServerUserName = tbSQLServerUN.Text,
                SQLServerPassword = Utilities.EncryptString(Utilities.ToSecureString(tbSQLServerPW.Text)),
                Databases = dbList,
                Connected = connected
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
                ShowIP = checkShowWiFiIP.Checked
            };

            var settings = new SettingsModel
            {
                DbManagement = dbManagement,
                BuildManagement = buildManagement,
                Other = other
            };

            return settings;
        }

        public void Connect(bool tf)
        {
            switch (tf)
            {
                case true:
                    connected = true;
                    btnConnect.Text = "Disconnect";
                    btnDeleteConnection.Enabled = false;
                    btnToggleVisibility.Enabled = false;
                    tf = false;
                    break;
                case false:
                    connected = false;
                    btnConnect.Text = "Connect";
                    btnDeleteConnection.Enabled = true;
                    btnToggleVisibility.Enabled = true;
                    tf = true;
                    break;
            }
            cbConnections.Enabled = tf;
            tbSQLServerUN.Enabled = tf;
            tbSQLServerPW.Enabled = tf;
        }

        private void SetStartingValues()
        {
            startingSettings = GetSettingsValues();
            connectionModified = false;
        }

        public static bool UnsavedChanges(SettingsModel currentSettings)
        {
            bool unsavedChanges = false;

            //DBMANAGEMENT
            DbManagement db1 = currentSettings.DbManagement;
            DbManagement db2 = startingSettings.DbManagement;
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

            //DBLIST
            List<string> dbList1 = currentSettings.DbManagement.Databases;
            List<string> dbList2 = startingSettings.DbManagement.Databases;
            bool equal = dbList1.SequenceEqual(dbList2);
            if (!equal)
                unsavedChanges = true;

            if (connectionModified)
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
                    File.WriteAllText(Utilities.GetFile("Settings.json"), json);
                }
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                //MessageBox.Show(String.Format("There was an issue attempting to save settings. Error is as follows:\n\n{0}\n\n{1}", e.Message, e.ToString()));
            }
        }

        public static string GetDirectory(string selectedPath = @"C:\")
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = selectedPath;
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    return folderBrowser.SelectedPath;
                }
                else
                {
                    return selectedPath;
                }
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
            {
                ToggleMode(true);
            }
            if (cbMode.Text == "SmartBear")
            {
                ToggleMode(false);
            }
        }

        public bool DoesConnectionExist(string connectionName)
        {
            List<string> connectionList = new List<string>();
            foreach (string connection in cbConnections.Items)
            {
                connectionList.Add(connection);
            }
            if (connectionList.Contains(connectionName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ConnectToSQLDatabase()
        {
            string script = @"SELECT name FROM master.dbo.sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'toolbox')";
            try
            {
                SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};", cbConnections.Text, tbSQLServerUN.Text, tbSQLServerPW.Text));
                var sqlQuery = sqlCon.Query<string>(script).AsList();
                lbDatabases.Items.Clear();
                foreach (string database in sqlQuery)
                {
                    lbDatabases.Items.Add(database);
                }
                Connect(true);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                //MessageBox.Show(String.Format("There was an error retrieveing existing databases:\n\n{0}\n\n{1}", sqlError.Message, sqlError.ToString()));
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            hidden = true;
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            LoadSettings(settingsModel);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings(GetSettingsValues());
            return;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
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
            connectionModified = true;
            if (connected)
            {
                //Disconnecting...
                Connect(false);
            }
            else
            {
                //Connecting...
                //Add new connection (Name + un/pw) to the ConnectionList list
                ConnectToSQLDatabase();

                //Check if the connection exists - stop if it does
                if (DoesConnectionExist(cbConnections.Text))
                {
                    return;
                }

                //Place the current connection info into a ConnectionList class
                Connection conn = new Connection();
                conn.ConnectionName = cbConnections.Text;
                conn.ConnectionUN = tbSQLServerUN.Text;
                conn.ConnectionPW = Utilities.EncryptString(Utilities.ToSecureString(tbSQLServerPW.Text));

                //Add the new connection to the list to be saved
                connectionsInMemory.Add(conn);

                //Add the new connection name to the combobox
                cbConnections.Items.Add(cbConnections.Text);
            }
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
                    tbSQLServerPW.Text = Utilities.ToInsecureString(Utilities.DecryptString(conn.ConnectionPW));
                }
            }
            return;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ConnectToSQLDatabase();
            return;
        }

        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            int indx = lbDatabases.SelectedIndex;
            lbDatabases.Items.RemoveAt(indx);
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
            {
                connectionNames.Add(connection.ConnectionName);
            }
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
            {
                return;
            }

            int count = connectionsInMemory.Count();
            var json = File.ReadAllText(Utilities.GetFile("Settings.json"));
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
    }
}
