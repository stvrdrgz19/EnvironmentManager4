using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                MessageBox.Show(String.Format("There was an issue resetting the database version for the '{0}' Database.\n\n{1}\n\n{2}", database, e.Message, e.ToString()));
                return;
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
                MessageBox.Show(String.Format("There was an error unzipping the selected backup: {0}\n\n{1}", e.Message, e.ToString()));
                if (Directory.Exists(unzippedBackupDirectory))
                    Directory.Delete(unzippedBackupDirectory, true);
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
                    if (e is SqlException)
                    {
                        ErrorHandling.LogException(e);
                    }
                }
            }
            try
            {
                Directory.Delete(unzippedBackupDirectory, true);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an error deleting the unzipped database backup at '{0}', please try manually deleting this filder.\n\n{1}\n\n{2}", unzippedBackupDirectory, e.Message, e.ToString()));
            }

            //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
            DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), "RESTORED", backupName);
            SqliteDataAccess.SaveDatabaseActivity(databaseActivity);

            string message = String.Format(@"Backup '{0}' was successfully restored.", backupName);
            string caption = "SUCCESS";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            MessageBox.Show(message, caption, buttons, icon);
            Form1.EnableDBControls(true);
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
                MessageBox.Show(String.Format("There was an error deleting the selected database backup file '{0}'\n\n{1}\n\n{2}", databaseFile, e.Message, e.ToString()));
            }
        }

        public static void NewDatabase(string databaseName, string newDatabaseDescription, string databaseBackupDirectory, string action, string existingDatabaseDescription)
        {
            //DISABLE DATABASE CONTROLS
            Form1.EnableDBControls(false);
            try
            {
                Directory.CreateDirectory(databaseBackupDirectory);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was an error creating the directory '{0}', the error is as follows:\n\n{1}\n\n{2}", databaseBackupDirectory, e.Message, e.ToString()));
                return;
            }
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (settingsModel.DbManagement.Databases.Count <= 0 || String.IsNullOrWhiteSpace(settingsModel.DbManagement.Connection))
            {
                MessageBox.Show("SQL Server/Databases aren't configured in Settings. Please ensure a SQL Server connection is established and databases are selected in Settings.");
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
                    MessageBox.Show(String.Format("There was an error creating a backup of the '{0}' database. Error is as follows:\n\n{1}\n\n{2}", databaseFile, e.Message, e.ToString()));
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
                MessageBox.Show(String.Format("There was an error zipping the database backup files to '{0}'. Error is as follows:\n\n{1}\n\n{2}", databaseBackupDirectory + ".zip", e.Message, e.ToString()));
                return;
            }
            try
            {
                Directory.Delete(databaseBackupDirectory, true);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("There was a problem deleting the database directory: {0}\n\n{1}\n\n{2}", databaseBackupDirectory, e.Message, e.ToString()));
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

            string message = String.Format("The database backup '{0}' has been {1} successfully.", databaseName, actionLabel);
            string caption = "SUCCESS";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            MessageBox.Show(message, caption, buttons, icon);
        }
    }
}
