
namespace EnvironmentManager4
{
    partial class Install
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Install));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemoveConfiguration = new System.Windows.Forms.Button();
            this.btnAddConfiguration = new System.Windows.Forms.Button();
            this.cbConfigurationList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkResetDBVersion = new System.Windows.Forms.CheckBox();
            this.checkInstallFolder = new System.Windows.Forms.CheckBox();
            this.checkRunDatabaseUpdate = new System.Windows.Forms.CheckBox();
            this.checkLaunchAfterInstall = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbInstallLocation = new System.Windows.Forms.TextBox();
            this.tbSelectedBuild = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lvCustomModules = new EnvironmentManager4.MultiSelectListView();
            this.chCustDLL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvExtendedModules = new EnvironmentManager4.MultiSelectListView();
            this.chExtendedDLL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(495, 364);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(420, 364);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRemoveConfiguration);
            this.groupBox2.Controls.Add(this.btnAddConfiguration);
            this.groupBox2.Controls.Add(this.cbConfigurationList);
            this.groupBox2.Location = new System.Drawing.Point(294, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 70);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Common Configurations";
            // 
            // btnRemoveConfiguration
            // 
            this.btnRemoveConfiguration.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveConfiguration.Image")));
            this.btnRemoveConfiguration.Location = new System.Drawing.Point(245, 28);
            this.btnRemoveConfiguration.Name = "btnRemoveConfiguration";
            this.btnRemoveConfiguration.Size = new System.Drawing.Size(23, 23);
            this.btnRemoveConfiguration.TabIndex = 9;
            this.btnRemoveConfiguration.UseVisualStyleBackColor = true;
            this.btnRemoveConfiguration.Click += new System.EventHandler(this.btnRemoveConfiguration_Click);
            // 
            // btnAddConfiguration
            // 
            this.btnAddConfiguration.Image = ((System.Drawing.Image)(resources.GetObject("btnAddConfiguration.Image")));
            this.btnAddConfiguration.Location = new System.Drawing.Point(222, 28);
            this.btnAddConfiguration.Name = "btnAddConfiguration";
            this.btnAddConfiguration.Size = new System.Drawing.Size(23, 23);
            this.btnAddConfiguration.TabIndex = 8;
            this.btnAddConfiguration.UseVisualStyleBackColor = true;
            this.btnAddConfiguration.Click += new System.EventHandler(this.btnAddConfiguration_Click);
            // 
            // cbConfigurationList
            // 
            this.cbConfigurationList.FormattingEnabled = true;
            this.cbConfigurationList.Location = new System.Drawing.Point(7, 29);
            this.cbConfigurationList.Name = "cbConfigurationList";
            this.cbConfigurationList.Size = new System.Drawing.Size(213, 21);
            this.cbConfigurationList.TabIndex = 7;
            this.cbConfigurationList.Text = "Select a Configuration";
            this.cbConfigurationList.SelectedIndexChanged += new System.EventHandler(this.cbConfigurationList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkResetDBVersion);
            this.groupBox1.Controls.Add(this.checkInstallFolder);
            this.groupBox1.Controls.Add(this.checkRunDatabaseUpdate);
            this.groupBox1.Controls.Add(this.checkLaunchAfterInstall);
            this.groupBox1.Location = new System.Drawing.Point(12, 290);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 70);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Build Options";
            // 
            // checkResetDBVersion
            // 
            this.checkResetDBVersion.AutoSize = true;
            this.checkResetDBVersion.Location = new System.Drawing.Point(132, 44);
            this.checkResetDBVersion.Name = "checkResetDBVersion";
            this.checkResetDBVersion.Size = new System.Drawing.Size(141, 17);
            this.checkResetDBVersion.TabIndex = 6;
            this.checkResetDBVersion.Text = "Reset Database Version";
            this.checkResetDBVersion.UseVisualStyleBackColor = true;
            // 
            // checkInstallFolder
            // 
            this.checkInstallFolder.AutoSize = true;
            this.checkInstallFolder.Location = new System.Drawing.Point(132, 20);
            this.checkInstallFolder.Name = "checkInstallFolder";
            this.checkInstallFolder.Size = new System.Drawing.Size(114, 17);
            this.checkInstallFolder.TabIndex = 4;
            this.checkInstallFolder.Text = "Open Install Folder";
            this.checkInstallFolder.UseVisualStyleBackColor = true;
            // 
            // checkRunDatabaseUpdate
            // 
            this.checkRunDatabaseUpdate.AutoSize = true;
            this.checkRunDatabaseUpdate.Location = new System.Drawing.Point(3, 44);
            this.checkRunDatabaseUpdate.Name = "checkRunDatabaseUpdate";
            this.checkRunDatabaseUpdate.Size = new System.Drawing.Size(133, 17);
            this.checkRunDatabaseUpdate.TabIndex = 5;
            this.checkRunDatabaseUpdate.Text = "Run Database Update";
            this.checkRunDatabaseUpdate.UseVisualStyleBackColor = true;
            // 
            // checkLaunchAfterInstall
            // 
            this.checkLaunchAfterInstall.AutoSize = true;
            this.checkLaunchAfterInstall.Location = new System.Drawing.Point(3, 20);
            this.checkLaunchAfterInstall.Name = "checkLaunchAfterInstall";
            this.checkLaunchAfterInstall.Size = new System.Drawing.Size(117, 17);
            this.checkLaunchAfterInstall.TabIndex = 3;
            this.checkLaunchAfterInstall.Text = "Launch After Install";
            this.checkLaunchAfterInstall.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(410, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Custom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Extended";
            // 
            // tbInstallLocation
            // 
            this.tbInstallLocation.Location = new System.Drawing.Point(12, 45);
            this.tbInstallLocation.Name = "tbInstallLocation";
            this.tbInstallLocation.Size = new System.Drawing.Size(556, 20);
            this.tbInstallLocation.TabIndex = 2;
            // 
            // tbSelectedBuild
            // 
            this.tbSelectedBuild.Location = new System.Drawing.Point(12, 23);
            this.tbSelectedBuild.Name = "tbSelectedBuild";
            this.tbSelectedBuild.ReadOnly = true;
            this.tbSelectedBuild.Size = new System.Drawing.Size(556, 20);
            this.tbSelectedBuild.TabIndex = 31;
            this.tbSelectedBuild.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Please enter the location you would like to install the following build to:";
            // 
            // lvCustomModules
            // 
            this.lvCustomModules.CheckBoxes = true;
            this.lvCustomModules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chCustDLL});
            this.lvCustomModules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvCustomModules.HideSelection = false;
            this.lvCustomModules.Location = new System.Drawing.Point(294, 84);
            this.lvCustomModules.Name = "lvCustomModules";
            this.lvCustomModules.Size = new System.Drawing.Size(274, 199);
            this.lvCustomModules.TabIndex = 36;
            this.lvCustomModules.UseCompatibleStateImageBehavior = false;
            this.lvCustomModules.View = System.Windows.Forms.View.Details;
            this.lvCustomModules.SelectedIndexChanged += new System.EventHandler(this.lvCustomModules_SelectedIndexChanged);
            // 
            // chCustDLL
            // 
            this.chCustDLL.Width = 252;
            // 
            // lvExtendedModules
            // 
            this.lvExtendedModules.CheckBoxes = true;
            this.lvExtendedModules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chExtendedDLL});
            this.lvExtendedModules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvExtendedModules.HideSelection = false;
            this.lvExtendedModules.Location = new System.Drawing.Point(12, 84);
            this.lvExtendedModules.Name = "lvExtendedModules";
            this.lvExtendedModules.Size = new System.Drawing.Size(274, 199);
            this.lvExtendedModules.TabIndex = 37;
            this.lvExtendedModules.UseCompatibleStateImageBehavior = false;
            this.lvExtendedModules.View = System.Windows.Forms.View.Details;
            this.lvExtendedModules.SelectedIndexChanged += new System.EventHandler(this.lvExtendedModules_SelectedIndexChanged);
            // 
            // chExtendedDLL
            // 
            this.chExtendedDLL.Width = 252;
            // 
            // Install
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(581, 395);
            this.Controls.Add(this.lvExtendedModules);
            this.Controls.Add(this.lvCustomModules);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbInstallLocation);
            this.Controls.Add(this.tbSelectedBuild);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Install";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Install";
            this.Load += new System.EventHandler(this.Install_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemoveConfiguration;
        private System.Windows.Forms.Button btnAddConfiguration;
        private System.Windows.Forms.ComboBox cbConfigurationList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkResetDBVersion;
        private System.Windows.Forms.CheckBox checkInstallFolder;
        private System.Windows.Forms.CheckBox checkRunDatabaseUpdate;
        private System.Windows.Forms.CheckBox checkLaunchAfterInstall;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbInstallLocation;
        private System.Windows.Forms.TextBox tbSelectedBuild;
        private System.Windows.Forms.Label label1;
        private MultiSelectListView lvCustomModules;
        private System.Windows.Forms.ColumnHeader chCustDLL;
        private MultiSelectListView lvExtendedModules;
        private System.Windows.Forms.ColumnHeader chExtendedDLL;
    }
}