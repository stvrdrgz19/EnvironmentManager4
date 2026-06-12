namespace EnvironmentManager4.src.WinForms.Controls
{
    partial class ProductVersionLists
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
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.cbVersions = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbProducts
            // 
            this.cbProducts.FormattingEnabled = true;
            this.cbProducts.Location = new System.Drawing.Point(1, 1);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(428, 21);
            this.cbProducts.TabIndex = 0;
            this.cbProducts.Text = "Select a Product...";
            // 
            // cbVersions
            // 
            this.cbVersions.FormattingEnabled = true;
            this.cbVersions.Location = new System.Drawing.Point(431, 1);
            this.cbVersions.Name = "cbVersions";
            this.cbVersions.Size = new System.Drawing.Size(68, 21);
            this.cbVersions.TabIndex = 1;
            // 
            // ProductVersionLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbVersions);
            this.Controls.Add(this.cbProducts);
            this.Name = "ProductVersionLists";
            this.Size = new System.Drawing.Size(500, 23);
            this.Load += new System.EventHandler(this.ProductVersionLists_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.ComboBox cbVersions;
    }
}
