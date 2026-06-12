namespace EnvironmentManager4.src.WinForms.Controls
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
            this.combinedProductVersionBuild1 = new EnvironmentManager4.src.WinForms.Controls.CombinedProductVersionBuild();
            this.productVersionLists1 = new EnvironmentManager4.src.WinForms.Controls.ProductVersionLists();
            this.button1 = new System.Windows.Forms.Button();
            this.buildList1 = new EnvironmentManager4.src.WinForms.Controls.BuildList();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // combinedProductVersionBuild1
            // 
            this.combinedProductVersionBuild1.Location = new System.Drawing.Point(0, 0);
            this.combinedProductVersionBuild1.Name = "combinedProductVersionBuild1";
            this.combinedProductVersionBuild1.Size = new System.Drawing.Size(500, 173);
            this.combinedProductVersionBuild1.TabIndex = 0;
            // 
            // productVersionLists1
            // 
            this.productVersionLists1.Location = new System.Drawing.Point(0, 242);
            this.productVersionLists1.Name = "productVersionLists1";
            this.productVersionLists1.Size = new System.Drawing.Size(500, 23);
            this.productVersionLists1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 271);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buildList1
            // 
            this.buildList1.Location = new System.Drawing.Point(0, 321);
            this.buildList1.Name = "buildList1";
            this.buildList1.Size = new System.Drawing.Size(498, 148);
            this.buildList1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 475);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 677);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buildList1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.productVersionLists1);
            this.Controls.Add(this.combinedProductVersionBuild1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CombinedProductVersionBuild combinedProductVersionBuild1;
        private ProductVersionLists productVersionLists1;
        private System.Windows.Forms.Button button1;
        private BuildList buildList1;
        private System.Windows.Forms.Button button2;
    }
}