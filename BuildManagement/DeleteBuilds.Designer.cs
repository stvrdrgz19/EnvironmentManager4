
namespace BuildManagement
{
    partial class DeleteBuilds
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvInstalledBuilds = new System.Windows.Forms.ListView();
            this.chInstallPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDateModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvInstalledBuilds);
            this.groupBox1.Controls.Add(this.cbVersion);
            this.groupBox1.Controls.Add(this.cbProducts);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnSelectNone);
            this.groupBox1.Controls.Add(this.btnSelectAll);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 223);
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
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDelete.Location = new System.Drawing.Point(450, 196);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(221, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSelectNone.Location = new System.Drawing.Point(228, 196);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(221, 23);
            this.btnSelectNone.TabIndex = 3;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSelectAll.Location = new System.Drawing.Point(6, 196);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(221, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // DeleteBuilds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 227);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DeleteBuilds";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delete Builds";
            this.Load += new System.EventHandler(this.DeleteBuilds_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvInstalledBuilds;
        private System.Windows.Forms.ColumnHeader chInstallPath;
        private System.Windows.Forms.ColumnHeader chDateModified;
        private System.Windows.Forms.ComboBox cbVersion;
        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
    }
}