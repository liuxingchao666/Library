using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
using WpfApp2.Controls;
using WpfApp2.DAL;
using WpfApp2.Model;

namespace WpfApp2.View
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
            MainControl mainControl = new MainControl(this);
            this.Grid.Children.Add(mainControl);
            thread = new Thread(new ThreadStart(conn));
            thread.IsBackground = true;
            threadSleep = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (ServerSeting.ISAdd)
                    {
                        ServerSeting.SYSleepTimes--;
                    }
                    if (ServerSeting.SYSleepTimes <= 0)
                    {
                        this.Dispatcher.BeginInvoke(new System.Action(delegate
                        {
                            Sleeping sleeping = new Sleeping(this);
                            Grid.Children.Clear();
                            Grid.Children.Add(sleeping);
                        }));
                        // mainControl.timer.Abort();
                        ServerSeting.SYSleepTimes = 60;
                        ServerSeting.ISAdd = false;

                        thread.Suspend();

                        threadSleep.Suspend();

                    }
                    Thread.Sleep(1000);
                }
            }));
            threadSleep.IsBackground = true;
            this.mainControl = mainControl;
        }
        public QueryControl QueryControl;
        public MainControl mainControl;
        public Thread thread;
        public Thread threadSleep;
        /// <summary>
        /// 人员信息
        /// </summary>
        public CardData user;
        /// <summary>
        /// IC卡卡号
        /// </summary>
        public string CardNum { get; set; }
        /// <summary>
        /// 进钞机
        /// </summary>
        public bool Connstate = true;

        /// <summary>
        /// 自启卸载
        /// </summary>
        /// <param name="exeName"></param>
        /// <returns></returns>
        public bool StartAutomaticallyDel(string exeName)
        {
            try
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".lnk");
                return true;
            }
            catch (Exception) { }
            return false;
        }
        /// <summary>
        /// 建立自启
        /// </summary>
        /// <returns></returns>
        public bool StartAutomaticallyCreate()
        {
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".lnk");
                shortcut.TargetPath = System.Windows.Forms.Application.ExecutablePath;
                shortcut.WorkingDirectory = System.Environment.CurrentDirectory;
                shortcut.WindowStyle = 1;
                shortcut.Description = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "_Ink";
                shortcut.Save();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Arcface arcface = new Arcface();
            //this.Grid.Children.Add(arcface);
            try
            {
                StartAutomaticallyCreate();
                ServerSeting.SYSleepTimes = 60;
                ServerSeting.ISAdd = true;
                //Task.Run(() =>
                //{
                //    string errormsg = "";
                //    ServerSeting.hairpin.conn(ref errormsg);
                //    object errorMsg = null;
                //    ServerSeting.bFeeder.IsConn(ref errorMsg);
                //    ServerSeting.ICConnState = ServerSeting.CcardBLL.conn();
                //    //ServerSeting.SMBll.conn();
                //    ServerSeting.connState = VerificationConn.GetVerification();
                //});
                thread.Start();
                threadSleep.Start();
            }
            catch { }
        }

        public void conn()
        {
            while (true)
            {
                try
                {
                    ServerSeting.connState = VerificationConn.GetVerification();
                    if (ServerSeting.ICConnState == false)
                    {
                        ServerSeting.CcardBLL.conn();
                    }
                    if (!ServerSeting.MConnState)
                    {
                        object errorMsg = null;
                        lock (this) { ServerSeting.bFeeder.IsConn(ref errorMsg); };
                    //    ServerSeting.newBFeeder.conn();
                    }
                    else
                    {
                        if (!ServerSeting.bFeeder.serialPort.IsOpen)
                            ServerSeting.MConnState = false;
                        //if (!ServerSeting.newBFeeder.serialPort.IsOpen)
                        //    ServerSeting.MConnState = false;
                    }

                    if (!ServerSeting.TConnState)
                    {
                        string errormsg = "";
                        ServerSeting.hairpin.conn(ref errormsg);

                    }
                    else
                    {
                        if (!ServerSeting.hairpin.serialPort.IsOpen)
                            ServerSeting.TConnState = false;
                    }

                    if (!ServerSeting.GetMessage.OpneOrClose())
                        ///错误信息展示
                        ServerSeting.IDConnState = false;
                    else
                        ServerSeting.IDConnState = true;
                    if (ServerSeting.hairpin.serialPort.IsOpen)
                    {
                        byte[] sends = new byte[6];
                        sends[0] = 0x02;
                        sends[1] = 0x52;
                        sends[2] = 0x46;
                        sends[3] = 0x03;
                        sends[4] = 0x15;
                        sends[5] = 0x05;
                        ServerSeting.hairpin.serialPort.Write(sends, 0, sends.Length);
                    }
                    else
                    {
                        ServerSeting.warehouseIsNull = false;
                    }
                    Thread.Sleep(800);
                }

                catch(Exception ex)

                {
                    Thread.Sleep(800);
                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ServerSeting.TConnState)
                ServerSeting.hairpin.serialPort.Close();
            if (ServerSeting.MConnState)
                ServerSeting.bFeeder.serialPort.Close();
            System.Environment.Exit(0);
        }
    }
}
