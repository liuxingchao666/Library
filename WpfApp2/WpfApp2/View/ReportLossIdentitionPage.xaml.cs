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
using WpfApp2.Controls;
using WpfApp2.DAL;
using WpfApp2.Model;

namespace WpfApp2.View
{
    /// <summary>
    /// ReportLossIdentitionPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReportLossIdentitionPage : UserControl
    {
        public ReportLossIdentitionPage(MainPage mainPage, string actionName)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(AddTimes));
            this.mainPage = mainPage;
        }
        public System.Threading.Thread timer;
        delegate void UpdateTimer();
        public int Times;
        public CardData user;
        public MainPage mainPage;
        public void AddTimes()
        {
            while (true)
            {
                Times++;
                this.Dispatcher.BeginInvoke(new UpdateTimer(isClose));
                Thread.Sleep(1000);
            }
        }

        public void isClose()
        {
            if (Times > 59)
            {
                this.Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    MainControl mainControl = new MainControl(mainPage);
                    mainPage.Grid.Children.Clear();
                    mainPage.Grid.Children.Add(mainControl);
                    MessageBox.Show("LossI");
                }));
                Times = 62;
                timer.Abort();
                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    Time.Content = "操作时间: " + (60 - Times) + "s";
                }));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.IsBackground = true;
            timer.Start();
        }
    }
}
