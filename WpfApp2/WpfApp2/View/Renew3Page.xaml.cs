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
using System.Windows.Shapes;
using System.Threading;
using WpfApp2.Model;
using WpfApp2.BLL;

namespace WpfApp2.View
{
    /// <summary>
    /// Renew3Page.xaml 的交互逻辑
    /// </summary>
    public partial class Renew3Page : Window
    {
        public Renew3Page()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(AddTimes));
        }
        System.Threading.Timer timer;
        public delegate void UpdateTimer();
        private int Times;
        public CardData user;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Change(0, 1000);
        }
        public void AddTimes(object e)
        {
            Times++;
            this.Dispatcher.BeginInvoke(new UpdateTimer(isClose));
        }
        public void isClose()
        {
            if (Times >= 30)
            {
                this.Close();
            }
        }
    }
}
