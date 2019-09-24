using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;
#pragma warning disable CS0105 // “System.Drawing”的 using 指令以前在此命名空间中出现过
using System.Drawing;
#pragma warning restore CS0105 // “System.Drawing”的 using 指令以前在此命名空间中出现过
using WpfApp2.BLL;
using WpfApp2.DAL;
using Emgu.CV;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// HandleidentitionShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class HandleidentitionShowControl : UserControl
    {
        public HandleidentitionShowControl(HandleidentitionPage handleidentitionPage)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(handleidentitionPage);
            page = handleidentitionPage;
            data = handleidentitionPage.user;
            LoadData();
        }
        public HandleidentitionPage page;
        public  System.Threading.Thread timer;
        delegate void UpdateTimer();
        public int Times;
        public CardData data;
        public void EditTimes()
        {
            while (true)
            {
                this.Dispatcher.BeginInvoke(new UpdateTimer(Edit));
                Thread.Sleep(1000);
            }
        }

        public void LoadData() 
        {
            this.userName.Text = data.Name.Trim();
            this.IdCard.Text = data.CardNo.Trim();
        
            ///
            this.PIC.ImageSource = StringToBitmapImage(data.PIC);
           
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

        public void Edit()
        {
            if (Times <= 30)
            {
                Times++;
              //  this.times.Content = (30 - Times).ToString();
            }
            else
                timer.Abort();
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            object idcard = data.CardNo;
            ///
            GetUserPhone getUserPhone = new GetUserPhone();
            if (getUserPhone.GetPhone(ref idcard))
            {
                page.user.phone = idcard.ToString();
                this.Phone.IsReadOnly = true;
                Phone.Text = idcard.ToString();
            }
            else
            {
                Phone.Focus();
            }
            timer.IsBackground = true;
            timer.Start();
        }
        private void Phone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Phone.Text == "请输入您的联系电话")
            {
                this.Phone.Clear();
            }
        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Phone.Text))
            {
                if (Phone.Text.Trim().Length <= 11)
                {
                    TS.Foreground = System.Windows.Media.Brushes.LightBlue;
                }
                else
                {
                    TS.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private void HandleidentitionPage_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Q_MouseEnter(object sender, MouseEventArgs e)
        {
          
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void Label_MouseWheel(object sender, MouseWheelEventArgs e)
        {
           
        }

        private void Q_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
