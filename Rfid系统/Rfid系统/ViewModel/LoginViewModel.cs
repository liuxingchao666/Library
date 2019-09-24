using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.BLL;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing;
using Image = System.Drawing.Image;
using Tesseract;
using System.IO;
using Newtonsoft.Json.Linq;
using Rfidϵͳ.DAL;

namespace Rfid系统.ViewModel
{
    public class LoginViewModel : NotificationObject
    {
        // 设置APPID/AK/SK
       
        public LoginViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            
               LoginAccount = ConfigurationManager.AppSettings["Account"];
           
        }
        public MainWindow mainWindow;
        /// <summary>
        /// 登陆
        /// </summary>
        private ICommand login { get; set; }
        public ICommand Login
        {
            get
            {
                return login ?? (login = new DelegateCommand<LoginControl>((data) =>
                {
                    ///图文交互(英文识别)
                    ServerSetting.IsOverDue = false;
                    
                    IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(data.LoginPassWord.SecurePassword);
                    string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
                    ///
                    data.Account.Focus();
                    if (string.IsNullOrEmpty(LoginAccount))
                    {
                        AccountError = "登陆账户不可为空";
                        return;
                    }
                    if (string.IsNullOrEmpty(password))
                    {
                        PassWordError = "登陆密码不可为空";
                        return;
                    }
                    LoginDAL loginDAL = new LoginDAL(LoginAccount, password);
                    object errorMsg = null;
                    if (loginDAL.GetLoginResult(ref errorMsg))
                    {
                        ServerSetting.BarcodePrinterName = null;
                        ServerSetting.BookmarkPrinterName = null;
                        ServerSetting.Account = LoginAccount;
                        ServerSetting.PassWord = password;
                        GetUserDAL.GetUser();
                        ServerSetting.userInfo.PassWord = password;
                        Task.Run(() =>
                        {
                            try
                            {
                                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                                cfa.AppSettings.Settings["Account"].Value = LoginAccount;
                                if (IsCheck)
                                {
                                    cfa.AppSettings.Settings["PassWord"].Value = password;
                                    cfa.AppSettings.Settings["IsCheck"].Value = "True";
                                }
                                else
                                {
                                    cfa.AppSettings.Settings["IsCheck"].Value = "False";
                                }
                                cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                                ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                            }
                            catch { }
                        });
                        MainControl mainControl = new MainControl(mainWindow);
                        mainWindow.gridControl.Children.Clear();
                        mainWindow.gridControl.Children.Add(mainControl);
                        mainControl.Focus();

                    }
                    else
                    {
                        try
                        {
                            RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                            if (retrunInfo.ResultCode.Equals("301"))
                                AccountError = retrunInfo.result.ToString();
                            if (retrunInfo.ResultCode.Equals("201"))
                                PassWordError = retrunInfo.result.ToString();
                        }
                        catch
                        {
                            ErrorPage errorPage = new ErrorPage(errorMsg.ToString(),mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                        }
                    }
                }));
            }
        }
        /// <summary>
        /// 登陆账号
        /// </summary>
        private string loginAccount { get; set; }
        public string LoginAccount
        {
            get { return loginAccount; }
            set
            {
                loginAccount = value;
                this.RaisePropertyChanged(() => LoginAccount);
            }
        }

        /// <summary>
        /// 账号错误
        /// </summary>
        private string accountError { get; set; }
        public string AccountError
        {
            get { return accountError; }
            set
            {
                accountError = value;
                this.RaisePropertyChanged(() => AccountError);
            }
        }
        /// <summary>
        /// 密码错误
        /// </summary>
        private string passwordError { get; set; }
        public string PassWordError
        {
            get { return passwordError; }
            set
            {
                passwordError = value; ;
                this.RaisePropertyChanged(() => PassWordError);
            }
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool isCheck { get; set; }
        public bool IsCheck
        {
            get { return isCheck; }
            set
            {
                isCheck = value;
                this.RaisePropertyChanged(() => IsCheck);
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        private ICommand changeCommond { get; set; }
        public ICommand ChangeCommond
        {
            get
            {
                return changeCommond ?? (changeCommond = new DelegateCommand(() =>
                {
                    if (string.IsNullOrEmpty(ServerSetting.Authorization))
                    {
                        //ErrorPage errorPage = new ErrorPage("暂没有登陆,没有修改权限");
                        //errorPage.ShowDialog();
                        return;
                    }
                    ChangeControl changeControl = new ChangeControl(ServerSetting.Account, ServerSetting.PassWord, mainWindow);
                    mainWindow.gridControl.Children.Clear();
                    mainWindow.gridControl.Children.Add(changeControl);
                }));
            }
        }

        private ICommand setCommond { get; set; }
        public ICommand SetCommond
        {
            get
            {
                return setCommond ?? (setCommond = new DelegateCommand(() =>
                {
                    //string AddressIP = string.Empty;
                    //foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                    //{
                    //    if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                    //    {
                    //        AddressIP = _IPAddress.ToString();
                    //    }
                    //}
                    //using (var client = new WebClient())
                    //{
                    //    var url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + AddressIP;
                    //    client.Encoding = Encoding.UTF8;
                    //    var str = client.DownloadString(url);
                    //    MessageBox.Show(str);
                    //}

                    SetControl setControl = new SetControl(mainWindow);
                    mainWindow.gridControl.Children.Clear();
                    mainWindow.gridControl.Children.Add(setControl);
                }));
            }
        }
    }
}
