namespace IliasDL
{
    partial class FormInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInfo));
            this.lbTitle = new System.Windows.Forms.Label();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.linkOctokit = new System.Windows.Forms.LinkLabel();
            this.linkWunder = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Location = new System.Drawing.Point(13, 13);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(382, 91);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "Made by Viperinius 2018\r\n(MIT License)\r\n\r\nInstallierte Version: $\r\n\r\nFür Hilfen u" +
    "nd aktuelle Versionshinweise besuchen Sie unseren Discord Server \r\n(Link im Hilf" +
    "emenü unter Feedback verfügbar).";
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.ForeColor = System.Drawing.Color.DarkBlue;
            this.linkGithub.Location = new System.Drawing.Point(13, 114);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(91, 13);
            this.linkGithub.TabIndex = 1;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "Github-Repository";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGithub_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Lizenzen / Verwendetes:";
            // 
            // linkOctokit
            // 
            this.linkOctokit.AutoSize = true;
            this.linkOctokit.ForeColor = System.Drawing.Color.DarkBlue;
            this.linkOctokit.Location = new System.Drawing.Point(13, 164);
            this.linkOctokit.Name = "linkOctokit";
            this.linkOctokit.Size = new System.Drawing.Size(69, 13);
            this.linkOctokit.TabIndex = 3;
            this.linkOctokit.TabStop = true;
            this.linkOctokit.Text = "Octokit (MIT)";
            this.linkOctokit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkOctokit_LinkClicked);
            // 
            // linkWunder
            // 
            this.linkWunder.AutoSize = true;
            this.linkWunder.ForeColor = System.Drawing.Color.DarkBlue;
            this.linkWunder.Location = new System.Drawing.Point(13, 187);
            this.linkWunder.Name = "linkWunder";
            this.linkWunder.Size = new System.Drawing.Size(174, 13);
            this.linkWunder.TabIndex = 4;
            this.linkWunder.TabStop = true;
            this.linkWunder.Text = "Wunder.ClickOnceUninstaller (MIT)";
            this.linkWunder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkWunder_LinkClicked);
            // 
            // FormInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 209);
            this.Controls.Add(this.linkWunder);
            this.Controls.Add(this.linkOctokit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkGithub);
            this.Controls.Add(this.lbTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormInfo";
            this.Load += new System.EventHandler(this.FormInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.LinkLabel linkGithub;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkOctokit;
        private System.Windows.Forms.LinkLabel linkWunder;
    }
}