using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
     public  class PeriodicalCollectionInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 馆内码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 馆藏地
        /// </summary>
        public string placeCode { get; set; }
        /// <summary>
        /// 合刊估价
        /// </summary>
        public string hkPrice { get; set; }
        /// <summary>
        /// 合刊备注
        /// </summary>
        public string hkRemark { get; set; }
        /// <summary>
        /// epc
        /// </summary>
        public string RFID { get; set; }
        /// <summary>
        /// 索取号
        /// </summary>
        public string callNumber { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string available { get; set; }
        /// <summary>
        /// 是否外界
        /// </summary>
        public string lendingPermission { get; set; }
        /// <summary>
        /// issn
        /// </summary>
        public string issn { get; set; }
    }
}
