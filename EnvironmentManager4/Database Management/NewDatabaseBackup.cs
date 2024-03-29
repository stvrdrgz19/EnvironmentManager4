﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class NewDatabaseBackup : Form
    {
        public NewDatabaseBackup()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        public static string existingDatabaseName;
        public static string existingDatabaseFile;
        public static string action;

        private void NewDatabaseBackup_Load(object sender, EventArgs e)
        {
            if (action == "OVERWRITE")
            {
                tbDatabaseName.TabStop = false;
                this.Text = "Overwrite Database Backup";
                tbDatabaseName.ReadOnly = true;
            }
            if (!String.IsNullOrEmpty(existingDatabaseName))
            {
                tbDatabaseName.Text = existingDatabaseName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string databaseName = tbDatabaseName.Text;
            string databaseDescription = tbDatabaseDescription.Text;
            if (String.IsNullOrWhiteSpace(databaseName))
            {
                MessageBox.Show("Please enter a database name to continue.");
                return;
            }

            if (action == "OVERWRITE")
            {
                if (File.Exists(existingDatabaseFile))
                {
                    databaseDescription = String.Format("{0}\n\n{1}", databaseDescription, Utilities.GetDatabaseDescription(databaseName));
                    DatabaseManagement.DeleteDatabaseBackup(databaseName, existingDatabaseFile, false, false);
                }
            }

            List<string> databases = DatabaseManagement.RetrieveSQLDatabases();

            SettingsModel settingsModel = SettingsUtilities.GetSettings();
            string databaseBackup = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, databaseName);
            string databaseBackupDirectory = String.Format(@"{0}\{1}", settingsModel.DbManagement.DatabaseBackupDirectory, databaseName);

            DatabaseManagement backup = new DatabaseManagement();
            backup.BackupName = databaseName;
            backup.BackupDescription = databaseDescription;
            backup.BackupLocation = databaseBackupDirectory;
            backup.Databases = databases;

            if (File.Exists(databaseBackup))
            {
                string message = String.Format("A database backup with the name '{0}' already exists. Do you want to overwrite the existing backup with the current dataset?", databaseName);
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                    DatabaseManagement.DeleteDatabaseBackup(databaseName, databaseBackup, true, false);
                else if (result == DialogResult.No)
                    return;
                else if (result == DialogResult.Cancel)
                {
                    this.Close();
                    return;
                }
            }

            Thread BackupDatabase = new Thread(() => DatabaseManagement.NewDatabase(backup, action));
            BackupDatabase.Start();
            this.Close();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            if (action == "OVERWRITE")
                Form1.s_OverwriteBackup = null;
            else
                Form1.s_NewBackup = null;
        }
    }
}
