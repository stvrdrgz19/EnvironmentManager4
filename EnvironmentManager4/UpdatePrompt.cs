using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class UpdatePrompt : Form
    {
        public static bool OpenFromStartup;

        public UpdatePrompt()
        {
            InitializeComponent();
        }

        private void Update_Load(object sender, EventArgs e)
        {
            string text = "";
            if (OpenFromStartup)
                text = "There is a new version of Environment Manager available, do you want to update? Environment Manager will be closed.";
            else
                text = "Are you sure you want to install the latest version of Environment Manager? Environment Manager will be closed.";
            tbText.Text = String.Format("{0}\r\n\r\nCurrent Version: {1}\r\nNew Version: {2}",
                text,
                Utilities.GetAppVersion(),
                Utilities.GetLatestVersion());
            return;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo(Utilities.GetFile("GetLatestEnvironmentManager.bat"));
            Process.Start(info);
            this.Close();
            return;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;
            Process.Start(Utilities.GetChangeLogLink());
            return;
        }
    }
}
