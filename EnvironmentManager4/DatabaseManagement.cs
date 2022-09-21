using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class DatabaseManagement
    {
        public string BackupName { get; set; }
        public string BackupDescription { get; set; }
        public const string dbDescLine1 = "===============================================================================";
        public const string dbDescLine2 = "=================== SELECTED DATABASE HAS NO DESCRIPTION ==================";
        public static string dbDescDefault = String.Format("{0}\n{0}\n{0}\n{0}\n{0}\n{1}\n{0}\n{0}\n{0}\n{0}\n{0}", dbDescLine1, dbDescLine2);
        public static UpdateDatabaseDescription udd;

        public static void LoadDatabaseList(ComboBox cb, TextBox tb)
        {
            cb.Items.Clear();
            cb.Text = "Select a Database Backup";
            LoadDatabaseDescription(cb, tb);
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
            cb.Items.AddRange(databases.ToArray());
        }

        public static void LoadDatabaseDescription(ComboBox cb, TextBox tb)
        {
            string backup = cb.Text;
            if (backup == "Select a Database Backup")
            {
                tb.Text = dbDescDefault;
            }
            else
            {
                try
                {
                    tb.Text = Utilities.GetDatabaseDescription(backup);
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                    tb.Text = dbDescDefault;
                }
            }
        }

        public static void ResetDatabaseVersion(string username, string password, string database = "TWO")
        {
            string script = String.Format("USE {0} EXEC dbo.sppResetDatabase", database);
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};", settingsModel.DbManagement.Connection, username, password));
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

        public static bool PreDatabaseActionValidation(string backupName, string backupZip, string action)
        {
            if (backupName == "Select a Database Backup")
            {
                MessageBox.Show(String.Format(@"Please select a backup to {0}", action));
                return false;
            }
            if (!File.Exists(backupZip))
            {
                string promptMessage = String.Format("The selected backup '{0}' does not exist in the below path:\n\n{1}", backupName, backupZip);
                string promptCaption = "ERROR";
                MessageBoxButtons promptButton = MessageBoxButtons.OK;
                MessageBoxIcon promptIcon = MessageBoxIcon.Error;

                MessageBox.Show(promptMessage, promptCaption, promptButton, promptIcon);
                return false;
            }
            return true;
        }

        public static void RestoreDatabase(string backupName, string backupZipFile)
        {
            //DISABLE DATABASE CONTROLS
            Form1.EnableDBControls(false);
            //Update cursor to use the waiting cursor
            Form1.EnableWaitCursor(true);

            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (settingsModel.DbManagement.Databases.Count <= 0 || String.IsNullOrWhiteSpace(settingsModel.DbManagement.Connection))
            {
                MessageBox.Show("SQL Server/Databases aren't configured in Settings. Please ensure a SQL Server connection is established and databases are selected in Settings.");
                return;
            }
            string unzippedBackupDirectory = String.Format(@"{0}\{1}", Path.GetDirectoryName(backupZipFile), Path.GetFileNameWithoutExtension(backupZipFile));
            try
            {
                ZipFile.ExtractToDirectory(backupZipFile, unzippedBackupDirectory);
            }
            catch (Exception e)
            {
                string extraMessage = "The existing unzipped backup will be deleted after this error message window is closed.";
                ErrorHandling.DisplayExceptionMessage(e, extraMessage);
                ErrorHandling.LogException(e);
                if (Directory.Exists(unzippedBackupDirectory))
                    Directory.Delete(unzippedBackupDirectory, true);
                Form1.EnableWaitCursor(false);
                return;
            }
            foreach (string databaseFile in settingsModel.DbManagement.Databases)
            {
                string script = String.Format(@"ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE {0} FROM DISK='{1}\{2}\{0}.bak' WITH FILE = 1, NOUNLOAD, REPLACE; ALTER DATABASE {0} SET MULTI_USER;"
                    ,databaseFile
                    ,Path.GetDirectoryName(backupZipFile)
                    ,Path.GetFileNameWithoutExtension(backupZipFile));

                try
                {
                    SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};",
                        settingsModel.DbManagement.Connection,
                        settingsModel.DbManagement.SQLServerUserName,
                        Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword))));
                    SqlDataAdapter restoreScript = new SqlDataAdapter(script, sqlCon);
                    DataTable restoreTable = new DataTable();
                    restoreScript.Fill(restoreTable);
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                }
            }
            try
            {
                Directory.Delete(unzippedBackupDirectory, true);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
            }

            //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
            DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), "RESTORED", backupName);
            SqliteDataAccess.SaveDatabaseActivity(databaseActivity);
            Form1.EnableWaitCursor(false);
            Form1.EnableDBControls(true);

            string message = String.Format(@"Backup '{0}' was successfully restored.", backupName);
            string caption = "SUCCESS";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            MessageBox.Show(message, caption, buttons, icon);
        }

        public static void DeleteDatabase(string backupName, string databaseFile, bool log, bool message)
        {
            try
            {
                File.Delete(databaseFile);

                if (message)
                {
                    MessageBox.Show(String.Format("Database '{0}' was successfully deleted.", backupName));
                }

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

        public static void NewDatabase(string databaseName, string newDatabaseDescription, string databaseBackupDirectory, string action, string existingDatabaseDescription)
        {
            //DISABLE DATABASE CONTROLS
            Form1.EnableDBControls(false);
            //Update cursor to use the waiting cursor
            Form1.EnableWaitCursor(true);
            try
            {
                Directory.CreateDirectory(databaseBackupDirectory);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (settingsModel.DbManagement.Databases.Count <= 0 || String.IsNullOrWhiteSpace(settingsModel.DbManagement.Connection))
            {
                MessageBox.Show("SQL Server/Databases aren't configured in Settings. Please ensure a SQL Server connection is established and databases are selected in Settings.");
                Form1.EnableWaitCursor(false);
                return;
            }
            foreach (string databaseFile in settingsModel.DbManagement.Databases)
            {
                string script = String.Format(@"BACKUP DATABASE {0} TO DISK='{1}\{2}\{0}.bak' WITH INIT", databaseFile, settingsModel.DbManagement.DatabaseBackupDirectory, databaseName);

                SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};", settingsModel.DbManagement.Connection, settingsModel.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settingsModel.DbManagement.SQLServerPassword))));
                SqlDataAdapter newDBScript = new SqlDataAdapter(script, sqlCon);
                DataTable newDBTable = new DataTable();
                try
                {
                    newDBScript.Fill(newDBTable);
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    Form1.EnableWaitCursor(false);
                    return;
                }
            }

            using (StreamWriter sw = File.AppendText(databaseBackupDirectory + @"\Description.txt"))
            {
                sw.WriteLine("===============================================================================");
                sw.WriteLine(String.Format("{0} - {1}", action, databaseName));
                sw.WriteLine(DateTime.Now);
                if (!String.IsNullOrWhiteSpace(existingDatabaseDescription))
                {
                    sw.WriteLine(newDatabaseDescription);
                    sw.Write(existingDatabaseDescription);
                }
                else
                    sw.Write(newDatabaseDescription);
            }

            //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
            DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), action, databaseName);
            SqliteDataAccess.SaveDatabaseActivity(databaseActivity);

            try
            {
                ZipFile.CreateFromDirectory(databaseBackupDirectory, databaseBackupDirectory + ".zip");
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
                Directory.Delete(databaseBackupDirectory, true);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }

            string actionLabel = "";
            switch(action)
            {
                case "BACKUP":
                    actionLabel = "created";
                    break;
                case "OVERWRITE":
                    actionLabel = "overwritten";
                    break;
            }

            Form1.EnableDBControls(true);
            Form1.newDBBackupName = databaseName;
            Form1.SetStaticBackup(true);
            Form1.EnableWaitCursor(false);

            string message = String.Format("The database backup '{0}' has been {1} successfully.", databaseName, actionLabel);
            string caption = "SUCCESS";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            MessageBox.Show(message, caption, buttons, icon);
        }
    }
}
