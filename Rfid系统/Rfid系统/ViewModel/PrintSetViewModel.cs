using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Rfid系统.ViewModel
{
    public class PrintSetViewModel : NotificationObject
    {
        public PrintSetViewModel(List<MarCodeInfo> infos, PrintSetControl PrintSetControl)
        {
            RowNumbers = "4";
            CloumnNumbers = "10";
            FontSize = 15;
            CellHeight = "150";
            CellWidth = "180";
            RowSpacing = 10;
            CloumnSpacing = 10;
            BookMarkHeight = 794;
            BookMarkWidth = 1123;
            LoadState = Visibility.Visible;
            if (infos != null && infos.Count > 0)
            {
                PIC = MakePrintImage(infos);
                int i = 9;
                while (i >= 0)
                {
                    if (infos[i].VisibleState == System.Windows.Visibility.Hidden)
                        infos.RemoveAt(i);
                    i--;
                }
            }
            LoadState = Visibility.Hidden;
            if (infos != null && infos.Count > 0)
                this.infos = infos;
            this.PrintSetControl = PrintSetControl;
            //处理空数据
        }
        public PrintSetControl PrintSetControl;
        public int loadResidualQuantity=30;
        public int loadRowNumber=0;
        public int loadCellHeight=0;
        public List<MarCodeInfo> infos = new List<MarCodeInfo>();
        public System.Drawing.Color BorderB;
        public System.Drawing.Color SBS;
        public System.Drawing.Color XBS;
        public BitmapImage bookImg;
        /// <summary>
        /// 自定义画布
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public BitmapImage MakePrintImage(List<MarCodeInfo> infos)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile("002.png"))///创建画布
            {
                using (Bitmap mainBitmap = new Bitmap(img, BookMarkWidth, BookMarkHeight))///固定画布大小（前台可选择）
                {
                    int i = 0;
                    int j = RowNumbers.ToInt();
                    Bitmap bitmap = mainBitmap;

                    foreach (MarCodeInfo info in infos)
                    {
                        if (info.VisibleState == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImage(bitmap, i, RowNumbers.ToInt() - j, info.MarCode);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = RowNumbers.ToInt();
                            }
                        }
                        else
                            break;
                        if (info.VisibleState1 == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImage(bitmap, i, RowNumbers.ToInt() - j, info.MarCode1);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = RowNumbers.ToInt();
                            }
                        }
                        else
                            break;
                        if (info.VisibleState2 == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImage(bitmap, i, RowNumbers.ToInt() - j, info.MarCode2);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = RowNumbers.ToInt();
                            }
                        }
                        else
                            break;
                        if (info.VisibleState3 == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImage(bitmap, i, RowNumbers.ToInt() - j, info.MarCode3);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = RowNumbers.ToInt();
                            }
                        }
                        else
                            break;
                    }
                    bitmap.Save("05.png");
                    bitmap.Dispose();
                    using (Image image = Image.FromFile("05.png"))
                    {
                        return ImageToBitmapImage(image);
                    }
                }
            }
        }
        public BitmapImage MakePrintImageBarMark(List<MarCodeInfo> infos)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile("002.png"))///创建画布
            {
                using (Bitmap mainBitmap = new Bitmap(img, BookMarkWidth, BookMarkHeight))///固定画布大小（前台可选择）
                {
                    int i = 0;
                    int j = 3;
                    Bitmap bitmap = mainBitmap;

                    foreach (MarCodeInfo info in infos)
                    {
                        if (info.VisibleState == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImageBarcode(bitmap, i, 3 - j, info.MarCode);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = 3;
                            }
                        }
                        else
                            break;
                        if (info.VisibleState1 == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImageBarcode(bitmap, i, 3 - j, info.MarCode1);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = 3;
                            }
                        }
                        else
                            break;
                        if (info.VisibleState2 == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImageBarcode(bitmap, i, 3 - j, info.MarCode2);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = 3;
                            }
                        }
                        else
                            break;
                        if (info.VisibleState3 == System.Windows.Visibility.Visible)
                        {
                            bitmap = AddCellImageBarcode(bitmap, i, 3 - j, info.MarCode3);
                            j--;
                            if (j <= 0)
                            {
                                i++;
                                j = 3;
                            }
                        }
                        else
                            break;
                    }
                    bitmap.Save("05.png");
                    bitmap.Dispose();


                    using (Image image = Image.FromFile("05.png"))
                    {
                        return ImageToBitmapImage(image);
                    }
                }
            }
        }
        /// <summary>
        /// 加单元框加字
        /// 上标色 下标色 边框色
        /// </summary>
        /// <param name="mainBitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Bitmap AddCellImage(Bitmap mainBitmap, int x, int y, string callNumber)
        {
            using (Graphics graphics = Graphics.FromImage(mainBitmap))
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromFile("02.png"))
                {
                    using (Bitmap bitmap = new Bitmap(img, CellWidth.ToInt(), CellHeight.ToInt()))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            int index = callNumber.IndexOf("/");
                            string str = callNumber.Substring(0, index);
                            string str1 = "";
                            if (callNumber.Length > index + 1)
                            {
                                str1 = callNumber.Substring(index + 1, callNumber.Length - index - 1);
                            }
                            Font font = new Font("楷体", FontSize, System.Drawing.FontStyle.Bold);
                            SizeF sizeF = g.MeasureString(str, font);
                            System.Drawing.Brush bush = System.Drawing.Brushes.Black;
                            if (SBS != null)
                                bush = new System.Drawing.SolidBrush(SBS);
                            g.DrawString(str, font, (SBS.Name == "0" ? System.Drawing.Brushes.Black : bush), new PointF(bitmap.Width / 2 - sizeF.Width / 2, bitmap.Height / 4));
                            if (!string.IsNullOrEmpty(str1))
                            {
                                if (XBS != null)
                                    bush = new System.Drawing.SolidBrush(XBS);

                                sizeF = g.MeasureString(str1, font);
                                g.DrawString(str1, font, (XBS.Name == "0" ? System.Drawing.Brushes.Black : bush), new PointF(bitmap.Width / 2 - sizeF.Width / 2, bitmap.Height / 2 + bitmap.Height / 10));
                            }
                        }
                        for (int i = 0; i < bitmap.Width; i++)
                        {
                            for (int j = 0; j < bitmap.Height; j++)
                            {
                                System.Drawing.Color color = bitmap.GetPixel(i, j);
                                if (color.G == 36 && color.B == 44 && BorderB.Name != "0")
                                    bitmap.SetPixel(i, j, BorderB);
                            }
                        }
                        float xx = y * (CellWidth.ToInt()) + CloumnSpacing * y;
                        float yy = x * (CellHeight.ToInt()) + RowSpacing * x;
                        graphics.DrawImage(bitmap, new PointF(xx, yy));
                    }
                }
            }
            mainBitmap.Save("222.png");
            return mainBitmap;
        }
        /// <summary>
        /// 添加书标打印单元个
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public Bitmap AddCellImageBarcode(Bitmap mainBitmap, int x, int y, string callNumber)
        {
            using (Graphics graphics = Graphics.FromImage(mainBitmap))
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromFile("11.png"))
                {
                    using (Bitmap bitmap = new Bitmap(img, BookMarkWidth.ToInt() / 3, BookMarkHeight.ToInt() / 10))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            int index = callNumber.IndexOf("/");
                            string str = callNumber.Substring(0, index);
                            string str1 = "";
                            if (callNumber.Length > index + 1)
                            {
                                str1 = callNumber.Substring(index + 1, callNumber.Length - index - 1);
                            }
                            Font font = new Font("楷体", FontSize, System.Drawing.FontStyle.Bold);
                            SizeF sizeF = g.MeasureString(str, font);
                            System.Drawing.Brush bush = System.Drawing.Brushes.Black;
                            if (SBS != null)
                                bush = new System.Drawing.SolidBrush(SBS);
                            g.DrawString(str, font, (SBS.Name == "0" ? System.Drawing.Brushes.Black : bush), new PointF(bitmap.Width / 2 - sizeF.Width / 2, bitmap.Height / 4));
                            if (!string.IsNullOrEmpty(str1))
                            {
                                if (XBS != null)
                                    bush = new System.Drawing.SolidBrush(XBS);

                                sizeF = g.MeasureString(str1, font);
                                g.DrawString(str1, font, (XBS.Name == "0" ? System.Drawing.Brushes.Black : bush), new PointF(bitmap.Width / 2 - sizeF.Width / 2, bitmap.Height / 2 + bitmap.Height / 7));
                            }
                        }
                        float xx = y * BookMarkWidth.ToInt() / 3;
                        float yy = x * BookMarkHeight.ToInt() / 10;
                        graphics.DrawImage(bitmap, new PointF(xx, yy));
                    }
                }
            }
            mainBitmap.Save("222.png");
            return mainBitmap;
        }
        public Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
                outStream.Dispose();
            }
            return new Bitmap(bitmap);
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
        /// <summary>
        /// 主图（打印）
        /// </summary>
        private BitmapImage pic { get; set; }
        public BitmapImage PIC
        {
            get { return pic; }
            set
            {
                pic = value;
                this.RaisePropertyChanged(() => PIC);
            }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        private float fontSize { get; set; }
        public float FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                this.RaisePropertyChanged(() => FontSize);
            }
        }
        /// <summary>
        /// 颜色控件（刷新）
        /// </summary>
        private ICommand colorCommand { get; set; }
        public ICommand ColorCommand
        {
            get
            {
                return colorCommand ?? (colorCommand = new DelegateCommand<Border>((data) =>
                {
                    ColorDialog colorDialog = new ColorDialog();
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        switch (data.Name)
                        {
                            case "Border":
                                BorderB = colorDialog.Color;
                                break;
                            case "XBColor":
                                XBS = colorDialog.Color;
                                break;
                            case "SBColor":
                                SBS = colorDialog.Color;
                                break;
                        }
                        try
                        {
                            data.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorDialog.Color.Name.ToString()));
                        }
                        catch
                        {
                            data.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + colorDialog.Color.Name.ToString()));
                        }
                    }
                }));
            }
        }
        /// <summary>
        /// 打印图刷新
        /// </summary>
        private ICommand setCommand { get; set; }
        public ICommand SetCommand
        {
            get
            {
                return setCommand ?? (setCommand = new DelegateCommand(() =>
                {
                    if (CellHeight.ToInt() <= 0 || CellWidth.ToInt() <= 0)
                    {
                        PIC = null;
                        return;
                    }
                    if (BookMarkHeight.ToInt() <= 0 || BookMarkWidth.ToInt() <= 0)
                    {
                        PIC = null;
                        return;
                    }
                    loadRowNumber = RowNumbers.ToInt();
                    loadCellHeight = CellHeight.ToInt();
                    LoadState = Visibility.Visible;
                    if (infos != null && infos.Count > 0)
                        PIC = MakePrintImage(infos);
                    LoadState = Visibility.Hidden;
                }));
            }
        }
        /// <summary>
        /// 连接打印机
        /// </summary>
        private ICommand printCommand { get; set; }
        public ICommand PrintCommand
        {
            get
            {
                return printCommand ?? (printCommand = new DelegateCommand(() =>
                {
                    if (infos == null || infos.Count == 0)
                        return;
                    if (PIC == null)
                        return;
                    printDocument = new PrintDocument();
                    if (string.IsNullOrEmpty(ServerSetting.BookmarkPrinterName))
                    {
                        System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
                        if (printDialog.ShowDialog() == true)
                        {
                            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printDialog.PrintQueue.FullName;
                            ServerSetting.BookmarkPrinterName = printDialog.PrintQueue.FullName;
                        }
                    }
                    else
                    {
                        printDocument.DefaultPageSettings.PrinterSettings.PrinterName = ServerSetting.BookmarkPrinterName;
                    }
                    //if (!CheckPrinter(printDocument.PrinterSettings.PrintFileName))
                    //{
                    //    MessageBox.Show("当前打印机不可用");
                    //    ServerSetting.BookmarkPrinterName = null;
                    //    return;
                    //}
                    printDocument.DefaultPageSettings.PaperSize = new PaperSize("A4",BookMarkHeight,BookMarkWidth);
                    printDocument.DefaultPageSettings.Landscape = true;

                    if (printDocument.PrinterSettings.IsValid)
                    {
                        Printt(PIC);
                    }
                }));
            }
        }

        
        public  bool CheckPrinter(string printerName1)
            {

                ManagementScope scope = new ManagementScope(@"\root\cimv2");
                scope.Connect();

                // Select Printers from WMI Object Collections
                ManagementObjectSearcher searcher = new
                 ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                string printerName = "";
                foreach (ManagementObject printer in searcher.Get())
                {
                    printerName = printer["Name"].ToString().ToLower();
                    if (printerName.IndexOf(printerName1.ToLower()) > -1)
                    {

                        if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                        {
                            return false;
                            // printer is offline by user

                        }
                        else
                        {
                            // printer is not offline

                            return true;
                        }
                    }
                }
                return false;
            }
        
        private ICommand bookMarkPrint { get; set; }
        public ICommand BookMarkPrint
        {
            get
            {
                return bookMarkPrint ?? (bookMarkPrint = new DelegateCommand(() =>
                {

                    if (infos == null || infos.Count == 0)
                        return;
                    printDocument = new PrintDocument();
                    if (string.IsNullOrEmpty(ServerSetting.BookmarkPrinterName))
                    {
                        System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
                        if (printDialog.ShowDialog() == true)
                        {
                            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printDialog.PrintQueue.FullName;
                            ServerSetting.BookmarkPrinterName = printDialog.PrintQueue.FullName;
                        }
                    }
                    else
                    {
                        printDocument.DefaultPageSettings.PrinterSettings.PrinterName = ServerSetting.BookmarkPrinterName;
                    }
                    ///设置书标打印页(书标纸整体高宽)
                    printDocument.DefaultPageSettings.PaperSize = new PaperSize("A4", BookMarkHeight, BookMarkWidth);
                    printDocument.DefaultPageSettings.Margins = new Margins(100,10,10,10);
                    if (printDocument.PrinterSettings.IsValid)
                    {
                        bookImg = MakePrintImageBarMark(infos);
                        PIC = bookImg;
                    }
                }));
            }
        }
        private ICommand addCommand { get; set; }
        public ICommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new DelegateCommand(() =>
                {
                    if (CellHeight.ToInt() <= 0 || CellWidth.ToInt() <= 0)
                    {
                        PIC = null;
                        return;
                    }
                    if (BookMarkHeight.ToInt() <= 0 || BookMarkWidth.ToInt() <= 0)
                    {
                        PIC = null;
                        return;
                    }
                    if (!string.IsNullOrEmpty(Prefix) || !string.IsNullOrEmpty(Suffix))
                    {
                        string callNumber = Prefix + "/" + Suffix;
                        int repeats = Repeat.ToInt();
                        if (repeats == 0)
                            repeats = 1;
                        if (infos == null || infos.Count <= 0)
                        {
                            ADD(repeats, callNumber);
                        }
                        else
                        {
                            if (infos[infos.Count - 1].VisibleState1 == System.Windows.Visibility.Hidden)
                            {
                                infos[infos.Count - 1].VisibleState1 = System.Windows.Visibility.Visible;
                                infos[infos.Count - 1].MarCode1 = callNumber;
                                repeats--;
                            }
                            if (repeats > 0 && infos[infos.Count - 1].VisibleState2 == System.Windows.Visibility.Hidden)
                            {
                                infos[infos.Count - 1].VisibleState2 = System.Windows.Visibility.Visible;
                                infos[infos.Count - 1].MarCode2 = callNumber;
                                repeats--;
                            }
                            if (repeats > 0 && infos[infos.Count - 1].VisibleState3 == System.Windows.Visibility.Hidden)
                            {
                                infos[infos.Count - 1].VisibleState3 = System.Windows.Visibility.Visible;
                                infos[infos.Count - 1].MarCode3 = callNumber;
                                repeats--;
                            }
                            if (repeats >= 0)
                            {
                                ADD(repeats, callNumber);
                            }
                        }
                        ///添加处理infos
                        if (infos != null && infos.Count > 0)
                        {
                            PIC = MakePrintImage(infos);
                            PrintState = Visibility.Visible;
                            ServerSetting.LoadState = false;
                        }
                        LoadState = Visibility.Hidden;

                    }
                }));
            }
        }
        public void ADD(int repeats, string callNumber)
        {
            MarCodeInfo marCodeInfo = new MarCodeInfo();
            while (repeats > 0)
            {
                if (string.IsNullOrEmpty(marCodeInfo.MarCode))
                {
                    marCodeInfo.VisibleState = System.Windows.Visibility.Visible;
                    marCodeInfo.MarCode = callNumber;
                    if (repeats == 1)
                        infos.Add(marCodeInfo);
                    repeats--;

                }
                else if (string.IsNullOrEmpty(marCodeInfo.MarCode1))
                {
                    marCodeInfo.VisibleState1 = System.Windows.Visibility.Visible;
                    marCodeInfo.MarCode1 = callNumber;
                    if (repeats == 1)
                        infos.Add(marCodeInfo);
                    repeats--;
                }
                else if (string.IsNullOrEmpty(marCodeInfo.MarCode2))
                {
                    marCodeInfo.VisibleState2 = System.Windows.Visibility.Visible;
                    marCodeInfo.MarCode2 = callNumber;
                    if (repeats == 1)
                        infos.Add(marCodeInfo);
                    repeats--;
                }
                else if (string.IsNullOrEmpty(marCodeInfo.MarCode3))
                {
                    marCodeInfo.VisibleState3 = System.Windows.Visibility.Visible;
                    marCodeInfo.MarCode3 = callNumber;
                    infos.Add(marCodeInfo);
                    marCodeInfo = new MarCodeInfo();
                    repeats--;
                }
            }
        }
        private Visibility loadState { get; set; }
        public Visibility LoadState
        {
            get { return loadState; }
            set
            {
                loadState = value;
                this.RaisePropertyChanged(() => LoadState);
            }
        }

        private Visibility printState { get; set; }
        public Visibility PrintState
        {
            get { return printState; }
            set
            {
                printState = value;
                this.RaisePropertyChanged(() => PrintState);
            }
        }

        public int bookMarkHeight { get; set; }
        public int BookMarkHeight
        {
            get { return bookMarkHeight; }
            set
            {
                bookMarkHeight = value;
                this.RaisePropertyChanged(() => BookMarkHeight);
            }
        }

        public int bookMarkWidth { get; set; }
        public int BookMarkWidth
        {
            get { return bookMarkWidth; }
            set
            {
                bookMarkWidth = value;
                this.RaisePropertyChanged(() => BookMarkWidth);
            }
        }

        private ICommand input { get; set; }
        public ICommand Input
        {
            get
            {
                return input ?? (input = new DelegateCommand(() =>
                {

                }));
            }
        }
        /// <summary>
        /// 前缀
        /// </summary>
        private string prefix { get; set; }
        public string Prefix
        {
            get { return prefix; }
            set
            {
                prefix = value;
                this.RaisePropertyChanged(() => Prefix);
            }
        }
        /// <summary>
        /// 后缀
        /// </summary>
        private string suffix { get; set; }
        public string Suffix
        {
            get { return suffix; }
            set
            {
                suffix = value;
                this.RaisePropertyChanged(() => Suffix);
            }
        }
        /// <summary>
        /// 单高
        /// </summary>
        private string cellHeight { get; set; }
        public string CellHeight
        {
            get { return cellHeight; }
            set
            {
                cellHeight = value;
                this.RaisePropertyChanged(() => CellHeight);
            }
        }
        /// <summary>
        /// 单宽
        /// </summary>
        private string cellWidth { get; set; }
        public string CellWidth
        {
            get { return cellWidth; }
            set
            {
                cellWidth = value;
                this.RaisePropertyChanged(() => CellWidth);
            }
        }
        /// <summary>
        /// 行距离
        /// </summary>
        private float rowSpacing { get; set; }
        public float RowSpacing
        {
            get { return rowSpacing; }
            set
            {
                rowSpacing = value;
                this.RaisePropertyChanged(() => RowSpacing);
            }
        }
        /// <summary>
        /// 列距离
        /// </summary>
        private float cloumnSpacing { get; set; }
        public float CloumnSpacing
        {
            get { return cloumnSpacing; }
            set
            {
                cloumnSpacing = value;
                this.RaisePropertyChanged(() => CloumnSpacing);
            }
        }
        /// <summary>
        /// 行显示数
        /// </summary>
        private string rowNumbers { get; set; }
        public string RowNumbers
        {
            get { return rowNumbers; }
            set
            {
                rowNumbers = value;
                this.RaisePropertyChanged(() => RowNumbers);
            }
        }
        /// <summary>
        /// 列显示数
        /// </summary>
        private string cloumnNumbers { get; set; }
        public string CloumnNumbers
        {
            get { return cloumnNumbers; }
            set
            {
                cloumnNumbers = value;
                this.RaisePropertyChanged(() => CloumnNumbers);
            }
        }
        /// <summary>
        /// 重复数
        /// </summary>
        private string repeat { get; set; }
        public string Repeat
        {
            get { return repeat; }
            set
            {
                repeat = value;
                this.RaisePropertyChanged(() => Repeat);
            }
        }
        /// <summary>
        /// 索取号
        /// </summary>
        private string marCode { get; set; }
        public string MarCode
        {
            get { return marCode; }
            set
            {
                marCode = value;
                this.RaisePropertyChanged(() => MarCode);
            }
        }
        /// <summary>
        /// ISBN
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

        private ICommand deleteCommand { get; set; }
        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new DelegateCommand(() =>
                {
                    if (DeleteNum.ToInt() == 0)
                        return;
                    if (infos == null || infos.Count <= 0)
                        return;
                    int rowCount = infos.Count;
                    int count = (rowCount - 1) * 4 + 1;
                    if (infos[rowCount - 1].VisibleState3 == Visibility.Visible)
                        count++;
                    if (infos[rowCount - 1].VisibleState2 == Visibility.Visible)
                        count++;
                    if (infos[rowCount - 1].VisibleState2 == Visibility.Visible)
                        count++;
                    if (count <= DeleteNum)
                    {
                        infos.Clear();
                        PIC = null;
                        return;
                    }
                    int index = DeleteNum;
                    List<int> indexL = new List<int>();
                    for (int i = rowCount - 1; i >= 0; i--)
                    {
                        if (infos[i].VisibleState3 == Visibility.Visible)
                        {
                            infos[i].VisibleState3 = Visibility.Hidden;
                            index--;
                            if (index == 0)
                                break;
                        }
                        if (infos[i].VisibleState2 == Visibility.Visible)
                        {
                            infos[i].VisibleState2 = Visibility.Hidden;
                            index--;
                            if (index == 0)
                                break;
                        }
                        if (infos[i].VisibleState1 == Visibility.Visible)
                        {
                            infos[i].VisibleState1 = Visibility.Hidden;
                            index--;
                            if (index == 0)
                                break;
                        }
                        if (infos[i].VisibleState == Visibility.Visible)
                        {
                            infos[i].VisibleState = Visibility.Hidden;
                            index--;
                            indexL.Add(i);
                            if (index == 0)
                                break;
                        }
                    }
                    foreach (int temp in indexL)
                    {
                        infos.RemoveAt(temp);
                    }
                    LoadState = Visibility.Visible;
                    if (infos != null && infos.Count > 0)
                        PIC = MakePrintImage(infos);

                }));
            }
        }
        private int deleteNum { get; set; }
        public int DeleteNum
        {
            get { return deleteNum; }
            set
            {
                deleteNum = value;
                this.RaisePropertyChanged(() => DeleteNum);
            }
        }
        /// <summary>
        /// 打印机
        /// </summary>
        PrintDocument printDocument;
        public void Printt(BitmapImage bitmapImage)
        {
            printDocument.PrintPage += (s, args) =>
            {
                using (System.Drawing.Image i = BitmapImageToBitmap(bitmapImage))
                {
                    using (Bitmap bitmap = new Bitmap(i))
                    {
                        #region 居中打印设置
                        //System.Drawing.Rectangle m = args.PageBounds;
                        //if (bitmap.Width < bitmap.Height)
                        //    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        //if (bitmap.Width >= bitmap.Height)
                        //{
                        //    if ((double)bitmap.Width / (double)bitmap.Height <= (double)m.Width / (double)m.Height)
                        //    {
                        //        int w = (int)((double)bitmap.Width / (double)bitmap.Height * (double)m.Height);
                        //        int dx = (m.Width - w) / 2;
                        //        m.X = dx;
                        //        m.Y = 0;
                        //        m.Width = w;
                        //    }
                        //    else
                        //    {
                        //        int h = (int)((double)bitmap.Height / (double)bitmap.Width * (double)m.Width);
                        //        int dy = (m.Height - h) / 2;
                        //        m.X = 0;
                        //        m.Y = dy;
                        //        m.Height = h;
                        //    }
                        //}
                        #endregion
                        args.Graphics.DrawImage(bitmap,new PointF(0,0));
                    }
                };
            };
            try
            {
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument;
                DialogResult result = printPreviewDialog.ShowDialog();
            }
            catch
            {
                MessageBox.Show("当前打印机不可用");
            }
        }
        public void BookMarkAdd(ref Bitmap bitmap, int x, int y, string callNumber)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int index = callNumber.IndexOf("/");
                string str = callNumber.Substring(0, index + 1);
                string str1 = "";
                if (callNumber.Length > index + 1)
                {
                    str1 = callNumber.Substring(index + 1, callNumber.Length - index - 1);
                }
                Font font = new Font("宋体", FontSize, System.Drawing.FontStyle.Bold);
                SizeF sizeF = g.MeasureString(str, font);
                System.Drawing.Brush bush = System.Drawing.Brushes.Black;
                if (SBS != null)
                    bush = new System.Drawing.SolidBrush(SBS);
                g.DrawString(str, font, (SBS.Name == "0" ? System.Drawing.Brushes.Black : bush), new PointF(bitmap.Width / 8 + (x * bitmap.Width / 4) - sizeF.Width / 2, y * bitmap.Height / 10 + bitmap.Height / 40));
                if (!string.IsNullOrEmpty(str1))
                {
                    if (XBS != null)
                        bush = new System.Drawing.SolidBrush(XBS);

                    sizeF = g.MeasureString(str1, font);
                    g.DrawString(str1, font, (XBS.Name == "0" ? System.Drawing.Brushes.Black : bush), new PointF(bitmap.Width / 8 + (x * bitmap.Width / 4) - sizeF.Width / 2, y * bitmap.Height / 10 + bitmap.Height / 15 + bitmap.Height / 70));
                }
            }
        }

        public void BookMarkPrinT()
        {
            int x = 0;
            int y = 0;
            BitmapImage bitmapImage;
            using (Image image = Image.FromFile(@"002.png"))
            {
                Bitmap bitmap = new Bitmap(image);
                foreach (MarCodeInfo info in infos)
                {
                    if (info.VisibleState == System.Windows.Visibility.Visible)
                    {
                        BookMarkAdd(ref bitmap, x, y, info.MarCode);
                        x++;
                    }
                    else
                    {
                        break;
                    }
                    if (info.VisibleState1 == System.Windows.Visibility.Visible)
                    {
                        BookMarkAdd(ref bitmap, x, y, info.MarCode1);
                        x++;
                    }
                    else
                    {
                        break;
                    }
                    if (info.VisibleState2 == System.Windows.Visibility.Visible)
                    {
                        BookMarkAdd(ref bitmap, x, y, info.MarCode2);
                        x++;
                    }
                    else
                    {
                        break;
                    }
                    if (info.VisibleState3 == System.Windows.Visibility.Visible)
                    {
                        BookMarkAdd(ref bitmap, x, y, info.MarCode3);
                        y++;
                        x = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                bitmapImage = ImageToBitmapImage(bitmap);
                bitmap.Dispose();
            }
            if (infos == null && infos.Count <= 0)
            {
                return;
            }
            ///
            printDocument.PrintPage += (s, args) =>
            {
                using (System.Drawing.Image i = BitmapImageToBitmap(bitmapImage))
                {
                    using (Bitmap bitmap = new Bitmap(i))
                    {
                        System.Drawing.Rectangle m = args.PageBounds;
                        if (bitmap.Width < bitmap.Height)
                            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        if (bitmap.Width >= bitmap.Height)
                        {
                            if ((double)bitmap.Width / (double)bitmap.Height <= (double)m.Width / (double)m.Height)
                            {
                                int w = (int)((double)bitmap.Width / (double)bitmap.Height * (double)m.Height);
                                int dx = (m.Width - w) / 2;
                                m.X = dx;
                                m.Y = 0;
                                m.Width = w;
                            }
                            else
                            {
                                int h = (int)((double)bitmap.Height / (double)bitmap.Width * (double)m.Width);
                                int dy = (m.Height - h) / 2;
                                m.X = 0;
                                m.Y = dy;
                                m.Height = h;
                            }
                        }
                        args.Graphics.DrawImage(bitmap, m);
                    }
                };
            };
            printDocument.Print();
        }
    }
}
