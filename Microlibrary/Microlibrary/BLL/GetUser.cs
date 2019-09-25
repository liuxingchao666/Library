using Microlibrary.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microlibrary.BLL
{
   
        public static class GetUpi

        {
            [DllImport("sdtapi.dll")]
            public static extern int SDT_OpenPort(Int16 iIfOpen);
            [DllImport("sdtapi.dll")]
            public static extern int SDT_ClosePort(int iIfOpen);
            [DllImport("sdtapi.dll")]
            public static extern int SDT_StartFindIDCard(int iPort, ref int pucManaInfo, int iIfOpen);
            [DllImport("sdtapi.dll")]
            private static extern int InitComm(int Port);//初始化
            [DllImport("Sdtapi.dll")]
            private static extern int Authenticate();//卡认证
            [DllImport("sdtapi.dll")]
            private static extern void CloseComm(); //关闭端口
            [DllImport("sdtapi.dll")]
            private static extern int ReadBaseInfos(StringBuilder Name,
                StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);//读取数据
            [DllImport("sdtapi.dll")]
            private static extern int HID_BeepLED(bool BeepON, bool LEDON, int duration); //蜂鸣器
            [DllImport("sdtapi.dll")]
            private static extern bool HIDSelect(int index); //设定当前操作的HID接口iDR210
            [DllImport("sdtapi.dll")]
            public static extern int SDT_GetCOMBaud(int iPort, out int puiBaudRate); //判断身份证是否在设备上
            [DllImport("sdtapi.dll")]
            public static extern int SDT_SelectIDCard(int iPort, ref int pucManaMsg, int iIfOpen);
            [DllImport("sdtapi.dll")]
            public static extern int SDT_ReadBaseMsgToFile(int iPort, string pcCHMsgFileName, ref int puiCHMsgFileLen, string pcPHMsgFileName, ref int puiPHMsgFileLen, int iIfOpen);
            [DllImport("sdtapi.dll")]
            public static extern int SDT_SetCOMBaud(int iPort, int uiCurrBaud, int uiSetBaud);
            [DllImport("sdtapi.dll")]
            public static extern int SDT_ResetSAM(int iPort, int uiCurrBaud, int uiSetBaud);
            [DllImport("WltRS.dll")]
            public static extern int GetBmp(string iPort, int uiSetBaud);
        }
        public class GetMessage
        {
            /// <summary>
            /// 正在运行的端口
            /// </summary>
            private int EdziPortID;
            /// <summary>
            /// 端口存在是否可用
            /// </summary>
            private bool bUsbPort;
            /// <summary>
            /// 临时返回值
            /// </summary>
            private int rtnTemp;
            private int EdziIfOpen = 1;
            private int pucIIN = 0;
            private int pucSN = 0;
            private int puiCHMsgLen = 0;
            private int puiPHMsgLen = 0;
            public UserMessage user;

            private readonly string path = @AppDomain.CurrentDomain.BaseDirectory + "\\人员档案.XML";
            /// <summary>
            /// 验证打开的端口
            /// </summary>
            /// <returns></returns>
            public bool OpneOrClose()
            {
                bUsbPort = false;
                for (Int16 i = 1001; i < 1016; i++)
                {
                    if (GetUpi.SDT_OpenPort(i) == 144)
                    {
                        bUsbPort = true;
                        EdziPortID = i;
                        break;
                    }
                }
                return bUsbPort;
            }
            /// <summary>
            /// 执行
            /// </summary>
            public void GO()
            {
                rtnTemp = GetUpi.SDT_StartFindIDCard(EdziPortID, ref pucIIN, EdziIfOpen);
                if (rtnTemp == 159)
                {
                    rtnTemp = GetUpi.SDT_SelectIDCard(EdziPortID, ref pucSN, EdziIfOpen);
                    if (rtnTemp != 144)
                    {
                        rtnTemp = GetUpi.SDT_ClosePort(EdziPortID);
                    }
                    else
                    {
                        ReadWriteMsg();
                        //  LoadControlsByTimer();
                    }
                }
            }
            /// <summary>
            /// 读取身份证信息入类
            /// </summary>
            public void ReadWriteMsg()
            {
                FileInfo objFile = new FileInfo("wz.txt");
                if (objFile.Exists)
                {
                    objFile.Attributes = FileAttributes.Normal;
                    objFile.Delete();
                }

                objFile = new FileInfo("zp.wlt");
                if (objFile.Exists)
                {
                    objFile.Attributes = FileAttributes.Normal;
                    objFile.Delete();
                }
                objFile = new FileInfo("zp.bmp");
                if (objFile.Exists)
                {
                    objFile.Attributes = FileAttributes.Normal;
                    objFile.Delete();
                }

                try
                {
                    rtnTemp = GetUpi.SDT_ReadBaseMsgToFile(EdziPortID, "wz.txt", ref puiCHMsgLen, "zp.wlt", ref puiPHMsgLen, EdziPortID);
                }
                catch (Exception ex)
                {
                    string m = ex.Message;
                }
                if (rtnTemp != 144)
                {
                    rtnTemp = GetUpi.SDT_ClosePort(EdziPortID);
                    return;
                }
                rtnTemp = GetUpi.SDT_ClosePort(EdziPortID);
                FileInfo f = new FileInfo("wz.txt");
                FileStream fs = f.OpenRead();
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, (int)fs.Length);
                fs.Close();
                fs.Dispose();
                f.Delete();
                if (bUsbPort)
                    rtnTemp = GetUpi.GetBmp("zp.wlt", 2);
                else
                    rtnTemp = GetUpi.GetBmp("zp.wlt", 1);
                string str = System.Text.UnicodeEncoding.Unicode.GetString(bt);
                string name = System.Text.UnicodeEncoding.Unicode.GetString(bt, 0, 30).Trim();
                string Sex_Code = System.Text.UnicodeEncoding.Unicode.GetString(bt, 30, 2).Trim();
                string sexName = string.Empty;
                switch (Sex_Code)
                {
                    case "1":
                        sexName = "男";
                        break;
                    default:
                        sexName = "女";
                        break;
                }
                string mation_code = System.Text.UnicodeEncoding.Unicode.GetString(bt, 32, 4).Trim();
                string mation_name = GetNation(mation_code);
                string strBird = System.Text.UnicodeEncoding.Unicode.GetString(bt, 36, 16).Trim();
                strBird = Convert.ToDateTime(strBird.Substring(0, 4) + "年" + strBird.Substring(4, 2) + "月" + strBird.Substring(6) + "日").ToShortDateString();

                string address = System.Text.UnicodeEncoding.Unicode.GetString(bt, 52, 70).Trim();

                string IdentificationCode = System.Text.UnicodeEncoding.Unicode.GetString(bt, 122, 36).Trim();

                string DealDepartment = System.Text.UnicodeEncoding.Unicode.GetString(bt, 158, 30).Trim();

                string strTem = System.Text.UnicodeEncoding.Unicode.GetString(bt, 188, bt.GetLength(0) - 188).Trim();
                string DealDate = Convert.ToDateTime(strTem.Substring(0, 4) + "年" + strTem.Substring(4, 2) + "月" + strTem.Substring(6, 2) + "日").ToShortDateString();

                strTem = strTem.Substring(8);
                string maxdate = string.Empty;
                if (strTem.Trim() != "长期")
                {
                    maxdate = Convert.ToDateTime(strTem.Substring(0, 4) + "年" + strTem.Substring(4, 2) + "月" + strTem.Substring(6, 2) + "日").ToShortDateString();
                }
                else
                {
                    maxdate = DateTime.MaxValue.ToShortDateString();
                }

                objFile = new FileInfo("zp.bmp");
                if (objFile.Exists)
                {
                    Image img = Image.FromFile("zp.bmp");
                    Image PIC_Image = (System.Drawing.Image)img.Clone();
                    System.IO.MemoryStream m = new MemoryStream();
                    img.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] PIC_Byte = m.ToArray();
                    m.Close();
                    m.Dispose();
                    PIC_Image.Dispose();

                    //
                    byte[] data = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Bitmap bitmap = new Bitmap(img))
                        {
                             bitmap.Save(ms, img.RawFormat);
                            ms.Position = 0;
                            data = new byte[ms.Length];
                            ms.Read(data, 0, Convert.ToInt32(ms.Length));
                            ms.Flush();
                        }
                    }
                    string Buffer = Convert.ToBase64String(data.ToArray());
                    //byte[] ImgByte = Convert.FromBase64String(Buffer);
                    //MemoryStream ms = new MemoryStream(ImgByte);
                    //Image image = Image.FromStream(ms);
                    ///人员信息
                    user = new UserMessage()
                    {
                        PIC = Buffer,
                        Name = name,
                        Sex = sexName,
                        NationName = mation_name,
                        BirdTh = Convert.ToDateTime(strBird),
                        IdentificationCode = IdentificationCode,
                        DealDate = Convert.ToDateTime(DealDate),
                        MaxDate = Convert.ToDateTime(maxdate),
                        Address = address,
                        DealDepartment = DealDepartment
                    };
                    img.Dispose();
                    img = null;

                }
            }

            private void LoadControlsByTimer()
            {
                try
                {
                    if (!File.Exists(path))
                    {
                        File.Copy("zp.bmp", AppDomain.CurrentDomain.BaseDirectory + "\\" + user.Name + ".bmp");
                    }
                    else
                    {
                        if (!GetXmlMessage(user.Name))
                        {
                            File.Copy("zp.bmp", AppDomain.CurrentDomain.BaseDirectory + "\\" + user.Name + ".bmp");
                        }
                    }
                    if (File.Exists(path))
                    {
                        if (!GetXmlMessage(user.Name))
                        {
                            AddUserForXml(user);
                        }
                    }
                    else
                    {
                        CreateXml(user);
                    }
                }
                catch
                {
                }
            }
            /// <summary>
            /// 获取民族信息
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public string GetNation(string code)
            {
                SortedList lstMZ = new SortedList();
                lstMZ.Add("01", "汉族");
                lstMZ.Add("02", "蒙古族");
                lstMZ.Add("03", "回族");
                lstMZ.Add("04", "藏族");
                lstMZ.Add("05", "维吾尔族");
                lstMZ.Add("06", "苗族");
                lstMZ.Add("07", "彝族");
                lstMZ.Add("08", "壮族");
                lstMZ.Add("09", "布依族");
                lstMZ.Add("10", "朝鲜族");
                lstMZ.Add("11", "满族");
                lstMZ.Add("12", "侗族");
                lstMZ.Add("13", "瑶族");
                lstMZ.Add("14", "白族");
                lstMZ.Add("15", "土家族");
                lstMZ.Add("16", "哈尼族");
                lstMZ.Add("17", "哈萨克族");
                lstMZ.Add("18", "傣族");
                lstMZ.Add("19", "黎族");
                lstMZ.Add("20", "傈僳族");
                lstMZ.Add("21", "佤族");
                lstMZ.Add("22", "畲族");
                lstMZ.Add("23", "高山族");
                lstMZ.Add("24", "拉祜族");
                lstMZ.Add("25", "水族");
                lstMZ.Add("26", "东乡族");
                lstMZ.Add("27", "纳西族");
                lstMZ.Add("28", "景颇族");
                lstMZ.Add("29", "柯尔克孜族");
                lstMZ.Add("30", "土族");
                lstMZ.Add("31", "达翰尔族");
                lstMZ.Add("32", "仫佬族");
                lstMZ.Add("33", "羌族");
                lstMZ.Add("34", "布朗族");
                lstMZ.Add("35", "撒拉族");
                lstMZ.Add("36", "毛南族");
                lstMZ.Add("37", "仡佬族");
                lstMZ.Add("38", "锡伯族");
                lstMZ.Add("39", "阿昌族");
                lstMZ.Add("40", "普米族");
                lstMZ.Add("41", "塔吉克族");
                lstMZ.Add("42", "怒族");
                lstMZ.Add("43", "乌孜别克族");
                lstMZ.Add("44", "俄罗斯族");
                lstMZ.Add("45", "鄂温克族");
                lstMZ.Add("46", "德昂族");
                lstMZ.Add("47", "保安族");
                lstMZ.Add("48", "裕固族");
                lstMZ.Add("49", "京族");
                lstMZ.Add("50", "塔塔尔族");
                lstMZ.Add("51", "独龙族");
                lstMZ.Add("52", "鄂伦春族");
                lstMZ.Add("53", "赫哲族");
                lstMZ.Add("54", "门巴族");
                lstMZ.Add("55", "珞巴族");
                lstMZ.Add("56", "基诺族");
                lstMZ.Add("57", "其它");
                lstMZ.Add("98", "外国人入籍");
                if (lstMZ.Contains(code))
                {
                    return lstMZ[code].ToString();
                }
                return null;
            }
            /// <summary>
            /// 首次添加
            /// </summary>
            /// <param name="user"></param>
            public void CreateXml(UserMessage user)
            {
                XmlDocument document = new XmlDocument();
                XmlDeclaration header = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                //保存个人信息
                XmlElement elements = document.CreateElement("人员档案");
                document.AppendChild(elements);
                //图片
                XmlElement element = document.CreateElement("PIC");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.PIC.ToString();
                elements.AppendChild(element);
                //名字
                element = document.CreateElement("Name");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.Name;
                elements.AppendChild(element);
                //性别
                element = document.CreateElement("Sex");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.Sex;
                elements.AppendChild(element);
                //民族
                element = document.CreateElement("Nation");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.NationName;
                elements.AppendChild(element);
                //出生日期
                element = document.CreateElement("Birth");
                element.InnerText = user.BirdTh.ToShortDateString();
                element.SetAttribute("name", user.Name);
                elements.AppendChild(element);
                //办理日期
                element = document.CreateElement("DealDate");
                element.InnerText = user.DealDate.ToShortDateString();
                element.SetAttribute("name", user.Name);
                elements.AppendChild(element);
                //有效日期
                element = document.CreateElement("MaxDate");
                element.InnerText = user.MaxDate.ToShortDateString();
                element.SetAttribute("name", user.Name);
                elements.AppendChild(element);
                //身份证号码
                element = document.CreateElement("Card");
                element.InnerText = user.IdentificationCode;
                element.SetAttribute("name", user.Name);
                elements.AppendChild(element);
                //家庭住址
                element = document.CreateElement("Address");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.Address;
                elements.AppendChild(element);
                //签发单位
                element = document.CreateElement("DealDepartment");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.DealDepartment;
                elements.AppendChild(element);
                ///
                document.Save(path);
            }
            /// <summary>
            /// 判断人员档案是否存在
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool GetXmlMessage(string name)
            {
                bool result = false;
                XmlDocument document = new XmlDocument();
                document.Load(path);

                //遍历Name
                XmlNode xml = document.SelectSingleNode("人员档案");
                XmlNodeList list = xml.ChildNodes;
                foreach (XmlNode node in list)
                {
                    if (node.Name == "Name" && node.InnerText == name)
                    {
                        return true;
                    }
                }
                return result;
            }
            /// <summary>
            /// 向人员档案添加人员信息
            /// </summary>
            /// <param name="user"></param>
            public void AddUserForXml(UserMessage user)
            {
                //加载xml文件
                XmlDocument document = new XmlDocument();
                document.Load(path);
                //读取人员档案节点
                XmlNode xml = document.SelectSingleNode("人员档案");
                //图片
                XmlElement element = document.CreateElement("PIC");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.PIC.ToString();
                xml.AppendChild(element);
                //名字
                element = document.CreateElement("Name");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.Name;
                xml.AppendChild(element);
                //性别
                element = document.CreateElement("Sex");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.Sex;
                xml.AppendChild(element);
                //民族
                element = document.CreateElement("Nation");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.NationName;
                xml.AppendChild(element);
                //出生日期
                element = document.CreateElement("Birth");
                element.InnerText = user.BirdTh.ToShortDateString();
                element.SetAttribute("name", user.Name);
                xml.AppendChild(element);
                //办理日期
                element = document.CreateElement("DealDate");
                element.InnerText = user.DealDate.ToShortDateString();
                element.SetAttribute("name", user.Name);
                xml.AppendChild(element);
                //有效日期
                element = document.CreateElement("MaxDate");
                element.InnerText = user.MaxDate.ToShortDateString();
                element.SetAttribute("name", user.Name);
                xml.AppendChild(element);
                //身份证号码
                element = document.CreateElement("Card");
                element.InnerText = user.IdentificationCode;
                element.SetAttribute("name", user.Name);
                xml.AppendChild(element);
                //家庭住址
                element = document.CreateElement("Address");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.Address;
                xml.AppendChild(element);
                //签发单位
                element = document.CreateElement("DealDepartment");
                element.SetAttribute("name", user.Name);
                element.InnerText = user.DealDepartment;
                xml.AppendChild(element);
                ///
                document.Save(path);
            }
        }
    
}
