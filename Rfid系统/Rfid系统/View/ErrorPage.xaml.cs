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
    /// ErrorPage.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorPage : Window
    {
        public ErrorPage(string errorMsg,MainWindow mainWindow)
        {
            InitializeComponent();
            this.errorMsg.Content = errorMsg;
            thread = new Thread(new ThreadStart(() =>
            {
                while (i < 29)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        this.times.Content = (30 - i) + "秒后关闭";
                    });
                    i++;
                    Thread.Sleep(1000);
                }
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    thread.Abort();
                    this.Close();
                    if (ServerSetting.IsOverDue)
                    {
                        LoginControl loginControl = new LoginControl(mainWindow);
                        mainWindow.gridControl.Children.Clear();
                        mainWindow.gridControl.Children.Add(loginControl);
                    }
                });
            }));
            thread.IsBackground = true;
            this.mainWindow = mainWindow;
        }
        public Thread thread;
        public int i = 0;
        public MainWindow mainWindow;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            this.Close();
            if (ServerSetting.IsOverDue)
            {
                LoginControl loginControl = new LoginControl(mainWindow);
                mainWindow.gridControl.Children.Clear();
                mainWindow.gridControl.Children.Add(loginControl);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread.Start();
            i = 0;
        }
    }
}
