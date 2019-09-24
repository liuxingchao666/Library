using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// PeriodicalControl.xaml 的交互逻辑
    /// </summary>
    public partial class PeriodicalControl : UserControl
    {
        public PeriodicalControl(MainControl mainControl)
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new PeriodicalViewModel(this);
            this.mainControl = mainControl;
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
                            if (ServerSetting.EPClist.Count == 0 && string.IsNullOrEmpty(EPC.Text))
                            {
                                BookCode.Text = "请先扫描RFID";
                                BookCode.IsReadOnly = true;
                                EPC.Clear();
                            }
                            else
                            {
                                if (BookCode.IsReadOnly)
                                {
                                    BookCode.IsReadOnly = false;
                                    BookCode.Text = "";
                                    BookCode.Focus();
                                    lock (ServerSetting.EPClist)
                                    {
                                        string epc = ServerSetting.EPClist.Dequeue();
                                        ServerSetting.EPClist.Enqueue(epc);
                                        EPC.Text = epc;
                                    }
                                }
                            }
                        }
                    });
                    Thread.Sleep(500);
                }
            }));
            mainControl.thread.IsBackground = true;
        }
        public bool isPrintSet = false;
        public Thread thread;
        public PeriodicalInfo periodicalInfo;
        public MainControl mainControl;
        public ISBNbookListInfo ISBNbookListInfo;
        public PNInfo pNInfo;
        public CallNumberInfo info;
        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="callNumber"></param>
        public void AddCell(List<MarCodeInfo> infos, string callNumber)
        {
            bool result = true;
            foreach (MarCodeInfo info in infos)
            {
                if (string.IsNullOrEmpty(info.MarCode))
                {
                    info.MarCode = callNumber;
                    info.VisibleState = Visibility.Visible;
                    info.PIC = GetBitmapImage(callNumber);
                    info.PrintPic = GetPrintBitmapImage(callNumber);
                    break;
                }
                else if (string.IsNullOrEmpty(info.MarCode1))
                {
                    info.MarCode1 = callNumber;
                    info.VisibleState1 = Visibility.Visible;
                    info.PIC1 = GetBitmapImage(callNumber);

                    info.PrintPic1 = GetPrintBitmapImage(callNumber);
                    break;
                }
                else if (string.IsNullOrEmpty(info.MarCode2))
                {
                    info.MarCode2 = callNumber;
                    info.VisibleState2 = Visibility.Visible;
                    info.PIC2 = GetBitmapImage(callNumber);

                    info.PrintPic2 = GetPrintBitmapImage(callNumber);
                    break;
                }
                else if (string.IsNullOrEmpty(info.MarCode3))
                {
                    info.MarCode3 = callNumber;
                    info.VisibleState3 = Visibility.Visible;
                    info.PIC3 = GetBitmapImage(callNumber);

                    info.PrintPic3 = GetPrintBitmapImage(callNumber);
                    result = false;
                    break;
                }
            }
            MarCodeList.ItemsSource = null;
            MarCodeList.ItemsSource = infos;

        }
        /// <summary>
        /// 得到文字图片
        /// </summary>
        /// <param name="callNumber"></param>
        /// <returns></returns>
        public BitmapImage GetBitmapImage(string callNumber)
        {
            BitmapImage images = null;
            var fontFamily = new System.Drawing.FontFamily("楷体");
            var fontStyle = System.Drawing.FontStyle.Bold;
            Font font = new Font(fontFamily, 120, fontStyle);

            using (System.Drawing.Image imgs = System.Drawing.Image.FromFile(@"001.png"))
            {
                using (Graphics g = Graphics.FromImage(imgs))
                {
                    int index = callNumber.IndexOf("/");
                    string str = callNumber.Substring(0, index + 1);
                    string str1 = "";
                    if (callNumber.Length > index + 1)
                    {
                        str1 = callNumber.Substring(index + 1, callNumber.Length - index - 1);
                    }
                    SizeF sizeF = g.MeasureString(str, font);
                    ; g.DrawString(str, font, System.Drawing.Brushes.Black, new PointF(imgs.Width / 2 - sizeF.Width / 2, imgs.Height / 4));
                    if (!string.IsNullOrEmpty(str1))
                    {
                        sizeF = g.MeasureString(str1, font);
                        g.DrawString(str1, font, System.Drawing.Brushes.Black, new PointF(imgs.Width / 2 - sizeF.Width / 2, imgs.Height / 2 + imgs.Height / 7));
                    }
                }
                images = ImageToBitmapImage(imgs);
            }
            return images;
        }
        /// <summary>
        /// 打印图
        /// </summary>
        /// <param name="callNumber"></param>
        /// <returns></returns>
        public BitmapImage GetPrintBitmapImage(string callNumber)
        {
            BitmapImage images = null;
            var fontFamily = new System.Drawing.FontFamily("宋体");
            var fontStyle = System.Drawing.FontStyle.Bold;
            Font font = new Font(fontFamily, 120, fontStyle);
            using (System.Drawing.Image imgs = System.Drawing.Image.FromFile("02.png"))
            {
                using (Graphics g = Graphics.FromImage(imgs))
                {
                    int index = callNumber.IndexOf("/");
                    string str = callNumber.Substring(0, index + 1);
                    string str1 = "";
                    if (callNumber.Length > index + 1)
                    {
                        str1 = callNumber.Substring(index + 1, callNumber.Length - index - 1);
                    }
                    SizeF sizeF = g.MeasureString(str, font);
                    ; g.DrawString(str, font, System.Drawing.Brushes.Black, new PointF(imgs.Width / 2 - sizeF.Width / 2, imgs.Height / 4));
                    if (!string.IsNullOrEmpty(str1))
                    {
                        sizeF = g.MeasureString(str1, font);
                        g.DrawString(str1, font, System.Drawing.Brushes.Black, new PointF(imgs.Width / 2 - sizeF.Width / 2, imgs.Height / 2 + imgs.Height / 7));
                    }
                    imgs.Save("print.png");
                    images = ImageToBitmapImage(imgs);
                }
            }
            return images;
        }

        /// <summary>
        /// image转镜像
        /// </summary>
        /// <param name="PIC"></param>
        /// <returns></returns>
        public BitmapImage ImageToBitmapImage(System.Drawing.Image PIC)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(PIC))
                {
                    Bitmap.Save(ms, PIC.RawFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(data);
            bitmap.EndInit();
            return bitmap;
        }
        private void Isbn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lock (Isbn.Text)
                {
                    object issn = Isbn.Text;
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
                                Name.Content = periodicalChooseControl.info.name;
                                fkTypeCode.Content = periodicalChooseControl.info.fkTypeCode;
                                fkTypeName.Content = periodicalChooseControl.info.fkTypeName;
                                fkPressName.Content = periodicalChooseControl.info.fkPressName;
                                Author.Content = periodicalChooseControl.info.author;
                                unifyNum.Content = periodicalChooseControl.info.unifyNum;
                                parallelTitle.Content = periodicalChooseControl.info.parallelTitle;
                                postIssueNumber.Content = periodicalChooseControl.info.postIssueNumber;
                                openBook.Content = periodicalChooseControl.info.openBook;
                                issnPrice.Content = periodicalChooseControl.info.issnPrice;
                                releaseCycle.Content = periodicalChooseControl.info.releaseCycle;
                                remark.Content = periodicalChooseControl.info.remark;
                                periodicalInfo = new PeriodicalInfo() { fkCataPeriodicalId = periodicalChooseControl.info.id, };
                                ///弹出框
                                #region 索取号
                                GetCallNumberByIdDAL getCsDAL = new GetCallNumberByIdDAL();
                                object errorMsg = periodicalChooseControl.info.id;
                                if (getCsDAL.GetCallNumberById(ref errorMsg))
                                {
                                    RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                                    if (retrunInfo.TrueOrFalse)
                                    {
                                        this.info = retrunInfo.result as CallNumberInfo;
                                        callNumbermsg.Visibility = Visibility.Hidden;
                                        if (combox.SelectedIndex == 0)
                                            CallNumberTxt.Text = this.info.searchNumberOrderNum;
                                        else
                                            CallNumberTxt.Text = this.info.searchNumberAuthorNum;
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (ServerSetting.IsOverDue)
                            {
                                ErrorPage errorPage = new ErrorPage(info.result.ToString(),mainControl.mainWindow);
                                DialogHelper.ShowDialog(errorPage);
                            }
                            else
                            {
                                MessageBox.Show("失败提示：" + info.result);
                            }
                        }
                    }
                }
            }
        }

        private void Isbn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Isbn.Text))
                isbnmsg.Visibility = Visibility.Visible;
            else
                isbnmsg.Visibility = Visibility.Hidden;
        }

        private void Isbn_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void BookCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !BookCode.IsReadOnly)
            {
              
            }
        }

        private void BookCode_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Isbnmsg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Isbn.Focus();
        }

        private void Isbnmsg_MouseEnter(object sender, MouseEventArgs e)
        {
            Isbn.Focus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BookCode.Text = "请先扫描RFID";
            BookCode.IsReadOnly = true;
            ServerSetting.EPClist.Clear();
            ServerSetting.OldEPClist.Clear();
            mainControl.thread.Start();
            Success.Visibility = Visibility.Hidden;
            False.Visibility = Visibility.Hidden;
            Isbn.Focus();
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Isbn.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainControl.thread.Abort();
            PeriodicalHistoryControl periodicalHistoryControl = new PeriodicalHistoryControl(mainControl);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(periodicalHistoryControl);
        }

        private void Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (combox.SelectedIndex == 1)
                {
                    CallNumberTxt.Text = info.searchNumberAuthorNum;
                }
                else
                {
                    CallNumberTxt.Text = info.searchNumberOrderNum;
                }
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lock (BookCode.Text)
            {
                if (place.SelectedIndex < 0)
                {
                    error.Content = "未选择馆藏地";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if ( periodicalInfo == null || string.IsNullOrEmpty(periodicalInfo.fkCataPeriodicalId))
                {
                    error.Content = "未选中需要绑定的期刊";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if (string.IsNullOrEmpty(CallNumberTxt.Text))
                {
                    error.Content = "索取号不能为空";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if (string.IsNullOrEmpty(periodicalInfo.pNumberId))
                {
                    error.Content = "未选定子刊";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if (string.IsNullOrEmpty(BookCode.Text))
                {
                    error.Content = "书籍编码不能为空";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if (string.IsNullOrEmpty(EPC.Text))
                {
                    error.Content = "书籍编码不能为空";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if (periodicalInfo == null)
                    return;
                if (lendingPermission.IsChecked.Value == true)
                    periodicalInfo.lendingPermission = "1";
                else
                    periodicalInfo.lendingPermission = "0";
                if (available.IsChecked.Value == true)
                    periodicalInfo.available = "1";
                else
                    periodicalInfo.available = "0";
                periodicalInfo.callNumber = CallNumberTxt.Text;
                periodicalInfo.code = BookCode.Text;
                string epc = ServerSetting.EPClist.Dequeue();
                periodicalInfo.rfid = epc;

                if (ServerSetting.OldEPClist.Contains(epc))
                {
                    error.Content = "RFID重复";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    EPC.Clear();
                    return;
                }
                PlaceInfo placeInfo = place.SelectedItem as PlaceInfo;
                periodicalInfo.placeId = placeInfo.id;
                PeriodicalAddDAL periodicalAddDAL = new PeriodicalAddDAL();
                object errorMsg = periodicalInfo;
                if (periodicalAddDAL.PeriodicalAdd(ref errorMsg))
                {
                    RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                    if (!retrunInfo.TrueOrFalse)
                    {
                        Success.Visibility = Visibility.Hidden;
                        False.Visibility = Visibility.Visible;
                        if (retrunInfo.result.Equals("RFID重复"))
                        {
                            ServerSetting.OldEPClist.Enqueue(epc);
                        }
                        if (ServerSetting.IsOverDue)
                        {
                            ErrorPage errorPage = new ErrorPage(error.Content.ToString(), mainControl.mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                        }
                        error.Content = retrunInfo.result.ToString();
                        BookCode.IsReadOnly = true;
                    }
                    else
                    {
                        Success.Visibility = Visibility.Visible;
                        False.Visibility = Visibility.Hidden;
                        EPC.Clear();
                        ///成功列入已处理列
                        ServerSetting.OldEPClist.Enqueue(epc);
                        BookCode.IsReadOnly = true;
                        ///增加图片
                        List<MarCodeInfo> infos = MarCodeList.ItemsSource as List<MarCodeInfo>;
                        AddCell(infos, CallNumberTxt.Text);
                        int index = CallNumberTxt.Text.IndexOf("/");
                        string str1 = "";
                        string str = CallNumberTxt.Text.Substring(0, index + 1);
                        if (CallNumberTxt.Text.Length > index + 1)
                        {
                            str1 = CallNumberTxt.Text.Substring(index + 1, CallNumberTxt.Text.Length - index - 1);
                        }
                        CallNumberTxt.Text = str + (str1.ToInt() + 1).ToString();
                        error.Content = "";
                        if (!string.IsNullOrEmpty(infos[infos.Count - 1].MarCode3))
                        {
                            MessageBox.Show("单次操作最多可打印40张书标,后续绑定将不再生成");
                        }
                    }
                }
                else
                {
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    EPC.Clear();
                }
                BookCode.Clear();
            }
            EPC.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Isbn.Clear();
            isbnmsg.Visibility = Visibility.Visible;
            Name.Content = "";
            fkTypeCode.Content = "";
            fkTypeName.Content = "";
            fkPressName.Content = "";
            Author.Content = "";
            unifyNum.Content = "";
            parallelTitle.Content = "";
            postIssueNumber.Content = "";
            openBook.Content = "";
            issnPrice.Content = "";
            releaseCycle.Content = "";
            remark.Content = "";
            periodicalInfo = new PeriodicalInfo();
            EPC.Clear();
            BookCode.Clear();
            info = new CallNumberInfo();
            CallNumberTxt.Clear();
            callNumbermsg.Visibility = Visibility.Visible;
            PeriodicalCode.Clear();
            PeriodicalMsg.Visibility = Visibility.Visible;
            sNumber.Content = "";
            page.Content = "";
            publicationDateStr.Content = "";
            remarks.Content = "";
            price.Content = "";
        }
    }
}
