using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class UpdateDatabaseDescription : Form
    {
        public UpdateDatabaseDescription()
        {
            InitializeComponent();
        }

        public static DatabaseManagement backupConfig = new DatabaseManagement();

        private void UpdateDatabaseDescription_Load(object sender, EventArgs e)
        {
            tbDatabaseName.Text = backupConfig.BackupName;
            tbDatabaseDescription.Text = backupConfig.BackupDescription;
            return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newDesc = tbDatabaseDescription.Text;
            if (String.IsNullOrWhiteSpace(newDesc))
            {
                string message = "The description for this backup is blank. Are you sure you want to save a blank description for this backup?";
                string caption = "CONFIRM";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.No)
                    return;
            }
            if (newDesc == backupConfig.BackupDescription)
                this.Close();
            SettingsModel settings = SettingsUtilities.GetSettings();
            string zipPath = String.Format(@"{0}\{1}.zip", settings.DbManagement.DatabaseBackupDirectory, backupConfig.BackupName);
            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive updateArchive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = updateArchive.GetEntry("Description.txt");
                    entry.Delete();

                    ZipArchiveEntry description = updateArchive.CreateEntry("Description.txt");
                    using (StreamWriter writer = new StreamWriter(description.Open()))
                    {
                        writer.Write(newDesc);
                    }
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
