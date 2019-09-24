using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using WpfApp2.View;
using System.Collections;
using WpfApp2.Controls;
using WpfApp2.Controls.ShowControl;
using WpfApp2.DAL;
using System.Threading;

namespace WpfApp2.ViewModel
{
    public class MainViewModel : NotificationObject
    {
        public Thread thread;
        public MainPage page;
        public MainViewModel(MainPage Page)
        {
            page = Page;
            if (!ServerSeting.connState)
            {
                Connstate = Visibility.Visible;
            }
            else
            {
                Connstate = Visibility.Hidden;
            }
            thread = new Thread(new ThreadStart(deal));
            thread.IsBackground = true;
            thread.Start();
        }
        public void deal()
        {
            int index = 0;
            try
            {
                while (true)
                {
                    List<string> statList = new List<string>();
                    string conn = "未连接服务器";
                    ///服务器
                    if (!ServerSeting.connState)
                        statList.Add(conn);
                    string TIC = "吐卡器读头正在启动...";
                    ///IC卡扫描
                    if (!ServerSeting.ICConnState)
                        statList.Add(TIC);
                    ///进钞机
                    if (!ServerSeting.MConnState)
                        statList.Add("进钞机正在启动...");
                    ///吐卡机
                    if (!ServerSeting.TConnState)
                        statList.Add("吐卡器正在启动...");
                    ///身份证
                    if (!ServerSeting.IDConnState)
                        statList.Add("身份证识别器正在启动...");
                    if (ServerSeting.warehouseIsNull)
                        statList.Add(" 吐卡机卡储量为空！！请加卡");
                    State = "";
                    state = "";
                    index = 0;
                    foreach (string temp in statList)
                    {
                        index++;
                        if (index == statList.Count)
                        {
                            State += temp;
                            state += temp;
                        }
                        else
                        {
                            State += temp + "  ";
                            state += temp + "  ";
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Visibility isNull { get; set; }
        public Visibility IsNull
        {
            get { return isNull; }
            set
            {
                isNull = value;
                this.RaisePropertyChanged(() => IsNull);
            }
        }

        private ICommand _SelectCommand;
        public ICommand SelectCommand
        {
            get
            {
                return _SelectCommand ?? (_SelectCommand = new DelegateCommand<Button>((data) =>
                {
                    QueryShow();
                }));
            }
        }
        private ICommand _RenewCommand;
        public ICommand RenewCommand
        {
            get
            {
                return _RenewCommand ?? (_RenewCommand = new DelegateCommand<Button>((data) =>
                {
                    RenewShow();
                }));
            }
        }
        private ICommand _PatchCardCommand;
        public ICommand PatchCardCommand
        {
            get
            {
                return _PatchCardCommand ?? (_PatchCardCommand = new DelegateCommand<Button>((data) =>
                {
                    PatchCardShow();
                }));
            }
        }
        private ICommand _handleCommand;
        public ICommand handleCommand
        {
            get
            {
                return _handleCommand ?? (_handleCommand = new DelegateCommand<Button>((data) =>
                {
                    handleShow();
                }));
            }
        }
        private Visibility connstate { get; set; }
        public Visibility Connstate
        {
            get { return connstate; }
            set
            {
                connstate = value;
                this.RaisePropertyChanged(() => Connstate);
            }
        }

        private Visibility TICconnstate { get; set; }
        public Visibility TICConnstate
        {
            get { return TICconnstate; }
            set
            {
                TICconnstate = value;
                this.RaisePropertyChanged(() => TICConnstate);
            }
        }

        private Visibility Tconnstate { get; set; }
        public Visibility TConnstate
        {
            get { return Tconnstate; }
            set
            {
                Tconnstate = value;
                this.RaisePropertyChanged(() => TConnstate);
            }
        }

        private Visibility IDconnstate { get; set; }
        public Visibility IDConnstate
        {
            get { return IDconnstate; }
            set
            {
                IDconnstate = value;
                this.RaisePropertyChanged(() => IDConnstate);
            }
        }

        private Visibility Mconnstate { get; set; }
        public Visibility MConnstate
        {
            get { return Mconnstate; }
            set
            {
                Mconnstate = value;
                this.RaisePropertyChanged(() => MConnstate);
            }
        }
        private string State { get; set; }
        public string state
        {
            get { return State; }
            set
            {
                value = State;
                this.RaisePropertyChanged(() => state);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void QueryShow()
        {
            ServerSeting.ISAdd = false;
            page.QueryControl = new QueryControl(page);
            page.Grid.Children.Clear();
            page.Grid.Children.Add(page.QueryControl);
        }
        /// <summary>
        /// 续借
        /// </summary>
        public void RenewShow()
        {
            ServerSeting.ISAdd = false;
            try
            {
                if (!ServerSeting.connState)
                {
                    errorPage errorPage = new errorPage("未连接服务器");
                    errorPage.ShowDialog();
                    return;
                }
                if (!ServerSeting.IDConnState)
                {
                    errorPage errorPage = new errorPage("身份证设备连接异常，请检查");
                    errorPage.ShowDialog();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            RenewShowControl renewShowControl = new RenewShowControl(page);
            renewShowControl.ShowDialog();
            //RenewPage renewPage = new RenewPage(page);
            //RenewShowControl control = new RenewShowControl(renewPage);
            //renewPage.GRID.Children.Add(control);
            //renewPage.isClosed = true;
            //renewPage.ShowDialog();

            //if (!string.IsNullOrEmpty(renewPage.CardNum))
            //{
            //    page.CardNum = renewPage.CardNum;
            //    RenewListControl control1 = new RenewListControl(page);
            //    control1.user = renewPage.RenewUser;
            //    page.Grid.Children.Clear();
            //    page.Grid.Children.Add(control1);
            //}
        }
        /// <summary>
        /// 办卡
        /// </summary>
        public void handleShow()
        {
            try
            {
                ServerSeting.OrderNumber = "";
                ServerSeting.ISAdd = false;
                if (!JudgeAction())
                    return;

                handleShowControl control = new handleShowControl(page);
                control.ShowDialog();
            }
            catch (Exception ex){ }
        }
        /// <summary>
        /// 挂失或者补办
        /// </summary>
        public void PatchCardShow()
        {
            ServerSeting.OrderNumber = "";
            ServerSeting.ISAdd = false;
            try
            {
                if (!ServerSeting.connState)
                {
                    errorPage errorPage = new errorPage("未连接服务器");
                    errorPage.ShowDialog();
                    return;
                }
                if (!ServerSeting.IDConnState)
                {
                    errorPage errorPage = new errorPage("身份证设备连接异常，请检查");
                    errorPage.ShowDialog();
                    return;
                }

                RenewPage renewPage = new RenewPage(page);
                PatchCardShowControl control = new PatchCardShowControl(renewPage);
                renewPage.GRID.Children.Add(control);
                renewPage.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool JudgeAction()
        {
            try
            {
                lock (this)
                {
                    if (!ServerSeting.connState)
                    {
                        errorPage errorPage = new errorPage("未连接服务器");
                        errorPage.ShowDialog();
                        return false;
                    }
                    if (ServerSeting.warehouseIsNull)
                    {
                        errorPage errorPage = new errorPage("卡储量为空");
                        errorPage.ShowDialog();
                        return false;
                    }
                    if (!ServerSeting.TConnState)
                    {
                        errorPage errorPage = new errorPage("吐卡机连接异常，请检查");
                        errorPage.ShowDialog();
                        return false;
                    }
                    if (!ServerSeting.bFeeder.serialPort.IsOpen)
                    {
                        errorPage errorPage = new errorPage("进钞机连接异常，请检查");
                        errorPage.ShowDialog();
                        return false;
                    }
                    if (!ServerSeting.IDConnState)
                    {
                        errorPage errorPage = new errorPage("身份证设备连接异常，请检查");
                        errorPage.ShowDialog();
                        return false;
                    }
                    if (!ServerSeting.ICConnState)
                    {
                        errorPage errorPage = new errorPage("IC卡读写设备连接异常，请检查");
                        errorPage.ShowDialog();
                        return false;
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            return true;
        }
    }
}
