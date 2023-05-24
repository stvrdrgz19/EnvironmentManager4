
namespace EnvironmentManager4.Build_Management
{
    partial class DLLManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLLManager));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.cbBuilds = new System.Windows.Forms.ComboBox();
            this.lvInstalledDLLs = new System.Windows.Forms.ListView();
            this.chDLLName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.lvInstalledDLLs);
            this.groupBox1.Controls.Add(this.cbBuilds);
            this.groupBox1.Controls.Add(this.cbVersion);
            this.groupBox1.Controls.Add(this.cbProducts);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 315);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 334);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 315);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // cbProducts
            // 
            this.cbProducts.FormattingEnabled = true;
            this.cbProducts.Location = new System.Drawing.Point(7, 20);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(121, 21);
            this.cbProducts.TabIndex = 0;
            this.cbProducts.SelectedIndexChanged += new System.EventHandler(this.cbProducts_SelectedIndexChanged);
            // 
            // cbVersion
            // 
            this.cbVersion.FormattingEnabled = true;
            this.cbVersion.Items.AddRange(new object[] {
            "x86",
            "x64",
            "Pre"});
            this.cbVersion.Location = new System.Drawing.Point(134, 20);
            this.cbVersion.Name = "cbVersion";
            this.cbVersion.Size = new System.Drawing.Size(58, 21);
            this.cbVersion.TabIndex = 1;
            this.cbVersion.Text = "x86";
            this.cbVersion.SelectedIndexChanged += new System.EventHandler(this.cbVersion_SelectedIndexChanged);
            // 
            // cbBuilds
            // 
            this.cbBuilds.FormattingEnabled = true;
            this.cbBuilds.Location = new System.Drawing.Point(199, 20);
            this.cbBuilds.Name = "cbBuilds";
            this.cbBuilds.Size = new System.Drawing.Size(305, 21);
            this.cbBuilds.TabIndex = 2;
            this.cbBuilds.SelectedIndexChanged += new System.EventHandler(this.cbBuilds_SelectedIndexChanged);
            // 
            // lvInstalledDLLs
            // 
            this.lvInstalledDLLs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDLLName,
            this.chType});
            this.lvInstalledDLLs.FullRowSelect = true;
            this.lvInstalledDLLs.GridLines = true;
            this.lvInstalledDLLs.HideSelection = false;
            this.lvInstalledDLLs.Location = new System.Drawing.Point(7, 46);
            this.lvInstalledDLLs.Name = "lvInstalledDLLs";
            this.lvInstalledDLLs.Size = new System.Drawing.Size(497, 232);
            this.lvInstalledDLLs.TabIndex = 3;
            this.lvInstalledDLLs.UseCompatibleStateImageBehavior = false;
            this.lvInstalledDLLs.View = System.Windows.Forms.View.Details;
            // 
            // chDLLName
            // 
            this.chDLLName.Text = "DLL";
            this.chDLLName.Width = 375;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 118;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 285);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DLLManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 661);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DLLManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DLL Manager";
            this.Load += new System.EventHandler(this.DLLManager_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbVersion;
        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.ComboBox cbBuilds;
        private System.Windows.Forms.ListView lvInstalledDLLs;
        private System.Windows.Forms.ColumnHeader chDLLName;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.Button button1;
    }
}