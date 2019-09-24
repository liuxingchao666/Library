using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rfid系统.Model
{
    public class MarInfo
    {
        /// <summary>
        /// 标头
        /// </summary>
        public string HeadText { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public BitmapImage MarCodePIC { get; set; }
        /// <summary>
        /// 条形码---内容
        /// </summary>
        public string MarCodeContent { get; set; }
        /// <summary>
        /// 条形码1
        /// </summary>
        public BitmapImage MarCodePIC1 { get; set; }
        /// <summary>
        /// 条形码---内容1
        /// </summary>
        public string MarCodeContent1 { get; set; }
        /// <summary>
        /// 条形码2
        /// </summary>
        public BitmapImage MarCodePIC2 { get; set; }
        /// <summary>
        /// 条形码---内容2
        /// </summary>
        public string MarCodeContent2 { get; set; }

        /// <summary>
        /// 条形码2
        /// </summary>
        public BitmapImage MarCodePIC3 { get; set; }
        /// <summary>
        /// 条形码---内容2
        /// </summary>
        public string MarCodeContent3 { get; set; }

        /// <summary>
        /// 条形码2
        /// </summary>
        public BitmapImage MarCodePIC4 { get; set; }
        /// <summary>
        /// 条形码---内容2
        /// </summary>
        public string MarCodeContent4 { get; set; }

        /// <summary>
        /// 条形码2
        /// </summary>
        public BitmapImage MarCodePIC5 { get; set; }
        /// <summary>
        /// 条形码---内容2
        /// </summary>
        public string MarCodeContent5 { get; set; }

        /// <summary>
        /// 条形码2
        /// </summary>
        public BitmapImage MarCodePIC6 { get; set; }
        /// <summary>
        /// 条形码---内容2
        /// </summary>
        public string MarCodeContent6 { get; set; }

        /// <summary>
        /// 条形码2
        /// </summary>
        public BitmapImage MarCodePIC7 { get; set; }
        /// <summary>
        /// 条形码---内容2
        /// </summary>
        public string MarCodeContent7 { get; set; }
        /// <summary>
        /// 内容字号
        /// </summary>
        public float FontSize { get; set; }
    }
}
