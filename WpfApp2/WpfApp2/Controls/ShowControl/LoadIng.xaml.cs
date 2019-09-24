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
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// LoadIng.xaml 的交互逻辑
    /// </summary>
    public partial class LoadIng : Window
    {
        public LoadIng(PrintParamtClass printParamt, CardData cardData, DealBusiness business, string reason)
        {
            InitializeComponent();
            this.reason = reason;
            this.printParamt = printParamt;
            this.CardData = cardData;
            getOrderNumber();
            thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    ServerSeting.ISAdd = false;
                    if (!ServerSeting.connState || !ServerSeting.hairpin.serialPort.IsOpen || ServerSeting.warehouseIsNull || times <= 0)
                    {
                        this.Dispatcher.BeginInvoke((System.Action)delegate
                        {
                            this.Close();
                          
                        });
                       
                        string a = PrintErrorMsg(printParamt, PrintClass.errorLog, this.reason);
                        this.Dispatcher.BeginInvoke((System.Action)delegate
                        {
                            string error = this.reason;
                            if (!string.IsNullOrEmpty(a))
                                error = error + "," + a;
                            errorPage errorPage = new errorPage(error);
                            errorPage.ShowDialog();
                        });
                        thread.Abort();
                        break;
                    }
                    if (string.IsNullOrEmpty(ICCard))
                    {
                        if (num >= 20)
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
                                Thread.Sleep(1000);
                                send = new byte[6];
                                send[0] = 0x02;
                                send[1] = 0x44;
                                send[2] = 0x48;
                                send[3] = 0x03;
                                send[4] = 0x0d;
                                send[5] = 0x05;
                                ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                                times--;
                            }
                            num = 0;
                        }
                        if (ServerSeting.CcardBLL.rfidReader.Connect(ServerSeting.ICPort, 9600))
                        {
                            ICcardMessage ccardMessage = new ICcardMessage();
                            if (ServerSeting.CcardBLL.ReadCard(ref ccardMessage))
                            {
                                string cardnum = "";
                                getstring(ccardMessage.CardNum, ref cardnum);
                                ICCard = cardnum;
                            }
                            try { ServerSeting.CcardBLL.rfidReader.DisConnect(); } catch { }
                        }
                        else
                        {
                            string a = PrintErrorMsg(printParamt, PrintClass.errorLog, this.reason);
                            string error = this.reason;
                            if (!string.IsNullOrEmpty(a))
                                error = error + "," + a;
                            this.Dispatcher.BeginInvoke((System.Action)delegate
                            {
                                errorPage errorPage = new errorPage(error);
                                errorPage.ShowDialog();
                            });

                            this.Dispatcher.BeginInvoke((System.Action)delegate
                            {
                                thread.Abort();
                                this.Close();
                            });
                            break;
                        }
                        num++;
                        Thread.Sleep(200);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(tempICcard))
                            tempICcard = ICCard;
                        else if (ICCard == tempICcard)
                        {
                            ICCard = null;
                            return;
                        }
                        cardData.cardNum = ICCard;
                        object errorMsg = cardData;
                        switch (business)
                        {
                            case DealBusiness.Add:
                                AddUserCard addUserCard = new AddUserCard();
                                if (!addUserCard.AddCard(ref errorMsg))
                                {
                                
                                    num = 20;
                                    this.reason = errorMsg.ToString();
                                    ICCard = null;
                                    break;
                                }
                                //   ServerSeting.LastCardNum = ServerSeting.TempCardNum;
                                printParamt.PIC = cardData.PIC;
                                printParamt.cardNo = errorMsg.ToString();
                                printParamt.costDespoit = info.CostDeposit;
                                printParamt.SecondDespoit = info.Deposit.toInt();
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    this.Close();
                                    AddLog(1, "");
                                    thread.Abort();
                                });
                                this.Dispatcher.BeginInvoke(new System.Action(delegate
                                {
                                    TransactionSucessPage transactionSucessPage = new TransactionSucessPage();
                                    transactionSucessPage.cardData = cardData;
                                    transactionSucessPage.mainPage = mainPage;
                                    TransactionSucessShowControl transactionSucessShowControl = new TransactionSucessShowControl(transactionSucessPage, 0);
                                    transactionSucessShowControl.printParamt = printParamt;
                                    transactionSucessPage.GRID.Children.Add(transactionSucessShowControl);
                                    transactionSucessPage.ShowDialog();
                                }));
                                break;
                            case DealBusiness.Reissue:
                                MakeUpDAL makeUpDAL = new MakeUpDAL();
                                if (!makeUpDAL.MakeUpCard(ref errorMsg))
                                {
                                    num = 20;
                                    this.reason = errorMsg.ToString();
                                    ICCard = null;
                                    break;
                                }
                                //ServerSeting.LastCardNum = ServerSeting.TempCardNum;
                                printParamt.PIC = cardData.PIC;
                                printParamt.cardNo = errorMsg.ToString();
                                printParamt.costDespoit = info.CostDeposit;
                                printParamt.SecondDespoit = info.Deposit.toInt();
                                // PrintErrorMsg(printParamt, PrintClass.Reissue);
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    this.Close();
                                    AddLog(1, "");
                                    thread.Abort();
                                });
                                this.Dispatcher.BeginInvoke(new System.Action(delegate
                                {
                                    TransactionSucessPage transactionSucessPage = new TransactionSucessPage();
                                    transactionSucessPage.cardData = cardData;
                                    transactionSucessPage.mainPage = mainPage;
                                    TransactionSucess2ShowControl transactionSucessShowControls = new TransactionSucess2ShowControl(transactionSucessPage);
                                    transactionSucessShowControls.printParamt = printParamt;
                                    transactionSucessPage.GRID.Children.Add(transactionSucessShowControls);
                                    transactionSucessPage.ShowDialog();

                                }));

                                break;
                        }
                        getOrderNumber();
                    }
                }
            }));
            thread.IsBackground = true;
        }
        public void getOrderNumber()
        {
            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            string url = string.Format("{0}/equipmentmodule/readerTbCardInfo/currency/getEquipmentTbOrderNumber?equipmentCode={1}&&equipmentName={2}", ServerSeting.urlPath, HDid, Dns.GetHostName());
            Http http = new Http(url, null);
            var result = JToken.Parse(http.HttpGet(url));
            orderNumber = result["row"].ToString();
            ServerSeting.OrderNumber = orderNumber;
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
        public MainPage mainPage;
        public ReturnInfo info;
        string ICCard;
        string tempICcard;
        string orderNumber;
        int num = 0;
        int times = 3;
        string reason = "";
        Thread thread;
        PrintParamtClass printParamt;
        CardData CardData;
        public string PrintErrorMsg(PrintParamtClass @class, PrintClass printClass, string reason)
        {
            @class.orderNumber = orderNumber;
            if (!ServerSeting.connState)
            {
                CreateLog(reason);
            }
            else
            {
                try
                {
                    if (!AddLog(0, reason))
                    {
                        CreateLog(reason);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            UseM5 useM5 = new UseM5(printClass, @class);
            object errorMsg = "";
            if (!useM5.ConnState(ref errorMsg))
            {
                return errorMsg.ToString();
            }

            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
            return "";
        }
        /// <summary>
        /// 保留日志
        /// </summary>
        /// <param name="reason"></param>
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
        /// <summary>
        /// 提交数据库
        /// </summary>
        /// <param name="state"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public bool AddLog(int state, string reason)
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
                string url = string.Format("{0}/equipmentmodule/readerTbCardInfo/currency/addEquipmentTbOrder", ServerSeting.urlPath);
                Http http = new Http(url, pairs);
                var result = JToken.Parse(http.HttpPosts());
                if (result["state"].ToString().ToLower().Equals("false"))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ServerSeting.connState)
            {
                this.Close();
                string a = PrintErrorMsg(printParamt, PrintClass.errorLog, reason);
                string error = reason;
                if (!string.IsNullOrEmpty(a))
                    error = error + "," + a;
                errorPage errorPage = new errorPage(error);
                errorPage.ShowDialog();
            }
            if (ServerSeting.hairpin.serialPort.IsOpen)
            {
                if (ServerSeting.warehouseIsNull)
                {
                    this.Close();
                    string a = PrintErrorMsg(printParamt, PrintClass.errorLog, reason);
                    string error = reason;
                    if (!string.IsNullOrEmpty(a))
                        error = error + "," + a;
                    errorPage errorPage = new errorPage(error);
                    errorPage.ShowDialog();

                }
                else
                {
                    byte[] send = new byte[6];
                    send[0] = 0x02;
                    send[1] = 0x44;
                    send[2] = 0x48;
                    send[3] = 0x03;
                    send[4] = 0x0d;
                    send[5] = 0x05;
                    ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
                    thread.Start();
                }
            }
            else
            {
                this.Close();
                string a = PrintErrorMsg(printParamt, PrintClass.errorLog, reason);
                string error = reason;
                if (!string.IsNullOrEmpty(a))
                    error = error + "," + a;
                errorPage errorPage = new errorPage(error);
                errorPage.ShowDialog();
            }
        }
    }
}
