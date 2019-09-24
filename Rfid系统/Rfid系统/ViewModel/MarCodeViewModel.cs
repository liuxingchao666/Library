using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode.Internal;
using BarcodeLib;
using System.Configuration;
using System.Windows.Controls;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using Rfid系统.DAL;
using Rfid系统.View;
using System.Windows.Forms;
using DataGrid = System.Windows.Controls.DataGrid;
using PrintDialog = System.Windows.Controls.PrintDialog;
using System.Management;
using Baidu.Aip.Speech;

namespace Rfid系统.ViewModel
{
    public class MarCodeViewModel : NotificationObject
    {
        public MarCodeViewModel()
        {
            FontSize = 15;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FontSize"].ToString()))
            {
                FontSize = ConfigurationManager.AppSettings["FontSize"].ToInt();
            }
            Initial = ConfigurationManager.AppSettings["InitialValue"].ToInt();
            Header = ConfigurationManager.AppSettings["HeaderTxt"].ToString();
            Suffix = ConfigurationManager.AppSettings["Suffix"].ToString();
            Prefix = ConfigurationManager.AppSettings["Prefix"].ToString();
            RowNumber = 1;
            Repeat = 1;
            PrintNum = 1;
        }
        /// <summary>
        /// BitmapImage 转图---增加文字
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
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
        /// 条码头
        /// </summary>
        private string header { get; set; }
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                this.RaisePropertyChanged(() => Header);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        private int initial { get; set; }
        public int Initial
        {
            get { return initial; }
            set
            {
                initial = value;
                this.RaisePropertyChanged(() => Initial);
            }
        }
        /// <summary>
        /// 行数
        /// </summary>
        private int rowNumber { get; set; }
        public int RowNumber
        {
            get { return rowNumber; }
            set
            {
                rowNumber = value;
                this.RaisePropertyChanged(() => RowNumber);
            }
        }
        /// <summary>
        /// 重复数
        /// </summary>
        private int repeat { get; set; }
        public int Repeat
        {
            get { return repeat; }
            set
            {
                repeat = value;
                this.RaisePropertyChanged(() => Repeat);
            }
        }
        /// <summary>
        /// 打印数（未重复）
        /// </summary>
        private int printNum { get; set; }
        public int PrintNum
        {
            get { return printNum; }
            set
            {
                printNum = value;
                this.RaisePropertyChanged(() => PrintNum);
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
        /// 字号
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
        /// 保存设置响应试图
        /// </summary>
        private ICommand saveSet { get; set; }
        public ICommand SaveSet
        {
            get
            {
                return saveSet ?? (saveSet = new DelegateCommand<DataGrid>((data) =>
                {
                   
                    FlushData(data);
                }));
            }
        }
        private ICommand addCommand { get; set; }
        public ICommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new DelegateCommand<DataGrid>((data)=> {
                    MarCodeAddControl marCodeAddControl = new MarCodeAddControl();
                    DialogHelper.ShowDialog(marCodeAddControl);
                    if (marCodeAddControl.infos != null && marCodeAddControl.infos.Count > 0)
                    {
                        int rowNum = marCodeAddControl.infos.Count / 8;
                        if (marCodeAddControl.infos.Count % 8 != 0)
                            rowNum++;
                        int i = 0;
                        if (MarList != null && MarList.Count > 0 && string.IsNullOrEmpty(MarList[MarList.Count - 1].MarCodeContent7))
                            rowNum--;
                        while (i < rowNum)
                        {
                            MarList.Add(new MarInfo() { FontSize = FontSize });
                            i++;
                        }
                        i = 0;
                        while (i < marCodeAddControl.infos.Count)
                        {
                            foreach (MarInfo info in MarList)
                            {
                                if (string.IsNullOrEmpty(info.MarCodeContent))
                                {
                                    info.MarCodePIC = marCodeAddControl.infos[i];
                                   
                                    info.MarCodeContent = "12";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                 if (string.IsNullOrEmpty(info.MarCodeContent1))
                                {
                                    info.MarCodePIC1 = marCodeAddControl.infos[i];
                                    info.MarCodeContent1 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                 if (string.IsNullOrEmpty(info.MarCodeContent2))
                                {
                                    info.MarCodePIC2 = marCodeAddControl.infos[i];
                                    info.MarCodeContent2 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                if (string.IsNullOrEmpty(info.MarCodeContent3))
                                {
                                    info.MarCodePIC3 = marCodeAddControl.infos[i];
                                    info.MarCodeContent3 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                if (string.IsNullOrEmpty(info.MarCodeContent4))
                                {
                                    info.MarCodePIC4 = marCodeAddControl.infos[i];
                                    info.MarCodeContent4 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                if (string.IsNullOrEmpty(info.MarCodeContent5))
                                {
                                    info.MarCodePIC5 = marCodeAddControl.infos[i];
                                    info.MarCodeContent5 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                if (string.IsNullOrEmpty(info.MarCodeContent6))
                                {
                                    info.MarCodePIC6 = marCodeAddControl.infos[i];
                                    info.MarCodeContent6 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                                if (string.IsNullOrEmpty(info.MarCodeContent7))
                                {
                                    info.MarCodePIC7 = marCodeAddControl.infos[i];
                                    info.MarCodeContent7 = "23";
                                    i++;
                                    if (i == marCodeAddControl.infos.Count)
                                        break;
                                }
                            }
                        }
                        data.ItemsSource = null;
                        data.ItemsSource = MarList;
                    }
                    marCodeAddControl = null;
                }));
            }
        }
        PrintDocument printDocument;
        public bool CheckPrinter(string printerName1)
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
        /// <summary>
        /// 打印写入配置文件
        /// </summary>
        private ICommand print { get; set; }
        public ICommand Print
        {
            get
            {
                return print ?? (print = new DelegateCommand<DataGrid>((data) =>
                {
                    if (MarList==null || MarList.Count<=0 || string.IsNullOrEmpty(MarList[0].MarCodeContent))
                        return;
                    if (string.IsNullOrEmpty(ServerSetting.BarcodePrinterName))
                    {
                        PrintDialog printDialog = new PrintDialog();
                        if (printDialog.ShowDialog() == true)
                        {
                            printDocument = new PrintDocument();
                            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printDialog.PrintQueue.FullName;
                            ServerSetting.BarcodePrinterName = printDialog.PrintQueue.FullName;
                            printDocument.DefaultPageSettings.PaperSize = new PaperSize("aaa", ServerSetting.MarCodeWidth /6 *5, ServerSetting.MarCodeHeight / 10 *7);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        printDocument = new PrintDocument();
                        printDocument.DefaultPageSettings.PrinterSettings.PrinterName = ServerSetting.BarcodePrinterName;

                        printDocument.DefaultPageSettings.PaperSize = new PaperSize("aaa", ServerSetting.MarCodeWidth /6 *5, ServerSetting.MarCodeHeight / 10 * 7);
                    }

                    if (!CheckPrinter(printDocument.PrinterSettings.PrintFileName))
                    {
                        ServerSetting.BarcodePrinterName = null;
                        MessageBox.Show("打印机不可用");
                        return;
                    }
                    foreach (MarInfo info in MarList)
                    {
                        if (!string.IsNullOrEmpty(info.MarCodeContent))
                        {
                            Printt(info.MarCodePIC);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent1))
                        {
                            Printt(info.MarCodePIC1);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent2))
                        {
                            Printt(info.MarCodePIC2);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent3))
                        {
                            Printt(info.MarCodePIC3);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent4))
                        {
                            Printt(info.MarCodePIC4);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent5))
                        {
                            Printt(info.MarCodePIC5);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent6))
                        {
                            Printt(info.MarCodePIC6);
                        }
                        if (!string.IsNullOrEmpty(info.MarCodeContent7))
                        {
                            Printt(info.MarCodePIC7);
                        }
                    }
                    ///防止只设样式没设定用户乱点
                    if (MarList != null && MarList.Count > 0)
                    {
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                        cfa.AppSettings.Settings["InitialValue"].Value = (Initial + PrintNum * Repeat + 1).ToString();
                        cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                        ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                        MarList = new List<MarInfo>();
                        data.ItemsSource = null;
                    }
                }));
            }
        }

        private ICommand clearCommand { get; set; }
        public ICommand ClearCommand
        {
            get
            {
                return clearCommand ?? (clearCommand=new DelegateCommand<DataGrid>((data)=> {
                    MarList.Clear();
                    MarList = new List<MarInfo>();
                    System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet = new System.IntPtr(5);
                    data.ItemsSource = null;
                }));
            }
        }
        public void Printt(BitmapImage bitmapImage)
        {
            printDocument.PrintPage += (s, args) =>
            {
                using (System.Drawing.Image i = BitmapImageToBitmap(bitmapImage))
                {
                    System.Drawing.Rectangle m = args.PageBounds;
                    if (i.Width < i.Height)
                        i.RotateFlip(RotateFlipType.Rotate90FlipNone);

                    if (i.Width >= i.Height)
                    {
                        if ((double)i.Width / (double)i.Height <= (double)m.Width / (double)m.Height)
                        {
                            int w = (int)((double)i.Width / (double)i.Height * (double)m.Height);
                            int dx = (m.Width - w) / 2;
                            m.X = dx;
                            m.Y = 0;
                            m.Width = w;
                        }
                        else
                        {
                            int h = (int)((double)i.Height / (double)i.Width * (double)m.Width);
                            int dy = (m.Height - h) / 2;
                            m.X = 0;
                            m.Y = dy;
                            m.Height = h;
                        }
                    }
                    args.Graphics.DrawImage(i, m);
                };
               
            };
          

            printDocument.Print();
        }
        /// <summary>
        /// 列表数据
        /// </summary>
        private List<MarInfo> marList { get; set; } = new List<MarInfo>();
        public List<MarInfo> MarList
        {
            get { return marList; }
            set
            {
                marList = value;
                this.RaisePropertyChanged(() => MarList);
            }
        }
        /// <summary>
        /// 获取显示图片
        /// </summary>
        /// <param name="printContent">打印内容</param>
        /// <returns>条码头像</returns>
        public BitmapImage GetBitmapImage(string printContent)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;
            b.IncludeLabel = true;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            System.Drawing.Font font = new System.Drawing.Font("verdana", FontSize);
            b.LabelFont = font;

            b.Encode(BarcodeLib.TYPE.CODE128, printContent);
            byte[] buffer = b.GetImageData(SaveTypes.PNG);

            Image bitmap;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                bitmap = (Image)Image.FromStream(ms);
                ms.Dispose();
            }
            if (File.Exists("1.png"))
                File.Delete("1.png");
            bitmap.Save("1.png");
            bitmap.Dispose();
            BitmapImage bitmapImage;
            using (Image image = Image.FromFile("1.png"))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    Font fonts = new Font("宋体", FontSize, FontStyle.Bold);
                    SizeF sizeF = graphics.MeasureString(Header, fonts);
                    using (Pen pen = new Pen(Color.White, FontSize * 2))
                    {
                        graphics.DrawRectangle(pen, new Rectangle(0, 0, image.Width, sizeF.Height.ToInt() + 10));//加椭圆边框
                    }
                    graphics.DrawString(Header, fonts, System.Drawing.Brushes.Black, new PointF(image.Width / 2 - sizeF.Width / 2, 0));
                }
                bitmapImage = ImageToBitmapImage(image);
                return bitmapImage;
            }
        }
        public void FlushData(DataGrid dataGrid)
        {
            int count = Repeat * PrintNum / 3;
            if (Repeat * PrintNum % 3 != 0)
                count++;
            int rowNum = 0;
            if (count > RowNumber)
                rowNum = count;
            else
                rowNum = RowNumber;
            int i = 0;
            ///强制适应显示row数
            RowNumber = rowNum;
            MarList.Clear();
            while (i < rowNum)
            {
                MarList.Add(new MarInfo() { FontSize = FontSize });
                i++;
            }
            i = 0;
            ///
            while (i < PrintNum)
            {
                int j = 0;
                ///打印内容
                string printContent = Prefix + (i + Initial).ToString() + Suffix;
                while (j < Repeat)
                {
                    foreach (MarInfo info in MarList)
                    {
                        if (string.IsNullOrEmpty(info.MarCodeContent))
                        {
                            info.MarCodePIC = GetBitmapImage(printContent);
                            info.MarCodeContent = "12";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent1))
                        {
                            info.MarCodePIC1 = GetBitmapImage(printContent);
                            info.MarCodeContent1 = "23";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent2))
                        {
                            info.MarCodePIC2 = GetBitmapImage(printContent);
                            info.MarCodeContent2 = "23";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent3))
                        {
                            info.MarCodePIC3 = GetBitmapImage(printContent);
                            info.MarCodeContent3 = "23";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent4))
                        {
                            info.MarCodePIC4 = GetBitmapImage(printContent);
                            info.MarCodeContent4 = "23";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent5))
                        {
                            info.MarCodePIC5 = GetBitmapImage(printContent);
                            info.MarCodeContent5 = "23";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent6))
                        {
                            info.MarCodePIC6 = GetBitmapImage(printContent);
                            info.MarCodeContent6 = "23";
                            break;
                        }
                        else if (string.IsNullOrEmpty(info.MarCodeContent7))
                        {
                            info.MarCodePIC7 = GetBitmapImage(printContent);
                            info.MarCodeContent7 = "23";
                            break;
                        }
                    }
                    j++;
                }
                i++;
            }
            //
            //
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = MarList;
        }
    }
}
