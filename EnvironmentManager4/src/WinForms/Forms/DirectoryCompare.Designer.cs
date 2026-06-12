
namespace EnvironmentManager4
{
    partial class DirectoryCompare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryCompare));
            this.checkMissing = new System.Windows.Forms.CheckBox();
            this.checkDuplicates = new System.Windows.Forms.CheckBox();
            this.checkIncludeSubDirectories = new System.Windows.Forms.CheckBox();
            this.tbPath1 = new System.Windows.Forms.TextBox();
            this.btnPath1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnPath2 = new System.Windows.Forms.Button();
            this.tbPath2 = new System.Windows.Forms.TextBox();
            this.tbList1Count = new System.Windows.Forms.TextBox();
            this.tbList2Count = new System.Windows.Forms.TextBox();
            this.lbList1 = new System.Windows.Forms.ListBox();
            this.lbList2 = new System.Windows.Forms.ListBox();
            this.tbResultsCount = new System.Windows.Forms.TextBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.btnCopySelected = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.btnCompareLists = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkMissing
            // 
            this.checkMissing.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkMissing.AutoSize = true;
            this.checkMissing.Checked = true;
            this.checkMissing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMissing.Location = new System.Drawing.Point(3, 3);
            this.checkMissing.Name = "checkMissing";
            this.checkMissing.Size = new System.Drawing.Size(52, 23);
            this.checkMissing.TabIndex = 0;
            this.checkMissing.Text = "Missing";
            this.checkMissing.UseVisualStyleBackColor = true;
            this.checkMissing.CheckedChanged += new System.EventHandler(this.checkMissing_CheckedChanged);
            // 
            // checkDuplicates
            // 
            this.checkDuplicates.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkDuplicates.AutoSize = true;
            this.checkDuplicates.Location = new System.Drawing.Point(56, 3);
            this.checkDuplicates.Name = "checkDuplicates";
            this.checkDuplicates.Size = new System.Drawing.Size(67, 23);
            this.checkDuplicates.TabIndex = 1;
            this.checkDuplicates.Text = "Duplicates";
            this.checkDuplicates.UseVisualStyleBackColor = true;
            this.checkDuplicates.CheckedChanged += new System.EventHandler(this.checkDuplicates_CheckedChanged);
            // 
            // checkIncludeSubDirectories
            // 
            this.checkIncludeSubDirectories.AutoSize = true;
            this.checkIncludeSubDirectories.Checked = true;
            this.checkIncludeSubDirectories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIncludeSubDirectories.Location = new System.Drawing.Point(128, 7);
            this.checkIncludeSubDirectories.Name = "checkIncludeSubDirectories";
            this.checkIncludeSubDirectories.Size = new System.Drawing.Size(134, 17);
            this.checkIncludeSubDirectories.TabIndex = 2;
            this.checkIncludeSubDirectories.Text = "Include Sub-directories";
            this.checkIncludeSubDirectories.UseVisualStyleBackColor = true;
            this.checkIncludeSubDirectories.CheckedChanged += new System.EventHandler(this.checkIncludeSubDirectories_CheckedChanged);
            // 
            // tbPath1
            // 
            this.tbPath1.Location = new System.Drawing.Point(4, 29);
            this.tbPath1.Name = "tbPath1";
            this.tbPath1.Size = new System.Drawing.Size(804, 20);
            this.tbPath1.TabIndex = 3;
            this.tbPath1.TextChanged += new System.EventHandler(this.tbPath1_TextChanged);
            // 
            // btnPath1
            // 
            this.btnPath1.ImageIndex = 0;
            this.btnPath1.ImageList = this.imageList1;
            this.btnPath1.Location = new System.Drawing.Point(808, 28);
            this.btnPath1.Name = "btnPath1";
            this.btnPath1.Size = new System.Drawing.Size(23, 22);
            this.btnPath1.TabIndex = 4;
            this.btnPath1.UseVisualStyleBackColor = true;
            this.btnPath1.Click += new System.EventHandler(this.btnPath1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ico4.ico");
            this.imageList1.Images.SetKeyName(1, "icon3.ico");
            this.imageList1.Images.SetKeyName(2, "EnvMgr.ico");
            // 
            // btnPath2
            // 
            this.btnPath2.ImageKey = "ico4.ico";
            this.btnPath2.ImageList = this.imageList1;
            this.btnPath2.Location = new System.Drawing.Point(808, 51);
            this.btnPath2.Name = "btnPath2";
            this.btnPath2.Size = new System.Drawing.Size(23, 22);
            this.btnPath2.TabIndex = 6;
            this.btnPath2.UseVisualStyleBackColor = true;
            this.btnPath2.Click += new System.EventHandler(this.btnPath2_Click);
            // 
            // tbPath2
            // 
            this.tbPath2.Location = new System.Drawing.Point(4, 52);
            this.tbPath2.Name = "tbPath2";
            this.tbPath2.Size = new System.Drawing.Size(804, 20);
            this.tbPath2.TabIndex = 5;
            this.tbPath2.TextChanged += new System.EventHandler(this.tbPath2_TextChanged);
            // 
            // tbList1Count
            // 
            this.tbList1Count.Location = new System.Drawing.Point(160, 78);
            this.tbList1Count.Name = "tbList1Count";
            this.tbList1Count.ReadOnly = true;
            this.tbList1Count.Size = new System.Drawing.Size(100, 20);
            this.tbList1Count.TabIndex = 7;
            this.tbList1Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbList2Count
            // 
            this.tbList2Count.Location = new System.Drawing.Point(574, 78);
            this.tbList2Count.Name = "tbList2Count";
            this.tbList2Count.ReadOnly = true;
            this.tbList2Count.Size = new System.Drawing.Size(100, 20);
            this.tbList2Count.TabIndex = 8;
            this.tbList2Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbList1
            // 
            this.lbList1.FormattingEnabled = true;
            this.lbList1.Location = new System.Drawing.Point(4, 100);
            this.lbList1.Name = "lbList1";
            this.lbList1.Size = new System.Drawing.Size(412, 186);
            this.lbList1.TabIndex = 9;
            // 
            // lbList2
            // 
            this.lbList2.FormattingEnabled = true;
            this.lbList2.Location = new System.Drawing.Point(418, 100);
            this.lbList2.Name = "lbList2";
            this.lbList2.Size = new System.Drawing.Size(412, 186);
            this.lbList2.TabIndex = 10;
            // 
            // tbResultsCount
            // 
            this.tbResultsCount.Location = new System.Drawing.Point(367, 294);
            this.tbResultsCount.Name = "tbResultsCount";
            this.tbResultsCount.ReadOnly = true;
            this.tbResultsCount.Size = new System.Drawing.Size(100, 20);
            this.tbResultsCount.TabIndex = 11;
            this.tbResultsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lvResults
            // 
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(4, 316);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(826, 216);
            this.lvResults.TabIndex = 12;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            // 
            // btnCopySelected
            // 
            this.btnCopySelected.Location = new System.Drawing.Point(3, 535);
            this.btnCopySelected.Name = "btnCopySelected";
            this.btnCopySelected.Size = new System.Drawing.Size(100, 23);
            this.btnCopySelected.TabIndex = 13;
            this.btnCopySelected.Text = "Copy Selected";
            this.btnCopySelected.UseVisualStyleBackColor = true;
            this.btnCopySelected.Click += new System.EventHandler(this.btnCopySelected_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(103, 535);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(100, 23);
            this.btnCopyAll.TabIndex = 14;
            this.btnCopyAll.Text = "Copy All";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // btnCompareLists
            // 
            this.btnCompareLists.Location = new System.Drawing.Point(367, 534);
            this.btnCompareLists.Name = "btnCompareLists";
            this.btnCompareLists.Size = new System.Drawing.Size(100, 23);
            this.btnCompareLists.TabIndex = 15;
            this.btnCompareLists.Text = "Compare Lists";
            this.btnCompareLists.UseVisualStyleBackColor = true;
            this.btnCompareLists.Click += new System.EventHandler(this.btnCompareLists_Click);
            // 
            // DirectoryCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.btnCompareLists);
            this.Controls.Add(this.btnCopyAll);
            this.Controls.Add(this.btnCopySelected);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.tbResultsCount);
            this.Controls.Add(this.lbList2);
            this.Controls.Add(this.lbList1);
            this.Controls.Add(this.tbList2Count);
            this.Controls.Add(this.tbList1Count);
            this.Controls.Add(this.btnPath2);
            this.Controls.Add(this.tbPath2);
            this.Controls.Add(this.btnPath1);
            this.Controls.Add(this.tbPath1);
            this.Controls.Add(this.checkIncludeSubDirectories);
            this.Controls.Add(this.checkDuplicates);
            this.Controls.Add(this.checkMissing);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DirectoryCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Directory Compare";
            this.Load += new System.EventHandler(this.DirectoryCompare_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkMissing;
        private System.Windows.Forms.CheckBox checkDuplicates;
        private System.Windows.Forms.CheckBox checkIncludeSubDirectories;
        private System.Windows.Forms.TextBox tbPath1;
        private System.Windows.Forms.Button btnPath1;
        private System.Windows.Forms.Button btnPath2;
        private System.Windows.Forms.TextBox tbPath2;
        private System.Windows.Forms.TextBox tbList1Count;
        private System.Windows.Forms.TextBox tbList2Count;
        private System.Windows.Forms.ListBox lbList1;
        private System.Windows.Forms.ListBox lbList2;
        private System.Windows.Forms.TextBox tbResultsCount;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.Button btnCopySelected;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.Button btnCompareLists;
        private System.Windows.Forms.ImageList imageList1;
    }
}