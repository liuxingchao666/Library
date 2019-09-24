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
    /// <summary>
    /// 发卡器
    /// </summary>
    public class Hairpin
    {
        public Hairpin()
        {

        }
#pragma warning disable CS0414 // 字段“Hairpin.errorMsg”已被赋值，但从未使用过它的值
        private string errorMsg = null;
#pragma warning restore CS0414 // 字段“Hairpin.errorMsg”已被赋值，但从未使用过它的值
        /// <summary>
        /// 实例
        /// </summary>
        public SerialPort serialPort = new SerialPort();
        /// <summary>
        /// 
        /// </summary>
        public bool SpConnstate = true;
        /// <summary>
        /// 发送命令
        /// </summary>
        byte[] send = new byte[6];
        /// <summary>
        /// 连接测试
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool conn(ref string errorMsg)
        {
            lock (this)
            {
                try
                {
                    if (ServerSeting.TConnState)
                        return true;
                    this.SpConnstate = true;
                    string[] spList = SerialPort.GetPortNames();
                    int i = 0;
                    byte[] send = new byte[]
                    {
                    2,
                    71,
                    86,
                    3,
                    16,
                    5
                    };
#pragma warning disable CS0168 // 声明了变量“result”，但从未使用过
                    bool result;
#pragma warning restore CS0168 // 声明了变量“result”，但从未使用过
                    while (this.SpConnstate)
                    {
                        try
                        {
                            bool isOpen = this.serialPort.IsOpen;
                            if (isOpen)
                            {
                                this.serialPort.Close();
                            }
                            bool flag = i > spList.Length - 1;
                            if (flag)
                            {
                                this.SpConnstate = false;
                                return false;
                            }
                            this.serialPort.PortName = spList[i];
                            this.serialPort.BaudRate = 9600;
                            this.serialPort.Open();
                            bool isOpen2 = this.serialPort.IsOpen;
                            if (isOpen2)
                            {
                                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.GetData);
                                this.serialPort.Write(send, 0, send.Length);
                                Thread.Sleep(500);
                                i++;
                            }
                            else
                            {
                                
                            }
                        }
                        catch
                        {
                            i++;
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return true;
        }

        public void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int ireadbit = serialPort.BytesToRead;
                byte[] RecvBuf = new byte[ireadbit];
                serialPort.Read(RecvBuf, 0, ireadbit);
                if (RecvBuf.Length >= 2 && RecvBuf[0] == 6 && RecvBuf[1] == 2)
                {
                    SpConnstate = false;
                    ServerSeting.TConnState = true;
                    ServerSeting.HRPort = serialPort.PortName;
                    Thread.Sleep(30);
                }
                if (RecvBuf.Length >= 5)
                {
                    string errorCode = "";
                    for (int i = 0; i < RecvBuf.Length; i++)
                    {
                        errorCode += RecvBuf[i];
                    }
                    Task.Run(() =>
                    {
                        lock (this)
                        {
                            if (errorCode.Contains("484856"))
                            {
                                ServerSeting.warehouseIsNull = true;
                            }
                            if (errorCode.Contains("484948") || errorCode.Contains("484848"))
                            {
                                ServerSeting.warehouseIsNull = false;
                            }
                        }
                    });
                }
            }
            catch { }
        }
    }
}
