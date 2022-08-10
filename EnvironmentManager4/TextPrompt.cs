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
    public partial class TextPrompt : Form
    {
        public TextPrompt()
        {
            InitializeComponent();
        }

        public static string title = "";
        public static string label = "";
        public static string output = "";
        public static bool isConfiguration = false;
        public static List<string> extended = new List<string>();
        public static List<string> custom = new List<string>();
        public static string product = "";

        private void TextPrompt_Load(object sender, EventArgs e)
        {
            this.Text = title;
            labelPrompt.Text = label;
            return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (isConfiguration)
            {
                output = tbText.Text;
                Configurations.SaveConfiguration(new Configurations(product, output, extended, custom));
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
