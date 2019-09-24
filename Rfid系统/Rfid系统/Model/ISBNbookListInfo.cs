using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
    public class ISBNbookListInfo
    {
        /// <summary>
        /// 操作id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// 分类号
        /// </summary>
        public string Classification { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 出版社
        /// </summary>
        public string Press { get; set; }
        /// <summary>
        /// ISBN
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// 卷册号
        /// </summary>
        public string JCH { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public string PageNumber { get; set; }
        /// <summary>
        /// 出版日期
        /// </summary>
        public string PressDate { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 索取号
        /// </summary>
        public string CallNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string fkTypeCode { get; set; }
        /// <summary>
        /// 套装书
        /// </summary>
        public string SetBooks { get; set; }
        /// <summary>
        /// 馆内码
        /// </summary>
        public string BookCdoe { get; set; }
        /// <summary>
        /// RFID
        /// </summary>
        public string EPC { get; set; }
        /// <summary>
        /// 馆藏地
        /// </summary>
        public string Place { get; set; }
    }
}
