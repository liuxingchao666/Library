using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
     public class PNInfo
    {
        /// <summary>
        /// 期刊号
        /// </summary>
        public string aNumber { get; set; }
        /// <summary>
        /// 编目期刊id
        /// </summary>
        public string fkCataPeriodicalId { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public string page { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 出版日期
        /// </summary>
        public string publicationDateStr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 总期号
        /// </summary>
        public string sNumber { get; set; }
    }
}
