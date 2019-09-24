using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rfidϵͳ.ViewModel
{
    public class BingCorrectionMessageViewModel : NotificationObject
    {
        public MainControl control;

        public QueryInfo info;

        private string bookName
        {
            get;
            set;
        }

        public string BookName
        {
            get
            {
                return this.bookName;
            }
            set
            {
                this.bookName = value;
                base.RaisePropertyChanged(()=>BookName);
            }
        }

        private string pageNumber
        {
            get;
            set;
        }

        public string PageNumber
        {
            get
            {
                return this.pageNumber;
            }
            set
            {
                this.pageNumber = value;
                base.RaisePropertyChanged(() => PageNumber);
            }
        }

        private string author
        {
            get;
            set;
        }

        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
                base.RaisePropertyChanged(()=> Author);
            }
        }

        private string collectionCode
        {
            get;
            set;
        }

        public string CollectionCode
        {
            get
            {
                return this.collectionCode;
            }
            set
            {
                this.collectionCode = value;
                base.RaisePropertyChanged(()=> CollectionCode);
            }
        }

        private string press
        {
            get;
            set;
        }

        public string Press
        {
            get
            {
                return this.press;
            }
            set
            {
                this.press = value;
                base.RaisePropertyChanged(()=> Press);
            }
        }

        private string classificationName
        {
            get;
            set;
        }

        public string ClassificationName
        {
            get
            {
                return this.classificationName;
            }
            set
            {
                this.classificationName = value;
                base.RaisePropertyChanged(()=>ClassificationName);
            }
        }

        private string pressDate
        {
            get;
            set;
        }

        public string PressDate
        {
            get
            {
                return this.pressDate;
            }
            set
            {
                this.pressDate = value;
                base.RaisePropertyChanged(()=>PressDate);
            }
        }

        private string rfid
        {
            get;
            set;
        }

        public string RFID
        {
            get
            {
                return this.rfid;
            }
            set
            {
                this.rfid = value;
                base.RaisePropertyChanged(() => RFID);
            }
        }

        private string marCode
        {
            get;
            set;
        }

        public string MarCode
        {
            get
            {
                return this.marCode;
            }
            set
            {
                this.marCode = value;
                base.RaisePropertyChanged(()=>MarCode);
            }
        }

        private string error
        {
            get;
            set;
        }

        public string Error
        {
            get
            {
                return this.error;
            }
            set
            {
                this.error = value;
                base.RaisePropertyChanged(()=> Error);
            }
        }

        private string color
        {
            get;
            set;
        }

        public string Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
                base.RaisePropertyChanged(()=> Color);
            }
        }

        private ICommand okCommand
        {
            get;
            set;
        }

        public ICommand OkCommand
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.okCommand) == null)
                {
                    arg_26_0 = (this.okCommand = new DelegateCommand<BingCorrectionMessageControl>(delegate (BingCorrectionMessageControl data)
                    {
                        Dictionary<string, object> keyValues = new Dictionary<string, object>();
                        keyValues.Add("id", this.info.id);
                        keyValues.Add("lendingPermission", 1);
                        keyValues.Add("dailyRent", 1);
                        keyValues.Add("place", this.info.Place);
                        keyValues.Add("callNumber", this.MarCode);
                        keyValues.Add("code", this.CollectionCode);
                        keyValues.Add("rfid", this.RFID);
                        bool flag = this.MarCode == this.info.CallNumber && this.CollectionCode == this.info.CorrectionCode && this.RFID == this.info.RFID;
                        if (!flag)
                        {
                            object errorMsg = keyValues;
                            BingCorrectionEditDAL editDAL = new BingCorrectionEditDAL();
                            editDAL.BingCorrectionEdit(ref errorMsg);
                            RetrunInfo Rinfo = errorMsg as RetrunInfo;
                            bool trueOrFalse = Rinfo.TrueOrFalse;
                            if (trueOrFalse)
                            {
                                this.Color = "#00FF00";
                                this.Error = Rinfo.result.ToString();
                                this.info.CallNumber = this.MarCode;
                                this.info.CorrectionCode = this.CollectionCode;
                                this.info.RFID = this.RFID;
                                data.info.CallNumber = this.MarCode;
                                data.info.CorrectionCode = this.CollectionCode;
                                data.info.RFID = this.RFID;
                                errorMsg = ServerSetting.parament;
                                SelectListDAL selectListDAL = new SelectListDAL();
                                bool flag2 = selectListDAL.SelectList(ServerSetting.@class, ref errorMsg);
                                if (flag2)
                                {
                                    RetrunInfo infoa = errorMsg as RetrunInfo;
                                    List<QueryInfo> Lists = infoa.result as List<QueryInfo>;
                                    ServerSetting.info = Lists;
                                    bool flag3 = Lists == null || Lists.Count <= 0;
                                    if (flag3)
                                    {
                                        ServerSetting.loadPage = 1;
                                    }
                                }
                                else
                                {
                                    ServerSetting.info = null;
                                    ServerSetting.loadPage = 1;
                                }
                            }
                            else
                            {
                                this.Color = "Red";
                                this.Error = Rinfo.result.ToString();
                                bool isOverDue = ServerSetting.IsOverDue;
                                if (isOverDue)
                                {
                                    ErrorPage errorPage = new ErrorPage(this.Error, this.control.mainWindow);
                                    DialogHelper.ShowDialog(errorPage);
                                }
                            }
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private ICommand canleCommand
        {
            get;
            set;
        }

        public ICommand CanleCommand
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.canleCommand) == null)
                {
                    arg_26_0 = (this.canleCommand = new DelegateCommand(delegate
                    {
                        BindingCorrectionControl bingCorrectionMessageControl = new BindingCorrectionControl(this.control);
                        this.control.Grid.Children.Clear();
                        this.control.Grid.Children.Add(bingCorrectionMessageControl);
                    }));
                }
                return arg_26_0;
            }
        }

        public BingCorrectionMessageViewModel(MainControl control, QueryInfo info)
        {
           
            this.control = control;
            this.info = info;
            Task.Run(()=>
            {

              BookName = info.BookName;

              PageNumber = info.PageNumber;

                Author = info.Author;

               CollectionCode = info.CorrectionCode;

               Press = info.Press;

               PressDate = info.PressDate;

               ClassificationName = info.ClassificationName;

               RFID = info.RFID;

               MarCode = info.CallNumber;
            });
        }
    }
}
