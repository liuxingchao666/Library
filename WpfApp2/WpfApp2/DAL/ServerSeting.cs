using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.BLL;
using System.Configuration;

namespace WpfApp2.DAL
{
    /// <summary>
    /// 服务器连接
    /// </summary>
     public static class ServerSeting
    {
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIP { get; set; } = ConfigurationManager.AppSettings["ServerIp"];
        /// <summary>
        /// 站点
        /// </summary>
        public static string ServerSite { get; set; } = ConfigurationManager.AppSettings["ServerPort"];
        /// <summary>
        /// true 为空
        /// false 不为空
        /// </summary>
        public static bool warehouseIsNull = false;
        public static bool connState =false;
        public static string ICPort = null;
        public static string BFPort = null;
        public static string HRPort = null;
        public static string urlPath = string.Format(@"http:\\{0}:{1}", ServerIP, ServerSite);
        /// <summary>
        /// 当前页面所缴纳面额
        /// </summary>
        public static int HostMoney = 0;
        /// <summary>
        /// 当前所需缴纳金额
        /// </summary>
        public static int HostNeedMoney = 0;

        /// <summary>
        /// IC卡连接状态
        /// </summary>
        public static bool ICConnState = false;
        /// <summary>
        /// 吐卡器连接状态
        /// </summary>
        public static bool TConnState = false;
        /// <summary>
        /// 进钞机连接状态
        /// </summary>
        public static bool MConnState = false;
        /// <summary>
        /// 身份证设备连接状态
        /// </summary>
        public static bool IDConnState = false;
        /// <summary>
        /// 订单号
        /// </summary>
        public static string OrderNumber = null;
        /// <summary>
        /// 是否存在需提交错误日志
        /// </summary>
        public static bool isGit = true;
        /// <summary>
        /// 沉睡时间增加
        /// </summary>
        public static bool ISAdd;
        /// <summary>
        /// 读书卡卡号
        /// </summary>
        public static string CardNum { get; set; } = "";
        /// <summary>
        /// 吐卡器
        /// </summary>
        public static Hairpin hairpin = new Hairpin();
        /// <summary>
        /// 进钞机
        /// </summary>
        public static BFeeder bFeeder = new BFeeder();
        /// <summary>
        /// 新型纸币接受器
        /// </summary>
        //public static NewBFeeder newBFeeder = new NewBFeeder();
        /// <summary>
        /// 吐卡器---IC扫描
        /// </summary>
        public static ICcardBLL CcardBLL = new ICcardBLL();
        /// <summary>
        /// ic卡正在使用的串口句柄
        /// </summary>
        public static IntPtr CardComHandle;
        /// <summary>
        /// 发卡器正在使用的串口句柄
        /// </summary>
        public static IntPtr HairpinComHandle;

        public static GetMessage GetMessage = new GetMessage();
        /// <summary>
        /// 单人操作记录
        /// </summary>
        public static List<TransactionRecordInfo> transactionRecordInfos = new List<TransactionRecordInfo>();
        /// <summary>
        /// 保留上一个卡号
        /// </summary>
        public static string LastCardNum;
        /// <summary>
        /// 及时卡号
        /// </summary>
        public static string TempCardNum;
        /// <summary>
        /// 距离沉睡时间
        /// </summary>
        public static int SYSleepTimes;
        public static bool Mus = true;
        public static bool AutomaticDisconnection = false;
    }
}
