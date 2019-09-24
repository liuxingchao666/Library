using RFID_Reader_Cmds;
using RFID_Reader_Com;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using WpfApp2.BLL;
using WpfApp2.DAL;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls
{
    /// <summary>
    /// MainControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainControl : UserControl
    {
        public ReceiveParser receiveParser;
        public MainControl(MainPage mainPage)
        {
            InitializeComponent();
            DataContext = new MainViewModel(mainPage);
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            this.page = mainPage;
        }

        public Thread timer;
        public BookRfid book;
        public MainPage page;
        delegate void UpdateTimer();
        protected int times = 0;
        protected List<int> Ilist = new List<int>();
        protected bool IsSet = false;
        public void EditTimes()
        {
            while (true)
            {
                try
                {
                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                    {
                        date.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }));
                    if (IsSet)
                    {
                        times++;
                        if (times >= 3)
                        {
                            times = 0;
                            Ilist = new List<int>();
                            IsSet = false;
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void Edit()
        {
            this.Dispatcher.BeginInvoke(new System.Action(delegate
            {
                date.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ServerSeting.Mus)
            {

                music.Stop();
                music.Play();

                ServerSeting.Mus = false;
            }
            date.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timer.IsBackground = true;
            timer.Start();
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
        }

        private void Image_TouchDown(object sender, TouchEventArgs e)
        {

        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ServerSeting.ISAdd = false;
        }

        private void Set_Click(object sender, RoutedEventArgs e)
        {
            if (Ilist.Count == 0)
            {
                times = 0;
                IsSet = true;
            }
            Ilist.Add(1);
            if (Ilist.Count == 5)
            {
                Setting setting = new Setting();
                DialogHelper.ShowDialog(setting);
                IsSet = false;
                times = 0;
                Ilist = new List<int>();
            }
        }
    }
}

