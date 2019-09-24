using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.PIC.Source = new BitmapImage(new Uri(@"E:\\创口通讯\\射频识别\\射频识别\\bin\\x64\\Debug\\af1d0255-c097-4809-baea-a6b4f9d06728.bmp"));
        }

        private void PIC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("cesjo ");
        }
    }
}
