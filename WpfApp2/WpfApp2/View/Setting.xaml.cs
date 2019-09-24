using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Shapes;
using WpfApp2.BLL;
using WpfApp2.DAL;
using Action = System.Action;

namespace WpfApp2.View
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
            ServerSeting.ISAdd = false;
            ServerSeting.SYSleepTimes = 60;
            int i = 60;
            thread = new Thread(new ThreadStart(() =>
            {
                while (i > 1)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        this.Time.Content = i + "s";
                    });
                    i--;
                    Thread.Sleep(1000);
                }
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    thread.Abort();
                    this.Close();
                    HideInputPanel();
                });
            }));
            thread.IsBackground = true;
            thread.Start();
            serverIp.Text = ConfigurationManager.AppSettings["ServerIp"];
            serverSite.Text = ConfigurationManager.AppSettings["ServerPort"];
        }
        Thread thread;

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
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            HideInputPanel();
            thread.Abort();
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
            this.Close();
        }

        private void Try_Click(object sender, RoutedEventArgs e)
        {
            HideInputPanel();
            if (string.IsNullOrEmpty(serverIp.Text))
            {
                errorCode.Content = "服务器Ip地址不可为空";
                clear();
                return;
            }
            if (string.IsNullOrEmpty(serverSite.Text))
            {
                errorCode.Content = "服务器端口不可为空";
                clear();
                return;
            }
            Regex regex = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
            if (!regex.IsMatch(serverIp.Text))
            {
                errorCode.Content = "服务器ip格式设置不对";
                clear();
                return;
            }
            if (serverSite.Text.Length != 4)
            {
                errorCode.Content = "服务器端口格式设置不对";
                clear();
                return;
            }
            string url = string.Format("http://{0}:{1}/{2}", serverIp.Text, serverSite.Text, "equipmentmodule/verifyConnection/getConnection");
            Http http = new Http(url, null);
            object jsonResult = http.HttpGet(url);
            if (!jsonResult.ToString().ToLower().Contains("row"))
            {
                errorCode.Content = "测试连接失败";
                clear();
            }
            else
            {
                lock (ServerSeting.ServerIP)
                {
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    cfa.AppSettings.Settings["ServerIp"].Value = serverIp.Text;
                    cfa.AppSettings.Settings["ServerPort"].Value = this.serverSite.Text;
                    cfa.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    ServerSeting.ServerIP = serverIp.Text;
                    errorCode.Content = "设置成功";
                    clear();

                    ServerSeting.ServerSite = serverSite.Text;
                    ServerSeting.urlPath = string.Format("http:\\{0}:{1}", serverIp, serverSite.Text); ;
                }
                ServerSeting.ISAdd = true;
            }
        }

        void clear()
        {
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    errorCode.Content = "";
                    if (ServerSeting.ISAdd)
                    {
                        ServerSeting.SYSleepTimes = 60;
                        thread.Abort();
                        this.Close();
                    }
                });
            });
        }
        private void ServerIp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
            if (string.IsNullOrEmpty(e.Text))
                e.Handled = true;
        }

        private void ServerIp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void ServerIp_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInputPanel();
        }
    }
}
