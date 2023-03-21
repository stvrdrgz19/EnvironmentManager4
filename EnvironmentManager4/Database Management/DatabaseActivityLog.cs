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
        private int sortColumn = -1;

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
            this.lvDatabaseActivityLog.ColumnClick += new ColumnClickEventHandler(ColumnClick);
            return;
        }

        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.  
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.  
                sortColumn = e.Column;
                // Set the sort order to ascending by default.  
                lvDatabaseActivityLog.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.  
                if (lvDatabaseActivityLog.Sorting == SortOrder.Ascending)
                    lvDatabaseActivityLog.Sorting = SortOrder.Descending;
                else
                    lvDatabaseActivityLog.Sorting = SortOrder.Ascending;
            }
            // Call the sort method to manually sort.  
            lvDatabaseActivityLog.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer  
            // object.  
            this.lvDatabaseActivityLog.ListViewItemSorter = new ListViewItemComparer(e.Column, lvDatabaseActivityLog.Sorting);
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_DbLog = null;
        }
    }
}
