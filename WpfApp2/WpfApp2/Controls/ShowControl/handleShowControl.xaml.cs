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
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// handleShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class handleShowControl : Window
    {
        public MainPage page;
        public GetMessage getMessage;
        public handleShowControl(MainPage renewpage)
        {
            InitializeComponent();
            timer = new Thread(new ThreadStart(EditTimes));
            timer.IsBackground = true;
            page = renewpage;
            ServerSeting.GetMessage.user = null;
        }
      Thread timer;
        public bool results ;
        delegate void UpdateTimer();
        public int Times;
        public bool judge = true;
        public void EditTimes()
        {
            while (Times < 59)
            {
                Times++;
                this.Dispatcher.BeginInvoke((System.Action)delegate {
                    if (Times <= 59)
                    {
                        this.times.Content = (60 - Times).ToString() + "秒后返回主页";
                        if (ServerSeting.GetMessage.user == null)
                        {
                            try
                            {
                                ServerSeting.GetMessage.GO();
                                if (ServerSeting.GetMessage.user != null && !string.IsNullOrEmpty(ServerSeting.GetMessage.user.Name))
                                {
                                    judge = false;
                                    CardData handleuser = new CardData()
                                    {
                                        Name = ServerSeting.GetMessage.user.Name,
                                        PIC = ServerSeting.GetMessage.user.PIC,
                                        Sex = ServerSeting.GetMessage.user.Sex,
                                        Nation = ServerSeting.GetMessage.user.NationName,
                                        CardNo = ServerSeting.GetMessage.user.IdentificationCode,
                                        Born = ServerSeting.GetMessage.user.BirdTh,
                                        Address = ServerSeting.GetMessage.user.Address
                                    };
                                    ServerSeting.GetMessage.user = null;
                                    if (handleuser != null && !string.IsNullOrEmpty(handleuser.Name))
                                    {
                                        UserMessage userMessage = new UserMessage()
                                        {
                                            Name = handleuser.Name,
                                            PIC = handleuser.PIC,
                                            IdentificationCode = handleuser.CardNo
                                        };
                                        timer.Abort();
                                        this.Close();
                                        page.Grid.Children.Clear();
                                    
                                        HandleidentitionPage handleidentitionPage = new HandleidentitionPage(page);
                                        GetNowUserDAL getNowUserDAL = new GetNowUserDAL();
                                        object errorMsg = userMessage;
                                        if (getNowUserDAL.GetPushDAL(ref errorMsg))
                                        {
                                            handleidentitionPage.ActionName = "个人信息";
                                            userMessage = errorMsg as UserMessage;
                                            HandleidentitionMessagexaml messagexaml = new HandleidentitionMessagexaml(handleidentitionPage, userMessage);
                                            handleidentitionPage.GRID.Children.Add(messagexaml);
                                            page.Grid.Children.Add(handleidentitionPage);
                                        }
                                        else
                                        {
                                            handleidentitionPage.user = handleuser;
                                            HandleidentitionShowControl1 handleidentitionShowControl = new HandleidentitionShowControl1(handleidentitionPage);
                                            handleidentitionPage.handleidentitionShowControl =handleidentitionShowControl;
                                            handleidentitionPage.GRID.Children.Add(handleidentitionShowControl);
                                            page.Grid.Children.Add(handleidentitionPage);
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                });
                Thread.Sleep(1000);
            }
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
            this.Dispatcher.BeginInvoke((System.Action)delegate
            {
                this.Close();
                timer.Abort();
            });
        }

     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Times = 0;
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Times = 60;
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
        }
    }
}
