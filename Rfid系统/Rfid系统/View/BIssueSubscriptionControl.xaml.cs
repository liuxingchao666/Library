using Rfid系统.DAL;
using Rfid系统.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Resources.ResXFileRef;

namespace Rfid系统.View
{
    /// <summary>
    /// BIssueSubscription_Control.xaml 的交互逻辑
    /// </summary>
    public partial class BIssueSubscription_Control : UserControl
    {
        public BIssueSubscription_Control(MainControl mainControl, string id)
        {
            InitializeComponent();
            EPC.IsReadOnly = true;
            this.mainControl = mainControl;
            this.EditId = id;

            if (!string.IsNullOrEmpty(id))
                IsEdit = true;
            string placeId = ConfigurationManager.AppSettings["PlaceId"];
            GetPlaceListDAL listDAL = new GetPlaceListDAL();
            object errorMsg = null;
            ServerSetting.OldEPClist.Clear();
            ServerSetting.EPClist.Clear();
            PlaceInfo Place = new PlaceInfo();
            List<PlaceInfo> PlaceList = new List<PlaceInfo>();
            if (listDAL.GetPlaceList(ref errorMsg))
            {
                RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                PlaceList = retrunInfo.result as List<PlaceInfo>;
                place.ItemsSource = PlaceList;
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
                        place.SelectedItem = Place;
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
                            place.SelectedItem = Place;
                        }
                        catch { }
                    }
                }
            }

           mainControl.thread = new Thread(new ThreadStart(() =>
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
                                // EPC.Clear();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(EPC.Text))
                                {
                                    string epc = ServerSetting.EPClist.Dequeue();
                                    ServerSetting.EPClist.Enqueue(epc);

                                    EPC.Text = epc;
                                }
                                if (!string.IsNullOrEmpty(EditId))
                                {
                                    string epc = ServerSetting.EPClist.Dequeue();
                                    ServerSetting.EPClist.Enqueue(epc);

                                    EPC.Text = epc;
                                }
                            }
                        }
                    });
                    Thread.Sleep(500);
                }
            }));
            mainControl.thread.IsBackground = true;
            Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(id))
                {
                    errorMsg = id;
                    SelectHDOneDAL selectHDOneDAL = new SelectHDOneDAL();
                    if (selectHDOneDAL.SelectHDOne(ref errorMsg))
                    {
                        this.Dispatcher.BeginInvoke((Action)delegate
                        {
                            backBtn.Visibility = Visibility.Visible;
                            RetrunInfo info = errorMsg as RetrunInfo;
                            if (info.TrueOrFalse)
                            {
                                PeriodicalsInfo periodicalsInfo = info.result as PeriodicalsInfo;
                                this.info = periodicalsInfo;
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
                                this.id = periodicalsInfo.id;

                                hkPrice.Text = periodicalsInfo.collectionInfo.hkPrice;
                                hkRemark.Text = periodicalsInfo.collectionInfo.hkRemark;
                                code.Text = periodicalsInfo.collectionInfo.code;
                                callNumber.Text = periodicalsInfo.collectionInfo.callNumber;
                                issnMsg.Visibility = Visibility.Hidden;
                                issn.Text = periodicalsInfo.issn;
                                EditId = periodicalsInfo.collectionInfo.id;
                                ISBNbookListInfo = new ISBNbookListInfo()
                                {
                                    fkTypeCode = periodicalsInfo.collectionInfo.callNumber.Split('/')[0].ToString(),
                                    OrderNum = periodicalsInfo.collectionInfo.callNumber.Split('/')[1].ToString()
                                };
                                EPC.Text = periodicalsInfo.collectionInfo.RFID;
                                grid.ItemsSource = periodicalsInfo.pNInfos;
                                if (periodicalsInfo.collectionInfo.available.Equals("0"))
                                    available.IsChecked = false;
                                else
                                    available.IsChecked = true;
                                if (periodicalsInfo.collectionInfo.lendingPermission.Equals("0"))
                                    lendingPermission.IsChecked = false;
                                else
                                    lendingPermission.IsChecked = true;
                                foreach (var place in PlaceList)
                                {
                                    if (place.id == periodicalsInfo.collectionInfo.placeCode)
                                        this.place.SelectedItem = place;
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
                                    MessageBox.Show("失败提示:" + info.result.ToString());
                                }
                            }
                        });
                    }
                    else
                    {
                        MessageBox.Show(errorMsg.ToString());
                    }
                }
            });
        }
        public Thread thread;
        public string id;
        public string EditId;
        public PeriodicalsInfo info;
        public MainControl mainControl;
        public List<HDDCQKInfo> infos;
        public ISBNbookListInfo ISBNbookListInfo;
        public bool IsEdit = false;
        public CallNumberInfo callNumberInfo;
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            issn.Focus();
        }

        private void Issn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(issn.Text))
                issnMsg.Visibility = Visibility.Visible;
            else
                issnMsg.Visibility = Visibility.Hidden;
        }

        private void IssnMsg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            issn.Focus();
        }

        public void Flush()
        {
            if (IsEdit)
                return;
            lock (issn.Text)
            {
                object isbn = issn.Text;
                SelectLocalDAL localDAL = new SelectLocalDAL();
                if (localDAL.SelectLoacl(ref isbn))
                {
                    RetrunInfo info = isbn as RetrunInfo;
                    if (info.TrueOrFalse)
                    {
                        List<PeriodicalsInfo> infos = info.result as List<PeriodicalsInfo>;
                        PeriodicalChooseControl periodicalChooseControl = new PeriodicalChooseControl(infos);
                        DialogHelper.ShowDialog(periodicalChooseControl);
                        if (periodicalChooseControl.info != null)
                        {
                            Name.Text = periodicalChooseControl.info.name;
                            fkTypeCode.Text = periodicalChooseControl.info.fkTypeCode;
                            fkTypeName.Text = periodicalChooseControl.info.fkTypeName;
                            fkPressName.Text = periodicalChooseControl.info.fkPressName;
                            unifyNum.Text = periodicalChooseControl.info.unifyNum;
                            parallelTitle.Text = periodicalChooseControl.info.parallelTitle;
                            postIssueNumber.Text = periodicalChooseControl.info.postIssueNumber;
                            openBook.Text = periodicalChooseControl.info.openBook;
                            issnPrice.Text = periodicalChooseControl.info.issnPrice;
                            releaseCycle.Text = periodicalChooseControl.info.releaseCycle;
                            remark.Text = periodicalChooseControl.info.remark;
                            this.id = periodicalChooseControl.info.id;
                            GetCallNumberByIdDAL getCsDAL = new GetCallNumberByIdDAL();
                            object errorMsg = periodicalChooseControl.info.id;
                            if (getCsDAL.GetCallNumberById(ref errorMsg))
                            {
                                RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                                if (retrunInfo.TrueOrFalse)
                                {
                                    this.callNumberInfo = retrunInfo.result as CallNumberInfo;
                                    if (combox.SelectedIndex == 0)
                                        callNumber.Text = this.callNumberInfo.searchNumberOrderNum;
                                    else
                                        callNumber.Text = this.callNumberInfo.searchNumberAuthorNum;
                                }
                            }
                            grid.ItemsSource = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("失败提示：" + info.result);
                    }
                }
            }
        }

        private void Issn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Flush();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Flush();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                issn.IsReadOnly = true;
            if (!string.IsNullOrEmpty(EditId))
            {
                issn.IsReadOnly = true;
                issnMsg.Visibility = Visibility.Hidden;
                backBtn.Visibility = Visibility.Visible;
            }
           mainControl.thread.Start();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            mainControl.thread.Abort();
            ///返回查询
            QueryControl queryControl = new QueryControl(mainControl);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(queryControl);
        }
        public string GetState(string lendState)
        {
            switch (lendState)
            {
                case "0":
                    return "不在架";
                case "1":
                    return "在架";
                case "2":
                    return "借出";
                default:
                    return "";
            }
        }
        private void ManageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(id))
                return;
            object errorMsg = id;
            GetHDDCQKDAL getHDDCQKDAL = new GetHDDCQKDAL();
            if (getHDDCQKDAL.GetHDDCQK(ref errorMsg))
            {
                RetrunInfo info = errorMsg as RetrunInfo;
                if (info.TrueOrFalse)
                {
                    this.infos = info.result as List<HDDCQKInfo>;
                    List<HDDCQKInfo> lists = grid.ItemsSource as List<HDDCQKInfo>;
                    if (lists != null && lists.Count > 0)
                    {
                        foreach (var temp in lists)
                        {
                            foreach (var emp in infos)
                            {
                                if (temp.id == emp.id)
                                    emp.IsCheck = true;
                            }
                        }
                        var temps = (from c in lists
                                     where !(from d in infos
                                             select d.id).Contains(c.id)
                                     select c
                      ).ToList();
                        foreach (var temp in temps)
                        {
                            temp.IsCheck = true;
                            infos.Add(temp);
                        }
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
            FrequencyChooseControl frequencyChooseControl = new FrequencyChooseControl(infos);
            //DialogHelper.ShowDialog(frequencyChooseControl);
            frequencyChooseControl.ShowDialog();
            List<HDDCQKInfo> list = new List<HDDCQKInfo>();
            int i = 1;
            double countPrice = 0;
            foreach (var temp in frequencyChooseControl.infos)
            {
                HDDCQKInfo info = new HDDCQKInfo()
                {
                    number = i,
                    anumber = temp.anumber,
                    snumber = temp.snumber,
                    price = temp.price,
                    code = temp.code,
                    id = temp.id,
                    lendState = GetState(temp.lendState),
                    callNumber = temp.callNumber
                };
                countPrice = info.price.ToDouble() + countPrice;
                list.Add(info);
                i++;
            }
            grid.ItemsSource = null;
            grid.ItemsSource = list;
            CountPrice.Content = "总价:" + countPrice;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }


        private void CallBtn_Click(object sender, RoutedEventArgs e)
        {
            CallNumberControl callNumber = new CallNumberControl(ISBNbookListInfo);
            DialogHelper.ShowDialog(callNumber);
            this.callNumber.Text = callNumber.info.fkTypeCode + "/" + callNumber.info.SetBooks;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lock (ServerSetting.EPClist)
            {
                if (place.SelectedIndex < 0)
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "未选择馆藏地";
                    return;
                }
                
                if (string.IsNullOrEmpty(id))
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "未选中需要合刊的刊期";
                    return;
                }
                if (string.IsNullOrEmpty(callNumber.Text))
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "索取号不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(EPC.Text))
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "未扫描到可用RFID";
                    return;
                }
                if (string.IsNullOrEmpty(code.Text))
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "书籍编码不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(EPC.Text))
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "未扫描到可用RFID";
                    return;
                }
                List<HDDCQKInfo> infos = grid.ItemsSource as List<HDDCQKInfo>;
                if (infos == null || infos.Count() <= 0)
                {
                    BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                    BindState.Content = "未选中需要添加如刊期的子刊";
                    return;
                }

                string epc = null;
                if (ServerSetting.EPClist.Count <= 0)
                    epc = EPC.Text;
                else
                    epc = ServerSetting.EPClist.Dequeue();
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
              
                if ((bool)available.IsChecked)
                    keyValuePairs.Add("available", 1);
                else
                    keyValuePairs.Add("available", 0);
                if ((bool)lendingPermission.IsChecked)
                    keyValuePairs.Add("lendingPermission", 1);
                else
                    keyValuePairs.Add("lendingPermission", 0);
                var list = (from c in infos
                            select c.id).ToArray();
                keyValuePairs.Add("rfid", epc);
                keyValuePairs.Add("callNumber", callNumber.Text);
                keyValuePairs.Add("fkCataPeriodicalId", id);
                keyValuePairs.Add("code", code.Text);
                PlaceInfo placeInfo = place.SelectedItem as PlaceInfo;
                keyValuePairs.Add("placeCode", placeInfo.id);
                keyValuePairs.Add("ids", list);
                keyValuePairs.Add("hkPrice", hkPrice.Text.ToInt());
                keyValuePairs.Add("hkRemark", hkRemark.Text);
                if (!IsEdit)
                {
                    object errorMsg = keyValuePairs;
                    ///接口
                    AddHDDAL addHDDAL = new AddHDDAL();
                    EPC.Clear();
                    if (addHDDAL.AddHD(ref errorMsg))
                    {
                        RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                        if (retrunInfo.TrueOrFalse)
                        {
                            BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#00E08E"));
                            BindState.Content = "已绑定";
                            grid.ItemsSource = null;
                            code.Clear();
                            EPC.Clear();
                        }
                        else
                        {
                            BindState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)"#FF2E2E"));
                            BindState.Content = "未绑定";
                            EPC.Clear();
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
                        BindState.Content = "未绑定";
                    }
                    EPC.Clear();
                }
                else
                {
                    keyValuePairs.Add("id", EditId);
                    object errorMsg = keyValuePairs;
                    ///接口
                    HDEditDAL addHDDAL = new HDEditDAL();
                    if (addHDDAL.HDEdit(ref errorMsg))
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
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                code.SelectAll();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            // thread.Suspend();
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //thread.Resume();
        }

        private void Place_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                hkPrice.Focus();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mainControl.thread.Abort();
            HDHistroyControl hDHistroyControl = new HDHistroyControl(mainControl, id);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(hDHistroyControl);
        }

        private void Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (combox.SelectedIndex == 1)
                {
                    callNumber.Text = callNumberInfo.searchNumberAuthorNum;
                }
                else
                {
                    callNumber.Text = callNumberInfo.searchNumberOrderNum;
                }
            }
            catch { }
        }
    }
}
