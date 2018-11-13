using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Windows;
using WPF_ILIAS_Sync2Folder.IliasSoapWebservice;
using IliasDL;
using System.Globalization;

namespace WPF_ILIAS_Sync2Folder
{ 
    public class CIliasHandling
    {
        private LIASSoapWebservicePortTypeClient client = new LIASSoapWebservicePortTypeClient();
        private string sSessionId;
        private int iUserId;

        private CConfig config = new CConfig();
        private CSimpleCalculations cSimple = new CSimpleCalculations();

        public ObservableCollection<FileInfo> lFiles = new ObservableCollection<FileInfo>();
        public ObservableCollection<CourseInfo> lCourseInfos = new ObservableCollection<CourseInfo>();
        public List<FileInfo> listFiles = new List<FileInfo>();
        public List<CourseInfo> listCourseInfos = new List<CourseInfo>();

        public int iFilePercentage = 0;
        public int iCoursePercentage = 0;
        public int iCurrentCourseNum = 0;
        public bool bLoggedIn;
        public bool bSyncRunning;
        public bool bCoursesDone;

        private MainWindow window;
        private ChangedPropertyNotifier changedPropertyNotifier;

        public CIliasHandling(MainWindow mainWindow, ChangedPropertyNotifier changedPropNotifier)
        {
            window = mainWindow;
            changedPropertyNotifier = changedPropNotifier;
        }

        /// <summary>
        /// Log in to ILIAS SOAP, returns false if not successful
        /// </summary>
        /// <param name="sUser">Username</param>
        /// <param name="sPassword">Password</param>
        /// <returns></returns>
        public bool IliasLogin(string sUser, string sPassword)
        {
            if (bLoggedIn)
            {
                return true;
            }
            else
            {
                if (sUser != "" && sPassword != "")
                {
                    config.SetUser(sUser);

                    //connect to ILIAS SOAP
                    try
                    {
                        //get session id / log in
                        sSessionId = client.loginLDAP(config.GetClient(), sUser, sPassword);
                        iUserId = client.getUserIdBySid(sSessionId);
                        bLoggedIn = true;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);

                        if (e.Message == "err_wrong_login")
                        {
                            //maybe implement own message (wrong credentials) to display in message
                        }

                        bLoggedIn = false;
                        return false;
                    }
                }
                else
                {
                    bLoggedIn = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Log out from ILIAS SOAP
        /// </summary>
        /// <returns></returns>
        public bool IliasLogout()
        {
            if (bLoggedIn)
            {
                bLoggedIn = false;
                return client.logout(sSessionId);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the course information of the user
        /// </summary>
        public void GetCourses()
        {
            listCourseInfos.Clear();

            if (!bLoggedIn)
            {
                return;
            }

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
                        //if course is not "FSR" course (only accurate for FH Bielefeld, might need some other workaround)
                        listCourseInfos.Add(new CourseInfo() { CourseId = tmp[3] });
                    }

                }
            }
        }

        /// <summary>
        /// Get the name of the given course
        /// </summary>
        /// <param name="sRef"></param>
        /// <returns></returns>
        public string GetCourseName(string sRef)
        {
            string xmlCourse = client.getObjectByReference(sSessionId, Int32.Parse(sRef), iUserId);

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
        public void GetCourseNames()
        {
            string sRef = "";
            foreach (CourseInfo course in listCourseInfos)
            {
                sRef = course.CourseId;
                course.CourseName = GetCourseName(sRef);
            }
        }

        /// <summary>
        /// Get the file information, create directories, add to list view and download files
        /// </summary>
        /// <param name="iCourseId">Current course ID</param>
        public void GetCourseFiles(int iCourseId)
        {
            listFiles.Clear();

            string tmpPathCrs = "";
            string tmpPathDownwards = "";
            string tmpPath = "";
            string[] temp = new string[] { "" };
            string sXmlFiles = client.getXMLTreeAsync(sSessionId, iCourseId, temp, iUserId).Result;

            
            window.WorkerSync_ChangeProgress(iFilePercentage);
            if (window.WorkerSync_IsCancelPending())
            {
                return;
            }

            XDocument xDoc = XDocument.Parse(sXmlFiles);

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
                            FileInfo currentFile;

                            //iterate over object to find ref & path
                            foreach (XElement subElement in element.Descendants("References"))
                            {
                                //add ref id
                                listFiles.Add(new FileInfo() { FileId = (string)subElement.Attribute("ref_id") });
                                //note current file for later use (filling the remaining info)
                                currentFile = listFiles.Last();
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

                                    currentFile.FilePath = Path.Combine(tmpPathCrs, tmpPath);
                                }
                            }

                            currentFile = listFiles.Last();

                            //find file size & version
                            foreach (XElement subElement in element.Descendants("Properties"))
                            {
                                foreach (XElement subSubElement in subElement.Descendants("Property"))
                                {
                                    if ((string)subSubElement.Attribute("name") == "fileSize")
                                    {
                                        currentFile.FileSize = subSubElement.Value;
                                    }
                                    if ((string)subSubElement.Attribute("name") == "fileVersion")
                                    {
                                        currentFile.FileVersion = subSubElement.Value;
                                    }
                                }
                            }

                            //find creation date
                            foreach (XElement subElement in element.Descendants("CreateDate"))
                            {
                                currentFile.FileDate = subElement.Value;
                            }
                            //find last modified/updated date
                            foreach (XElement subElement in element.Descendants("LastUpdate"))
                            {
                                currentFile.FileLastUpdate = subElement.Value;
                            }
                            //find title
                            foreach (XElement subElement in element.Descendants("Title"))
                            {
                                currentFile.FileName = subElement.Value;
                            }
                        }
                        tmpPathDownwards = "";
                        tmpPath = "";
                    }
                }
            }

            if (window.WorkerSync_IsCancelPending())
            {
                return;
            }

            //calculate percentages for progress bars
            int iFileCount = listFiles.Count;
            int iCourseCount = 0;
            if (config.GetSyncAll() == "true")
            {
                iCourseCount = listCourseInfos.Count;
            }
            else
            {
                foreach (CourseInfo course in listCourseInfos)
                {
                    if (config.GetCourse(course.CourseId) == course.CourseId)
                    {
                        iCourseCount++;
                    }
                }
            }

            if (iFileCount > 0)
            {
                iFilePercentage = cSimple.GetPercentage(0, iFileCount);
            }
            else
            {
                iFilePercentage = 0;
            }
            iCoursePercentage = cSimple.GetPercentage(iCurrentCourseNum, iCourseCount);

            window.WorkerSync_ChangeProgress(iFilePercentage, iCoursePercentage);
            DownloadFiles(iCourseId, iFileCount);

            

        }

        /// <summary>
        /// Show each file on listview and download them (if setting is set)
        /// </summary>
        /// <param name="iCourseId">Current course ID</param>
        /// <param name="iFileCount">Count of all files in the current course</param>
        private void DownloadFiles(int iCourseId, int iFileCount)
        {
            if (window.WorkerSync_IsCancelPending())
            {
                return;
            }                       

            int iCounter = 0;
            foreach (FileInfo file in listFiles)
            {
                window.WorkerSync_ChangeProgress(iFilePercentage);
                if (window.WorkerSync_IsCancelPending())
                {
                    return;
                }

                string sStatus = "Not present";

                file.FileStatus = sStatus;
                file.FileIsVisible = false;

                //create path directories
                string sPath = file.FilePath;
                if (config.GetShowOnly() == "false")
                {
                    //files should be downloaded, create directories
                    CreateDirectories(ref sPath, iCourseId.ToString(), false);
                }
                else
                {
                    //no download, only build the full path
                    CreateDirectories(ref sPath, iCourseId.ToString(), true);
                }
                file.FilePath = sPath;


                //check file status
                if (File.Exists(Path.Combine(file.FilePath, file.FileName)))
                {
                    sStatus = "Found on disk";
                    file.FileStatus = sStatus;

                    //check if file has been updated                    
                    if (int.Parse(file.FileVersion) > 1)
                    {
                        DateTime localLastModified = File.GetLastWriteTime(Path.Combine(file.FilePath, file.FileName));
                        DateTime fileLastModifyDate = DateTime.ParseExact(file.FileLastUpdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        if (fileLastModifyDate > localLastModified)  //wrong for testing
                        {
                            //ilias version is more current
                            if (config.GetFileIgnore(file.FileId) == file.FileId || config.GetOverwriteNone() == "true")
                            {
                                sStatus = "Update available";
                            }
                            else
                            {
                                sStatus = "Update available!";
                            }
                            file.FileStatus = sStatus;
                        }
                    }
                }

                //check file ignore rule
                if (config.GetFileIgnore(file.FileId) == file.FileId || config.GetOverwriteNone() == "true")
                {
                    file.FileIgnore = "Ignored";
                }
                else
                {
                    file.FileIgnore = "Not ignored";
                }

                //format size to be human readable
                int iSize = int.Parse(file.FileSize);
                if (iSize < 1049000)
                {
                    //if smaller than 1 MB
                    file.FileSize = cSimple.GetSizeInKiB(iSize).ToString() + " KB";
                }
                else
                {
                    file.FileSize = cSimple.GetSizeInMiB(iSize).ToString() + " MB";
                }

                window.WorkerSync_ChangeProgress(iFilePercentage);
                if (window.WorkerSync_IsCancelPending())
                {
                    return;
                }

                
                
                bool bNewFile = false;
                if (config.GetShowOnly() == "false")
                {
                    //download file
                    if (!File.Exists(Path.Combine(file.FilePath, file.FileName)))
                    {
                        sStatus = "Loading...";
                        bNewFile = true;
                        file.FileStatus = sStatus;
                        file.FileIsVisible = true;
                        //report progress with fake percentage to add the current file to listview
                        window.WorkerSync_ChangeProgress(500, listFiles.IndexOf(file).ToString());
                    }

                    if (config.GetOverwriteAll() == "true" && sStatus.StartsWith("Update available"))
                    {
                        string sSize = file.FileSize;
                        GetFileByRefGZIP(Int32.Parse(file.FileId), file.FilePath, file.FileName, ref sStatus, ref sSize);

                        file.FileSize = sSize;
                    }
                    else
                    {
                        if (!sStatus.StartsWith("Update available"))
                        {
                            GetFileByRefGZIP(Int32.Parse(file.FileId), file.FilePath, file.FileName, ref sStatus);
                        }
                    }
                    file.FileStatus = sStatus;
                }

                if (bNewFile)
                {
                    changedPropertyNotifier.NewFilesCount++;

                    file.FileStatus = sStatus;
                    file.FileIsVisible = true;
                    //report progress with fake percentage to change the current file in listview
                    window.WorkerSync_ChangeProgress(501, listFiles.IndexOf(file).ToString());
                }
                else if (config.GetShowOnlyNew() == "true" && (sStatus == "Not present" || sStatus == "New" || sStatus == "Update available!"))
                {
                    changedPropertyNotifier.NewFilesCount++;

                    file.FileStatus = sStatus;
                    file.FileIsVisible = true;
                    //report progress with fake percentage to add the current file to listview
                    window.WorkerSync_ChangeProgress(500, listFiles.IndexOf(file).ToString());
                }
                else if (config.GetShowOnlyNew() == "false")
                {
                    if (sStatus == "Not present" || sStatus == "Update available!")
                    {
                        changedPropertyNotifier.NewFilesCount++;
                    }

                    file.FileIsVisible = true;
                    //report progress with fake percentage to add the current file to listview
                    window.WorkerSync_ChangeProgress(500, listFiles.IndexOf(file).ToString());
                }

                iFilePercentage = cSimple.GetPercentage(iCounter + 1, iFileCount);
                window.WorkerSync_ChangeProgress(iFilePercentage);
                iCounter++;
            }
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
                    if (sOwnName != "__NO_VAL__" && sOwnName != "")
                    {
                        sTempPath = sPath.Replace(sCourseName, sOwnName);
                    }
                    else
                    {
                        sTempPath = sPath;
                    }

                    if (sStructTemplate != "__NO_VAL__" && sStructTemplate != "")
                    {
                        sStructTemplate = cSimple.ReplaceTemplatePlaceholder(sCourseName, sStructTemplate);
                        sPath = Path.Combine(sStructTemplate, sTempPath);
                    }
                }
                else if (config.GetUseOwnNames() == "false")
                {
                    if (sStructTemplate != "__NO_VAL__" && sStructTemplate != "")
                    {
                        sStructTemplate = cSimple.ReplaceTemplatePlaceholder(sCourseName, sStructTemplate);
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
                    if (sOwnName != "__NO_VAL__" && sOwnName != "")
                    {
                        sPath = sPath.Replace(sCourseName, sOwnName);
                    }
                }
            }

            sPath = Path.Combine(sConfigPath, sPath);

            if (!bBuildOnlyPath)
            {
                if (!Directory.Exists(sPath))
                {
                    if (sConfigPath == "")
                    {
                        window.WorkerSync_Cancel();
                        MessageBox.Show("No save path specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    try
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    catch (PathTooLongException)
                    {
                        //"Path too long!"
                    }
                }
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
        private void GetFileByRefGZIP(int iRefId, string sPath, string sName, ref string sFileStatus)
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
                    sFileStatus = "New";
                }
                catch (PathTooLongException)
                {
                    sFileStatus = "Path too long!";
                }

                //##############
                //Console.WriteLine("Writing file to disk done.");
                //##############
            }
            else
            {
                sFileStatus = "Found on disk";
            }
        }

        private void GetFileByRefGZIP(int iRefId, string sPath, string sName, ref string sFileStatus, ref string sSize)
        {
            string sFullPath = "";

            //build full path
            sFullPath = Path.Combine(sPath, sName);
            //Console.WriteLine("Path: " + sFullPath);

            if (File.Exists(sFullPath))                             //different from the other method version
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

                XDocument xDoc = XDocument.Parse(xmlFile);
                foreach (XNode node in xDoc.Nodes())
                {
                    if (node is XElement)
                    {
                        XElement element = (XElement)node;
                        if (element.Name.LocalName.Equals("File"))
                        {
                            int iTmp = int.Parse((string)element.Attribute("size"));

                            if (iTmp < 1049000)
                            {
                                //if smaller than 1 MB
                                sSize = cSimple.GetSizeInKiB(iTmp).ToString() + " KB";
                            }
                            else
                            {
                                sSize = cSimple.GetSizeInMiB(iTmp).ToString() + " MB";
                            }
                        }
                    }
                }

                foreach (XNode node in xDoc.DescendantNodes())
                {
                    if (node is XElement)
                    {
                        XElement element = (XElement)node;
                        if (element.Name.LocalName.Equals("Content"))
                        {
                            sContent = element.Value;
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
                    sFileStatus = "New";
                }
                catch (PathTooLongException)
                {
                    sFileStatus = "Path too long!";
                }

                //##############
                //Console.WriteLine("Writing file to disk done.");
                //##############
            }
        }

        public void GetSingleFile(FileInfo file)
        {
            string sStatus = "";
            string sSize = "";
            GetFileByRefGZIP(int.Parse(file.FileId), file.FilePath, file.FileName, ref sStatus, ref sSize);

            file.FileStatus = sStatus;
            file.FileSize = sSize;

            //report progress with fake percentage to change the current file in listview
            window.WorkerSyncOneFile_ChangeProgress(501, lFiles.IndexOf(file).ToString());
        }
    }

    public class CSimpleCalculations
    {
        private CConfig config = new CConfig();

        /// <summary>
        /// Get the semester number from a course name
        /// </summary>
        /// <param name="sCourseName">Raw course name</param>
        /// <returns>semester number or 0 in case of error</returns>
        public int GetSemesterNum(string sCourseName)
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
        public string GetCourseYear(string sCourseName)
        {
            //..., Wetter, SS2018
            string[] sTmpArray = sCourseName.Split(',');
            //SS2018 (or WS2017-18)
            try
            {
                if (sTmpArray[sTmpArray.Length - 1].Contains("SS"))
                {
                    string[] sTmpArray2 = sTmpArray[sTmpArray.Length - 1].Split('S');
                    //2018
                    return sTmpArray2[2];
                }
                else
                {
                    string[] sTmpArray2 = sTmpArray[sTmpArray.Length - 1].Split('S');
                    //2017-18
                    return sTmpArray2[1];
                }

            }
            catch (IndexOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// Replace the folder template with semester num and year
        /// </summary>
        /// <param name="sCourseName">Raw course name</param>
        /// <param name="sStructTemplate">Folder template</param>
        /// <returns>Filled in template</returns>
        public string ReplaceTemplatePlaceholder(string sCourseName, string sStructTemplate)
        {
            int iSemesterNum = GetSemesterNum(sCourseName);
            bool bUseYear = false;
            if (config.GetUseYearInStructure() == "true")
            {
                bUseYear = true;
            }
            string sCourseYear = GetCourseYear(sCourseName);

            if (iSemesterNum == 0)
            {
                return "Allgemein";
            }

            string sTmp = sStructTemplate.Replace("%", iSemesterNum.ToString());
            if (bUseYear)
            {
                if (sCourseYear == "")
                {
                    return sTmp.Replace("$", "");
                }
                else
                {
                    return sTmp.Replace("$", sCourseYear);
                }
            }
            else
            {
                return sTmp.Replace("$", "");
            }
        }

        /// <summary>
        /// Get the percentage of the given values
        /// </summary>
        /// <param name="iCurrent">Current value</param>
        /// <param name="iMax">Maximum value</param>
        /// <returns>Percentage</returns>
        public int GetPercentage(int iCurrent, int iMax)
        {
            return (int)(((double)iCurrent / iMax) * 100);
        }

        /// <summary>
        /// Get kibibyte size from byte
        /// </summary>
        /// <param name="iSizeInByte">Size in byte</param>
        /// <returns>Size in KiB</returns>
        public int GetSizeInKiB(int iSizeInByte)
        {
            return (int)((double)iSizeInByte / 1024.0);
        }

        /// <summary>
        /// Get mebibyte size from byte
        /// </summary>
        /// <param name="iSizeInByte">Size in byte</param>
        /// <returns>Size in MiB</returns>
        public int GetSizeInMiB(int iSizeInByte)
        {
            double dTemp = ((double)iSizeInByte / 1024.0);
            return (int)(dTemp / 1024.0);
        }
    }
}
