using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class ActionErrormsg
    {
        #region 传递参数
        /// <summary>
        /// 卡号
        /// </summary>
        public string ICCardNum { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string BardCode { get; set; }
        /// <summary>
        /// 参数id集合
        /// </summary>
        public string[] logids { get; set; }
        #endregion
        #region 操作返回值
        /// <summary>
        /// 操作id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorMsg { get; set; }
        /// <summary>
        /// 预计到期时间
        /// </summary>
        public string planReturnTime { get; set; }
        #endregion
    }
}
