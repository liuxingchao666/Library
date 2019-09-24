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
using WpfApp2.BLL;
using WpfApp2.DAL;
using WpfApp2.Model;

namespace WpfApp2.View
{
    /// <summary>
    /// ReportLossSuccessPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReportLossSuccessPage : Window
    {
        public ReportLossSuccessPage(MainPage mainPage)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(AddTimes));
            this.mainPage = mainPage;
        }
        public MainPage mainPage;
        public System.Threading.Thread timer;
        delegate void UpdateTimer();
        public int Times;
        public CardData user;
        public void AddTimes()
        {
            while (true)
            {
                Times++;
                ServerSeting.ISAdd = false;
                this.Dispatcher.BeginInvoke(new UpdateTimer(isClose));
                Thread.Sleep(1000);
            }
        }

        public void isClose()
        {
            if (Times > 59)
            {
                Times = 62;
              
                if (!ServerSeting.ISAdd)
                {
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                }
                timer.Abort();
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.IsBackground = true;
            timer.Start();
        }
    }
}
