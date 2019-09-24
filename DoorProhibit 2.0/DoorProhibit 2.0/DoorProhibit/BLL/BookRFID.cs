﻿using DoorProhibit.Model;
using RFID_Reader_Cmds;
using RFID_Reader_Com;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoorProhibit.BLL
{
    public class BookRFID
    {
    }
    /// <summary>
    /// 书籍扫描
    /// </summary>
    public class BookRfid
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
        public Action action { get; set; }
        /// <summary>
        /// 书籍扫描信息
        /// </summary>
        public BookMessage book { get; set; }
        /// <summary>
        /// 书籍信息
        /// </summary>
        public List<BookMessage> bookList = new List<BookMessage>();
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
        public object Control;
        public BookRfid(Action Action, object obj)
        {
            GetConnState();
            action = Action;
            Control = obj;
        }
        /// <summary>
        /// 获取连接状态
        /// </summary>
        public void GetConnState()
        {
            string[] ports = Sp.GetInstance().GetPortNames();
            Sp.GetInstance().Config(ports[1], Int32.Parse("115200"), 0, 8, System.IO.Ports.StopBits.One);
            ConnState = Sp.GetInstance().Open();
            if (ConnState)
            {
                string send = string.Empty;
                switch (action)
                {
                    case Action.oneTime:
                        send = Commands.BuildReadSingleFrame();
                        break;
                    default:
                        send = Commands.BuildReadMultiFrame(Int32.Parse("65535"));
                        break;
                }
                Sp.GetInstance().Send(send);
                receiveParser = new ReceiveParser();
                Sp.GetInstance().ComDevice.DataReceived += new SerialDataReceivedEventHandler(receiveParser.DataReceived);
                receiveParser.PacketReceived += new EventHandler<StrArrEventArgs>(rp_PaketReceived);
            }
        }
        /// <summary>
        /// 扫描信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rp_PaketReceived(object sender, StrArrEventArgs e)
        {
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
                TempData = epc;
                book = new BookMessage() { EPC = TempData };
                bool isSave = false;
                if (bookList != null && bookList.Count > 0)
                {
                    foreach (BookMessage message in bookList)
                    {
                        if (message.EPC == book.EPC)
                        {
                            isSave = true;
                        }
                    }
                }
                if (!isSave)
                {
                    bookList.Add(book);
                    PublicData.PublicData.bookList.Add(book);
                }

                if (epc.Contains("E2 00 68 0A"))
                {
                    foreach (BookMessage bookMessage in bookList)
                    {
                        if (bookMessage.EPC == epc && bookMessage.User == null)
                        {
                            parameter = epc;
                            ChangeVistsRegion();
                        }
                    }
                }
                else
                {
                    // ChangeVistsRegion();
                   
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
              


                switch (region)
                {
                    case VisitRegion.User:
                        for (int i = 0; i < bookList.Count; i++)
                        {
                            if (bookList[i].EPC == parameter && bookList[i].User == null)
                            {
                                bookList[i].User = Commands.AutoAddSpace(strInvtReadData);
                               
                            }
                        }
                        break;
                    case VisitRegion.TID:
                        for (int i = 0; i < bookList.Count; i++)
                        {
                            if (bookList[i].EPC == parameter && bookList[i].TID == null)
                            {
                                bookList[i].TID = Commands.AutoAddSpace(strInvtReadData);
                            }
                        }
                        region = VisitRegion.User;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 扫描内容切换
        /// </summary>
        public void ChangeVistsRegion()
        {
            string result = Commands.BuildSetSelectFrame(0, 0, 1, 32, 96, 0, TempData);
            Sp.GetInstance().Send(result);
            Thread.Sleep(3);
            string send = string.Empty;
            switch (region)
            {
                case VisitRegion.User:
                    send = Commands.BuildReadDataFrame("00000000", 3, 0, 8);
                    break;
                case VisitRegion.TID:
                    send = Commands.BuildReadDataFrame("00000000", 2, 0, 8);
                    break;
            }
            Sp.GetInstance().Send(send);
        }
        /// <summary>
        /// 暂停扫描
        /// </summary>
        public void StopConntion()
        {
            Sp.GetInstance().Send(Commands.BuildStopReadFrame());
        }
    }
    public enum Action
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