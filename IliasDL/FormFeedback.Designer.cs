namespace IliasDL
{
    partial class FormFeedback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeedback));
            this.lbTitle = new System.Windows.Forms.Label();
            this.linklbDiscord = new System.Windows.Forms.LinkLabel();
            this.linklbMail = new System.Windows.Forms.LinkLabel();
            this.lbSubTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Location = new System.Drawing.Point(12, 9);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(382, 26);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "Gibt es Fragen oder Probleme?\r\nDann nutzen Sie bitte eine der folgenden Möglichke" +
    "iten, Kontakt aufzunehmen:";
            // 
            // linklbDiscord
            // 
            this.linklbDiscord.AutoSize = true;
            this.linklbDiscord.Location = new System.Drawing.Point(12, 46);
            this.linklbDiscord.Name = "linklbDiscord";
            this.linklbDiscord.Size = new System.Drawing.Size(77, 13);
            this.linklbDiscord.TabIndex = 1;
            this.linklbDiscord.TabStop = true;
            this.linklbDiscord.Text = "Discord Server";
            this.linklbDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinklbDiscord_LinkClicked);
            // 
            // linklbMail
            // 
            this.linklbMail.AutoSize = true;
            this.linklbMail.Location = new System.Drawing.Point(13, 63);
            this.linklbMail.Name = "linklbMail";
            this.linklbMail.Size = new System.Drawing.Size(139, 13);
            this.linklbMail.TabIndex = 2;
            this.linklbMail.TabStop = true;
            this.linklbMail.Text = "E-mail to viperinius@gmx.de";
            this.linklbMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinklbMail_LinkClicked);
            // 
            // lbSubTitle
            // 
            this.lbSubTitle.AutoSize = true;
            this.lbSubTitle.Location = new System.Drawing.Point(12, 87);
            this.lbSubTitle.Name = "lbSubTitle";
            this.lbSubTitle.Size = new System.Drawing.Size(318, 26);
            this.lbSubTitle.TabIndex = 3;
            this.lbSubTitle.Text = "Falls Sie einen Bug oder ein anderes Problem melden möchten,\r\nbitte (sofern mögli" +
    "ch) einen Screenshot des Problems mitschicken.";
            // 
            // FormFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 135);
            this.Controls.Add(this.lbSubTitle);
            this.Controls.Add(this.linklbMail);
            this.Controls.Add(this.linklbDiscord);
            this.Controls.Add(this.lbTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFeedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Feedback";
            this.Load += new System.EventHandler(this.FormFeedback_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.LinkLabel linklbDiscord;
        private System.Windows.Forms.LinkLabel linklbMail;
        private System.Windows.Forms.Label lbSubTitle;
    }
}