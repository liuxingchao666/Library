using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
    public class QueryInfo
    {
        /// <summary>
        /// 操作id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 出版社
        /// </summary>
        public string Press { get; set; }
        /// <summary>
        /// 出版日期
        /// </summary>
        public string PressDate { get; set; }
        /// <summary>
        /// ISBN
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// 馆藏码
        /// </summary>
        public string CorrectionCode { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public string CorrectionDate { get; set; }
        /// <summary>
        /// 分类号
        /// </summary>
        public string ClassificationName { get; set; }
        /// <summary>
        /// 索取号
        /// </summary>
        public string CallNumber { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public string PageNumber { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }
        /// <summary>
        /// 馆藏地
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// 图书类型
        /// 0  图书  1期刊
        /// </summary>
        public string bop { get; set; }
        /// <summary>
        /// 期刊类型
        /// 0 期刊 1 合刊
        /// </summary>
        public string merge { get; set; }
    }
}
