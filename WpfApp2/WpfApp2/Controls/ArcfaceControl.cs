using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using System.Threading;
using WpfApp2.DAL;
using MessageBox = System.Windows.Forms.MessageBox;
using AsfFace;
using WpfApp2.Model;
using AsfFace.Models;
using Emgu.CV;
// using Emgu.CV.Structure;
using static AsfFace.AsfStruct;
using static WpfApp2.BLL.Util;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;

namespace WpfApp2.Controls
{
    public partial class ArcfaceControl : UserControl
    {
        public static string APP_ID = "16997252";
        public static string API_KEY = "M8iCXpKiXYIORRePX4FDT2TD";
        public static string SECRET_KEY = "6OcG9nasAwDX31ScydR8trfWG3eqhPGd";
        /// <summary>
        /// 图片识别OCR
        /// </summary>
        /// <param name="imgData">二进制图片</param>
        /// <returns></returns>
        public static string readOCR(byte[] imgData)
        {
            try
            {
                var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
                var result = client.GeneralBasic(imgData);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"detect_direction", "true"},
        {"probability", "true"}
    };

                // 带参数调用通用文字识别, 图片参数为本地图片
                result = client.AccurateBasic(imgData, options);
                StringBuilder stringBuilder = new StringBuilder();
                var r = JToken.Parse(result.ToString());
                int indexs = r["words_result_num"].ToString().ToInt();
                for (int i = 0; i < indexs; i++)
                {
                    stringBuilder.Append(r["words_result"][i]["words"].ToString());
                    stringBuilder.AppendLine();
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public ArcfaceControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            #region 线程
            thread = new Thread(new ThreadStart(() =>
            {
#pragma warning disable CS0219 // 变量“picName”已被赋值，但从未使用过它的值
                string picName = "1";
#pragma warning restore CS0219 // 变量“picName”已被赋值，但从未使用过它的值
                while (true)
                {
                    this.Invoke((Action)delegate
                    {
                        ///获取扫描图像
                        #region
                        //if (player.IsRunning)
                        //{
                        //    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        //                    player.GetCurrentVideoFrame().GetHbitmap(),
                        //                    IntPtr.Zero,
                        //                     Int32Rect.Empty,
                        //                    BitmapSizeOptions.FromEmptyOptions());
                        //    PngBitmapEncoder pE = new PngBitmapEncoder();
                        //    pE.Frames.Add(BitmapFrame.Create(bitmapSource));
                        //    if (File.Exists(picName))
                        //    {
                        //        File.Delete(picName);
                        //    }
                        //    picName = AppDomain.CurrentDomain.BaseDirectory + "\\" + Guid.NewGuid() + ".png";
                        //    using (Stream stream = File.Create(picName))
                        //    {
                        //        pE.Save(stream);
                        //    }
                        //}
                        ///处理图像
                        #endregion
                        if (player.IsRunning)
                        {
                            try
                            {
                                Bitmap bitmap = player.GetCurrentVideoFrame();
                                //List<byte[]> byteList = GetPICs(bitmap);
                                byte[] data = null;
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    bitmap.Save(stream, ImageFormat.Jpeg);
                                    data = new byte[stream.Length];
                                    stream.Seek(0, SeekOrigin.Begin);
                                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                                }
                                Task.Run(() =>
                                {
                                    string result = readOCR(data);
                                    if (!string.IsNullOrEmpty(result))
                                        richTextBox1.Text = result;
                                });
                            }
                            catch (Exception ex)
                            {

                            }
                            #region
                            //if (byteList.Count == 0)
                            //    return;
                            //foreach (byte[] temp in byteList)
                            //{
                            //    List<FaceInfoModel> faceInfoList = new List<FaceInfoModel>();
                            //    int res = face.GetFaceInfo(hEngine, temp, out faceInfoList);
                            //    if (res == AsfConstants.MOK)
                            //    {
                            //        foreach (FaceInfoModel faceinfo in faceInfoList)
                            //        {
                            //            ///扫描区增加内容
                            //            MemoryStream ms = new MemoryStream(); //新建内存流
                            //            ms.Write(temp, 0, temp.Length); //附值
                            //            pictureBox2.Image = Image.FromStream(ms);
                            //            ms.Close();
                            //            ms.Dispose();
                            //            lb7.Text = faceinfo.age.ToString();
                            //            switch (faceinfo.gender)
                            //            {
                            //                case 1:
                            //                    lb8.Text = "女";
                            //                    break;
                            //                default:
                            //                    lb8.Text = "男";
                            //                    break;
                            //            }
                            //            Lbclear();
                            //            foreach (UserMessage user in UserList)
                            //            {
                            //                float similarity = 0;
                            //                byte[] Pic = Convert.FromBase64String(user.PIC);
                            //                int reCode = face.FaceMatching(hEngine, temp, Pic, out similarity);
                            //                if (reCode == AsfConstants.MOK)
                            //                {
                            //                    user.Similarity = similarity;
                            //                }
                            //            }
                            //            ///获取相似度最高一个
                            //            ///判断是否大于60
                            //            if (UserList != null && UserList.Count > 0)
                            //            {
                            //                UserMessage message = UserList.FirstOrDefault(p => p.Similarity == UserList.Max(y => y.Similarity));
                            //                if (message.Similarity > 0.6)
                            //                {
                            //                    byte[] Pic = Convert.FromBase64String(message.PIC);
                            //                    MemoryStream m = new MemoryStream(); 
                            //                    m.Write(Pic, 0, Pic.Length);
                            //                    pictureBox1.Image = Image.FromStream(m);
                            //                    m.Close();
                            //                    m.Dispose();
                            //                    lb9.Text = message.Name;
                            //                    lb10.Text = message.Sex;
                            //                    lb11.Text = message.age.ToString();
                            //                    lb12.Text = message.Similarity + "";
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                    });
                    Thread.Sleep(5000);
                }
            }));
            #endregion
            thread.IsBackground = true;
        }
        public void Lbclear()
        {

            lb9.Text = "";
            lb10.Text = "";
            lb11.Text = "";
            lb12.Text = "";
            pictureBox1.Image = null;
        }
        public AsfFace.AsfFace face = new AsfFace.AsfFace();
        public IntPtr hEngine;
        public Thread thread;
        public FilterInfoCollection videoDevices;
        public List<UserMessage> UserList;
        //public List<byte[]> GetPICs(Bitmap filePath)
        //{
        //    List<byte[]> bitmapList = new List<byte[]>();
        //    CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;

        //    //构建级联分类器,利用已经训练好的数据,识别人脸
        //    var face = new CascadeClassifier(@"haarcascade_frontalface_alt.xml");

        //    var img2 = new Image<Gray, byte>(filePath);

        //    //亮度增强
        //    CvInvoke.EqualizeHist(img2, img2);

        //    //在这一步就已经识别出来了,返回的是人脸所在的位置和大小
        //    var facesDetected = face.DetectMultiScale(img2, 1.1, 3, new System.Drawing.Size(45, 50));

        //    //循环把人脸部分切出来并保存
        //    int count = 0;
        //    var b = filePath;

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
        //    // img.Dispose();
        //    img2.Dispose();
        //    face.Dispose();

        //    return bitmapList;
        //}
        private void ArcfaceControl_Load(object sender, EventArgs e)
        {
            try
            {

                VideoCaptureDevice videoSourcee = new VideoCaptureDevice(videoDevices[0].MonikerString)
                {

                    DesiredFrameSize = new System.Drawing.Size(320, 240),
                    DesiredFrameRate = 1
                };
                player.VideoSource = videoSourcee;
                player.Start();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            ///初始化引擎
            int res = face.InitAsfSDK(out hEngine);
            if (res != AsfConstants.MOK)
            {
                MessageBox.Show("初始化引擎失败," + face.GetCodeString(res));
            }
            else
            {
                thread.Start();
            }
            //object errorMsg = null;
            //GetArcfoaceUserList userDal = new GetArcfoaceUserList();
            //if (userDal.getBookClass(ref errorMsg))
            //{
            //    UserList = errorMsg as List<UserMessage>;
            //}
        }
        public Pen pen = new Pen(Color.Red);
        /// <summary>
        /// 画框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void player_Paint(object sender, PaintEventArgs e)
        {
            //if (player.IsRunning)
            //{
            //    //得到当前摄像头下的图片
            //    Bitmap bitmap = player.GetCurrentVideoFrame();
            //    if (bitmap == null)
            //    {
            //        return;
            //    }
            //    Graphics g = e.Graphics;
            //    float offsetX = player.Width * 1f / bitmap.Width;
            //    float offsetY = player.Height * 1f / bitmap.Height;
            //    //构建级联分类器,利用已经训练好的数据,识别人脸

            //    var faces = new CascadeClassifier(@"haarcascade_frontalface_alt.xml");
            //    var img2 = new Image<Gray, byte>(bitmap);

            //    var facesDetected = faces.DetectMultiScale(img2, 1.1, 3, new System.Drawing.Size(45, 50));
            //    img2.Dispose();
            //    foreach (var item in facesDetected)
            //    {
            //        float x = item.Left * offsetX;
            //        float width = item.Right * offsetX - x;
            //        float y = item.Top * offsetY;
            //        float height = item.Bottom * offsetY - y;
            //        g.DrawRectangle(pen, x, y, width, height);
            //    }
            //}
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}