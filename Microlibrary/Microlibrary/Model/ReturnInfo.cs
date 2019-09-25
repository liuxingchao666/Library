using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microlibrary.Model
{
    /// <summary>
    /// [服务器]返回数据集[try catch 不进入类]
    /// </summary>
     public class ReturnInfo
    {
        /// <summary>
        /// state
        /// </summary>
        public bool TrueFalse { get; set; }
        /// <summary>
        /// 需要的数据或错误信息
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// code[状态码]
        /// </summary>
        public string Code { get; set; }
    }
}
