
namespace BuildManagement
{
    partial class BuildLog
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
            this.cbProduct = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbDlls = new System.Windows.Forms.CheckBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvDlls = new System.Windows.Forms.ListView();
            this.chDllName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvBuilds = new System.Windows.Forms.ListView();
            this.chPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProduct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProduct
            // 
            this.cbProduct.AutoSize = true;
            this.cbProduct.Checked = true;
            this.cbProduct.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProduct.Location = new System.Drawing.Point(546, 439);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(90, 17);
            this.cbProduct.TabIndex = 11;
            this.cbProduct.Text = "Copy Product";
            this.cbProduct.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(8, 435);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // cbDlls
            // 
            this.cbDlls.AutoSize = true;
            this.cbDlls.Checked = true;
            this.cbDlls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDlls.Location = new System.Drawing.Point(642, 439);
            this.cbDlls.Name = "cbDlls";
            this.cbDlls.Size = new System.Drawing.Size(70, 17);
            this.cbDlls.TabIndex = 12;
            this.cbDlls.Text = "Copy Dlls";
            this.cbDlls.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(718, 435);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 9;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvDlls);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(2, 213);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(796, 219);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dlls";
            // 
            // lvDlls
            // 
            this.lvDlls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDllName,
            this.chType});
            this.lvDlls.FullRowSelect = true;
            this.lvDlls.GridLines = true;
            this.lvDlls.HideSelection = false;
            this.lvDlls.Location = new System.Drawing.Point(7, 20);
            this.lvDlls.Name = "lvDlls";
            this.lvDlls.Size = new System.Drawing.Size(783, 181);
            this.lvDlls.TabIndex = 0;
            this.lvDlls.TabStop = false;
            this.lvDlls.UseCompatibleStateImageBehavior = false;
            this.lvDlls.View = System.Windows.Forms.View.Details;
            // 
            // chDllName
            // 
            this.chDllName.Text = "Name";
            this.chDllName.Width = 534;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 245;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvBuilds);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(796, 207);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Builds";
            // 
            // lvBuilds
            // 
            this.lvBuilds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPath,
            this.chVersion,
            this.chDate,
            this.chProduct});
            this.lvBuilds.FullRowSelect = true;
            this.lvBuilds.GridLines = true;
            this.lvBuilds.HideSelection = false;
            this.lvBuilds.Location = new System.Drawing.Point(7, 20);
            this.lvBuilds.MultiSelect = false;
            this.lvBuilds.Name = "lvBuilds";
            this.lvBuilds.Size = new System.Drawing.Size(783, 181);
            this.lvBuilds.TabIndex = 0;
            this.lvBuilds.TabStop = false;
            this.lvBuilds.UseCompatibleStateImageBehavior = false;
            this.lvBuilds.View = System.Windows.Forms.View.Details;
            // 
            // chPath
            // 
            this.chPath.Text = "Path";
            this.chPath.Width = 484;
            // 
            // chVersion
            // 
            this.chVersion.Text = "Version";
            this.chVersion.Width = 50;
            // 
            // chDate
            // 
            this.chDate.Text = "Date";
            this.chDate.Width = 140;
            // 
            // chProduct
            // 
            this.chProduct.Text = "Product";
            this.chProduct.Width = 105;
            // 
            // BuildLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 460);
            this.Controls.Add(this.cbProduct);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cbDlls);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BuildLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Build Log";
            this.Load += new System.EventHandler(this.BuildLog_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbProduct;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox cbDlls;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvDlls;
        private System.Windows.Forms.ColumnHeader chDllName;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvBuilds;
        private System.Windows.Forms.ColumnHeader chPath;
        private System.Windows.Forms.ColumnHeader chVersion;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chProduct;
    }
}