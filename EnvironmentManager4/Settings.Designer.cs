
namespace EnvironmentManager4
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSelectWebAPIDirectory = new System.Windows.Forms.Button();
            this.tbWebAPIDirectory = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSelectGPWebDirectory = new System.Windows.Forms.Button();
            this.tbGPWebDirectory = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSelectShipCenterDirectory = new System.Windows.Forms.Button();
            this.tbShipCenterDirectory = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectSalesPadMobileDirectory = new System.Windows.Forms.Button();
            this.tbSalesPadMobileDirectory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSelectDatacollectionDirectory = new System.Windows.Forms.Button();
            this.tbDataCollectionDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelectx64SPDirectory = new System.Windows.Forms.Button();
            this.tbSalesPadx64Directory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectSPx86Directory = new System.Windows.Forms.Button();
            this.tbSalesPadx86Directory = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnToggleVisibility = new System.Windows.Forms.Button();
            this.btnDeleteConnection = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cbConnections = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbSQLServerPW = new System.Windows.Forms.TextBox();
            this.tbSQLServerUN = new System.Windows.Forms.TextBox();
            this.lbDatabases = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectBackupDirectory = new System.Windows.Forms.Button();
            this.tbdatabaseBackupDirectory = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(428, 529);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 27;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(353, 529);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 26;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnSelectWebAPIDirectory);
            this.groupBox2.Controls.Add(this.tbWebAPIDirectory);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnSelectGPWebDirectory);
            this.groupBox2.Controls.Add(this.tbGPWebDirectory);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.btnSelectShipCenterDirectory);
            this.groupBox2.Controls.Add(this.tbShipCenterDirectory);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnSelectSalesPadMobileDirectory);
            this.groupBox2.Controls.Add(this.tbSalesPadMobileDirectory);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnSelectDatacollectionDirectory);
            this.groupBox2.Controls.Add(this.tbDataCollectionDirectory);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnSelectx64SPDirectory);
            this.groupBox2.Controls.Add(this.tbSalesPadx64Directory);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnSelectSPx86Directory);
            this.groupBox2.Controls.Add(this.tbSalesPadx86Directory);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(2, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(532, 188);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Build Management";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(7, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Web API Directory:";
            // 
            // btnSelectWebAPIDirectory
            // 
            this.btnSelectWebAPIDirectory.Location = new System.Drawing.Point(501, 158);
            this.btnSelectWebAPIDirectory.Name = "btnSelectWebAPIDirectory";
            this.btnSelectWebAPIDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectWebAPIDirectory.TabIndex = 24;
            this.btnSelectWebAPIDirectory.Text = "...";
            this.btnSelectWebAPIDirectory.UseVisualStyleBackColor = true;
            this.btnSelectWebAPIDirectory.Click += new System.EventHandler(this.btnSelectWebAPIDirectory_Click);
            // 
            // tbWebAPIDirectory
            // 
            this.tbWebAPIDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbWebAPIDirectory.Location = new System.Drawing.Point(151, 159);
            this.tbWebAPIDirectory.Name = "tbWebAPIDirectory";
            this.tbWebAPIDirectory.Size = new System.Drawing.Size(348, 20);
            this.tbWebAPIDirectory.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(7, 139);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "GPWeb Directory:";
            // 
            // btnSelectGPWebDirectory
            // 
            this.btnSelectGPWebDirectory.Location = new System.Drawing.Point(501, 135);
            this.btnSelectGPWebDirectory.Name = "btnSelectGPWebDirectory";
            this.btnSelectGPWebDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectGPWebDirectory.TabIndex = 22;
            this.btnSelectGPWebDirectory.Text = "...";
            this.btnSelectGPWebDirectory.UseVisualStyleBackColor = true;
            this.btnSelectGPWebDirectory.Click += new System.EventHandler(this.btnSelectGPWebDirectory_Click);
            // 
            // tbGPWebDirectory
            // 
            this.tbGPWebDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbGPWebDirectory.Location = new System.Drawing.Point(151, 136);
            this.tbGPWebDirectory.Name = "tbGPWebDirectory";
            this.tbGPWebDirectory.Size = new System.Drawing.Size(348, 20);
            this.tbGPWebDirectory.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(7, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "ShipCenter Directory:";
            // 
            // btnSelectShipCenterDirectory
            // 
            this.btnSelectShipCenterDirectory.Location = new System.Drawing.Point(501, 112);
            this.btnSelectShipCenterDirectory.Name = "btnSelectShipCenterDirectory";
            this.btnSelectShipCenterDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectShipCenterDirectory.TabIndex = 20;
            this.btnSelectShipCenterDirectory.Text = "...";
            this.btnSelectShipCenterDirectory.UseVisualStyleBackColor = true;
            this.btnSelectShipCenterDirectory.Click += new System.EventHandler(this.btnSelectShipCenterDirectory_Click);
            // 
            // tbShipCenterDirectory
            // 
            this.tbShipCenterDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbShipCenterDirectory.Location = new System.Drawing.Point(151, 113);
            this.tbShipCenterDirectory.Name = "tbShipCenterDirectory";
            this.tbShipCenterDirectory.Size = new System.Drawing.Size(348, 20);
            this.tbShipCenterDirectory.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(7, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "SalesPad Mobile Directory:";
            // 
            // btnSelectSalesPadMobileDirectory
            // 
            this.btnSelectSalesPadMobileDirectory.Location = new System.Drawing.Point(501, 89);
            this.btnSelectSalesPadMobileDirectory.Name = "btnSelectSalesPadMobileDirectory";
            this.btnSelectSalesPadMobileDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectSalesPadMobileDirectory.TabIndex = 18;
            this.btnSelectSalesPadMobileDirectory.Text = "...";
            this.btnSelectSalesPadMobileDirectory.UseVisualStyleBackColor = true;
            this.btnSelectSalesPadMobileDirectory.Click += new System.EventHandler(this.btnSelectSalesPadMobileDirectory_Click);
            // 
            // tbSalesPadMobileDirectory
            // 
            this.tbSalesPadMobileDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbSalesPadMobileDirectory.Location = new System.Drawing.Point(151, 90);
            this.tbSalesPadMobileDirectory.Name = "tbSalesPadMobileDirectory";
            this.tbSalesPadMobileDirectory.Size = new System.Drawing.Size(348, 20);
            this.tbSalesPadMobileDirectory.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(7, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "DataCollection Directory:";
            // 
            // btnSelectDatacollectionDirectory
            // 
            this.btnSelectDatacollectionDirectory.Location = new System.Drawing.Point(501, 66);
            this.btnSelectDatacollectionDirectory.Name = "btnSelectDatacollectionDirectory";
            this.btnSelectDatacollectionDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectDatacollectionDirectory.TabIndex = 16;
            this.btnSelectDatacollectionDirectory.Text = "...";
            this.btnSelectDatacollectionDirectory.UseVisualStyleBackColor = true;
            this.btnSelectDatacollectionDirectory.Click += new System.EventHandler(this.btnSelectDatacollectionDirectory_Click);
            // 
            // tbDataCollectionDirectory
            // 
            this.tbDataCollectionDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbDataCollectionDirectory.Location = new System.Drawing.Point(151, 67);
            this.tbDataCollectionDirectory.Name = "tbDataCollectionDirectory";
            this.tbDataCollectionDirectory.Size = new System.Drawing.Size(348, 20);
            this.tbDataCollectionDirectory.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(7, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "SalesPad x64 Directory:";
            // 
            // btnSelectx64SPDirectory
            // 
            this.btnSelectx64SPDirectory.Location = new System.Drawing.Point(501, 43);
            this.btnSelectx64SPDirectory.Name = "btnSelectx64SPDirectory";
            this.btnSelectx64SPDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectx64SPDirectory.TabIndex = 14;
            this.btnSelectx64SPDirectory.Text = "...";
            this.btnSelectx64SPDirectory.UseVisualStyleBackColor = true;
            this.btnSelectx64SPDirectory.Click += new System.EventHandler(this.btnSelectx64SPDirectory_Click);
            // 
            // tbSalesPadx64Directory
            // 
            this.tbSalesPadx64Directory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbSalesPadx64Directory.Location = new System.Drawing.Point(151, 44);
            this.tbSalesPadx64Directory.Name = "tbSalesPadx64Directory";
            this.tbSalesPadx64Directory.Size = new System.Drawing.Size(348, 20);
            this.tbSalesPadx64Directory.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(7, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "SalesPad x86 Directory:";
            // 
            // btnSelectSPx86Directory
            // 
            this.btnSelectSPx86Directory.Location = new System.Drawing.Point(501, 20);
            this.btnSelectSPx86Directory.Name = "btnSelectSPx86Directory";
            this.btnSelectSPx86Directory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectSPx86Directory.TabIndex = 12;
            this.btnSelectSPx86Directory.Text = "...";
            this.btnSelectSPx86Directory.UseVisualStyleBackColor = true;
            this.btnSelectSPx86Directory.Click += new System.EventHandler(this.btnSelectSPx86Directory_Click);
            // 
            // tbSalesPadx86Directory
            // 
            this.tbSalesPadx86Directory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbSalesPadx86Directory.Location = new System.Drawing.Point(151, 21);
            this.tbSalesPadx86Directory.Name = "tbSalesPadx86Directory";
            this.tbSalesPadx86Directory.Size = new System.Drawing.Size(348, 20);
            this.tbSalesPadx86Directory.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnToggleVisibility);
            this.groupBox1.Controls.Add(this.btnDeleteConnection);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnReload);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.cbConnections);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tbSQLServerPW);
            this.groupBox1.Controls.Add(this.tbSQLServerUN);
            this.groupBox1.Controls.Add(this.lbDatabases);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSelectBackupDirectory);
            this.groupBox1.Controls.Add(this.tbdatabaseBackupDirectory);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 275);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Management";
            // 
            // btnToggleVisibility
            // 
            this.btnToggleVisibility.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnToggleVisibility.Location = new System.Drawing.Point(501, 89);
            this.btnToggleVisibility.Name = "btnToggleVisibility";
            this.btnToggleVisibility.Size = new System.Drawing.Size(24, 22);
            this.btnToggleVisibility.TabIndex = 6;
            this.btnToggleVisibility.Text = "---";
            this.btnToggleVisibility.UseVisualStyleBackColor = true;
            this.btnToggleVisibility.Click += new System.EventHandler(this.btnToggleVisibility_Click);
            // 
            // btnDeleteConnection
            // 
            this.btnDeleteConnection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteConnection.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteConnection.Image")));
            this.btnDeleteConnection.Location = new System.Drawing.Point(501, 42);
            this.btnDeleteConnection.Name = "btnDeleteConnection";
            this.btnDeleteConnection.Size = new System.Drawing.Size(24, 23);
            this.btnDeleteConnection.TabIndex = 3;
            this.btnDeleteConnection.UseVisualStyleBackColor = true;
            this.btnDeleteConnection.Click += new System.EventHandler(this.btnDeleteConnection_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRemove.Location = new System.Drawing.Point(425, 247);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click_1);
            // 
            // btnReload
            // 
            this.btnReload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReload.Location = new System.Drawing.Point(350, 247);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 9;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnConnect.Location = new System.Drawing.Point(425, 111);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cbConnections
            // 
            this.cbConnections.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbConnections.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbConnections.FormattingEnabled = true;
            this.cbConnections.Location = new System.Drawing.Point(151, 43);
            this.cbConnections.Name = "cbConnections";
            this.cbConnections.Size = new System.Drawing.Size(348, 21);
            this.cbConnections.TabIndex = 2;
            this.cbConnections.SelectedIndexChanged += new System.EventHandler(this.cbConnections_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(7, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "SQLPassword";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(7, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "SQL Username:";
            // 
            // tbSQLServerPW
            // 
            this.tbSQLServerPW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbSQLServerPW.Location = new System.Drawing.Point(151, 90);
            this.tbSQLServerPW.Name = "tbSQLServerPW";
            this.tbSQLServerPW.Size = new System.Drawing.Size(348, 20);
            this.tbSQLServerPW.TabIndex = 5;
            this.tbSQLServerPW.UseSystemPasswordChar = true;
            // 
            // tbSQLServerUN
            // 
            this.tbSQLServerUN.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbSQLServerUN.Location = new System.Drawing.Point(151, 67);
            this.tbSQLServerUN.Name = "tbSQLServerUN";
            this.tbSQLServerUN.Size = new System.Drawing.Size(348, 20);
            this.tbSQLServerUN.TabIndex = 4;
            // 
            // lbDatabases
            // 
            this.lbDatabases.FormattingEnabled = true;
            this.lbDatabases.Location = new System.Drawing.Point(151, 138);
            this.lbDatabases.Name = "lbDatabases";
            this.lbDatabases.Size = new System.Drawing.Size(348, 108);
            this.lbDatabases.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(7, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Databases:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(7, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Connection:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Database Backup Directory:";
            // 
            // btnSelectBackupDirectory
            // 
            this.btnSelectBackupDirectory.Location = new System.Drawing.Point(501, 19);
            this.btnSelectBackupDirectory.Name = "btnSelectBackupDirectory";
            this.btnSelectBackupDirectory.Size = new System.Drawing.Size(24, 22);
            this.btnSelectBackupDirectory.TabIndex = 1;
            this.btnSelectBackupDirectory.Text = "...";
            this.btnSelectBackupDirectory.UseVisualStyleBackColor = true;
            this.btnSelectBackupDirectory.Click += new System.EventHandler(this.btnSelectBackupDirectory_Click);
            // 
            // tbdatabaseBackupDirectory
            // 
            this.tbdatabaseBackupDirectory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbdatabaseBackupDirectory.Location = new System.Drawing.Point(151, 20);
            this.tbdatabaseBackupDirectory.Name = "tbdatabaseBackupDirectory";
            this.tbdatabaseBackupDirectory.Size = new System.Drawing.Size(348, 20);
            this.tbdatabaseBackupDirectory.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbMode);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.ForeColor = System.Drawing.Color.Blue;
            this.groupBox3.Location = new System.Drawing.Point(2, 476);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(533, 51);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other";
            // 
            // cbMode
            // 
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Standard",
            "SmartBear",
            "Kyle"});
            this.cbMode.Location = new System.Drawing.Point(151, 17);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(349, 21);
            this.cbMode.TabIndex = 25;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(9, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Mode:";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(537, 554);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectx64SPDirectory;
        private System.Windows.Forms.TextBox tbSalesPadx64Directory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectSPx86Directory;
        private System.Windows.Forms.TextBox tbSalesPadx86Directory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbDatabases;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectBackupDirectory;
        private System.Windows.Forms.TextBox tbdatabaseBackupDirectory;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSelectWebAPIDirectory;
        private System.Windows.Forms.TextBox tbWebAPIDirectory;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSelectGPWebDirectory;
        private System.Windows.Forms.TextBox tbGPWebDirectory;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSelectShipCenterDirectory;
        private System.Windows.Forms.TextBox tbShipCenterDirectory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSelectSalesPadMobileDirectory;
        private System.Windows.Forms.TextBox tbSalesPadMobileDirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSelectDatacollectionDirectory;
        private System.Windows.Forms.TextBox tbDataCollectionDirectory;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbSQLServerPW;
        private System.Windows.Forms.TextBox tbSQLServerUN;
        private System.Windows.Forms.ComboBox cbConnections;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDeleteConnection;
        private System.Windows.Forms.Button btnToggleVisibility;
    }
}