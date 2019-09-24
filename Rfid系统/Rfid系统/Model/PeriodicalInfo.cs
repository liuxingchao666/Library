using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
     public class PeriodicalInfo
    {
        /// <summary>
        /// 期刊id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 索取号
        /// </summary>
        public string callNumber { get; set; }
        /// <summary>
        /// 馆内码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 编目期刊ID
        /// </summary>
        public string fkCataPeriodicalId { get; set; }
        /// <summary>
        /// 馆藏码
        /// </summary>
        public string libraryBookCode { get; set; }
        /// <summary>
        /// 期刊号id
        /// </summary>
        public string pNumberId { get; set; }
        /// <summary>
        /// rfid
        /// </summary>
        public string rfid { get; set; }
        /// <summary>
        /// 外借
        /// </summary>
        public string lendingPermission { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public string available { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        public string placeId { get; set; }
    }
}
