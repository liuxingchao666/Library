using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rfid系统.ViewModel
{
    public class PeriodicalViewModel : NotificationObject
    {
        public PeriodicalViewModel(PeriodicalControl periodicalControl)
        {
            int i = 0;
            this.control = periodicalControl;
            while (i < 10)
            {
                MarCodeList.Add(new MarCodeInfo() { PICBG = new BitmapImage(new Uri("02.png", UriKind.RelativeOrAbsolute)), VisibleState = System.Windows.Visibility.Hidden, VisibleState1 = System.Windows.Visibility.Hidden, VisibleState2 = System.Windows.Visibility.Hidden, VisibleState3 = System.Windows.Visibility.Hidden });
                i++;
            }
            GetPlaceListDAL listDAL = new GetPlaceListDAL();
            object errorMsg = null;
            if (listDAL.GetPlaceList(ref errorMsg))
            {
                string placeId = ConfigurationManager.AppSettings["PlaceId"];
                RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                PlaceList = retrunInfo.result as List<PlaceInfo>;
                if (PlaceList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(placeId))
                    {
                        foreach (PlaceInfo info in PlaceList)
                        {
                            if (info.id == placeId)
                                Place = info;
                        }
                        ServerSetting.Place = Place.PlaceName;
                    }
                    else
                    {
                        try
                        {
                            Place = PlaceList[0];
                            ServerSetting.Place = Place.PlaceName;
                            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                            cfa.AppSettings.Settings["PlaceId"].Value = Place.id;
                            cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                            ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                        }
                        catch { }
                    }
                }
            }
        }
        public PeriodicalControl control;
        /// <summary>
        /// 显示内容
        /// </summary>
        private List<MarCodeInfo> marCodeList { get; set; } = new List<MarCodeInfo>();
        public List<MarCodeInfo> MarCodeList
        {
            get { return marCodeList; }
            set
            {
                marCodeList = value;
                this.RaisePropertyChanged(() => MarCodeList);
            }
        }
        private int selectIndex { get; set; }
        public int SelectIndex
        {
            get { return selectIndex; }
            set
            {
                selectIndex = value.ToInt();
                this.RaisePropertyChanged(() => SelectIndex);
            }
        }
        /// <summary>
        /// issn
        /// </summary>
        private string issn { get; set; }
        public string ISSN
        {
            get { return issn; }
            set
            {
                issn = value;
                this.RaisePropertyChanged(() => ISSN);
            }
        }
        /// <summary>
        /// 同一刊号
        /// </summary>
        private string unifynum { get; set; }
        public string unifyNum
        {
            get { return unifynum; }
            set
            {
                unifynum = value;
                this.RaisePropertyChanged(() => unifyNum);
            }
        }
        /// <summary>
        /// 刊期名称
        /// </summary>
        private string name { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged(() => Name);
            }
        }
        /// <summary>
        /// 主编
        /// </summary>
        private string author { get; set; }
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                this.RaisePropertyChanged(() => Author);
            }
        }
        /// <summary>
        /// 发行单位
        /// </summary>
        private string fkpressName { get; set; }
        public string fkPressName
        {
            get { return fkpressName; }
            set
            {
                fkpressName = value;
                this.RaisePropertyChanged(() => fkPressName);
            }
        }
        /// <summary>
        /// 分类名
        /// </summary>
        private string fktypeName { get; set; }
        public string fkTypeName
        {
            get { return fktypeName; }
            set
            {
                fktypeName = value;
                this.RaisePropertyChanged(() => fkTypeName);
            }
        }
        /// <summary>
        /// 分类号
        /// </summary>
        private string fktypeCode { get; set; }
        public string fkTypeCode
        {
            get { return fktypeCode; }
            set
            {
                fktypeCode = value;
                this.RaisePropertyChanged(() => fkTypeCode);
            }
        }
        /// <summary>
        /// 并列题名
        /// </summary>
        private string paralleltitle { get; set; }
        public string parallelTitle
        {
            get { return paralleltitle; }
            set
            {
                paralleltitle = value;
                this.RaisePropertyChanged(() => parallelTitle);
            }
        }
        /// <summary>
        /// 邮发代号
        /// </summary>
        private string postissueNumber { get; set; }
        public string postIssueNumber
        {
            get { return postissueNumber; }
            set
            {
                postissueNumber = value;
                this.RaisePropertyChanged(() => postIssueNumber);
            }
        }
        /// <summary>
        /// 发行周期
        /// </summary>
        private string releasecycle { get; set; }
        public string releaseCycle
        {
            get { return releasecycle; }
            set
            {
                releasecycle = value;
                this.RaisePropertyChanged(() => releaseCycle);
            }
        }
        /// <summary>
        /// 价格
        /// </summary>
        private string issnprice { get; set; }
        public string issnPrice
        {
            get { return issnprice; }
            set
            {
                issnprice = value;
                this.RaisePropertyChanged(() => issnPrice);
            }
        }
        /// <summary>
        /// 参考开本
        /// </summary>
        private string openbook { get; set; }
        public string openBook
        {
            get { return openbook; }
            set
            {
                openbook = value;
                this.RaisePropertyChanged(() => openBook);
            }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        private string remark { get; set; }
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged(() => Remark);
            }
        }
        /// <summary>
        /// 刊期号
        /// </summary>
        private string periodicalCode { get; set; }
        public string PeriodicalCode
        {
            get { return periodicalCode; }
            set
            {
                periodicalCode = value;
                this.RaisePropertyChanged(() => PeriodicalCode);
            }
        }
        /// <summary>
        /// 索取号
        /// </summary>
        private string callNumber { get; set; }
        public string CallNumber
        {
            get { return callNumber; }
            set
            {
                callNumber = value;
                this.RaisePropertyChanged(() => CallNumber);
            }
        }
        /// <summary>
        /// isbn搜索
        /// </summary>
        private ICommand ISBNcommond { get; set; }
        public ICommand ISBNCommond
        {
            get
            {
                return ISBNcommond ?? (ISBNcommond = new DelegateCommand<TextBox>((data) =>
                {
                    lock (ISSN)
                    {
                        object issn = ISSN;
                        SelectLocalDAL localDAL = new SelectLocalDAL();
                        if (localDAL.SelectLoacl(ref issn))
                        {
                            RetrunInfo info = issn as RetrunInfo;
                            if (info.TrueOrFalse)
                            {
                                List<PeriodicalsInfo> infos = info.result as List<PeriodicalsInfo>;
                                PeriodicalChooseControl periodicalChooseControl = new PeriodicalChooseControl(infos);
                                DialogHelper.ShowDialog(periodicalChooseControl);
                                if (periodicalChooseControl.info != null)
                                {
                                    Name = periodicalChooseControl.info.name;
                                    fkTypeCode = periodicalChooseControl.info.fkTypeCode;
                                    fkTypeName = periodicalChooseControl.info.fkTypeName;
                                    fkPressName = periodicalChooseControl.info.fkPressName;
                                    Author = periodicalChooseControl.info.author;
                                    unifyNum = periodicalChooseControl.info.unifyNum;
                                    parallelTitle = periodicalChooseControl.info.parallelTitle;
                                    postIssueNumber = periodicalChooseControl.info.postIssueNumber;
                                    openBook = periodicalChooseControl.info.openBook;
                                    issnPrice = periodicalChooseControl.info.issnPrice;
                                    releaseCycle = periodicalChooseControl.info.releaseCycle;
                                    remark = periodicalChooseControl.info.remark;
                                    control.periodicalInfo = new PeriodicalInfo() { fkCataPeriodicalId = periodicalChooseControl.info.id };
                                    GetCallNumberByIdDAL getCallNumberById = new GetCallNumberByIdDAL();
                                    object errorMsg = periodicalChooseControl.info.id;
                                    if (getCallNumberById.GetCallNumberById(ref errorMsg))
                                    {
                                        RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                                        if (retrunInfo.TrueOrFalse)
                                        {
                                            control.info = retrunInfo.result as CallNumberInfo;
                                            CallNumberState = Visibility.Hidden;
                                            if (SelectIndex == 0)
                                                CallNumber = control.info.searchNumberOrderNum;
                                            else
                                                CallNumber = control.info.searchNumberAuthorNum;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ServerSetting.IsOverDue)
                                {
                                    ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainControl.mainWindow);
                                    DialogHelper.ShowDialog(errorPage);
                                }
                                else
                                {
                                    MessageBox.Show("失败提示：" + info.result);
                                }
                            }
                        }
                    }
                }));
            }
        }
        /// <summary>
        /// 索取号
        /// </summary>
        private ICommand CallNumbercommond { get; set; }
        public ICommand CallNumberCommond
        {
            get
            {
                return CallNumbercommond ?? (CallNumbercommond = new DelegateCommand(() =>
                {
                    #region 索取号
                    CallNumberControl callNumberControl = new CallNumberControl(control.ISBNbookListInfo);
                    DialogHelper.ShowDialog(callNumberControl);
                    CallNumber = control.ISBNbookListInfo.fkTypeCode + "/" + control.ISBNbookListInfo.SetBooks;
                    #endregion
                }));
            }
        }
        /// <summary>
        /// 馆藏地集合
        /// </summary>
        private List<PlaceInfo> placeList { get; set; } = new List<PlaceInfo>();
        public List<PlaceInfo> PlaceList
        {
            get { return placeList; }
            set
            {
                placeList = value;
                this.RaisePropertyChanged(() => PlaceList);
            }
        }
        /// <summary>
        /// 默认选中馆藏地
        /// </summary>
        private PlaceInfo place { get; set; } = new PlaceInfo();
        public PlaceInfo Place
        {
            get { return place; }
            set
            {
                place = value;
                this.RaisePropertyChanged(() => Place);
            }
        }
        /// <summary>
        /// 修改默认馆藏地
        /// </summary>
        private ICommand placeChange { get; set; }
        public ICommand PlaceChange
        {
            get
            {
                return placeChange ?? (placeChange = new DelegateCommand(() =>
                {
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                    cfa.AppSettings.Settings["PlaceId"].Value = Place.id;
                    ServerSetting.Place = Place.PlaceName;
                    cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                    ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                }));
            }
        }
        private ICommand periodicalCommond { get; set; }
        public ICommand PeriodicalCommond
        {
            get
            {
                return periodicalCommond ?? (periodicalCommond = new DelegateCommand(() =>
                {
                    if (control.periodicalInfo == null || string.IsNullOrEmpty(control.periodicalInfo.fkCataPeriodicalId))
                        return;
                    GetPNDAL getPN = new GetPNDAL();
                    object errorMsg = control.periodicalInfo.fkCataPeriodicalId;
                    if (getPN.GetPN(ref errorMsg))
                    {
                        RetrunInfo info = errorMsg as RetrunInfo;
                        if (info.TrueOrFalse)
                        {
                            ///弹出列表
                            List<PNInfo> pNInfos = info.result as List<PNInfo>;
                            PNChooseControl pNChooseControl = new PNChooseControl(pNInfos, control.mainControl.mainWindow);
                            pNChooseControl.fkCataPeriodicalId = control.periodicalInfo.fkCataPeriodicalId;
                            DialogHelper.ShowDialog(pNChooseControl);
                            if (pNChooseControl.info != null)
                            {
                                PeriodicalCode = pNChooseControl.info.aNumber;
                                sNumber = pNChooseControl.info.sNumber;
                                price = pNChooseControl.info.price;
                                page = pNChooseControl.info.page;
                                remarks = pNChooseControl.info.remark;
                                publicationDateStr = pNChooseControl.info.publicationDateStr;
                                PeriodicalState = Visibility.Hidden;
                                control.periodicalInfo.pNumberId = pNChooseControl.info.fkCataPeriodicalId;
                            }
                        }
                        else
                        {
                            if (ServerSetting.IsOverDue)
                            {
                                ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainControl.mainWindow);
                                DialogHelper.ShowDialog(errorPage);
                            }
                            else
                            {
                                MessageBox.Show("失败提示：" + info.result);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(errorMsg.ToString());
                    }
                    ///弹出列表
                }));
            }
        }

        private Visibility periodicalState { get; set; }
        public Visibility PeriodicalState
        {
            get { return periodicalState; }
            set
            {
                periodicalState = value;
                this.RaisePropertyChanged(() => PeriodicalState);
            }
        }
        private Visibility callNumberState { get; set; }
        public Visibility CallNumberState
        {
            get { return callNumberState; }
            set
            {
                callNumberState = value;
                this.RaisePropertyChanged(() => CallNumberState);
            }
        }
        /// <summary>
        /// 出版日期
        /// </summary>
        private string PublicationDateStr { get; set; }
        public string publicationDateStr
        {
            get { return PublicationDateStr; }
            set
            {
                PublicationDateStr = value;
                this.RaisePropertyChanged(() => publicationDateStr);
            }
        }
        /// <summary>
        /// 总期号
        /// </summary>
        private string SNumber { get; set; }
        public string sNumber
        {
            get { return SNumber; }
            set
            {
                SNumber = value;
                this.RaisePropertyChanged(() => sNumber);
            }
        }
        /// <summary>
        /// 定价
        /// </summary>
        private string Price { get; set; }
        public string price
        {
            get { return Price; }
            set
            {
                Price = value;
                this.RaisePropertyChanged(() => price);
            }
        }
        /// <summary>
        /// 页数
        /// </summary>
        private string Page { get; set; }
        public string page
        {
            get { return Page; }
            set
            {
                Page = value;
                this.RaisePropertyChanged(() => page);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string Remarks { get; set; }
        public string remarks
        {
            get { return Remarks; }
            set
            {
                Remarks = value;
                this.RaisePropertyChanged(() => remarks);
            }
        }
        private ICommand printSetCommand { get; set; }
        public ICommand PrintSetCommand
        {
            get
            {
                return printSetCommand ?? (printSetCommand = new DelegateCommand(() =>
                {
                    control.isPrintSet = true;

                    PrintSetControl printSetControl = new PrintSetControl(MarCodeList);
                    control.mainControl.Grid.Children.Clear();
                    control.mainControl.Grid.Children.Add(printSetControl);
                }));
            }
        }
    }
}
