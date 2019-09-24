using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
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
using System.Windows.Shapes;
using System.Xml;
using WpfApp2.BLL;
using WpfApp2.Controls;
using WpfApp2.Controls.ShowControl;
using WpfApp2.DAL;
using WpfApp2.Model;

namespace WpfApp2.View
{
    /// <summary>
    /// PayMentPage.xaml 的交互逻辑
    /// </summary>
    public partial class PayMentPage : UserControl
    {
        public DealBusiness business;
        public PayStep payStep;
        public ReturnInfo info;
        public MainPage mainPage;
        int needMoney;
        public PayMentPage(DealBusiness dealBusiness, PayStep step, MainPage mainPage)
        {
            InitializeComponent();
            ///打开显示灯并进入收钱状态
            //if (ServerSeting.newBFeeder.serialPort.IsOpen)
            //{
            //   byte[] send = new byte[6];
            //    send[0] = 0x7F;
            //    send[1] = 0x00;
            //    send[2] = 0x01;
            //    send[3] = 0x03;
            //    send[4] = 0x11;
            //    send[5] = 0x88;
            //    ServerSeting.newBFeeder.serialPort.Write(send, 0, send.Length);
            //    send = new byte[6];
            //    send[0] = 0x7F;
            //    send[1] = 0x08;
            //    send[2] = 0x01;
            //    send[3] = 0x0A;
            //    send[4] = 0x11;
            //    send[5] = 0x88;
            //    ServerSeting.newBFeeder.serialPort.Write(send, 0, send.Length);
            //}
            needMoney = ServerSeting.HostNeedMoney;
            business = dealBusiness;
            payStep = step;
            timer = new Thread(new ThreadStart(AddTimes));
            timer.IsBackground = true;
            isPrint = true;
            this.mainPage = mainPage;
            Times = 0;
            orderNumber = null;
            Task.Run(() =>
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["ProcessorId"].Value;
                    break;
                }
                string url = string.Format("http://{0}:{1}/equipmentmodule/readerTbCardInfo/currency/getEquipmentTbOrderNumber?equipmentCode={2}&&equipmentName={3}", ServerSeting.ServerIP,ServerSeting.ServerSite, HDid, Dns.GetHostName());
                
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));
                orderNumber = result["row"].ToString();
                ServerSeting.OrderNumber = orderNumber;
            });
        }
        public string orderNumber;
        public Thread timer;
        public bool isPrint = true;
        delegate void UpdateTimer();
        public int Times = 0;
        /// <summary>
        /// 人员信息
        /// </summary>
        public CardData user;
        public bool result = false;
        public void AddTimes()
        {
            while (true)
            {
                Times++;
                isClose();
                Thread.Sleep(1000);
            }
        }
        public void getstring(string num, ref string newNum)
        {
            if (num != "" && num.Length == 2)
            {
                newNum += num;
            }
            else
            {
                newNum += num.Substring(num.Length - 2, 2);
                num = num.Substring(0, num.Length - 2);
                getstring(num, ref newNum);
            }
        }
        public bool Judage = true;
        public PrintParamtClass printParamt;
        private TransactionSucessPage transactionSucessPage;
        public int index = 0;
        public void isClose()
        {
            if (Times > 59)
            {
                ServerSeting.HostNeedMoney = 0;

                this.Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    if (transactionSucessPage != null)
                    {
                        transactionSucessPage.timer.Abort();
                        transactionSucessPage.Close();
                    }
                    mainPage.Grid.Children.Clear();
                    MainControl mainControl = new MainControl(mainPage);
                    mainPage.Grid.Children.Add(mainControl);
                }));
                if (ServerSeting.transactionRecordInfos != null && ServerSeting.transactionRecordInfos.Count > 0 && isPrint)
                {
                    PrintErrorMsg(printParamt, PrintClass.errorLog, "收钞时间到期,投入金额小于所需金额");
                }
                timer.Abort();
            }
            else
            {
                this.Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    Time.Content = "操作时间: " + (60 - Times) + "s";
                }));
                if (Judage)
                {
                    if (Times > 2 && string.IsNullOrEmpty(user.cardNum))
                    {
                        this.Dispatcher.BeginInvoke(new System.Action(delegate
                        {
                            ServerSeting.HostNeedMoney = 0;
                            MainControl mainControle = new MainControl(mainPage);
                            mainPage.Grid.Children.Clear();
                            mainPage.Grid.Children.Add(mainControle);
                            ServerSeting.ISAdd = false;
                            errorPage errorPage = new errorPage("获取读者卡卡号失败");
                            errorPage.mainPage = mainPage;
                            errorPage.ShowDialog();
                        }));
                        timer.Abort();
                        return;
                    }
                    else
                    {
                        try
                        {
                            try
                            {
                                ServerSeting.CcardBLL.rfidReader.DisConnect();
                            }
                            catch { }
                            if (ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                            {
                                ICcardMessage ccardMessage = new ICcardMessage();
                                if (ServerSeting.CcardBLL.ReadCard(ref ccardMessage))
                                {
                                    string cardnum = "";
                                    getstring(ccardMessage.CardNum, ref cardnum);
                                    ServerSeting.TempCardNum = cardnum;
                                    user.cardNum = cardnum;
                                    ServerSeting.CcardBLL.rfidReader.DisConnect();
                                    if (!string.IsNullOrEmpty(ServerSeting.LastCardNum))
                                    {
                                        if (ServerSeting.LastCardNum == cardnum)
                                        {
                                            if (ServerSeting.hairpin.serialPort.IsOpen)
                                            {
                                                byte[] send = new byte[6];
                                                send[0] = 0x02;
                                                send[1] = 0x43;
                                                send[2] = 0x50;
                                                send[3] = 0x03;
                                                send[4] = 0x12;
                                                send[5] = 0x05;
                                                ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                            }
                                            this.Dispatcher.BeginInvoke(new System.Action(delegate
                                            {
                                                ServerSeting.HostNeedMoney = 0;
                                                MainControl mainControle = new MainControl(mainPage);
                                                mainPage.Grid.Children.Clear();
                                                mainPage.Grid.Children.Add(mainControle);
                                                ServerSeting.ISAdd = false;
                                                errorPage errorPage = new errorPage("操作失败,请联系工作人员");
                                                errorPage.mainPage = mainPage;
                                                errorPage.ShowDialog();
                                            }));
                                            timer.Abort();
                                            return;
                                        }
                                        Judage = false;
                                    }
                                }
                                try { ServerSeting.CcardBLL.rfidReader.DisConnect(); } catch { }
                            }
                            else
                            {
                                this.Dispatcher.BeginInvoke(new System.Action(delegate
                                {
                                    ServerSeting.HostNeedMoney = 0;
                                    MainControl mainControle = new MainControl(mainPage);
                                    mainPage.Grid.Children.Clear();
                                    mainPage.Grid.Children.Add(mainControle);
                                    ServerSeting.ISAdd = false;
                                    errorPage errorPage = new errorPage("IC卡扫描设备连接失败");
                                    errorPage.mainPage = mainPage;
                                    errorPage.ShowDialog();
                                }));
                                timer.Abort();
                                return;
                            }
                        }
                        catch { }
                    }
                }
                if (!ServerSeting.connState)
                {
                  
                    printParamt.IdCard = user.CardNo;
                    ServerSeting.HostNeedMoney = 0;
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        MainControl mainControl = new MainControl(mainPage);
                        mainPage.Grid.Children.Clear();
                        mainPage.Grid.Children.Add(mainControl);
                    });
                    string a = null;
                    if (ServerSeting.transactionRecordInfos != null && ServerSeting.transactionRecordInfos.Count > 0)
                    {
                        a = PrintErrorMsg(printParamt, PrintClass.errorLog, "服务器通讯断开");
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                    {
                        string error = "连接服务器失败";
                        if (!string.IsNullOrEmpty(a))
                            error += "," + a;
                        errorPage errorPage = new errorPage(error);
                        errorPage.mainPage = mainPage;
                        errorPage.ShowDialog();
                    }));
                    ServerSeting.ISAdd = false;
                    timer.Abort();
                }
                else
                {
                    if (ServerSeting.HostNeedMoney == 0)
                    {
                        result = true;

                    }
                }
                Task.Run(() =>
                {
                    if (result && index == 0)
                    {
                        index = 1;
                        ServerSeting.ISAdd = false;
                        object errorMsg = user;
                        switch (business)
                        {
                            case DealBusiness.Add:
                                // 办卡添加数据
                                printParamt.IdCard = user.CardNo;
                                AddUserCard addUserCard = new AddUserCard();
                                if (!addUserCard.AddCard(ref errorMsg))
                                {
                                    
                                    this.Dispatcher.BeginInvoke((System.Action)delegate
                                    {
                                        MainControl mainControl = new MainControl(mainPage);
                                        mainPage.Grid.Children.Clear();
                                        mainPage.Grid.Children.Add(mainControl);
                                    });
                                    Task.Run(() =>
                                    {
                                        if (ServerSeting.hairpin.serialPort.IsOpen)
                                        {
                                            byte[] send = new byte[6];
                                            send[0] = 0x02;
                                            send[1] = 0x43;
                                            send[2] = 0x50;
                                            send[3] = 0x03;
                                            send[4] = 0x12;
                                            send[5] = 0x05;
                                            ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                        }
                                    });
                                    
                                    this.Dispatcher.BeginInvoke((System.Action)delegate
                                    {
                                        LoadIng loadIng = new LoadIng(printParamt, user, DealBusiness.Add, errorMsg.ToString());
                                        loadIng.info = info;
                                        loadIng.mainPage = mainPage;
                                        loadIng.ShowDialog(); 
                                    });
                                    timer.Abort();
                                    break;
                                }
                             
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    MainControl mainControl = new MainControl(mainPage);
                                    mainPage.Grid.Children.Clear();
                                    mainPage.Grid.Children.Add(mainControl);
                                });
                                if (AddLog(1, ""))
                                {
                                   
                                }
                                ServerSeting.LastCardNum = ServerSeting.TempCardNum;
                                printParamt.PIC = user.PIC;
                                printParamt.cardNo = errorMsg.ToString();
                                printParamt.costDespoit = info.CostDeposit;
                                printParamt.SecondDespoit = info.Deposit.toInt();
                                this.Dispatcher.BeginInvoke(new System.Action(delegate
                                {
                                    transactionSucessPage = new TransactionSucessPage();
                                    transactionSucessPage.cardData = user;
                                    transactionSucessPage.mainPage = mainPage;
                                    TransactionSucessShowControl transactionSucessShowControl = new TransactionSucessShowControl(transactionSucessPage, 0);
                                    transactionSucessShowControl.printParamt = printParamt;
                                    transactionSucessPage.GRID.Children.Add(transactionSucessShowControl);
                                    DialogHelper.ShowDialog(transactionSucessPage);
                                }));
                                isPrint = false;
                                timer.Abort();
                                break;
                            case DealBusiness.Reissue:
                                MakeUpDAL makeUpDAL = new MakeUpDAL();
                                if (!makeUpDAL.MakeUpCard(ref errorMsg))
                                {
                                    this.Dispatcher.BeginInvoke((System.Action)delegate
                                    {
                                        MainControl mainControl = new MainControl(mainPage);
                                        mainPage.Grid.Children.Clear();
                                        mainPage.Grid.Children.Add(mainControl);
                                    });
                                    Task.Run(() =>
                                    {
                                        if (ServerSeting.hairpin.serialPort.IsOpen)
                                        {
                                            byte[] send = new byte[6];
                                            send[0] = 0x02;
                                            send[1] = 0x43;
                                            send[2] = 0x50;
                                            send[3] = 0x03;
                                            send[4] = 0x12;
                                            send[5] = 0x05;
                                            ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                        }
                                    });
                                    this.Dispatcher.BeginInvoke((System.Action)delegate
                                    {
                                        LoadIng loadIng = new LoadIng(printParamt, user, DealBusiness.Reissue, errorMsg.ToString());
                                        loadIng.info = info;
                                        loadIng.mainPage = mainPage;
                                        loadIng.ShowDialog();
                                    });
                                    ServerSeting.ISAdd = false;
                                    timer.Abort();
                                    break;
                                }
                                else
                                {
                                    this.Dispatcher.BeginInvoke((System.Action)delegate
                                    {
                                        MainControl mainControl = new MainControl(mainPage);
                                        mainPage.Grid.Children.Clear();
                                        mainPage.Grid.Children.Add(mainControl);
                                    });
                                    AddLog(1, "");
                                    ServerSeting.LastCardNum = ServerSeting.TempCardNum;
                                    printParamt.cardNo = errorMsg.ToString();
                                    printParamt.costDespoit = info.CostDeposit;
                                    printParamt.SecondDespoit = info.Deposit.toInt();
                                    // PrintErrorMsg(printParamt, PrintClass.Reissue);
                                    this.Dispatcher.BeginInvoke(new System.Action(delegate
                                    {
                                        TransactionSucessPage transactionSucessPage = new TransactionSucessPage();
                                        transactionSucessPage.cardData = user;
                                        transactionSucessPage.mainPage = mainPage;
                                        TransactionSucess2ShowControl transactionSucessShowControls = new TransactionSucess2ShowControl(transactionSucessPage);
                                        transactionSucessShowControls.printParamt = printParamt;
                                        transactionSucessPage.GRID.Children.Add(transactionSucessShowControls);
                                        transactionSucessPage.ShowDialog();
                                    }));
                                    isPrint = false;
                                    timer.Abort();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                });
            }
        }

        public bool  AddLog(int state,string reason)
        {
            try
            {
                int money = 0;
                foreach (TransactionRecordInfo info in ServerSeting.transactionRecordInfos)
                {
                    money = money + info.Denomination;
                }
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["ProcessorId"].Value;
                    break;
                }
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("orderNumber", orderNumber);
                pairs.Add("state", state);
                pairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                pairs.Add("reason", reason);
                pairs.Add("money", money);
                pairs.Add("equipmentName", Dns.GetHostName());
                pairs.Add("equipmentCode", HDid);
                string url = string.Format("http://{0}:{1}/equipmentmodule/readerTbCardInfo/currency/addEquipmentTbOrder", ServerSeting.ServerIP,ServerSeting.ServerSite);
                Http http = new Http(url, pairs);
                var result = JToken.Parse(http.HttpPosts());
                if (result["state"].ToString().ToLower().Equals("false"))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string PrintErrorMsg(PrintParamtClass @class, PrintClass printClass, string reason)
        {
            @class.orderNumber = orderNumber;
            if (!ServerSeting.connState)
            {
                CreateLog(reason);
            }
            else
            {
                if (!AddLog(0, reason))
                {
                    CreateLog(reason);
                }
            }
            UseM5 useM5 = new UseM5(printClass, @class);
            object errorMsg = "";
            if (!useM5.ConnState(ref errorMsg))
            {
                return errorMsg.ToString();
            }
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
            return null;
        }
        public void CreateLog(string reason)
        {
            if (!Directory.Exists("错误日志/" + DateTime.Now.Year))
                Directory.CreateDirectory("错误日志/" + DateTime.Now.Year);
            if (!Directory.Exists("错误日志/" + DateTime.Now.Year + "/" + DateTime.Now.Month))
                Directory.CreateDirectory("错误日志/" + DateTime.Now.Year + "/" + DateTime.Now.Month);
            string path = "错误日志/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
            XmlDocument document = new XmlDocument();
            if (!File.Exists(path))
            {
                XmlDeclaration header = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement elementt = document.CreateElement("人员档案");
                document.AppendChild(elementt);
                document.Save(path);
            }
            document = new XmlDocument();
            document.Load(path);
            XmlNode xml = document.SelectSingleNode("人员档案");
            XmlElement elements = document.CreateElement("订单号");
            elements.SetAttribute("orderNumber", orderNumber);
            xml.AppendChild(elements);

            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            XmlElement element2 = document.CreateElement("设备编码");
            element2.InnerText = HDid;
            elements.AppendChild(element2);

            XmlElement element3 = document.CreateElement("设备名称");
            element3.InnerText = Dns.GetHostName();
            elements.AppendChild(element3);

            XmlElement element4 = document.CreateElement("交易时间");
            element4.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            elements.AppendChild(element4);

            int money = 0;
            foreach (TransactionRecordInfo info in ServerSeting.transactionRecordInfos)
            {
                money = money + info.Denomination;
            }
            XmlElement element1 = document.CreateElement("收取金额");
            element1.InnerText = money.ToString();
            elements.AppendChild(element1);

            XmlElement element5 = document.CreateElement("失败原因");
            element5.InnerText = reason;
            elements.AppendChild(element5);
            document.Save(path);
            ServerSeting.isGit = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lock (this)
            {
                printParamt = new PrintParamtClass() { name = user.Name, phone = user.phone, cardNo = user.cardNum, costDespoit = info.CostDeposit, SecondDespoit = user.cost };
                ServerSeting.HostNeedMoney = info.CostDeposit;
                ServerSeting.transactionRecordInfos = new List<TransactionRecordInfo>();

                if (string.IsNullOrEmpty(ServerSeting.ICPort))
                {
                    if (!ServerSeting.CcardBLL.conn())
                    {
                        ServerSeting.HostNeedMoney = 0;
                        MainControl mainControl = new MainControl(mainPage);
                        mainPage.Grid.Children.Clear();
                        mainPage.Grid.Children.Add(mainControl);
                        ServerSeting.ISAdd = false;
                        errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查连接");
                        errorPage.mainPage = mainPage;
                        errorPage.ShowDialog();
                        return;
                    }
                }
                else
                {
                    try { ServerSeting.CcardBLL.rfidReader.DisConnect(); } catch { }
                    try
                    {
                        if (!ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                        {
                            ServerSeting.HostNeedMoney = 0;
                            MainControl mainControl = new MainControl(mainPage);
                            mainPage.Grid.Children.Clear();
                            mainPage.Grid.Children.Add(mainControl);
                            ServerSeting.ISAdd = false;
                            errorPage errorPage = new errorPage("IC卡扫描设备出错，请检查连接");
                            errorPage.mainPage = mainPage;
                            errorPage.ShowDialog();
                            return;
                        }
                        else
                        {
                            try { ServerSeting.CcardBLL.rfidReader.DisConnect(); } catch { }
                        }
                    }
                    catch
                    {

                    }
                }
                if (ServerSeting.hairpin.serialPort.IsOpen)
                {
                    byte[] send = new byte[6];
                    send[0] = 0x02;
                    send[1] = 0x44;
                    send[2] = 0x48;
                    send[3] = 0x03;
                    send[4] = 0x0d;
                    send[5] = 0x05;
                    ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                }
                else
                {
                    ServerSeting.HostNeedMoney = 0;
                    MainControl mainControl = new MainControl(mainPage);
                    mainPage.Grid.Children.Clear();
                    mainPage.Grid.Children.Add(mainControl);
                    ServerSeting.ISAdd = false;
                    errorPage errorPage = new errorPage("吐卡器连接异常，请重新连接");
                    errorPage.mainPage = mainPage;
                    errorPage.ShowDialog();
                    return;
                }
                if (!ServerSeting.bFeeder.serialPort.IsOpen)
                {
                    ServerSeting.HostNeedMoney = 0;
                    MainControl mainControl = new MainControl(mainPage);
                    mainPage.Grid.Children.Clear();
                    mainPage.Grid.Children.Add(mainControl);
                    ServerSeting.ISAdd = false;
                    errorPage errorPage = new errorPage("进钞机连接异常，请重新连接");
                    errorPage.mainPage = mainPage;
                    errorPage.ShowDialog();
                    return;
                }
            }
            timer.Start();
        }
    }
    /// <summary>
    /// 补办、新加
    /// </summary>
    public enum DealBusiness
    {
        Add = 0, Reissue = 1
    }
    /// <summary>
    /// 缴费步骤
    /// </summary>
    public enum PayStep
    {
        SecondAdd = 1, FirstAdd = 0, Reissue = 2
    }
}
