using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
     public class dealInfo
    {
        /// <summary>
        /// 传递参数
        /// </summary>
        public object paramter { get; set; }
        /// <summary>
        /// 判断参数
        /// </summary>
        public ListClass Class { get; set; }
    }
    public enum ListClass
    {
        list = 0,
        classList = 1,
        introduce=2
    }
}
