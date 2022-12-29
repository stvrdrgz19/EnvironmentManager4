
namespace EnvironmentManager4
{
    partial class TestForm
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
            this.lvBuilds = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvBuilds
            // 
            this.lvBuilds.FullRowSelect = true;
            this.lvBuilds.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvBuilds.HideSelection = false;
            this.lvBuilds.Location = new System.Drawing.Point(13, 55);
            this.lvBuilds.Name = "lvBuilds";
            this.lvBuilds.Size = new System.Drawing.Size(775, 383);
            this.lvBuilds.TabIndex = 0;
            this.lvBuilds.UseCompatibleStateImageBehavior = false;
            this.lvBuilds.View = System.Windows.Forms.View.Details;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lvBuilds);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvBuilds;
    }
}