using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace IliasDL
{
    class CConfig
    {
        public CConfig()
        {
        }

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




        public void SetPath(string sPath)
        {
            SetAppSetting("path", sPath);
        }

        public string GetPath()
        {
            return GetAppSetting("path");
        }

        public void SetServer(string sServer)
        {
            SetAppSetting("server", sServer);
        }

        public string GetServer()
        {
            return GetAppSetting("server");
        }

        public void SetUser(string sUser)
        {
            SetAppSetting("user", sUser);
        }

        public string GetUser()
        {
            return GetAppSetting("user");
        }

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
