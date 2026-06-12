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
    public partial class ProductVersionLists : UserControl
    {
        public ProductVersionLists()
        {
            InitializeComponent();

            cbProducts.SelectedIndexChanged += OnSelectionChanged;
            cbVersions.SelectedIndexChanged += OnSelectionChanged;
        }
        public string SelectedProduct => cbProducts.Text;
        public string SelectedVersion => cbVersions.Enabled ? cbVersions.Text : "x86";
        public event EventHandler SelectionUpdated;

        protected virtual void OnSelectionUpdated()
        {
            SelectionUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateVersionEnabledState();

            OnSelectionUpdated();
        }

        private void UpdateVersionEnabledState()
        {
            cbVersions.Enabled =
                SelectedProduct == "SalesPad" ||
                SelectedProduct == "ShipCenter";

            // update cbVersions value to x86 if Inventory Control or SalesPad Mobile
            if (SelectedProduct == Products.InventoryControl || SelectedProduct == Products.SalesPadMobile)
                cbVersions.SelectedIndex = cbVersions.FindStringExact(SelectedVersion);
        }

        private void ProductVersionLists_Load(object sender, EventArgs e)
        {
            // get and populate product and versions lists
            cbProducts.Items.AddRange(Products.All.ToArray());
            cbVersions.Items.AddRange(Versions.All.ToArray());

            // default the product select to SalesPad
            cbProducts.SelectedIndex = cbProducts.FindStringExact(Products.SalesPad);

            // default the product version to x64
            cbVersions.SelectedIndex = cbVersions.FindStringExact(Versions.X64);
            return;
        }
    }
}
