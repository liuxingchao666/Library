using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.Model
{
    /// <summary>
    /// 书籍信息
    /// </summary>
    public class BookMessage
    {
        /// <summary>
        /// RFID
        /// </summary>
        public string EPC { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        public string User { get; set; }
        public string TID { get; set; }
        /// <summary>
        /// 档案名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 是否处于借出状态
        /// </summary>
        public bool IsOut { get; set; }
        /// <summary>
        /// 库房id
        /// </summary>
        public string fkStoreId { get; set; }
        /// <summary>
        /// 库房名称
        /// </summary>
        public string fkStoreName { get; set; }
        /// <summary>
        /// 报警时间
        /// </summary>
        public DateTime AlarmTime { get; set; }
    }
}
