﻿
namespace EnvironmentManager4
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelReloadVPNIPAddress = new System.Windows.Forms.Label();
            this.tbSPVPNIPAddress = new System.Windows.Forms.TextBox();
            this.cbAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.labelReloadIPAddress = new System.Windows.Forms.Label();
            this.tbWiFiIPAddress = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnOpenBuildFolder = new System.Windows.Forms.Button();
            this.btnBuildFolder = new System.Windows.Forms.Button();
            this.cbSPGPVersion = new System.Windows.Forms.ComboBox();
            this.cbProductList = new System.Windows.Forms.ComboBox();
            this.btnLaunchProduct = new System.Windows.Forms.Button();
            this.btnInstallProduct = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnEditDescription = new System.Windows.Forms.Button();
            this.btnDeleteBackup = new System.Windows.Forms.Button();
            this.btnNewDB = new System.Windows.Forms.Button();
            this.btnOverwriteDB = new System.Windows.Forms.Button();
            this.tbDBDesc = new System.Windows.Forms.TextBox();
            this.btnRestoreDB = new System.Windows.Forms.Button();
            this.btnDBBackupFolder = new System.Windows.Forms.Button();
            this.cbDatabaseList = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvInstalledSQLServers = new System.Windows.Forms.ListView();
            this.chService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnStopService = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.btnRestartService = new System.Windows.Forms.Button();
            this.labelSQLVersions = new System.Windows.Forms.Label();
            this.btnInstallService = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLaunchGPUtils = new System.Windows.Forms.Button();
            this.btnLaunchSelectedGP = new System.Windows.Forms.Button();
            this.cbGPListToInstall = new System.Windows.Forms.ComboBox();
            this.btnInstallGP = new System.Windows.Forms.Button();
            this.lbGPVersionsInstalled = new System.Windows.Forms.ListBox();
            this.labelGPInstallationList = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetDatabaseVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteBuildInstallsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killSalesPadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installPropertiesMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelNotConnected = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelReloadVPNIPAddress
            // 
            this.labelReloadVPNIPAddress.AutoSize = true;
            this.labelReloadVPNIPAddress.Location = new System.Drawing.Point(89, 566);
            this.labelReloadVPNIPAddress.Name = "labelReloadVPNIPAddress";
            this.labelReloadVPNIPAddress.Size = new System.Drawing.Size(45, 13);
            this.labelReloadVPNIPAddress.TabIndex = 18;
            this.labelReloadVPNIPAddress.Text = "VPN IP:";
            this.labelReloadVPNIPAddress.Click += new System.EventHandler(this.labelReloadVPNIPAddress_Click);
            // 
            // tbSPVPNIPAddress
            // 
            this.tbSPVPNIPAddress.Location = new System.Drawing.Point(136, 563);
            this.tbSPVPNIPAddress.Name = "tbSPVPNIPAddress";
            this.tbSPVPNIPAddress.ReadOnly = true;
            this.tbSPVPNIPAddress.Size = new System.Drawing.Size(123, 20);
            this.tbSPVPNIPAddress.TabIndex = 17;
            this.tbSPVPNIPAddress.TabStop = false;
            this.tbSPVPNIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbAlwaysOnTop
            // 
            this.cbAlwaysOnTop.AutoSize = true;
            this.cbAlwaysOnTop.Location = new System.Drawing.Point(419, 4);
            this.cbAlwaysOnTop.Name = "cbAlwaysOnTop";
            this.cbAlwaysOnTop.Size = new System.Drawing.Size(98, 17);
            this.cbAlwaysOnTop.TabIndex = 16;
            this.cbAlwaysOnTop.Text = "Always On Top";
            this.cbAlwaysOnTop.UseVisualStyleBackColor = true;
            this.cbAlwaysOnTop.CheckedChanged += new System.EventHandler(this.cbAlwaysOnTop_CheckedChanged);
            // 
            // labelReloadIPAddress
            // 
            this.labelReloadIPAddress.AutoSize = true;
            this.labelReloadIPAddress.Location = new System.Drawing.Point(339, 566);
            this.labelReloadIPAddress.Name = "labelReloadIPAddress";
            this.labelReloadIPAddress.Size = new System.Drawing.Size(47, 13);
            this.labelReloadIPAddress.TabIndex = 15;
            this.labelReloadIPAddress.Text = "Wi-Fi IP:";
            this.labelReloadIPAddress.Click += new System.EventHandler(this.labelReloadIPAddress_Click);
            // 
            // tbWiFiIPAddress
            // 
            this.tbWiFiIPAddress.Location = new System.Drawing.Point(386, 563);
            this.tbWiFiIPAddress.Name = "tbWiFiIPAddress";
            this.tbWiFiIPAddress.ReadOnly = true;
            this.tbWiFiIPAddress.Size = new System.Drawing.Size(123, 20);
            this.tbWiFiIPAddress.TabIndex = 14;
            this.tbWiFiIPAddress.TabStop = false;
            this.tbWiFiIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnOpenBuildFolder);
            this.groupBox4.Controls.Add(this.btnBuildFolder);
            this.groupBox4.Controls.Add(this.cbSPGPVersion);
            this.groupBox4.Controls.Add(this.cbProductList);
            this.groupBox4.Controls.Add(this.btnLaunchProduct);
            this.groupBox4.Controls.Add(this.btnInstallProduct);
            this.groupBox4.ForeColor = System.Drawing.Color.Blue;
            this.groupBox4.Location = new System.Drawing.Point(5, 491);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(510, 70);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Build Management";
            // 
            // btnOpenBuildFolder
            // 
            this.btnOpenBuildFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOpenBuildFolder.Location = new System.Drawing.Point(255, 42);
            this.btnOpenBuildFolder.Name = "btnOpenBuildFolder";
            this.btnOpenBuildFolder.Size = new System.Drawing.Size(125, 23);
            this.btnOpenBuildFolder.TabIndex = 19;
            this.btnOpenBuildFolder.Text = "Open FileServer Folder";
            this.toolTip1.SetToolTip(this.btnOpenBuildFolder, "Opens the default build directory for the selected product");
            this.btnOpenBuildFolder.UseVisualStyleBackColor = true;
            this.btnOpenBuildFolder.Click += new System.EventHandler(this.btnOpenBuildFolder_Click);
            // 
            // btnBuildFolder
            // 
            this.btnBuildFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBuildFolder.Location = new System.Drawing.Point(380, 42);
            this.btnBuildFolder.Name = "btnBuildFolder";
            this.btnBuildFolder.Size = new System.Drawing.Size(125, 23);
            this.btnBuildFolder.TabIndex = 20;
            this.btnBuildFolder.Text = "Build Folder";
            this.btnBuildFolder.UseVisualStyleBackColor = true;
            this.btnBuildFolder.Click += new System.EventHandler(this.btnBuildFolder_Click);
            // 
            // cbSPGPVersion
            // 
            this.cbSPGPVersion.FormattingEnabled = true;
            this.cbSPGPVersion.Items.AddRange(new object[] {
            "x86",
            "x64",
            "Pre"});
            this.cbSPGPVersion.Location = new System.Drawing.Point(436, 18);
            this.cbSPGPVersion.Name = "cbSPGPVersion";
            this.cbSPGPVersion.Size = new System.Drawing.Size(68, 21);
            this.cbSPGPVersion.TabIndex = 16;
            this.cbSPGPVersion.Text = "x86";
            // 
            // cbProductList
            // 
            this.cbProductList.FormattingEnabled = true;
            this.cbProductList.Location = new System.Drawing.Point(6, 18);
            this.cbProductList.Name = "cbProductList";
            this.cbProductList.Size = new System.Drawing.Size(425, 21);
            this.cbProductList.TabIndex = 15;
            this.toolTip1.SetToolTip(this.cbProductList, "Select a product to install or launch");
            this.cbProductList.SelectedIndexChanged += new System.EventHandler(this.cbProductList_SelectedIndexChanged);
            // 
            // btnLaunchProduct
            // 
            this.btnLaunchProduct.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLaunchProduct.Location = new System.Drawing.Point(130, 42);
            this.btnLaunchProduct.Name = "btnLaunchProduct";
            this.btnLaunchProduct.Size = new System.Drawing.Size(125, 23);
            this.btnLaunchProduct.TabIndex = 18;
            this.btnLaunchProduct.Text = "Launch Build";
            this.toolTip1.SetToolTip(this.btnLaunchProduct, "Launch a build for the selected product");
            this.btnLaunchProduct.UseVisualStyleBackColor = true;
            this.btnLaunchProduct.Click += new System.EventHandler(this.btnLaunchProduct_Click);
            // 
            // btnInstallProduct
            // 
            this.btnInstallProduct.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInstallProduct.Location = new System.Drawing.Point(5, 42);
            this.btnInstallProduct.Name = "btnInstallProduct";
            this.btnInstallProduct.Size = new System.Drawing.Size(125, 23);
            this.btnInstallProduct.TabIndex = 17;
            this.btnInstallProduct.Text = "Install";
            this.toolTip1.SetToolTip(this.btnInstallProduct, "Install a build for the selected product");
            this.btnInstallProduct.UseVisualStyleBackColor = true;
            this.btnInstallProduct.Click += new System.EventHandler(this.btnInstallProduct_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnEditDescription);
            this.groupBox3.Controls.Add(this.btnDeleteBackup);
            this.groupBox3.Controls.Add(this.btnNewDB);
            this.groupBox3.Controls.Add(this.btnOverwriteDB);
            this.groupBox3.Controls.Add(this.tbDBDesc);
            this.groupBox3.Controls.Add(this.btnRestoreDB);
            this.groupBox3.Controls.Add(this.btnDBBackupFolder);
            this.groupBox3.Controls.Add(this.cbDatabaseList);
            this.groupBox3.ForeColor = System.Drawing.Color.Blue;
            this.groupBox3.Location = new System.Drawing.Point(5, 254);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(510, 235);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Database Management";
            // 
            // btnEditDescription
            // 
            this.btnEditDescription.ForeColor = System.Drawing.SystemColors.MenuText;
            this.btnEditDescription.Image = ((System.Drawing.Image)(resources.GetObject("btnEditDescription.Image")));
            this.btnEditDescription.Location = new System.Drawing.Point(458, 19);
            this.btnEditDescription.Name = "btnEditDescription";
            this.btnEditDescription.Size = new System.Drawing.Size(23, 23);
            this.btnEditDescription.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnEditDescription, "Edit the description of the selected database backup");
            this.btnEditDescription.UseVisualStyleBackColor = true;
            this.btnEditDescription.Click += new System.EventHandler(this.btnEditDescription_Click);
            // 
            // btnDeleteBackup
            // 
            this.btnDeleteBackup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteBackup.Location = new System.Drawing.Point(380, 206);
            this.btnDeleteBackup.Name = "btnDeleteBackup";
            this.btnDeleteBackup.Size = new System.Drawing.Size(125, 23);
            this.btnDeleteBackup.TabIndex = 14;
            this.btnDeleteBackup.Text = "Delete DB Backup";
            this.toolTip1.SetToolTip(this.btnDeleteBackup, "Delete the selected database backup.");
            this.btnDeleteBackup.UseVisualStyleBackColor = true;
            this.btnDeleteBackup.Click += new System.EventHandler(this.btnDeleteBackup_Click);
            // 
            // btnNewDB
            // 
            this.btnNewDB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNewDB.Location = new System.Drawing.Point(255, 206);
            this.btnNewDB.Name = "btnNewDB";
            this.btnNewDB.Size = new System.Drawing.Size(125, 23);
            this.btnNewDB.TabIndex = 13;
            this.btnNewDB.Text = "New DB Backup";
            this.toolTip1.SetToolTip(this.btnNewDB, "Create a new database backup.");
            this.btnNewDB.UseVisualStyleBackColor = true;
            this.btnNewDB.Click += new System.EventHandler(this.btnNewDB_Click);
            // 
            // btnOverwriteDB
            // 
            this.btnOverwriteDB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOverwriteDB.Location = new System.Drawing.Point(130, 206);
            this.btnOverwriteDB.Name = "btnOverwriteDB";
            this.btnOverwriteDB.Size = new System.Drawing.Size(125, 23);
            this.btnOverwriteDB.TabIndex = 12;
            this.btnOverwriteDB.Text = "Overwrite DB";
            this.toolTip1.SetToolTip(this.btnOverwriteDB, "Overwrite the selected backup using your current environment.");
            this.btnOverwriteDB.UseVisualStyleBackColor = true;
            this.btnOverwriteDB.Click += new System.EventHandler(this.btnOverwriteDB_Click);
            // 
            // tbDBDesc
            // 
            this.tbDBDesc.BackColor = System.Drawing.Color.AliceBlue;
            this.tbDBDesc.Location = new System.Drawing.Point(6, 46);
            this.tbDBDesc.Multiline = true;
            this.tbDBDesc.Name = "tbDBDesc";
            this.tbDBDesc.ReadOnly = true;
            this.tbDBDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDBDesc.Size = new System.Drawing.Size(498, 157);
            this.tbDBDesc.TabIndex = 5;
            this.tbDBDesc.TabStop = false;
            // 
            // btnRestoreDB
            // 
            this.btnRestoreDB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestoreDB.Location = new System.Drawing.Point(5, 206);
            this.btnRestoreDB.Name = "btnRestoreDB";
            this.btnRestoreDB.Size = new System.Drawing.Size(125, 23);
            this.btnRestoreDB.TabIndex = 11;
            this.btnRestoreDB.Text = "Restore DB";
            this.toolTip1.SetToolTip(this.btnRestoreDB, "Restore the selected database backup.");
            this.btnRestoreDB.UseVisualStyleBackColor = true;
            this.btnRestoreDB.Click += new System.EventHandler(this.btnRestoreDB_Click);
            // 
            // btnDBBackupFolder
            // 
            this.btnDBBackupFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDBBackupFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnDBBackupFolder.Image")));
            this.btnDBBackupFolder.Location = new System.Drawing.Point(482, 19);
            this.btnDBBackupFolder.Name = "btnDBBackupFolder";
            this.btnDBBackupFolder.Size = new System.Drawing.Size(23, 23);
            this.btnDBBackupFolder.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnDBBackupFolder, "Open the Settings defined database backup folder");
            this.btnDBBackupFolder.UseVisualStyleBackColor = true;
            this.btnDBBackupFolder.Click += new System.EventHandler(this.btnDBBackupFolder_Click);
            // 
            // cbDatabaseList
            // 
            this.cbDatabaseList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbDatabaseList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDatabaseList.FormattingEnabled = true;
            this.cbDatabaseList.Location = new System.Drawing.Point(6, 20);
            this.cbDatabaseList.Name = "cbDatabaseList";
            this.cbDatabaseList.Size = new System.Drawing.Size(450, 21);
            this.cbDatabaseList.TabIndex = 8;
            this.toolTip1.SetToolTip(this.cbDatabaseList, "Select a database backup to work with.");
            this.cbDatabaseList.SelectedIndexChanged += new System.EventHandler(this.cbDatabaseList_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvInstalledSQLServers);
            this.groupBox2.Controls.Add(this.btnStopService);
            this.groupBox2.Controls.Add(this.btnStartService);
            this.groupBox2.Controls.Add(this.btnRestartService);
            this.groupBox2.Controls.Add(this.labelSQLVersions);
            this.groupBox2.Controls.Add(this.btnInstallService);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(191, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 222);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Service Management";
            // 
            // lvInstalledSQLServers
            // 
            this.lvInstalledSQLServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chService,
            this.chStatus});
            this.lvInstalledSQLServers.FullRowSelect = true;
            this.lvInstalledSQLServers.GridLines = true;
            this.lvInstalledSQLServers.HideSelection = false;
            this.lvInstalledSQLServers.Location = new System.Drawing.Point(6, 37);
            this.lvInstalledSQLServers.Name = "lvInstalledSQLServers";
            this.lvInstalledSQLServers.Size = new System.Drawing.Size(312, 132);
            this.lvInstalledSQLServers.TabIndex = 11;
            this.lvInstalledSQLServers.TabStop = false;
            this.toolTip1.SetToolTip(this.lvInstalledSQLServers, "List of installed SQL and SalesPad Services.");
            this.lvInstalledSQLServers.UseCompatibleStateImageBehavior = false;
            this.lvInstalledSQLServers.View = System.Windows.Forms.View.Details;
            // 
            // chService
            // 
            this.chService.Text = "Service Name";
            this.chService.Width = 218;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Status";
            this.chStatus.Width = 90;
            // 
            // btnStopService
            // 
            this.btnStopService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStopService.Location = new System.Drawing.Point(162, 170);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(157, 23);
            this.btnStopService.TabIndex = 5;
            this.btnStopService.Text = "Stop Service(s)";
            this.toolTip1.SetToolTip(this.btnStopService, "Stop Selected Service.");
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStartService.Location = new System.Drawing.Point(5, 170);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(157, 23);
            this.btnStartService.TabIndex = 4;
            this.btnStartService.Text = "Start Service(s)";
            this.toolTip1.SetToolTip(this.btnStartService, "Start Selected Service.");
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // btnRestartService
            // 
            this.btnRestartService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestartService.Location = new System.Drawing.Point(162, 193);
            this.btnRestartService.Name = "btnRestartService";
            this.btnRestartService.Size = new System.Drawing.Size(157, 23);
            this.btnRestartService.TabIndex = 7;
            this.btnRestartService.Text = "Restart Service(s)";
            this.toolTip1.SetToolTip(this.btnRestartService, "Stop all services in the list.");
            this.btnRestartService.UseVisualStyleBackColor = true;
            this.btnRestartService.Click += new System.EventHandler(this.btnStopAllServices_Click);
            // 
            // labelSQLVersions
            // 
            this.labelSQLVersions.AutoSize = true;
            this.labelSQLVersions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSQLVersions.Location = new System.Drawing.Point(6, 20);
            this.labelSQLVersions.Name = "labelSQLVersions";
            this.labelSQLVersions.Size = new System.Drawing.Size(51, 13);
            this.labelSQLVersions.TabIndex = 3;
            this.labelSQLVersions.Text = "Services:";
            this.toolTip1.SetToolTip(this.labelSQLVersions, "Clicking on this label reloads the Services table.");
            this.labelSQLVersions.Click += new System.EventHandler(this.labelSQLVersions_Click);
            // 
            // btnInstallService
            // 
            this.btnInstallService.Enabled = false;
            this.btnInstallService.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInstallService.Location = new System.Drawing.Point(5, 193);
            this.btnInstallService.Name = "btnInstallService";
            this.btnInstallService.Size = new System.Drawing.Size(157, 23);
            this.btnInstallService.TabIndex = 6;
            this.btnInstallService.Text = "Install Service";
            this.toolTip1.SetToolTip(this.btnInstallService, "Install a service.");
            this.btnInstallService.UseVisualStyleBackColor = true;
            this.btnInstallService.Click += new System.EventHandler(this.btnInstallService_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLaunchGPUtils);
            this.groupBox1.Controls.Add(this.btnLaunchSelectedGP);
            this.groupBox1.Controls.Add(this.cbGPListToInstall);
            this.groupBox1.Controls.Add(this.btnInstallGP);
            this.groupBox1.Controls.Add(this.lbGPVersionsInstalled);
            this.groupBox1.Controls.Add(this.labelGPInstallationList);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(5, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 222);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dynamics GP Management";
            // 
            // btnLaunchGPUtils
            // 
            this.btnLaunchGPUtils.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLaunchGPUtils.Location = new System.Drawing.Point(90, 146);
            this.btnLaunchGPUtils.Name = "btnLaunchGPUtils";
            this.btnLaunchGPUtils.Size = new System.Drawing.Size(85, 23);
            this.btnLaunchGPUtils.TabIndex = 1;
            this.btnLaunchGPUtils.Text = "Launch Utils";
            this.toolTip1.SetToolTip(this.btnLaunchGPUtils, "Launch GP Utils for the selected installed GP Instance.");
            this.btnLaunchGPUtils.UseVisualStyleBackColor = true;
            this.btnLaunchGPUtils.Click += new System.EventHandler(this.btnLaunchGPUtils_Click);
            // 
            // btnLaunchSelectedGP
            // 
            this.btnLaunchSelectedGP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLaunchSelectedGP.Location = new System.Drawing.Point(5, 146);
            this.btnLaunchSelectedGP.Name = "btnLaunchSelectedGP";
            this.btnLaunchSelectedGP.Size = new System.Drawing.Size(85, 23);
            this.btnLaunchSelectedGP.TabIndex = 0;
            this.btnLaunchSelectedGP.Text = "Launch GP";
            this.toolTip1.SetToolTip(this.btnLaunchSelectedGP, "Launch GP for the selected installed instance.\r\n\r\nShift-Clicking will launch the " +
        "selected installs folder.");
            this.btnLaunchSelectedGP.UseVisualStyleBackColor = true;
            this.btnLaunchSelectedGP.Click += new System.EventHandler(this.btnLaunchSelectedGP_Click);
            // 
            // cbGPListToInstall
            // 
            this.cbGPListToInstall.FormattingEnabled = true;
            this.cbGPListToInstall.Location = new System.Drawing.Point(6, 171);
            this.cbGPListToInstall.Name = "cbGPListToInstall";
            this.cbGPListToInstall.Size = new System.Drawing.Size(168, 21);
            this.cbGPListToInstall.TabIndex = 2;
            // 
            // btnInstallGP
            // 
            this.btnInstallGP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInstallGP.Location = new System.Drawing.Point(5, 193);
            this.btnInstallGP.Name = "btnInstallGP";
            this.btnInstallGP.Size = new System.Drawing.Size(170, 23);
            this.btnInstallGP.TabIndex = 3;
            this.btnInstallGP.Text = "Install GP";
            this.btnInstallGP.UseVisualStyleBackColor = true;
            this.btnInstallGP.Click += new System.EventHandler(this.btnInstallGP_Click);
            // 
            // lbGPVersionsInstalled
            // 
            this.lbGPVersionsInstalled.FormattingEnabled = true;
            this.lbGPVersionsInstalled.Location = new System.Drawing.Point(6, 37);
            this.lbGPVersionsInstalled.Name = "lbGPVersionsInstalled";
            this.lbGPVersionsInstalled.Size = new System.Drawing.Size(168, 108);
            this.lbGPVersionsInstalled.TabIndex = 1;
            this.lbGPVersionsInstalled.TabStop = false;
            this.toolTip1.SetToolTip(this.lbGPVersionsInstalled, "List of installed Dynamics GP Instances");
            // 
            // labelGPInstallationList
            // 
            this.labelGPInstallationList.AutoSize = true;
            this.labelGPInstallationList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelGPInstallationList.Location = new System.Drawing.Point(7, 20);
            this.labelGPInstallationList.Name = "labelGPInstallationList";
            this.labelGPInstallationList.Size = new System.Drawing.Size(83, 13);
            this.labelGPInstallationList.TabIndex = 0;
            this.labelGPInstallationList.Text = "GP Installations:";
            this.labelGPInstallationList.Click += new System.EventHandler(this.labelGPInstallationList_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(520, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetDatabaseVersionToolStripMenuItem,
            this.deleteBuildInstallsToolStripMenuItem,
            this.databaseLogToolStripMenuItem,
            this.killSalesPadToolStripMenuItem,
            this.notesToolStripMenuItem,
            this.directoryCompareToolStripMenuItem,
            this.installPropertiesMonitorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // resetDatabaseVersionToolStripMenuItem
            // 
            this.resetDatabaseVersionToolStripMenuItem.Name = "resetDatabaseVersionToolStripMenuItem";
            this.resetDatabaseVersionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.resetDatabaseVersionToolStripMenuItem.Text = "Reset Database Version";
            this.resetDatabaseVersionToolStripMenuItem.Click += new System.EventHandler(this.resetDatabaseVersionToolStripMenuItem_Click);
            // 
            // deleteBuildInstallsToolStripMenuItem
            // 
            this.deleteBuildInstallsToolStripMenuItem.Name = "deleteBuildInstallsToolStripMenuItem";
            this.deleteBuildInstallsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.deleteBuildInstallsToolStripMenuItem.Text = "Delete Build Installs";
            this.deleteBuildInstallsToolStripMenuItem.Click += new System.EventHandler(this.deleteBuildInstallsToolStripMenuItem_Click);
            // 
            // databaseLogToolStripMenuItem
            // 
            this.databaseLogToolStripMenuItem.Name = "databaseLogToolStripMenuItem";
            this.databaseLogToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.databaseLogToolStripMenuItem.Text = "Database Log";
            this.databaseLogToolStripMenuItem.Click += new System.EventHandler(this.databaseLogToolStripMenuItem_Click);
            // 
            // killSalesPadToolStripMenuItem
            // 
            this.killSalesPadToolStripMenuItem.Name = "killSalesPadToolStripMenuItem";
            this.killSalesPadToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.killSalesPadToolStripMenuItem.Text = "Kill SalesPad";
            this.killSalesPadToolStripMenuItem.Click += new System.EventHandler(this.killSalesPadToolStripMenuItem_Click);
            // 
            // notesToolStripMenuItem
            // 
            this.notesToolStripMenuItem.Name = "notesToolStripMenuItem";
            this.notesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.notesToolStripMenuItem.Text = "Notes";
            this.notesToolStripMenuItem.Click += new System.EventHandler(this.notesToolStripMenuItem_Click);
            // 
            // directoryCompareToolStripMenuItem
            // 
            this.directoryCompareToolStripMenuItem.Name = "directoryCompareToolStripMenuItem";
            this.directoryCompareToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.directoryCompareToolStripMenuItem.Text = "Directory Compare";
            this.directoryCompareToolStripMenuItem.Click += new System.EventHandler(this.directoryCompareToolStripMenuItem_Click);
            // 
            // installPropertiesMonitorToolStripMenuItem
            // 
            this.installPropertiesMonitorToolStripMenuItem.Name = "installPropertiesMonitorToolStripMenuItem";
            this.installPropertiesMonitorToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.installPropertiesMonitorToolStripMenuItem.Text = "Install Properties Monitor";
            this.installPropertiesMonitorToolStripMenuItem.Click += new System.EventHandler(this.installPropertiesMonitorToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelVersion.Location = new System.Drawing.Point(14, 566);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(35, 13);
            this.labelVersion.TabIndex = 19;
            this.labelVersion.Text = "label1";
            // 
            // labelNotConnected
            // 
            this.labelNotConnected.AutoSize = true;
            this.labelNotConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNotConnected.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelNotConnected.Location = new System.Drawing.Point(169, 7);
            this.labelNotConnected.Name = "labelNotConnected";
            this.labelNotConnected.Size = new System.Drawing.Size(198, 13);
            this.labelNotConnected.TabIndex = 20;
            this.labelNotConnected.Text = "You are NOT connected to the network.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 587);
            this.Controls.Add(this.labelNotConnected);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelReloadVPNIPAddress);
            this.Controls.Add(this.tbSPVPNIPAddress);
            this.Controls.Add(this.cbAlwaysOnTop);
            this.Controls.Add(this.labelReloadIPAddress);
            this.Controls.Add(this.tbWiFiIPAddress);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Environment Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelReloadVPNIPAddress;
        private System.Windows.Forms.TextBox tbSPVPNIPAddress;
        private System.Windows.Forms.CheckBox cbAlwaysOnTop;
        private System.Windows.Forms.Label labelReloadIPAddress;
        private System.Windows.Forms.TextBox tbWiFiIPAddress;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnBuildFolder;
        private System.Windows.Forms.ComboBox cbSPGPVersion;
        private System.Windows.Forms.ComboBox cbProductList;
        private System.Windows.Forms.Button btnLaunchProduct;
        private System.Windows.Forms.Button btnInstallProduct;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDeleteBackup;
        private System.Windows.Forms.Button btnNewDB;
        private System.Windows.Forms.Button btnOverwriteDB;
        private System.Windows.Forms.TextBox tbDBDesc;
        private System.Windows.Forms.Button btnRestoreDB;
        private System.Windows.Forms.Button btnDBBackupFolder;
        private System.Windows.Forms.ComboBox cbDatabaseList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvInstalledSQLServers;
        private System.Windows.Forms.ColumnHeader chService;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Button btnRestartService;
        private System.Windows.Forms.Label labelSQLVersions;
        private System.Windows.Forms.Button btnInstallService;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLaunchGPUtils;
        private System.Windows.Forms.Button btnLaunchSelectedGP;
        private System.Windows.Forms.ComboBox cbGPListToInstall;
        private System.Windows.Forms.Button btnInstallGP;
        private System.Windows.Forms.ListBox lbGPVersionsInstalled;
        private System.Windows.Forms.Label labelGPInstallationList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetDatabaseVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killSalesPadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryCompareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteBuildInstallsToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnOpenBuildFolder;
        private System.Windows.Forms.Button btnEditDescription;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.ToolStripMenuItem installPropertiesMonitorToolStripMenuItem;
        private System.Windows.Forms.Label labelNotConnected;
    }
}

