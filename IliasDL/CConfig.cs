﻿//==================================================================
//
//    (c) Copyright by Viperinius
//
//==================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Net;
using System.IO;

namespace IliasDL
{
    class CConfig
    {
        public CConfig()
        {
        }

        /// <summary>
        /// Load an appsetting element from app.config
        /// </summary>
        /// <param name="key">Entry to look for</param>
        /// <returns>Value of key</returns>
        private string GetAppSetting(string key)
        {
            //load AppSettings
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //check if key exists
            if (config.AppSettings.Settings[key] != null)
            {
                //get value paired with the key
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                return "__NO_VAL__";
            }
            
        }

        /// <summary>
        /// Store an appsetting element in app.config
        /// </summary>
        /// <param name="key">Entry to store to</param>
        /// <param name="value">Value to be stored</param>
        private void SetAppSetting(string key, string value)
        {
            //load AppSettings
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //check if key exists
            if (config.AppSettings.Settings[key] != null)
            {
                //key exists, remove it for overriding
                config.AppSettings.Settings.Remove(key);
            }
            //add new key value pair
            config.AppSettings.Settings.Add(key, value);
            //save updated AppSettings
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Remove an element from app.config
        /// </summary>
        /// <param name="key">Entry to remove</param>
        private void ClearAppSetting(string key)
        {
            //load AppSettings
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //check if key exists
            if (config.AppSettings.Settings[key] != null)
            {
                //remove key
                config.AppSettings.Settings.Remove(key);
            }
            config.Save(ConfigurationSaveMode.Modified);
        }

        
        /// <summary>
        /// Set the endpoint address of the linked service reference
        /// </summary>
        /// <param name="sUrl">New endpoint address</param>
        public void SetIliasReference(string sUrl)
        {
            //load config
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNodeList endpoints = doc.GetElementsByTagName("endpoint");
            //search for url
            foreach (XmlNode node in endpoints)
            {
                var addressAttribute = node.Attributes["address"];

                if (!(addressAttribute is null))
                {
                    addressAttribute.Value = sUrl;
                }
            }
            doc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        /// <summary>
        /// Check if the given address is correct
        /// </summary>
        /// <param name="sUrl">URL to check</param>
        /// <returns>True if correct</returns>
        private bool IliasUrlIsCorrect(string sUrl)
        {
            if (sUrl.Contains(@"/webservice/soap/server.php") || sUrl.Contains(@"/login.php"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the client ID of the ILIAS server
        /// </summary>
        /// <param name="sUrl">URL to ILIAS</param>
        /// <returns>Client ID</returns>
        private string IliasGetClientId(string sUrl)
        {
            CookieContainer cookieJar = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);

            request.CookieContainer = cookieJar;
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;

            Stream dataStream = request.GetRequestStream();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var respCookies = response.Cookies;
            response.Close();

            return respCookies[0].Value;            
        }

        /// <summary>
        /// Format the given link to fit the endpoint address formatting
        /// </summary>
        /// <param name="sUrl">Raw URL</param>
        /// <param name="sClient">Client ID</param>
        /// <returns>Formatted URL or an empty string in case of errors</returns>
        public string FormatIliasUrlToWebServiceLink(string sUrl, ref string sClient)
        {         
            if (!IliasUrlIsCorrect(sUrl))
            {
                //url unlike expected login or webservice page
                return "";
            }
            
            string sResult = "";
            if (sUrl.Contains(@"/login.php"))
            {
                //given url is login page

                //get client id for further use (login)
                if (sUrl.Contains(@"client_id="))
                {
                    string sClientTemplate = "client_id=";
                    sClient = sUrl.Substring(sUrl.LastIndexOf(sClientTemplate) + sClientTemplate.Length, sUrl.Length - (sUrl.LastIndexOf(sClientTemplate) + sClientTemplate.Length));
                    if (sClient.Contains("&"))
                    {
                        sClient = sClient.Split('&')[0];
                    }
                }
                else
                {
                    sClient = IliasGetClientId(sUrl);
                }

                sResult = sUrl.Substring(0, sUrl.LastIndexOf("login.php")) + @"webservice/soap/server.php";
                return sResult;
            }
            else if (sUrl.Contains(@"/webservice/soap/server.php"))
            {
                //given url is webservice page
                if (sUrl.Contains("?"))
                {
                    sResult = sUrl.Split('?')[0];
                }
                else
                {
                    sResult = sUrl;
                }
                return sResult;
            }
            return "";
        }

        /// <summary>
        /// Set the save path setting
        /// </summary>
        /// <param name="sPath">Save path</param>
        public void SetPath(string sPath)
        {
            SetAppSetting("path", sPath);
        }

        /// <summary>
        /// Get the save path setting
        /// </summary>
        /// <returns>Save Path</returns>
        public string GetPath()
        {
            return GetAppSetting("path");
        }

        /// <summary>
        /// Set the server setting
        /// </summary>
        /// <param name="sServer">ILIAS server address</param>
        public void SetServer(string sServer)
        {
            SetAppSetting("server", sServer);
        }

        /// <summary>
        /// Get the server setting
        /// </summary>
        /// <returns>ILIAS server address</returns>
        public string GetServer()
        {
            return GetAppSetting("server");
        }

        /// <summary>
        /// Set the user setting
        /// </summary>
        /// <param name="sUser">Username</param>
        public void SetUser(string sUser)
        {
            SetAppSetting("user", sUser);
        }

        /// <summary>
        /// Get the user setting
        /// </summary>
        /// <returns>Username</returns>
        public string GetUser()
        {
            return GetAppSetting("user");
        }

        /// <summary>
        /// Set the client setting
        /// </summary>
        /// <param name="sClient"></param>
        public void SetClient(string sClient)
        {
            SetAppSetting("client", sClient);
        }

        public string GetClient()
        {
            return GetAppSetting("client");
        }

        public void SetSyncAll(bool bState)
        {
            if (bState)
            {
                SetAppSetting("syncall", "true");
            }
            else
            {
                SetAppSetting("syncall", "false");
            }
        }

        public string GetSyncAll()
        {
            return GetAppSetting("syncall");
        }

        public void SetCourse(string sCourseId)
        {
            SetAppSetting(sCourseId, sCourseId);
        }

        public string GetCourse(string sCourseId)
        {
            return GetAppSetting(sCourseId);
        }

        public void ClearCoursesSettings(string sCourseId)
        {
            ClearAppSetting(sCourseId);
        }

        public void SetCourseName(string sCourseId, string sName)
        {
            SetAppSetting("Name" + sCourseId, sName);
        }

        public string GetCourseName(string sCourseId)
        {
            return GetAppSetting("Name" + sCourseId);
        }

        public void ClearCourseName(string sCourseId)
        {
            ClearAppSetting("Name" + sCourseId);
        }

        public void SetUseOwnNames(bool bState)
        {
            if (bState)
            {
                SetAppSetting("useownnames", "true");
            }
            else
            {
                SetAppSetting("useownnames", "false");
            }
        }

        public string GetUseOwnNames()
        {
            return GetAppSetting("useownnames");
        }

        public void SetUseOwnStructure(bool bState)
        {
            if (bState)
            {
                SetAppSetting("useownstructure", "true");
            }
            else
            {
                SetAppSetting("useownstructure", "false");
            }
        }

        public string GetUseOwnStructures()
        {
            return GetAppSetting("useownstructure");
        }

        public void SetStructureTemplate(string sTemplate)
        {
            SetAppSetting("structuretemplate", sTemplate);
        }

        public string GetStructureTemplate()
        {
            return GetAppSetting("structuretemplate");
        }

        public void SetUseYearInStructure(bool bState)
        {
            if (bState)
            {
                SetAppSetting("useyear", "true");
            }
            else
            {
                SetAppSetting("useyear", "false");
            }
        }

        public string GetUseYearInStructure()
        {
            return GetAppSetting("useyear");
        }

        public void SetShowTrayIcon(bool bState)
        {
            if (bState)
            {
                SetAppSetting("trayicon", "true");
            }
            else
            {
                SetAppSetting("trayicon", "false");
            }
        }

        public string GetShowTrayIcon()
        {
            return GetAppSetting("trayicon");
        }
    }
}
