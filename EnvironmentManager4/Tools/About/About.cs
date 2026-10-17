using EnvironmentManager4.src.Core.Utilities;
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
            labelVersion.Text = String.Format("Environment Manager v{0}", AppVersionUtilities.GetAppVersion());
            if (AppVersionUtilities.IsUpToDate())
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

        private void linkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkWiki.LinkVisited = true;
            Process.Start(new ProcessStartInfo(UtilitiesOld.GetWikiLink()) { UseShellExecute = true });
            return;
        }

        private void linkRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkRepo.LinkVisited = true;
            Process.Start(new ProcessStartInfo(UtilitiesOld.GetRepoLink()) { UseShellExecute = true });
            return;
        }

        private void LinkChangeLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LinkChangeLog.LinkVisited = true;
            Process.Start(new ProcessStartInfo(UtilitiesOld.GetChangeLogLink()) { UseShellExecute = true });
            return;
        }

        private void linkDev1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkDev1.LinkVisited = true;
            Process.Start(new ProcessStartInfo(UtilitiesOld.GetDev1()) { UseShellExecute = true });
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
