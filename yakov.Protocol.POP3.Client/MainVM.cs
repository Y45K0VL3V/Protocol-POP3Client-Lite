using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Protocol.POP3.Client
{
    public class MainVM : INotifyPropertyChanged
    {
        public MainVM()
        {

        }

        #region Binding properties. Input.

        private string _host;
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
                OnPropertyChanged("Host");
            }
        }

        private string _port;
        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                OnPropertyChanged("Port");
            }
        }

        private bool _isClientConnected = false;
        public bool IsClientConnected
        {
            get
            {
                return _isClientConnected;
            }
            set
            {
                _isClientConnected = value;
                OnPropertyChanged("IsClientConnected");
            }
        }

        private string _inputCommand;
        public string InputCommand
        {
            get
            {
                return _inputCommand;
            }
            set
            {
                _inputCommand = value;
                OnPropertyChanged("InputCommand");
            }
        }

        #endregion

        private List<string> _activityHistory = new List<string>();
        public List<string> ActivityHistory
        {
            get
            {
                return _activityHistory;
            }
            set
            {
                _activityHistory = value;
                OnPropertyChanged("ActivityCommand");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
