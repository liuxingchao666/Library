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
    /// Renew.xaml 的交互逻辑
    /// </summary>
    public partial class RenewPage : Window
    {
        public RenewPage(MainPage page)
        {
            InitializeComponent();
            CardNum = null;
            ServerSeting.GetMessage.user = null;
            handleuser = null;
            PatchCarduser = null;
            this.page = page;
        }

        public MainPage page;
        public Thread timer;
        delegate void UpdateTimer();
        private int Times;
        public CardData RenewUser;
        /// <summary>
        /// 办卡扫描信息返回
        /// </summary>
        public CardData handleuser;
        /// <summary>
        /// 挂失卡扫描信息返回
        /// </summary>
        public CardData PatchCarduser;
        /// <summary>
        /// IC卡号
        /// </summary>
        public string CardNum { get; set; }
        public bool isClosed;
        public void AddTimes()
        {
            while (true)
            {
                Times++;
                this.Dispatcher.BeginInvoke(new UpdateTimer(isClose));
                Thread.Sleep(1000);
            }
        }
        public UserMessage userMessage;
        public void isClose()
        {
            if (Times > 59)
            {
                this.Close();
                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
                timer.Abort();
            }
            else
            {
                if (!string.IsNullOrEmpty(CardNum))
                {
                    Times = 62;
                    timer.Abort();
                    this.Close();
                }
                if (PatchCarduser != null && !string.IsNullOrEmpty(PatchCarduser.Name))
                {
                    userMessage = new UserMessage()
                    {
                        Name = PatchCarduser.Name,
                        PIC = PatchCarduser.PIC,
                        IdentificationCode = PatchCarduser.CardNo
                    };
                    Times = 62;
                    timer.Abort();
                    this.Close();
                    GetNowUserDAL getNowUserDAL = new GetNowUserDAL();
                    object errorMsg = userMessage;
                    if (getNowUserDAL.GetPushDAL(ref errorMsg))
                    {
                        userMessage = errorMsg as UserMessage;
                        if (userMessage.State.Equals("1"))
                        {
                            errorPage errorPage = new errorPage("您存在失信记录，请先处理");
                            errorPage.ShowDialog();
                        }
                        else if (userMessage.OverDueState.Equals("1"))
                        {
                            errorPage errorPage = new errorPage("您存在逾期记录，请先处理");
                            errorPage.ShowDialog();
                        }
                        else if (userMessage.CardState.Equals("1"))
                        {
                            ReportLossSuccessPage report = new ReportLossSuccessPage(page);
                            report.user = PatchCarduser;
                            ReportLossSuccessShowControl showControl = new ReportLossSuccessShowControl(report, DealClass.indirect);
                            report.GRID.Children.Add(showControl);
                            report.ShowDialog();
                        }
                        else
                        {
                            IsReportPage isReportPage = new IsReportPage(page, PatchCarduser);
                            isReportPage.ShowDialog();
                        }
                    }
                    else
                    {
                        errorPage errorPage = new errorPage("用户不存在读者卡，请先办理");
                        errorPage.ShowDialog();
                    }
                    #region
                    //}
                    //else
                    //{
                    //    if (errormsg.ToString().Contains("301"))
                    //    {
                    //        ActionName = "补办";
                    //        GetReplacementCost getReplacementCost = new GetReplacementCost();
                    //        if (getReplacementCost.Getdesosit(ref errormsg))
                    //        {
                    //            ReturnInfo info = errormsg as ReturnInfo;
                    //            if (info.CostDeposit > 0)
                    //            {
                    //                if (!ServerSeting.MConnState)
                    //                {
                    //                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                    //                    {
                    //                        MainControl mainControl = new MainControl(page);
                    //                        page.Grid.Children.Clear();
                    //                        page.Grid.Children.Add(mainControl);
                    //                    }));
                    //                    errorPage errorPage = new errorPage("进钞机设备连接异常，请检查设备连接");
                    //                    errorPage.mainPage = page;
                    //                    errorPage.ShowDialog();
                    //                }
                    //                else
                    //                {
                    //                    PayMentPage mentPage = new PayMentPage(DealBusiness.Reissue, PayStep.Reissue, page);
                    //                    mentPage.info = info;
                    //                    PatchCarduser.cost = info.CostDeposit;
                    //                    mentPage.user = PatchCarduser;
                    //                    Transaction3ShowControl transaction3ShowControl = new Transaction3ShowControl(mentPage);
                    //                    mentPage.GRID.Children.Add(transaction3ShowControl);
                    //                    page.Grid.Children.Clear();
                    //                    page.Grid.Children.Add(mentPage);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (ServerSeting.hairpin.serialPort.IsOpen)
                    //                {
                    //                    byte[] send = new byte[6];
                    //                    send[0] = 0x02;
                    //                    send[1] = 0x44;
                    //                    send[2] = 0x48;
                    //                    send[3] = 0x03;
                    //                    send[4] = 0x0d;
                    //                    send[5] = 0x05;
                    //                    ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                    //                }
                    //                Thread.Sleep(1500);
                    //                if (!ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                    //                {
                    //                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                    //                    {
                    //                        MainControl mainControl = new MainControl(page);
                    //                        page.Grid.Children.Clear();
                    //                        page.Grid.Children.Add(mainControl);
                    //                        errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查设备连接");
                    //                        errorPage.mainPage = page;
                    //                        errorPage.ShowDialog();
                    //                    }));
                    //                    Times = 32;
                    //                    timer.Abort();
                    //                }
                    //                else
                    //                {

                    //                    ICcardMessage ccardMessage = new ICcardMessage();
                    //                    if (ServerSeting.CcardBLL.ReadCard(ref ccardMessage))
                    //                    {
                    //                        PatchCarduser.cardNum = ccardMessage.CardNum;
                    //                    }
                    //                    ServerSeting.CcardBLL.rfidReader.DisConnect();
                    //                }

                    //                //补办
                    //                MakeUpDAL makeUpDAL = new MakeUpDAL();
                    //                errormsg = PatchCarduser;
                    //                if (!makeUpDAL.MakeUpCard(ref errormsg))
                    //                {

                    //                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                    //                    {
                    //                        MainControl mainControl = new MainControl(page);
                    //                        page.Grid.Children.Clear();
                    //                        page.Grid.Children.Add(mainControl);
                    //                        errorPage errorPage = new errorPage(errormsg.ToString());
                    //                        errorPage.mainPage = page;
                    //                        errorPage.ShowDialog();
                    //                    }));
                    //                    Times = 32;
                    //                    timer.Abort();
                    //                }
                    //                else
                    //                {
                    //                    if (ServerSeting.hairpin.serialPort.IsOpen)
                    //                    {
                    //                        byte[] send = new byte[6];
                    //                        send[0] = 0x02;
                    //                        send[1] = 0x45;
                    //                        send[2] = 0x53;
                    //                        send[3] = 0x03;
                    //                        send[4] = 0x17;
                    //                        send[5] = 0x05;
                    //                        ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                    //                    }
                    //                    //成功后
                    //                    PrintParamtClass printParamt = new PrintParamtClass() { name = PatchCarduser.Name, phone = PatchCarduser.phone, cardNo = PatchCarduser.cardNum };
                    //                    UseM5 useM5 = new UseM5(PrintClass.Reissue, printParamt);
                    //                    object error = "";
                    //                    if (!useM5.ConnState(ref error))
                    //                    {
                    //                        MessageBox.Show(error.ToString());
                    //                    }
                    //                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                    //                    {
                    //                        MainControl mainControl = new MainControl(page);
                    //                        page.Grid.Children.Clear();
                    //                        page.Grid.Children.Add(mainControl);
                    //                        TransactionSucessPage transactionSucessPage = new TransactionSucessPage();
                    //                        transactionSucessPage.mainPage = page;
                    //                        TransactionSucess2ShowControl transactionSucessShowControls = new TransactionSucess2ShowControl(transactionSucessPage);
                    //                        transactionSucessPage.GRID.Children.Add(transactionSucessShowControls);
                    //                        transactionSucessPage.ShowDialog();
                    //                        Times = 32;
                    //                        timer.Abort();
                    //                    }));
                    //                }

                    //            }
                    //        }
                    //    }
                    //    else 
                    //    {
                    //        this.Dispatcher.BeginInvoke(new System.Action(delegate
                    //        {
                    //            MainControl mainControl = new MainControl(page);
                    //            //page.Grid.Children.Clear();
                    //            page.Grid.Children.Add(mainControl);
                    //        }));
                    //        errorPage errorPage = new errorPage(errormsg.ToString());
                    //        errorPage.mainPage = page;
                    //        errorPage.ShowDialog();
                    //    }
                    //}
                    #endregion
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CardNum = null;
                timer = new Thread(new ThreadStart(AddTimes));
                timer.IsBackground = true;
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Card_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Card.Text.Trim().Length == 10)
            {
                CardNum = Card.Text.Trim();
                Card.Clear();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Abort();
        }
    }
}
