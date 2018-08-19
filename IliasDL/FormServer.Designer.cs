namespace IliasDL
{
    partial class FormServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbServer = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.lbClient = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.picCheck = new System.Windows.Forms.PictureBox();
            this.picWaiting = new System.Windows.Forms.PictureBox();
            this.picFail = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFail)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(135, 150);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(235, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Location = new System.Drawing.Point(13, 13);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(401, 13);
            this.lbServer.TabIndex = 2;
            this.lbServer.Text = "Bitte entweder den Link zur ILIAS-Loginseite oder zum SOAP-Webservice einfügen:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(16, 29);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(398, 20);
            this.txtUrl.TabIndex = 3;
            // 
            // txtClient
            // 
            this.txtClient.Location = new System.Drawing.Point(16, 117);
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(429, 20);
            this.txtClient.TabIndex = 4;
            // 
            // lbClient
            // 
            this.lbClient.AutoSize = true;
            this.lbClient.Location = new System.Drawing.Point(13, 101);
            this.lbClient.Name = "lbClient";
            this.lbClient.Size = new System.Drawing.Size(427, 13);
            this.lbClient.TabIndex = 5;
            this.lbClient.Text = "Hier die Client-ID eintragen, falls diese nach der Überprüfung nicht automatisch " +
    "erscheint:";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(188, 57);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 6;
            this.btnCheck.Text = "Überprüfen";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
            // 
            // picCheck
            // 
            this.picCheck.Image = ((System.Drawing.Image)(resources.GetObject("picCheck.Image")));
            this.picCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picCheck.Location = new System.Drawing.Point(425, 29);
            this.picCheck.Name = "picCheck";
            this.picCheck.Size = new System.Drawing.Size(20, 21);
            this.picCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCheck.TabIndex = 7;
            this.picCheck.TabStop = false;
            this.picCheck.Visible = false;
            // 
            // picWaiting
            // 
            this.picWaiting.Image = ((System.Drawing.Image)(resources.GetObject("picWaiting.Image")));
            this.picWaiting.Location = new System.Drawing.Point(425, 29);
            this.picWaiting.Name = "picWaiting";
            this.picWaiting.Size = new System.Drawing.Size(20, 21);
            this.picWaiting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picWaiting.TabIndex = 8;
            this.picWaiting.TabStop = false;
            // 
            // picFail
            // 
            this.picFail.Image = ((System.Drawing.Image)(resources.GetObject("picFail.Image")));
            this.picFail.Location = new System.Drawing.Point(425, 29);
            this.picFail.Name = "picFail";
            this.picFail.Size = new System.Drawing.Size(20, 21);
            this.picFail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFail.TabIndex = 9;
            this.picFail.TabStop = false;
            this.picFail.Visible = false;
            // 
            // FormServer
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(457, 196);
            this.Controls.Add(this.picFail);
            this.Controls.Add(this.picWaiting);
            this.Controls.Add(this.picCheck);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.lbClient);
            this.Controls.Add(this.txtClient);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lbServer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server einstellen...";
            ((System.ComponentModel.ISupportInitialize)(this.picCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtClient;
        private System.Windows.Forms.Label lbClient;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.PictureBox picCheck;
        private System.Windows.Forms.PictureBox picWaiting;
        private System.Windows.Forms.PictureBox picFail;
    }
}