using RFID_Reader_Cmds;
using RFID_Reader_Com;
using Rfid系统.DAL;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rfid系统.BLL
{
    public class Rfid
    {
        /// <summary>
        /// 设备连接状态
        /// </summary>
        public bool ConnState { get; set; }

        /// <summary>
        /// 临时值
        /// </summary>
        public string TempData { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public Ac action { get; set; }
        /// <summary>
        /// 访问区域 
        /// </summary>
        public VisitRegion region = VisitRegion.User;
        /// <summary>
        /// EPC
        /// </summary>
        public string parameter;
        /// <summary>
        /// User
        /// </summary>
        public string ReturnUser;
        /// <summary>
        /// 层架标签集合
        /// </summary>
        public List<string> EPCList = new List<string>();
        public ReceiveParser receiveParser { get; set; }
        //public object Control;
        public int i = 0;
        public bool result = true;
        public Rfid(Ac Ac)
        {

            action = Ac;
            //Control = obj;
        }
        /// <summary>
        /// 获取连接状态
        /// </summary>
        public void GetConnState()
        {
            result = true;
            try
            {
                i = 0;
                string[] ports = Sp.GetInstance().GetPortNames();
                while (result)
                {
                    if (i > ports.Length - 1)
                    {
                        result = false;
                        ServerSetting.RfidConnState = false;
                        return;
                    }
                    Sp.GetInstance().Config(ports[i], Int32.Parse("115200"), 0, 8, System.IO.Ports.StopBits.One);
                    ConnState = Sp.GetInstance().Open();
                    if (ConnState)
                    {
                        string send = string.Empty;
                        switch (action)
                        {
                            case Ac.oneTime:
                                send = Commands.BuildReadSingleFrame();
                                break;
                            default:
                                send = Commands.BuildReadMultiFrame(Int32.Parse("65535"));
                                break;
                        }
                        Sp.GetInstance().Send(send);
                        Sp.GetInstance().Send(Commands.BuildGetModuleInfoFrame(ConstCode.MODULE_HARDWARE_VERSION_FIELD));
                        receiveParser = new ReceiveParser();
                        Sp.GetInstance().ComDevice.DataReceived += new SerialDataReceivedEventHandler(receiveParser.DataReceived);
                        receiveParser.PacketReceived += new EventHandler<StrArrEventArgs>(rp_PaketReceived);
                        Thread.Sleep(300);
                        if (result)
                        {
                            Sp.GetInstance().Send(Commands.BuildStopReadFrame());
                            Sp.GetInstance().Close();
                            i++;
                            ServerSetting.RfidConnState = false;
                        }
                        else
                        {
                            ///减少扫描范围
                            Sp.GetInstance().Send("BB 00 B6 00 02 04 E2 9E 7E ");
                            ServerSetting.RfidConnState = true;
                        }
                    }
                    else { i++; }

                }
            }
            catch
            {
                ServerSetting.RfidConnState = false;
            }
        }

        public int s = 0;
        /// <summary>
        /// 扫描信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rp_PaketReceived(object sender, StrArrEventArgs e)
        {
            s++;
            if (s > 50000)
            {
                s = 0;
                Sp.GetInstance().Send(Commands.BuildReadMultiFrame(60000));
            }
            string[] packetRx = e.Data;
            string strPacket = string.Empty;
            for (int i = 0; i < packetRx.Length; i++)
            {
                strPacket += packetRx[i] + " ";
            }

            if (packetRx[1] == ConstCode.FRAME_TYPE_INFO && packetRx[2] == ConstCode.CMD_INVENTORY)
            {
                int PCEPCLength = ((Convert.ToInt32((packetRx[6]), 16)) / 8 + 1) * 2;
                string pc = packetRx[6] + " " + packetRx[7];
                string epc = string.Empty;

                for (int i = 0; i < PCEPCLength - 2; i++)
                {
                    epc = epc + packetRx[8 + i];
                }
                epc = Commands.AutoAddSpace(epc);
                if (!epc.Contains("E2 00 68 0A 8A A8"))
                {
                    TempData = epc;

                    if (epc != null && ServerSetting.EPClist.Count == 0 && !ServerSetting.OldEPClist.Contains(epc))
                    {
                        ServerSetting.EPClist.Enqueue(epc);
                    }
                }
            }
            if (packetRx[2] == ConstCode.CMD_READ_DATA)
            {
                string strInvtReadData = "";
                int dataLen = Convert.ToInt32(packetRx[3], 16) * 256 + Convert.ToInt32(packetRx[4], 16);
                int pcEpcLen = Convert.ToInt32(packetRx[5], 16);

                for (int i = 0; i < dataLen - pcEpcLen - 1; i++)
                {
                    strInvtReadData = strInvtReadData + packetRx[i + pcEpcLen + 6];
                }
                ReturnUser = Commands.AutoAddSpace(strInvtReadData);
            }
            if (packetRx[2] == ConstCode.CMD_GET_MODULE_INFO)
            {
                string hardwareVersion = String.Empty;
                if (packetRx[5] == ConstCode.MODULE_HARDWARE_VERSION_FIELD)
                {
                    try
                    {
                        for (int i = 0; i < Convert.ToInt32(packetRx[4], 16) - 1; i++)
                        {
                            hardwareVersion += (char)Convert.ToInt32(packetRx[6 + i], 16);
                        }
                        result = false;
                        Sp.GetInstance().Send(Commands.BuildReadMultiFrame(60000));
                        // Stop();
                    }
                    catch
                    {
                        hardwareVersion = packetRx[6].Substring(1, 1) + "." + packetRx[7];
                        result = true;
                    }
                }
            }
        }


        public bool IsOpen()
        {
            return Sp.GetInstance().IsOpen();
        }
        /// <summary>
        ///  关闭
        /// </summary>
        public void CloseD()
        {
            Sp.GetInstance().Close();
        }
        /// <summary>
        /// 停止扫描
        /// </summary>
        public void Stop()
        {
            Sp.GetInstance().Send("BB 00 28 00 00 28 7E");
        }
        /// <summary>
        /// 开始扫描
        /// </summary>
        public void Start()
        {
            Sp.GetInstance().Send("BB 00 27 00 03 22 FF FF 4A 7E");
        }
    }

    public enum Ac
    {
        oneTime = 1,
        moreTimes = 0
    }
    public enum VisitRegion
    {
        EPC = 1,
        TID = 2,
        User = 3
    }

}
