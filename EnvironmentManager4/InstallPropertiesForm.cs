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
    public partial class InstallPropertiesForm : Form
    {
        public InstallPropertiesForm()
        {
            InitializeComponent();
        }

        public static string path = "";

        public void PopulateForm(string path)
        {
            InstallProperties installProperties = InstallProperties.RetrieveInstallProperties(path);
            tbProduct.Text = installProperties.Product;
            tbVersion.Text = installProperties.Version;

            List<DLLFileModel> extendedDLLs = installProperties.ExtendedDLLs;
            List<DLLFileModel> customDLLs = installProperties.CustomDLLs;

            foreach (DLLFileModel dll in extendedDLLs)
                lbExtended.Items.Add(dll.CoreDLL);

            foreach (DLLFileModel dll in customDLLs)
                lbCustom.Items.Add(dll.CoreDLL);
        }

        private void InstallPropertiesForm_Load(object sender, EventArgs e)
        {
            PopulateForm(path);
            return;
        }
    }
}
