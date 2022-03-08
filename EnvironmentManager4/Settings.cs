using Dapper;
using Newtonsoft.Json;
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
        public static List<string> startingDatabases;
        public static bool startingLocked;

        public void LoadSettings()
        {
            try
            {
                SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
                tbdatabaseBackupDirectory.Text = settingsModel.DbManagement.DatabaseBackupDirectory;
                tbSQLServer.Text = settingsModel.DbManagement.SQLServer;
                foreach (string item in settingsModel.DbManagement.Databases)
                {
                    lbDatabases.Items.Add(item);
                }
                if (settingsModel.DbManagement.LockedIn)
                {
                    cbLocked.Checked = true;
                    tbSQLServer.Enabled = false;
                    lbDatabases.Enabled = true;
                    btnAdd.Enabled = true;
                    btnRemove.Enabled = true;
                }
                else
                {
                    cbLocked.Checked = false;
                    tbSQLServer.Enabled = true;
                    lbDatabases.Enabled = false;
                    btnAdd.Enabled = false;
                    btnRemove.Enabled = false;
                    lbDatabases.Items.Clear();
                }

                tbSalesPadx86Directory.Text = settingsModel.BuildManagement.SalesPadx86Directory;
                tbSalesPadx64Directory.Text = settingsModel.BuildManagement.SalesPadx64Directory;
                tbDataCollectionDirectory.Text = settingsModel.BuildManagement.DataCollectionDirectory;
                tbSalesPadMobileDirectory.Text = settingsModel.BuildManagement.SalesPadMobileDirectory;
                tbShipCenterDirectory.Text = settingsModel.BuildManagement.ShipCenterDirectory;
                tbGPWebDirectory.Text = settingsModel.BuildManagement.GPWebDirectory;
                tbWebAPIDirectory.Text = settingsModel.BuildManagement.WebAPIDirectory;
                cbMode.Text = settingsModel.Other.Mode;
                SetStartingValues();
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an error loading the Settings File. Error is as follows:\n\n{0}", e.Message));
            }
        }

        private void SetStartingValues()
        {
            List<string> dbs = new List<string>();
            foreach (string item in lbDatabases.Items)
            {
                dbs.Add(item);
            }
            startingDatabaseBackupDirectory = tbdatabaseBackupDirectory.Text;
            startingSQLServer = tbSQLServer.Text;
            startingDatabases = dbs;
            startingLocked = cbLocked.Checked;
            startingSalesPadx86Directory = tbSalesPadx86Directory.Text;
            startingSalesPadx64Directory = tbSalesPadx64Directory.Text;
            startingDataCollectionDirectory = tbDataCollectionDirectory.Text;
            startingSalesPadMobileDirectory = tbSalesPadMobileDirectory.Text;
            startingShipCenterDirectory = tbShipCenterDirectory.Text;
            startingGPWebDirectory = tbGPWebDirectory.Text;
            startingWebAPIDirectory = tbWebAPIDirectory.Text;
            startingMode = cbMode.Text;
        }

        private void SaveSettings()
        {
            try
            {
                List<string> unsavedChanges = GetUnsavedChanges();
                if (unsavedChanges.Count > 0)
                {
                    if (File.Exists(Utilities.GetSettingsFile()))
                    {
                        File.Delete(Utilities.GetSettingsFile());
                    }
                    List<string> dbList = new List<string>();
                    foreach (string item in lbDatabases.Items)
                    {
                        dbList.Add(item);
                    }

                    var dbManagement = new DbManagement
                    {
                        DatabaseBackupDirectory = tbdatabaseBackupDirectory.Text,
                        SQLServer = tbSQLServer.Text,
                        Databases = dbList,
                        LockedIn = cbLocked.Checked
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
                    SetStartingValues();
                    //https://stackoverflow.com/questions/55981217/serialize-a-nested-object-json-net
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an issue attempting to save settings. Error is as follows:\n\n{0}\n\n{1}", e.Message, e.ToString()));
            }
        }

        public List<string> GetUnsavedChanges()
        {
            List<string> unsavedChanges = new List<string>();
            List<string> dbs = new List<string>();
            foreach (string item in lbDatabases.Items)
            {
                dbs.Add(item);
            }
            if (startingDatabaseBackupDirectory != tbdatabaseBackupDirectory.Text)
            {
                unsavedChanges.Add("DatabaseBackupDirectory");
            }
            if (startingSQLServer != tbSQLServer.Text)
            {
                unsavedChanges.Add("SQLServer");
            }
            bool isEqual = Enumerable.SequenceEqual(startingDatabases, dbs);
            if (!isEqual)
            {
                unsavedChanges.Add("Databases");
            }
            if (startingLocked != cbLocked.Checked)
            {
                unsavedChanges.Add("LockedIn");
            }
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
            List<string> unsavedChanges = GetUnsavedChanges();
            if (unsavedChanges.Count > 0)
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
                }
                if (result == DialogResult.No)
                {
                    this.Close();
                }
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            this.Close();
            return;
        }

        private void cbLocked_CheckedChanged(object sender, EventArgs e)
        {
            if (tbSQLServer.Enabled)
            {
                tbSQLServer.Enabled = false;
                lbDatabases.Enabled = true;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
            }
            else
            {
                tbSQLServer.Enabled = true;
                lbDatabases.Enabled = false;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                lbDatabases.Items.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string sqlServer = tbSQLServer.Text;
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
            return;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int indx = lbDatabases.SelectedIndex;
            lbDatabases.Items.RemoveAt(indx);
            return;
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleModeExecute();
            return;
        }
    }
}
