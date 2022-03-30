using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.Protocol.POP3.Client.Model;

namespace yakov.Protocol.POP3.Client
{
    public class MainVM : INotifyPropertyChanged
    {
        public MainVM()
        {

        }

        #region Binding properties. Input.

        private string _host = "pop.gmail.com";
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

        private int? _port = 995;
        public string Port
        {
            get
            {
                return _port.ToString();
            }
            set
            {
                try
                {
                    _port = int.Parse(value);
                    IsDataCorrect = true;
                }
                catch
                {
                    IsDataCorrect = false;
                    throw new ArgumentException("Invalid port.");
                }
                OnPropertyChanged("Port");
            }
        }

        private bool _isDataCorrect = false;
        public bool IsDataCorrect
        {
            get
            {
                return _isDataCorrect;
            }
            set
            {
                _isDataCorrect = value;
                OnPropertyChanged("IsDataCorrect");
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

        private RelayCommand _tryConnect;
        public RelayCommand TryConnect
        {
            get
            {
                return _tryConnect ??
                  (_tryConnect = new RelayCommand(obj =>
                  {
                      if (IsClientConnected)
                          ActivityHistory.Add($"Server: {InteractionControl.Execute("QUIT")}");

                      ActivityHistory.Add($"Connecting to {Host}:{Port}");
                      string answer = InteractionControl.ClientConnect(Host, (int)_port);
                      if (answer != null)
                      {
                          IsClientConnected = true;
                          ActivityHistory.Add($"Server: {answer}");
                      }
                      else
                      {
                          ActivityHistory.Add($"Connection error.");
                          IsClientConnected = false;
                      }
                  }));
            }
        }

        private RelayCommand _sendCommand;
        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand ??
                  (_sendCommand = new RelayCommand(obj =>
                  {
                      if (String.IsNullOrEmpty(InputCommand))
                          return;

                      ActivityHistory.Add($"Client: {InputCommand}");
                      string answer = InteractionControl.Execute(InputCommand);
                      InputCommand = null;
                      if (answer != "")
                      {
                          ActivityHistory.Add($"Server: {answer}");
                      }
                      else
                      {
                          ActivityHistory.Add($"Error occured");
                      }
                  }));
            }
        }

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
