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
    public partial class Notes : Form
    {
        public Notes()
        {
            InitializeComponent();
        }

        const string divider = "=================================================================================";
        private string noteContents = "";

        private void LoadNotes()
        {
            string notesFile = Utilities.GetFile("Notes.txt");
            if (!File.Exists(notesFile))
            {
                string message = "There is no notes file to load notes from. Would you like to create a notes file?";
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    var createNoteFile = File.Create(notesFile);
                    createNoteFile.Close();
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            tbNotes.Clear();
            tbNotes.Text = File.ReadAllText(notesFile);
            noteContents = File.ReadAllText(notesFile);
            this.FormClosing += new FormClosingEventHandler(Notes_FormClosing);
        }

        private void SaveNotes(string contents)
        {
            string notesFile = Utilities.GetFile("Notes.txt");
            var notesPathFile = File.Create(notesFile);
            notesPathFile.Close();
            using (StreamWriter sw = File.AppendText(notesFile))
            {
                sw.Write(contents.TrimEnd('\r', '\n'));
            }
            LoadNotes();
        }

        private void Notes_Load(object sender, EventArgs e)
        {
            LoadNotes();
            return;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadNotes();
            return;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string notesFile = Utilities.GetFile("Notes.txt");
            string textToAdd = divider + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + tbAddNotes.Text;
            string oldFileContents = File.ReadAllText(notesFile);
            SaveNotes(textToAdd + Environment.NewLine + oldFileContents);
            tbAddNotes.Clear();
            return;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (File.Exists(Utilities.GetFile("Notes.txt")))
            {
                if (tbNotes.ReadOnly)
                {
                    tbNotes.ReadOnly = false;
                    btnEdit.Text = "Save File";
                    return;
                }
                if (!tbNotes.ReadOnly)
                {
                    tbNotes.ReadOnly = true;
                    btnEdit.Text = "Edit File";
                    SaveNotes(tbNotes.Text);
                    return;
                }
            }
        }

        private void Notes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tbNotes.Text != noteContents)
            {
                string message = "There are un-saved changes to the notes. Do you want to save these changes to the notes file?";
                string caption = "UN-SAVED CHANGES";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Exclamation;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                if (result == DialogResult.Yes)
                {
                    SaveNotes(tbNotes.Text);
                }
                else if (result == DialogResult.No)
                {
                    //don't save the changes
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                return;
            }
            Form1.notes = null;
        }
    }
}
