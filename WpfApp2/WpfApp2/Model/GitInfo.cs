using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
     public class GitInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNumber { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int money { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string equipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string equipmentName { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 交易失败原因
        /// </summary>
        public string reason { get; set; }
        /// <summary>
        /// 上传失败原因
        /// </summary>
        public string Gitreason { get; set; }
        /// <summary>
        /// 上传失败时间
        /// </summary>
        public string GitTime { get; set; }
        public int state { get; set; } = 0;
    }
}
