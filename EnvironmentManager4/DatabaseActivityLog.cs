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
        }

        private void ReloadDatabaseActivityLog()
        {
            List<DatabaseActivityLogModel> databaseActivity = new List<DatabaseActivityLogModel>();
            lvDatabaseActivityLog.Items.Clear();
            databaseActivity = SqliteDataAccess.LoadDatabaseActivity();
            foreach (var activity in databaseActivity)
            {
                ListViewItem item1 = new ListViewItem(activity.Created_On);
                item1.SubItems.Add(activity.Action);
                item1.SubItems.Add(activity.Backup);
                lvDatabaseActivityLog.Items.Add(item1);
            }
        }

        private void DatabaseActivityLog_Load(object sender, EventArgs e)
        {
            ReloadDatabaseActivityLog();
            return;
        }
    }
}
