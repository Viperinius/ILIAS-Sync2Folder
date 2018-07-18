namespace IliasDL
{
    partial class FormPath
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPath));
            this.lbPathInfo = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btn = new System.Windows.Forms.Button();
            this.dialogSavePath = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbPathInfo
            // 
            this.lbPathInfo.AutoSize = true;
            this.lbPathInfo.Location = new System.Drawing.Point(13, 9);
            this.lbPathInfo.Name = "lbPathInfo";
            this.lbPathInfo.Size = new System.Drawing.Size(285, 13);
            this.lbPathInfo.TabIndex = 0;
            this.lbPathInfo.Text = "Bitte wählen Sie Ihr gewünschtes Speicherverzeichnis aus:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(13, 30);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(273, 20);
            this.txtPath.TabIndex = 1;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(292, 30);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(31, 20);
            this.btn.TabIndex = 2;
            this.btn.Text = "...";
            this.btn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.Btn_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(49, 67);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(211, 67);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormPath
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(339, 102);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lbPathInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Verzeichnis";
            this.Load += new System.EventHandler(this.FormPath_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbPathInfo;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.FolderBrowserDialog dialogSavePath;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}