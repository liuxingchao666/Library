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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.DAL;
using WpfApp2.View;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// errorPage.xaml 的交互逻辑
    /// </summary>
    public partial class errorPage : Window
    {
        public errorPage(string errorMsg)
        {
            InitializeComponent();
            this.errorMsg.Content = errorMsg;
            thread = new Thread(new ThreadStart(() => {
                while (i > 1)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate {
                        this.times.Content = i+"秒后返回主页";
                        
                    });
                    i--;
                    Thread.Sleep(1000);
                    ServerSeting.ISAdd = false;
                }
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    thread.Abort();
                    this.Close();
                  
                });
                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
            }));
            thread.IsBackground = true;
        }
        public MainPage mainPage;
        public Thread thread;
        public int i = 30;
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            this.Close();
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
            if (mainPage == null)
            {
                return;
            }
            MainControl mainControl = new MainControl(mainPage);
            mainPage.Grid.Children.Add(mainControl);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread.Start();
        }
    }
}
