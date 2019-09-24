using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Rfid系统.Model
{
    public  class MarCodeInfo
    {
        /// <summary>
        /// 背景图片
        /// </summary>
        public BitmapImage PICBG { get; set; }
        #region 第一列
        /// <summary>
        /// 图片
        /// </summary>
        public BitmapImage PIC { get; set; }
        /// <summary>
        /// 显示
        /// </summary>
        public Visibility VisibleState { get; set; } = Visibility.Hidden;
        /// <summary>
        /// 馆藏码
        /// </summary>
        public string MarCode { get; set; }
        /// <summary>
        /// 打印图
        /// </summary>
        public BitmapImage PrintPic { get; set; }
        #endregion
        #region 第二列
        /// <summary>
        /// 图片
        /// </summary>
        public BitmapImage PIC1 { get; set; }
        /// <summary>
        /// 显示
        /// </summary>
        public Visibility VisibleState1 { get; set; } = Visibility.Hidden;
        /// <summary>
        /// 馆藏码
        /// </summary>
        public string MarCode1 { get; set; }
        /// <summary>
        /// 打印图
        /// </summary>
        public BitmapImage PrintPic1 { get; set; }
        #endregion
        #region 第三列
        /// <summary>
        /// 图片
        /// </summary>
        public BitmapImage PIC2 { get; set; }
        /// <summary>
        /// 显示
        /// </summary>
        public Visibility VisibleState2 { get; set; } = Visibility.Hidden;
        /// <summary>
        /// 馆藏码
        /// </summary>
        public string MarCode2 { get; set; }
        /// <summary>
        /// 打印图
        /// </summary>
        public BitmapImage PrintPic2 { get; set; }
        #endregion
        #region 第四列
        /// <summary>
        /// 图片
        /// </summary>
        public BitmapImage PIC3 { get; set; }
        /// <summary>
        /// 显示
        /// </summary>
        public Visibility VisibleState3 { get; set; } = Visibility.Hidden;
        /// <summary>
        /// 馆藏码
        /// </summary>
        public string MarCode3 { get; set; }
        /// <summary>
        /// 打印图
        /// </summary>
        public BitmapImage PrintPic3{ get; set; }
        #endregion
    }
}
