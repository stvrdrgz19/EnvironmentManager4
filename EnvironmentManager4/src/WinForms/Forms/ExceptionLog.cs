using EnvironmentManager4.src.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4.ErrorManagement
{
    public partial class ExceptionLog : Form
    {
        public ExceptionLog()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);

            tbLog.TabStop = false;  
        }

        private void ExceptionLog_Load(object sender, EventArgs e)
        {
            string logFile = DirectoryUtilities.GetFile("Log.txt");

            // 1. Read existing log (if it exists)
            string logContents = File.Exists(logFile)
                ? File.ReadAllText(logFile)
                : string.Empty;

            tbLog.Text = logContents;
            return;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string fileToOpen = DirectoryUtilities.GetFile("Log.txt");

            try
            {
                // Use the operating system shell to open the file with its default application
                var process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true, // Essential for using default file associations
                    FileName = fileToOpen
                };

                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching file: {ex.Message}");
            }
            return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Form1.s_ExceptionLog = null;
        }
    }
}
