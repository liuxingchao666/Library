using Rfid系统.DAL;
using Rfid系统.Model;
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
    /// EditControl.xaml 的交互逻辑
    /// </summary>
    public partial class EditControl : Window
    {
        public EditControl(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = null;
            this.mainWindow = mainWindow;
        }
        public MainWindow mainWindow;
        public string passWord;
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Lb1.Visibility = Visibility.Hidden;
            OldPassWord.Focus();
        }

        private void Label_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Lb2.Visibility = Visibility.Hidden;
            NewPassWord.Focus();
        }

        private void Label_MouseEnter_2(object sender, MouseEventArgs e)
        {
            Lb3.Visibility = Visibility.Hidden;
            SecondNewPassWord.Focus();
        }

        private void OldPassWord_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text))
                Lb1.Visibility = Visibility.Hidden;
        }

        private void OldPassWord_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
                if (OldPassWord.Password.Length <= 1)
                {
                    Lb1.Visibility = Visibility.Visible;
                }
        }

        private void NewPassWord_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
                if (NewPassWord.Password.Length <= 1)
                {
                    Lb2.Visibility = Visibility.Visible;
                }
        }

        private void NewPassWord_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text))
                Lb2.Visibility = Visibility.Hidden;
        }

        private void SecondNewPassWord_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
                if (SecondNewPassWord.Password.Length <= 1)
                {
                    Lb3.Visibility = Visibility.Visible;
                }
        }

        private void SecondNewPassWord_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text))
                Lb3.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(OldPassWord.SecurePassword);
            string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            if (string.IsNullOrEmpty(password))
            {
                OldError.Content = "请输入密码";
                return;
            }
            if (password != passWord)
            {
                OldError.Content = "密码错误";
                return;
            }
            OldError.Content = "";
            list.Add(password);
            p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(NewPassWord.SecurePassword);
            password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            if (string.IsNullOrEmpty(password))
            {
                NewError.Content = "新密码不能为空";
                return;
            }
            if (password == passWord)
            {
                NewError.Content = "新密码不能等于原始密码";
                return;
            }
            string newPassWord = password;
            NewError.Content = "";
            p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(SecondNewPassWord.SecurePassword);
            password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            if (string.IsNullOrEmpty(password))
            {
                NewSecondError.Content = "请再次输入密码";
                return;
            }
            if (password != newPassWord)
            {
                NewSecondError.Content = "两次输入密码不一致";
                return;
            }
            NewSecondError.Content = "";
            list.Add(password);
            this.Close();
            ChangePassWordDAL changePassWordDAL = new ChangePassWordDAL();
            object errorMsg = list;
            if (changePassWordDAL.ChangePassWord(ref errorMsg))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                cfa.AppSettings.Settings["PassWord"].Value ="";
                cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件

                ServerSetting.Authorization = null;
                ChangeSuccessControl loginControl = new ChangeSuccessControl(mainWindow);
                DialogHelper.ShowDialog(loginControl);
            }
            else
            {
                RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                ErrorPage errorPage = new ErrorPage(retrunInfo.result.ToString(),mainWindow);
                errorPage.ShowDialog();
            }
        }

        private void OldPassWord_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OldPassWord.Password))
                Lb1.Visibility = Visibility.Visible;
        }

        private void NewPassWord_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPassWord.Password))
                Lb2.Visibility = Visibility.Visible;
        }

        private void SecondNewPassWord_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SecondNewPassWord.Password))
                Lb3.Visibility = Visibility.Visible;
        }
    }
}
