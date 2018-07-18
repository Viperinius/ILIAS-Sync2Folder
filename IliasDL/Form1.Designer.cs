namespace IliasDL
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.courseNamingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coursesLikeILIASToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.courseOwnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderLikeILIASToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.semesterStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konfigurierenToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.configureFoldersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.synchronisationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coursesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSynchronisationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackBugsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lbProgAll = new System.Windows.Forms.Label();
            this.lbProgressAll = new System.Windows.Forms.Label();
            this.progressBarAll = new System.Windows.Forms.ProgressBar();
            this.lbProgActiveCourse = new System.Windows.Forms.Label();
            this.lbProgressCourse = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.chkListSyncOptions = new System.Windows.Forms.CheckedListBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.groupLogin = new System.Windows.Forms.GroupBox();
            this.picCheck = new System.Windows.Forms.PictureBox();
            this.lbLoggedIn = new System.Windows.Forms.Label();
            this.btnLoginConfirm = new System.Windows.Forms.Button();
            this.txtPasswInput = new System.Windows.Forms.TextBox();
            this.lbPassw = new System.Windows.Forms.Label();
            this.txtUserInput = new System.Windows.Forms.TextBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.listViewData = new System.Windows.Forms.ListView();
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.groupLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localPathToolStripMenuItem,
            this.filesToolStripMenuItem,
            this.synchronisationToolStripMenuItem,
            this.languageToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            // 
            // localPathToolStripMenuItem
            // 
            this.localPathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPathToolStripMenuItem,
            this.changePathToolStripMenuItem});
            this.localPathToolStripMenuItem.Name = "localPathToolStripMenuItem";
            resources.ApplyResources(this.localPathToolStripMenuItem, "localPathToolStripMenuItem");
            // 
            // openPathToolStripMenuItem
            // 
            this.openPathToolStripMenuItem.Name = "openPathToolStripMenuItem";
            resources.ApplyResources(this.openPathToolStripMenuItem, "openPathToolStripMenuItem");
            this.openPathToolStripMenuItem.Click += new System.EventHandler(this.OpenPathToolStripMenuItem_Click);
            // 
            // changePathToolStripMenuItem
            // 
            this.changePathToolStripMenuItem.Name = "changePathToolStripMenuItem";
            resources.ApplyResources(this.changePathToolStripMenuItem, "changePathToolStripMenuItem");
            this.changePathToolStripMenuItem.Click += new System.EventHandler(this.ChangePathToolStripMenuItem_Click);
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.courseNamingToolStripMenuItem,
            this.folderStructureToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            resources.ApplyResources(this.filesToolStripMenuItem, "filesToolStripMenuItem");
            // 
            // courseNamingToolStripMenuItem
            // 
            this.courseNamingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coursesLikeILIASToolStripMenuItem,
            this.courseOwnToolStripMenuItem});
            this.courseNamingToolStripMenuItem.Name = "courseNamingToolStripMenuItem";
            resources.ApplyResources(this.courseNamingToolStripMenuItem, "courseNamingToolStripMenuItem");
            // 
            // coursesLikeILIASToolStripMenuItem
            // 
            this.coursesLikeILIASToolStripMenuItem.Checked = true;
            this.coursesLikeILIASToolStripMenuItem.CheckOnClick = true;
            this.coursesLikeILIASToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coursesLikeILIASToolStripMenuItem.Name = "coursesLikeILIASToolStripMenuItem";
            resources.ApplyResources(this.coursesLikeILIASToolStripMenuItem, "coursesLikeILIASToolStripMenuItem");
            this.coursesLikeILIASToolStripMenuItem.Click += new System.EventHandler(this.CoursesLikeILIASToolStripMenuItem_Click);
            // 
            // courseOwnToolStripMenuItem
            // 
            this.courseOwnToolStripMenuItem.CheckOnClick = true;
            this.courseOwnToolStripMenuItem.Name = "courseOwnToolStripMenuItem";
            resources.ApplyResources(this.courseOwnToolStripMenuItem, "courseOwnToolStripMenuItem");
            this.courseOwnToolStripMenuItem.Click += new System.EventHandler(this.CourseOwnToolStripMenuItem_Click);
            // 
            // folderStructureToolStripMenuItem
            // 
            this.folderStructureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.folderLikeILIASToolStripMenuItem,
            this.semesterStructureToolStripMenuItem,
            this.konfigurierenToolStripMenuItem,
            this.configureFoldersToolStripMenuItem1});
            this.folderStructureToolStripMenuItem.Name = "folderStructureToolStripMenuItem";
            resources.ApplyResources(this.folderStructureToolStripMenuItem, "folderStructureToolStripMenuItem");
            // 
            // folderLikeILIASToolStripMenuItem
            // 
            this.folderLikeILIASToolStripMenuItem.Checked = true;
            this.folderLikeILIASToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.folderLikeILIASToolStripMenuItem.Name = "folderLikeILIASToolStripMenuItem";
            resources.ApplyResources(this.folderLikeILIASToolStripMenuItem, "folderLikeILIASToolStripMenuItem");
            this.folderLikeILIASToolStripMenuItem.Click += new System.EventHandler(this.FolderLikeILIASToolStripMenuItem_Click);
            // 
            // semesterStructureToolStripMenuItem
            // 
            this.semesterStructureToolStripMenuItem.Name = "semesterStructureToolStripMenuItem";
            resources.ApplyResources(this.semesterStructureToolStripMenuItem, "semesterStructureToolStripMenuItem");
            this.semesterStructureToolStripMenuItem.Click += new System.EventHandler(this.SemesterStructureToolStripMenuItem_Click);
            // 
            // konfigurierenToolStripMenuItem
            // 
            this.konfigurierenToolStripMenuItem.Name = "konfigurierenToolStripMenuItem";
            resources.ApplyResources(this.konfigurierenToolStripMenuItem, "konfigurierenToolStripMenuItem");
            // 
            // configureFoldersToolStripMenuItem1
            // 
            this.configureFoldersToolStripMenuItem1.Name = "configureFoldersToolStripMenuItem1";
            resources.ApplyResources(this.configureFoldersToolStripMenuItem1, "configureFoldersToolStripMenuItem1");
            this.configureFoldersToolStripMenuItem1.Click += new System.EventHandler(this.ConfigureFoldersToolStripMenuItem1_Click);
            // 
            // synchronisationToolStripMenuItem
            // 
            this.synchronisationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverToolStripMenuItem,
            this.coursesToolStripMenuItem,
            this.autoSynchronisationToolStripMenuItem});
            this.synchronisationToolStripMenuItem.Name = "synchronisationToolStripMenuItem";
            resources.ApplyResources(this.synchronisationToolStripMenuItem, "synchronisationToolStripMenuItem");
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            resources.ApplyResources(this.serverToolStripMenuItem, "serverToolStripMenuItem");
            // 
            // coursesToolStripMenuItem
            // 
            this.coursesToolStripMenuItem.Name = "coursesToolStripMenuItem";
            resources.ApplyResources(this.coursesToolStripMenuItem, "coursesToolStripMenuItem");
            this.coursesToolStripMenuItem.Click += new System.EventHandler(this.CoursesToolStripMenuItem_Click);
            // 
            // autoSynchronisationToolStripMenuItem
            // 
            this.autoSynchronisationToolStripMenuItem.Name = "autoSynchronisationToolStripMenuItem";
            resources.ApplyResources(this.autoSynchronisationToolStripMenuItem, "autoSynchronisationToolStripMenuItem");
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.germanToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.EnglishToolStripMenuItem_Click);
            // 
            // germanToolStripMenuItem
            // 
            this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            resources.ApplyResources(this.germanToolStripMenuItem, "germanToolStripMenuItem");
            this.germanToolStripMenuItem.Click += new System.EventHandler(this.GermanToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.feedbackBugsToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // feedbackBugsToolStripMenuItem
            // 
            this.feedbackBugsToolStripMenuItem.Name = "feedbackBugsToolStripMenuItem";
            resources.ApplyResources(this.feedbackBugsToolStripMenuItem, "feedbackBugsToolStripMenuItem");
            this.feedbackBugsToolStripMenuItem.Click += new System.EventHandler(this.FeedbackBugsToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.InfoToolStripMenuItem_Click);
            // 
            // panelHeader
            // 
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panelHeader.Controls.Add(this.lbProgAll);
            this.panelHeader.Controls.Add(this.lbProgressAll);
            this.panelHeader.Controls.Add(this.progressBarAll);
            this.panelHeader.Controls.Add(this.lbProgActiveCourse);
            this.panelHeader.Controls.Add(this.lbProgressCourse);
            this.panelHeader.Controls.Add(this.progressBar);
            this.panelHeader.Controls.Add(this.chkListSyncOptions);
            this.panelHeader.Controls.Add(this.btnSync);
            this.panelHeader.Controls.Add(this.groupLogin);
            this.panelHeader.Name = "panelHeader";
            // 
            // lbProgAll
            // 
            resources.ApplyResources(this.lbProgAll, "lbProgAll");
            this.lbProgAll.Name = "lbProgAll";
            // 
            // lbProgressAll
            // 
            resources.ApplyResources(this.lbProgressAll, "lbProgressAll");
            this.lbProgressAll.Name = "lbProgressAll";
            // 
            // progressBarAll
            // 
            resources.ApplyResources(this.progressBarAll, "progressBarAll");
            this.progressBarAll.Name = "progressBarAll";
            this.progressBarAll.Step = 1;
            this.progressBarAll.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // lbProgActiveCourse
            // 
            resources.ApplyResources(this.lbProgActiveCourse, "lbProgActiveCourse");
            this.lbProgActiveCourse.Name = "lbProgActiveCourse";
            // 
            // lbProgressCourse
            // 
            resources.ApplyResources(this.lbProgressCourse, "lbProgressCourse");
            this.lbProgressCourse.Name = "lbProgressCourse";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // chkListSyncOptions
            // 
            this.chkListSyncOptions.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.chkListSyncOptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkListSyncOptions.CheckOnClick = true;
            this.chkListSyncOptions.FormattingEnabled = true;
            this.chkListSyncOptions.Items.AddRange(new object[] {
            resources.GetString("chkListSyncOptions.Items"),
            resources.GetString("chkListSyncOptions.Items1")});
            resources.ApplyResources(this.chkListSyncOptions, "chkListSyncOptions");
            this.chkListSyncOptions.Name = "chkListSyncOptions";
            // 
            // btnSync
            // 
            this.btnSync.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.btnSync, "btnSync");
            this.btnSync.Name = "btnSync";
            this.btnSync.UseVisualStyleBackColor = false;
            this.btnSync.Click += new System.EventHandler(this.BtnSync_Click);
            // 
            // groupLogin
            // 
            this.groupLogin.Controls.Add(this.picCheck);
            this.groupLogin.Controls.Add(this.lbLoggedIn);
            this.groupLogin.Controls.Add(this.btnLoginConfirm);
            this.groupLogin.Controls.Add(this.txtPasswInput);
            this.groupLogin.Controls.Add(this.lbPassw);
            this.groupLogin.Controls.Add(this.txtUserInput);
            this.groupLogin.Controls.Add(this.lbUser);
            resources.ApplyResources(this.groupLogin, "groupLogin");
            this.groupLogin.Name = "groupLogin";
            this.groupLogin.TabStop = false;
            // 
            // picCheck
            // 
            resources.ApplyResources(this.picCheck, "picCheck");
            this.picCheck.Name = "picCheck";
            this.picCheck.TabStop = false;
            // 
            // lbLoggedIn
            // 
            resources.ApplyResources(this.lbLoggedIn, "lbLoggedIn");
            this.lbLoggedIn.BackColor = System.Drawing.Color.Transparent;
            this.lbLoggedIn.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbLoggedIn.Name = "lbLoggedIn";
            // 
            // btnLoginConfirm
            // 
            resources.ApplyResources(this.btnLoginConfirm, "btnLoginConfirm");
            this.btnLoginConfirm.Name = "btnLoginConfirm";
            this.btnLoginConfirm.UseVisualStyleBackColor = true;
            this.btnLoginConfirm.Click += new System.EventHandler(this.BtnLoginConfirm_Click);
            // 
            // txtPasswInput
            // 
            resources.ApplyResources(this.txtPasswInput, "txtPasswInput");
            this.txtPasswInput.Name = "txtPasswInput";
            this.txtPasswInput.Enter += new System.EventHandler(this.TxtPasswInput_Enter);
            // 
            // lbPassw
            // 
            resources.ApplyResources(this.lbPassw, "lbPassw");
            this.lbPassw.Name = "lbPassw";
            // 
            // txtUserInput
            // 
            resources.ApplyResources(this.txtUserInput, "txtUserInput");
            this.txtUserInput.Name = "txtUserInput";
            // 
            // lbUser
            // 
            resources.ApplyResources(this.lbUser, "lbUser");
            this.lbUser.Name = "lbUser";
            // 
            // listViewData
            // 
            this.listViewData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStatus,
            this.colName,
            this.colPath,
            this.colDate,
            this.colSize,
            this.colId});
            resources.ApplyResources(this.listViewData, "listViewData");
            this.listViewData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewData.Name = "listViewData";
            this.listViewData.UseCompatibleStateImageBehavior = false;
            this.listViewData.View = System.Windows.Forms.View.Details;
            // 
            // colStatus
            // 
            resources.ApplyResources(this.colStatus, "colStatus");
            // 
            // colName
            // 
            resources.ApplyResources(this.colName, "colName");
            // 
            // colPath
            // 
            resources.ApplyResources(this.colPath, "colPath");
            // 
            // colDate
            // 
            resources.ApplyResources(this.colDate, "colDate");
            // 
            // colSize
            // 
            resources.ApplyResources(this.colSize, "colSize");
            // 
            // colId
            // 
            resources.ApplyResources(this.colId, "colId");
            // 
            // Form1
            // 
            this.AcceptButton = this.btnLoginConfirm;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewData);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.groupLogin.ResumeLayout(false);
            this.groupLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem courseNamingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem synchronisationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coursesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSynchronisationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem feedbackBugsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.GroupBox groupLogin;
        private System.Windows.Forms.TextBox txtPasswInput;
        private System.Windows.Forms.Label lbPassw;
        private System.Windows.Forms.TextBox txtUserInput;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem germanToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox chkListSyncOptions;
        private System.Windows.Forms.Label lbProgressCourse;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListView listViewData;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.Button btnLoginConfirm;
        private System.Windows.Forms.Label lbLoggedIn;
        private System.Windows.Forms.ToolStripMenuItem coursesLikeILIASToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem courseOwnToolStripMenuItem;
        private System.Windows.Forms.Label lbProgActiveCourse;
        private System.Windows.Forms.Label lbProgAll;
        private System.Windows.Forms.Label lbProgressAll;
        private System.Windows.Forms.ProgressBar progressBarAll;
        private System.Windows.Forms.ToolStripMenuItem folderStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderLikeILIASToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem semesterStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator konfigurierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureFoldersToolStripMenuItem1;
        private System.Windows.Forms.PictureBox picCheck;
    }
}

