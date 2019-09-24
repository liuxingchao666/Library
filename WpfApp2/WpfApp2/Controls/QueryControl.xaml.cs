using Newtonsoft.Json.Linq;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using WpfApp2.BLL;
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls
{
    /// <summary>
    /// QueryControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryControl : UserControl
    {
        public MainPage MainPage;
        public QueryControl(MainPage mainpage)
        {
            InitializeComponent();
            DataContext = new QueryViewModel(mainpage);
            MainPage = mainpage;
            timer = new Thread(new ThreadStart(EditTimes));
            timer.IsBackground = true;
            times = 60;
        }
        public int times = 60;
        public Thread timer;
        delegate void UpdateTimer();

        public List<string> Test;
#pragma warning disable CS0414 // 字段“QueryControl.a”已被赋值，但从未使用过它的值
        bool a = true;
#pragma warning restore CS0414 // 字段“QueryControl.a”已被赋值，但从未使用过它的值
        public MqttClient mqttClient;
        public void EditTimes()
        {
#pragma warning disable CS0219 // 变量“result”已被赋值，但从未使用过它的值
            bool result = true;
#pragma warning restore CS0219 // 变量“result”已被赋值，但从未使用过它的值
            while (true)
            {
                Thread.Sleep(1000);
                times--;
                if (times < 1)
                {
                    times = 0;
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {

                        MainControl mainControl = new MainControl(MainPage);
                        MainPage.Grid.Children.Clear();
                        MainPage.Grid.Children.Add(mainControl);

                        ServerSeting.ISAdd = true;
                        ServerSeting.SYSleepTimes = 60;

                    });
                    timer.Abort();
                }
                this.Dispatcher.BeginInvoke(new UpdateTimer(Edit));
            }

        }

        public void Edit()
        {
            this.Dispatcher.BeginInvoke((System.Action)delegate ()
            {
                this.time.Content = "操作时间: " + times + "s";
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        string Url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "borrowmodule/InAndOutEquipment/getHot");
                        Http http = new Http(Url, null);
                        string Json = http.HttpGet(Url);
                        try
                        {
                            var Data = JObject.Parse(Json);
                            if (Data["state"].ToString() == "True")
                            {
                                for (int i = 0; i < Data["row"].Count(); i++)
                                {
                                    string xaml = string.Format("<Button xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"  " +
                                        " Grid.Column=\"3\" Command=\"{0}\"  FontSize=\"18\" Content=\"{1}\" Tag=\"{1}\" Style=\"{2}\" CommandParameter=\"{3}\" Name=\"btn{4}\"></Button>",
                                        "{Binding HotSearchCommand}", Data["row"][i]["name"] != null ? Data["row"][i]["name"].ToString() : "", "{StaticResource 热搜榜}", "{ Binding ElementName = btn" + i + "}", i);
                                    StringReader strreader = new StringReader(xaml);
                                    XmlTextReader xmlreader = new XmlTextReader(strreader);
                                    object obj = XamlReader.Load(xmlreader);
                                    wrqp.Children.Add((UIElement)obj);
                                }
                            }
                        }
                        catch { }
                    });
                });
            }
            catch { }
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HideInputPanel();
        }
        //private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    this.txt.Visibility = Visibility.Hidden;
        //}
        //private void TextBox1_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(textBox1.Text))
        //    {
        //        this.txt.Visibility = Visibility.Visible;
        //    }
        //}

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {

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
        private void TextBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            ShowInputPanel();
            textBox1.SelectAll();
        }

        private void TextBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            HideInputPanel();
        }

        private void Txt_MouseEnter(object sender, MouseEventArgs e)
        {
            textBox1.Focus();
        }

        private void TextBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Txt_GotFocus_1(object sender, RoutedEventArgs e)
        {
            ShowInputPanel();
        }

        private void TextBox1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInputPanel();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HideInputPanel();
        }

        private void TextBox1_LostFocus_1(object sender, RoutedEventArgs e)
        {

        }

        private void Txt_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInputPanel();
        }

        private void Txt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowInputPanel();
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                txt.Visibility = Visibility.Visible;
            }
            else
            {
                txt.Visibility = Visibility.Hidden;
            }
            textBox1.Focus();
        }
    }
}
