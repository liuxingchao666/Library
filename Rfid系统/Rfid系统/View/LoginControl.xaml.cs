using Rfidϵͳ.DAL;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rfid系统.View
{
    /// <summary>
    /// LoginControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new LoginViewModel(mainWindow);
            this.mainWindow = mainWindow;
        }
        public MainWindow mainWindow;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string result = ConfigurationManager.AppSettings["IsCheck"];
            Account.Text = ConfigurationManager.AppSettings["Account"];
            if (!string.IsNullOrEmpty(Account.Text))
                UserV.Visibility = Visibility.Hidden;
            if (result.Equals("True"))
            {
                LoginPassWord.Password = ConfigurationManager.AppSettings["PassWord"];
                Check.IsChecked = true;
                UserV.Visibility = Visibility.Hidden;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PassWord"]))
                    PassWordV.Visibility = Visibility.Hidden;
            }
        }

        private void UerV_MouseEnter(object sender, MouseEventArgs e)
        {
           // Account.Focus();
        }

        private void PassWordV_MouseEnter(object sender, MouseEventArgs e)
        {
            //LoginPassWord.Focus();
        }

        private void Account_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (Account.Text.Length == 0 || Account.Text.Length == 1)
                    UserV.Visibility = Visibility.Visible;
            }
            else
            {
                UserV.Visibility = Visibility.Hidden;
            }
        }

        private void LoginPassWord_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (LoginPassWord.Password.Length == 0 || LoginPassWord.Password.Length == 1)
                    PassWordV.Visibility = Visibility.Visible;
            }
            else
            {
                PassWordV.Visibility = Visibility.Hidden;
            }
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Account.Focus();
            //}
            //catch { }
        }

        private void LoginPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(LoginPassWord.SecurePassword);
                string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
                ///
                string LoginAccount = Account.Text;
                Account.Focus();
                if (string.IsNullOrEmpty(LoginAccount))
                {
                    AccountError.Content = "登陆账户不可为空";
                    return;
                }
                if (string.IsNullOrEmpty(password))
                {
                    PassWordError.Content = "登陆密码不可为空";
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
                    try
                    {
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                        cfa.AppSettings.Settings["Account"].Value = LoginAccount;
                        if (Check.IsChecked == true)
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
                            AccountError.Content = retrunInfo.result.ToString();
                        if (retrunInfo.ResultCode.Equals("201"))
                            PassWordError.Content = retrunInfo.result.ToString();
                    }
                    catch
                    {
                        ErrorPage errorPage = new ErrorPage(errorMsg.ToString(),mainWindow);
                        errorPage.ShowDialog();
                    }
                }
            }
        }
       
        private void UserV_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Account.Focus();
        }

        private void PassWordV_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoginPassWord.Focus();
        }
    }
}
