using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rfid系统.Model
{
    public class RetrunInfo
    {
        /// <summary>
        /// 访问失败（和服务器断开连接）
        /// 返回结果
        /// </summary>
        public bool TrueOrFalse { get; set; }
        /// <summary>
        /// 访问成功
        /// 服务器返回Code码
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// 访问成功结果
        /// 保存错误信息
        /// </summary>
        public object result { get; set; }
        /// <summary>
        /// 分页查询专用
        /// </summary>
        public int page { get; set; }
    }
    public class kk
    {
        public BitmapImage img;
        public string l;
    }
}
