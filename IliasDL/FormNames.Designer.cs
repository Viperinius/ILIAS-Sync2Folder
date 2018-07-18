namespace IliasDL
{
    partial class FormNames
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNames));
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.listViewCourses = new System.Windows.Forms.ListView();
            this.colCourse = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtCourseIlias = new System.Windows.Forms.TextBox();
            this.txtCourseOwn = new System.Windows.Forms.TextBox();
            this.colRef = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // progBar
            // 
            this.progBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBar.Location = new System.Drawing.Point(12, 10);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(638, 19);
            this.progBar.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(352, 750);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(236, 750);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // listViewCourses
            // 
            this.listViewCourses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCourses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCourse,
            this.colLocal,
            this.colRef});
            this.listViewCourses.FullRowSelect = true;
            this.listViewCourses.Location = new System.Drawing.Point(0, 35);
            this.listViewCourses.Name = "listViewCourses";
            this.listViewCourses.Size = new System.Drawing.Size(663, 594);
            this.listViewCourses.TabIndex = 8;
            this.listViewCourses.UseCompatibleStateImageBehavior = false;
            this.listViewCourses.View = System.Windows.Forms.View.Details;
            // 
            // colCourse
            // 
            this.colCourse.Text = "Kurs";
            this.colCourse.Width = 300;
            // 
            // colLocal
            // 
            this.colLocal.Text = "Eigener Name";
            this.colLocal.Width = 280;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Location = new System.Drawing.Point(263, 678);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(142, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Markierte Zeile bearbeiten";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(263, 707);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(142, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Änderung abspeichern";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // txtCourseIlias
            // 
            this.txtCourseIlias.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCourseIlias.Location = new System.Drawing.Point(12, 635);
            this.txtCourseIlias.Name = "txtCourseIlias";
            this.txtCourseIlias.ReadOnly = true;
            this.txtCourseIlias.Size = new System.Drawing.Size(299, 20);
            this.txtCourseIlias.TabIndex = 11;
            // 
            // txtCourseOwn
            // 
            this.txtCourseOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCourseOwn.Location = new System.Drawing.Point(352, 635);
            this.txtCourseOwn.Name = "txtCourseOwn";
            this.txtCourseOwn.Size = new System.Drawing.Size(299, 20);
            this.txtCourseOwn.TabIndex = 12;
            // 
            // colRef
            // 
            this.colRef.Text = "Ref ID";
            // 
            // FormNames
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(663, 784);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtCourseIlias);
            this.Controls.Add(this.txtCourseOwn);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.listViewCourses);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormNames";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kursnamen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNames_FormClosing);
            this.Load += new System.EventHandler(this.FormNames_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListView listViewCourses;
        private System.Windows.Forms.ColumnHeader colCourse;
        private System.Windows.Forms.ColumnHeader colLocal;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCourseIlias;
        private System.Windows.Forms.TextBox txtCourseOwn;
        private System.Windows.Forms.ColumnHeader colRef;
    }
}