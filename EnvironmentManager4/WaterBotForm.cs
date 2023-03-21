using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class WaterBotForm : Form
    {
        public WaterBotForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.FormIsClosing);
        }

        //private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private void WaterBotForm_Load(object sender, EventArgs e)
        {
            //timer.Interval = 3000;
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
            this.BringToFront();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //timer.Stop();
            //WaterBot.s_waterBot = null;
            this.Close();
            return;
        }

        private void FormIsClosing(object sender, FormClosingEventArgs eventArgs)
        {
            WaterBot.s_waterBot = null;
        }
    }
}
