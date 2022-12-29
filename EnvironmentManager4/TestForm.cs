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
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        public const string buildPath = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\";
        public string[] buildFolders = new string[] {
            "CustomDev"
            ,"Customers"
            ,"Featuers"
            ,"Features"
            ,"master"
            ,"ProductDev"
            ,"Release"};

        public void GetTheFiles()
        {
            foreach (string folder in buildFolders)
                foreach (string file in Directory.GetFiles(String.Format("{0}{1}", buildPath, folder), "SalesPad.Desktop.Setup.*X64.exe", SearchOption.AllDirectories))
                    lvBuilds.Items.Add(file.Replace(Path.GetFileName(file), ""));
            return;
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            lvBuilds.Columns.Add("Results", -2);
            foreach (string file in Directory.GetFiles(String.Format("{0}{1}", buildPath, "Features"), "SalesPad.Desktop.Setup.*X64.exe", SearchOption.AllDirectories))
                lvBuilds.Items.Add(file.Replace(Path.GetFileName(file), "").Substring(0, file.Replace(Path.GetFileName(file), "").Count()-1));
            return;
        }
    }
}
