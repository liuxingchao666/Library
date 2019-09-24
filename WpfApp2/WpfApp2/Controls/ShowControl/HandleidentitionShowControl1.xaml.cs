using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// HandleidentitionShowControl1.xaml 的交互逻辑
    /// </summary>
    public partial class HandleidentitionShowControl1 : UserControl
    {
        public HandleidentitionShowControl1(HandleidentitionPage handleidentitionPage)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(handleidentitionPage);
            page = handleidentitionPage;
            data = handleidentitionPage.user;
            LoadData();
        }
       
        public HandleidentitionPage page;
        public System.Threading.Thread timer;
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
            {
                timer.Abort();
                HideInputPanel();
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            object idcard = data.CardNo;
            ///
            GetUserPhone getUserPhone = new GetUserPhone();
            if (getUserPhone.GetPhone(ref idcard))
            {
                page.user.phone = idcard.ToString();
                //this.Phone.IsReadOnly = true;
                Phone.Text = idcard.ToString();
            }
            else
            {
                Phone.Focus();
            }
            timer.IsBackground = true;
          //  timer.Start();
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
                data.phone = Phone.Text;
            }
        }

        private void Phone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Phone.Text == "请输入您的联系电话")
            {
                this.Phone.Clear();
            }
        }
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private const Int32 WM_SYSCOMMAND = 274;
        private const UInt32 SC_CLOSE = 61536;
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        private static Process softKey;
        /// <summary>
        /// 显示软键盘，前提是 Touch Keyboard and Handwriting Panel Service 服务要启动
        /// </summary>
        public static void ShowInputPanel()
        {

            string path = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";
            string path32 = @"C:\Program Files (x86)\Common Files\Microsoft Shared\Ink\TabTip32.exe";
            if (File.Exists(path))
            {
                softKey = Process.Start(path);

                //IntPtr intptr = IntPtr.Zero;
                //while (IntPtr.Zero == intptr)
                //{
                //    System.Threading.Thread.Sleep(100);
                //    intptr = FindWindow(null, "屏幕键盘");
                //}
                ////设定键盘显示位置
                //MoveWindow(intptr, 0, 0, 100, 100, true);
                ////设置软键盘到前端显示
                //SetForegroundWindow(intptr);

                // Process.Start(path);
            }
            else if (File.Exists(path32))
            {
                softKey = Process.Start(path32);

                //IntPtr intptr = IntPtr.Zero;
                //while (IntPtr.Zero == intptr)
                //{
                //    System.Threading.Thread.Sleep(100);
                //    intptr = FindWindow(null, "屏幕键盘");
                //}
                ////设定键盘显示位置
                //MoveWindow(intptr, 0, 0, 100, 100, true);
                ////设置软键盘到前端显示
                //SetForegroundWindow(intptr);

                //Process.Start(path32);
            }
        }


        /// <summary>
        ///  隐藏屏幕键盘
        /// </summary>
        public static void HideInputPanel()
        {
            IntPtr TouchhWnd = new IntPtr(0);
            TouchhWnd = FindWindow("IPTip_Main_Window", null);
            if (TouchhWnd == IntPtr.Zero)
                return;
            PostMessage(TouchhWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
        }
        private void Phone_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInputPanel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HideInputPanel();
        }
    }
}
