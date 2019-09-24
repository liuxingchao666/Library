using Newtonsoft.Json.Linq;
using Rfid系统.BLL;
using Rfid系统.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.DAL
{
    public static class ServerSetting
    {
        /// <summary>
        /// 身份令牌
        /// 登陆成功更新
        /// </summary>
        public static string Authorization { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIP { get; set; } = ConfigurationManager.AppSettings["ServerIp"];
        /// <summary>
        /// 服务器端口
        /// </summary>
        public static string ServerPort { get; set; } = ConfigurationManager.AppSettings["ServerPort"];
        /// <summary>
        /// 访问路径
        /// </summary>
        public static string UrlPath { get; set; } = string.Format(@"http://{0}:{1}/", ServerIP, ServerPort);
        /// <summary>
        /// 图片路径前缀
        /// </summary>
        public static string PICPath { get; set; } = string.Format(@"http://{0}:{1}/filemodule/showFile/getShow", ServerIP, "8090");
        /// <summary>
        /// 当前用户
        /// </summary>
        public static string UserName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public static string HeaderAddress { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public static string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public static string PassWord { get; set; }
        /// <summary>
        /// 操作的EPC
        /// </summary>

        public static Queue<string> EPClist = new Queue<string>();
        /// <summary>
        /// 操作完成后的EPC
        /// </summary>
        public static Queue<string> OldEPClist = new Queue<string>();
        /// <summary>
        /// Rfid连接状态
        /// </summary>
        public static bool RfidConnState;
        /// <summary>
        /// 易控制
        /// </summary>
        public static Rfid rfid = new Rfid(Ac.moreTimes);
        /// <summary>
        /// 使用馆藏地
        /// </summary>
        public static string Place { get; set; }
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static UserInfo userInfo = new UserInfo();
        /// <summary>
        /// 书标打印机
        /// </summary>
        public static string BookmarkPrinterName { get; set; }
        /// <summary>
        /// 条码打印机
        /// </summary>
        public static string BarcodePrinterName { get; set; }
        /// <summary>
        /// 查询结果总页数
        /// </summary>
        public static int totalPages { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public static int loadPage { get; set; }
        /// <summary>
        /// 查询总数据
        /// </summary>
        public static List<QueryInfo> List { get; set; }
        /// <summary>
        /// 当前展示数据
        /// </summary>
        public static List<QueryInfo> info { get; set; }
        /// <summary>
        /// 加载状态
        /// </summary>
        public static bool LoadState { get; set; } = true;
        /// <summary>
        /// 查询参数
        /// </summary>
        public static object parament;
        /// <summary>
        /// 查询参数类型
        /// </summary>
        public static SelectClass @class;
        public static int MarCodeWidth;
        public static int MarCodeHeight;
        public static bool IsOverDue = false;
        public static string APP_ID = "16997252";
        public static string API_KEY = "M8iCXpKiXYIORRePX4FDT2TD";
        public static string SECRET_KEY = "6OcG9nasAwDX31ScydR8trfWG3eqhPGd";
        /// <summary>
        /// 图片识别OCR
        /// </summary>
        /// <param name="imgData">二进制图片</param>
        /// <returns></returns>
        public static string readOCR(byte[] imgData)
        {
            try
            {
                var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
                var result = client.GeneralBasic(imgData);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
                                       {"language_type", "CHN_ENG"},
                                       {"detect_direction", "true"},
                                       {"detect_language", "true"},
                                       {"probability", "true"}};
                // 带参数调用通用文字识别, 图片参数为本地图片
                result = client.GeneralBasic(imgData, options);
                StringBuilder stringBuilder = new StringBuilder();
                var r = JToken.Parse(result.ToString());
                int indexs = r["words_result_num"].ToString().ToInt();
                for (int i = 0; i < indexs; i++)
                {
                    stringBuilder.Append(r["words_result"][i]["words"].ToString());
                }
                return stringBuilder.ToString();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
