using Rfid系统.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rfid系统.View
{
    /// <summary>
    /// Load.xaml 的交互逻辑
    /// </summary>
    public partial class Load : Window
    {
        public Load()
        {
            InitializeComponent();
        }
        public bool IsQuit;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsQuit = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsQuit = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
