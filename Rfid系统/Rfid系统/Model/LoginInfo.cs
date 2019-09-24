using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
    public class LoginInfo
    {
        /// <summary>
        /// 登陆用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeaderAddress { get; set; }
        /// <summary>
        /// 最近登陆时间
        /// </summary>
        public string UpdateDate { get; set; }
    }
}
