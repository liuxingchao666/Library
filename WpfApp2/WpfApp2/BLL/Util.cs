using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WpfApp2.Model;

namespace WpfApp2.BLL
{
    public class Util
    {
        public class MemoryUtil
        {
            public static IntPtr Malloc(int len)
            {
                return Marshal.AllocHGlobal(len);
            }

            public static void Free(IntPtr ptr)
            {
                Marshal.FreeHGlobal(ptr);
            }

            public static void Copy(byte[] source, int startIndex, IntPtr destination, int length)
            {
                Marshal.Copy(source, startIndex, destination, length);
            }

            public static void Copy(IntPtr source, byte[] destination, int startIndex, int length)
            {
                Marshal.Copy(source, destination, startIndex, length);
            }

            public static T PtrToStructure<T>(IntPtr ptr)
            {
                return Marshal.PtrToStructure<T>(ptr);
            }

            public static void StructureToPtr<T>(T t, IntPtr ptr)
            {
                Marshal.StructureToPtr<T>(t, ptr, false);
            }

            public static int SizeOf<T>()
            {
                return Marshal.SizeOf<T>();
            }
        }

        public class ImageUtil
        {
            public static ImageInfo ReadBMP(Image image)
            {
                ImageInfo imageInfo = new ImageInfo();
                Bitmap bm = new Bitmap(image);
                BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                ImageInfo result;
                try
                {
                    IntPtr ptr = data.Scan0;
                    int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
                    byte[] sourceBitArray = new byte[soureBitArrayLength];
                    Util.MemoryUtil.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);
                    imageInfo.width = data.Width;
                    imageInfo.height = data.Height;
                    imageInfo.format = 513;
                    int line = imageInfo.width * 3;
                    int pitch = Math.Abs(data.Stride);
                    int bgr_len = line * imageInfo.height;
                    byte[] destBitArray = new byte[bgr_len];
                    for (int i = 0; i < imageInfo.height; i++)
                    {
                        Array.Copy(sourceBitArray, i * pitch, destBitArray, i * line, line);
                    }
                    imageInfo.imgData = Util.MemoryUtil.Malloc(destBitArray.Length);
                    Util.MemoryUtil.Copy(destBitArray, 0, imageInfo.imgData, destBitArray.Length);
                    result = imageInfo;
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    bm.UnlockBits(data);
                }
                result = null;
                return result;
            }

            public static Image MarkRect(Image image, int startX, int startY, int width, int height)
            {
                Image clone = (Image)image.Clone();
                Graphics g = Graphics.FromImage(clone);
                Image result;
                try
                {
                    Brush brush = new SolidBrush(Color.Red);
                    g.DrawRectangle(new Pen(brush, 2f)
                    {
                        DashStyle = DashStyle.Dash
                    }, new Rectangle(startX, startY, width, height));
                    result = clone;
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    g.Dispose();
                }
                result = null;
                return result;
            }

            public static Image MarkRectAndString(Image image, int startX, int startY, int width, int height, int age, int gender, int showWidth)
            {
                Image clone = (Image)image.Clone();
                Graphics g = Graphics.FromImage(clone);
                Image result;
                try
                {
                    Brush brush = new SolidBrush(Color.Red);
                    int penWidth = image.Width / showWidth;
                    g.DrawRectangle(new Pen(brush, (float)((penWidth > 1) ? (2 * penWidth) : 2))
                    {
                        DashStyle = DashStyle.Dash
                    }, new Rectangle((startX < 1) ? 0 : startX, (startY < 1) ? 0 : startY, width, height));
                    string genderStr = "";
                    bool flag = gender >= 0;
                    if (flag)
                    {
                        bool flag2 = gender == 0;
                        if (flag2)
                        {
                            genderStr = "ÄÐ";
                        }
                        else
                        {
                            bool flag3 = gender == 1;
                            if (flag3)
                            {
                                genderStr = "Å®";
                            }
                        }
                    }
                    int fontSize = image.Width / showWidth;
                    bool flag4 = fontSize > 1;
                    if (flag4)
                    {
                        int temp = 12;
                        for (int i = 0; i < fontSize; i++)
                        {
                            temp += 6;
                        }
                        fontSize = temp;
                    }
                    else
                    {
                        bool flag5 = fontSize == 1;
                        if (flag5)
                        {
                            fontSize = 14;
                        }
                        else
                        {
                            fontSize = 12;
                        }
                    }
                    g.DrawString(string.Format("Age:{0}   Gender:{1}", age, genderStr), new Font(FontFamily.GenericSerif, (float)fontSize), brush, (float)((startX < 1) ? 0 : startX), (float)((startY - 20 < 1) ? 0 : (startY - 20)));
                    result = clone;
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    g.Dispose();
                }
                result = null;
                return result;
            }

            public static Image ScaleImage(Image image, int dstWidth, int dstHeight)
            {
                Graphics g = null;
                Image result;
                try
                {
                    float scaleRate = Util.ImageUtil.getWidthAndHeight(image.Width, image.Height, dstWidth, dstHeight);
                    int width = (int)((float)image.Width * scaleRate);
                    int height = (int)((float)image.Height * scaleRate);
                    bool flag = width % 4 != 0;
                    if (flag)
                    {
                        width -= width % 4;
                    }
                    Bitmap destBitmap = new Bitmap(width, height);
                    g = Graphics.FromImage(destBitmap);
                    g.Clear(Color.Transparent);
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle((width - width) / 2, (height - height) / 2, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                    EncoderParameters encoderParams = new EncoderParameters();
                    long[] quality = new long[]
                    {
                        100L
                    };
                    EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, quality);
                    encoderParams.Param[0] = encoderParam;
                    result = destBitmap;
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    bool flag2 = g != null;
                    if (flag2)
                    {
                        g.Dispose();
                    }
                }
                result = null;
                return result;
            }

            public static float getWidthAndHeight(int oldWidth, int oldHeigt, int newWidth, int newHeight)
            {
                bool flag = oldWidth >= newWidth && oldHeigt >= newHeight;
                float scaleRate;
                if (flag)
                {
                    int widthDis = oldWidth - newWidth;
                    int heightDis = oldHeigt - newHeight;
                    bool flag2 = widthDis > heightDis;
                    if (flag2)
                    {
                        scaleRate = (float)newWidth * 1f / (float)oldWidth;
                    }
                    else
                    {
                        scaleRate = (float)newHeight * 1f / (float)oldHeigt;
                    }
                }
                else
                {
                    bool flag3 = oldWidth >= newWidth && oldHeigt < newHeight;
                    if (flag3)
                    {
                        scaleRate = (float)newWidth * 1f / (float)oldWidth;
                    }
                    else
                    {
                        bool flag4 = oldWidth < newWidth && oldHeigt >= newHeight;
                        if (flag4)
                        {
                            scaleRate = (float)newHeight * 1f / (float)oldHeigt;
                        }
                        else
                        {
                            int widthDis2 = newWidth - oldWidth;
                            int heightDis2 = newHeight - oldHeigt;
                            bool flag5 = widthDis2 > heightDis2;
                            if (flag5)
                            {
                                scaleRate = (float)newHeight * 1f / (float)oldHeigt;
                            }
                            else
                            {
                                scaleRate = (float)newWidth * 1f / (float)oldWidth;
                            }
                        }
                    }
                }
                return scaleRate;
            }

            public static Image CutImage(Image src, int left, int top, int right, int bottom)
            {
                Image result;
                try
                {
                    Bitmap srcBitmap = new Bitmap(src);
                    Bitmap dstBitmap = srcBitmap.Clone(new Rectangle(left, top, right - left, bottom - top), PixelFormat.Undefined);
                    result = dstBitmap;
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                result = null;
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ASF_ImagePixelFormat
        {
            public const int ASVL_PAF_RGB24_B8G8R8 = 513;
        }

        public struct MRECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;
        }
    }
}
