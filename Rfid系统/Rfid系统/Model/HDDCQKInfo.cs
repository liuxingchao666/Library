using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
     public class HDDCQKInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 馆内码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 索取号
        /// </summary>
        public string callNumber { get; set; }
        /// <summary>
        /// 在馆状态
        /// </summary>
        public string lendState { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 总期号
        /// </summary>
        public string snumber { get; set; }
        /// <summary>
        /// 刊期号
        /// </summary>
        public string anumber { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsCheck { get; set; }
    }
}
