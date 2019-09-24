using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
     public class ReturnInfo
    {
        /// <summary>
        /// 成功失败
        /// </summary>
        public bool SuccessOrFalse;
        /// <summary>
        /// 押金
        /// </summary>
        public int Deposit;
        /// <summary>
        /// 错误信息
        /// </summary>
        public object errorMsg;
        /// <summary>
        /// 总需缴纳
        /// </summary>
        public int CostDeposit;
    }
}
