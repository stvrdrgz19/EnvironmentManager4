
namespace EnvironmentManager4
{
    partial class DatabaseActivityLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseActivityLog));
            this.lvDatabaseActivityLog = new System.Windows.Forms.ListView();
            this.chTimeStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBackup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvDatabaseActivityLog
            // 
            this.lvDatabaseActivityLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTimeStamp,
            this.chAction,
            this.chBackup});
            this.lvDatabaseActivityLog.FullRowSelect = true;
            this.lvDatabaseActivityLog.GridLines = true;
            this.lvDatabaseActivityLog.HideSelection = false;
            this.lvDatabaseActivityLog.Location = new System.Drawing.Point(12, 13);
            this.lvDatabaseActivityLog.MultiSelect = false;
            this.lvDatabaseActivityLog.Name = "lvDatabaseActivityLog";
            this.lvDatabaseActivityLog.Size = new System.Drawing.Size(737, 289);
            this.lvDatabaseActivityLog.TabIndex = 1;
            this.lvDatabaseActivityLog.TabStop = false;
            this.lvDatabaseActivityLog.UseCompatibleStateImageBehavior = false;
            this.lvDatabaseActivityLog.View = System.Windows.Forms.View.Details;
            // 
            // chTimeStamp
            // 
            this.chTimeStamp.Text = "Time Stamp";
            this.chTimeStamp.Width = 140;
            // 
            // chAction
            // 
            this.chAction.Text = "Action";
            this.chAction.Width = 105;
            // 
            // chBackup
            // 
            this.chBackup.Text = "Backup";
            this.chBackup.Width = 488;
            // 
            // DatabaseActivityLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 315);
            this.Controls.Add(this.lvDatabaseActivityLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DatabaseActivityLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Activity Log";
            this.Load += new System.EventHandler(this.DatabaseActivityLog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDatabaseActivityLog;
        private System.Windows.Forms.ColumnHeader chTimeStamp;
        private System.Windows.Forms.ColumnHeader chAction;
        private System.Windows.Forms.ColumnHeader chBackup;
    }
}