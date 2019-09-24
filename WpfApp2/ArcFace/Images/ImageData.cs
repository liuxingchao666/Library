using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AsfFace.Images
{
    /// <summary>
    /// 图片信息类
    /// 作者:西瓜码农 博客:https://www.xgblog.cn
    /// </summary>
    public class ImageData:IDisposable
    {
        public ImageData() { }

        public ImageData(int width, int height, IntPtr pImageData, int format = AsfConstants.AsfFacePixelFormat.ASVL_PAF_RGB24_B8G8R8)
        {
            this.Width = width;
            this.Height = height;
            this.PImageData = pImageData;
            this.Format = format;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Format { get; set; }

        public IntPtr PImageData { get; set; }

        public void Dispose()
        {
            Marshal.FreeCoTaskMem(PImageData);
        }

    }
}
