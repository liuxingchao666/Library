using DoorProhibit.BLL;
using DoorProhibit.DAL;
using DoorProhibit.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DoorProhibit.Controls
{
    /// <summary>
    /// MainControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = new mainVeiwModel(mainWindow,this);
            thread = new Thread(new ThreadStart(() => {
                while (true)
                {
                    if (PublicData.PublicData.state == "未连接服务器")
                    {
                        PublicData.PublicData.pic = "../images/网络连接失败.png";
                        PublicData.PublicData.color = "#13D0FF";
                        if (!Login.login())
                        {
                            PublicData.PublicData.state = "未连接服务器";
                            PublicData.PublicData.color = "#13D0FF";
                            PublicData.PublicData.pic = "../images/网络连接失败.png";
                            return;
                        }
                        else
                        {
                            if (mysp.IsOpen)
                            {
                                PublicData.PublicData.state = "正常";
                                PublicData.PublicData.pic = "../images/图标.png";
                            }
                        }
                    }
                    if (mysp.IsOpen && Login.login())
                    {
                        PublicData.PublicData.state = "正常";
                        PublicData.PublicData.pic = "../images/图标.png";
                    }
                    if (!mysp.IsOpen)
                    {
                        PublicData.PublicData.state = "串口连接失败";
                    }
                    if (!string.IsNullOrEmpty(PublicData.PublicData.BJEPC))
                    {
                        byte[] data = getByte(); ;
                        mysp.RtsEnable = true;
                        mysp.Write(data, 0, data.Length);
                        mysp.RtsEnable = false;
                        PublicData.PublicData.BJEPC = null;
                        PublicData.PublicData.state = "报警";
                        PublicData.PublicData.color = "Red";
                        PublicData.PublicData.pic = "../images/报警状态.png";
                    }
                    else
                    {
                        if (PublicData.PublicData.state != "报警")
                        {
                            PublicData.PublicData.color = "#FF7E00";
                            PublicData.PublicData.pic = "../images/图标.png";
                        }
                        if (PublicData.PublicData.state == "串口连接失败")
                        {
                            PublicData.PublicData.color = "#07D7BD";
                            PublicData.PublicData.pic = "../images/串口连接失败.png";
                        }
                        if (PublicData.PublicData.state == "未连接服务器")
                        {
                            PublicData.PublicData.color = "#13D0FF";
                            PublicData.PublicData.pic = "../images/网络连接失败.png";
                        }
                    }
                 
                    Thread.Sleep(1000);
                    if (!mysp.IsOpen)
                    {
                        infraredStart();
                    }
                }
               
            }));
            thread.IsBackground = true;
            thread.Start();
        }
        public byte[] getByte()
        {
            byte[] data = new byte[5];
            data[0] = 0x31;
            data[1] = 0x03;
            data[2] = 0x00;
            data[3] = 0x02;
            data[4] = CheckSum(data, 4);
            return data;
        }
        public byte CheckSum(byte[] bytesToCalc, int dataNum)
        {
            int num = 0;
            byte result = 0x01;
            for (int i = 0; i < dataNum; i++)
            {
                num = (num + bytesToCalc[i]) % 0xffff;
            }
            result = (byte)(~num + 1);
            //实际上num 这里已经是结果了，如果只是取int 可以直接返回了  
            return result;
        }
        public int i = 0;
        public int count = 0;
        public bool InfraredRfidState = false;
        public void infraredStart()
        {
            if (!InfraredRfidState)
            {
                byte[] data = new byte[8];
                data[0] = 0xBB;
                data[1] = 0x00;
                data[2] = 0x03;
                data[3] = 0x00;
                data[4] = 0x01;
                data[5] = 0x00;
                data[6] = 0x04;
                data[7] = 0x7e;
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                count = ports.Length;
                if (count == 0)
                {
                    PublicData.PublicData.state = "串口连接失败";
                    return;
                }
                if (i >= count - 1)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
                InfraredRfid(ports[i], 115200, "None", 8, "1");
                data = WriteIO(data);
                try
                {
                    if (data == null || data[0] != 0xBB)
                    {
                        PublicData.PublicData.state = "串口连接失败";
                        mysp.Close();
                        Thread.Sleep(50);
                        return;
                    }
                    else
                    {
                        InfraredRfidState = true;
                        
                        mysp.Write("BB 00 B6 00 02 07 D0 8F 7E");
                    }
                }
                catch { InfraredRfidState = false; }
            }
            if (!mysp.IsOpen)
            {
                InfraredRfidState = false;
                PublicData.PublicData.state = "串口连接失败";
                return;
            }
            else
            {
                InfraredRfidState = true;
            }
        }
       
        public Thread thread;

        #region
        public SerialPort mysp = new SerialPort();
        /// <summary>
        /// 数据缓冲
        /// </summary>
        byte[] RecvBuf = new byte[5000];
        /// <summary>
        /// 
        /// </summary>
        byte[] RFIDTest;

        int m_nLenth = 0;
        private int m_nReceiveFlag = 0;
        byte[] m_btAryBuffer = new byte[4096];
        public void InfraredRfid(String PortName, int BandRate, String Parity, int DataBits, String StopBits)
        {
            try
            {
                mysp.PortName = PortName.Trim();
                mysp.BaudRate = BandRate;
                mysp.Parity = (Parity)Enum.Parse(typeof(Parity), Parity.Trim(), true);
                mysp.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits.Trim(), true);
                mysp.DataBits = DataBits;
                mysp.ReceivedBytesThreshold = 1;
                mysp.RtsEnable = true;
                mysp.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(sp_DataReceived);
                try
                {
                    mysp.Open();
                    mysp.DiscardInBuffer();
                    mysp.DiscardOutBuffer();
                }
                catch (Exception ee)
                {
                    PublicData.PublicData.state = "串口连接失败";
                }
            }
            catch (Exception EX)
            {
                PublicData.PublicData.state = "串口连接失败";
            }
        }
        /// <summary>
        /// 扫描-->数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">返回值</param>
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int ireadbit = mysp.BytesToRead;
                RecvBuf = new byte[ireadbit];
                mysp.Read(RecvBuf, 0, ireadbit);
                if (RecvBuf.Length <= 0)
                {
                    return;
                }
                if (RecvBuf[0] != 0x00)
                {
                    RFIDTest = RecvBuf;
                }

                DealInfraredData dealInfraredData = new DealInfraredData();
                try
                {
                    dealInfraredData.DealZT(RecvBuf, this);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            catch (Exception ee)
            {
                mysp.DiscardInBuffer();
            }
            // mysp.Close();
        }

        /// <summary>
        /// 不返回
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public byte[] WriteIO(byte[] Command)
        {
            if (!mysp.IsOpen)
            {
                return null;
            }
            mysp.DiscardInBuffer();
            mysp.DiscardOutBuffer();

            mysp.RtsEnable = true;
            mysp.Write(Command, 0, Command.Length);
            mysp.RtsEnable = false;



            RecvBuf = new byte[256];
            byte[] Result = GetReadResult();


            return Result;
        }


        /// <summary>
        /// modbus协议
        /// </summary>
        /// <returns></returns>
        private byte[] GetReadResult()
        {
            int iWaitCount = 0;
            bool a = false;
            Thread.Sleep(500);
            return RecvBuf;
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mysp.Close();
            thread.Abort();
        }
    }
}
