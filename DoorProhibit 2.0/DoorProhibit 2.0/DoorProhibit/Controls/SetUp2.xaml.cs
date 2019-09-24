using DoorProhibit.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DoorProhibit.Controls
{
    /// <summary>
    /// SetUp2.xaml 的交互逻辑
    /// </summary>
    public partial class SetUp2 : UserControl
    {
        public SetUp2(MainWindow setUpMain)
        {
            InitializeComponent();
            DataContext = new SetUpViewModel(setUpMain);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string passWord = ConfigurationManager.AppSettings["passWord"];
            if (oldPassWord.Text != passWord)
            {
                errorPIC.Source= new BitmapImage(new Uri("../images/测试连接失败.png", UriKind.RelativeOrAbsolute));
                errorCode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E00"));
                errorCode.Content = "密码错误";
                return;
            }
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(newPassWord.SecurePassword);
            string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
           if (password == passWord)
            {
                errorPIC.Source = new BitmapImage(new Uri("../images/测试连接失败.png", UriKind.RelativeOrAbsolute));
                errorCode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E00"));
                errorCode.Content = "密码重复";
                return;
            }
             p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(sPassWord.SecurePassword);
             passWord = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            if (passWord != password)
            {
                errorPIC.Source = new BitmapImage(new Uri("../images/测试连接失败.png", UriKind.RelativeOrAbsolute));
                errorCode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E00"));
                errorCode.Content = "密码不一致";
                return;
            }
            Task.Run(() =>
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                cfa.AppSettings.Settings["passWord"].Value = passWord;
                cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
            });
            success.Visibility = Visibility.Visible;
            pass.Visibility = Visibility.Hidden;
        }

        private void OldPassWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            string passWord = ConfigurationManager.AppSettings["passWord"];
            if (oldPassWord.Text != passWord && oldPassWord.Text.Length == passWord.Length)
            {
                errorPIC.Source = new BitmapImage(new Uri("../images/测试连接失败.png", UriKind.RelativeOrAbsolute));
                errorCode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E00"));//背景色
                errorCode.Content = "密码错误";
            }
            else if (oldPassWord.Text == passWord)
            {
                errorPIC.Source = new BitmapImage(new Uri("../images/测试连接成功.png", UriKind.RelativeOrAbsolute));
                errorCode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#07D6BF"));
                errorCode.Content = "密码正确";
            }
            else
            {
                errorPIC.Source = null;
                errorCode.Content = null;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
            if (string.IsNullOrEmpty(e.Text))
                e.Handled = true;
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
