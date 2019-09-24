using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rfid系统.ViewModel
{
    public class DetailsViewModel : NotificationObject
    {
        public DetailsViewModel(MainControl control, QueryInfo info)
        {
            this.control = control;
            DetailBing = Visibility.Hidden;
            this.info = info;
            Task.Run(() =>
            {
                BookName = info.BookName;
                ISBN = info.ISBN;
                PageNumber = info.PageNumber;
                Author = info.Author;
                CollectionCode = info.CorrectionCode;
                Price = info.Price;
                Press = info.Press;
                PressDate = info.PressDate;
                ClassificationName = info.ClassificationName;
                CallNumber = info.CallNumber;
                RFID = info.RFID;
            });
        }
        public QueryInfo info;
        public MainControl control;
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
        /// isbn
        /// </summary>
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
                this.RaisePropertyChanged(() => Author);
            }
        }
        /// <summary>
        /// 馆藏吗
        /// </summary>
        private string collectionCode { get; set; }
        public string CollectionCode
        {
            get { return collectionCode; }
            set
            {
                collectionCode = value;
                this.RaisePropertyChanged(() => CollectionCode);
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
        /// 分类名
        /// </summary>
        private string classificationName { get; set; }
        public string ClassificationName
        {
            get { return classificationName; }
            set
            {
                classificationName = value;
                this.RaisePropertyChanged(() => ClassificationName);
            }
        }
        /// <summary>
        /// 出版社
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
        /// 返回
        /// </summary>
        private ICommand backCommand { get; set; }
        public ICommand BackCommand
        {
            get
            {
                return backCommand ?? (backCommand = new DelegateCommand(() =>
                {
                    control.Grid.Children.Clear();
                    QueryControl queryControl = new QueryControl(control);
                    control.Grid.Children.Add(queryControl);
                }));
            }
        }
        private ICommand correctionCommand { get; set; }
        public ICommand CorrectionCommand
        {
            get
            {
                return correctionCommand ?? (correctionCommand=new DelegateCommand(()=> {
                    if (DetailBing == Visibility.Hidden)
                        DetailBing = Visibility.Visible;
                }));
            }
        }
        #region 绑定更正
        /// <summary>
        /// 编辑
        /// </summary>
        private Visibility detailBing { get; set; }
        public Visibility DetailBing
        {
            get { return detailBing; }
            set
            {
                detailBing = value;
                this.RaisePropertyChanged(()=> DetailBing);
            }
        }
        /// <summary>
        /// rfid
        /// </summary>
        private string rfid { get; set; }
        public string RFID
        {
            get { return rfid; }
            set
            {
                rfid = value;
                this.RaisePropertyChanged(()=>RFID);
            }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        private string error { get; set; }
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                this.RaisePropertyChanged(() => Error);
            }
        }

        private string color { get; set; }
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                this.RaisePropertyChanged(() => Color);
            }
        }

        private ICommand okCommand { get; set; }
        public ICommand OkCommand
        {
            get
            {
                return okCommand ?? (okCommand = new DelegateCommand<DetailsControl>((data) =>
                {
                    ///刷新当前页返回
                    Dictionary<string, object> keyValues = new Dictionary<string, object>();
                    keyValues.Add("id", info.id);
                    keyValues.Add("lendingPermission", 1);
                    keyValues.Add("dailyRent", 1);
                    keyValues.Add("available", 1);
                    keyValues.Add("place", info.Place);
                    keyValues.Add("callNumber", CallNumber);
                    keyValues.Add("code", CollectionCode);
                    keyValues.Add("rfid", RFID);
                    if (CallNumber == info.CallNumber && CollectionCode == info.CorrectionCode && RFID == info.RFID)
                        return;
                    object errorMsg = keyValues;
                    BingCorrectionEditDAL editDAL = new BingCorrectionEditDAL();
                    editDAL.BingCorrectionEdit(ref errorMsg);
                    RetrunInfo Rinfo = errorMsg as RetrunInfo;
                    if (Rinfo.TrueOrFalse)
                    {
                        Color = "#FF344561";
                        Error = Rinfo.result.ToString();

                        info.CallNumber = CallNumber;
                        info.CorrectionCode = CollectionCode;
                        info.RFID = RFID;

                        data.info.CallNumber = CallNumber;
                        data.info.CorrectionCode = CollectionCode;
                        data.info.RFID = RFID;
                        ///刷新列表
                        errorMsg = ServerSetting.parament;
                        SelectListDAL selectListDAL = new SelectListDAL();
                        if (selectListDAL.SelectList(ServerSetting.@class, ref errorMsg))
                        {
                            RetrunInfo infoa = errorMsg as RetrunInfo;
                            List<QueryInfo> Lists = infoa.result as List<QueryInfo>;
                            ServerSetting.info = Lists;
                            if (Lists == null || Lists.Count <= 0)
                                ServerSetting.loadPage = 1;
                        }
                        else
                        {
                            ServerSetting.info = null;
                            ServerSetting.loadPage = 1;
                        }
                    }
                    else
                    {
                        Color = "Red";
                        Error = Rinfo.result.ToString();
                        if (ServerSetting.IsOverDue)
                        {
                            ErrorPage errorPage = new ErrorPage(errorMsg.ToString(), control.mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                        }
                    }
                }));
            }
        }
        private ICommand canleCommand { get; set; }
        public ICommand CanleCommand
        {
            get
            {
                return canleCommand ?? (canleCommand = new DelegateCommand(() =>
                {
                    ///返回
                    DetailBing = Visibility.Hidden;
                    Error = "";
                }));
            }
        }
        #endregion
    }
}
