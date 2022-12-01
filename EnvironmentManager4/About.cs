using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        private void About_Load(object sender, EventArgs e)
        {
            labelVersion.Text = String.Format("Environment Manager v{0}", Utilities.GetAppVersion());
            if (Utilities.IsProgramUpToDate())
            {
                labelIsAppUpdated.Text = "Environment Manager is up to date.";
                btnUpdate.Enabled = false;
            }
            else
                labelIsAppUpdated.Text = "There is a new version of Environment Manager available.";
            return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkProject.LinkVisited = true;
            Process.Start(Utilities.GetProjectLink());
            return;
        }

        private void linkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkWiki.LinkVisited = true;
            Process.Start(Utilities.GetWikiLink());
            return;
        }

        private void linkRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkRepo.LinkVisited = true;
            Process.Start(Utilities.GetRepoLink());
            return;
        }

        private void LinkChangeLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LinkChangeLog.LinkVisited = true;
            Process.Start(Utilities.GetChangeLogLink());
            return;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdatePrompt update = new UpdatePrompt();
            UpdatePrompt.OpenFromStartup = false;
            update.ShowDialog();
            return;
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_AboutForm = null;
        }
    }
}
