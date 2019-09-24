using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rfid系统.ViewModel
{
    public class ChangePassWordViewModel:NotificationObject
    {
        public ChangePassWordViewModel(MainWindow control)
        {
            this.main = control;
            LoginAccount = ServerSetting.Account;
        }
        public MainWindow main;
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
        /// 确认密码错误
        /// </summary>
        private string passwordvsError { get; set; }
        public string PassWordVSError
        {
            get { return passwordvsError; }
            set
            {
                passwordvsError = value; ;
                this.RaisePropertyChanged(() => PassWordVSError);
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
        private ICommand login { get; set; }
        public ICommand Login
        {
            get
            {
                return login ?? (login = new DelegateCommand<ChangeControl>((data) => {
                    List<string> list = new List<string>();
                    IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(data.LoginPassWord.SecurePassword);
                    string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
                    if(string.IsNullOrEmpty(password))
                    {
                        PassWordError = "登陆密码不可为空";
                        return;
                    }
                    PassWordError = "";
                    if (password != data.passWord)
                    {
                        PassWordError = "登陆密码错误";
                        return;
                    }
                    PassWordError = "";
                    list.Add(password);
                    p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(data.LoginPassWordVS.SecurePassword);
                    password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
                    if (string.IsNullOrEmpty(password))
                    {
                        PassWordVSError = "修改后密码不可为空";
                        return;
                    }
                    if (password == data.passWord)
                    {
                        PassWordVSError = "不能修改为正在使用的密码";
                        return;
                    }
                    PassWordVSError = "";
                    list.Add(password);
                    ChangePassWordDAL wordDAL = new ChangePassWordDAL();
                    object errorMsg = list;
                    if (wordDAL.ChangePassWord(ref errorMsg))
                    {
                        ServerSetting.Authorization = "";
                        LoginDAL loginDAL = new LoginDAL(LoginAccount,password);
                         errorMsg = null;
                        if (loginDAL.GetLoginResult(ref errorMsg))
                        {
                            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                            if (IsCheck)
                            {
                                cfa.AppSettings.Settings["Account"].Value = LoginAccount;
                                cfa.AppSettings.Settings["PassWord"].Value = password;
                                cfa.AppSettings.Settings["IsCheck"].Value = "True";
                            }
                            else
                            {
                                cfa.AppSettings.Settings["IsCheck"].Value = "False";
                            }
                            cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                            ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                            MainControl mainControl = new MainControl(main);
                            main.gridControl.Children.Clear();
                            main.gridControl.Children.Add(mainControl);
                        }
                    }
                    else
                    {
                        RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                        PassWordVSError = retrunInfo.result.ToString();
                    }
                }));
            }
        }
    }
}
