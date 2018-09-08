using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MahApps.Metro.IconPacks;

namespace WPF_ILIAS_Sync2Folder
{
    class ChangedPropertyNotifier : INotifyPropertyChanged
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
}
