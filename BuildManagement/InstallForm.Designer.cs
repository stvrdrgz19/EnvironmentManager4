
namespace BuildManagement
{
    partial class InstallForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallForm));
            this.lbCustomModules = new System.Windows.Forms.ListBox();
            this.lbExtendedModules = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemoveConfiguration = new System.Windows.Forms.Button();
            this.btnAddConfiguration = new System.Windows.Forms.Button();
            this.cbConfigurationList = new System.Windows.Forms.ComboBox();
            this.checkResetDBVersion = new System.Windows.Forms.CheckBox();
            this.checkInstallFolder = new System.Windows.Forms.CheckBox();
            this.checkRunDatabaseUpdate = new System.Windows.Forms.CheckBox();
            this.checkLaunchAfterInstall = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbInstallLocation = new System.Windows.Forms.TextBox();
            this.tbSelectedBuild = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCustomModules
            // 
            this.lbCustomModules.FormattingEnabled = true;
            this.lbCustomModules.Location = new System.Drawing.Point(294, 84);
            this.lbCustomModules.Name = "lbCustomModules";
            this.lbCustomModules.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbCustomModules.Size = new System.Drawing.Size(274, 199);
            this.lbCustomModules.TabIndex = 45;
            this.lbCustomModules.TabStop = false;
            // 
            // lbExtendedModules
            // 
            this.lbExtendedModules.FormattingEnabled = true;
            this.lbExtendedModules.Location = new System.Drawing.Point(12, 84);
            this.lbExtendedModules.Name = "lbExtendedModules";
            this.lbExtendedModules.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbExtendedModules.Size = new System.Drawing.Size(274, 199);
            this.lbExtendedModules.TabIndex = 44;
            this.lbExtendedModules.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(495, 364);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(420, 364);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 35;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRemoveConfiguration);
            this.groupBox2.Controls.Add(this.btnAddConfiguration);
            this.groupBox2.Controls.Add(this.cbConfigurationList);
            this.groupBox2.Location = new System.Drawing.Point(294, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 70);
            this.groupBox2.TabIndex = 42;
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
            // 
            // btnAddConfiguration
            // 
            this.btnAddConfiguration.Image = ((System.Drawing.Image)(resources.GetObject("btnAddConfiguration.Image")));
            this.btnAddConfiguration.Location = new System.Drawing.Point(222, 28);
            this.btnAddConfiguration.Name = "btnAddConfiguration";
            this.btnAddConfiguration.Size = new System.Drawing.Size(23, 23);
            this.btnAddConfiguration.TabIndex = 8;
            this.btnAddConfiguration.UseVisualStyleBackColor = true;
            // 
            // cbConfigurationList
            // 
            this.cbConfigurationList.FormattingEnabled = true;
            this.cbConfigurationList.Location = new System.Drawing.Point(7, 29);
            this.cbConfigurationList.Name = "cbConfigurationList";
            this.cbConfigurationList.Size = new System.Drawing.Size(213, 21);
            this.cbConfigurationList.TabIndex = 7;
            this.cbConfigurationList.Text = "Select a Configuration";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkResetDBVersion);
            this.groupBox1.Controls.Add(this.checkInstallFolder);
            this.groupBox1.Controls.Add(this.checkRunDatabaseUpdate);
            this.groupBox1.Controls.Add(this.checkLaunchAfterInstall);
            this.groupBox1.Location = new System.Drawing.Point(12, 290);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 70);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Build Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(410, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Custom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Extended";
            // 
            // tbInstallLocation
            // 
            this.tbInstallLocation.Location = new System.Drawing.Point(12, 45);
            this.tbInstallLocation.Name = "tbInstallLocation";
            this.tbInstallLocation.Size = new System.Drawing.Size(556, 20);
            this.tbInstallLocation.TabIndex = 37;
            // 
            // tbSelectedBuild
            // 
            this.tbSelectedBuild.Location = new System.Drawing.Point(12, 23);
            this.tbSelectedBuild.Name = "tbSelectedBuild";
            this.tbSelectedBuild.ReadOnly = true;
            this.tbSelectedBuild.Size = new System.Drawing.Size(556, 20);
            this.tbSelectedBuild.TabIndex = 43;
            this.tbSelectedBuild.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Please enter the location you would like to install the following build to:";
            // 
            // InstallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 395);
            this.Controls.Add(this.lbCustomModules);
            this.Controls.Add(this.lbExtendedModules);
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
            this.MaximizeBox = false;
            this.Name = "InstallForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Install";
            this.Load += new System.EventHandler(this.InstallForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbCustomModules;
        private System.Windows.Forms.ListBox lbExtendedModules;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemoveConfiguration;
        private System.Windows.Forms.Button btnAddConfiguration;
        private System.Windows.Forms.ComboBox cbConfigurationList;
        private System.Windows.Forms.CheckBox checkResetDBVersion;
        private System.Windows.Forms.CheckBox checkInstallFolder;
        private System.Windows.Forms.CheckBox checkRunDatabaseUpdate;
        private System.Windows.Forms.CheckBox checkLaunchAfterInstall;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbInstallLocation;
        private System.Windows.Forms.TextBox tbSelectedBuild;
        private System.Windows.Forms.Label label1;
    }
}