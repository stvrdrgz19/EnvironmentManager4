﻿using Dapper;
using EnvironmentManager4.Service_Management;
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
        public List<string> Databases { get; set; }
        public string BackupLocation { get; set; }
        public static string dbDescDefault = String.Format("{0}\n{0}\n{0}\n{0}\n{0}\n{1}\n{0}\n{0}\n{0}\n{0}\n{0}"
            ,Constants.DescriptionFullLine
            ,Constants.DescriptionFileNotPresent);
        public static UpdateDatabaseDescription udd;

        public static void LoadDatabaseList(ComboBox cb, TextBox tb)
        {
            bool backupSelected = false;
            string startingBackupLabel = cb.Text;
            cb.Text = "Select a Database Backup";

            if (startingBackupLabel != "Select a Database Backup")
                backupSelected = true;

            cb.Items.Clear();
            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            if (String.IsNullOrWhiteSpace(settingsModel.DbManagement.DatabaseBackupDirectory))
            {
                MessageBox.Show("There is no value in the Database Backup Directory Setting. Please set one in Settings.");
                LoadDatabaseDescription(cb, tb);
                return;
            }
            if (!Directory.Exists(settingsModel.DbManagement.DatabaseBackupDirectory))
            {
                MessageBox.Show(String.Format("The provided database backup directory '{0}' doesn't exist.", settingsModel.DbManagement.DatabaseBackupDirectory));
                LoadDatabaseDescription(cb, tb);
                return;
            }
            cb.Items.AddRange(Utilities.GetFilesFromDirectoryByExtension(settingsModel.DbManagement.DatabaseBackupDirectory, "zip"));

            if (backupSelected)
                cb.SelectedIndex = cb.FindStringExact(startingBackupLabel);
            else
                cb.Text = "Select a Database Backup";
            LoadDatabaseDescription(cb, tb);
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

        public static void ResetDatabaseVersion(string database = "TWO")
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            string username = settings.DbManagement.SQLServerUserName;
            string password = Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword));

            if (settings.DbManagement.DBToRestore != "")
                database = settings.DbManagement.DBToRestore;

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

        public static List<string> DatabaseFilesFromBackup(string unzipDirectory)
        {
            List<string> databaseFiles = new List<string>();
            foreach (string database in Directory.GetFiles(unzipDirectory))
            {
                string name = Path.GetFileNameWithoutExtension(database);
                if (name != "Description")
                    databaseFiles.Add(name);
            }
            return databaseFiles;
        }

        public static void RestoreDatabases(List<string> databaseFiles, string zipFile, SettingsModel settings)
        {
            foreach (string databaseFile in databaseFiles)
            {
                RestoreDatabase(databaseFile, zipFile, settings);
            }
        }

        public static void RestoreDatabase(string databaseFile, string zipFile, SettingsModel settings)
        {
            string script = String.Format(@"ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE {0} FROM DISK='{1}\{2}\{0}.bak' WITH FILE = 1, NOUNLOAD, REPLACE; ALTER DATABASE {0} SET MULTI_USER;"
                , databaseFile
                , Path.GetDirectoryName(zipFile)
                , Path.GetFileNameWithoutExtension(zipFile));

            try
            {
                SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};",
                    settings.DbManagement.Connection,
                    settings.DbManagement.SQLServerUserName,
                    Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword))));
                SqlDataAdapter restoreScript = new SqlDataAdapter(script, sqlCon);
                DataTable restoreTable = new DataTable();
                restoreScript.Fill(restoreTable);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
            }
        }

        public static void Restore(string backupName)
        {
            Form1.EnableDBControls(false);
            Form1.EnableWaitCursor(true);
            SettingsModel settings = SettingsUtilities.GetSettings();

            if (!IsSQLConnectionStoredInSettings(settings))
            {
                MessageBox.Show("SQL Server is not configured in Settings. Please ensure a SQL Server connection is established in Settings.");
                Form1.EnableDBControls(true);
                Form1.EnableWaitCursor(false);
                return;
            }

            string zipFile = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, backupName);
            string unzipDirectory = String.Format(@"{0}\{1}", Path.GetDirectoryName(zipFile), Path.GetFileNameWithoutExtension(zipFile));

            UnzipBackup(zipFile, unzipDirectory);
            List<string> databaseFiles = DatabaseFilesFromBackup(unzipDirectory);
            RestoreDatabases(databaseFiles, zipFile, settings);

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

            //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
            DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), "RESTORED", backupName);
            SqliteDataAccess.SaveDatabaseActivity(databaseActivity);
            Form1.EnableWaitCursor(false);
            Form1.EnableDBControls(true);

            if (settings.DbManagement.ResetDatabaseAfterRestore)
                ResetDatabaseVersion();

            //Inform the user the install was successful via toast
            Toasts.Toast(
                "SUCCESS"
                , String.Format(@"Backup '{0}' was successfully restored.", backupName)
                , 1);
        }

        public static void DeleteDatabaseBackup(string backupName, string databaseFile, bool log, bool message)
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

        public static bool IsSQLConnectionStoredInSettings(SettingsModel settings)
        {
            if (String.IsNullOrWhiteSpace(settings.DbManagement.Connection) ||
                String.IsNullOrWhiteSpace(settings.DbManagement.SQLServerUserName) ||
                String.IsNullOrWhiteSpace(settings.DbManagement.SQLServerPassword))
                return false;
            else
                return true;
        }

        public void BackupDatabases(SettingsModel settings)
        {
            foreach (string database in this.Databases)
                BackupDatabase(database, this.BackupName, settings);
        }

        public static void BackupDatabase(string databaseFile, string backupName, SettingsModel settings)
        {
            string script = String.Format(@"BACKUP DATABASE {2} TO DISK='{0}\{1}\{2}.bak' WITH INIT", settings.DbManagement.DatabaseBackupDirectory, backupName, databaseFile);

            SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};", settings.DbManagement.Connection, settings.DbManagement.SQLServerUserName, Utilities.ToInsecureString(Utilities.DecryptString(settings.DbManagement.SQLServerPassword))));
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

        public void CreateDatabaseDescriptionFile(string action)
        {
            using (StreamWriter writer = File.AppendText(String.Format(@"{0}\Description.txt", this.BackupLocation)))
            {
                writer.WriteLine("===============================================================================");
                writer.WriteLine(String.Format("{0} - {1}", action, this.BackupName));
                writer.WriteLine(DateTime.Now);
                writer.Write(this.BackupDescription);
            }
        }

        public void ZipBackupFolderAndRemove()
        {
            try
            {
                ZipFile.CreateFromDirectory(this.BackupLocation, String.Format("{0}.zip", this.BackupLocation));
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
                Directory.Delete(this.BackupLocation, true);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }
        }

        public static void NewDatabase(DatabaseManagement databaseBackup, string action)
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            Form1.EnableDBControls(false);
            Form1.EnableWaitCursor(true);

            //Check for a saved connection
            if (!IsSQLConnectionStoredInSettings(settings))
            {
                MessageBox.Show("SQL Server is not configured in Settings. Please ensure a SQL Server connection is established in Settings.");
                Form1.EnableWaitCursor(false);
                return;
            }

            try
            {
                Directory.CreateDirectory(databaseBackup.BackupLocation);
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e);
                ErrorHandling.DisplayExceptionMessage(e);
                Form1.EnableWaitCursor(false);
                return;
            }

            databaseBackup.BackupDatabases(settings);
            databaseBackup.CreateDatabaseDescriptionFile(action);

            //SAVE DATABASE ACTIVITY TO DATABASEACTIVITY TABLE
            DatabaseActivityLogModel databaseActivity = new DatabaseActivityLogModel(Convert.ToString(DateTime.Now), action, databaseBackup.BackupName);
            SqliteDataAccess.SaveDatabaseActivity(databaseActivity);

            databaseBackup.ZipBackupFolderAndRemove();

            string actionLabel = "";
            switch (action)
            {
                case "BACKUP":
                    actionLabel = "created";
                    break;
                case "OVERWRITE":
                    actionLabel = "overwritten";
                    break;
            }

            Form1.EnableDBControls(true);
            Form1.s_NewDBBackupName = databaseBackup.BackupName;
            Form1.SetStaticBackup(true);
            Form1.EnableWaitCursor(false);

            Toasts.Toast(
                "SUCCESS"
                , String.Format("The database backup '{0}' has been {1} successfully.", databaseBackup.BackupName, actionLabel)
                , 1);
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
            List<string> databaseList = DatabaseManagement.RetrieveSQLDatabases();
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

        public static void RunSalesPadDatabaseUpdate(string build)
        {
            //Get settings
            SettingsModel settings = SettingsUtilities.GetSettings();

            //delete dbupdate log if it exists
            ErrorHandling.DeleteLogFiles();

            //reset the database version
            ResetDatabaseVersion();

            Process dbUpdate = new Process();
            dbUpdate.StartInfo.FileName = String.Format("{0}\\SalesPad.exe", build);
            dbUpdate.StartInfo.Arguments = String.Format(@"/dbUpdate /userfields /conn={0}", settings.DbManagement.DBToRestore);
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
    }
}
