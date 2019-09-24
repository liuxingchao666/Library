using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.ViewModel;
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
using System.Drawing;
using Image = System.Drawing.Image;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ColorConverter = System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;
using System.Text.RegularExpressions;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Rfid系统.View
{
    /// <summary>
    /// RFIDBindingControl.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDBindingControl : UserControl
    {
        public RFIDBindingControl(MainControl mainControl)
        {
            InitializeComponent();
            this.mainControl = mainControl;
            DataContext = null;
            DataContext = new RFIDBindingViewModel(mainControl,this);

            ServerSetting.OldEPClist.Clear();
            ServerSetting.EPClist.Clear();
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
                                    epc = ServerSetting.EPClist.Dequeue();
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
        }
        public string epc;
        public MainControl mainControl;
        public string id;
        public Thread thread;
        public CallNumberInfo info;
        /// <summary>
        /// 弹出列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(Isbn.Text))
                return;
            if (e.Key == Key.Enter)
            {
                object errorMsg = Isbn.Text;
                ISBNListControl iSBNListControl;
                GetBookListByISBNDAL iSBNDAL = new GetBookListByISBNDAL();
                iSBNDAL.GetBookListByISBN(ref errorMsg);

                RetrunInfo info = errorMsg as RetrunInfo;
                if (info.TrueOrFalse)
                {
                    iSBNListControl = new ISBNListControl(Isbn.Text, info);
                    DialogHelper.ShowDialog(iSBNListControl);
                    if (iSBNListControl.info != null)
                    {
                        mainControl.info = iSBNListControl.info;
                        BookName.Content = iSBNListControl.info.BookName;
                        Price.Content = iSBNListControl.info.Price;
                        Author.Content = iSBNListControl.info.Author;
                        Press.Content = iSBNListControl.info.Press;
                        PressDate.Content = iSBNListControl.info.PressDate;
                        CallNumber.Content = iSBNListControl.info.CallNumber;
                        PageNumber.Content = iSBNListControl.info.PageNumber;
                        Classification.Content = iSBNListControl.info.Classification;
                        callNumbermsg.Visibility = Visibility.Hidden;
                    }
                    if (iSBNListControl.info == null)
                        return;
                    errorMsg = iSBNListControl.info.id;
                    GetCallNumberByIdDAL selectCataOrderByIDDAL = new GetCallNumberByIdDAL();
                    if (selectCataOrderByIDDAL.GetCallNumberById(ref errorMsg))
                    {
                        RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                        this.info = retrunInfo.result as CallNumberInfo;
                        try
                        {
                            if (combox.SelectedIndex == 0)
                            {
                                CallNumber.Content = this.info.searchNumberOrderNum;
                                CallNumberTxt.Text = this.info.searchNumberOrderNum;
                            }
                            else
                            {
                                CallNumber.Content = this.info.searchNumberAuthorNum;
                                CallNumberTxt.Text = this.info.searchNumberAuthorNum;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        if (ServerSetting.IsOverDue)
                        {
                            ErrorPage errorPage = new ErrorPage(errorMsg.ToString(), mainControl.mainWindow);
                            DialogHelper.ShowDialog(errorPage);
                        }
                    }
                }
                else
                {
                    if (ServerSetting.IsOverDue)
                    {
                        ErrorPage errorPage = new ErrorPage(info.result.ToString(), mainControl.mainWindow);
                        DialogHelper.ShowDialog(errorPage);
                        return;
                    }
                    else
                    {
                        error.Content = info.result + "";
                    }
                }

            }
        }

        private void Isbn_GotFocus(object sender, RoutedEventArgs e)
        {
            Isbn.Focus();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BookCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !BookCode.IsReadOnly)
            {
               
            }
        }
        /// <summary>
        /// 加载是俺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            using (System.Drawing.Image imgs = Image.FromFile(@"001.png"))
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
            using (System.Drawing.Image imgs = Image.FromFile("02.png"))
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //List<MarCodeInfo> infos = MarCodeList.ItemsSource as List<MarCodeInfo>;
            //MarCodeList.ItemsSource = null;
            //ColorDialog colorDialog = new ColorDialog();
            //colorDialog.ShowDialog();
            //ColorA = colorDialog.Color.A;
            //ColorR = colorDialog.Color.R;
            //ColorG = colorDialog.Color.G;
            //ColorB = colorDialog.Color.B;
            //try
            //{
            //    Color.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorDialog.Color.Name.ToString()));
            //}
            //catch
            //{
            //    Color.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + colorDialog.Color.Name.ToString()));
            //}
            //finally
            //{
            //    string imagePath = "02.png";
            //    Bitmap image = new Bitmap(imagePath);

            //    for (int i = 0; i < image.Width; i++)
            //    {
            //        for (int j = 0; j < image.Height; j++)
            //        {
            //            System.Drawing.Color color = image.GetPixel(i, j);
            //            if ( color.G == 36 && color.B == 44)
            //                image.SetPixel(i, j, colorDialog.Color);
            //        }
            //    }
            //    foreach (MarCodeInfo info in infos)
            //    {
            //        info.PICBG = ImageToBitmapImage(image);
            //    }
            //    image.Dispose();
            //}

            //MarCodeList.ItemsSource = infos;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.V)
                e.Handled = true;
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
          //  thread.Suspend();
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
               // thread.Resume();
            }
            catch { }
        }

        private void BookCode_KeyUp(object sender, KeyEventArgs e)
        {

        }
        private void Label_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Isbn.Focus();
        }

        private void Isbn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Isbn.Text))
                isbnmsg.Visibility = Visibility.Visible;
            else
                isbnmsg.Visibility = Visibility.Hidden;
        }

        private void Isbnmsg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Isbn.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainControl.thread.Abort();
            BingHistoryControl bingHistoryControl = new BingHistoryControl(mainControl);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(bingHistoryControl);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (combox.SelectedIndex == 0)
                {
                    CallNumber.Content = info.searchNumberOrderNum;
                    CallNumberTxt.Text = info.searchNumberOrderNum;
                }
                else
                {
                    CallNumber.Content = info.searchNumberAuthorNum;
                    CallNumberTxt.Text = info.searchNumberAuthorNum;
                }
            }
            catch { }
        }

        private void Border_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            CallNumberTxt.Focus();
        }
        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Isbn.Focus();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lock (BookCode.Text)
            {
                if (mainControl.info == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(mainControl.info.id))
                {
                    error.Content = "未选中需要绑定的书籍";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                if (string.IsNullOrEmpty(EPC.Text))
                {
                    error.Content = "未扫描到可用RFID";
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
                if (string.IsNullOrEmpty(BookCode.Text))
                {
                    error.Content = "书籍编码不能为空";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    return;
                }
                mainControl.info.CallNumber = CallNumberTxt.Text;
                mainControl.info.EPC = ServerSetting.EPClist.Dequeue();
                if (ServerSetting.OldEPClist.Contains(mainControl.info.EPC))
                {
                    error.Content = "RFID重复";
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    EPC.Clear();
                    return;
                }
                mainControl.info.BookCdoe = BookCode.Text;
                AddRfidDAL addRfidDAL = new AddRfidDAL();
                object errorMsg = mainControl.info;
                if (addRfidDAL.AddRfid(ref errorMsg))
                {
                    Success.Visibility = Visibility.Visible;
                    False.Visibility = Visibility.Hidden;
                    EPC.Clear();
                    ///成功列入已处理列
                    ServerSetting.OldEPClist.Enqueue(mainControl.info.EPC);
                    BookCode.IsReadOnly = true;
                    ///增加图片
                    List<MarCodeInfo> infos = MarCodeList.ItemsSource as List<MarCodeInfo>;
                    AddCell(infos, mainControl.info.CallNumber);
                    int index = CallNumberTxt.Text.IndexOf("/");
                    string str1 = "";
                    string str = CallNumberTxt.Text.Substring(0, index + 1);
                    if (CallNumberTxt.Text.Length > index + 1)
                    {
                        str1 = CallNumberTxt.Text.Substring(index + 1, CallNumberTxt.Text.Length - index - 1);
                    }
                    CallNumberTxt.Text = str + (str1.ToInt() + 1).ToString();
                    mainControl.info.CallNumber = CallNumberTxt.Text;
                    error.Content = "";
                    if (!string.IsNullOrEmpty(infos[infos.Count - 1].MarCode3))
                    {
                        MessageBox.Show("单次操作最多可打印40张书标,后续绑定将不再生成");
                    }
                }
                else
                {
                    try
                    {
                        RetrunInfo retrunInfo = errorMsg as RetrunInfo;
                        if (retrunInfo.result.Equals("RFID重复"))
                        {
                            ServerSetting.OldEPClist.Enqueue(mainControl.info.EPC);
                        }
                        error.Content = retrunInfo.result.ToString();
                    }
                    catch
                    { }
                    Success.Visibility = Visibility.Hidden;
                    False.Visibility = Visibility.Visible;
                    EPC.Clear();
                    if (ServerSetting.IsOverDue)
                    {
                        ErrorPage errorPage = new ErrorPage(error.Content.ToString(), mainControl.mainWindow);
                        DialogHelper.ShowDialog(errorPage);
                    }
                }
                BookCode.Clear();
            }
            BookCode.IsReadOnly = true;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            EPC.Clear();
            BookCode.Clear();
            CallNumberTxt.Clear();
            Isbn.Clear();
            isbnmsg.Visibility = Visibility.Visible;
            BookName.Content="";
            Price.Content = "";
            Author.Content = "";
            Press.Content = "";
            PressDate.Content = "";
            CallNumber.Content = "";
            PageNumber.Content = "";
            Classification.Content = "";
            info = new CallNumberInfo();
        }
    }
}
