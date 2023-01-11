using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class DatabaseActivityLog : Form
    {
        public DatabaseActivityLog()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        private void ReloadDatabaseActivityLog()
        {
            lvDatabaseActivityLog.Items.Clear();
            List<DatabaseActivityLogModel> databaseActivity = SqliteDataAccess.LoadDatabaseActivity();
            foreach (var entry in databaseActivity)
            {
                ListViewItem row = new ListViewItem(entry.TimeStamp);
                row.SubItems.Add(entry.Action);
                row.SubItems.Add(entry.Backup);
                lvDatabaseActivityLog.Items.Add(row);
            }
            Utilities.ResizeListViewColumnWidthForScrollBar(lvDatabaseActivityLog, 15, 2);
        }

        private void DatabaseActivityLog_Load(object sender, EventArgs e)
        {
            ReloadDatabaseActivityLog();
            return;
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_DbLog = null;
        }
    }
}
