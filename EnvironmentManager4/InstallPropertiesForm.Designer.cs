
namespace EnvironmentManager4
{
    partial class InstallPropertiesForm
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
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.lbExtended = new System.Windows.Forms.ListBox();
            this.lbCustom = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(13, 13);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.ReadOnly = true;
            this.tbProduct.Size = new System.Drawing.Size(775, 20);
            this.tbProduct.TabIndex = 0;
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(13, 39);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.ReadOnly = true;
            this.tbVersion.Size = new System.Drawing.Size(775, 20);
            this.tbVersion.TabIndex = 1;
            // 
            // lbExtended
            // 
            this.lbExtended.Enabled = false;
            this.lbExtended.FormattingEnabled = true;
            this.lbExtended.Location = new System.Drawing.Point(13, 66);
            this.lbExtended.Name = "lbExtended";
            this.lbExtended.Size = new System.Drawing.Size(380, 186);
            this.lbExtended.TabIndex = 2;
            // 
            // lbCustom
            // 
            this.lbCustom.Enabled = false;
            this.lbCustom.FormattingEnabled = true;
            this.lbCustom.Location = new System.Drawing.Point(408, 66);
            this.lbCustom.Name = "lbCustom";
            this.lbCustom.Size = new System.Drawing.Size(380, 186);
            this.lbCustom.TabIndex = 3;
            // 
            // InstallPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 263);
            this.Controls.Add(this.lbCustom);
            this.Controls.Add(this.lbExtended);
            this.Controls.Add(this.tbVersion);
            this.Controls.Add(this.tbProduct);
            this.Name = "InstallPropertiesForm";
            this.Text = "InstallPropertiesForm";
            this.Load += new System.EventHandler(this.InstallPropertiesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.ListBox lbExtended;
        private System.Windows.Forms.ListBox lbCustom;
    }
}