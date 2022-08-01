namespace samsung_mainLine
{
    partial class startingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(startingForm));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.loadingStatus = new Bunifu.UI.WinForms.BunifuLabel();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // loadingStatus
            // 
            this.loadingStatus.AllowParentOverrides = false;
            this.loadingStatus.AutoEllipsis = false;
            this.loadingStatus.CursorType = System.Windows.Forms.Cursors.Default;
            this.loadingStatus.Font = new System.Drawing.Font("Inter Thin", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadingStatus.ForeColor = System.Drawing.Color.Gainsboro;
            this.loadingStatus.Location = new System.Drawing.Point(41, 330);
            this.loadingStatus.Name = "loadingStatus";
            this.loadingStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.loadingStatus.Size = new System.Drawing.Size(93, 16);
            this.loadingStatus.TabIndex = 0;
            this.loadingStatus.Text = "Loading Status";
            this.loadingStatus.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.loadingStatus.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // startingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1057, 440);
            this.Controls.Add(this.loadingStatus);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "startingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "startingForm";
            this.Shown += new System.EventHandler(this.startingForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private Bunifu.UI.WinForms.BunifuLabel loadingStatus;
    }
}