using DoorProhibit.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reader;
using DoorProhibit.Controls;

namespace DoorProhibit.BLL
{
    public  class InfraredRfid
    {
        public MainControl mainControl;
        /// <summary>
        /// 串口
        /// </summary>
       public  SerialPort mysp = new SerialPort();
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
        public InfraredRfid(String PortName, int BandRate, String Parity, int DataBits, String StopBits)
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
            catch(Exception EX)
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
                  
                        dealInfraredData.DealZT(RecvBuf,mainControl);
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
    }
}
