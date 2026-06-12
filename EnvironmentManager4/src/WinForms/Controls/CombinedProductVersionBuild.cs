using EnvironmentManager4.src.Core;
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
    public partial class CombinedProductVersionBuild : UserControl
    {
        public CombinedProductVersionBuild()
        {
            InitializeComponent();

            productVersionLists1.SelectionUpdated += ProductVersionLists_SelectionUpdated;
        }

        private void ProductVersionLists_SelectionUpdated(object sender, EventArgs e)
        {
            var selector = (ProductVersionLists)sender;

            var product = selector.SelectedProduct;
            var version = selector.SelectedVersion;

            // populate the build list accordingly
            buildList1.PopulateBuildList(product, version);
            return;
        }
    }
}
