using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
     public class BingHistoryInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 索取号
        /// </summary>
        public string callNumber { get; set; }
        /// <summary>
        /// 书籍编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// rfid
        /// </summary>
        public string rfid { get; set; }
        /// <summary>
        /// 0 失败  1 成功
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 绑定员
        /// </summary>
        public string person { get; set; }
    }
}
