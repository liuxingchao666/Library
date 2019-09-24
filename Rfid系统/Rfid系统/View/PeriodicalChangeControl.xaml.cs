using Rfid系统.DAL;
using Rfid系统.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rfid系统.View
{
    /// <summary>
    /// PeriodicalChangeControl.xaml 的交互逻辑
    /// </summary>
    public partial class PeriodicalChangeControl : UserControl
    {
        public PeriodicalChangeControl(MainControl mainControl,string id)
        {
            InitializeComponent();
            this.mainControl = mainControl;
            ServerSetting.OldEPClist.Clear();
            ServerSetting.EPClist.Clear();
            thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {

                    if (ServerSetting.rfid.IsOpen())
                        ServerSetting.rfid.Start();
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        lock (ServerSetting.EPClist)
                        {
                            if (ServerSetting.EPClist.Count == 0)
                            {
                              //  EPC.Clear();
                            }
                            else
                            {
                                string epc = ServerSetting.EPClist.Dequeue();
                                ServerSetting.EPClist.Enqueue(epc);
                                EPC.Text = epc;
                            }
                        }
                    });
                    Thread.Sleep(500);
                }
            }));
            thread.IsBackground = true;
            Task.Run(() => {
                object errorMsg = id;
                SelectOneDAL selectOneDAL = new SelectOneDAL();
                if (selectOneDAL.SelectOne(ref errorMsg))
                {
                    RetrunInfo info = errorMsg as RetrunInfo;
                    if (info.TrueOrFalse)
                    {
                        this.Dispatcher.BeginInvoke((Action)delegate {
                            PeriodicalsInfo periodicalsInfo = info.result as PeriodicalsInfo;
                            Name.Text = periodicalsInfo.name;
                            fkTypeCode.Text = periodicalsInfo.fkTypeCode;
                            fkTypeName.Text = periodicalsInfo.fkTypeName;
                            fkPressName.Text = periodicalsInfo.fkPressName;
                            unifyNum.Text = periodicalsInfo.unifyNum;
                            parallelTitle.Text = periodicalsInfo.parallelTitle;
                            postIssueNumber.Text = periodicalsInfo.postIssueNumber;
                            openBook.Text = periodicalsInfo.openBook;
                            issnPrice.Text = periodicalsInfo.issnPrice;
                            releaseCycle.Text = periodicalsInfo.releaseCycle;
                            remark.Text = periodicalsInfo.remark;
                            fkCataPeriodicalId = periodicalsInfo.id;

                            this.id = periodicalsInfo.collectionInfo.id;
                            code.Text = periodicalsInfo.collectionInfo.code;
                            callNumber.Text = periodicalsInfo.collectionInfo.callNumber;
                            issn.Text = periodicalsInfo.issn;

                            Remark.Text= periodicalsInfo.pNInfo.remark;
                            price.Text = periodicalsInfo.pNInfo.price;
                            page.Text = periodicalsInfo.pNInfo.page;
                            aNumber.Content = periodicalsInfo.pNInfo.aNumber;
                            sNumber.Text = periodicalsInfo.pNInfo.sNumber;
                            date.Text = periodicalsInfo.pNInfo.publicationDateStr;
                            periodicalTbNumberId = periodicalsInfo.pNInfo.fkCataPeriodicalId;

                            ISBNbookListInfo = new ISBNbookListInfo()
                            {
                                fkTypeCode = periodicalsInfo.collectionInfo.callNumber.Split('/')[0].ToString(),
                                OrderNum = periodicalsInfo.collectionInfo.callNumber.Split('/')[1].ToString()
                            };
                            EPC.Text = periodicalsInfo.collectionInfo.RFID;
                            if (periodicalsInfo.collectionInfo.available.Equals("0"))
                                available.IsChecked = false;
                            else
                                available.IsChecked = true;
                            if (periodicalsInfo.collectionInfo.lendingPermission.Equals("0"))
                                lendingPermission.IsChecked = false;
                            else
                                lendingPermission.IsChecked = true;

                            GetPlaceListDAL listDAL = new GetPlaceListDAL();
                            List<PlaceInfo> PlaceList = new List<PlaceInfo>();
                            if (listDAL.GetPlaceList(ref errorMsg))
                            {
                                RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                                PlaceList = retrunInfo.result as List<PlaceInfo>;
                                place.ItemsSource = PlaceList;
                            }
                            foreach (var place in PlaceList)
                            {
                                if (place.id == periodicalsInfo.collectionInfo.placeCode)
                                    this.place.SelectedItem = place;
                            }
                        });
                    }
                    else
                    {
                        if (ServerSetting.IsOverDue)
                        {
                            ErrorPage errorPage = new ErrorPage(info.result.ToString(), mainControl.mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                        }
                        else
                        {
                            MessageBox.Show("失败提示:" + info.result.ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("失败提示："+errorMsg.ToString());
                }
            });
        }
        public Thread thread;
        public ISBNbookListInfo ISBNbookListInfo;
        public MainControl mainControl;
        public string fkCataPeriodicalId;
        public string periodicalTbNumberId;
        public string id;
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            lock (ServerSetting.EPClist)
            {
                if (string.IsNullOrEmpty(code.Text))
                    return;
                if (string.IsNullOrEmpty(EPC.Text))
                    return;
                string epc = ServerSetting.EPClist.Dequeue();
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                if ((bool)available.IsChecked)
                    keyValuePairs.Add("available", 1);
                else
                    keyValuePairs.Add("available", 0);
                if ((bool)lendingPermission.IsChecked)
                    keyValuePairs.Add("lendingPermission", 1);
                else
                    keyValuePairs.Add("lendingPermission", 0);

                keyValuePairs.Add("callNumber",callNumber.Text);
                keyValuePairs.Add("code", code.Text);
                keyValuePairs.Add("rfid", epc);

                PlaceInfo placeInfo = place.SelectedItem as PlaceInfo;
                keyValuePairs.Add("placeCode", placeInfo.id);
                keyValuePairs.Add("fkCataPeriodicalId",fkCataPeriodicalId);
                keyValuePairs.Add("pNumberId", periodicalTbNumberId);
                keyValuePairs.Add("id",id);

                PeriadicalChangeDAL periadical = new PeriadicalChangeDAL();
                object errorMsg = keyValuePairs;
                if (periadical.PeriadicalChange(ref errorMsg))
                {
                    RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                    if (retrunInfo.TrueOrFalse)
                    {
                       BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#00E08E"));
                        BindState.Content = "修改成功";
                    }
                    else
                    {
                        BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                        BindState.Content = "修改失败";
                        if (ServerSetting.IsOverDue)
                        {
                            ErrorPage errorPage = new ErrorPage(errorMsg.ToString(), mainControl.mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                        }
                    }
                }
                else
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "修改失败";
                }
            }
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            QueryControl queryControl = new QueryControl(mainControl);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(queryControl);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void Place_GotFocus(object sender, RoutedEventArgs e)
        {
            code.Focus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            thread.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetPNDAL getPN = new GetPNDAL();
            object errorMsg = fkCataPeriodicalId;
            if (getPN.GetPN(ref errorMsg))
            {
                RetrunInfo info = errorMsg as RetrunInfo;
                if (info.TrueOrFalse)
                {
                    ///弹出列表
                    List<PNInfo> pNInfos = info.result as List<PNInfo>;
                    PNChooseControl pNChooseControl = new PNChooseControl(pNInfos, mainControl.mainWindow);
                    pNChooseControl.fkCataPeriodicalId = fkCataPeriodicalId;
                    DialogHelper.ShowDialog(pNChooseControl);
                    if (pNChooseControl.info != null)
                    {
                        aNumber.Content = pNChooseControl.info.aNumber;
                        sNumber.Text = pNChooseControl.info.sNumber;
                        price.Text = pNChooseControl.info.price;
                        page.Text = pNChooseControl.info.page;
                        Remark.Text = pNChooseControl.info.remark;
                        date.Text = pNChooseControl.info.publicationDateStr;
                        periodicalTbNumberId = pNChooseControl.info.fkCataPeriodicalId;
                    }
                }
                else
                {
                    if (ServerSetting.IsOverDue)
                    {
                        ErrorPage errorPage = new ErrorPage(info.result.ToString(), mainControl.mainWindow);
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
        }
    }
}
