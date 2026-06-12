namespace EnvironmentManager4.src.WinForms.Controls
{
    partial class BuildList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvBuilds = new System.Windows.Forms.ListView();
            this.chBuildPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDateModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvBuilds
            // 
            this.lvBuilds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chBuildPath,
            this.chDateModified});
            this.lvBuilds.FullRowSelect = true;
            this.lvBuilds.GridLines = true;
            this.lvBuilds.HideSelection = false;
            this.lvBuilds.Location = new System.Drawing.Point(0, 0);
            this.lvBuilds.Name = "lvBuilds";
            this.lvBuilds.Size = new System.Drawing.Size(498, 148);
            this.lvBuilds.TabIndex = 2;
            this.lvBuilds.UseCompatibleStateImageBehavior = false;
            this.lvBuilds.View = System.Windows.Forms.View.Details;
            // 
            // chBuildPath
            // 
            this.chBuildPath.Text = "Build Path";
            this.chBuildPath.Width = 340;
            // 
            // chDateModified
            // 
            this.chDateModified.Text = "Date Modified";
            this.chDateModified.Width = 154;
            // 
            // BuildList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvBuilds);
            this.Name = "BuildList";
            this.Size = new System.Drawing.Size(498, 148);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvBuilds;
        private System.Windows.Forms.ColumnHeader chBuildPath;
        private System.Windows.Forms.ColumnHeader chDateModified;
    }
}
