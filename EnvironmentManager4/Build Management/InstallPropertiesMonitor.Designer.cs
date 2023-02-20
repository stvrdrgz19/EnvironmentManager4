
namespace EnvironmentManager4.Build_Management
{
    partial class InstallPropertiesMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallPropertiesMonitor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvInstalledBuilds = new System.Windows.Forms.ListView();
            this.chInstallPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDateModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvInstallProperties = new System.Windows.Forms.ListView();
            this.tbInstallPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBuildPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chDLLName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvInstalledBuilds);
            this.groupBox1.Controls.Add(this.cbVersion);
            this.groupBox1.Controls.Add(this.cbProducts);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(2, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 202);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Builds";
            // 
            // lvInstalledBuilds
            // 
            this.lvInstalledBuilds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chInstallPath,
            this.chDateModified});
            this.lvInstalledBuilds.FullRowSelect = true;
            this.lvInstalledBuilds.GridLines = true;
            this.lvInstalledBuilds.HideSelection = false;
            this.lvInstalledBuilds.Location = new System.Drawing.Point(7, 48);
            this.lvInstalledBuilds.Name = "lvInstalledBuilds";
            this.lvInstalledBuilds.Size = new System.Drawing.Size(663, 148);
            this.lvInstalledBuilds.TabIndex = 14;
            this.lvInstalledBuilds.TabStop = false;
            this.lvInstalledBuilds.UseCompatibleStateImageBehavior = false;
            this.lvInstalledBuilds.View = System.Windows.Forms.View.Details;
            this.lvInstalledBuilds.SelectedIndexChanged += new System.EventHandler(this.lvInstalledBuilds_SelectedIndexChanged);
            // 
            // chInstallPath
            // 
            this.chInstallPath.Text = "Install Path";
            this.chInstallPath.Width = 500;
            // 
            // chDateModified
            // 
            this.chDateModified.Text = "Date Modified";
            this.chDateModified.Width = 159;
            // 
            // cbVersion
            // 
            this.cbVersion.FormattingEnabled = true;
            this.cbVersion.Items.AddRange(new object[] {
            "x86",
            "x64"});
            this.cbVersion.Location = new System.Drawing.Point(549, 20);
            this.cbVersion.Name = "cbVersion";
            this.cbVersion.Size = new System.Drawing.Size(121, 21);
            this.cbVersion.TabIndex = 1;
            this.cbVersion.Text = "x86";
            this.cbVersion.SelectedIndexChanged += new System.EventHandler(this.cbVersion_SelectedIndexChanged);
            // 
            // cbProducts
            // 
            this.cbProducts.FormattingEnabled = true;
            this.cbProducts.Items.AddRange(new object[] {
            "SalesPad GP",
            "DataCollection",
            "SalesPad Mobile",
            "ShipCenter"});
            this.cbProducts.Location = new System.Drawing.Point(7, 20);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(538, 21);
            this.cbProducts.TabIndex = 0;
            this.cbProducts.Text = "Select a Product...";
            this.cbProducts.SelectedIndexChanged += new System.EventHandler(this.cbProducts_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvInstallProperties);
            this.groupBox2.Controls.Add(this.tbInstallPath);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbBuildPath);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbVersion);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbProduct);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(2, 209);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(677, 335);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Install Properties";
            // 
            // lvInstallProperties
            // 
            this.lvInstallProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDLLName,
            this.chFileName});
            this.lvInstallProperties.FullRowSelect = true;
            this.lvInstallProperties.GridLines = true;
            this.lvInstallProperties.HideSelection = false;
            this.lvInstallProperties.Location = new System.Drawing.Point(7, 112);
            this.lvInstallProperties.Name = "lvInstallProperties";
            this.lvInstallProperties.Size = new System.Drawing.Size(663, 217);
            this.lvInstallProperties.TabIndex = 8;
            this.lvInstallProperties.UseCompatibleStateImageBehavior = false;
            this.lvInstallProperties.View = System.Windows.Forms.View.Details;
            // 
            // tbInstallPath
            // 
            this.tbInstallPath.Location = new System.Drawing.Point(72, 86);
            this.tbInstallPath.Name = "tbInstallPath";
            this.tbInstallPath.ReadOnly = true;
            this.tbInstallPath.Size = new System.Drawing.Size(598, 20);
            this.tbInstallPath.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label3.Location = new System.Drawing.Point(7, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Install Path";
            // 
            // tbBuildPath
            // 
            this.tbBuildPath.Location = new System.Drawing.Point(72, 64);
            this.tbBuildPath.Name = "tbBuildPath";
            this.tbBuildPath.ReadOnly = true;
            this.tbBuildPath.Size = new System.Drawing.Size(598, 20);
            this.tbBuildPath.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label4.Location = new System.Drawing.Point(7, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Build Path";
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(72, 42);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.ReadOnly = true;
            this.tbVersion.Size = new System.Drawing.Size(598, 20);
            this.tbVersion.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Version";
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(72, 20);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.ReadOnly = true;
            this.tbProduct.Size = new System.Drawing.Size(598, 20);
            this.tbProduct.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Product";
            // 
            // chDLLName
            // 
            this.chDLLName.Text = "DLL";
            this.chDLLName.Width = 330;
            // 
            // chFileName
            // 
            this.chFileName.Text = "File";
            this.chFileName.Width = 329;
            // 
            // InstallPropertiesMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 546);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "InstallPropertiesMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Install Properties Monitor";
            this.Load += new System.EventHandler(this.InstallPropertiesMonitor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvInstalledBuilds;
        private System.Windows.Forms.ColumnHeader chInstallPath;
        private System.Windows.Forms.ColumnHeader chDateModified;
        private System.Windows.Forms.ComboBox cbVersion;
        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvInstallProperties;
        private System.Windows.Forms.TextBox tbInstallPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbBuildPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chDLLName;
        private System.Windows.Forms.ColumnHeader chFileName;
    }
}