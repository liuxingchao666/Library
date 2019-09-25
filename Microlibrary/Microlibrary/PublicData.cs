using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microlibrary
{
    /// <summary>
    /// 静态公有资源类
    /// </summary>
    public static class PublicData
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string serverIP = ConfigurationManager.AppSettings["serverIP"];
        /// <summary>
        /// 站点
        /// </summary>
        public static string serverPort = ConfigurationManager.AppSettings["serverPort"];
    }
}
