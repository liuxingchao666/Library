using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfApp2.BLL;
using WpfApp2.Controls;
using WpfApp2.Controls.ShowControl;
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class ShowControlViewModel : NotificationObject
    {
        public object Page;
        public ShowControlViewModel(object page)
        {
            Task.Run(() =>
            {
                Page = page;
                //try
                //{
                //    HandleidentitionPage handleidentition = page as HandleidentitionPage;
                //    GetUserPhone userPhone = new GetUserPhone();
                //    object errorMsg = handleidentition.user.CardNo;
                //    if (userPhone.GetPhone(ref errorMsg))
                //    {
                //        phone = errorMsg.ToString();
                //    }
                //} catch { }
                Remark = null;
                bstate = Visibility.Hidden;
                try
                {
                    if (ServerSeting.connState)

                    {
                        ENDState = Visibility.Hidden;
                        endState = Visibility.Hidden;
                        PICstate = Visibility.Hidden;
                        PICState = Visibility.Hidden;
                        MainPage p = page as MainPage;
                        if (!string.IsNullOrEmpty(p.CardNum))
                        {
                            Task.Run(() =>
                            {
                                object errorMsg = null;
                                GetRenewList renewList = new GetRenewList(p.CardNum, null);
                                if (GetRenewList.GetRenews(ref errorMsg))
                                {
                                    Me = errorMsg as List<ArchivesInfo>;
                                    Remark = new List<ArchivesInfo>();
                                    if (Me.Count == 0)
                                    {
                                        PICSJState = Visibility.Visible;
                                        PICSJstate = Visibility.Visible;
                                        GRIDstate = Visibility.Hidden;
                                        GRIDState = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        PICSJState = Visibility.Hidden;
                                        PICSJstate = Visibility.Hidden;
                                        GRIDstate = Visibility.Visible;
                                        GRIDState = Visibility.Visible;
                                    }
                                }
                                else
                                {
                                    PICSJState = Visibility.Visible;
                                    PICSJstate = Visibility.Visible;
                                    GRIDstate = Visibility.Hidden;
                                    GRIDState = Visibility.Hidden;
                                }
                            });
                        }
                    }
                    else
                    {
                        PICstate = Visibility.Visible;
                        GRIDstate = Visibility.Hidden;
                        PICSJState = Visibility.Hidden;
                        PICSJstate = Visibility.Hidden;
                    }
                }
                catch { }
            });
        }

        private string phone { get; set; }
        public string Phone
        {
            get { return string.IsNullOrEmpty(phone) ? "请输入您的联系电话" : phone; }
            set
            {
                phone = value;

                HandleidentitionPage handleidentition = Page as HandleidentitionPage;
                handleidentition.user.phone = phone;

                this.RaisePropertyChanged(() => Phone);
            }
        }
        private string phones { get; set; }
        public string Phones
        {
            get { return string.IsNullOrEmpty(phones) ? "请输入您的联系电话" : phones; }
            set
            {
                phones = value;
                ReportLossIdentitionPage handleidentition = Page as ReportLossIdentitionPage;
                handleidentition.user.phone = phones;
                this.RaisePropertyChanged(() => Phones);
            }
        }
        private Visibility bstate { get; set; }
        public Visibility Bstate
        {
            get { return bstate; }
            set
            {
                bstate = value;
                this.RaisePropertyChanged(() => Bstate);
            }
        }
        private Visibility endState { get; set; }
        public Visibility ENDState
        {
            get { return endState; }
            set
            {
                value = endState;
                this.RaisePropertyChanged(() => ENDState);
            }
        }
        private Visibility GRIDstate { get; set; }
        public Visibility GRIDState
        {
            get { return GRIDstate; }
            set
            {
                value = GRIDstate;
                this.RaisePropertyChanged(() => GRIDState);
            }
        }
        private Visibility PICstate { get; set; }
        public Visibility PICState
        {
            get { return PICstate; }
            set
            {
                value = PICstate;
                this.RaisePropertyChanged(() => PICState);
            }
        }
        private Visibility PICSJstate { get; set; }
        public Visibility PICSJState
        {
            get { return PICSJstate; }
            set
            {
                value = PICSJstate;
                this.RaisePropertyChanged(() => PICSJState);
            }
        }
        private ICommand PICClickcomand { get; set; }
        public ICommand PICClickComand
        {
            get
            {
                return PICClickcomand ?? (PICClickcomand = new DelegateCommand(getconn));
            }
        }

        private string wxts { get; set; }
        public string WXTS
        {
            get { return string.IsNullOrEmpty(wxts) ? "(温馨提示：手机号限制11位数)" : wxts; }
            set
            {
                wxts = value;
                this.RaisePropertyChanged(() => WXTS);
            }
        }
        public void getconn()
        {
            if (VerificationConn.GetVerification())
            {
                PICstate = Visibility.Hidden;
                GRIDstate = Visibility.Visible;
                if (string.IsNullOrEmpty(TestStr) || TestStr == "请输入查询所借的书籍名或作家")
                {
                    object errorMsg = null;
                    MainPage p = Page as MainPage;
                    GetRenewList renewList = new GetRenewList(p.CardNum, null);
                    if (GetRenewList.GetRenews(ref errorMsg))
                    {
                        Me = errorMsg as List<ArchivesInfo>;
                    }
                }
            }
            else
            {
                PICstate = Visibility.Visible;
                GRIDstate = Visibility.Hidden;
            }
        }
        /// <summary>
        /// 选中续借行
        /// </summary>

        private List<ArchivesInfo> me = new List<ArchivesInfo>();
        public List<ArchivesInfo> Me
        {
            get { return me; }
            set
            {
                me = value;
                this.RaisePropertyChanged(() => Me);
            }
        }
        private List<ArchivesInfo> remark = new List<ArchivesInfo>();
        public List<ArchivesInfo> Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged(() => Remark);
            }
        }
        private string pic { get; set; }
        public string PIC
        {
            get { return pic; }
            set
            {
                pic = value;
                this.RaisePropertyChanged(() => PIC);
            }
        }
        /// <summary>
        /// 确认
        /// </summary>
        private ICommand _OkCommond;
        public ICommand OkCommond
        {
            get
            {
                return _OkCommond ?? (_OkCommond = new DelegateCommand<UserControl>((data) =>
                {
                    OkAction(data);
                }));
            }
        }
        private string colorf { get; set; }
        public string colorF
        {
            get { return string.IsNullOrEmpty(colorf) ? "LightBlue" : "Red"; }
            set
            {
                colorf = value;
                this.RaisePropertyChanged(() => colorF);
            }
        }


        private ICommand printCommond;
        public ICommand PrintCommond
        {
            get
            {
                return printCommond ?? (printCommond = new DelegateCommand<RenewListControl>((data) =>
                {
                    print(data);
                }));
            }
        }
        /// <summary>
        /// 关闭弹出窗
        /// </summary>
        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new DelegateCommand<UserControl>((data) =>
                {
                    BackAction(data);
                }));
            }
        }
        /// <summary>
        /// 返回首页
        /// </summary>
        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return _BackCommand ?? (_BackCommand = new DelegateCommand<UserControl>((data) =>
                {
                    RollBackAction(data);
                }));
            }
        }
        /// <summary>
        /// 点击复选框
        /// </summary>
        private ICommand _ClikCommand;
        public ICommand ClikCommand
        {
            get
            {
                return _ClikCommand ?? (_ClikCommand = new DelegateCommand<RenewListControl>((data) =>
                {
                    SelectRow(data);
                }));
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        private ICommand _SelectAllCommond;
        public ICommand SelectAllCommond
        {
            get
            {
                return _SelectAllCommond ?? (_SelectAllCommond = new DelegateCommand<RenewListControl>((data) =>
                {
                    SelectAll(data);
                }));
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        private ICommand _BtnCommond;
        public ICommand btnCommond
        {
            get
            {
                return _BtnCommond ?? (_BtnCommond = new DelegateCommand<RenewListControl>((data) =>
                {
                    BtnAction(data);
                }));
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        private ICommand _ShowCommond;
        public ICommand ShowCommond
        {
            get
            {
                return _ShowCommond ?? (_ShowCommond = new DelegateCommand<DataGrid>((data) =>
                {
                    ShowAction(data);
                }));
            }
        }
        /// <summary>
        /// 单行续借
        /// </summary>
        private ICommand _XJCommond;
        public ICommand XJCommond
        {
            get
            {
                return _XJCommond ?? (_XJCommond = new DelegateCommand<DataGrid>((data) =>
                {
                    XJShow(data);
                }));
            }
        }
        private ICommand _VSCommond;
        public ICommand VSCommond
        {
            get
            {
                return _VSCommond ?? (_VSCommond = new DelegateCommand<DataGrid>((data) =>
                {
                    VSShow(data);
                }));
            }
        }
        /// <summary>
        /// 一键续借
        /// </summary>
        private ICommand _OnekeyCommond;
        public ICommand OnekeyCommond
        {
            get
            {
                return _OnekeyCommond ?? (_OnekeyCommond = new DelegateCommand<RenewListControl>((data) =>
                {
                    OnekeyAction(data);
                }));
            }
        }
        /// <summary>
        /// 页码
        /// </summary>
        private string inputPage { get; set; }
        public string InputPage
        {
            get { return string.IsNullOrEmpty(inputPage) ? "1" : inputPage; }
            set
            {
                inputPage = value;
                RaisePropertyChanged("InputPage");
            }
        }
        /// <summary>
        /// 查询参数
        /// </summary>
        private string testStr { get; set; }
        public string TestStr
        {
            get { return testStr; }
            set
            {
                testStr = value;
                RaisePropertyChanged("TestStr");
            }
        }
        /// <summary>
        /// 弹出框取消
        /// </summary>
        /// <param name="control"></param>
        public void BackAction(UserControl control)
        {
            PrintParamtClass @class1;
            UseM5 useM5;
            MainControl mainPage;

            switch (control.Name)
            {
                case "RenewPage":
                    RenewPage r = Page as RenewPage;
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                    r.timer.Abort();

                    r.Close();
                    break;
                case "HandleidentitionPage":
                    HandleidentitionPage H = Page as HandleidentitionPage;
                    H.timer.Abort();
                    H.mainPage.Grid.Children.Clear();
                    MainControl mainControl = new MainControl(H.mainPage);
                    H.mainPage.Grid.Children.Add(mainControl);
                    break;
                case "PayMentPage":
                    ServerSeting.HostNeedMoney = 0;
                    PayMentPage p = Page as PayMentPage;
                    p.timer.Abort();
                    ServerSeting.LastCardNum = null;
                    mainPage = new MainControl(p.mainPage);
                    p.mainPage.Grid.Children.Add(mainPage);
                    if (ServerSeting.transactionRecordInfos != null && ServerSeting.transactionRecordInfos.Count > 0)
                    {
                        @class1 = new PrintParamtClass() { name = p.user.Name, phone = p.user.phone, cardNo = p.user.cardNum };
                        useM5 = new UseM5(PrintClass.errorLog, @class1);
                        object error = "";
                        if (!useM5.ConnState(ref error))
                        {
                            errorPage errorPage = new errorPage(error.ToString());
                            errorPage.ShowDialog();
                            break;
                        };
                    }
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                    break;
                case "PayMentPage1":
                    ServerSeting.HostNeedMoney = 0;
                    PayMentPage p1 = Page as PayMentPage;
                    p1.timer.Abort();
                    ServerSeting.LastCardNum = null;
                    p1.GRID.Children.Clear();
                    mainPage = new MainControl(p1.mainPage);
                    p1.mainPage.Grid.Children.Add(mainPage);
                    if (ServerSeting.transactionRecordInfos != null && ServerSeting.transactionRecordInfos.Count > 0)
                    {
                        @class1 = new PrintParamtClass() { name = p1.user.Name, phone = p1.user.phone, cardNo = p1.user.cardNum };
                        useM5 = new UseM5(PrintClass.errorLog, @class1);
                        object error = "";
                        if (!useM5.ConnState(ref error))
                        {
                            errorPage errorPage = new errorPage(error.ToString());
                            errorPage.ShowDialog();
                            break;
                        };
                    }
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                    break;
                case "PayMentPage2":
                    PayMentPage p2 = Page as PayMentPage;
                    p2.Times = 130;
                    p2.timer.Abort();
                    p2.GRID.Children.Clear();
                    ServerSeting.HostNeedMoney = 0;
                    mainPage = new MainControl(p2.mainPage);
                    p2.mainPage.Grid.Children.Add(mainPage);
                    TransactionSucessPage transactionPage = new TransactionSucessPage();
                    TransactionSucess2ShowControl transactionSucess2ShowControl = new TransactionSucess2ShowControl(transactionPage);
                    transactionSucess2ShowControl.printParamt = new PrintParamtClass() { name = p2.user.Name, phone = p2.user.phone, cardNo = p2.user.cardNum };
                    transactionPage.GRID.Children.Add(transactionSucess2ShowControl);
                    transactionPage.ShowDialog();
                    break;
                case "Renew2Page":
                    Renew2Page r2 = Page as Renew2Page;
                    r2.Close();
                    break;
                case "Renew3Page":
                    Renew3Page r3 = Page as Renew3Page;
                    r3.Close();
                    break;
                case "ReportLossIdentitionPage":
                    ReportLossIdentitionPage RL = Page as ReportLossIdentitionPage;
                    RL.Times = 60;
                    RL.timer.Abort();
                    RL.mainPage.Grid.Children.Clear();
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                    mainPage = new MainControl(RL.mainPage);
                    RL.mainPage.Grid.Children.Add(mainPage);
                    break;
                case "ReportLossSuccessPage":
                    ReportLossSuccessPage RLP = Page as ReportLossSuccessPage;
                    RLP.Times = 60;
                    RLP.timer.Abort();
                    RLP.Close();
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                    mainPage = new MainControl(RLP.mainPage);
                    RLP.mainPage.Grid.Children.Add(mainPage);
                    break;
                case "TransactionSucessPage":
                    TransactionSucessPage TS = Page as TransactionSucessPage;
                    TS.Times = 60;
                    TS.timer.Abort();
                    TS.Close();
                    if (TS.mainPage == null)
                    {
                        break;
                    }
                    UserMessage userMessage = new UserMessage() { IdentificationCode = TS.cardData.CardNo };
                    HandleidentitionPage handleidentitionPage = new HandleidentitionPage(TS.mainPage);
                    object errorMsg = userMessage;
                    GetNowUserDAL getNowUserDAL = new GetNowUserDAL();
                    if (getNowUserDAL.GetPushDAL(ref errorMsg))
                    {
                        handleidentitionPage.ActionName = "个人信息";
                        userMessage = errorMsg as UserMessage;
                        userMessage.PIC = TS.cardData.PIC;
                        HandleidentitionMessagexaml messagexaml = new HandleidentitionMessagexaml(handleidentitionPage, userMessage);
                        handleidentitionPage.GRID.Children.Add(messagexaml);
                        TS.mainPage.Grid.Children.Add(handleidentitionPage);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 续借返回
        /// </summary>
        public void RollBackAction(UserControl data)
        {
            RenewListControl control = data as RenewListControl;
            control.thread.Abort();
            MainPage mainPage = Page as MainPage;
            MainControl mainControl = new MainControl(mainPage);
            mainPage.Grid.Children.Clear();
            mainPage.Grid.Children.Add(mainControl);
        }
        /// <summary>
        /// 单行选中
        /// </summary>
        /// <param name="data"></param>
        public void SelectRow(RenewListControl data)
        {
            DataGrid da = data.GRID;
            try
            {
                if (me == null || me.Count <= 0)
                    return;
                List<ArchivesInfo> list = new List<ArchivesInfo>();
                foreach (ArchivesInfo info in me)
                {
                    list.Add(info);
                }
                if (da.SelectedIndex < 0)
                    return;
                ArchivesInfo message = da.SelectedItem as ArchivesInfo;
                bool isSelectAll = true;
                ///展示数据源
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Id == message.Id)
                    {
                        if (!message.source.Contains("灰色"))
                        {
                            list[i].source = @"..\ControlImages\灰色选择.png";
                            list[i].isCheck = true;
                        }
                        else
                        {
                            list[i].source = @"..\ControlImages\未选择.png";
                            list[i].isCheck = false;
                        }
                    }
                }
                isSelectAll = true;
                foreach (ArchivesInfo info in list)
                {
                    if (info.source.Contains("未"))
                    {
                        isSelectAll = false;
                    }
                }
                me = null;
                me = list;
                Me = null;
                Me = list;
                BitmapImage imgSource;
                if (isSelectAll)
                {
                    imgSource = new BitmapImage(new Uri(@"..\ControlImages\闪勾.png", UriKind.Relative));
                    data.img.Source = imgSource;
                    data.img.Tag = 0;
                    da.Tag = 0;
                    data.Tag = 1;
                }
                else
                {
                    imgSource = new BitmapImage(new Uri(@"..\ControlImages\闪空.png", UriKind.Relative));
                    data.img.Source = imgSource;
                    data.img.Tag = 1;
                    da.Tag = 1;
                    data.Tag = 0;
                }
                me = list;
                Me = list;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex) { return; }
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="data"></param>
        public void SelectAll(RenewListControl data)
        {

            List<ArchivesInfo> list = data.GRID.ItemsSource as List<ArchivesInfo>;
            bool result = false;
            if (data.Tag.toInt() == 0)
            {
                result = true;
            }
            BitmapImage imgSource;
            ///展示数据操作
            for (int i = 0; i < list.Count; i++)
            {
                switch (int.Parse(data.Tag.ToString()))
                {
                    case 0:
                        result = true;
                        list[i].source = @"..\ControlImages\灰色选择.png";
                        list[i].isCheck = true;
                        break;
                    default:
                        result = false;
                        list[i].source = @"..\ControlImages\未选择.png";
                        list[i].isCheck = false;
                        break;
                }
            }
            if (result)
            {
                data.Tag = 1;
                imgSource = new BitmapImage(new Uri(@"..\ControlImages\闪勾.png", UriKind.Relative));
                data.img.Source = imgSource;
            }
            else
            {
                data.Tag = 0;
                imgSource = new BitmapImage(new Uri(@"..\ControlImages\闪空.png", UriKind.Relative));
                data.img.Source = imgSource;
            }
            me = null;
            me = list;
            Me = null;
            Me = list;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="data"></param>
        public void BtnAction(RenewListControl data)
        {
            switch (data.Tag)
            {
                case 0:
                    data.NewPage = inputPage.ToInt();
                    data.LoadPageData();
                    data.tet.Content = data.LoadPage.ToString();
                    break;
                case 1:
                    data.NewPage = 1;
                    data.LoadPageData();
                    data.tet.Content = 1.ToString();
                    break;
                case 2:
                    if (data.LoadPage == 1)
                    {
                        break;
                    }

                    data.NewPage -= 1;
                    data.LoadPageData();
                    data.tet.Content = data.LoadPage.ToString();
                    break;
                case 3:
                    if (data.LoadPage == data.MaxPage)
                    {
                        break;
                    }
                    data.NewPage += 1;
                    data.LoadPageData();
                    data.tet.Content = data.LoadPage.ToString();
                    break;
                default:

                    break;
            };
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="data"></param>
        public void ShowAction(DataGrid data)
        {
            MainPage p = Page as MainPage;
            object errorMsg = null;
            if (string.IsNullOrEmpty(TestStr) || TestStr.Trim() != "请输入查询所借的书籍名或作家")
            {
                GetRenewList renewList = new GetRenewList(p.CardNum, TestStr);
                if (GetRenewList.GetRenews(ref errorMsg))
                {
                    Me = errorMsg as List<ArchivesInfo>;
                }
            }

        }
        /// <summary>
        /// 续借点击
        /// </summary>
        /// <param name="data"></param>
        public void XJShow(DataGrid data)
        {
            MainPage page = Page as MainPage;
            ArchivesInfo message = data.SelectedItem as ArchivesInfo;
            if (message.SurplusRenewableTimes <= 0)
            {
                Renew3Page renew3Page = new Renew3Page();
                Renew3ShowControl renew3ShowControl = new Renew3ShowControl(renew3Page);
                renew3Page.user = page.user;
                renew3Page.GRID.Children.Add(renew3ShowControl);
                renew3Page.ShowDialog();
            }
            else
            {
                RenewActions renewActions = new RenewActions();
                object errormsg = new ActionErrormsg() { ICCardNum = page.CardNum, logids = new string[] { message.Id } };
                if (renewActions.GetActionState(ref errormsg))
                {
                    List<ActionErrormsg> g = errormsg as List<ActionErrormsg>;
                    for (int i = 0; i < Me.Count - 1; i++)
                    {
                        if (Me[i].Id == message.Id)
                        {
                            if (g[0].state)
                            {
                                Me[i].XJState = Visibility.Hidden;
                                Me[i].SState = Visibility.Visible;
                                Me[i].FState = Visibility.Hidden;
                            }
                            else
                            {
                                Me[i].XJState = Visibility.Hidden;
                                Me[i].SState = Visibility.Hidden;
                                Me[i].FState = Visibility.Visible;
                                Me[i].errorMsg = g[0].errorMsg;
                            }
                        }
                    }
                }
            }
            List<ArchivesInfo> m = Me;
            Me = null;
            Me = m;
        }
        public void VSShow(DataGrid data)
        {

            //MainPage page = Page as MainPage;
            //Renew3Page renew3Page = new Renew3Page();
            //Renew3ShowControl renew3ShowControl = new Renew3ShowControl(renew3Page);
            //renew3Page.user = page.user;
            //renew3Page.GRID.Children.Add(renew3ShowControl);
            //renew3Page.ShowDialog();
            ArchivesInfo archives = data.SelectedItem as ArchivesInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void OnekeyAction(RenewListControl data)
        {
            List<ArchivesInfo> checkList = new List<ArchivesInfo>();
            foreach (ArchivesInfo info in me)
            {
                if (info.source.Contains("灰色"))
                {
                    checkList.Add(info);
                }
            }
            MainPage page = Page as MainPage;
            RenewActions renewActions = new RenewActions();
            List<string> inlist = new List<string>();
            foreach (ArchivesInfo info in checkList)
            {
                inlist.Add(info.Id);
            }
            for (int i = 0; i < checkList.Count; i++)
            {
                if (checkList[i].SurplusRenewableTimes <= 0)
                {
                    checkList[i].XJState = Visibility.Hidden;
                    checkList[i].SState = Visibility.Hidden;
                    checkList[i].FState = Visibility.Visible;
                    checkList[i].errorMsg = "可续借次数已用完，如需再续借请走人工服务";
                }
            }
            for (int i = 0; i < Me.Count; i++)
            {
                for (int j = 0; j < checkList.Count; j++)
                {
                    if (Me[i].Id == checkList[j].Id && Me[i].SurplusRenewableTimes <= 0)
                    {
                        Me[i] = checkList[j];
                    }
                }
            }
            if (checkList.Count == 0)
            {
                return;
            }

            object errormsg = new ActionErrormsg() { ICCardNum = page.CardNum, logids = inlist.ToArray() };
            if (renewActions.GetActionState(ref errormsg))
            {
                List<ArchivesInfo> infos = new List<ArchivesInfo>();
                List<ActionErrormsg> g = errormsg as List<ActionErrormsg>;
                for (int i = 0; i < Me.Count; i++)
                {
                    foreach (ActionErrormsg action in g)
                    {
                        data.user.cardNum = action.ICCardNum;
                        if (Me[i].Id == action.id)
                        {
                            ArchivesInfo info = new ArchivesInfo();
                            info.ArchivesName = Me[i].ArchivesName;
                            info.EDTime = action.planReturnTime.ToString();
                            info.BSTime = Me[i].BSTime;
                            info.errorMsg = action.errorMsg;
                            info.barcode = action.BardCode;
                            info.num = i + 1;
                            if (!string.IsNullOrEmpty(info.errorMsg) && info.errorMsg.Contains("成功"))
                            {
                                info.color = "#0FD7A5";
                            }
                            else
                            {
                                info.color = "#ff764d";
                            }
                            if (action.state)
                            {
                                Me[i].XJState = Visibility.Hidden;
                                Me[i].SState = Visibility.Visible;
                                Me[i].FState = Visibility.Hidden;
                            }
                            else
                            {
                                Me[i].XJState = Visibility.Hidden;
                                Me[i].SState = Visibility.Hidden;
                                Me[i].FState = Visibility.Visible;
                                Me[i].errorMsg = action.errorMsg;
                            }

                            infos.Add(info);
                        }
                    }
                }
                bstate = Visibility.Visible;
                Bstate = Visibility.Visible;
                remark = infos;
                Remark = infos;
                List<ArchivesInfo> m = Me;
                for (int i = 0; i < m.Count; i++)
                {
                    m[i].source = @"..\ControlImages\未选择.png";
                }
                BitmapImage imgSource = new BitmapImage(new Uri(@"..\ControlImages\未选择.png", UriKind.Relative));
                data.img.Source = imgSource;
                data.img.Tag = 0;
                data.BGRID.Visibility = Visibility.Hidden;
                GRIDstate = Visibility.Hidden;
                data.datagrid.Visibility = Visibility.Visible;
                ENDState = Visibility.Visible;
                endState = Visibility.Visible;
                data.TH.Visibility = Visibility.Hidden;
                data.THH.Visibility = Visibility.Visible;
                data.T.Visibility = Visibility.Hidden;
                data.W.Visibility = Visibility.Hidden;
                data.X.Visibility = Visibility.Hidden;
                data.A.Visibility = Visibility.Hidden;
                data.S.Visibility = Visibility.Hidden;
                data.QX.Visibility = Visibility.Hidden;
                data.T1.Visibility = Visibility.Hidden;
                data.T2.Visibility = Visibility.Hidden;
                Me = null;
                Me = m;
                checkList.Clear();
                data.i = 60;
            }
        }
        public void getstring(string num, ref string newNum)
        {
            if (num != "" && num.Length == 2)
            {
                newNum += num;
            }
            else
            {
                newNum += num.Substring(num.Length - 2, 2);
                num = num.Substring(0, num.Length - 2);
                getstring(num, ref newNum);
            }
        }
        public void OkAction(UserControl control)
        {
            PayStep step = PayStep.FirstAdd;
            switch (control.Name)
            {
                //case "Renew2Page":
                //    Renew2Page r2 = Page as Renew2Page;
                //    r2.GRID.Children.Clear();
                //    RenewPayMent2ShowControl renew2ShowControl = new RenewPayMent2ShowControl(r2);
                //    r2.GRID.Children.Add(renew2ShowControl);

                // break;
                //case "Renew3Page":
                //    Renew3Page r3 = Page as Renew3Page;
                //    r3.Close();
                //    PayMentPage payMentPage = new PayMentPage(DealBusiness.Reissue, PayStep.FirstAdd,);
                //    payMentPage.user = r3.user;
                //    TransactionShowControl transactionShowControl = new TransactionShowControl(payMentPage);
                //    payMentPage.GRID.Children.Add(transactionShowControl);
                //    payMentPage.ShowDialog();
                //    break;
                case "HandleidentitionPage":
                    HandleidentitionPage page = Page as HandleidentitionPage;
                    CardData cardData = page.user;
                    page.timer.Abort();
                    if (!ServerSeting.connState)
                    {
                        MainControl mainControl = new MainControl(page.mainPage);
                        page.mainPage.Grid.Children.Clear();
                        page.mainPage.Grid.Children.Add(mainControl);
                        ServerSeting.ISAdd = true;
                        ServerSeting.SYSleepTimes = 60;
                        return;
                    }
                    if (string.IsNullOrEmpty(cardData.phone))
                    {
                        WXTS = "(温馨提示：手机号不可为空)";
                        return;
                    }
                    Regex regex = new Regex("^1[34578]\\d{9}$");
                    if (!regex.IsMatch(cardData.phone))
                    {
                        WXTS = "(温馨提示：手机号格式不对)";
                        return;
                    }
                  //  page.mainPage.mainControl.timer.Abort();
                    if (!ServerSeting.hairpin.serialPort.IsOpen)
                    {
                        MainControl mainControl = new MainControl(page.mainPage);
                        page.mainPage.Grid.Children.Clear();
                        page.mainPage.Grid.Children.Add(mainControl);
                        ServerSeting.ISAdd = false;
                        errorPage errorPage = new errorPage("办卡设备出错，请检查设备连接");
                        errorPage.mainPage = page.mainPage;
                        errorPage.ShowDialog();
                        break;
                    }
                    if (!ServerSeting.bFeeder.serialPort.IsOpen)
                    {
                        MainControl mainControl = new MainControl(page.mainPage);
                        page.mainPage.Grid.Children.Clear();
                        page.mainPage.Grid.Children.Add(mainControl);
                        ServerSeting.ISAdd = false;
                        errorPage errorPage = new errorPage("进钞机设备出错，请检查设备连接");
                        errorPage.mainPage = page.mainPage;
                        errorPage.ShowDialog();
                        break;
                    }
                    try
                    {
                        lock (ServerSeting.CcardBLL)
                        {
                            try
                            {
                                ///支付界面断开可能没
                                ServerSeting.CcardBLL.rfidReader.DisConnect();
                            }
                            catch { }
                            if (!ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                            {
                                MainControl mainControl = new MainControl(page.mainPage);
                                page.mainPage.Grid.Children.Clear();
                                page.mainPage.Grid.Children.Add(mainControl);
                                ServerSeting.ISAdd = false;
                                errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查设备连接1");
                                errorPage.mainPage = page.mainPage;
                                errorPage.ShowDialog();
                                break;
                            }
                            ServerSeting.CcardBLL.rfidReader.DisConnect();
                        }
                    }
                    catch
                    {

                    }

                    //获取等级判断
                    GetDeposit getDeposit = new GetDeposit();
                    ///注销用户---办卡判断
                    object errorMsg = cardData;

                    if (getDeposit.Getdesosit(ref errorMsg))
                    {
                       
                        ReturnInfo info = errorMsg as ReturnInfo;
                        page.user.Despoit = info.Deposit;
                        if (info.SuccessOrFalse)
                        {
                            int BXJ = 0;
                            ///判断办卡是否必须金额
                            GetMinimumDeposit getMinimumDeposit = new GetMinimumDeposit();
                            if (!getMinimumDeposit.Getdesosit(ref errorMsg))
                            {
                                MainControl mainControl = new MainControl(page.mainPage);
                                page.mainPage.Grid.Children.Clear();
                                page.mainPage.Grid.Children.Add(mainControl);
                                ServerSeting.ISAdd = false;
                                errorPage errorPage = new errorPage(errorMsg.ToString());
                                errorPage.mainPage = page.mainPage;
                                errorPage.ShowDialog();
                                break;
                            }
                            BXJ = errorMsg.toInt();
                            page.user.cost = BXJ;
                            ///二次办卡判断
                            if (info.Deposit != 0)
                            {
                              
                                info.CostDeposit = info.Deposit + BXJ;
                                step = PayStep.SecondAdd;
                                PayMentPage payMent = new PayMentPage(DealBusiness.Add, step, page.mainPage);
                                payMent.info = info;
                                payMent.user = page.user;
                                Transaction2ShowControl transaction2ShowControl = new Transaction2ShowControl(payMent);
                                payMent.GRID.Children.Add(transaction2ShowControl);
                                page.mainPage.Grid.Children.Clear();
                                page.mainPage.Grid.Children.Add(payMent);
                                break;
                            }
                            else
                            {
                                if (BXJ != 0)
                                {
                                   
                                    info.CostDeposit = BXJ;
                                    step = PayStep.FirstAdd;
                                    PayMentPage payMent = new PayMentPage(DealBusiness.Add, step, page.mainPage);
                                    payMent.info = info;
                                    payMent.user = page.user;
                                    Transaction2ShowControl transaction2ShowControl = new Transaction2ShowControl(payMent);
                                    payMent.GRID.Children.Add(transaction2ShowControl);
                                    page.mainPage.Grid.Children.Clear();
                                    page.mainPage.Grid.Children.Add(payMent);
                                    break;
                                }
                                else
                                {
                                   
                                    if (ServerSeting.hairpin.serialPort.IsOpen)
                                    {
                                        byte[] send = new byte[6];
                                        send[0] = 0x02;
                                        send[1] = 0x44;
                                        send[2] = 0x48;
                                        send[3] = 0x03;
                                        send[4] = 0x0d;
                                        send[5] = 0x05;
                                        ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                    }
                                    Thread.Sleep(300);

                                    if (string.IsNullOrEmpty(ServerSeting.ICPort))
                                    {
                                        if (!ServerSeting.CcardBLL.conn())
                                        {
                                            ServerSeting.HostNeedMoney = 0;
                                            MainControl mainControll = new MainControl(page.mainPage);
                                            page.mainPage.Grid.Children.Clear();
                                            page.mainPage.Grid.Children.Add(mainControll);
                                            ServerSeting.ISAdd = false;
                                            errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查连接");
                                            errorPage.mainPage = page.mainPage;
                                            errorPage.ShowDialog();
                                            break;
                                        }
                                    }

                                    try
                                    {
                                        ServerSeting.CcardBLL.rfidReader.DisConnect();
                                    }
                                    catch { }
                                    if (!ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                                    {
                                        MainControl mainControls = new MainControl(page.mainPage);
                                        page.mainPage.Grid.Children.Clear();
                                        page.mainPage.Grid.Children.Add(mainControls);
                                        ServerSeting.ISAdd = false;
                                        errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查设备连接");
                                        errorPage.mainPage = page.mainPage;
                                        errorPage.ShowDialog();
                                        break;
                                    }
                                    else
                                    {
                                        ICcardMessage ccardMessage = new ICcardMessage();
                                        if (ServerSeting.CcardBLL.ReadCard(ref ccardMessage))
                                        {
                                            string cardnum = "";
                                            getstring(ccardMessage.CardNum, ref cardnum);
                                            page.user.cardNum = cardnum;
                                            ServerSeting.TempCardNum = cardnum;
                                            ServerSeting.CcardBLL.rfidReader.DisConnect();
                                            if (!string.IsNullOrEmpty(ServerSeting.LastCardNum))
                                            {
                                                if (ServerSeting.LastCardNum == cardnum)
                                                {
                                                    if (ServerSeting.hairpin.serialPort.IsOpen)
                                                    {
                                                        byte[] send = new byte[6];
                                                        send[0] = 0x02;
                                                        send[1] = 0x43;
                                                        send[2] = 0x50;
                                                        send[3] = 0x03;
                                                        send[4] = 0x12;
                                                        send[5] = 0x05;
                                                        ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                                    }
                                                    MainControl mainControle = new MainControl(page.mainPage);
                                                    page.mainPage.Grid.Children.Clear();
                                                    page.mainPage.Grid.Children.Add(mainControle);
                                                    ServerSeting.ISAdd = false;
                                                    errorPage errorPage = new errorPage("操作失败,请联系工作人员");
                                                    errorPage.mainPage = page.mainPage;
                                                    errorPage.ShowDialog();
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ServerSeting.CcardBLL.rfidReader.DisConnect();
                                            MainControl mainControle = new MainControl(page.mainPage);
                                            page.mainPage.Grid.Children.Clear();
                                            page.mainPage.Grid.Children.Add(mainControle);
                                            ServerSeting.ISAdd = false;
                                            errorPage errorPage = new errorPage("获取读者卡卡号失败");
                                            errorPage.mainPage = page.mainPage;
                                            errorPage.ShowDialog();
                                            break;
                                        }
                                    }
                                    AddUserCard addUserCard = new AddUserCard();
                                    page.user.cost = 0;
                                    object errormsgs = page.user;
                                    if (!addUserCard.AddCard(ref errormsgs))
                                    {
                                        MainControl mainControle = new MainControl(page.mainPage);
                                        page.mainPage.Grid.Children.Clear();
                                        page.mainPage.Grid.Children.Add(mainControle);
                                        ServerSeting.ISAdd = false;
                                        errorPage errorPage = new errorPage(errormsgs.ToString());
                                        errorPage.mainPage = page.mainPage;
                                        errorPage.ShowDialog();
                                        if (ServerSeting.hairpin.serialPort.IsOpen)
                                        {
                                            byte[] send = new byte[6];
                                            send[0] = 0x02;
                                            send[1] = 0x43;
                                            send[2] = 0x50;
                                            send[3] = 0x03;
                                            send[4] = 0x12;
                                            send[5] = 0x05;
                                            ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        ServerSeting.LastCardNum = ServerSeting.TempCardNum;
                                    }
                                    if (ServerSeting.bFeeder.serialPort.IsOpen)
                                    {
                                        byte[] send = new byte[6];
                                        send[0] = 0x02;
                                        send[1] = 0x45;
                                        send[2] = 0x53;
                                        send[3] = 0x03;
                                        send[4] = 0x17;
                                        send[5] = 0x05;
                                        ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                    }
                                    ServerSeting.ISAdd = false;
                                    ///弹出成功界面获取卡号在办卡
                                    page.mainPage.Grid.Children.Clear();
                                    MainControl mainControl = new MainControl(page.mainPage);
                                    page.mainPage.Grid.Children.Add(mainControl);
                                    ServerSeting.ISAdd = false;
                                    TransactionSucessPage transactionSucessPage = new TransactionSucessPage();
                                    transactionSucessPage.mainPage = page.mainPage;
                                    transactionSucessPage.cardData = page.user;
                                    TransactionSucessShowControl transactionSucessShowControl = new TransactionSucessShowControl(transactionSucessPage, 0);
                                    PrintParamtClass printParamt = new PrintParamtClass() { PIC = page.user.PIC, IdCard = page.user.CardNo, name = page.user.Name, phone = page.user.phone, cardNo = errormsgs.ToString() };
                                    transactionSucessShowControl.printParamt = printParamt;
                                    transactionSucessPage.GRID.Children.Add(transactionSucessShowControl);
                                    transactionSucessPage.ShowDialog();
                                    break;
                                }
                            }
                        }
                        else
                        {
                           
                            MainControl mainControl = new MainControl(page.mainPage);
                            page.mainPage.Grid.Children.Clear();
                            page.mainPage.Grid.Children.Add(mainControl);
                            errorPage errorPage = new errorPage(info.errorMsg.ToString());
                            errorPage.mainPage = page.mainPage;
                            errorPage.ShowDialog();
                        }
                    }
                    else
                    {
                      
                        string error = null;
                        try
                        {
                            ReturnInfo info = errorMsg as ReturnInfo;
                            error = info.errorMsg.ToString();
                        }
                        catch
                        {
                            error = errorMsg.ToString();
                        }
                        finally
                        {
                            MainControl mainControl = new MainControl(page.mainPage);
                            page.mainPage.Grid.Children.Clear();
                            page.mainPage.Grid.Children.Add(mainControl);
                            errorPage errorPage = new errorPage(error);
                            errorPage.mainPage = page.mainPage;
                            errorPage.ShowDialog();
                        }
                    }
                    break;
                case "ReportLossIdentitionPage":
                    ReportLossIdentitionPage reportLossIdentitionPage = Page as ReportLossIdentitionPage;
                    // reportLossIdentitionPage.Times = 60;
                    reportLossIdentitionPage.timer.Abort();
                    reportLossIdentitionPage.mainPage.Grid.Children.Clear();
                    MainControl mainControlr = new MainControl(reportLossIdentitionPage.mainPage);
                    reportLossIdentitionPage.mainPage.Grid.Children.Add(mainControlr);
                    if (!ServerSeting.connState)
                    {
                        return;
                    }
                    ServerSeting.ISAdd = false;
                    ReportLossDAL lossDAL = new ReportLossDAL();
                    object errormsg = reportLossIdentitionPage.user;
                    if (lossDAL.AddCard(ref errormsg))
                    {
                        ServerSeting.ISAdd = false;
                        ReportLossSuccessPage report = new ReportLossSuccessPage(reportLossIdentitionPage.mainPage);
                        report.user = reportLossIdentitionPage.user;
                        ReportLossSuccessShowControl showControl = new ReportLossSuccessShowControl(report, DealClass.Direct);
                        report.GRID.Children.Add(showControl);
                        report.ShowDialog();
                    }
                    else
                    {
                        if (errormsg.ToString().Contains("301"))
                        {
                            ServerSeting.ISAdd = false;
                            ReportLossSuccessPage report = new ReportLossSuccessPage(reportLossIdentitionPage.mainPage);
                            report.user = reportLossIdentitionPage.user;
                            ReportLossSuccessShowControl showControl = new ReportLossSuccessShowControl(report, DealClass.indirect);
                            report.GRID.Children.Add(showControl);
                            report.ShowDialog();
                        }
                        else
                        {
                            ServerSeting.ISAdd = false;
                            MainControl mainControl = new MainControl(reportLossIdentitionPage.mainPage);
                            reportLossIdentitionPage.mainPage.Grid.Children.Clear();
                            reportLossIdentitionPage.mainPage.Grid.Children.Add(mainControl);
                            ServerSeting.ISAdd = false;
                            errorPage errorPage = new errorPage(errormsg.ToString());
                            errorPage.mainPage = reportLossIdentitionPage.mainPage;
                            errorPage.ShowDialog();
                        }
                    }
                    break;
                case "ReportLossSuccessPage":

                    ReportLossSuccessPage successPage = Page as ReportLossSuccessPage;
                    successPage.timer.Abort();
                    successPage.Close();
                    try
                    {
                        lock (this)
                        {
                            if (ServerSeting.warehouseIsNull)
                            {
                                MainControl mainControl = new MainControl(successPage.mainPage);
                                successPage.mainPage.Grid.Children.Clear();
                                successPage.mainPage.Grid.Children.Add(mainControl);
                                ServerSeting.ISAdd = false;
                                errorPage errorPage = new errorPage("吐卡机卡储量为空！！请加卡");
                                errorPage.ShowDialog();
                                break;
                            }
                            try
                            {
                                ServerSeting.CcardBLL.rfidReader.DisConnect();
                            }
                            catch { }
                            if (string.IsNullOrEmpty(ServerSeting.ICPort) || !ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                            {
                                MainControl mainControl = new MainControl(successPage.mainPage);
                                successPage.mainPage.Grid.Children.Clear();
                                successPage.mainPage.Grid.Children.Add(mainControl);
                                ServerSeting.ISAdd = false;
                                errorPage errorPage = new errorPage("IC卡扫描异常");
                                errorPage.ShowDialog();
                                break;
                            }
                        }

                        ServerSeting.CcardBLL.rfidReader.DisConnect();
                    }
                    catch { }
                    //查询补办手续费
                    errormsg = "";
                    GetReplacementCost getReplacementCost = new GetReplacementCost();
                    if (getReplacementCost.Getdesosit(ref errormsg))
                    {
                        ReturnInfo info = errormsg as ReturnInfo;
                        successPage.user.cost = info.CostDeposit;
                        if (info.CostDeposit > 0)
                        {
                            if (!ServerSeting.MConnState)
                            {
                                MainControl mainControl = new MainControl(successPage.mainPage);
                                successPage.mainPage.Grid.Children.Clear();
                                successPage.mainPage.Grid.Children.Add(mainControl);
                                ServerSeting.ISAdd = false;
                                errorPage errorPage = new errorPage("进钞机设备连接异常，请检查设备连接");
                                errorPage.mainPage = successPage.mainPage;
                                errorPage.ShowDialog();
                                break;
                            }
                            PayMentPage mentPage = new PayMentPage(DealBusiness.Reissue, PayStep.Reissue, successPage.mainPage);
                            mentPage.info = info;
                            mentPage.user = successPage.user;
                            Transaction3ShowControl transaction3ShowControl = new Transaction3ShowControl(mentPage);
                            
                            mentPage.GRID.Children.Add(transaction3ShowControl);
                            successPage.mainPage.Grid.Children.Clear();
                   
                            successPage.mainPage.Grid.Children.Add(mentPage);
                        }
                        else
                        {
                            //补办直接成功
                            //读卡

                            if (ServerSeting.hairpin.serialPort.IsOpen)
                            {
                                byte[] send = new byte[6];
                                send[0] = 0x02;
                                send[1] = 0x44;
                                send[2] = 0x48;
                                send[3] = 0x03;
                                send[4] = 0x0d;
                                send[5] = 0x05;
                                ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                            }

                            Thread.Sleep(300);

                            if (string.IsNullOrEmpty(ServerSeting.ICPort))
                            {
                                if (!ServerSeting.CcardBLL.conn())
                                {
                                    ServerSeting.HostNeedMoney = 0;
                                    MainControl mainControll = new MainControl(successPage.mainPage);
                                    successPage.mainPage.Grid.Children.Clear();
                                    successPage.mainPage.Grid.Children.Add(mainControll);
                                    ServerSeting.ISAdd = false;
                                    errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查连接");
                                    errorPage.mainPage = successPage.mainPage;
                                    errorPage.ShowDialog();
                                    break;
                                }
                            }

                            try
                            {
                                ServerSeting.CcardBLL.rfidReader.DisConnect();
                            }
                            catch { }

                            if (!ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                            {
                                MainControl mainControls = new MainControl(successPage.mainPage);
                                successPage.mainPage.Grid.Children.Clear();
                                successPage.mainPage.Grid.Children.Add(mainControls);
                                ServerSeting.ISAdd = false;
                                errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查设备连接");
                                errorPage.mainPage = successPage.mainPage;
                                errorPage.ShowDialog();
                                break;
                            }
                            else
                            {
                                ICcardMessage ccardMessage = new ICcardMessage();
                                if (ServerSeting.CcardBLL.ReadCard(ref ccardMessage))
                                {
                                    string cardnum = "";
                                    getstring(ccardMessage.CardNum, ref cardnum);
                                    successPage.user.cardNum = cardnum;
                                    ServerSeting.TempCardNum = cardnum;
                                    ServerSeting.CcardBLL.rfidReader.DisConnect();
                                    if (!string.IsNullOrEmpty(ServerSeting.LastCardNum))
                                    {
                                        if (ServerSeting.LastCardNum == cardnum)
                                        {
                                            if (ServerSeting.hairpin.serialPort.IsOpen)
                                            {
                                                byte[] send = new byte[6];
                                                send[0] = 0x02;
                                                send[1] = 0x43;
                                                send[2] = 0x50;
                                                send[3] = 0x03;
                                                send[4] = 0x12;
                                                send[5] = 0x05;
                                                ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                            }
                                            MainControl mainControle = new MainControl(successPage.mainPage);
                                            successPage.mainPage.Grid.Children.Clear();
                                            successPage.mainPage.Grid.Children.Add(mainControle);
                                            ServerSeting.ISAdd = false;
                                            errorPage errorPage = new errorPage("操作失败,请联系工作人员");
                                            errorPage.mainPage = successPage.mainPage;
                                            errorPage.ShowDialog();
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    ServerSeting.CcardBLL.rfidReader.DisConnect();
                                    MainControl mainControle = new MainControl(successPage.mainPage);
                                    successPage.mainPage.Grid.Children.Clear();
                                    successPage.mainPage.Grid.Children.Add(mainControle);
                                    ServerSeting.ISAdd = false;
                                    errorPage errorPage = new errorPage("获取读者卡卡号失败");
                                    errorPage.mainPage = successPage.mainPage;
                                    errorPage.ShowDialog();
                                    break;
                                }
                            }
                            //补办
                            MakeUpDAL makeUpDAL = new MakeUpDAL();
                            errormsg = successPage.user;
                            if (!makeUpDAL.MakeUpCard(ref errormsg))
                            {
                                MainControl mainControle = new MainControl(successPage.mainPage);
                                successPage.mainPage.Grid.Children.Clear();
                                successPage.mainPage.Grid.Children.Add(mainControle);
                                ServerSeting.ISAdd = false;
                                if (ServerSeting.hairpin.serialPort.IsOpen)
                                {
                                    byte[] send = new byte[6];
                                    send[0] = 0x02;
                                    send[1] = 0x43;
                                    send[2] = 0x50;
                                    send[3] = 0x03;
                                    send[4] = 0x12;
                                    send[5] = 0x05;
                                    ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                }
                                errorPage errorPage = new errorPage(errormsg.ToString());
                                errorPage.mainPage = successPage.mainPage;
                                errorPage.ShowDialog();
                                break;
                            }
                            else
                            {
                                ServerSeting.LastCardNum = ServerSeting.TempCardNum;
                            }
                            string cardNum = errormsg.ToString();
                            if (ServerSeting.bFeeder.serialPort.IsOpen)
                            {
                                byte[] send = new byte[6];
                                send[0] = 0x02;
                                send[1] = 0x45;
                                send[2] = 0x53;
                                send[3] = 0x03;
                                send[4] = 0x17;
                                send[5] = 0x05;
                                ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                            }
                            //PrintParamtClass printParamt = new PrintParamtClass() { name = successPage.user.Name, phone = successPage.user.phone, cardNo = successPage.user.cardNum };
                            //UseM5 useM5 = new UseM5(PrintClass.Reissue, printParamt);
                            //object error = "";
                            //if (!useM5.ConnState(ref error))
                            //{
                            //    MessageBox.Show(error.ToString());
                            //};
                            //成功后

                            MainControl mainControl = new MainControl(successPage.mainPage);
                            successPage.mainPage.Grid.Children.Clear();
                            successPage.mainPage.Grid.Children.Add(mainControl);
                            ServerSeting.ISAdd = false;
                            TransactionSucessPage transactionSucessPage = new TransactionSucessPage();
                            TransactionSucess2ShowControl transactionSucessShowControls = new TransactionSucess2ShowControl(transactionSucessPage);
                            transactionSucessPage.cardData = successPage.user;
                            transactionSucessShowControls.printParamt = new PrintParamtClass() { name = successPage.user.Name, phone = successPage.user.phone, cardNo = cardNum };
                            transactionSucessPage.GRID.Children.Add(transactionSucessShowControls);
                            transactionSucessPage.ShowDialog();
                        }
                    }
                    else
                    {
                        MainControl mainControle = new MainControl(successPage.mainPage);
                        ReturnInfo returnInfo = errormsg as ReturnInfo;
                        successPage.mainPage.Grid.Children.Clear();
                        successPage.mainPage.Grid.Children.Add(mainControle);
                        errorPage errorPage = new errorPage(returnInfo.errorMsg.ToString());
                        errorPage.mainPage = successPage.mainPage;
                        errorPage.ShowDialog();
                    }
                    break;
            }
        }
        public void print(RenewListControl data)
        {
            data.PrintShow = new PrintShow(data.page, data);
            data.PrintShow.printAction = PrintAction.renew;
            data.PrintShow.infos = Remark;
            data.PrintShow.data = data.user;
            data.PrintShow.listControl = data;
            data.PrintShow.ShowDialog();
        }
    }
}
