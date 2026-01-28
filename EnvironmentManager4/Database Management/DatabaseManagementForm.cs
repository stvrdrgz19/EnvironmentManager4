using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Windows.Devices.AllJoyn;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using Dapper;

namespace EnvironmentManager4.Database_Management
{
    public partial class DatabaseManagementForm : Form
    {
        public DatabaseManagementForm()
        {
            InitializeComponent();

            // handle closing the form
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);

            // tab stop non-tabable elements
            gbBackupDetails.TabStop = true;
            labelBackupName.TabStop = true;
            labelDescription.TabStop = true;
            gbBackupDetails.TabStop = true;
            labelSQLServer.TabStop = true;
            labelSQLServerText.TabStop = true;
        }

        public static DBUtils.DBManagementType Type { get; set; }
        public static string BackupName { get; set; }
        public static string BackupDescription { get; set; }

        private void DBMgmtTest_Load(object sender, EventArgs e)
        {
            // get settings
            SettingsModel settings = SettingsUtilities.GetSettings();

            // set form name dynamically
            this.Text = $"{Type} Database Backup";

            // set SQL Server from settings
            labelSQLServer.Text = settings.DbManagement.Connection;

            // configure form depending on type
            switch (Type)
            {
                case DBUtils.DBManagementType.Restore:
                    tbDatabaseName.ReadOnly = true;
                    tbDatabaseName.Text = BackupName;
                    tbDatabaseName.BackColor = Color.WhiteSmoke;
                    tbDatabaseDescription.ReadOnly = true;
                    tbDatabaseDescription.BackColor = Color.WhiteSmoke;
                    tbDatabaseDescription.Text = GetDatabaseDescription();
                    break;
                case DBUtils.DBManagementType.Overwrite:
                    tbDatabaseName.ReadOnly = true;
                    tbDatabaseName.Text = BackupName;
                    tbDatabaseName.BackColor = Color.WhiteSmoke;
                    break;
            }

            // populate database list
            PopulateDatabaseList(Type);

            // check all items by default
            foreach (ListViewItem item in lvDatabases.Items)
            {
                item.Checked = true;
            }
            return;
        }

        private static bool BackupExists(string backupName)
        {
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            string backupZip = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, backupName);
            return File.Exists(backupZip);
        }

        private void PopulateDatabaseList(DBUtils.DBManagementType type)
        {
            if (type == DBUtils.DBManagementType.Create || type == DBUtils.DBManagementType.Overwrite)
            {
                String[] databases = RetrieveSQLDatabases().ToArray();
                foreach (string database in databases)
                {
                    ListViewItem item = new ListViewItem(database);
                    lvDatabases.Items.Add(item);
                }
            }
            else
            {
                List<string> databaseFiles = GetDatabaseFiles(BackupName);
                foreach (string database in databaseFiles)
                {
                    ListViewItem item = new ListViewItem(database);
                    lvDatabases.Items.Add(item);
                }    
            }
            return;
        }

        private static List<string> GetDatabaseFiles(string backupName)
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            string zipPath = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, backupName);
            List<string> databaseFiles = new List<string>();

            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name != "Description.txt")
                            databaseFiles.Add(Path.GetFileNameWithoutExtension(entry.Name));
                    }
                }
            }
            return databaseFiles;
        }

        public static string GetDatabaseDescription()
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            string zipPath = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, BackupName);

            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    try
                    {
                        ZipArchiveEntry description = archive.GetEntry("Description.txt");

                        if (description != null)
                        {
                            using (StreamReader reader = new StreamReader(description.Open()))
                                return reader.ReadToEnd();
                        }
                        else
                        {
                            // return default description if no description file was found
                            return DBUtils.defaultBackupDescription;
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorHandling.LogException(e);
                        return DBUtils.defaultBackupDescription;
                    }
                }
            }
        }

        private void CreateDatabaseBackup(List<string> databases)
        {

            // update main form
            Form1.EnableDBControls(false);
            Form1.EnableWaitCursor(true);

            // get settings and ensure SQL connection is configured
            SettingsModel settings = SettingsUtilities.GetSettings();
            if (!SettingsUtilities.SQLSettingsConfigured(settings))
            {
                MessageBox.Show("SQL Server is not configured in Settings. Please ensure a SQL Server connection is established in Settings.");
                Form1.EnableDBControls(true);
                Form1.EnableWaitCursor(false);
                return;
            }

            string backupDirectory = String.Format(@"{0}\{1}", settings.DbManagement.DatabaseBackupDirectory, BackupName);

            // attempt to create the directory
            try
            {
                Directory.CreateDirectory(backupDirectory);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }

            bool backupSuccessful = true;
            foreach (string database in databases)
                backupSuccessful = BackupDatabase(database, settings);

            // establish what action was taken
            string action = "";
            if (Type == DBUtils.DBManagementType.Create)
                action = "BACKUP";
            else if (Type == DBUtils.DBManagementType.Overwrite)
                action = "OVERWRITE";

            if (backupSuccessful)
            {
                // create the description file
                CreateDatabaseDescriptionFile(action, backupDirectory);

                // save database activity to table
                DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), action, BackupName);
                SqliteDataAccess.SaveDatabaseActivity(databaseActivity);

                // Zip the backup and remove the folder
                ZipBackupFolderAndRemove(backupDirectory);

                // restore control access
                Form1.EnableDBControls(true);
                Form1.s_NewDBBackupName = BackupName;
                Form1.SetStaticBackup(true);
                Form1.EnableWaitCursor(false);

                // inform the user that the backup was successful
                Toasts.Toast(
                    "SUCCESS"
                    , String.Format("The database backup '{0}' has been {1} successfully.", BackupName, action)
                    , 1);
            }
            else
            {
                Form1.EnableDBControls(true);
                Form1.EnableWaitCursor(false);
                Toasts.Toast(
                    "FAILURE"
                    , String.Format("The database backup '{0}' was unable to be {1}. Please update the Database Backup Directory to one Environment Manager has access to and try again.", BackupName, action)
                    , 1);

                try
                {
                    Directory.Delete(backupDirectory);
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                    Form1.EnableWaitCursor(false);
                    return;
                }
            }
        }

        private static bool BackupDatabase(string database, SettingsModel settings)
        {
            string script = String.Format(@"BACKUP DATABASE {2} TO DISK='{0}\{1}\{2}.bak' WITH INIT", settings.DbManagement.DatabaseBackupDirectory, BackupName, database);
            string connString = String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};", settings.DbManagement.Connection, settings.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword)));

            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter adapter = new SqlDataAdapter(script, conn);
            DataTable table = new DataTable();

            // backup databases
            try
            {
                adapter.Fill(table);
                return true;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Operating system error 5(Access is denied.)"))
                {
                    string projectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                    string path = settings.DbManagement.DatabaseBackupDirectory;
                    string message = $"{projectName} is unable to read or write to path '{path}'.\n\nPlease modify the 'Database Backup Directory' setting to use a path Environment Manager can access, such as 'C:\\DatabaseBackups'.";
                    string caption = "ERROR";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                }
                else
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                    Form1.EnableWaitCursor(false);
                }
                return false;
            }
        }

        public void CreateDatabaseDescriptionFile(string action, string backupDirectory)
        {
            using (StreamWriter writer = File.AppendText(String.Format(@"{0}\Description.txt", backupDirectory)))
            {
                writer.WriteLine("===============================================================================");
                writer.WriteLine(String.Format("{0} - {1}", action, BackupName));
                writer.WriteLine(DateTime.Now);
                writer.Write(BackupDescription);
            }
        }

        public void ZipBackupFolderAndRemove(string backupLocation)
        {
            try
            {
                ZipFile.CreateFromDirectory(backupLocation, String.Format("{0}.zip", backupLocation));
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }
            try
            {
                Directory.Delete(backupLocation, true);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }
        }

        private void RestoreDatabaseBackup(List<string> databases)
        {
            // update main form
            Form1.EnableDBControls(false);
            Form1.EnableWaitCursor(true);

            // get settings and ensure SQL connection is configured
            SettingsModel settings = SettingsUtilities.GetSettings();
            if (!SettingsUtilities.SQLSettingsConfigured(settings))
            {
                MessageBox.Show("SQL Server is not configured in Settings. Please ensure a SQL Server connection is established in Settings.");
                Form1.EnableDBControls(true);
                Form1.EnableWaitCursor(false);
                return;
            }

            // unzip the selected backup
            string zipFile = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, BackupName);
            string unzipDirectory = String.Format(@"{0}\{1}", Path.GetDirectoryName(zipFile), Path.GetFileNameWithoutExtension(zipFile));

            UnzipBackup(zipFile, unzipDirectory);

            // restore selected databases
            foreach (string database in databases)
                RestoreDatabase(database, zipFile, settings);

            //delete unzipped backup
            try
            {
                Directory.Delete(unzipDirectory, true);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }

            // save database activity to database activity table
            DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), "RESTORED", BackupName);
            SqliteDataAccess.SaveDatabaseActivity(databaseActivity);
            Form1.EnableWaitCursor(false);
            Form1.EnableDBControls(true);

            // inform the user the install was successful via toast
            Toasts.Toast(
                "SUCCESS"
                , String.Format(@"Backup '{0}' was successfully restored.", BackupName)
                , 1);
        }

        private void RestoreDatabase(string database, string zipFile, SettingsModel settings)
        {
            // define scripts
            string singleUserScript = $"ALTER DATABASE {database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
            string restoreScript = String.Format(@"RESTORE DATABASE {0} FROM DISK='{1}\{2}\{0}.bak' WITH FILE = 1, NOUNLOAD, REPLACE;"
                , database
                , Path.GetDirectoryName(zipFile)
                , Path.GetFileNameWithoutExtension(zipFile));
            string multiUserScript = $"ALTER DATABASE {database} SET MULTI_USER;";

            // get a list of existing databases
            string[] databases = RetrieveSQLDatabases().ToArray();

            // establish sql connection
            SqlConnection conn = new SqlConnection();
            try
            {
                conn = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};",
                    settings.DbManagement.Connection,
                    settings.DbManagement.SQLServerUserName,
                    Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword))));
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }

            // Set database to SINGLE_USER with Rollback immediate if database exists
            if (databases.Contains(database))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(singleUserScript, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
            }

            // Run the restore script
            SqlDataAdapter restoreAdapter = new SqlDataAdapter(restoreScript, conn);
            DataTable restoreTable = new DataTable();
            restoreAdapter.Fill(restoreTable);

            // Set database to MULTI_USER if backup exists
            if (databases.Contains(database))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(multiUserScript, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
            }
        }

        public static void UnzipBackup(string zipFile, string unzipDirectory)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipFile, unzipDirectory);
            }
            catch (Exception e)
            {
                string extraMessage = "The existing unzipped backup will be deleted after this error message window is closed.";
                ErrorHandling.DisplayExceptionMessage(e, false, extraMessage);
                ErrorHandling.LogException(e);
                if (Directory.Exists(unzipDirectory))
                    Directory.Delete(unzipDirectory, true);
                Form1.EnableWaitCursor(false);
                return;
            }
        }

        private void DeleteDatabaseBackup(string backupName, string databaseFile, bool log, bool message)
        {
            try
            {
                File.Delete(databaseFile);

                if (message)
                    Toasts.Toast(
                        "SUCCESS"
                        , String.Format("Database '{0}' was successfully deleted.", backupName)
                        , 1);

                if (log)
                {
                    //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
                    DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), "DELETED", backupName);
                    SqliteDataAccess.SaveDatabaseActivity(databaseActivity);
                }
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }
        }

        public static void RunSalesPadDatabaseUpdate(string build, string database)
        {
            //delete dbupdate log if it exists
            ErrorHandling.DeleteLogFiles();

            //reset the database version
            ResetDatabaseVersion(database);

            Process dbUpdate = new Process();
            dbUpdate.StartInfo.FileName = String.Format("{0}\\SalesPad.exe", build);
            dbUpdate.StartInfo.Arguments = String.Format(@"/dbUpdate /userfields /conn={0}", database);
            dbUpdate.StartInfo.UseShellExecute = false;
            try
            {
                dbUpdate.Start();
                dbUpdate.WaitForExit();
                //check for pass/fail log
                if (ErrorHandling.IsThereAFailLog())
                {
                    ErrorHandling.DisplayDatabaseUpdateFailure();
                    ErrorHandling.LogDatabaseUpdateFailure();
                }
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }
        }

        public static void ResetDatabaseVersion(string database = "TWO")
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            string username = settings.DbManagement.SQLServerUserName;
            string password = Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword));

            string script = String.Format("USE {0} EXEC dbo.sppResetDatabase", database);
            SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};", settings.DbManagement.Connection, username, password));
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(script, sqlCon);
            DataTable dataTable = new DataTable();
            try
            {
                sqlAdapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                return;
            }
        }

        public static void LaunchDBBackupFolder()
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
        }

        public static List<string> RetrieveSQLDatabases()
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            List<string> databaseList = new List<string>();
            string script = @"SELECT name FROM master.dbo.sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'toolbox')";
            try
            {
                //get the service for the connection - start it if it wasn't running.
                //string serviceName = SQLServiceList.GetServiceFromConnection(settings.DbManagement.Connection);
                //if (SQLServiceList.IsServiceRunning(serviceName) == false)
                //    ServiceManagement.StartService(serviceName);

                //get list of databases.
                SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};",
                    settings.DbManagement.Connection, settings.DbManagement.SQLServerUserName,
                    Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword))));
                databaseList.AddRange(sqlCon.Query<string>(script).AsList());
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }
            return databaseList;
        }

        public static List<string> GetCompanyDatabases()
        {
            List<string> databaseList = RetrieveSQLDatabases();
            List<string> companyDatabaseList = new List<string>();
            foreach (string database in databaseList)
            {
                if (!database.Contains("DYNAMICS"))
                {
                    companyDatabaseList.Add(database);
                }
            }
            return companyDatabaseList;
        }

        public static void DeleteDatabaseBackup(bool log, bool message)
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            string databaseFile = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, BackupName);
            try
            {
                File.Delete(databaseFile);

                if (message)
                    Toasts.Toast(
                        "SUCCESS"
                        , String.Format("Database '{0}' was successfully deleted.", BackupName)
                        , 1);

                if (log)
                {
                    //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
                    DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), "DELETED", BackupName);
                    SqliteDataAccess.SaveDatabaseActivity(databaseActivity);
                }
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_DBMgmtTest = null;
            return;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvDatabases.Items)
            {
                item.Checked = true;
            }
            return;
        }

        private void btnClearSelections_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvDatabases.Items)
            {
                item.Checked = false;
            }
            return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SettingsModel settings = SettingsUtilities.GetSettings();

            // get backup name if creating
            if (Type == DBUtils.DBManagementType.Create)
                BackupName = tbDatabaseName.Text;

            // get the backup description
            BackupDescription = tbDatabaseDescription.Text;

            // get list of selected backups
            List<string> databases = new List<string>();
            foreach (ListViewItem item in lvDatabases.Items)
            {
                if (item.Checked)
                    databases.Add(item.Text);
            }

            // prompt the user to select at least one database before continuing
            if (databases.Count == 0)
            {
                string message = "Please check at least one database.";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(message, caption, buttons, icon);
                return;
            }

            if (Type == DBUtils.DBManagementType.Overwrite || Type == DBUtils.DBManagementType.Restore)
            {
                // ensure the backup exists
                if (!BackupExists(BackupName))
                {
                    string backupPath = settings.DbManagement.DatabaseBackupDirectory;
                    string message = $"The selected backup '{BackupName}' does not exist in the below path:\n\n{backupPath}/{BackupName}.zip";
                    string caption = "ERROR";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;
                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }

                // define message dynamically based on Type
                string typeMessage = "";
                if (Type == DBUtils.DBManagementType.Overwrite)
                    typeMessage = $"Are you sure you want to overwrite the selected backup '{BackupName}'? This action cannot be undone.";
                else if (Type == DBUtils.DBManagementType.Restore)
                    typeMessage = $"Are you sure you want to restore the backup '{BackupName}' over your current environment?";
                string typeCaption = "CONFIRM";
                MessageBoxButtons typeButtons = MessageBoxButtons.YesNo;
                MessageBoxIcon typeIcon = MessageBoxIcon.Question;
                DialogResult result = MessageBox.Show(typeMessage, typeCaption, typeButtons, typeIcon);

                // stop process if no
                if (result == DialogResult.No)
                    return;

                // delete old backup if yes and overwrite is the type
                if (result == DialogResult.Yes && Type == DBUtils.DBManagementType.Overwrite)
                {
                    string databaseFile = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, BackupName);
                    BackupDescription = String.Format("{0}\n\n{1}", BackupDescription, GetDatabaseDescription());
                    DeleteDatabaseBackup(BackupName, databaseFile, false, false);
                }
            }

            // prompt the user if creating a new backup that already exists
            if (Type == DBUtils.DBManagementType.Create && BackupExists(BackupName))
            {
                string message = $"A backup with the name '{BackupName}' already exists, do you want to overwrite the existing backup with the curent dataset?";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    string databaseFile = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, BackupName);
                    DeleteDatabaseBackup(BackupName, databaseFile, true, false);
                }
                else if (result == DialogResult.No)
                    return;
                else if (result == DialogResult.Cancel)
                {
                    this.Close();
                    return;
                }
            }

            // Restore the selected database backup
            if (Type == DBUtils.DBManagementType.Restore)
            {
                // push toast notification informing the user the databases are being restored
                Toasts.Toast(
                    "RESTORING"
                    , String.Format(@"Backup '{0}' is being restored.", BackupName)
                    , 1);

                Thread restoreBackup = new Thread(() => RestoreDatabaseBackup(databases));
                restoreBackup.Start();
            }
            else
            {
                // push toast notification informing the user that the selected databases are being backed up
                Toasts.Toast(
                    "CREATING BACKUP"
                    , String.Format(@"Backup '{0}' is being created.", BackupName)
                    , 1);

                Thread createBackup = new Thread(() => CreateDatabaseBackup(databases));
                createBackup.Start();
            }
            this.Close();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
