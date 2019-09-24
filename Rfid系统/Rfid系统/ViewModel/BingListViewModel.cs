using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rfid系统.ViewModel
{
    public class BingListViewModel : NotificationObject
    {
        public MainControl control;

        private string parameter
        {
            get;
            set;
        }

        public string Parameter
        {
            get
            {
                return this.parameter;
            }
            set
            {
                this.parameter = value;
                base.RaisePropertyChanged(()=> Parameter);
            }
        }

        private int selectIndex
        {
            get;
            set;
        }

        public int SelectIndex
        {
            get
            {
                return this.selectIndex;
            }
            set
            {
                this.selectIndex = value;
                base.RaisePropertyChanged(()=> SelectIndex);
            }
        }

        private ICommand input
        {
            get;
            set;
        }

        public ICommand Input
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.input) == null)
                {
                    arg_26_0 = (this.input = new DelegateCommand<TextBox>(delegate (TextBox data)
                    {
                        bool flag = string.IsNullOrEmpty(data.Text);
                        if (!flag)
                        {
                            object errorMsg = data.Text.ToString();
                            ServerSetting.parament = errorMsg.ToString();
                            int selectIndex = this.SelectIndex;
                            SelectClass selectClass;
                            if (selectIndex != 0)
                            {
                                if (selectIndex != 1)
                                {
                                    selectClass = SelectClass.isbn;
                                }
                                else
                                {
                                    selectClass = SelectClass.CorrectionCode;
                                }
                            }
                            else
                            {
                                selectClass = SelectClass.callNumber;
                            }
                            ServerSetting.parament = data.Text;
                            ServerSetting.@class = selectClass;
                            ServerSetting.loadPage = 1;
                            this.GetData();
                            data.SelectAll();
                            data.Focus();
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private List<QueryInfo> list
        {
            get;
            set;
        }

        public List<QueryInfo> List
        {
            get
            {
                return this.list;
            }
            set
            {
                this.list = value;
                base.RaisePropertyChanged(()=> List);
            }
        }

        private ICommand click
        {
            get;
            set;
        }

        public ICommand Click
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.click) == null)
                {
                    arg_26_0 = (this.click = new DelegateCommand<DataGrid>(delegate (DataGrid data)
                    {
                        bool flag = data.SelectedIndex < 0;
                        if (!flag)
                        {
                            QueryInfo info = data.SelectedItem as QueryInfo;
                            if (info.bop == "图书")
                            {
                                BingCorrectionMessageControl detailsControl = new BingCorrectionMessageControl(this.control, info);
                                this.control.Grid.Children.Clear();
                                this.control.Grid.Children.Add(detailsControl);
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

                                }
                            }
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private string loadPage
        {
            get;
            set;
        }

        public string LoadPage
        {
            get
            {
                return this.loadPage;
            }
            set
            {
                this.loadPage = value;
                base.RaisePropertyChanged(()=> LoadPage);
            }
        }

        private ICommand firstCommand
        {
            get;
            set;
        }

        public ICommand FirstCommand
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.firstCommand) == null)
                {
                    arg_26_0 = (this.firstCommand = new DelegateCommand(delegate
                    {
                        bool flag = ServerSetting.totalPages <= 1;
                        if (!flag)
                        {
                            bool flag2 = ServerSetting.loadPage <= 1;
                            if (!flag2)
                            {
                                ServerSetting.loadPage = 1;
                                this.GetData();
                            }
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private ICommand finallyCommand
        {
            get;
            set;
        }

        public ICommand FinallyCommand
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.finallyCommand) == null)
                {
                    arg_26_0 = (this.finallyCommand = new DelegateCommand(delegate
                    {
                        bool flag = ServerSetting.totalPages <= 1 || ServerSetting.loadPage == ServerSetting.totalPages;
                        if (!flag)
                        {
                            ServerSetting.loadPage = ServerSetting.totalPages;
                            this.GetData();
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private ICommand lastCommand
        {
            get;
            set;
        }

        public ICommand LastCommand
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.lastCommand) == null)
                {
                    arg_26_0 = (this.lastCommand = new DelegateCommand(delegate
                    {
                        bool flag = ServerSetting.totalPages <= 1 || ServerSetting.loadPage <= 1;
                        if (!flag)
                        {
                            ServerSetting.loadPage--;
                            this.GetData();
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private ICommand nextCommand
        {
            get;
            set;
        }

        public ICommand NextCommand
        {
            get
            {
                ICommand arg_26_0;
                if ((arg_26_0 = this.nextCommand) == null)
                {
                    arg_26_0 = (this.nextCommand = new DelegateCommand(delegate
                    {
                        bool flag = ServerSetting.totalPages <= 1 || ServerSetting.loadPage == ServerSetting.totalPages;
                        if (!flag)
                        {
                            ServerSetting.loadPage++;
                            this.GetData();
                        }
                    }));
                }
                return arg_26_0;
            }
        }

        private BitmapImage picState
        {
            get;
            set;
        }

        public BitmapImage PICState
        {
            get
            {
                return this.picState;
            }
            set
            {
                this.picState = value;
                base.RaisePropertyChanged(()=> PICState);
            }
        }

        private Visibility picVisible
        {
            get;
            set;
        }

        public Visibility PICVisible
        {
            get
            {
                return this.picVisible;
            }
            set
            {
                this.picVisible = value;
                base.RaisePropertyChanged(()=> PICVisible);
            }
        }

        private Visibility dataVisible
        {
            get;
            set;
        }

        public Visibility DataVisible
        {
            get
            {
                return this.dataVisible;
            }
            set
            {
                this.dataVisible = value;
                base.RaisePropertyChanged(()=> DataVisible);
            }
        }

        public BingListViewModel(MainControl mainControl)
        {
            this.control = mainControl;
            bool flag = ServerSetting.info != null;
            if (flag)
            {
                this.DataVisible = Visibility.Visible;
                this.List = ServerSetting.info;
                this.LoadPage = ServerSetting.loadPage.ToString();
                this.PICVisible = Visibility.Hidden;
            }
            else
            {
                this.PICVisible = Visibility.Visible;
                this.PICState = new BitmapImage(new Uri("../Images/无数据.jpg", UriKind.RelativeOrAbsolute));
                this.DataVisible = Visibility.Hidden;
            }
        }

        public void GetData()
        {
            object errorMsg = ServerSetting.parament;
            SelectListDAL selectListDAL = new SelectListDAL();
            bool flag = selectListDAL.SelectList(ServerSetting.@class, ref errorMsg);
            if (flag)
            {
                RetrunInfo info = errorMsg as RetrunInfo;
                List<QueryInfo> Lists = info.result as List<QueryInfo>;
                bool flag2 = Lists.Count > 0;
                if (flag2)
                {
                    this.PICVisible = Visibility.Hidden;
                    this.DataVisible = Visibility.Visible;
                    ServerSetting.info = Lists;
                    this.List = Lists;
                    this.LoadPage = ServerSetting.loadPage.ToString();
                }
                else
                {
                    this.PICVisible = Visibility.Visible;
                    this.PICState = new BitmapImage(new Uri("../Images/无数据.jpg", UriKind.RelativeOrAbsolute));
                    this.DataVisible = Visibility.Hidden;
                    ServerSetting.totalPages = 0;
                    ServerSetting.List = null;
                    ServerSetting.loadPage = 1;
                    ServerSetting.info = null;
                }
            }
            else
            {
                this.PICVisible = Visibility.Visible;
                this.PICState = new BitmapImage(new Uri("../Images/未连接.png", UriKind.RelativeOrAbsolute));
                this.DataVisible = Visibility.Hidden;
                bool isOverDue = ServerSetting.IsOverDue;
                if (isOverDue)
                {
                    RetrunInfo info2 = errorMsg as RetrunInfo;
                    ErrorPage errorPage = new ErrorPage(info2.result.ToString(), this.control.mainWindow);
                    DialogHelper.ShowDialog(errorPage);
                }
            }
        }
    }
}