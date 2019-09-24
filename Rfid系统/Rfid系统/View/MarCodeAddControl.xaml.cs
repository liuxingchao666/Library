using BarcodeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using Image = System.Drawing.Image;
using Pen = System.Drawing.Pen;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using Rfid系统.DAL;
using System.Diagnostics;

namespace Rfid系统.View
{
    /// <summary>
    /// MarCodeAddControl.xaml 的交互逻辑
    /// </summary>
    public partial class MarCodeAddControl : Window
    {
        public MarCodeAddControl()
        {
            InitializeComponent();
            fontSize = 25;
        }
        public int fontSize;
        public List<BitmapImage> infos;

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pic.Source = null;
            infos = new List<BitmapImage>();
            this.Close();
            GC.SuppressFinalize(this);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(value.Text))
                return;
            if (FontSize.Text.ToInt() > 0)
            {
                fontSize = FontSize.Text.ToInt();
            }
            string printContent = prefix.Text + value.Text + suffix.Text;
           
            pic.Source = GetCell(printContent);
        }

        public BitmapImage GetCell(string content)
        {
           
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;
            b.IncludeLabel = true;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            System.Drawing.Font font = new System.Drawing.Font("utf-8", 15);
            b.LabelFont = font;
            #region 限制字号和长度
            double screenWidth = SystemParameters.PrimaryScreenWidth; // 屏幕整体宽度
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double rPower = screenWidth / screenHeight;

            double cPower = (content.Length * 11 + 35) * 0.01693;
            #endregion
            try
            {
                b.Encode(BarcodeLib.TYPE.CODE128, content);
                error.Visibility = Visibility.Hidden;
            }
            catch
            {
                error.Visibility = Visibility.Visible;
                return null;
            }
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
            b.Dispose();
            BitmapImage bitmapImage;
            using (Image image = Image.FromFile("1.png"))
            {
                ServerSetting.MarCodeHeight = image.Height;
                ServerSetting.MarCodeWidth = image.Width;
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    Font fonts = new Font("楷体", fontSize, System.Drawing.FontStyle.Bold);
                    SizeF sizeF = graphics.MeasureString(header.Text, fonts);
                    using (Pen pen = new Pen(Color.White, fontSize * 2))
                    {
                        graphics.DrawRectangle(pen, new Rectangle(0, 0, image.Width, sizeF.Height.ToInt() + 10));//加椭圆边框
                    }
                    graphics.DrawString(header.Text, fonts, System.Drawing.Brushes.Black, new PointF(image.Width / 2 - sizeF.Width / 2, 0));
                }
                bitmapImage = ImageToBitmapImage(image);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                image.Dispose();
            }
            return bitmapImage;
        
        }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int i = 1;
            if (i == 0)
            {
                Process process = Process.GetCurrentProcess();
              MessageBox.Show(process
                  .WorkingSet64 / 1024 / 1024 + "M (" + (process.WorkingSet64 / 1024).ToString() + "KB)");//占用内存
                PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            string Se = ramCounter.NextValue().ToString("0.00");
           
            PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set", process.ProcessName);
            string workingSe = String.Format("{0:F}", pf1.NextValue() / 1024.00 / 1024.00).ToString();
            MessageBox.Show("程序运行内存："+workingSe+",系统可用内存："+Se);
            return;
        }
            try
            {
                if (pic.Source != null)
                {
                    infos = new List<BitmapImage>();
                    int numbers = this.numbers.Text.ToInt();//打印数
                    if (numbers == 0)
                        numbers = 1;
                    int repeat = this.repeat.Text.ToInt();//重复数
                    if (repeat == 0)
                        repeat = 1;
                    int NIndex = 0;
                    int RIndex = 0;
                    if (numbers * repeat > 30000)
                    {
                        error.Content = "单次添加数不能超过30000";
                        error.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        pic.Source = null;
                        error.Content = "条码内容过长";
                        error.Visibility = Visibility.Hidden;
                    }
                    int InitialValue = value.Text.ToInt();
                    while (NIndex < numbers)
                    {
                        RIndex = 0;
                        while (RIndex < repeat)
                        {
                            string content = prefix.Text + (InitialValue + NIndex).ToString() + suffix.Text;
                            infos.Add(GetCell(content));
                            RIndex++;
                        }
                        NIndex++;
                    }
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("内存不足,条码生成超时！！生成个数："+infos.Count);
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            infos = new List<BitmapImage>();
        }
    }
}
