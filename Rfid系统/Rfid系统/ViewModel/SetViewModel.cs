using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.View;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Rfid系统.ViewModel
{
    public class SetViewModel : NotificationObject
    {
        public MainWindow mainWindow;

        private ICommand closeCommond
        {
            get;
            set;
        }

        public ICommand CloseCommond
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.closeCommond) == null)
                {
                    arg_26_0 = (this.closeCommond = new DelegateCommand(delegate
                    {
                        LoginControl mainControl = new LoginControl(this.mainWindow);
                        this.mainWindow.gridControl.Children.Clear();
                        this.mainWindow.gridControl.Children.Add(mainControl);
                    }));
                }
                return arg_26_0;
            }
        }

        private string serverPort
        {
            get;
            set;
        }

        public string ServerPort
        {
            get
            {
                return this.serverPort;
            }
            set
            {
                this.serverPort = value;
                base.RaisePropertyChanged(()=> ServerPort);
            }
        }

        private string serverIP
        {
            get;
            set;
        }

        public string ServerIP
        {
            get
            {
                return this.serverIP;
            }
            set
            {
                this.serverIP = value;
                base.RaisePropertyChanged(()=> ServerIP);
            }
        }

        private string serverIPError
        {
            get;
            set;
        }

        public string ServerIPError
        {
            get
            {
                return this.serverIPError;
            }
            set
            {
                this.serverIPError = value;
                base.RaisePropertyChanged(()=> ServerIPError);
            }
        }

        private string ServerportError
        {
            get;
            set;
        }

        public string ServerPortError
        {
            get
            {
                return this.ServerportError;
            }
            set
            {
                this.ServerportError = value;
                base.RaisePropertyChanged(()=> ServerPortError);
            }
        }

        private ICommand okCommond
        {
            get;
            set;
        }

        public ICommand OkCommond
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.okCommond) == null)
                {
                    arg_26_0 = (this.okCommond = new DelegateCommand(delegate
                    {
                        ServerSetting.ServerIP = this.ServerIP;
                        if (string.IsNullOrEmpty(this.ServerIP))
                        {
                            this.ServerIPError = "服务器ip地址不能为空";
                            return;
                        }
                        if (!Regex.IsMatch(ServerIP, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$"))
                        {
                            this.ServerIPError = "服务器ip地址设置格式有误";
                            return;
                        }
                        ServerSetting.ServerPort = this.ServerPort;
                        if (string.IsNullOrEmpty(this.ServerPort))
                        {
                            this.ServerPortError = "服务器端口不能为空";
                            return;
                        }
                        if (this.ServerPort.Length != 4)
                        {
                            ServerPortError = "服务器端口设置有误";
                            return;
                        }
                        bool verification = VerificationConn.GetVerification();
                        if (verification)
                        {
                            this.ServerIPError = "";
                            this.ServerPortError = "连接服务器成功";
                            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            cfa.AppSettings.Settings["ServerIp"].Value = this.ServerIP;
                            cfa.AppSettings.Settings["ServerPort"].Value = this.ServerPort;
                            cfa.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            ServerSetting.UrlPath = string.Format("http://{0}:{1}/", this.ServerIP, this.ServerPort);
                            ServerSetting.PICPath = string.Format("http://{0}:{1}/filemodule/showFile/getShow", this.ServerIP, "8090");
                        }
                        else
                        {
                            ServerSetting.ServerIP = ConfigurationManager.AppSettings["ServerIp"];
                            ServerSetting.ServerPort = ConfigurationManager.AppSettings["ServerPort"];
                            this.ServerIPError = "测试服务器连接失败";
                            this.ServerPortError = "";
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        public SetViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.ServerPort = ConfigurationManager.AppSettings["ServerPort"];
            this.ServerIP = ConfigurationManager.AppSettings["ServerIp"];
        }

        public static Bitmap ToGray(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color color = bmp.GetPixel(i, j);
                    int gray = (int)((double)color.R * 0.3 + (double)color.G * 0.59 + (double)color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }
    }
}
