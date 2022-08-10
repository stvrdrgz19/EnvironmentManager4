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
        public ExceptionForm()
        {
            InitializeComponent();
        }

        private void ExceptionForm_Load(object sender, EventArgs e)
        {
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
    }
}
