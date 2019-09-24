using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;
using Rfid系统.Model;
using System.IO;
using Rfid系统.ViewModel;

namespace Rfid系统.View
{
    /// <summary>
    /// PrintSetControl.xaml 的交互逻辑
    /// </summary>
    public partial class PrintSetControl : UserControl
    {
        public PrintSetControl(List<MarCodeInfo> infos)
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new PrintSetViewModel(infos,this);
            
            this.infos = infos;
        }
        /// <summary>
        /// 操作数据源
        /// </summary>
        public List<MarCodeInfo> infos;
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
     
        /// <summary>
        /// 打印---清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

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

       
     
    }
   
}
