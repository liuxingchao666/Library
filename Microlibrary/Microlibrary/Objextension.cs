using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microlibrary
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static class Objextension
    {
        /// <summary>
        /// 整数类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this object obj)
        {
            if (obj == null)
                return 0;
            if (int.TryParse(obj.ToString(), out int result))
                return result;
            return 0;
        }
        /// <summary>
        /// 字符串类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(this object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString();
        }
        /// <summary>
        /// 日期类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this object obj)
        {
            if (obj == null)
                return null;
            if (DateTime.TryParse(obj.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }
    }
}
