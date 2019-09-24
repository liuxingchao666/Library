using DoorProhibit.Controls;
using DoorProhibit.DAL;
using DoorProhibit.Model;
using DoorProhibit.PublicData;
using DoorProhibit.ViewModel;
using Reader;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DoorProhibit.BLL
{
    public class DealInfraredData
    {
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

        public void DealZT(byte[] btAryReceiveData,MainControl control)
        {
            //数据的长度
            int nCount = btAryReceiveData.Length;

            byte[] btAryBuffer = new byte[nCount + m_nLenth];
            ///Copy(Array, Array, Int64) 	

            //从第一个元素开始复制 Array 中的一系列元素，
            //将它们粘贴到另一 Array 中（从第一个元素开始）。 长度指定为 64 位整数。
            Array.Copy(m_btAryBuffer, btAryBuffer, m_nLenth);

            //复制 Array 中的一系列元素（从指定的源索引开始），
            //并将它们粘贴到另一 Array 中（从指定的目标索引开始）。
            //长度和索引指定为 32 位整数。
            Array.Copy(btAryReceiveData, 0, btAryBuffer, m_nLenth, btAryReceiveData.Length);

            //分析接收数据，以0xA0为数据起点，以协议中数据长度为数据终止点
            int nIndex = 0;//当数据中存在A0时，记录数据的终止点
            int nMarkIndex = 0;//当数据中不存在A0时，nMarkIndex等于数据组最大索引
            for (int nLoop = 0; nLoop < btAryBuffer.Length; nLoop++)
            {
                if (btAryBuffer.Length > nLoop + 1)
                {

                    if (btAryBuffer[nLoop] == 0x31)
                    {
                        try
                        {
                            int nLen0 = Convert.ToInt32(btAryBuffer[nLoop + 1]);
                            //数据包从Len后面开始的字节数，不包含Len本身。

                            if (nLoop + 1 + nLen0 < btAryBuffer.Length)
                            {
                                byte[] btAryAnaly = new byte[nLen0 + 2];
                                Array.Copy(btAryBuffer, nLoop, btAryAnaly, 0, nLen0 + 2);

                                //MessageMan messageMan
                                MessageMan messageMan = new MessageMan(btAryAnaly);

                                AnalyDataMan(messageMan,control);

                                nLoop += 1 + nLen0;
                                nIndex = nLoop + 1;
                            }
                            else
                            {
                                nLoop += 1 + nLen0;
                            }
                            if (nIndex < nMarkIndex)
                            {
                                nIndex = nMarkIndex + 1;
                            }

                            if (nIndex < btAryBuffer.Length)
                            {
                                m_nLenth = btAryBuffer.Length - nIndex;
                                Array.Clear(m_btAryBuffer, 0, 4096);
                                Array.Copy(btAryBuffer, nIndex, m_btAryBuffer, 0, btAryBuffer.Length - nIndex);
                            }
                            else
                            {
                                m_nLenth = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }

            DealRF100(btAryReceiveData,control);
        }
        public void AnalyDataMan(MessageMan messagemMan,MainControl control)
        {
            if (messagemMan.BtPacketType != 0x31)
            {
                return;
            }
            switch (messagemMan.BtCmd)
            {
                case 0x04:
                    ProcessMan(messagemMan,control);
                    break;
            }
        }
        private void ProcessMan(MessageMan messagemMan,MainControl control)
        {
            if (messagemMan.BtAryData.Length == 2)
            {
                if (PublicData.PublicData.ManNum != messagemMan.BtAryData[1])
                {

                    AddPeopleInOrOutList addPeople = new AddPeopleInOrOutList();
                    if (PublicData.PublicData.ManNum > messagemMan.BtAryData[1])
                    {
                        PublicData.PublicData.OutNum++;
                        Task.Run(() => { 
                       control.Dispatcher.Invoke(() => {
                         
                           control.OutNum.Content = PublicData.PublicData.OutNum.ToInt() + "人";

                        });
                        });
                        PublicData.PublicData.inOrOut.Enqueue(PublicData.InOrOut.Out);
                        addPeople.AddPeopleInOrOut(PublicData.InOrOut.Out);
                    }
                    else
                    {
                        PublicData.PublicData.IntoNum++;
                        Task.Run(()=> { 
                        control.Dispatcher.Invoke(() => {
                          
                            control.InNum.Content = PublicData.PublicData.IntoNum.ToInt() + "人";

                        });
                        });


                        PublicData.PublicData.inOrOut.Enqueue(PublicData.InOrOut.In);
                        addPeople.AddPeopleInOrOut(PublicData.InOrOut.In);
                    }
                    PublicData.PublicData.ManNum = messagemMan.BtAryData[1];
                }
            }
        }
        public void DealRF100(byte[] data,MainControl control)
        {
            lock (this)
            {
                if (PublicData.PublicData.TJDate == Convert.ToDateTime("2018-1-1"))
                {
                    PublicData.PublicData.TJDate = DateTime.Now;
                }
                else
                {
                    if ((DateTime.Now - PublicData.PublicData.TJDate).TotalSeconds > 5)
                    {
                        PublicData.PublicData.TempEPCList.Clear();
                    }
                }
                int start = 0;
                if (data.Length < 2)
                {
                    return;
                }
                List<string> j = byteToHexStr(data, data.Length);
                bool result = false;
                foreach (string strEPC in j)
                {
                    lock (strEPC)
                    {
                        if (PublicData.PublicData.TempEPCList.Contains(strEPC))
                        {
                            // return;
                        }
                        else
                        {
                            PublicData.PublicData.TJDate = DateTime.Now;
                            PublicData.PublicData.TempEPCList.Add(strEPC);
                            PublicData.PublicData.EPC.Enqueue(strEPC);
                            SelectDAL select = new SelectDAL();
                            ReturnInfo returnInfo = select.Select(strEPC);
                            BookMessage message = new BookMessage() { EPC = strEPC };
                            ///添加档案进出记录
                            if (returnInfo.stateCode != "202")
                            {
                                result = true;
                                AddArchivesInOrOutList addArchivesIn = new AddArchivesInOrOutList(message);
                                addArchivesIn.GetArchivesInOrOut();
                            }
                            ///报警记录判断
                            if (returnInfo.stateCode == "201")
                            {
                                ///添加报警记录
                                AddAlarmList addAlarmList = new AddAlarmList(message);
                                addAlarmList.AddAlarm();
                            }
                        }
                    }
                }
                if (result)
                {
                    Task.Run(() => {
                   
                        GetArchivesAccessList getArchivesAccessList = new GetArchivesAccessList();
                        List<Archives> list = GetArchivesListByList(getArchivesAccessList.GetNewestMessage());
                        ////报警记录
                        GetNewestAlarmList getNewestAlarmList = new GetNewestAlarmList();
                        List<Alarm> alarms = GetAlarmListByList(getNewestAlarmList.GetNewestMessage());
                        control.Dispatcher.Invoke(() =>
                        {
                            control.message.ItemsSource = list;
                            control.AlarmList.ItemsSource = alarms;
                        });
                    });
                }
            }
        }

        public List<string> byteToHexStr(byte[] bytes, int len)  //数组转十六进制字符
        {
            List<string> list = new List<string>();
            string[] Rbyte = new string[bytes.Length];
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    Rbyte[i] = bytes[i].ToString("X2");
                }
            }
            if (Rbyte.Length < 20)
                return list;
            int index = 0;
            while (index < Rbyte.Length)
            {
                if (Rbyte[index] == "BB" && Rbyte[index + 1] == "02")
                {
                    string value = null;
                    for (int i = 0; i < 12; i++)
                    {
                        try
                        {
                            value += Rbyte[index + 8 + i].ToString();
                        }
                        catch
                        {
                            return list;
                        }
                    }
                    list.Add(value);
                    index = index + 12;
                }
                else
                {
                    index++;
                }
            }
            return list;
        }
        public List<Archives> GetArchivesListByList(List<BookMessage> books)
        {
            List<Archives> list = new List<Archives>();
            int index = 1;

            foreach (BookMessage temp in books)
            {
                Archives archives = new Archives()
                {
                    num = index + "、",
                    FileName = temp.FileName,
                    date = temp.AlarmTime
                };
                index++;
                list.Add(archives);
            }
            return list;
        }
        /// <summary>
        /// 获取报警记录
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        public List<Alarm> GetAlarmListByList(List<BookMessage> books)
        {
            List<Alarm> list = new List<Alarm>();
            int index = 1;
            foreach (BookMessage temp in books)
            {
                Alarm archives = new Alarm()
                {
                    num = index + "、",
                    FileName = temp.FileName,
                    AlarmTime = temp.AlarmTime
                };
                index++;
                list.Add(archives);
            }
            return list;
        }
    }
}
