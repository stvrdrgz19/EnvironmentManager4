namespace EnvironmentManager4.src.WinForms.Controls
{
    partial class CombinedProductVersionBuild
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
            this.productVersionLists1 = new EnvironmentManager4.src.WinForms.Controls.ProductVersionLists();
            this.buildList1 = new EnvironmentManager4.src.WinForms.Controls.BuildList();
            this.SuspendLayout();
            // 
            // productVersionLists1
            // 
            this.productVersionLists1.Location = new System.Drawing.Point(0, 0);
            this.productVersionLists1.Name = "productVersionLists1";
            this.productVersionLists1.Size = new System.Drawing.Size(500, 23);
            this.productVersionLists1.TabIndex = 2;
            // 
            // buildList1
            // 
            this.buildList1.Location = new System.Drawing.Point(1, 24);
            this.buildList1.Name = "buildList1";
            this.buildList1.Size = new System.Drawing.Size(498, 148);
            this.buildList1.TabIndex = 3;
            // 
            // CombinedProductVersionBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buildList1);
            this.Controls.Add(this.productVersionLists1);
            this.Name = "CombinedProductVersionBuild";
            this.Size = new System.Drawing.Size(500, 173);
            this.ResumeLayout(false);

        }

        #endregion
        private ProductVersionLists productVersionLists1;
        private BuildList buildList1;
    }
}
