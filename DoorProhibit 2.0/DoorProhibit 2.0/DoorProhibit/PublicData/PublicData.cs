using DoorProhibit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DoorProhibit.PublicData
{
    public static class PublicData
    {

        public static string toString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }
        public static DateTime? ToDate(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(obj.toString()))
                return null;
            if (DateTime.TryParse(obj.toString(), out DateTime result))
            {
                return result;
            }
            return null;
        }
        public static int ToInt(this object obj)
        {
            if (obj == null)
                return 0;
            if (string.IsNullOrEmpty(obj.ToString()))
                return 0;
            if (int.TryParse(obj.ToString(), out int result))
                return result;
            return 0;
               
        }
        /// <summary>
        /// 运行状态颜色
        /// </summary>
        public static string color;
        /// <summary>
        /// 运行状态
        /// </summary>
        public static string state;
        /// <summary>
        /// 服务器站点
        /// </summary>
        public static string serverSite = ConfigurationManager.AppSettings["ServerPort"];
        /// <summary>
        /// 服务器ip
        /// </summary>
        public static string serverIp = ConfigurationManager.AppSettings["ServerIP"];
        /// <summary>
        /// 书籍扫描模块
        /// 扫描带入信息
        /// </summary>
        public static List<BookMessage> bookList = new List<BookMessage>();
        /// <summary>
        /// 人员进出记录
        /// </summary>
        public static List<BookMessage> PeopleInOrOutList = new List<BookMessage>();
        /// <summary>
        /// 库房编号
        /// </summary>
        public static string StorageRoomNum { get; set; } = ConfigurationManager.AppSettings["StorageRoomNum"];
        /// <summary>
        /// 设备编号
        /// </summary>
        public static string EquipmentNume { get; set; } = ConfigurationManager.AppSettings["EquipmentNume"];
        /// <summary>
        /// 红外扫描到下一个人的进出状态
        /// </summary>
        public static Queue<InOrOut> inOrOut = new Queue<InOrOut>();
        /// <summary>
        /// 人数值
        /// </summary>
        public static int ManNum { get; set; }
        /// <summary>
        /// 进入人数
        /// </summary>
        public static int IntoNum { get; set; }
        /// <summary>
        /// 出去人数
        /// </summary>
        public static int OutNum { get; set; }
        public static string pic = "../images/图标.png";
        public static Delegate adelegate;
        /// <summary>
        /// 单次扫描获取的EPC集合
        /// </summary>
        public static List<string> TempEPCList = new List<string>();
        /// <summary>
        /// 临时值---书籍档案借出列表
        /// </summary>
        public static List<BookMessage> TempbookMessages = new List<BookMessage>();
        /// <summary>
        /// 当前操作的epc
        /// </summary>
        public static Queue<string> EPC = new Queue<string>();
        /// <summary>
        /// 红外扫描到的数据流
        /// </summary>
        public static Queue<byte[]> infraredData = new Queue<byte[]>();
        public static DateTime TJDate { get; set; } =Convert.ToDateTime( "2018-1-1");
        public static string BJEPC { get; set; }
        
    }
    public enum InOrOut
    {
        In=0,
        Out=1
    }
}
