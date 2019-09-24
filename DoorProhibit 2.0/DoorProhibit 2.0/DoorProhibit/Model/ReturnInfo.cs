using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.Model
{
    public class ReturnInfo
    {
        /// <summary>
        /// 成功 失败
        /// </summary>
        public bool trueOrFlase { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        public object result { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string stateCode { get; set; }
    }
}
