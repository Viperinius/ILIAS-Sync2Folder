﻿using System;
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
        public List<CourseInfo> listCourseInfos = new List<CourseInfo>();

        private int iFilePercentage = 0;
        private int iCoursePercentage = 0;
        private int iCurrentCourseNum = 0;
        public bool bLoggedIn;

        private MainWindow window;
        public CIliasHandling(MainWindow mainWindow)
        {
            window = mainWindow;
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
                                lFiles.Add(new FileInfo() { FileId = (string)subElement.Attribute("ref_id") });
                                //note current file for later use (filling the remaining info)
                                currentFile = lFiles.Last();
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

                            currentFile = lFiles.Last();                                                                    //test if still the same as above

                            //find file extension & size
                            foreach (XElement subElement in element.Descendants("Properties"))
                            {
                                foreach (XElement subSubElement in subElement.Descendants("Property"))
                                {
                                    if ((string)subSubElement.Attribute("name") == "fileSize")
                                    {
                                        currentFile.FileSize = subSubElement.Value;
                                    }
                                }
                            }

                            //find last modified/updated date
                            foreach (XElement subElement in element.Descendants("LastUpdate"))
                            {
                                currentFile.FileDate = subElement.Value;
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
            int iFileCount = lFiles.Count;
            int iCourseCount = listCourseInfos.Count;

            iFilePercentage = cSimple.GetPercentage(0, iFileCount);
            iCourseCount = cSimple.GetPercentage(iCurrentCourseNum, iCourseCount);

            DownloadFiles(iCourseId, iFileCount);

            

        }

        /// <summary>
        /// Show each file on listview and download them (if setting is set)
        /// </summary>
        /// <param name="iCourseId">Current course ID</param>
        /// <param name="iFileCount">Count of all files in the current course</param>
        private void DownloadFiles(int iCourseId, int iFileCount)
        {
            string sStatus = "Not present";

            int iCounter = 0;
            foreach (FileInfo file in lFiles)
            {
                window.WorkerSync_ChangeProgress(iFilePercentage);
                if (window.WorkerSync_IsCancelPending())
                {
                    return;
                }

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
                }

                //format size to be human readable
                int iSize = Int32.Parse(file.FileSize);
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
                    }

                    GetFileByRefGZIP(Int32.Parse(file.FileId), file.FilePath, file.FileName, ref sStatus);
                    file.FileStatus = sStatus;
                }

                if (bNewFile)
                {
                    file.FileStatus = sStatus;
                    file.FileIsVisible = true;
                }
                else if (config.GetShowOnlyNew() == "true" && (sStatus == "Not present" || sStatus == "New"))
                {
                    file.FileStatus = sStatus;
                    file.FileIsVisible = true;
                }
                else if (config.GetShowOnlyNew() == "false")
                {
                    file.FileIsVisible = true;
                }

                iFilePercentage = cSimple.GetPercentage(iCounter + 1, iFileCount);
                window.WorkerSync_ChangeProgress(iFilePercentage);
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
                    sTempPath = sPath.Replace(sCourseName, sOwnName);

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
                        window.WorkerSync_Cancel();
                        MessageBox.Show("No save path specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Directory.CreateDirectory(sPath);
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
        public int GetCourseYear(string sCourseName)
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
        public string ReplaceTemplatePlaceholder(string sCourseName, string sStructTemplate)
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
