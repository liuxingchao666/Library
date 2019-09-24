using Rfid系统.DAL;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Rfid系统
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(MainWindow_Resize);

            thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    lock (ServerSetting.rfid)
                    {
                        if (!ServerSetting.RfidConnState)
                            ServerSetting.rfid.GetConnState();
                        else if (!ServerSetting.rfid.IsOpen())
                        {
                            ServerSetting.RfidConnState = false;
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                        {
                            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                        }
                    }
                    Thread.Sleep(800);
                }
            }));
            thread.IsBackground = true;
        }
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        private void MainWindow_Resize(object sender, System.EventArgs e)
        {

        }
        /// <summary>
        /// 线程
        /// </summary>
        protected Thread thread;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginControl loginControl = new LoginControl(this);
            this.gridControl.Children.Add(loginControl);

        thread.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ServerSetting.RfidConnState)
                try
                {
                    ServerSetting.rfid.Stop();
                    ServerSetting.rfid.CloseD();
                    System.Environment.Exit(0);
                }
                catch { }
        }
    }
   
}

