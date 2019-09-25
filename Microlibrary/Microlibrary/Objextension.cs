using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        /// <summary>
        /// 接受数据流转图片显示
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BitmapImage ToBitmapImageByByte(this byte[] data)
        {
            if (data == null)
                return null;
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // here
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 本地显示图片转byte[] 数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToByteByBitmapImage(this BitmapImage obj)
        {
            if (obj == null)
                return null;
            byte[] bytearray = null;
            try
            {
                Stream smarket = obj.StreamSource; ;
                if (smarket != null && smarket.Length > 0)
                {
                    //设置当前位置
                    smarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(smarket))
                    {
                        bytearray = br.ReadBytes((int)smarket.Length);
                    }
                }
            }
            catch
            {
                return null;
            }
            return bytearray;
        }
    }
}
