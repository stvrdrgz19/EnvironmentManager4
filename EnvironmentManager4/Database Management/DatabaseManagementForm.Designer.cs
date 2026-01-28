namespace EnvironmentManager4.Database_Management
{
    partial class DatabaseManagementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseManagementForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelBackupName = new System.Windows.Forms.Label();
            this.tbDatabaseName = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.tbDatabaseDescription = new System.Windows.Forms.TextBox();
            this.gbDatabases = new System.Windows.Forms.GroupBox();
            this.labelSQLServer = new System.Windows.Forms.Label();
            this.labelSQLServerText = new System.Windows.Forms.Label();
            this.btnClearSelections = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lvDatabases = new System.Windows.Forms.ListView();
            this.DatabaseName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbBackupDetails = new System.Windows.Forms.GroupBox();
            this.gbDatabases.SuspendLayout();
            this.gbBackupDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(437, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(357, 461);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelBackupName
            // 
            this.labelBackupName.AutoSize = true;
            this.labelBackupName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labelBackupName.Location = new System.Drawing.Point(6, 28);
            this.labelBackupName.Name = "labelBackupName";
            this.labelBackupName.Size = new System.Drawing.Size(78, 13);
            this.labelBackupName.TabIndex = 6;
            this.labelBackupName.Text = "Backup Name:";
            // 
            // tbDatabaseName
            // 
            this.tbDatabaseName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbDatabaseName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.tbDatabaseName.BackColor = System.Drawing.SystemColors.Window;
            this.tbDatabaseName.Location = new System.Drawing.Point(6, 45);
            this.tbDatabaseName.Name = "tbDatabaseName";
            this.tbDatabaseName.Size = new System.Drawing.Size(498, 20);
            this.tbDatabaseName.TabIndex = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labelDescription.Location = new System.Drawing.Point(6, 76);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(63, 13);
            this.labelDescription.TabIndex = 8;
            this.labelDescription.Text = "Description:";
            // 
            // tbDatabaseDescription
            // 
            this.tbDatabaseDescription.AcceptsReturn = true;
            this.tbDatabaseDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbDatabaseDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.tbDatabaseDescription.BackColor = System.Drawing.SystemColors.Window;
            this.tbDatabaseDescription.Location = new System.Drawing.Point(6, 92);
            this.tbDatabaseDescription.Multiline = true;
            this.tbDatabaseDescription.Name = "tbDatabaseDescription";
            this.tbDatabaseDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDatabaseDescription.Size = new System.Drawing.Size(498, 157);
            this.tbDatabaseDescription.TabIndex = 1;
            // 
            // gbDatabases
            // 
            this.gbDatabases.Controls.Add(this.labelSQLServer);
            this.gbDatabases.Controls.Add(this.labelSQLServerText);
            this.gbDatabases.Controls.Add(this.btnClearSelections);
            this.gbDatabases.Controls.Add(this.btnSelectAll);
            this.gbDatabases.Controls.Add(this.lvDatabases);
            this.gbDatabases.ForeColor = System.Drawing.Color.Blue;
            this.gbDatabases.Location = new System.Drawing.Point(5, 269);
            this.gbDatabases.Margin = new System.Windows.Forms.Padding(2);
            this.gbDatabases.Name = "gbDatabases";
            this.gbDatabases.Padding = new System.Windows.Forms.Padding(2);
            this.gbDatabases.Size = new System.Drawing.Size(510, 189);
            this.gbDatabases.TabIndex = 17;
            this.gbDatabases.TabStop = false;
            this.gbDatabases.Text = "Databases to Include";
            // 
            // labelSQLServer
            // 
            this.labelSQLServer.AutoSize = true;
            this.labelSQLServer.Enabled = false;
            this.labelSQLServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSQLServer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labelSQLServer.Location = new System.Drawing.Point(262, 24);
            this.labelSQLServer.Name = "labelSQLServer";
            this.labelSQLServer.Size = new System.Drawing.Size(35, 13);
            this.labelSQLServer.TabIndex = 15;
            this.labelSQLServer.Text = "label4";
            // 
            // labelSQLServerText
            // 
            this.labelSQLServerText.AutoSize = true;
            this.labelSQLServerText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labelSQLServerText.Location = new System.Drawing.Point(200, 24);
            this.labelSQLServerText.Name = "labelSQLServerText";
            this.labelSQLServerText.Size = new System.Drawing.Size(65, 13);
            this.labelSQLServerText.TabIndex = 14;
            this.labelSQLServerText.Text = "SQL Server:";
            // 
            // btnClearSelections
            // 
            this.btnClearSelections.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnClearSelections.Location = new System.Drawing.Point(101, 18);
            this.btnClearSelections.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearSelections.Name = "btnClearSelections";
            this.btnClearSelections.Size = new System.Drawing.Size(94, 24);
            this.btnClearSelections.TabIndex = 3;
            this.btnClearSelections.Text = "Uncheck All";
            this.btnClearSelections.UseVisualStyleBackColor = true;
            this.btnClearSelections.Click += new System.EventHandler(this.btnClearSelections_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnSelectAll.Location = new System.Drawing.Point(5, 18);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(94, 24);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Check All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lvDatabases
            // 
            this.lvDatabases.CheckBoxes = true;
            this.lvDatabases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DatabaseName});
            this.lvDatabases.GridLines = true;
            this.lvDatabases.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvDatabases.HideSelection = false;
            this.lvDatabases.Location = new System.Drawing.Point(6, 46);
            this.lvDatabases.Margin = new System.Windows.Forms.Padding(2);
            this.lvDatabases.Name = "lvDatabases";
            this.lvDatabases.Size = new System.Drawing.Size(498, 132);
            this.lvDatabases.TabIndex = 4;
            this.lvDatabases.UseCompatibleStateImageBehavior = false;
            this.lvDatabases.View = System.Windows.Forms.View.Details;
            // 
            // DatabaseName
            // 
            this.DatabaseName.Text = "Database Name";
            this.DatabaseName.Width = 494;
            // 
            // gbBackupDetails
            // 
            this.gbBackupDetails.Controls.Add(this.labelBackupName);
            this.gbBackupDetails.Controls.Add(this.tbDatabaseName);
            this.gbBackupDetails.Controls.Add(this.labelDescription);
            this.gbBackupDetails.Controls.Add(this.tbDatabaseDescription);
            this.gbBackupDetails.ForeColor = System.Drawing.Color.Blue;
            this.gbBackupDetails.Location = new System.Drawing.Point(5, 8);
            this.gbBackupDetails.Margin = new System.Windows.Forms.Padding(2);
            this.gbBackupDetails.Name = "gbBackupDetails";
            this.gbBackupDetails.Padding = new System.Windows.Forms.Padding(2);
            this.gbBackupDetails.Size = new System.Drawing.Size(510, 259);
            this.gbBackupDetails.TabIndex = 16;
            this.gbBackupDetails.TabStop = false;
            this.gbBackupDetails.Text = "Backup Details";
            // 
            // DatabaseManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 492);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbDatabases);
            this.Controls.Add(this.gbBackupDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "DatabaseManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBMgmtTest";
            this.Load += new System.EventHandler(this.DBMgmtTest_Load);
            this.gbDatabases.ResumeLayout(false);
            this.gbDatabases.PerformLayout();
            this.gbBackupDetails.ResumeLayout(false);
            this.gbBackupDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelBackupName;
        private System.Windows.Forms.TextBox tbDatabaseName;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox tbDatabaseDescription;
        private System.Windows.Forms.GroupBox gbDatabases;
        private System.Windows.Forms.Label labelSQLServer;
        private System.Windows.Forms.Label labelSQLServerText;
        private System.Windows.Forms.Button btnClearSelections;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ListView lvDatabases;
        private System.Windows.Forms.ColumnHeader DatabaseName;
        private System.Windows.Forms.GroupBox gbBackupDetails;
    }
}