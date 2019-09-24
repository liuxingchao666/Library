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
    public class RFIDBindingViewModel : NotificationObject
    {
        public RFIDBindingViewModel(MainControl mainControl, RFIDBindingControl control)
        {
            this.mainControl = mainControl;
            this.control = control;
            CallNumberState = Visibility.Hidden;
            int i = 0;
            while (i < 10)
            {
                MarCodeList.Add(new MarCodeInfo() { PICBG = new BitmapImage(new Uri("02.png", UriKind.RelativeOrAbsolute)), VisibleState = System.Windows.Visibility.Hidden, VisibleState1 = System.Windows.Visibility.Hidden, VisibleState2 = System.Windows.Visibility.Hidden, VisibleState3 = System.Windows.Visibility.Hidden });
                i++;
            }
            string placeId = ConfigurationManager.AppSettings["PlaceId"];
            GetPlaceListDAL listDAL = new GetPlaceListDAL();
            object errorMsg = null;
            if (listDAL.GetPlaceList(ref errorMsg))
            {
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
                        mainControl.info.Place = Place.PlaceName;
                        ServerSetting.Place = Place.PlaceName;
                    }
                    else
                    {
                        try
                        {
                            Place = PlaceList[0];
                            mainControl.info.Place = Place.PlaceName;
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
        public MainControl mainControl;
        public RFIDBindingControl control;
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
        private string isbn { get; set; }
        public string ISBN
        {
            get { return isbn; }
            set
            {
                isbn = value;
                this.RaisePropertyChanged(() => ISBN);
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
        /// 书籍编码
        /// </summary>
        private string bookCode { get; set; }
        public string BookCode
        {
            get { return bookCode; }
            set
            {
                bookCode = value;
                this.RaisePropertyChanged(() => BookCode);
            }
        }
        /// <summary>
        /// 书名
        /// </summary>
        private string bookName { get; set; }
        public string BookName
        {
            get { return bookName; }
            set
            {
                bookName = value;
                this.RaisePropertyChanged(() => BookName);
            }
        }
        /// <summary>
        /// 出版社
        /// </summary>
        private string press { get; set; }
        public string Press
        {
            get { return press; }
            set
            {
                press = value;
                this.RaisePropertyChanged(() => Press);
            }
        }
        /// <summary>
        /// 分类号
        /// </summary>
        private string classification { get; set; }
        public string Classification
        {
            get { return classification; }
            set
            {
                classification = value;
                this.RaisePropertyChanged(() => Classification);
            }
        }
        /// <summary>
        /// 页码
        /// </summary>
        private string pageNumber { get; set; }
        public string PageNumber
        {
            get { return pageNumber; }
            set
            {
                pageNumber = value;
                this.RaisePropertyChanged(() => PageNumber);
            }
        }
        /// <summary>
        /// 作者
        /// </summary>
        private string author { get; set; }
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                this.RaisePropertyChanged(() => author);
            }
        }
        /// <summary>
        /// 出版日期
        /// </summary>
        private string pressDate { get; set; }
        public string PressDate
        {
            get { return pressDate; }
            set
            {
                pressDate = value;
                this.RaisePropertyChanged(() => PressDate);
            }
        }
        /// <summary>
        /// 价格
        /// </summary>
        private string price { get; set; }
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                this.RaisePropertyChanged(() => Price);
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
        /// 列表弹出窗
        /// </summary>
        private ICommand isbnCommond { get; set; }
        public ICommand ISBNCommond
        {
            get
            {
                return isbnCommond ?? (isbnCommond = new DelegateCommand<TextBox>(((data) =>
                {
                    ISBN = data.Text;
                    if (string.IsNullOrEmpty(ISBN))
                        return;
                    object errorMsg = ISBN;
                    GetBookListByISBNDAL iSBNDAL = new GetBookListByISBNDAL();
                    iSBNDAL.GetBookListByISBN(ref errorMsg);
                    RetrunInfo info = errorMsg as RetrunInfo;
                    if (!info.TrueOrFalse)
                    {
                        if (ServerSetting.IsOverDue)
                        {
                            ErrorPage errorPage = new ErrorPage(info.result.ToString(), mainControl.mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                            return;
                        }
                    }
                    ISBNListControl iSBNListControl = new ISBNListControl(ISBN, info);
                    DialogHelper.ShowDialog(iSBNListControl);
                    if (iSBNListControl.info != null)
                    {
                        mainControl.info = iSBNListControl.info;
                        BookName = iSBNListControl.info.BookName;
                        Price = iSBNListControl.info.Price;
                        Author = iSBNListControl.info.Author;
                        Press = iSBNListControl.info.Press;
                        PressDate = iSBNListControl.info.PressDate;
                        CallNumber = iSBNListControl.info.CallNumber;
                        PageNumber = iSBNListControl.info.PageNumber;
                        Classification = iSBNListControl.info.Classification;
                        CallNumberState = Visibility.Hidden;
                    }
                    if (iSBNListControl.info == null)
                        return;
                    errorMsg = iSBNListControl.info.id;
                    SelectCataOrderByIDDAL selectCataOrderByIDDAL = new SelectCataOrderByIDDAL();
                    if (selectCataOrderByIDDAL.selectCataOrderByID(ref errorMsg))
                    {
                        RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                        control.info = retrunInfo.result as CallNumberInfo;
                        try
                        {
                            if (SelectIndex == 1)
                                CallNumber = control.info.searchNumberAuthorNum;
                            else
                                CallNumber = control.info.searchNumberOrderNum;
                        }
                        catch { }
                    }
                })));
            }
        }
        /// <summary>
        /// 索取号选择
        /// </summary>
        private ICommand callNumberCommond { get; set; }
        public ICommand CallNumberCommond
        {
            get
            {
                return callNumberCommond ?? (callNumberCommond = new DelegateCommand(() =>
                {
                    if (string.IsNullOrEmpty(CallNumber))
                        return;
                    CallNumberControl callNumberControl = new CallNumberControl(mainControl.info);
                    DialogHelper.ShowDialog(callNumberControl);
                    int index = mainControl.info.CallNumber.IndexOf("/");
                    CallNumber = mainControl.info.CallNumber.Substring(0, index + 1) + callNumberControl.info.SetBooks;
                    mainControl.info.CallNumber = CallNumber;
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
                    cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                    ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                    mainControl.info.Place = Place.PlaceName;
                }));
            }
        }
        private ICommand printSetCommand { get; set; }
        public ICommand PrintSetCommand
        {
            get
            {
                return printSetCommand ?? (printSetCommand = new DelegateCommand(() =>
                {
                    PrintSetControl printSetControl = new PrintSetControl(MarCodeList);
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(printSetControl);
                }));
            }
        }
    }
}
