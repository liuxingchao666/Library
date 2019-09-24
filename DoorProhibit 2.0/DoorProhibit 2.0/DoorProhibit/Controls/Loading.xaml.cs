using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace DoorProhibit.Controls
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : Window
    {
        public Loading()
        {
            InitializeComponent();
        }
         public   bool isLogin;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isLogin = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string passWord = ConfigurationManager.AppSettings["passWord"];
            Regex regex = new Regex("^1[34578]\\d{9}$");
            if (regex.IsMatch(this.passWord.Text))
                MessageBox.Show("555");
            if (!this.passWord.Text.ToString().Equals( passWord))
            {
                error.Visibility = Visibility.Visible;
                return;
            }

            isLogin = true;
            this.Close();
        }
    }
}
