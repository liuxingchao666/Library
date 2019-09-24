using Rfid系统.ViewModel;
using System;
using System.Collections.Generic;
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
    /// ChangeControl.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeControl : UserControl
    {
        public ChangeControl(string UserName,string passWord,MainWindow main)
        {
            InitializeComponent();
            this.passWord = passWord;
            DataContext = new ChangePassWordViewModel(main);
        }
        public string passWord;
        private void UserV_MouseEnter(object sender, MouseEventArgs e)
        {
            Account.Focus();
        }

        private void PassWordV_MouseEnter(object sender, MouseEventArgs e)
        {
            LoginPassWord.Focus();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            LoginPassWordVS.Focus();
        }

        private void Account_PreviewKeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void LoginPassWord_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (LoginPassWord.Password.Length <= 1)
                {
                    PassWordV.Visibility = Visibility.Visible;
                }
            }
            else
            {
                PassWordV.Visibility = Visibility.Hidden;
            }
        }

        private void LoginPassWordVS_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (LoginPassWordVS.Password.Length <= 1)
                {
                    PassWordVS.Visibility = Visibility.Visible;
                }
            }
            else
            {
                PassWordVS.Visibility = Visibility.Hidden;
            }
        }
    }
}
