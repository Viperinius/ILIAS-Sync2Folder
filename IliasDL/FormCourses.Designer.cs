namespace IliasDL
{
    partial class FormCourses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCourses));
            this.listViewCourses = new System.Windows.Forms.ListView();
            this.colCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCourse = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRefIds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkSyncAll = new System.Windows.Forms.CheckBox();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listViewCourses
            // 
            this.listViewCourses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCourses.CheckBoxes = true;
            this.listViewCourses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCheck,
            this.colCourse,
            this.colLocal,
            this.colRefIds});
            this.listViewCourses.Location = new System.Drawing.Point(0, 41);
            this.listViewCourses.Name = "listViewCourses";
            this.listViewCourses.Size = new System.Drawing.Size(595, 574);
            this.listViewCourses.TabIndex = 0;
            this.listViewCourses.UseCompatibleStateImageBehavior = false;
            this.listViewCourses.View = System.Windows.Forms.View.Details;
            // 
            // colCheck
            // 
            this.colCheck.Text = "";
            this.colCheck.Width = 30;
            // 
            // colCourse
            // 
            this.colCourse.Text = "Kurs";
            this.colCourse.Width = 300;
            // 
            // colLocal
            // 
            this.colLocal.Text = "Eigener Name";
            this.colLocal.Width = 200;
            // 
            // colRefIds
            // 
            this.colRefIds.Text = "Ref ID";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(188, 655);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 655);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkSyncAll
            // 
            this.chkSyncAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSyncAll.AutoSize = true;
            this.chkSyncAll.Location = new System.Drawing.Point(12, 638);
            this.chkSyncAll.Name = "chkSyncAll";
            this.chkSyncAll.Size = new System.Drawing.Size(149, 17);
            this.chkSyncAll.TabIndex = 3;
            this.chkSyncAll.Text = "Alle Kurse synchronisieren";
            this.chkSyncAll.UseVisualStyleBackColor = true;
            this.chkSyncAll.CheckedChanged += new System.EventHandler(this.ChkSyncAll_CheckedChanged);
            // 
            // progBar
            // 
            this.progBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBar.Location = new System.Drawing.Point(13, 12);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(570, 23);
            this.progBar.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 618);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(508, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Um nur ausgewählte Kurse zu synchronisieren, bitte alle nicht benötigten Kurse un" +
    "d \"Alle Kurse\" abwählen\r\n";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(12, 661);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(123, 17);
            this.chkSelectAll.TabIndex = 6;
            this.chkSelectAll.Text = "Alle aus- / abwählen";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.ChkSelectAll_CheckedChanged);
            // 
            // FormCourses
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(595, 688);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.chkSyncAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.listViewCourses);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCourses";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kurse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCourses_FormClosing);
            this.Load += new System.EventHandler(this.FormCourses_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewCourses;
        private System.Windows.Forms.ColumnHeader colCourse;
        private System.Windows.Forms.ColumnHeader colLocal;
        private System.Windows.Forms.ColumnHeader colRefIds;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colCheck;
        private System.Windows.Forms.CheckBox chkSyncAll;
        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}