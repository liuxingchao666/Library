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
using WpfApp2.Controls.ShowControl;
using WpfApp2.DAL;

namespace WpfApp2.View
{
    /// <summary>
    /// IsReportPage.xaml 的交互逻辑
    /// </summary>
    public partial class IsReportPage : Window
    {
        public IsReportPage(MainPage page, CardData cardData)
        {
            InitializeComponent();
            thread = new Thread(new ThreadStart(() =>
            {
                while (time < 59)
                {
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        this.times.Content = 60 - time + "秒后返回主页";

                    });
                    time++;
                    Thread.Sleep(1000);
                }

                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
                this.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    this.Close();
                    thread.Abort();
                });
            }));
            this.page = page;
            this.cardData = cardData;
        }
        public CardData cardData;
        public Thread thread;
        public int time = 0;
        public MainPage page;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread.IsBackground = true;
            thread.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            time = 60;
            this.Close();
            thread.Abort();
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ServerSeting.ISAdd = false;

            ReportLossDAL lossDAL = new ReportLossDAL();
            object errormsg = cardData;
            if (lossDAL.AddCard(ref errormsg))
            {
                this.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    time = 60;
                    thread.Abort();
                    this.Close();
                    ServerSeting.ISAdd = false;
                    ReportLossSuccessPage report = new ReportLossSuccessPage(page);
                    report.user = cardData;
                    ReportLossSuccessShowControl showControl = new ReportLossSuccessShowControl(report, DealClass.Direct);
                    report.GRID.Children.Add(showControl);
                    report.ShowDialog();
                });
            }
            else
            {
                if (errormsg.ToString().Contains("301"))
                {
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        time = 60;
                        thread.Abort();
                        this.Close();
                        ServerSeting.ISAdd = false;
                        ReportLossSuccessPage report = new ReportLossSuccessPage(page);
                        report.user = cardData;
                        ReportLossSuccessShowControl showControl = new ReportLossSuccessShowControl(report, DealClass.indirect);
                        report.GRID.Children.Add(showControl);
                        report.ShowDialog();
                    });
                }
                else
                {
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        time = 60;
                        thread.Abort();
                        this.Close();
                        ServerSeting.ISAdd = false;
                        MainControl mainControl = new MainControl(page);
                        page.Grid.Children.Clear();
                        page.Grid.Children.Add(mainControl);
                        ServerSeting.ISAdd = false;
                        errorPage errorPage = new errorPage(errormsg.ToString());
                        errorPage.mainPage = page;
                        errorPage.ShowDialog();
                    });
                }
            }
        }
    }
}
