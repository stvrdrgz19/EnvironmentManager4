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
    public partial class ExceptionForm : Form
    {
        public static Exception exception;
        public static string extraMessage;
        public static bool dbUpdateFail;
        public static string action;
        public static string variables;
        public ExceptionForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        private void ExceptionForm_Load(object sender, EventArgs e)
        {
            if (dbUpdateFail)
            {
                textBox1.Text = @"The database update failed. This message is also being logged to the Log.txt file in the ..\EnvMgr\Files directory.";
                tbException.Text = extraMessage;
                return;
            }
            if (!String.IsNullOrEmpty(extraMessage))
            {
                tbException.Text = String.Format("Exception Message: {0}\r\nException Type: {1}\r\nException Source: {2}\r\nException Traget Site: {3}\r\n\r\n{4}\r\n\r\nSTACK TRACE\r\n{5}",
                    exception.Message,
                    exception.GetType().ToString(),
                    exception.Source,
                    exception.TargetSite,
                    extraMessage,
                    exception.StackTrace);
                return;
            }
            tbException.Text = String.Format("Exception Message: {0}\r\nException Type: {1}\r\nException Source: {2}\r\nException Traget Site: {3}\r\n\r\nSTACK TRACE\r\n{4}",
                exception.Message,
                exception.GetType().ToString(),
                exception.Source,
                exception.TargetSite,
                exception.StackTrace);
            return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
