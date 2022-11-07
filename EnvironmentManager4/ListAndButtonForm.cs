using Newtonsoft.Json;
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
    public partial class ListAndButtonForm : Form
    {
        public ListAndButtonForm()
        {
            InitializeComponent();
        }

        public static string title = "";
        public static string button = "";
        public static string output = "";

        public void LoadDatabases()
        {
            listBox1.Items.AddRange(DatabaseManagement.GetCompanyDatabases().ToArray());
            return;
        }

        private void ListAndButtonForm_Load(object sender, EventArgs e)
        {
            this.Text = title;
            button1.Text = button;
            LoadDatabases();
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            output = listBox1.Text;
            this.Close();
            return;
        }
    }
}
