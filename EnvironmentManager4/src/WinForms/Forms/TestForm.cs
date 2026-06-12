using EnvironmentManager4.src.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4.src.WinForms.Controls
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            string product = Products.SalesPad;
            string version = Versions.X64;
            buildList1.SelectedProduct = product;
            buildList1.SelectedVersion = version;
            buildList1.PopulateBuildList(product, version);
        }

        private void buildList1_Load(object sender, EventArgs e)
        {
            //buildList1.SelectedProduct = Products.SalesPad;
            //buildList1.SelectedVersion = Versions.X64;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedProduct = productVersionLists1.SelectedProduct;
            string selectedVersion = productVersionLists1.SelectedVersion;
            MessageBox.Show($"Product: {selectedProduct}\n\nVersion: {selectedVersion}");
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> selectedBuilds = buildList1.SelectedBuilds;
            string listOfBuilds = String.Join(Environment.NewLine, selectedBuilds);
            MessageBox.Show($"Selected Builds:\n\n{listOfBuilds}");
            return;
        }
    }
}
