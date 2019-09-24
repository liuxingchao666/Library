using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using WpfApp2.BLL;
using WpfApp2.DAL;
using WpfApp2.Model;
using Action = WpfApp2.BLL.Action;
using AsfFace;
using System.IO;

namespace WpfApp2.View
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : Window
    {
       public TestView()
        {
            InitializeComponent();
        
        }
      
#pragma warning disable CS0414 // 字段“TestView.hEngine”已被赋值，但从未使用过它的值
        IntPtr hEngine = new IntPtr();
#pragma warning restore CS0414 // 字段“TestView.hEngine”已被赋值，但从未使用过它的值
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetArcfoaceUserList getArcfoaceUserList = new GetArcfoaceUserList();
            object errorMsg = null;
            if (getArcfoaceUserList.getBookClass(ref errorMsg))
            {
                List<UserMessage> user = errorMsg as List<UserMessage>;
                this.pic.Source = StringToBitmapImage(user[0].PIC);
            }
        }

        public BitmapImage StringToBitmapImage(string PIC)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            byte[] ImgByte = Convert.FromBase64String(PIC);
            bitmap.StreamSource = new MemoryStream(ImgByte);
            bitmap.EndInit();
            return bitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
        }
        private void Window_Closed(object sender, EventArgs e)
        {
           
        }

        private void Tet_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
         
            //Thread.Sleep(3000);
        }
    }
}
