using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Rfid系统.ViewModel
{
    public class QueryViewModel : NotificationObject
    {
        public QueryViewModel(MainControl control)
        {
            SelectIndex = 0;
            this.control = control;
            if (ServerSetting.info == null)
            {
                PICVisible = Visibility.Visible;
                PICState = "../Images/无数据.jpg";
                DataVisible = Visibility.Hidden;

                ServerSetting.totalPages = 0;
                ServerSetting.List = null;
                ServerSetting.loadPage = 1;
                ServerSetting.info = null;
            }
            else
            {
                List = ServerSetting.info;
                LoadPage = ServerSetting.loadPage.ToString();
            }
            control.thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (ServerSetting.rfid.IsOpen())
                        ServerSetting.rfid.Start();
                    lock (ServerSetting.EPClist)
                    {
                        if (ServerSetting.EPClist.Count > 0)
                        {
                            string epc = ServerSetting.EPClist.Dequeue();
                            lock (RFIDlist)
                            {
                                if (SelectIndex == 1)
                                {
                                    Parameter = epc;
                                    if (!RFIDlist.Contains(epc))
                                    {
                                        Dispatcher.CurrentDispatcher.Invoke(() =>
                                        {
                                            RFIDlist.Add(epc);
                                            ServerSetting.@class = SelectClass.RFID;
                                            ServerSetting.parament = RFIDlist;
                                            GetData();
                                        });
                                    }
                                }
                                else
                                {
                                    RFIDlist.Clear();
                                    ServerSetting.OldEPClist.Clear();
                                }
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
            }));
           control.thread.IsBackground = true;
           control.thread.Start();
        }
        public List<string> RFIDlist=new List<string>();
        public Thread thread;
        public MainControl control;
        /// <summary>
        /// 查询参数
        /// </summary>
        private string parameter { get; set; }
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = value;
                this.RaisePropertyChanged(()=>Parameter);
            }
        }
        /// <summary>
        /// 选中类型（位置）
        /// </summary>
        private int selectIndex { get; set; }
        public int SelectIndex
        {
            get { return selectIndex; }
            set
            {
                selectIndex = value;
                this.RaisePropertyChanged(()=> SelectIndex);
            }
        }
        /// <summary>
        /// 查询--kedown--click
        /// </summary>
        private ICommand input { get; set; }
        public ICommand Input
        {
            get
            {
                return input ?? (input = new DelegateCommand<TextBox>((data)=> {
                    ///查询
                    if (string.IsNullOrEmpty(data.Text))
                        return;

                    object errorMsg = data.Text.ToString();
                    ServerSetting.parament = errorMsg.ToString();
                    SelectClass selectClass;
                    switch (SelectIndex)
                    {
                        case 0:
                            selectClass = SelectClass.isbn;
                            break;
                        case 2:
                            selectClass = SelectClass.CorrectionCode;
                            break;
                        case 3:
                            selectClass = SelectClass.callNumber;
                            break;
                        default:
                            return;
                    }
                    ServerSetting.parament = data.Text;
                    ServerSetting.@class = selectClass;
                    ServerSetting.loadPage = 1;
                    GetData();
                    data.SelectAll();
                    data.Focus();
                }));
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        private List<QueryInfo> list { get; set; } = new List<QueryInfo>();
        public List<QueryInfo> List
        {
            get { return list; }
            set
            {
                list = value;
                this.RaisePropertyChanged(() => List);
            }
        }
        /// <summary>
        /// 选中
        /// </summary>
        private ICommand click { get; set; }
        public ICommand Click
        {
            get
            {
                return click ?? (click = new DelegateCommand<DataGrid>((data) => {
                    ///选中
                    if (data.SelectedIndex < 0)
                        return;
                    ServerSetting.info = List;
                    QueryInfo info = data.SelectedItem as QueryInfo;
                    if (info.bop == "图书")
                    {
                        DetailsControl detailsControl = new DetailsControl(control, info);
                        control.Grid.Children.Clear();
                        control.Grid.Children.Add(detailsControl);
                    }
                    else
                    {
                        if (info.merge.Equals("0"))
                        {
                            PeriodicalChangeControl periodicalChangeControl = new PeriodicalChangeControl(control,info.id);
                            this.control.Grid.Children.Clear();
                            this.control.Grid.Children.Add(periodicalChangeControl);
                        }
                        else
                        {
                            BIssueSubscription_Control bIssueSubscription_Control = new BIssueSubscription_Control(control,info.id);
                            this.control.Grid.Children.Clear();
                            this.control.Grid.Children.Add(bIssueSubscription_Control);
                        }
                    }
                  
                }));
            }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        private string loadPage { get; set; }
        public string LoadPage
        {
            get { return loadPage; }
            set
            {
                loadPage = value;
                this.RaisePropertyChanged(()=> LoadPage);
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        private ICommand firstCommand { get; set; }
        public ICommand FirstCommand
        {
            get
            {
                return firstCommand ?? (firstCommand = new DelegateCommand(()=> {
                    if (ServerSetting.totalPages <= 1)
                        return;
                    if (ServerSetting.loadPage <= 1)
                        return;
                    ServerSetting.loadPage = 1;
                    GetData();
                }));
            }
        }
        /// <summary>
        /// 末页
        /// </summary>
        private ICommand finallyCommand { get; set; }
        public ICommand FinallyCommand
        {
            get
            {
                return finallyCommand ?? (finallyCommand = new DelegateCommand(() => {
                    if (ServerSetting.totalPages <= 1 || ServerSetting.loadPage == ServerSetting.totalPages)
                        return;
                    ServerSetting.loadPage = ServerSetting.totalPages;
                    GetData();
                }));
            }
        }
        /// <summary>
        /// 上一页
        /// </summary>
        private ICommand lastCommand { get; set; }
        public ICommand LastCommand
        {
            get
            {
                return lastCommand ?? (lastCommand = new DelegateCommand(() => {
                    if (ServerSetting.totalPages <= 1 || ServerSetting.loadPage <= 1)
                        return;
                    ServerSetting.loadPage = ServerSetting.loadPage - 1;
                    GetData();
                }));
            }
        }
        /// <summary>
        /// 下一页
        /// </summary>
        private ICommand nextCommand { get; set; }
        public ICommand NextCommand
        {
            get
            {
                return nextCommand ?? (nextCommand = new DelegateCommand(() => {
                    if (ServerSetting.totalPages <= 1 || ServerSetting.loadPage == ServerSetting.totalPages)
                        return;
                    ServerSetting.loadPage = ServerSetting.loadPage + 1;
                    GetData();
                    
                }));
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        private string picState { get; set; }
        public string PICState
        {
            get { return picState; }
            set
            {
                picState = value;
                this.RaisePropertyChanged(()=>PICState);
            }
        }
        /// <summary>
        /// 图片显示
        /// </summary>
        private Visibility picVisible { get; set; }
        public Visibility PICVisible
        {
            get { return picVisible; }
            set
            {
                picVisible = value;
                this.RaisePropertyChanged(()=>PICVisible);
            }
        }
        /// <summary>
        /// 数据显示
        /// </summary>
        private Visibility dataVisible { get; set; }
        public Visibility DataVisible
        {
            get { return dataVisible; }
            set
            {
                dataVisible = value;
                this.RaisePropertyChanged(() => DataVisible);
            }
        }
        public void GetData()
        {
            object errorMsg = ServerSetting.parament;
            SelectListDAL selectListDAL = new SelectListDAL();
            if (selectListDAL.SelectList(ServerSetting.@class, ref errorMsg))
            {
                RetrunInfo info = errorMsg as RetrunInfo;
                List<QueryInfo> Lists = info.result as List<QueryInfo>;
                if (Lists.Count > 0)
                {
                    PICVisible = Visibility.Hidden;
                    DataVisible = Visibility.Visible;
                    ServerSetting.info = Lists;
                    List = Lists;
                    LoadPage = ServerSetting.loadPage.ToString();
                }
                else
                {
                    PICVisible = Visibility.Visible;
                    PICState ="../Images/无数据.jpg";
                    DataVisible = Visibility.Hidden;

                    ServerSetting.totalPages = 0;
                    ServerSetting.List = null;
                    ServerSetting.loadPage = 1;
                    ServerSetting.info = null;
                }
            }
            else
            {
                if (ServerSetting.IsOverDue)
                {
                    RetrunInfo info = errorMsg as RetrunInfo;
                    ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainWindow);
                    DialogHelper.ShowDialog(errorPage);
                }
                PICVisible = Visibility.Visible;
                PICState ="../Images/未连接.png";
                DataVisible = Visibility.Hidden;
            }
        }
    }
}
