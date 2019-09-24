using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class PrintParamtClass
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNumber { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string PIC { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 读书卡
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public int costDespoit { get; set; }
        /// <summary>
        /// 二次办卡消费
        /// </summary>
        public int SecondDespoit { get; set; }
        /// <summary>
        /// 续借操作状态集合
        /// </summary>
        public List<string> renewlist { get; set; }
    }
}
