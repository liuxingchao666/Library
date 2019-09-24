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
using WpfApp2.Model;

namespace WpfApp2.View
{
    /// <summary>
    /// HandleidentitionPage.xaml 的交互逻辑
    /// </summary>
    public partial class HandleidentitionPage : UserControl
    {
        public HandleidentitionPage(MainPage mainPage)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(AddTimes));
            this.mainPage = mainPage;
            Times = 0;
        }
        public string ActionName;
        public MainPage mainPage;
        public System.Threading.Thread timer;
        delegate void UpdateTimer();
        public int Times;
        public HandleidentitionShowControl1 handleidentitionShowControl;
        public CardData user;
        public void AddTimes()
        {
            while (true)
            {
                isClose();
                Thread.Sleep(1000);
            }
        }

        public void isClose()
        {
            if (Times > 58)
            {
                this.Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    MainControl mainControl = new MainControl(mainPage);
                    mainPage.Grid.Children.Clear();
                    mainPage.Grid.Children.Add(mainControl);
                }));
                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
                timer.Abort();
            }

            this.Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Time.Content = "操作时间: " + (60 - Times) + "s";
            }));
            ServerSeting.ISAdd = false;
            Times++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.IsBackground = true;
                timer.Start();
                if (!string.IsNullOrEmpty(ActionName))
                {
                    this.actionName.Content = ActionName;
                    ActionName = null;
                }
            }
            catch
            {

            }
        }
    }
}
