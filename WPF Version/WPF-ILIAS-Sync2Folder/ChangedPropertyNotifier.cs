using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MahApps.Metro.IconPacks;

namespace WPF_ILIAS_Sync2Folder
{
    public class ChangedPropertyNotifier : INotifyPropertyChanged
    {
        private string _BtnLoginText = "Login";
        private string _BtnSyncText = "Start synchronisation";

        public string BtnLoginText
        {
            get { return _BtnLoginText; }
            set
            {
                _BtnLoginText = value;
                OnPropertyChanged("BtnLoginText");
            }
        }

        public string BtnSyncText
        {
            get { return _BtnSyncText; }
            set
            {
                _BtnSyncText = value;
                OnPropertyChanged("BtnSyncText");
            }
        }


        

        public void ChangeBtnLoginText()
        {
            if (_BtnLoginText == "Login")
            {
                BtnLoginText = "Logout";
            }
            else
            {
                BtnLoginText = "Login";
            }
        }

        public void ChangeBtnSyncText()
        {
            if (_BtnSyncText == "Start synchronisation")
            {
                BtnSyncText = "Stop synchronisation";
            }
            else
            {
                BtnSyncText = "Start synchronisation";
            }
        }               

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string sPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sPropertyName));
        }

        #endregion
    }

    public class CourseInfo : INotifyPropertyChanged
    {
        private bool _bCourseChecked = false;
        private string _sCourseName = "";
        private string _sCourseOwnName = "";
        private string _sCourseId = "";

        public bool CourseChecked
        {
            get { return _bCourseChecked; }
            set
            {
                _bCourseChecked = value;
                OnPropertyChanged("CourseChecked");
            }
        }

        public string CourseName
        {
            get { return _sCourseName; }
            set
            {
                _sCourseName = value;
                OnPropertyChanged("CourseName");
            }
        }

        public string CourseOwnName
        {
            get { return _sCourseOwnName; }
            set
            {
                _sCourseOwnName = value;
                OnPropertyChanged("CourseOwnName");
            }
        }

        public string CourseId
        {
            get { return _sCourseId; }
            set
            {
                _sCourseId = value;
                OnPropertyChanged("CourseId");
            }
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string sPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sPropertyName));
        }

        #endregion
    }

    public class FileInfo : INotifyPropertyChanged
    {
        private string _sFileStatus = "Missing";
        private string _sFileName = "";
        private string _sFilePath = "";
        private string _sFileDate = "";
        private string _sFileSize = "";
        private string _sFileId = "";

        private bool _bFileIsVisible = true;

        public string FileStatus
        {
            get { return _sFileStatus; }
            set
            {
                _sFileStatus = value;
                OnPropertyChanged("FileStatus");
            }
        }

        public string FileName
        {
            get { return _sFileName; }
            set
            {
                _sFileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string FilePath
        {
            get { return _sFilePath; }
            set
            {
                _sFilePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public string FileDate
        {
            get { return _sFileDate; }
            set
            {
                _sFileDate = value;
                OnPropertyChanged("FileDate");
            }
        }

        public string FileSize
        {
            get { return _sFileSize; }
            set
            {
                _sFileSize = value;
                OnPropertyChanged("FileSize");
            }
        }

        public string FileId
        {
            get { return _sFileId; }
            set
            {
                _sFileId = value;
                OnPropertyChanged("FileId");
            }
        }

        public bool FileIsVisible
        {
            get { return _bFileIsVisible; }
            set
            {
                _bFileIsVisible = value;
                OnPropertyChanged("FileIsVisible");
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string sPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sPropertyName));
        }

        #endregion
    }
}
