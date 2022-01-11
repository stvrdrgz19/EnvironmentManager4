
namespace EnvironmentManager4
{
    partial class LaunchProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaunchProduct));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Launch = new System.Windows.Forms.Button();
            this.InstalledBuilds = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RemoveDLLs = new System.Windows.Forms.Button();
            this.CopyLabels = new System.Windows.Forms.Button();
            this.SelectedBuildDLLs = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Launch);
            this.groupBox1.Controls.Add(this.InstalledBuilds);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 208);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Builds";
            // 
            // Launch
            // 
            this.Launch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Launch.Location = new System.Drawing.Point(7, 174);
            this.Launch.Name = "Launch";
            this.Launch.Size = new System.Drawing.Size(550, 23);
            this.Launch.TabIndex = 1;
            this.Launch.Text = "Launch";
            this.Launch.UseVisualStyleBackColor = true;
            this.Launch.Click += new System.EventHandler(this.Launch_Click);
            // 
            // InstalledBuilds
            // 
            this.InstalledBuilds.FormattingEnabled = true;
            this.InstalledBuilds.Location = new System.Drawing.Point(7, 20);
            this.InstalledBuilds.Name = "InstalledBuilds";
            this.InstalledBuilds.Size = new System.Drawing.Size(550, 147);
            this.InstalledBuilds.TabIndex = 0;
            this.InstalledBuilds.SelectedIndexChanged += new System.EventHandler(this.InstalledBuilds_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RemoveDLLs);
            this.groupBox2.Controls.Add(this.CopyLabels);
            this.groupBox2.Controls.Add(this.SelectedBuildDLLs);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(2, 216);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(565, 208);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DLLs";
            // 
            // RemoveDLLs
            // 
            this.RemoveDLLs.Enabled = false;
            this.RemoveDLLs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RemoveDLLs.Location = new System.Drawing.Point(284, 174);
            this.RemoveDLLs.Name = "RemoveDLLs";
            this.RemoveDLLs.Size = new System.Drawing.Size(273, 23);
            this.RemoveDLLs.TabIndex = 2;
            this.RemoveDLLs.Text = "Remove DLL(s)";
            this.RemoveDLLs.UseVisualStyleBackColor = true;
            this.RemoveDLLs.Click += new System.EventHandler(this.RemoveDLLs_Click);
            // 
            // CopyLabels
            // 
            this.CopyLabels.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CopyLabels.Location = new System.Drawing.Point(7, 174);
            this.CopyLabels.Name = "CopyLabels";
            this.CopyLabels.Size = new System.Drawing.Size(273, 23);
            this.CopyLabels.TabIndex = 1;
            this.CopyLabels.Text = "Copy Label(s)";
            this.CopyLabels.UseVisualStyleBackColor = true;
            this.CopyLabels.Click += new System.EventHandler(this.CopyLabels_Click);
            // 
            // SelectedBuildDLLs
            // 
            this.SelectedBuildDLLs.FormattingEnabled = true;
            this.SelectedBuildDLLs.Location = new System.Drawing.Point(7, 20);
            this.SelectedBuildDLLs.Name = "SelectedBuildDLLs";
            this.SelectedBuildDLLs.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.SelectedBuildDLLs.Size = new System.Drawing.Size(550, 147);
            this.SelectedBuildDLLs.TabIndex = 0;
            // 
            // LaunchProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 426);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LaunchProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launch Product";
            this.Load += new System.EventHandler(this.LaunchProduct_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Launch;
        private System.Windows.Forms.ListBox InstalledBuilds;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button RemoveDLLs;
        private System.Windows.Forms.Button CopyLabels;
        private System.Windows.Forms.ListBox SelectedBuildDLLs;
    }
}