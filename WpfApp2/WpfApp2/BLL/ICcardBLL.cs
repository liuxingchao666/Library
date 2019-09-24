using HFrfid;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp2.DAL;
using WpfApp2.Model;

namespace WpfApp2.BLL
{
    public static class newICcardBLL
    {
        /// <summary>
        /// 自动获取串口
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_OpenRU")]
        public static extern int RF610_USB_OpenRU(out IntPtr HidHandle);
        /// <summary>
        /// 关闭当前模块
        /// </summary>
        /// <param name="HidHandle"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_CloseRU")]
        public static extern int RF610_USB_CloseRU(out IntPtr HidHandle);
        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <param name="HidHandle"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_GetHardVersion")]
        public static extern int RF610_USB_GetHardVersion(IntPtr HidHandle,out byte[] VerCode);
        /// <summary>
        /// 读卡号
        /// </summary>
        /// <param name="HidHandle"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_15693GetUid")]
        public static extern int RF610_USB_15693GetUid(IntPtr HidHandle, out byte[] ICcard);
    }
    public class ICcardBLL
    {
        public SerialPort Yserial = new SerialPort();
        public byte[] RevDataBuffer = new byte[30];
        /// <summary>
        /// RFID
        /// </summary>
        public RfidReader rfidReader = new RfidReader();
        /// <summary>
        /// 串口集合
        /// </summary>
        public string[] ports;
        /// <summary>
        /// 使用串口
        /// </summary>
        public string port;
        public bool ConnState = true;
        public bool conn()
        {
            try
            {
                if (ServerSeting.ICConnState)
                    return true;
                ConnState = true;
                ports = SerialPort.GetPortNames();
                int i = 0;
                if (ports.Length > 0)
                {
                    while (i <= ports.Length - 1)
                    {
                        try
                        {
                            Yserial.PortName = ports[i];
                            Yserial.BaudRate = 9600;
                            Yserial.Open();
                            Yserial.DataReceived += new SerialDataReceivedEventHandler(get);
                            byte[] GetWorkModeCmd = new byte[8];

                            GetWorkModeCmd[0] = 0x02;
                            GetWorkModeCmd[1] = 0x08;
                            GetWorkModeCmd[2] = 0xb6;
                            GetWorkModeCmd[3] = 0x20;
                            GetWorkModeCmd[4] = 0x00;
                            GetWorkModeCmd[5] = 0x00;
                            GetWorkModeCmd[6] = 0x00;
                            GetWorkModeCmd[7] = 0x63;
                            Yserial.Write(GetWorkModeCmd, 0, GetWorkModeCmd.Length);
                            if (Yserial.IsOpen)
                            {
                                Thread.Sleep(500);

                                if (!ConnState)
                                {
                                    Yserial.Close();
                                    ServerSeting.ICConnState = true;

                                    return true;
                                }
                                Yserial.Close();
                                i++;

                                if (i > ports.Count() - 1)
                                {
                                    ServerSeting.ICConnState = false;
                                    return false;
                                }
                            }
                            else
                            {
                            
                            }
                        }
                        catch
                        {
                        
                            i++;
                            if (i > ports.Count() - 1)
                            {
                                ServerSeting.ICConnState = false;
                                return false;
                            }
                        }
                    }
                }
                return false;

            }
            catch
            {
              
                return false;
            }
        }
        /// <summary>
        /// 获取可用串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void get(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (Yserial)
                {
                    if (ConnState)
                    {
                        int ireadbit = Yserial.BytesToRead;
                        byte[] data = new byte[8];
                        Yserial.Read(data, 0, data.Length);
                        ServerSeting.ICPort = Yserial.PortName;
                        if (data != null && data[0] == 0x02 && data[1] == 0x09)
                        {
                            ConnState = false;
                            ServerSeting.ICConnState = true;
                        }
                    }
                }
            }
            catch { }
        }
        public string cardnum;
        public string getCard()
        {
            if (string.IsNullOrEmpty(ServerSeting.CardNum))
            {
                Thread.Sleep(300);
                return getCard();
            }
            return ServerSeting.CardNum;
        }
        /// <summary>
        /// 读取卡数据
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ReadCard(ref ICcardMessage message)
        {
            try
            {
                int status; byte[] type = new byte[2]; byte[] id = new byte[4];
                rfidReader.Cmd = Cmd.M1_ReadId;
                //读卡号操作 
                rfidReader.Addr = 0x20;
                //读写器地址 
                // rfidReader.Beep = Beep.On;
                //读卡号成功蜂鸣器发声提示
                status = rfidReader.M1_Operation();
                // 调用M1_Operation方法 
                if (status == 0)
                //判断读卡是否成功 
                {
                    for (int i = 0; i < 2; i++)
                    //获取2字节卡类型 
                    {
                        type[i] = rfidReader.RxBuffer[i];
                    }
                    for (int i = 0; i < 4; i++)
                    //获取4字节卡号 
                    {
                        id[i] = rfidReader.RxBuffer[i + 2];
                    }
                    message = new ICcardMessage()
                    {
                        CardType = byteToHexStr(type, 2),
                        CardNum = byteToHexStr(id, 4)
                    };
                    return true;
                }
                message = new ICcardMessage()
                {
                    CardType = status.ToString(),
                    CardNum = "读取卡号失败"
                };
                return false;
            }
            catch (Exception ex)
            {
                message = new ICcardMessage()
                {
                    CardType = ex.GetType().ToString(),
                    CardNum = ex.Message
                };
                return false;
            }
        }

        public string byteToHexStr(byte[] bytes, int len)  //数组转十六进制字符
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public bool WriteCard(ref string errorMsg, byte[] buff)
        {
            try
            {
                rfidReader.Cmd = Cmd.M1_KeyB_WriteBlock;
                //验证KeyB写数据块操作
                rfidReader.Addr = 0x20;
                //读写器地址
                rfidReader.M1Block = 6;
                //数据块号，M1卡数据块6
                rfidReader.Beep = Beep.On;
                //写数据成功蜂鸣器发声提示
                for (int i = 0; i < 16; i++)
                    rfidReader.TxBuffer[i] = buff[i];
                //将要写的16字节数保存到TxBuffer数组中
                int status = rfidReader.M1_Operation();
                // 调用M1_Operation方法写数据到数据块6中 
                if (status == 0x00)
                {
                    return true;
                }
                errorMsg = "数据写入IC卡失败";
                return false;
            }
            catch (Exception ex)
            {
                errorMsg = "数据写入IC卡失败" + ex.Message;
                return false;
            }
        }
    }
}
