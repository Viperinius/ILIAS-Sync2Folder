//==================================================================
//
//    (c) Copyright by Viperinius
//
//==================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using IliasDL.IliasSoapServiceReference;



namespace IliasDL
{
    public partial class Form1 : Form
    {
        private string sUsername;
        private string sPassword;
        private string sIliasClient;
        public bool bLoggedIn;
        private bool bLogInFail;

        private CConfig config = new CConfig();
        private CUpdate updater;
        private BackgroundWorker worker;
        private BackgroundWorker loginWorker;


        public LIASSoapWebservicePortTypeClient client = new LIASSoapWebservicePortTypeClient();
        public string sSessionId;
        private int iUserId;


        public List<string> lCourses = new List<string>();
        public List<string> lCourseNames = new List<string>();

        private int iFilePercentage = 0;
        private int iCoursesPercentage = 0;
        private int iCurrentCourseNum = 0;
        private bool bCoursesDone = false;

        private bool bSuccess;


        public Form1()
        {
            // Sets the UI culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");  //en-EN for English culture

            InitializeComponent();

            updater = new CUpdate(notifyIcon1);
            
            sUsername = "";
            sPassword = "";
            bLoggedIn = false;
            bLogInFail = false;
            sIliasClient = "";
        }

        private void EnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en");
        }

        private void GermanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("de");
        }

        private void ChangeLanguage(string lang)
        {
            CultureInfo culture = new CultureInfo(lang);
            ComponentResourceManager resources = new ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this", culture);

            foreach (Control c in this.Controls)
            {
                resources.ApplyResources(c, c.Name, culture);
            }
        }

        /// <summary>
        /// Check if capslock is active when entering the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtPasswInput_Enter(object sender, EventArgs e)
        {
            // check if capslock is active and warn user
            if (IsKeyLocked(Keys.CapsLock))
            {
                ToolTip tt = new ToolTip();
                tt.Show("Caps Lock is active!", (TextBox)sender, 0, 10, 1000);
            }
        }

        /// <summary>
        /// When login button is clicked, start login worker.
        /// If already running, cancel its task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoginConfirm_Click(object sender, EventArgs e)
        {
            if (loginWorker.IsBusy)
            {
                loginWorker.CancelAsync();
            }
            else
            {
                loginWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Starts / Stops the sync background worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSync_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
                btnSync.Text = "Starte Synchronisation";
            }
            else
            {
                if (progressBar.Value == progressBar.Maximum)
                {
                    progressBar.Value = progressBar.Minimum;
                }
                worker.RunWorkerAsync(progressBar.Value);
                btnSync.Text = "Stoppe Synchronisation";
            }


        }

        /// <summary>
        /// Open the path form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormPath pathForm = new FormPath())
            {
                if (pathForm.ShowDialog() == DialogResult.OK)
                {
                    pathForm.Close();
                }
            }
        }

        /// <summary>
        /// Open the save path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sPath = config.GetPath();
            if (Directory.Exists(sPath))
            {
                Process.Start("explorer.exe", sPath);
            }
            else
            {
                MessageBox.Show(@"Pfad wurde nicht gefunden.", @"Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Open the course selection form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormCourses courseForm = new FormCourses(this))
            {
                if (courseForm.ShowDialog() == DialogResult.OK)
                {
                    courseForm.Close();
                }
            }
        }

        /// <summary>
        /// Set course names setting to same as ILIAS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoursesLikeILIASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coursesLikeILIASToolStripMenuItem.CheckState = CheckState.Checked;
            courseOwnToolStripMenuItem.CheckState = CheckState.Unchecked;
            config.SetUseOwnNames(false);
        }

        /// <summary>
        /// Set course names setting to own
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourseOwnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bLoggedIn)
            {
                config.SetUseOwnNames(true);
                using (FormNames nameForm = new FormNames(this))
                {
                    if (nameForm.ShowDialog() == DialogResult.OK)
                    {
                        nameForm.Close();
                    }
                }
                coursesLikeILIASToolStripMenuItem.CheckState = CheckState.Unchecked;
                courseOwnToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                MessageBox.Show(@"Please log in first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Set folder structure setting to same as ILIAS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderLikeILIASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderLikeILIASToolStripMenuItem.CheckState = CheckState.Checked;
            semesterStructureToolStripMenuItem.CheckState = CheckState.Unchecked;

            config.SetUseOwnStructure(false);
        }

        /// <summary>
        /// Set folder structure setting to own
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SemesterStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            semesterStructureToolStripMenuItem.CheckState = CheckState.Checked;
            folderLikeILIASToolStripMenuItem.CheckState = CheckState.Unchecked;

            config.SetUseOwnStructure(true);
        }

        /// <summary>
        /// Open the folder template form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigureFoldersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (FormStructure folderForm = new FormStructure())
            {
                if (folderForm.ShowDialog() == DialogResult.OK)
                {
                    folderForm.Close();
                }
            }
        }

        /// <summary>
        /// Open the info form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormInfo infoForm = new FormInfo())
            {
                infoForm.ShowDialog();
            }
        }

        /// <summary>
        /// Open the feedback form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedbackBugsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormFeedback feedbackForm = new FormFeedback())
            {
                feedbackForm.ShowDialog();
            }
        }

        /// <summary>
        /// Open the settings export form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormSettingsExport exportForm = new FormSettingsExport())
            {
                if (exportForm.ShowDialog() == DialogResult.OK)
                {
                    exportForm.Close();
                }
            }
        }

        /// <summary>
        /// Open the settings import form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormSettingsImport importForm = new FormSettingsImport())
            {
                if (importForm.ShowDialog() == DialogResult.OK)
                {
                    importForm.Close();
                }
            }
        }

        /// <summary>
        /// Change if a tray icon is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrayIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trayIconToolStripMenuItem.CheckState == CheckState.Checked)
            {
                trayIconToolStripMenuItem.CheckState = CheckState.Checked;
                config.SetShowTrayIcon(true);
                notifyIcon1.Visible = true;
            }
            else
            {
                trayIconToolStripMenuItem.CheckState = CheckState.Unchecked;
                config.SetShowTrayIcon(false);
                notifyIcon1.Visible = false;
            }
        }

        /// <summary>
        /// Open the server form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormServer serverForm = new FormServer())
            {
                if (serverForm.ShowDialog() == DialogResult.OK)
                {
                    serverForm.Close();
                }
            }
        }

        /// <summary>
        /// Add the given data as a new row to the list view
        /// </summary>
        /// <param name="sStatus">Status of the file</param>
        /// <param name="sName">Name of the file</param>
        /// <param name="sPath">Path to the file</param>
        /// <param name="sDate">Creation date of the file</param>
        /// <param name="sSize">Size of the file</param>
        /// <param name="sRefId">Ref ID of the file</param>
        private void AddToListView(string sStatus, string sName, string sPath, string sDate, string sSize, string sRefId)
        {
            string[] row = { sStatus, sName, sPath, sDate, sSize, sRefId };
            if (listViewData.InvokeRequired)
            {
                ListViewItem listViewItem = new ListViewItem(row);
                listViewData.Invoke(new AddToListViewCallback(AddToListView), new object[] {
                    sStatus,
                    sName,
                    sPath,
                    sDate,
                    sSize,
                    sRefId
                });
            }
            else
            {
                ListViewItem listViewItem = new ListViewItem(row);
                listViewData.Items.Add(listViewItem);
                listViewData.Items[listViewData.Items.Count - 1].EnsureVisible();
            }

        }
        public delegate void AddToListViewCallback(string sStatus, string sName, string sPath, string sDate, string sSize, string sRefId);

        /// <summary>
        /// Remove all elements of the list view
        /// </summary>
        private void ClearListView()
        {
            if (listViewData.InvokeRequired)
            {
                listViewData.Invoke(new ClearListViewCallback(ClearListView), new object[] { });
            }
            else
            {
                listViewData.Items.Clear();
            }
        }
        public delegate void ClearListViewCallback();

        /// <summary>
        /// Change an entry in the list view
        /// </summary>
        /// <param name="iRow">Row of the item</param>
        /// <param name="sCategory">Column of the item</param>
        /// <param name="sNewVal">The new item value</param>
        private void ChangeListViewItem(int iRow, string sCategory, string sNewVal)
        {
            int iColumn = 0;
            switch (sCategory)
            {
                case "Status":
                    iColumn = 0;
                    break;
                case "Dateiname":
                    iColumn = 1;
                    break;
                case "Pfad":
                    iColumn = 2;
                    break;
                case "Änderungsdatum":
                    iColumn = 3;
                    break;
                case "Größe":
                    iColumn = 4;
                    break;
                case "Ref ID":
                    iColumn = 5;
                    break;
                default:
                    Console.WriteLine("====ERROR WHEN CHANGING LIST VIEW ITEM===");
                    break;
            }

            if (listViewData.InvokeRequired)
            {
                listViewData.Invoke(new ChangeListViewItemCallback(ChangeListViewItem), new object[]
                {
                    iRow,
                    sCategory,
                    sNewVal
                });
            }
            else
            {
                ListViewItem item = listViewData.Items[iRow];
                item.SubItems[iColumn].Text = sNewVal;
                if (sNewVal == "Pfad zu lang!")
                {
                    item.BackColor = Color.Red;
                }
            }
        }
        public delegate void ChangeListViewItemCallback(int iRow, string sCategory, string sNewVal);

        /// <summary>
        /// Build the path to the files and create non existing directories
        /// </summary>
        /// <param name="sPath">Path to create</param>
        /// <param name="sCourseId">Course Id to retrieve own names, if activated</param>
        private void CreateDirectories(ref string sPath, string sCourseId)
        {
            string sTempPath = "";
            string sConfigPath = config.GetPath();
            
            string[] sTempNames = sPath.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            string sCourseName = sTempNames[0];

            if (config.GetUseOwnStructures() == "true")
            {
                string sStructTemplate = config.GetStructureTemplate();
                
                if (config.GetUseOwnNames() == "true")
                {      
                    string sOwnName = config.GetCourseName(sCourseId);
                    sTempPath = sPath.Replace(sCourseName, sOwnName);

                    if (sStructTemplate != "__NO_VAL__" && sStructTemplate != "")
                    {
                        sStructTemplate = ReplaceTemplatePlaceholder(sCourseName, sStructTemplate);
                        sPath = Path.Combine(sStructTemplate, sTempPath);
                    }
                }
                else if (config.GetUseOwnNames() == "false")
                {
                    if (sStructTemplate != "__NO_VAL__" && sStructTemplate != "")
                    {
                        sStructTemplate = ReplaceTemplatePlaceholder(sCourseName, sStructTemplate);
                        sTempPath = sPath;
                        sPath = Path.Combine(sStructTemplate, sTempPath);
                    }
                }
            }
            else if (config.GetUseOwnStructures() == "false")
            {
                if (config.GetUseOwnNames() == "true")
                {
                    string sOwnName = config.GetCourseName(sCourseId);
                    sPath = sPath.Replace(sCourseName, sOwnName);
                }
            }

            sPath = Path.Combine(sConfigPath, sPath);

            if (!Directory.Exists(sPath))
            {
                if (sConfigPath == "")
                {
                    worker.CancelAsync();
                    MessageBox.Show("Es ist kein Speicherpfad angegeben.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Directory.CreateDirectory(sPath);
            }
            //Console.WriteLine("inner: " + sPath);
        }

        /// <summary>
        /// Build the path to the files and (optionally) create non existing directories
        /// </summary>
        /// <param name="sPath">Path to create</param>
        /// <param name="sCourseId">Course Id to retrieve own names, if activated</param>
        /// <param name="bBuildOnlyPath">If false, the directories will be created</param>
        private void CreateDirectories(ref string sPath, string sCourseId, bool bBuildOnlyPath)
        {
            string sTempPath = "";
            string sConfigPath = config.GetPath();

            string[] sTempNames = sPath.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            string sCourseName = sTempNames[0];

            if (config.GetUseOwnStructures() == "true")
            {
                string sStructTemplate = config.GetStructureTemplate();

                if (config.GetUseOwnNames() == "true")
                {
                    string sOwnName = config.GetCourseName(sCourseId);
                    sTempPath = sPath.Replace(sCourseName, sOwnName);

                    if (sStructTemplate != "__NO_VAL__" && sStructTemplate != "")
                    {
                        sStructTemplate = ReplaceTemplatePlaceholder(sCourseName, sStructTemplate);
                        sPath = Path.Combine(sStructTemplate, sTempPath);
                    }
                }
                else if (config.GetUseOwnNames() == "false")
                {
                    if (sStructTemplate != "__NO_VAL__" && sStructTemplate != "")
                    {
                        sStructTemplate = ReplaceTemplatePlaceholder(sCourseName, sStructTemplate);
                        sTempPath = sPath;
                        sPath = Path.Combine(sStructTemplate, sTempPath);
                    }
                }
            }
            else if (config.GetUseOwnStructures() == "false")
            {
                if (config.GetUseOwnNames() == "true")
                {
                    string sOwnName = config.GetCourseName(sCourseId);
                    sPath = sPath.Replace(sCourseName, sOwnName);
                }
            }

            sPath = Path.Combine(sConfigPath, sPath);

            if (!bBuildOnlyPath)
            {
                if (!Directory.Exists(sPath))
                {
                    if (sConfigPath == "")
                    {
                        worker.CancelAsync();
                        MessageBox.Show("Es ist kein Speicherpfad angegeben.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Directory.CreateDirectory(sPath);
                }
            }
            //Console.WriteLine("inner: " + sPath);
        }

        /// <summary>
        /// Get the semester number from a course name
        /// </summary>
        /// <param name="sCourseName">Raw course name</param>
        /// <returns>semester number or 0 in case of error</returns>
        private int GetSemesterNum(string sCourseName)
        {
            //ELM-4.4-Embe.....
            string[] sTmpArray = sCourseName.Split('-');
            //4.4
            try
            {
                string[] sTmpArray2 = sTmpArray[1].Split('.');
                //4 (the first one)
                return Int32.Parse(sTmpArray2[0]);
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }            
        }

        /// <summary>
        /// Get the semester year of the course
        /// </summary>
        /// <param name="sCourseName">Raw Course Name</param>
        /// <returns>course year or 0 in case of errors</returns>
        private int GetCourseYear(string sCourseName)
        {
            //..., Wetter, SS2018
            string[] sTmpArray = sCourseName.Split(',');
            //SS2018 (or WS2017-18)
            try
            {
                if (sTmpArray[3].Contains("SS"))
                {
                    string[] sTmpArray2 = sTmpArray[3].Split('S');
                    //2018
                    return Int32.Parse(sTmpArray2[2]);
                }
                else
                {
                    string[] sTmpArray2 = sTmpArray[3].Split('S');
                    //2017-18
                    return Int32.Parse(sTmpArray2[1]);
                }
                
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Replace the folder template with semester num and year
        /// </summary>
        /// <param name="sCourseName">Raw course name</param>
        /// <param name="sStructTemplate">Folder template</param>
        /// <returns>Filled in template</returns>
        private string ReplaceTemplatePlaceholder(string sCourseName, string sStructTemplate)
        {
            int iSemesterNum = GetSemesterNum(sCourseName);
            bool bUseYear = false;
            if (config.GetUseYearInStructure() == "true")
            {
                bUseYear = true;
            }
            int iCourseYear = GetCourseYear(sCourseName);

            if (iSemesterNum == 0)
            {
                return "Allgemein";
            }

            string sTmp = sStructTemplate.Replace("%", iSemesterNum.ToString());
            if (bUseYear)
            {
                if (iCourseYear == 0)
                {
                    return sTmp.Replace("$", "");
                }
                else
                {
                    return sTmp.Replace("$", iCourseYear.ToString());
                }
            }
            else
            {
                return sTmp.Replace("$", "");
            }
        }

        //------------------------------------
        //          ILIAS functions
        //------------------------------------    

        /// <summary>
        /// Get the course information of the user
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="sSessionId">Session ID</param>
        public void GetCourses(ref LIASSoapWebservicePortTypeClient client, string sSessionId)
        {
            List<string> lRoleTitles = new List<string>();
            string xmlUserRoles = client.getUserRoles(sSessionId, iUserId);
            string[] tmp = new string[4];

            //convert string to editable xml
            XDocument xDoc = XDocument.Parse(xmlUserRoles);
           
            //scan for "Title" tags (contain the roles with the course ids)
            foreach (XNode node in xDoc.DescendantNodes())
            {
                if (node is XElement)
                {
                    XElement element = (XElement)node;
                    if (element.Name.LocalName.Equals("Title"))
                    {
                        lRoleTitles.Add(element.Value);
                    }
                }
            }

            foreach (string role in lRoleTitles)
            {
                //filter for courses
                if (role.StartsWith("il_crs_member") || role.StartsWith("il_crs_tutor"))
                {
                    tmp = role.Split('_');
                    if (tmp[3] != "39643")
                    {
                        //if course is not "FSR" course
                        lCourses.Add(tmp[3]);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Get the name of the given course
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="sSessionId">Session ID</param>
        /// <param name="sRef">Reference ID of the course</param>
        /// <returns>Course name</returns>
        public string GetCourseName(ref LIASSoapWebservicePortTypeClient client, string sSessionId, string sRef)
        {
            string xmlCourse = client.getObjectByReference(sSessionId, Int32.Parse(sRef) , iUserId);

            string temp = "";
            XDocument xDoc = XDocument.Parse(xmlCourse);
            foreach (XNode node in xDoc.DescendantNodes())
            {
                if (node is XElement)
                {
                    XElement element = (XElement)node;
                    if (element.Name.LocalName.Equals("Object"))
                    {
                        foreach (XElement subElement in element.Descendants("Title"))
                        {
                            temp = subElement.Value;
                        }
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Get names of all users' courses
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="sSessionId">Session ID</param>
        public void GetCourseNames(ref LIASSoapWebservicePortTypeClient client, string sSessionId)
        {
            foreach (string sRef in lCourses)
            {
                lCourseNames.Add(GetCourseName(ref client, sSessionId, sRef));
            }
        }

        /// <summary>
        /// Get the file information, create directories, add to list view and download files
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="iCourseId">Course ID to get the files from</param>
        /// <param name="sSessionId">Session ID</param>
        private void GetCourseFiles(ref LIASSoapWebservicePortTypeClient client, int iCourseId, string sSessionId)
        {
            List<string> lRefs = new List<string>();
            List<string> lTypes = new List<string>();
            List<string> lExtensions = new List<string>();
            List<string> lDates = new List<string>();
            List<string> lTitles = new List<string>();
            List<string> lSizes = new List<string>();
            List<string> lPaths = new List<string>();

            string tmpPathCrs = "";
            string tmpPathDownwards = "";
            string tmpPath = "";

            string[] temp = new string[] { "" };
            string xmlFiles = client.getXMLTreeAsync(sSessionId, iCourseId, temp, iUserId).Result;
            //Console.WriteLine("Course Files: " + xmlFiles);

            //--------------------------------------
            iFilePercentage = 0;
            worker.ReportProgress(iFilePercentage);
            //check again if cancelling needed
            if (worker.CancellationPending)
            {
                return;
            }
            //--------------------------------------

            XDocument xDoc = XDocument.Parse(xmlFiles);

            foreach (XNode node in xDoc.DescendantNodes())
            {
                if (node is XElement)
                {
                    XElement element = (XElement)node;
                    if (element.Name.LocalName.Equals("Object"))
                    {
                        //check if object is file
                        if ((string)element.Attribute("type") == "file")
                        {
                            //set type to file
                            lTypes.Add((string)element.Attribute("type"));
                            
                            //iterate over object to find ref & path
                            foreach (XElement subElement in element.Descendants("References"))
                            {
                                //add ref id
                                lRefs.Add((string)subElement.Attribute("ref_id"));
                                //find path
                                foreach (XElement subSubElement in subElement.Descendants("Path"))
                                {
                                    //iterate over elements to get the path structure
                                    foreach (XElement subSubSubElement in subSubElement.Descendants("Element"))
                                    {
                                        string type = (string)subSubSubElement.Attribute("type");
                                        if (type == "crs")
                                        {
                                            tmpPathCrs = subSubSubElement.Value;
                                        }
                                        else if (type == "fold")
                                        {
                                            tmpPathDownwards = subSubSubElement.Value;
                                            tmpPath = Path.Combine(tmpPath, tmpPathDownwards);
                                        }                                        
                                    }
                                    lPaths.Add(Path.Combine(tmpPathCrs, tmpPath));
                                }
                            }
                            //find file extension & size
                            foreach (XElement subElement in element.Descendants("Properties"))
                            {
                                foreach (XElement subSubElement in subElement.Descendants("Property"))
                                {
                                    if ((string)subSubElement.Attribute("name") == "fileExtension")
                                    {
                                        lExtensions.Add(subSubElement.Value);
                                    }
                                    else if ((string)subSubElement.Attribute("name") == "fileSize")
                                    {
                                        lSizes.Add(subSubElement.Value);
                                    }
                                }
                            }
                            //find last modified/updated date
                            foreach (XElement subElement in element.Descendants("LastUpdate"))
                            {
                                lDates.Add(subElement.Value);
                            }
                            //find title
                            foreach (XElement subElement in element.Descendants("Title"))
                            {
                                lTitles.Add(subElement.Value);
                            }

                        }
                        tmpPathDownwards = "";
                        tmpPath = "";
                    }
                }
            }

            //##############
            //Console.WriteLine("XML Tree sorting done.");
            //##############

            //--------------------------------------
            worker.ReportProgress(iFilePercentage);
            //check again if cancelling needed
            if (worker.CancellationPending)
            {
                return;
            }
            //--------------------------------------

            int iPathCount = lPaths.Count();
            int iCourseCount = lCourses.Count();

            //calculate percentage for progressbar
            iFilePercentage = GetPercentage(0, iPathCount);
            iCoursesPercentage = GetPercentage(iCurrentCourseNum, iCourseCount);

            string sPath = "";
            string sRefId = "";
            string sTitle = "";
            string sSize = "";
            string sDate = "";
            int iSize = 0;
            string sStatus = "Nicht vorhanden";

            for (int i = 0; i < iPathCount; i++)
            {
                sStatus = "Nicht vorhanden";

                sPath = lPaths[i];
                sRefId = lRefs[i];
                sTitle = lTitles[i];
                sSize = lSizes[i];
                sDate = lDates[i];

                //--------------------------------------
                worker.ReportProgress(iFilePercentage);
                //check again if cancelling needed
                if (worker.CancellationPending)
                {
                    return;
                }
                //--------------------------------------

                //create path directories
                if (!chkListSyncOptions.GetItemChecked(0))
                {
                    //CreateDirectories(ref sPath, iCourseId.ToString());
                    CreateDirectories(ref sPath, iCourseId.ToString(), false);
                }
                else
                {
                    CreateDirectories(ref sPath, iCourseId.ToString(), true);
                }

                //##############
                //Console.WriteLine("Creating directories done.");
                //##############

                //check file status
                if (File.Exists(Path.Combine(sPath, sTitle)))
                {
                    sStatus = "Vorhanden";
                }

                //format size to be human readable
                iSize = Int32.Parse(sSize);
                if (iSize < 1049000)   //if smaller than 1 MB
                {
                    sSize = GetSizeInKiB(iSize).ToString() + " KB";
                }
                else
                {
                    sSize = GetSizeInMiB(iSize).ToString() + " MB";
                }

                //##############
                //Console.WriteLine("Size formatting done.");
                //##############                

                //--------------------------------------
                worker.ReportProgress(iFilePercentage);
                //check again if cancelling needed
                if (worker.CancellationPending)
                {
                    return;
                }
                //--------------------------------------

                bool bNewFile = false;
                //download Files
                if (!chkListSyncOptions.GetItemChecked(0))
                {
                    if (!File.Exists(Path.Combine(sPath, sTitle)))
                    {
                        sStatus = "Lädt...";
                        bNewFile = true;
                        AddToListView(sStatus, sTitle, sPath, sDate, sSize, sRefId);
                    }
                    //Console.WriteLine("now dling: " + sTitle);
                    //GetFileByRef(ref client, Int32.Parse(sRefId), Path.Combine(config.GetPath(), sPath), sTitle, sSessionId, ref sStatus);
                    GetFileByRefGZIP(ref client, Int32.Parse(sRefId), sPath, sTitle, sSessionId, ref sStatus);
                }

                //add to list view
                if (bNewFile)
                {
                    ChangeListViewItem(listViewData.Items.Count - 1, "Status", sStatus);
                }
                else if (chkListSyncOptions.GetItemChecked(1) && (sStatus == "Nicht vorhanden" || sStatus == "Neu"))
                {
                    AddToListView(sStatus, sTitle, sPath, sDate, sSize, sRefId);
                }
                else if (!chkListSyncOptions.GetItemChecked(1))
                {
                    AddToListView(sStatus, sTitle, sPath, sDate, sSize, sRefId);
                }

                //calculate percentage for progressbar
                iFilePercentage = GetPercentage(i+1, iPathCount);
                worker.ReportProgress(iFilePercentage);
            }

            //implement return val?
        }

        /// <summary>
        /// Download file as uncompressed base64 encoded string
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="iRefId">Ref ID of the file</param>
        /// <param name="sPath">Path to download to</param>
        /// <param name="sName">Name of the file</param>
        /// <param name="sSessionId">Session ID</param>
        /// <param name="sFileStatus">File status</param>
        private void GetFileByRef(ref LIASSoapWebservicePortTypeClient client, int iRefId, string sPath, string sName, string sSessionId, ref string sFileStatus)
        {
            string sFullPath = "";

            //build full path
            sFullPath = Path.Combine(sPath, sName);
            //Console.WriteLine("Path: " + sFullPath);

            if (!File.Exists(sFullPath))
            {
                //##############
                //Console.WriteLine("Requesting file " + sName + " ...");
                //##############

                //request file xml from SOAP
                //string xmlFile = client.getFileXML(sSessionId, iRefId, 1);
                string xmlFile = client.getFileXMLAsync(sSessionId, iRefId, 1).Result;
                //Console.WriteLine("XML FILE: " + xmlFile);

                //scan xml for <Content>, i.e. the file base64 string
                string sContent = "";
                string sFilename = "";

                XDocument xDoc = XDocument.Parse(xmlFile);
                foreach (XNode node in xDoc.DescendantNodes())
                {
                    if (node is XElement)
                    {
                        XElement element = (XElement)node;
                        if (element.Name.LocalName.Equals("Content"))
                        {
                            sContent = element.Value;
                        }
                        else if (element.Name.LocalName.Equals("Filename"))
                        {
                            sFilename = element.Value;
                        }
                    }
                }

                //##############
                //Console.WriteLine("Searching for content in file done.");
                //##############

                //generate file from base64 string
                File.WriteAllBytes(sFullPath, Convert.FromBase64String(sContent));

                sFileStatus = "Neu";

                //##############
                //Console.WriteLine("Writing file to disk done.");
                //##############
            }
            else
            {
                sFileStatus = "Vorhanden";
            }
        }

        /// <summary>
        /// Download file as compressed GZIP string
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="iRefId">Ref ID of the file</param>
        /// <param name="sPath">Path to download to</param>
        /// <param name="sName">Name of the file</param>
        /// <param name="sSessionId">Session ID</param>
        /// <param name="sFileStatus">File status</param>
        private void GetFileByRefGZIP(ref LIASSoapWebservicePortTypeClient client, int iRefId, string sPath, string sName, string sSessionId, ref string sFileStatus)
        {
            string sFullPath = "";

            //build full path
            sFullPath = Path.Combine(sPath, sName);
            //Console.WriteLine("Path: " + sFullPath);

            if (!File.Exists(sFullPath))
            {
                //##############
                //Console.WriteLine("Requesting file " + sName + " ...");
                //##############

                //request file xml from SOAP
                string xmlFile = client.getFileXML(sSessionId, iRefId, 3);
                //string xmlFile = client.getFileXMLAsync(sSessionId, iRefId, 3).Result;
                //Console.WriteLine("XML FILE GZIP: " + xmlFile);

                //scan xml for <Content>, i.e. the file base64 string
                string sContent = "";
                string sFilename = "";

                XDocument xDoc = XDocument.Parse(xmlFile);
                foreach (XNode node in xDoc.DescendantNodes())
                {
                    if (node is XElement)
                    {
                        XElement element = (XElement)node;
                        if (element.Name.LocalName.Equals("Content"))
                        {
                            sContent = element.Value;
                        }
                        else if (element.Name.LocalName.Equals("Filename"))
                        {
                            sFilename = element.Value;
                        }
                    }
                }

                //##############
                //Console.WriteLine("Searching for content in file done.");
                //##############

                //convert base64 string
                byte[] byB64 = Convert.FromBase64String(sContent);

                //decompress gzip
                byte[] byDecompressed;
                using (var compressedStream = new MemoryStream(byB64))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    int iRead = 0;

                    while ((iRead = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        resultStream.Write(buffer, 0, iRead);
                    }
                    byDecompressed = resultStream.ToArray();
                }

                //generate file from base64 string
                try
                {
                    File.WriteAllBytes(sFullPath, byDecompressed);
                    sFileStatus = "Neu";
                }
                catch (PathTooLongException)
                {
                    sFileStatus = "Pfad zu lang!";
                }

                //##############
                //Console.WriteLine("Writing file to disk done.");
                //##############
            }
            else
            {
                sFileStatus = "Vorhanden";
            }
        }

        /// <summary>
        /// Log out from ILIAS SOAP
        /// </summary>
        /// <param name="client">ILIAS SOAP webservice client as reference</param>
        /// <param name="sSessionId">Session ID</param>
        /// <returns>true if logout successful</returns>
        private bool Logout(ref LIASSoapWebservicePortTypeClient client, string sSessionId)
        {
            return client.logout(sSessionId);
        }             

        /// <summary>
        /// Get the percentage of the given values
        /// </summary>
        /// <param name="iCurrent">Current value</param>
        /// <param name="iMax">Maximum value</param>
        /// <returns>Percentage</returns>
        private int GetPercentage(int iCurrent, int iMax)
        {
            return (int)(((double)iCurrent / iMax)*100);
        }

        /// <summary>
        /// Get kibibyte size from byte
        /// </summary>
        /// <param name="iSizeInByte">Size in byte</param>
        /// <returns>Size in KiB</returns>
        private int GetSizeInKiB(int iSizeInByte)
        {
            return (int)((double)iSizeInByte / 1024.0);
        }

        /// <summary>
        /// Get mebibyte size from byte
        /// </summary>
        /// <param name="iSizeInByte">Size in byte</param>
        /// <returns>Size in MiB</returns>
        private int GetSizeInMiB(int iSizeInByte)
        {
            double dTemp = ((double)iSizeInByte / 1024.0);
            return (int)(dTemp / 1024.0);
        }

        //------------------------------------
        //          update handling
        //------------------------------------

        /// <summary>
        /// Update notification clicked, call updater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Es wurde ein Update gefunden. Möchten Sie es herunterladen und installieren?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                //call update programme
                bool bResult = updater.GetUpdate();
                if (!bResult)
                {
                    MessageBox.Show("Beim Update ist leider etwas schiefgelaufen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        //------------------------------------
        //          background workers
        //------------------------------------

        /// <summary>
        /// Adjust window settings when loading the form, prepare the background workers, check for update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            txtUserInput.Text = config.GetUser();

            if (config.GetUseOwnStructures() == "true")
            {
                semesterStructureToolStripMenuItem.CheckState = CheckState.Checked;
                folderLikeILIASToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else
            {
                semesterStructureToolStripMenuItem.CheckState = CheckState.Unchecked;
                folderLikeILIASToolStripMenuItem.CheckState = CheckState.Checked;
            }

            if (config.GetUseOwnNames() == "true")
            {
                coursesLikeILIASToolStripMenuItem.CheckState = CheckState.Unchecked;
                courseOwnToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                coursesLikeILIASToolStripMenuItem.CheckState = CheckState.Checked;
                courseOwnToolStripMenuItem.CheckState = CheckState.Unchecked;
            }

            if (config.GetShowTrayIcon() == "true")
            {
                trayIconToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                trayIconToolStripMenuItem.CheckState = CheckState.Unchecked;
            }

            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanges);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);

            loginWorker = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = true
            };

            loginWorker.DoWork += new DoWorkEventHandler(LoginWorker_DoWork);
            loginWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoginWorker_RunWorkerCompleted);


            //-----------------------------
            //      check for update
            //-----------------------------

            if (updater.CheckForUpdate())
            {
                //notify user
                updater.DisplayNotification("Update verfügbar", "Es ist ein Update für ILIAS Sync2Folder verfügbar!");
            }

        }

        /// <summary>
        /// Sync background worker, manages the synchronisation process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //clear old list view data
            ClearListView();
            //reset progress
            iFilePercentage = 0;
            iCoursesPercentage = 0;
            iCurrentCourseNum = 0;

            //get the courses of the user
            lCourses.Clear();
            GetCourses(ref client, sSessionId);

            if (!worker.CancellationPending && !bCoursesDone)
            {
                worker.ReportProgress(iFilePercentage);

                iCurrentCourseNum = 1;
                if (config.GetSyncAll() == "true")
                {
                    foreach (string sRef in lCourses)
                    {
                        GetCourseFiles(ref client, Int32.Parse(sRef), sSessionId);
                        iCurrentCourseNum++;
                    }
                }
                else if (config.GetSyncAll() == "false")
                {
                    foreach (string sRef in lCourses)
                    {
                        if (config.GetCourse(sRef) == sRef)
                        {
                            GetCourseFiles(ref client, Int32.Parse(sRef), sSessionId);
                            iCurrentCourseNum++;
                        }
                    }
                }

                bCoursesDone = true;
                iCoursesPercentage = 100;

                worker.ReportProgress(100);
            }
            
            int[] iResults = new int[] { iFilePercentage, iCoursesPercentage };
            e.Result = iResults;
        }

        /// <summary>
        /// Update the sync progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lbProgressCourse.Text = e.ProgressPercentage.ToString() + " %";

            progressBarAll.Value = iCoursesPercentage;
            lbProgressAll.Text = iCoursesPercentage.ToString() + " %";
        }

        /// <summary>
        /// Sync is done, reset some values 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSync.Text = "Starte Synchronisation";
            bCoursesDone = false;
        }

        /// <summary>
        /// login background worker to manage the login to ILIAS SOAP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (bLoggedIn)
            {
                bSuccess = Logout(ref client, sSessionId);
                //Console.WriteLine("Logging out.");
            }
            else
            {
                if (txtUserInput.Text != "" && txtPasswInput.Text != "")
                {
                    bLogInFail = false;
                    //Console.WriteLine("Logging in.");

                    sUsername = txtUserInput.Text;
                    sPassword = txtPasswInput.Text;
                    config.SetUser(sUsername);

                    //connect to ILIAS SOAP                    
                    try
                    {
                        //get client id
                        sIliasClient = config.GetClient();

                        //get session id / log in
                        sSessionId = client.loginLDAP(sIliasClient, sUsername, sPassword);
                        
                        //get user id
                        iUserId = client.getUserIdBySid(sSessionId);
                    }
                    catch (Exception)
                    {
                        bLogInFail = true;
                        MessageBox.Show("Login failed. Wrong credentials?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                }
                else
                {
                    MessageBox.Show(@"Username or Password not given!");
                    bLogInFail = true;
                }
            }
        }

        /// <summary>
        /// Login is done, check if failed and adjust window settings accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (bLoggedIn && !bLogInFail)
            {
                lbLoggedIn.Hide();
                picCheck.Hide();
                lbUser.Enabled = true;
                lbPassw.Enabled = true;
                txtUserInput.Enabled = true;
                txtPasswInput.Enabled = true;
                btnLoginConfirm.Text = "Einloggen";
                bLoggedIn = false;

                btnSync.Enabled = false;
            }
            else if (!bLogInFail)
            {
                lbUser.Enabled = false;
                lbPassw.Enabled = false;
                txtUserInput.Enabled = false;
                txtPasswInput.Enabled = false;
                btnLoginConfirm.Text = "Ausloggen";

                lbLoggedIn.Show();
                picCheck.Show();
                bLoggedIn = true;

                btnSync.Enabled = true;
            }
        }

        
    }
}
