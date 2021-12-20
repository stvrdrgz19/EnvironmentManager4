using Newtonsoft.Json;
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
        }

        public static string existingDatabaseName;
        public static string existingDatabaseFile;

        private void NewDatabaseBackup_Load(object sender, EventArgs e)
        {
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

            if (File.Exists(existingDatabaseFile))
            {
                DatabaseManagement.DeleteDatabase(existingDatabaseFile);
            }

            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            string databaseBackup = String.Format(@"{0}\{1}.zip", settingsModel.DbManagement.DatabaseBackupDirectory, databaseName);
            string databaseBackupDirectory = String.Format(@"{0}\{1}", settingsModel.DbManagement.DatabaseBackupDirectory, databaseName);
            if (File.Exists(databaseBackup))
            {
                string message = String.Format("A database backup with the name '{0}' already exists. Do you want to overwrite the existing backup with the current dataset?", databaseName);
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    DatabaseManagement.DeleteDatabase(databaseBackup);
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
                else if (result == DialogResult.Cancel)
                {
                    this.Close();
                    return;
                }
            }
            Thread newDatabaseBackup = new Thread(() => DatabaseManagement.NewDatabase(databaseName, databaseDescription, databaseBackupDirectory));
            newDatabaseBackup.Start();
            this.Close();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
