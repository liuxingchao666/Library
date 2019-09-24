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
using WpfApp2.BLL;
using WpfApp2.Controls.ShowControl;
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.Controls
{
    /// <summary>
    /// PrintShow.xaml 的交互逻辑
    /// </summary>
    public partial class PrintShow : Window
    {
        public PrintShow(MainPage mainPage,RenewListControl data)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            thread = new Thread(new ThreadStart(print));
            thread.IsBackground = true;
            this.listControl = data;
        }
        public PrintAction printAction;
        public RenewListControl listControl;
        public Thread thread;
        public CardData data;
        public List<ArchivesInfo> infos = new List<ArchivesInfo>();
        public MainPage mainPage;
        public int i = 60;
        public void print()
        {
            i = 60;
            while (i > 1)
            {
                this.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    this.Timings.Content = i + "秒后将返回主页";
                });
                i--;
                Thread.Sleep(1000);
            }
            this.Dispatcher.BeginInvoke((System.Action)delegate
            {
                this.Close();
                thread.Abort();
                listControl.thread.Abort();
            });
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
           
            listControl.thread.Abort();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            i = 0;
            thread.Abort();
            this.Close();
            if (mainPage != null)
            {
                MainControl control = new MainControl(mainPage);
                mainPage.Grid.Children.Clear();
                mainPage.Grid.Children.Add(control);
            }

            ServerSeting.ISAdd = false;
            listControl.thread.Abort();
            switch (printAction)
            {
                case PrintAction.renew:
                    PrintParamtClass printParamt = new PrintParamtClass() { name = data.Name, phone = data.phone, cardNo = data.cardNum };
                    UseM5 useM5 = new UseM5(PrintClass.Renew, printParamt);
                    useM5.renewInfos = infos;
                    object errorMsg = "";
                    if (!useM5.ConnState(ref errorMsg))
                    {
                        errorPage errorPage = new errorPage(errorMsg.ToString());
                        errorPage.ShowDialog();
                        break;
                    }
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60; ;
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bool result = true;
                switch (printAction)
                {
                    case PrintAction.renew:
                        int Snum = 0;
                        int Fnum = 0;
                        foreach (ArchivesInfo info in infos)
                        {
                            if (info.errorMsg != null && info.errorMsg.Contains("成功"))
                            {
                                Snum++;
                                result = false;
                            }
                            else
                                Fnum++;
                        }
                    if (!result)
                    {
                        this.PIC.Source = new BitmapImage(new Uri("../ControlImages/成功.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        this.PIC.Source = new BitmapImage(new Uri("../ControlImages/失败.png", UriKind.RelativeOrAbsolute));
                    }
                    this.lbl.Content = "（温馨提示：此次借书成功" + Snum + "本，借书失败" + Fnum + "本）";
                        break;
                    case PrintAction.add:
                        break;
                    case PrintAction.loss:
                        break;
                    case PrintAction.reissue:
                        break;
                    default:
                        break;
                }
           
            thread.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            i = 0;
            thread.Abort();
            this.Close();
            listControl.thread.Abort();
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
            if (mainPage != null)
            {
                MainControl control = new MainControl(mainPage);
                mainPage.Grid.Children.Clear();
                mainPage.Grid.Children.Add(control);
            }
        }
    }
    public enum PrintAction
    {
        renew = 0,
        add = 1,
        reissue = 2,
        loss = 3
    }
}
