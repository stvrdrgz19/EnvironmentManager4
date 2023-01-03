using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
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

        public List<ModuleFileContents> GetModuleFileContents(string file)
        {
            List<ModuleFileContents> mfc = new List<ModuleFileContents>();
            using (ZipArchive archive = ZipFile.OpenRead(file))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    ModuleFileContents fc = new ModuleFileContents();
                    fc.ModuleName = Path.GetFileNameWithoutExtension(file);
                    fc.FileName = entry.FullName;
                    mfc.Add(fc);
                }
            }
            return mfc;
        }

        public void LoadModules()
        {
            lvBuilds.Columns.Add("ModuleName", -2);
            lvBuilds.Columns.Add("FileName", -2);

            string path = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\master\5.2.39.13\ExtModules\x64";
            List<Modules> extendedModuleList = new List<Modules>();
            foreach (string file in Directory.GetFiles(path))
            {
                Modules module = new Modules();
                module.ModuleName = Path.GetFileNameWithoutExtension(file);
                module.ModulePath = file;
                module.ModuleContents = GetModuleFileContents(file);
                extendedModuleList.Add(module);
            }

            foreach (Modules module in extendedModuleList)
            {
                foreach (ModuleFileContents mfc in module.ModuleContents)
                {
                    ListViewItem item = new ListViewItem(mfc.ModuleName.Replace("SalesPad.Module.", "").Replace(".5.2.39.X64", ""));
                    item.SubItems.Add(mfc.FileName);
                    lvBuilds.Items.Add(item);
                }
            }
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            LoadModules();
            //lvBuilds.Columns.Add("Results", -2);
            //foreach (string file in Directory.GetFiles(String.Format("{0}{1}", buildPath, "Features"), "SalesPad.Desktop.Setup.*X64.exe", SearchOption.AllDirectories))
            //    lvBuilds.Items.Add(file.Replace(Path.GetFileName(file), "").Substring(0, file.Replace(Path.GetFileName(file), "").Count()-1));
            return;
        }
    }
}
