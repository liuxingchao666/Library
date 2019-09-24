using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.DAL;

namespace WpfApp2.BLL
{
    public class TransactionRecordInfo
    {
        /// <summary>
        /// 单次交易面额
        /// </summary>
        public int Denomination { get; set; }
        /// <summary>
        /// 单次交易时间
        /// </summary>
        public DateTime? DenominationTime { get; set; }
    }
    public class BFeeder
    {
        public BFeeder()
        {
            thread = new Thread(new ThreadStart(SendTX));
            thread.IsBackground = true;
            thread.Start();

        }
#pragma warning disable CS0414 // 字段“BFeeder.errorMsg”已被赋值，但从未使用过它的值
        private object errorMsg = null;
#pragma warning restore CS0414 // 字段“BFeeder.errorMsg”已被赋值，但从未使用过它的值
        byte[] send = new byte[1];
        public void SendTX()
        {
            try
            {
                send[0] = 0x02;
                while (true)
                {
                    if (serialPort != null && !SpConnState)
                        while (serialPort.IsOpen)
                        {
                            Task.WaitAny();
                            serialPort.Write(send, 0, 1);
                            Thread.Sleep(300);
                        }
                    Thread.Sleep(2000);
                }
            }
            catch { }
        }
        /// <summary>
        /// 实时通讯
        /// </summary>
        public Thread thread;
        /// <summary>
        /// 串口
        /// </summary>
        public SerialPort serialPort = new SerialPort();
        /// <summary>
        /// 串口连接---测试
        /// </summary>
        public bool SpConnState = true;
        /// <summary>
        /// 连接测试
        /// </summary>
        /// <returns></returns>
      	public bool IsConn(ref object errorMsg)
        {
            bool result;
            try
            {
                bool mConnState = ServerSeting.MConnState;
                if (mConnState)
                {
                    return true;
                }
                else
                {
                    byte[] send = new byte[1];
                    send[0] = 0x02;

                    string[] SpList = SerialPort.GetPortNames();
                    this.SpConnState = true;
                    int i = 0;
                    bool result2;
                    while (i <= SpList.Length - 1)
                    {
                        if (!SpConnState)
                        {
                            ServerSeting.MConnState = true;
                            return true;
                        }
                        else
                        {
                            bool isOpen = this.serialPort.IsOpen;
                            if (isOpen)
                            {
                                this.serialPort.Close();
                            }
                            bool flag2 = i > SpList.Length - 1;
                            if (!flag2)
                            {
                                this.serialPort.PortName = SpList[i];
                                this.serialPort.BaudRate = 9600;
                                try
                                {
                                    this.serialPort.Open();
                                    bool isOpen2 = this.serialPort.IsOpen;
                                    if (isOpen2)
                                    {
                                        this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.GetData);
                                        this.serialPort.Write(send, 0, 1);
                                        send[0] = 62;
                                        this.serialPort.Write(send, 0, 1);
                                        Thread.Sleep(500);
                                    }
                                }
                                catch
                                {
                                }
                                i++;
                                continue;
                            }
                            else
                            {
                                this.SpConnState = true;
                                ServerSeting.MConnState = false;
                                this.serialPort.Close();
                                return false;
                            }
                        }
                    }
                    result2 = false;
                    return result2;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] sends = new byte[1];
            int ireadbit = this.serialPort.BytesToRead;
            byte[] RecvBuf = new byte[ireadbit];
            this.serialPort.Read(RecvBuf, 0, ireadbit);
            if (SpConnState)
            {
                string errorCode = "";
                for (int i = 0; i < RecvBuf.Length; i++)
                {
                    errorCode += RecvBuf[i];
                }
                ServerSeting.AutomaticDisconnection = false;
                bool flag = RecvBuf.Length != 0 && RecvBuf[0].toInt() == 62;
                if (flag)
                {
                    SpConnState = false;
                    ServerSeting.MConnState = true;
                    ServerSeting.BFPort = this.serialPort.PortName;
                    sends[0] = 2;
                    this.serialPort.Write(sends, 0, 1);
                    Thread.Sleep(30);
                    return;
                }
            }
            else
            {
                string errorCode = "";
                for (int i = 0; i < RecvBuf.Length; i++)
                {
                    errorCode += RecvBuf[i];
                }
                bool flag6 = errorCode.Contains("128143");
                if (flag6)
                {
                    SpConnState = true;
                    serialPort.Close();
                    ServerSeting.MConnState = false;
                    Thread.Sleep(500);
                    return;
                }

                bool flag3 = RecvBuf.Length >= 2;
                if (flag3)
                {
                    bool flag4 = this.GetCNY(RecvBuf[1]) == 0;
                    if (flag4)
                    {
                        return;
                    }

                    bool flag5 = ServerSeting.HostNeedMoney == 0 || ServerSeting.HostNeedMoney < this.GetCNY(RecvBuf[1]) || !ServerSeting.connState;
                    if (flag5)
                    {
                        sends[0] = 15;

                        this.send[0] = 15;
                        this.serialPort.Write(sends, 0, 1);
                        return;
                    }
                    else
                    {
                        ServerSeting.HostNeedMoney -= this.GetCNY(RecvBuf[1]);
                        this.send[0] = 2;
                        this.serialPort.Write(send, 0, 1);
                        TransactionRecordInfo info = new TransactionRecordInfo
                        {
                            Denomination = this.GetCNY(RecvBuf[1]),
                            DenominationTime = DateTime.Now.ToDate()
                        };
                        ServerSeting.transactionRecordInfos.Add(info);
                    }
                }
                this.serialPort.Write(send, 0, 1);
                sends[0] = 48;
                this.serialPort.Write(sends, 0, 1);
            }
        }

        public int GetCNY(byte data)
        {
            switch (data.toInt())
            {
                case 64:
                    return 1;
                case 65:
                    return 5;
                case 66:
                    return 10;
                case 67:
                    return 20;
                case 68:
                    return 50;
                case 69:
                    return 100;
            }
            return 1000;
        }

    }
    public class NewBFeeder
    {
        private int tempMoney;
        public byte data;
        public Thread thread;
        public void sendMessage()
        {
            bool result = false;
            byte[] send;
            data = 0x07;
            thread = new Thread(new ThreadStart(() => {
                while (true)
                {
                    if (ServerSeting.MConnState)
                    {
                        if (result)
                        {
                            send = new byte[6];
                            send[0] = 0x7F;
                            send[1] = 0x00;
                            send[2] = 0x01;
                            send[3] = data;
                            send[4] = 0x11;
                            send[5] = 0x18;
                            result = false;
                        }
                        else
                        {
                            send = new byte[6];
                            send[0] = 0x7F;
                            send[1] = 0x00;
                            send[2] = 0x01;
                            send[3] = data;
                            send[4] = 0x12;
                            send[5] = 0x02;
                            result = true;
                        }
                        serialPort.Write(send,0,send.Length);
                    }
                    Thread.Sleep(200);
                    data = 0x07;//查询（依次接收）
                }
            }));
            thread.IsBackground = true;
        }
        public SerialPort serialPort;
        public void conn()
        {
            string[] prots = SerialPort.GetPortNames();
            lock (prots)
            {
                byte[] send = new byte[6];
                send[0] = 0x7F;
                send[1] = 0x80;
                send[2] = 0x01;
                send[3] = 0x11;
                send[4] = 0x65;
                send[5] = 0x82;
                foreach (string temp in prots)
                {
                    if (!ServerSeting.MConnState)
                    {
                        serialPort = new SerialPort(temp, 9600);
                        serialPort.DataReceived += new SerialDataReceivedEventHandler(GetData);
                        serialPort.Open();
                        serialPort.Write(send, 0, send.Length);
                        Thread.Sleep(300);
                        if (ServerSeting.TConnState)
                        {
                            ///关闭灯光显示板
                            send = new byte[6];
                            send[0] = 0x7F;
                            send[1] = 0x00;
                            send[2] = 0x01;
                            send[3] = 0x04;
                            send[4] = 0x11;
                            send[5] = 0x88;
                            serialPort.Write(send, 0, send.Length);
                            Thread.Sleep(200);
                            ///进入禁能状态
                            send = new byte[6];
                            send[0] = 0x7F;
                            send[1] = 0x08;
                            send[2] = 0x01;
                            send[3] = 0x09;
                            send[4] = 0x11;
                            send[5] = 0x88;
                            serialPort.Write(send, 0, send.Length);
                            Thread.Sleep(200);
                            ///默认识别所有纸币
                            send = new byte[6];
                            send[0] = 0x7F;
                            send[1] = 0x03;
                            send[2] = 0x02;
                            send[3] = 0xFF;
                            send[4] = 0x24;
                            send[5] = 0x34;
                            serialPort.Write(send, 0, send.Length);
                            break;
                        }
                        else
                        {
                            serialPort.Close();
                            serialPort.Dispose();
                        }
                    }
                }
            }
        }
        public void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            int ireadbit = this.serialPort.BytesToRead;
            byte[] RecvBuf = new byte[ireadbit];
            this.serialPort.Read(RecvBuf, 0, ireadbit);
            string result = "";
            for (int i = 0; i < ireadbit; i++)
            {
                result += RecvBuf[i].ToString("X2");
            }
            if (result.ToLower().Equals("7f8001f02380"))
            {
                ServerSeting.MConnState = true;
            }
            lock (this)
            {
                if (ServerSeting.HostNeedMoney == 0 || !ServerSeting.connState)
                {
                    data = 0x08;//拒收
                    return;
                }
                if (result.ToLower().Contains("d7f6"))
                {
                    if (ServerSeting.HostNeedMoney < GetCNY(RecvBuf[5].ToString("X2")))
                    {
                        data = 0x08;//拒收
                    }
                    else
                    {
                        tempMoney = GetCNY(RecvBuf[5].ToString("X2"));
                    }
                }
                if (result.ToLower().Contains("cceb8d90"))
                {
                    ServerSeting.HostNeedMoney = ServerSeting.HostNeedMoney - tempMoney;
                }
                data=0x01;//复位
            }
        }

        public int GetCNY(string data)
        {
            int result = 0;
            switch (data.toInt())
            {
                case 1:
                    result= 1;
                    break;
                case 2:
                    result = 5;
                    break;
                case 3:
                    result = 10;
                    break;
                case 4:
                    result =20;
                    break;
                case 5:
                    result = 50;
                    break;
                case 6:
                    result = 100;
                    break;
            }
            return result;
        }
    }
}
