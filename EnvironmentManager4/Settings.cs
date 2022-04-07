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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
        }

        public static string startingDatabaseBackupDirectory;
        public static string startingSalesPadx86Directory;
        public static string startingSalesPadx64Directory;
        public static string startingDataCollectionDirectory;
        public static string startingSalesPadMobileDirectory;
        public static string startingShipCenterDirectory;
        public static string startingGPWebDirectory;
        public static string startingWebAPIDirectory;
        public static string startingMode;
        public static string startingSQLServer;
        public static string startingConnection;
        public static List<string> startingConnectionsList;
        public static string startingSQLServerUsername;
        public static string startingSQLServerPassword;
        public static bool startingRememberPassword;
        public static List<string> startingDatabases;
        public static bool connected;
        public static bool startingIsConnected;
        public static List<ConnectionList> connectionsInMemory = new List<ConnectionList>();
        public static bool overrideClose = false;

        public void LoadSettings()
        {
            try
            {
                //Clear fields
                cbConnections.Items.Clear();
                lbDatabases.Items.Clear();

                SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));

                //==============================================[ DATABASE MANAGEMENT SETTINGS ]===============================================
                tbdatabaseBackupDirectory.Text = settingsModel.DbManagement.DatabaseBackupDirectory;
                cbConnections.Text = settingsModel.DbManagement.Connection;

                //Loading the array of connections
                //Adding the connection name of each connection to the list
                List<ConnectionList> connectionsLists = new List<ConnectionList>();
                connectionsLists.AddRange(settingsModel.DbManagement.ConnectionsList);
                connectionsInMemory.AddRange(settingsModel.DbManagement.ConnectionsList);
                foreach (ConnectionList conn in connectionsLists)
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
                SetStartingValues();
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an error loading the Settings File. Error is as follows:\n\n{0}", e.Message));
            }
        }

        public void Connect(bool tf)
        {
            switch(tf)
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
            //==============================================[ DATABASE MANAGEMENT SETTINGS ]===============================================
            startingDatabaseBackupDirectory = tbdatabaseBackupDirectory.Text;
            startingConnection = cbConnections.Text;
            List<string> connections = new List<string>();
            foreach (string conn in cbConnections.Items)
            {
                connections.Add(conn);
            }
            startingConnectionsList = connections;
            startingSQLServerUsername = tbSQLServerUN.Text;
            startingSQLServerPassword = tbSQLServerPW.Text;
            startingIsConnected = connected;
            List<string> dbs = new List<string>();
            foreach (string item in lbDatabases.Items)
            {
                dbs.Add(item);
            }
            startingDatabases = dbs;

            //================================================[ BUILD MANAGEMENT SETTINGS ]================================================
            startingSalesPadx86Directory = tbSalesPadx86Directory.Text;
            startingSalesPadx64Directory = tbSalesPadx64Directory.Text;
            startingDataCollectionDirectory = tbDataCollectionDirectory.Text;
            startingSalesPadMobileDirectory = tbSalesPadMobileDirectory.Text;
            startingShipCenterDirectory = tbShipCenterDirectory.Text;
            startingGPWebDirectory = tbGPWebDirectory.Text;
            startingWebAPIDirectory = tbWebAPIDirectory.Text;

            //=====================================================[ OTHER SETTINGS ]======================================================
            startingMode = cbMode.Text;
        }

        private void SaveSettings()
        {
            try
            {
                List<string> unsavedChanges = GetUnsavedChanges();
                if (unsavedChanges.Count > 0)
                {
                    //Check if the Settings file exists
                    if (File.Exists(Utilities.GetSettingsFile()))
                    {
                        File.Delete(Utilities.GetSettingsFile());
                    }

                    //Gets the list of databases
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
                        Mode = cbMode.Text
                    };

                    var settings = new SettingsModel
                    {
                        DbManagement = dbManagement,
                        BuildManagement = buildManagement,
                        Other = other
                    };

                    string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    File.WriteAllText(Utilities.GetSettingsFile(), json);
                    connectionsInMemory.Clear();
                    LoadSettings();
                    //https://stackoverflow.com/questions/55981217/serialize-a-nested-object-json-net
                }
                unsavedChanges.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an issue attempting to save settings. Error is as follows:\n\n{0}\n\n{1}", e.Message, e.ToString()));
            }
        }

        public List<string> GetUnsavedChanges()
        {
            List<string> unsavedChanges = new List<string>();
            //==============================================[ DATABASE MANAGEMENT SETTINGS ]===============================================
            if (startingDatabaseBackupDirectory != tbdatabaseBackupDirectory.Text)
            {
                unsavedChanges.Add("DatabaseBackupDirectory");
            }
            if (startingConnection != cbConnections.Text.ToString())
            {
                unsavedChanges.Add("Connection");
            }
            List<string> connectionList = new List<string>();
            foreach (string conn in cbConnections.Items)
            {
                connectionList.Add(conn);
            }
            bool areConnectionsEqual = Enumerable.SequenceEqual(startingConnectionsList, connectionList);
            if (!areConnectionsEqual)
            {
                unsavedChanges.Add("Connections");
            }
            if (startingSQLServerUsername != tbSQLServerUN.Text)
            {
                unsavedChanges.Add("SQLServerUserName");
            }
            if (startingSQLServerPassword != tbSQLServerPW.Text)
            {
                unsavedChanges.Add("SQLServerPassword");
            }
            if (startingIsConnected != connected)
            {
                unsavedChanges.Add("Connected");
            }
            List<string> dbs = new List<string>();
            foreach (string item in lbDatabases.Items)
            {
                dbs.Add(item);
            }
            bool areDBsEqual = Enumerable.SequenceEqual(startingDatabases, dbs);
            if (!areDBsEqual)
            {
                unsavedChanges.Add("Databases");
            }

            //================================================[ BUILD MANAGEMENT SETTINGS ]================================================
            if (startingSalesPadx86Directory != tbSalesPadx86Directory.Text)
            {
                unsavedChanges.Add("SalesPadx86Directory");
            }
            if (startingSalesPadx64Directory != tbSalesPadx64Directory.Text)
            {
                unsavedChanges.Add("SalesPadx64Directory");
            }
            if (startingDataCollectionDirectory != tbDataCollectionDirectory.Text)
            {
                unsavedChanges.Add("DataCollectionDirectory");
            }
            if (startingSalesPadMobileDirectory != tbSalesPadMobileDirectory.Text)
            {
                unsavedChanges.Add("SalesPadMobileDirectory");
            }
            if (startingShipCenterDirectory != tbShipCenterDirectory.Text)
            {
                unsavedChanges.Add("ShipCenterDirectory");
            }
            if (startingGPWebDirectory != tbGPWebDirectory.Text)
            {
                unsavedChanges.Add("GPWebDirectory");
            }
            if (startingWebAPIDirectory != tbWebAPIDirectory.Text)
            {
                unsavedChanges.Add("WebAPIDirectory");
            }

            //=====================================================[ OTHER SETTINGS ]======================================================
            if (startingMode != cbMode.Text)
            {
                unsavedChanges.Add("Mode");
            }
            return unsavedChanges;
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
            if (cbMode.Text == "Standard")
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
            string sqlServer = cbConnections.Text;
            string script = @"SELECT name FROM master.dbo.sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')";
            try
            {
                SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID=sa;Password=sa;", sqlServer));
                var sqlQuery = sqlCon.Query<string>(script).AsList();
                lbDatabases.Items.Clear();
                foreach (string database in sqlQuery)
                {
                    lbDatabases.Items.Add(database);
                }
            }
            catch (Exception sqlError)
            {
                MessageBox.Show(String.Format("There was an error retrieveing existing databases:\n\n{0}\n\n{1}", sqlError.Message, sqlError.ToString()));
            }
        }

        public bool AreThereUnsavedChanges()
        {
            List<string> unsavedChanges = GetUnsavedChanges();
            if (unsavedChanges.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            LoadSettings();
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
            SaveSettings();
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
            if (connected)
            {
                //Disconnecting...
                Connect(false);
            }
            else
            {
                //Connecting...
                //Add new connection (Name + un/pw) to the ConnectionList list
                Connect(true);
                ConnectToSQLDatabase();

                //Check if the connection exists - stop if it does
                if (DoesConnectionExist(cbConnections.Text))
                {
                    return;
                }

                //Place the current connection info into a ConnectionList class
                ConnectionList conn = new ConnectionList();
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
            foreach (ConnectionList conn in connectionsInMemory)
            {
                if (selectedConnection == conn.ConnectionName)
                {
                    tbSQLServerUN.Text = conn.ConnectionUN;
                    tbSQLServerPW.Text = conn.ConnectionPW;
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
            int count = connectionsInMemory.Count();
            var json = File.ReadAllText(Utilities.GetSettingsFile());
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
            if (btnToggleVisibility.Text == "---")
            {
                tbSQLServerPW.UseSystemPasswordChar = false;
                btnToggleVisibility.Text = "0";
                return;
            }
            else
            {
                tbSQLServerPW.UseSystemPasswordChar = true;
                btnToggleVisibility.Text = "---";
                return;
            }
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            if (!overrideClose)
            {
                if (AreThereUnsavedChanges())
                {
                    string message = "There are unsaved changes. Do you want to save these changes?";
                    string caption = "UNSAVED CHANGES";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    MessageBoxIcon icon = MessageBoxIcon.Question;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, buttons, icon);
                    if (result == DialogResult.Yes)
                    {
                        SaveSettings();
                        overrideClose = true;
                        return;
                    }
                    if (result == DialogResult.No)
                    {
                        overrideClose = true;
                        return;
                    }
                    if (result == DialogResult.Cancel)
                    {
                        overrideClose = true;
                        e.Cancel = true;
                        return;
                    }
                }
            }
            overrideClose = false;
        }
    }
}
