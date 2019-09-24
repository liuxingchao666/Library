using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.BLL
{
    class 新中二代身份证 { }
    /// <summary>
    /// 新中新二代身份证读取
    /// </summary>
    public static class GetNEWID
    {
        #region 接口
        /************************端口类API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetCOMBaud", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetCOMBaud(int iPort, ref uint puiBaudRate);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetCOMBaud", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetCOMBaud(int iPort, uint uiCurrBaud, uint uiSetBaud);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_OpenPort", CharSet = CharSet.Ansi)]
        public static extern int Syn_OpenPort(int iPort);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ClosePort", CharSet = CharSet.Ansi)]
        public static extern int Syn_ClosePort(int iPort);
        /**************************SAM类函数 **************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetMaxRFByte", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetMaxRFByte(int iPort, byte ucByte, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ResetSAM", CharSet = CharSet.Ansi)]
        public static extern int Syn_ResetSAM(int iPort, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMStatus", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMStatus(int iPort, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMID", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMID(int iPort, ref byte pucSAMID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMIDToStr", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMIDToStr(int iPort, ref byte pcSAMID, int iIfOpen);
        /*************************身份证卡类函数 ***************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_StartFindIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_StartFindIDCard(int iPort, ref byte pucIIN, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SelectIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_SelectIDCard(int iPort, ref byte pucSN, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseMsg(int iPort, ref byte pucCHMsg, ref uint puiCHMsgLen, ref byte pucPHMsg, ref uint puiPHMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseMsgToFile", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseMsgToFile(int iPort, ref byte pcCHMsgFileName, ref uint puiCHMsgFileLen, ref byte pcPHMsgFileName, ref uint puiPHMsgFileLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseFPMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseFPMsg(int iPort, ref byte pucCHMsg, ref uint puiCHMsgLen, ref byte pucPHMsg, ref uint puiPHMsgLen, ref byte pucFPMsg, ref uint puiFPMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseFPMsgToFile", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseFPMsgToFile(int iPort, ref byte pcCHMsgFileName, ref uint puiCHMsgFileLen, ref byte pcPHMsgFileName, ref uint puiPHMsgFileLen, ref byte pcFPMsgFileName, ref uint puiFPMsgFileLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadNewAppMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadNewAppMsg(int iPort, ref byte pucAppMsg, ref uint puiAppMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetBmp", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetBmp(int iPort, ref byte Wlt_File);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadFPMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadFPMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData, ref byte cFPhotoname);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindReader", CharSet = CharSet.Ansi)]
        public static extern int Syn_FindReader();
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindUSBReader", CharSet = CharSet.Ansi)]
        public static extern int Syn_FindUSBReader();
        /***********************设置附加功能函数 ************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoPath", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoPath(int iOption, ref byte cPhotoPath);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoName", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoName(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetSexType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetSexType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetNationType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetNationType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetBornType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetBornType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetUserLifeBType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetUserLifeBType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetUserLifeEType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetUserLifeEType(int iType, int iOption);
        #endregion
        /// <summary>
        /// 连接的串口
        /// </summary>
        public static int Port;
        /// <summary>
        /// 波特率
        /// </summary>
        public static int Route = 115200;
        public static IDCardData cardData = new IDCardData();
        public static bool GetData(ref object errorMsg)
        {
            try
            {
                Port = Syn_FindReader();
                if (Port == 0)
                {
                    errorMsg = "获取身份证扫描设备失败";
                    return false;
                }
#pragma warning disable CS0219 // 变量“iPhotoType”已被赋值，但从未使用过它的值
                int nRet, nPort, iPhotoType;
#pragma warning restore CS0219 // 变量“iPhotoType”已被赋值，但从未使用过它的值

                byte[] cPath = new byte[255];
                byte[] pucIIN = new byte[4];
                byte[] pucSN = new byte[8];
                nPort = Port;
                Syn_SetPhotoPath(1, ref cPath[0]);  //设置照片路径	iOption 路径选项	0=C:	1=当前路径	2=指定路径
                                                    //cPhotoPath	绝对路径,仅在iOption=2时有效
                iPhotoType = 0;
                Syn_SetPhotoType(0); //0 = bmp ,1 = jpg , 2 = base64 , 3 = WLT ,4 = 不生成
                Syn_SetPhotoName(1); // 生成照片文件名 0=tmp 1=姓名 2=身份证号 3=姓名_身份证号 

                Syn_SetSexType(1);  // 0=卡中存储的数据	1=解释之后的数据,男、女、未知
                Syn_SetNationType(1);// 0=卡中存储的数据	1=解释之后的数据 2=解释之后加"族"
                Syn_SetBornType(1);         // 0=YYYYMMDD,1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD
                Syn_SetUserLifeBType(1);    // 0=YYYYMMDD,1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD
                Syn_SetUserLifeEType(1, 1); // 0=YYYYMMDD(不转换),1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD,
                                            // 0=长期 不转换,	1=长期转换为 有效期开始+50年           
                if (Syn_OpenPort(nPort) == 0)
                {
                    if (Syn_SetMaxRFByte(nPort, 80, 0) == 0)
                    {
                        nRet = Syn_StartFindIDCard(nPort, ref pucIIN[0], 0);
                        nRet = Syn_SelectIDCard(nPort, ref pucSN[0], 0);
                        nRet = Syn_ReadMsg(nPort, 0, ref cardData);

                        CardData card = new CardData()
                        {
                            Name = cardData.Name,
                            Address = cardData.Address,
                            Nation = cardData.Nation,
                            Sex = cardData.Sex,
                          //  Born = cardData.Born,
                            CardNo = cardData.IDCardNo,
                            GrantDept = cardData.GrantDept,
                            UserLifeBegin = cardData.UserLifeBegin,
                            UserLifeEnd = cardData.UserLifeEnd
                        };
                        //图片转流转string
                        cardData.PhotoFileName = @AppDomain.CurrentDomain.BaseDirectory + cardData.Name.Trim() + ".bmp";
                        if (File.Exists(cardData.PhotoFileName))
                        {
                            FileStream fileStream = new FileStream(cardData.PhotoFileName, FileMode.Open);
                            byte[] data = new byte[fileStream.Length];
                            byte[] temp = new byte[1];
                            for (int i = 0; i < fileStream.Length; i++)
                            {
                                fileStream.Read(temp, 0, 1);
                                data[i] = temp[0];
                            }

                            card.PIC = Convert.ToBase64String(data.ToArray());
                        }
                        errorMsg = card;
                        Syn_ClosePort(nPort);
                        if (string.IsNullOrEmpty(card.Name))
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        Syn_ClosePort(nPort);
                        errorMsg = "设置射频适配器通信字节数失败";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "  打开端口失败";
                    return false;
                }
            }
            catch (Exception ex)
            {

                errorMsg = ex.Message;
                return false;
            }
        }
    }
    /// <summary>
    /// 身份证信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct IDCardData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name; //姓名   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string Sex;   //性别
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Nation; //名族
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string Born; //出生日期
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 72)]
        public string Address; //住址
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
        public string IDCardNo; //身份证号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string GrantDept; //发证机关
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string UserLifeBegin; // 有效开始日期
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string UserLifeEnd;  // 有效截止日期
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
        public string reserved; // 保留
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string PhotoFileName; // 照片路径
    }
    public class CardData
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 名族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Born { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 发证机关
        /// </summary>
        public string GrantDept { get; set; }
        /// <summary>
        /// 生效开始日期
        /// </summary>
        public string UserLifeBegin { get; set; }
        /// <summary>
        /// 有效结束日期
        /// </summary>
        public string UserLifeEnd { get; set; }
        /// <summary>
        /// 图片流
        /// </summary>
        public string PIC { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 读书卡卡号
        /// </summary>
        public string cardNum { get; set; }
        /// <summary>
        /// 缴纳金额
        /// </summary>
        public int cost { get; set; }
        /// <summary>
        /// 二尺办卡金额
        /// </summary>
        public int Despoit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PBcost { get; set; }
    }
}
