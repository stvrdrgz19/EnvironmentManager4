
namespace EnvironmentManager4
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelIsAppUpdated = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkProject = new System.Windows.Forms.LinkLabel();
            this.linkWiki = new System.Windows.Forms.LinkLabel();
            this.linkRepo = new System.Windows.Forms.LinkLabel();
            this.LinkChangeLog = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(113)))), ((int)(((byte)(155)))));
            this.labelVersion.Location = new System.Drawing.Point(13, 13);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(85, 29);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "label1";
            // 
            // labelIsAppUpdated
            // 
            this.labelIsAppUpdated.AutoSize = true;
            this.labelIsAppUpdated.Location = new System.Drawing.Point(16, 42);
            this.labelIsAppUpdated.Name = "labelIsAppUpdated";
            this.labelIsAppUpdated.Size = new System.Drawing.Size(35, 13);
            this.labelIsAppUpdated.TabIndex = 1;
            this.labelIsAppUpdated.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(113)))), ((int)(((byte)(155)))));
            this.label1.Location = new System.Drawing.Point(14, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Links";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(15, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Project Board:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Wiki Page:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(15, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Repo:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(16, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Change Log:";
            // 
            // linkProject
            // 
            this.linkProject.AutoSize = true;
            this.linkProject.BackColor = System.Drawing.Color.White;
            this.linkProject.Location = new System.Drawing.Point(122, 102);
            this.linkProject.Name = "linkProject";
            this.linkProject.Size = new System.Drawing.Size(314, 13);
            this.linkProject.TabIndex = 2;
            this.linkProject.TabStop = true;
            this.linkProject.Text = "https://github.com/stvrdrgz19/EnvironmentManager4/projects/1";
            this.linkProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkProject_LinkClicked);
            // 
            // linkWiki
            // 
            this.linkWiki.AutoSize = true;
            this.linkWiki.BackColor = System.Drawing.Color.White;
            this.linkWiki.Location = new System.Drawing.Point(122, 117);
            this.linkWiki.Name = "linkWiki";
            this.linkWiki.Size = new System.Drawing.Size(284, 13);
            this.linkWiki.TabIndex = 8;
            this.linkWiki.TabStop = true;
            this.linkWiki.Text = "https://github.com/stvrdrgz19/EnvironmentManager4/wiki";
            this.linkWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWiki_LinkClicked);
            // 
            // linkRepo
            // 
            this.linkRepo.AutoSize = true;
            this.linkRepo.BackColor = System.Drawing.Color.White;
            this.linkRepo.Location = new System.Drawing.Point(122, 132);
            this.linkRepo.Name = "linkRepo";
            this.linkRepo.Size = new System.Drawing.Size(261, 13);
            this.linkRepo.TabIndex = 9;
            this.linkRepo.TabStop = true;
            this.linkRepo.Text = "https://github.com/stvrdrgz19/EnvironmentManager4";
            this.linkRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRepo_LinkClicked);
            // 
            // LinkChangeLog
            // 
            this.LinkChangeLog.AutoSize = true;
            this.LinkChangeLog.BackColor = System.Drawing.Color.White;
            this.LinkChangeLog.Location = new System.Drawing.Point(122, 147);
            this.LinkChangeLog.Name = "LinkChangeLog";
            this.LinkChangeLog.Size = new System.Drawing.Size(347, 13);
            this.LinkChangeLog.TabIndex = 10;
            this.LinkChangeLog.TabStop = true;
            this.LinkChangeLog.Text = "https://github.com/stvrdrgz19/EnvironmentManager4/wiki/Change-Log";
            this.LinkChangeLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkChangeLog_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(-2, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(610, 117);
            this.panel1.TabIndex = 11;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(526, 187);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::EnvironmentManager4.Properties.Resources.EnvMgr;
            this.btnUpdate.Location = new System.Drawing.Point(551, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(50, 50);
            this.btnUpdate.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnUpdate, "Install Latest Version");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 218);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.LinkChangeLog);
            this.Controls.Add(this.linkRepo);
            this.Controls.Add(this.linkWiki);
            this.Controls.Add(this.linkProject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelIsAppUpdated);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Environment Manager - About";
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelIsAppUpdated;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkProject;
        private System.Windows.Forms.LinkLabel linkWiki;
        private System.Windows.Forms.LinkLabel linkRepo;
        private System.Windows.Forms.LinkLabel LinkChangeLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}