using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class DirectoryCompare : Form
    {
        public DirectoryCompare()
        {
            InitializeComponent();
        }

        public const string path1Load = "Enter/Select Directory 1...";
        public const string path2Load = "Enter/Select Directory 2...";
        public bool checkSubDirectories = true;

        public void SetDefaultValues()
        {
            tbPath1.Text = path1Load;
            tbPath2.Text = path2Load;
        }

        public void LoadListCounts(TextBox tb, ListBox lb = null, ListView lv = null)
        {
            if (lb != null)
                tb.Text = String.Format("{0} files", lb.Items.Count);

            if (lv != null)
                tb.Text = String.Format("{0} files", lv.Items.Count);
        }

        public void PopulateListBoxes(string dir, ListBox lb, TextBox tb)
        {
            lb.Items.Clear();
            if (Directory.Exists(dir))
            {
                SearchOption so = SearchOption.AllDirectories;
                switch (checkSubDirectories)
                {
                    case true:
                        so = SearchOption.AllDirectories;
                        break;
                    case false:
                        so = SearchOption.TopDirectoryOnly;
                        break;
                }

                foreach (string file in Directory.GetFiles(dir, "*", so))
                    lb.Items.Add(Path.GetFileName(file));
            }
            LoadListCounts(tb, lb);
        }

        public void CompareResults()
        {
            if (lbList1.Items.Count == 0 || lbList2.Items.Count == 0)
                return;

            List<string> list1 = lbList1.Items.OfType<string>().ToList();
            List<string> list2 = lbList2.Items.OfType<string>().ToList();

            lvResults.Columns.Clear();
            lvResults.Items.Clear();

            if (checkMissing.Checked)
            {
                lvResults.Columns.Add("File Name", 410);
                lvResults.Columns.Add("Not found in", 410);

                List<string> uniques1 = list1.Except(list2).ToList();
                List<string> uniques2 = list2.Except(list1).ToList();

                foreach (string file in uniques1)
                {
                    ListViewItem item = new ListViewItem(file);
                    item.SubItems.Add(tbPath2.Text);
                    lvResults.Items.Add(item);
                }
                foreach (string file in uniques2)
                {
                    ListViewItem item = new ListViewItem(file);
                    item.SubItems.Add(tbPath1.Text);
                    lvResults.Items.Add(item);
                }
                Utilities.ResizeUpdateableListViewColumnWidthForScrollBar(lvResults, 11, 1, 410);
            }

            if (checkDuplicates.Checked)
            {
                lvResults.Columns.Add("File Name", -2);
                List<string> duplicates = list1.Intersect(list2).ToList();
                foreach (string file in duplicates)
                {
                    ListViewItem item = new ListViewItem(file);
                    lvResults.Items.Add(item);
                }
                Utilities.ResizeUpdateableListViewColumnWidthForScrollBar(lvResults, 11, 0, 820);
            }

            LoadListCounts(tbResultsCount, null, lvResults);
        }

        private void DirectoryCompare_Load(object sender, EventArgs e)
        {
            SetDefaultValues();
            LoadListCounts(tbList1Count, lbList1);
            LoadListCounts(tbList2Count, lbList2);
            LoadListCounts(tbResultsCount, null, lvResults);
            return;
        }

        private void btnPath1_Click(object sender, EventArgs e)
        {
            string path = Utilities.GetDirectory(tbPath1.Text);
            tbPath1.Text = path;
            PopulateListBoxes(path, lbList1, tbList1Count);
            CompareResults();
            return;
        }

        private void tbPath1_TextChanged(object sender, EventArgs e)
        {
            PopulateListBoxes(tbPath1.Text, lbList1, tbList1Count);
            CompareResults();
            return;
        }

        private void btnPath2_Click(object sender, EventArgs e)
        {
            string path = Utilities.GetDirectory(tbPath2.Text);
            tbPath2.Text = path;
            PopulateListBoxes(path, lbList2, tbList2Count);
            CompareResults();
            return;
        }

        private void tbPath2_TextChanged(object sender, EventArgs e)
        {
            PopulateListBoxes(tbPath2.Text, lbList2, tbList2Count);
            CompareResults();
            return;
        }

        private void checkMissing_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkMissing.Checked;
            if (isChecked)
            {
                checkDuplicates.Checked = false;
                CompareResults();
            }
            return;
        }

        private void checkDuplicates_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkDuplicates.Checked;
            if (isChecked)
            {
                checkMissing.Checked = false;
                CompareResults();
            }
            return;
        }

        private void checkIncludeSubDirectories_CheckedChanged(object sender, EventArgs e)
        {
            if (checkIncludeSubDirectories.Checked)
                checkSubDirectories = true;
            else
                checkSubDirectories = false;

            PopulateListBoxes(tbPath1.Text, lbList1, tbList1Count);
            PopulateListBoxes(tbPath2.Text, lbList2, tbList2Count);
            CompareResults();
            return;
        }

        private void btnCopySelected_Click(object sender, EventArgs e)
        {
            int count = lvResults.SelectedItems.Count;
            if (count == 0)
                return;

            StringBuilder builder = new StringBuilder();
            foreach (ListViewItem lvi in lvResults.SelectedItems)
                builder.Append(lvi.SubItems[0].Text).AppendLine();

            Clipboard.SetText(builder.ToString());

            string message = String.Format("({0}) files were copied to the clipboard.", count);
            if (count <= 10)
                message = String.Format("The following ({0}) file(s) were copied to the clipboard:\n\n{1}", count, builder.ToString());
            string caption = "COPIED";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            MessageBox.Show(message, caption, buttons, icon);
            return;
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            int count = lvResults.Items.Count;
            if (count == 0)
                return;

            StringBuilder builder = new StringBuilder();
            foreach (ListViewItem lvi in lvResults.Items)
                builder.Append(lvi.SubItems[0].Text).AppendLine();

            Clipboard.SetText(builder.ToString());

            string message = String.Format("({0}) files were copied to the clipboard.", count);
            if (count <= 10)
                message = String.Format("The following ({0}) file(s) were copied to the clipboard:\n\n{1}", count, builder.ToString());
            string caption = "COPIED";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;

            MessageBox.Show(message, caption, buttons, icon);
            return;

        }

        private void btnCompareLists_Click(object sender, EventArgs e)
        {
            CompareResults();
            return;
        }
    }
}
