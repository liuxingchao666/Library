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
    /// ChangeSuccessControl.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeSuccessControl : Window
    {
        public ChangeSuccessControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            thread = new Thread(new ThreadStart(() => {
                while (times > 1)
                {
                   
                    this.Dispatcher.BeginInvoke((Action)delegate {
                        time.Content = times + "秒后返回主页";
                    });
                    times--;
                    Thread.Sleep(1000);
                }
                
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    thread.Abort();
                    this.Close();
                    LoginControl loginControl = new LoginControl(mainWindow);
                    mainWindow.gridControl.Children.Clear();
                    mainWindow.gridControl.Children.Add(loginControl);
                });
            }));
        }
        public MainWindow mainWindow;
        public Thread thread;
        public int times = 5;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            this.Close();
            LoginControl loginControl = new LoginControl(mainWindow);
            mainWindow.gridControl.Children.Clear();
            mainWindow.gridControl.Children.Add(loginControl);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
