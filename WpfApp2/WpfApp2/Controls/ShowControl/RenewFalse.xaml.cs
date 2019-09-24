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
using WpfApp2.DAL;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// RenewFalse.xaml 的交互逻辑
    /// </summary>
    public partial class RenewFalse : Window
    {
        public RenewFalse(object errorMsg)
        {
            InitializeComponent();
            Task.Run(() =>
            {

                if (errorMsg != null)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        this.textb.Text = errorMsg.ToString();
                    });
                }
            });
            thread = new Thread(new ThreadStart(() =>
            {
                while (i > 1)
                {
                    i--;
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        this.Timings.Content = i + "秒后将返回主页";
                    });
                    Thread.Sleep(1000);
                }

                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    this.Close();
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                });
                thread.Abort();

            }));
            thread.IsBackground = true;
        }
        public int i = 20;
        private Thread thread;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            this.Close();
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread.Start();
        }
    }
}
