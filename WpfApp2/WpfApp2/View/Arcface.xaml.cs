using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;

using System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using AsfFace.Models;
using AsfFace;
using WpfApp2.Model;
using WpfApp2.DAL;
using WpfApp2.Controls;

namespace WpfApp2.View
{
    /// <summary>
    /// Arcface.xaml 的交互逻辑
    /// </summary>
    public partial class Arcface : UserControl
    {
        //public AsfFace.AsfFace face = new AsfFace.AsfFace();
        //public IntPtr hEngine;
        public Arcface()
        {
            InitializeComponent();
#pragma warning disable CS0219 // 变量“result”已被赋值，但从未使用过它的值
            bool result = true;
#pragma warning restore CS0219 // 变量“result”已被赋值，但从未使用过它的值
            ///初始化引擎
            //窗体加载立马初始化引擎
          //  videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //thread = new Thread(new ThreadStart(() =>
            //{
            //    Thread.Sleep(5000);
            //    while (result)
            //    {
            //        this.Dispatcher.BeginInvoke((Action)delegate
            //        {
            //            string picName = "";
            //            ///获取扫描图像
            //            if (player.IsRunning)
            //            {
            //                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //                                player.GetCurrentVideoFrame().GetHbitmap(),
            //                                IntPtr.Zero,
            //                                 Int32Rect.Empty,
            //                                BitmapSizeOptions.FromEmptyOptions());
            //                PngBitmapEncoder pE = new PngBitmapEncoder();
            //                pE.Frames.Add(BitmapFrame.Create(bitmapSource));
            //                picName = AppDomain.CurrentDomain.BaseDirectory + "\\" + Guid.NewGuid() + ".png";
            //                if (File.Exists(picName))
            //                {
            //                    File.Delete(picName);
            //                }
            //                using (Stream stream = File.Create(picName))
            //                {
            //                    pE.Save(stream);
            //                }
            //            }
            //            ///处理图像
            //            if (!string.IsNullOrEmpty(picName))
            //            {
                           // List<byte[]> byteList = GetPICs(picName);
            //                if (byteList.Count == 0)
            //                    return;
            //                foreach (byte[] temp in byteList)
            //                {
            //                    List<FaceInfoModel> faceInfoList = new List<FaceInfoModel>();
            //                    int res = face.GetFaceInfo(hEngine, temp, out faceInfoList);
            //                    MessageBox.Show(faceInfoList.Count + "");
            //                    if (res == AsfConstants.MOK)
            //                    {
            //                        int index = 1;
            //                        foreach (FaceInfoModel faceinfo in faceInfoList)
            //                        {
            //                            foreach (UserMessage user in UserList)
            //                            {
            //                                float similarity = 0;
            //                                byte[] Pic = System.Text.Encoding.Default.GetBytes(user.PIC);
            //                                int reCode = face.FaceMatching(hEngine, temp, Pic, out similarity);
            //                                if (reCode == AsfConstants.MOK)
            //                                {
            //                                    user.Similarity = similarity;
            //                                }
            //                            }
            //                            ///获取相似度最高一个
            //                            ///判断是否大于85
            //                            UserMessage message = UserList.FirstOrDefault(p => p.Similarity == UserList.Max(y => y.Similarity));
            //                            if (message.Similarity > 0)
            //                            {
            //                                MessageBox.Show(message.Name + "相似度：" + message.Similarity);
            //                                result = false;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        });
            //        Thread.Sleep(3000);
            //    }
            //}));
            //thread.IsBackground = true;
        }
        public Thread thread;
        public FilterInfoCollection videoDevices;
        /// <summary>
        /// 数据库人脸数据集
        /// </summary>
        public List<UserMessage> UserList = new List<UserMessage>();
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            ArcfaceControl control = new ArcfaceControl();
            this.grid.Controls.Add(control);
            ///枚举获取第一个摄像设备

            //try
            //{
            //    VideoCaptureDevice videoSourcee = new VideoCaptureDevice(videoDevices[0].MonikerString)
            //    {
            //        DesiredFrameSize = new System.Drawing.Size(320, 240),
            //        DesiredFrameRate = 1
            //    };
            //    player.VideoSource = videoSourcee;
            //    player.Start();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            ///初始化引擎
            //int res = face.InitAsfSDK(out hEngine);
            //if (res != AsfConstants.MOK)
            //{
            //    MessageBox.Show("初始化引擎失败," + face.GetCodeString(res));
            //}
            //else
            //{
            //  thread.Start();
            //}
            //object errorMsg = null;
            //GetArcfoaceUserList userDal = new GetArcfoaceUserList();
            //if (userDal.getBookClass(ref errorMsg))
            //{
            //    UserList = errorMsg as List<UserMessage>;
            //}

        }
        /// <summary>
        /// 获取所有人脸的字节流
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        //public List<byte[]> GetPICs(string filePath)
        //{
        //    List<byte[]> bitmapList = new List<byte[]>();
        //    CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;

        //    //构建级联分类器,利用已经训练好的数据,识别人脸
        //    var face = new CascadeClassifier(@"haarcascade_frontalface_alt.xml");

        //    //加载要识别的图片
        //    var img = new Image<Bgr, byte>(filePath);
        //    var img2 = new Image<Gray, byte>(img.ToBitmap());

        //    //把图片从彩色转灰度
        //    CvInvoke.CvtColor(img, img2, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

        //    //亮度增强
        //    CvInvoke.EqualizeHist(img2, img2);

        //    //在这一步就已经识别出来了,返回的是人脸所在的位置和大小
        //    var facesDetected = face.DetectMultiScale(img2, 1.1, 3, new System.Drawing.Size(45, 50));

        //    //循环把人脸部分切出来并保存
        //    int count = 0;
        //    var b = img.ToBitmap();
        //    foreach (var item in facesDetected)
        //    {
        //        count++;
        //        var bmpOut = new Bitmap(item.Width, item.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //        var g = Graphics.FromImage(bmpOut);
        //        g.DrawImage(b, new Rectangle(0, 0, item.Width, item.Height), new Rectangle(item.X, item.Y, item.Width, item.Height), GraphicsUnit.Pixel);
        //        g.Dispose();
        //        if (File.Exists($"1.png"))
        //            File.Delete($"1.png");
        //        bmpOut.Save($"1.png", System.Drawing.Imaging.ImageFormat.Png);

        //        byte[] data = null;
        //        System.Drawing.Image image = System.Drawing.Image.FromFile($"1.png");
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (Bitmap Bitmap = new Bitmap(image))
        //            {
        //                Bitmap.Save(ms, image.RawFormat);
        //                ms.Position = 0;
        //                data = new byte[ms.Length];
        //                ms.Read(data, 0, Convert.ToInt32(ms.Length));
        //                ms.Flush();
        //            }
        //        }
        //        bitmapList.Add(data);
        //        image.Dispose();
        //        bmpOut.Dispose();
        //    }

        //    //释放资源退出
        //    b.Dispose();
        //    img.Dispose();
        //    img2.Dispose();
        //    face.Dispose();

        //    return bitmapList;
        //}
#pragma warning disable CS0414 // 字段“Arcface.picName”已被赋值，但从未使用过它的值
        string picName = "";
#pragma warning restore CS0414 // 字段“Arcface.picName”已被赋值，但从未使用过它的值
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ///获取扫描图像
            //if (player.IsRunning)
            //{
            //    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //                    player.GetCurrentVideoFrame().GetHbitmap(),
            //                    IntPtr.Zero,
            //                     Int32Rect.Empty,
            //                    BitmapSizeOptions.FromEmptyOptions());
            //    MessageBox.Show("1");
            //    PngBitmapEncoder pE = new PngBitmapEncoder();
            //    if (!string.IsNullOrEmpty(picName))
            //    {
            //        if (File.Exists(picName))
            //        {
            //            File.Delete(picName);
            //        }
            //    }
            //    pE.Frames.Add(BitmapFrame.Create(bitmapSource));
            //    picName = AppDomain.CurrentDomain.BaseDirectory + "\\" + Guid.NewGuid() + ".png";
              
            //    using (Stream stream = File.Create(picName))
            //    {
            //        pE.Save(stream);
            //    }
            //}
            /////处理图像
            //if (!string.IsNullOrEmpty(picName))
            //{
            //    List<byte[]> byteList = GetPICs(picName);
            //    MessageBox.Show(byteList.Count+"");
            //    if (byteList.Count == 0)
            //        return;
            //    foreach (byte[] temp in byteList)
            //    {
            //        List<FaceInfoModel> faceInfoList = new List<FaceInfoModel>();
            //        int res = face.GetFaceInfo(hEngine, temp, out faceInfoList);
            //        MessageBox.Show(faceInfoList.Count + "");
            //        if (res == AsfConstants.MOK)
            //        {
            //            int index = 1;
            //            foreach (FaceInfoModel faceinfo in faceInfoList)
            //            {
            //                foreach (UserMessage user in UserList)
            //                {
            //                    float similarity = 0;
            //                    byte[] Pic = System.Text.Encoding.Default.GetBytes(user.PIC);
            //                    int reCode = face.FaceMatching(hEngine, temp, Pic, out similarity);
            //                    if (reCode == AsfConstants.MOK)
            //                    {
            //                        user.Similarity = similarity;
            //                    }
            //                }
            //                ///获取相似度最高一个
            //                ///判断是否大于85
            //                UserMessage message = UserList.FirstOrDefault(p => p.Similarity == UserList.Max(y => y.Similarity));
            //                if (message.Similarity > 0)
            //                {
            //                    MessageBox.Show(message.Name + "相似度：" + message.Similarity);
            //                   // result = false;
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
