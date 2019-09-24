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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Rfid系统.ViewModel
{
    public class BingHistoryViewModel : NotificationObject
    {
        public BingHistoryViewModel(MainControl mainControl)
        {
            this.mainControl = mainControl;
            Dictionary<string, object> valuePairs = new Dictionary<string, object>();
            valuePairs.Add("pageSize", 10);
            valuePairs.Add("currentPage", 1);
            valuePairs.Add("beginTime", DateTime.Now.AddDays(-3).ToString("yyyy/MM/dd"));
            valuePairs.Add("endTime", DateTime.Now.ToString("yyyy/MM/dd"));
            if (GetData(valuePairs, 1))
            {
                LoadPage = "1";
            }
           mainControl.thread = new Thread(new ThreadStart(() =>
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
                                            Dictionary<string, object> urlParamant = new Dictionary<string, object>();
                                            urlParamant.Add("pageSize", 10);
                                            urlParamant.Add("currentPage", 1);
                                            urlParamant.Add("rfids", RFIDlist.ToArray());
                                            if (!GetData(urlParamant, 1))
                                            {
                                                RFIDlist.Remove(epc);
                                                //ServerSetting.OldEPClist.Enqueue(epc);
                                            }
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
           mainControl.thread.IsBackground = true;
           mainControl.thread.Start();
        }
        public List<string> RFIDlist = new List<string>();
        public Thread thread;
        /// <summary>
        /// 总页数
        /// </summary>
        public int countPage;
        public MainControl mainControl;
        /// <summary>
        /// 参数
        /// </summary>
        private string parameter { get; set; }
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = value;
                this.RaisePropertyChanged(() => Parameter);
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
                this.RaisePropertyChanged(() => SelectIndex);
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
                return input ?? (input = new DelegateCommand<System.Windows.Controls.TextBox>((data) =>
                {
                    ///查询
                    Dictionary<string, object> urlParamant = new Dictionary<string, object>();
                    urlParamant.Add("pageSize", 10);
                    urlParamant.Add("currentPage", 1);
                    switch (SelectIndex)
                    {
                        case 0:
                            urlParamant.Add("callNumber", data.Text);
                            break;
                        case 1:
                            return;
                        case 2:
                            urlParamant.Add("code", data.Text);
                            break;
                        default:
                            urlParamant.Add("beginTime", sDate);
                            urlParamant.Add("endTime", eDate);
                            break;
                    }
                    if (SelectIndex == 0 || SelectIndex == 2)
                        if (string.IsNullOrEmpty(data.Text))
                            return;
                    if (GetData(urlParamant, 1))
                    {
                        LoadPage = "1";
                    }
                }));
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        private List<BingHistoryInfo> list { get; set; } = new List<BingHistoryInfo>();
        public List<BingHistoryInfo> List
        {
            get { return list; }
            set
            {
                list = value;
                this.RaisePropertyChanged(() => List);
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
                this.RaisePropertyChanged(() => LoadPage);
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
                return firstCommand ?? (firstCommand = new DelegateCommand(() =>
                {
                    if (countPage == 1 || LoadPage.ToInt() <= 1)
                        return;
                    Dictionary<string, object> urlParamant = new Dictionary<string, object>();
                    urlParamant.Add("pageSize", 10);
                    urlParamant.Add("currentPage", 1);
                    switch (SelectIndex)
                    {
                        case 0:
                            urlParamant.Add("callNumber", Parameter);
                            break;
                        case 1:
                            urlParamant.Add("rfids", RFIDlist.ToString());
                            break;
                        case 2:
                            urlParamant.Add("code", Parameter);
                            break;
                        default:
                            urlParamant.Add("beginTime", sDate);
                            urlParamant.Add("endTime", eDate);
                            break;
                    }
                    if (!GetData(urlParamant, 1))
                    {
                     
                    }
                    LoadPage = "1";
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
                return finallyCommand ?? (finallyCommand = new DelegateCommand(() =>
                {
                    if (LoadPage.ToInt() == countPage)
                        return;
                    Dictionary<string, object> urlParamant = new Dictionary<string, object>();
                    urlParamant.Add("pageSize", 10);
                    urlParamant.Add("currentPage", countPage);
                    switch (SelectIndex)
                    {
                        case 0:
                            urlParamant.Add("callNumber", Parameter);
                            break;
                        case 1:
                            urlParamant.Add("rfids", RFIDlist.ToString());
                            break;
                        case 2:
                            urlParamant.Add("code", Parameter);
                            break;
                        default:
                            urlParamant.Add("beginTime", sDate);
                            urlParamant.Add("endTime", eDate);
                            break;
                    }
                    if (GetData(urlParamant, countPage))
                    {
                        LoadPage = countPage.ToString();
                    }
                    else
                    {
                        LoadPage = "1";
                    }
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
                return lastCommand ?? (lastCommand = new DelegateCommand(() =>
                {
                    if (countPage <= 1 || LoadPage.ToInt() <= 1)
                        return;
                    Dictionary<string, object> urlParamant = new Dictionary<string, object>();
                    urlParamant.Add("pageSize", 10);
                    urlParamant.Add("currentPage", LoadPage.ToInt()-1);
                    switch (SelectIndex)
                    {
                        case 0:
                            urlParamant.Add("callNumber", Parameter);
                            break;
                        case 1:
                            urlParamant.Add("rfids", RFIDlist.ToString());
                            break;
                        case 2:
                            urlParamant.Add("code", Parameter);
                            break;
                        default:
                            urlParamant.Add("beginTime", sDate);
                            urlParamant.Add("endTime", eDate);
                            break;
                    }
                    if (GetData(urlParamant, LoadPage.ToInt() - 1))
                    {
                        LoadPage = (LoadPage.ToInt() - 1).ToString();
                    }
                    else
                    {
                        LoadPage = "1";
                    }
                   
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
                return nextCommand ?? (nextCommand = new DelegateCommand(() =>
                {
                    if (countPage <= 1 || LoadPage.ToInt() == countPage)
                        return;
                    Dictionary<string, object> urlParamant = new Dictionary<string, object>();
                    urlParamant.Add("pageSize", 10);
                    urlParamant.Add("currentPage", LoadPage.ToInt() + 1);
                    switch (SelectIndex)
                    {
                        case 0:
                            urlParamant.Add("callNumber", Parameter);
                            break;
                        case 1:
                            urlParamant.Add("rfids", RFIDlist.ToString());
                            break;
                        case 2:
                            urlParamant.Add("code", Parameter);
                            break;
                        default:
                            urlParamant.Add("beginTime", sDate);
                            urlParamant.Add("endTime", eDate);
                            break;
                    }
                    if (GetData(urlParamant, LoadPage.ToInt() + 1))
                    {
                        LoadPage = (LoadPage.ToInt() + 1).ToString();
                    }
                    else
                    {
                        LoadPage = "1";
                    }
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
                this.RaisePropertyChanged(() => PICState);
            }
        }
        /// <summary>
        /// 图片显示
        /// </summary>
        private System.Windows.Visibility picVisible { get; set; }
        public Visibility PICVisible
        {
            get { return picVisible; }
            set
            {
                picVisible = value;
                this.RaisePropertyChanged(() => PICVisible);
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
        /// <summary>
        /// 开始时间
        /// </summary>
        private string sdate { get; set; }
        public string sDate
        {
            get { return sdate; }
            set
            {
                sdate = value;
                this.RaisePropertyChanged(() => sDate);
            }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        private string edate { get; set; }
        public string eDate
        {
            get { return edate; }
            set
            {
                edate = value;
                this.RaisePropertyChanged(() => eDate);
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="paramant">条件</param>
        /// <param name="Querypage">请求页</param>
        /// <returns></returns>
        public bool GetData(object paramant, int Querypage)
        {
            object errorMsg = paramant;
            GetBingHistoryDAL getBingHistoryDAL = new GetBingHistoryDAL();
            if (getBingHistoryDAL.GetBingHistory(ref errorMsg))
            {
                RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                if (retrunInfo.TrueOrFalse)
                {
                    List<BingHistoryInfo> infos = retrunInfo.result as List<BingHistoryInfo>;
                    if (infos.Count <= 0)
                    {
                        DataVisible = Visibility.Hidden;
                        PICVisible = Visibility.Visible;

                        PICState = "../Images/无数据.jpg";
                        return false;
                    }
                    else
                    {
                        DataVisible = Visibility.Visible;
                        PICVisible = Visibility.Hidden;
                        countPage = retrunInfo.page;
                        List<BingHistoryInfo> bingHistories = new List<BingHistoryInfo>();
                        int i = 1;
                        foreach (var temp in infos)
                        {
                            temp.number = (Querypage - 1) * 10 + i;
                            bingHistories.Add(temp);
                            i++;
                        }
                        List = bingHistories;
                        return true;
                    }
                }
                else
                {
                    if (ServerSetting.IsOverDue)
                    {
                        ErrorPage errorPage = new ErrorPage(retrunInfo.result.ToString(), mainControl.mainWindow);
                        DialogHelper.ShowDialog(errorPage);
                        return false;
                    }
                    else
                    {
                        DataVisible = Visibility.Hidden;
                        PICVisible = Visibility.Visible;
                        PICState = "../Images/未连接.png";
                        return false;
                    }
                }
            }
            else
            {
                DataVisible = Visibility.Hidden;
                PICVisible = Visibility.Visible;
                PICState = "../Images/未连接.png";
                return false;
            }
        }
    }
}
