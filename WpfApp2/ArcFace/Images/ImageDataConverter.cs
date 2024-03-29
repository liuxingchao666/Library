﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace AsfFace.Images
{
    /// <summary>
    /// 图片信息转换类
    /// 作者:西瓜码农 博客:https://www.xgblog.cn
    /// </summary>
    public class ImageDataConverter
    {
        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, int length);

        /// <summary>
        /// Bitmap转ImageData同时将宽度不为4的倍数的图像进行调整，注意ImageData在用完之后要用Dispose释放掉
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageData ConvertToImageData(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int width = (bitmap.Width + 3) / 4 * 4;
            int bytesCount = bmpData.Height * width * 3;
            IntPtr pImageData = Marshal.AllocCoTaskMem(bytesCount);
            if (width == bitmap.Width)
                CopyMemory(pImageData, bmpData.Scan0, bytesCount);
            else
                for (int i = 0; i < bitmap.Width; i++)
                    //CopyMemory(IntPtr.Add(pImageData, i * width * 3), IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), bmpData.Stride);
                    CopyMemory(new IntPtr(pImageData.ToInt32() + i * width * 3), new IntPtr(bmpData.Scan0.ToInt32() + i * bmpData.Stride), bmpData.Stride);
            bitmap.UnlockBits(bmpData);
            return new ImageData(width, bitmap.Height, pImageData);
        }
    }
}
