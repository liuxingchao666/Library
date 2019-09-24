using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;
using Rfid系统.Model;
using System.Configuration;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;
using System.Windows;
using Size = System.Drawing.Size;

namespace Rfid系统.ViewModel
{
    public class MainViewModel : NotificationObject
    {
        public MainViewModel(MainControl mainControl)
        {
            this.mainControl = mainControl;
            Task.Run(() => {
            UserName = ServerSetting.UserName;
            LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            });
            try
            {
             
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServerSetting.PICPath + ServerSetting.HeaderAddress);
                WebResponse response = request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                LoginPIC = ImageToBitmapImage(img);
                
            }
            catch
            {
                LoginPIC = new BitmapImage(new Uri("默认头像.png", UriKind.RelativeOrAbsolute));
            }
            finally
            {
                RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定点击样式.png", UriKind.RelativeOrAbsolute));
                RFIDBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                SetBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                QuitBK = new BitmapImage(new Uri("BKWDJ1.png", UriKind.RelativeOrAbsolute));
            }
            ShowPIC = "../images/三角形1.png";
            Show = Visibility.Hidden;
            ///个人中心
            User = ServerSetting.userInfo.User;
            PassWord = ServerSetting.PassWord;
            Email = ServerSetting.userInfo.Email;
            IDCard = ServerSetting.userInfo.IDCard;
            Phone = ServerSetting.userInfo.Phone;
            RoleLevel = ServerSetting.userInfo.RoleLevel;
          
        }
        /// <summary>
        /// 当前界面数据
        /// </summary>
        public ISBNbookListInfo info;
        private Image CutEllipse(Image img, Rectangle rec, System.Drawing.Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (TextureBrush br = new TextureBrush(img, System.Drawing.Drawing2D.WrapMode.Clamp, rec))
                {
                    br.ScaleTransform(bitmap.Width / (float)rec.Width, bitmap.Height / (float)rec.Height);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillEllipse(br, new Rectangle(System.Drawing.Point.Empty, size));
                }
            }
            return bitmap;
        }
        public BitmapImage ImageToBitmapImage(System.Drawing.Image PIC)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(PIC))
                {
                    Bitmap.Save(ms, PIC.RawFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(data);
            bitmap.EndInit();
            return bitmap;
        }
        public MainControl mainControl;
        public controlClass controlClass = controlClass.rfid;
        /// <summary>
        /// 登陆用户头像
        /// </summary>
        private BitmapImage Loginpic { get; set; }
        public BitmapImage LoginPIC
        {
            get { return Loginpic; }
            set
            {
                Loginpic = value;
                this.RaisePropertyChanged(() => LoginPIC);
            }
        }
        /// <summary>
        /// 登陆用户
        /// </summary>
        private string userName { get; set; }
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                this.RaisePropertyChanged(() => UserName);
            }
        }
        /// <summary>
        /// 登陆时间
        /// </summary>
        private string loginTime { get; set; }
        public string LoginTime
        {
            get { return loginTime; }
            set
            {
                loginTime = value;
                this.RaisePropertyChanged(() => LoginTime);
            }
        }
        /// <summary>
        /// RFID绑定
        /// </summary>
        private ICommand Rfid { get; set; }
        public ICommand RFID
        {
            get
            {
                return Rfid ?? (Rfid = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    if (controlClass == controlClass.rfid)
                        return;

                    controlShow(controlClass.rfid);
                    mainControl.rFIDBindingControl = new RFIDBindingControl(mainControl);
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(mainControl.rFIDBindingControl);
                }));
            }
        }
        public void controlShow(controlClass @class)
        {
            mainControl.loginCS.Focus();
            switch (@class)
            {
                case controlClass.rfid:
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.rfid;
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
                case controlClass.bookmark:
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标2.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.bookmark;
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定无点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
                case controlClass.barcode:
                    BarCodePIC = new BitmapImage(new Uri("../images/条码2.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.barcode;
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定无点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
                case controlClass.query:
                    QueryPIC = new BitmapImage(new Uri("../images/查询2.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.query;
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定无点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
                case controlClass.user:
                    SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.user;
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定无点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
                case controlClass.periodical:
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订2.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.periodical;
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定无点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    SetPIC = new BitmapImage(new Uri("../images/Bing1.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    SetPIC = new BitmapImage(new Uri("../images/期刊绑定2.png", UriKind.RelativeOrAbsolute));
                    SetBK = new BitmapImage(new Uri("BKDJ.png", UriKind.RelativeOrAbsolute));
                    controlClass = controlClass.set;
                    QuitPIC = new BitmapImage(new Uri("../images/过刊合订1.png", UriKind.RelativeOrAbsolute));
                    QuitBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    RFIDPIC = new BitmapImage(new Uri("../images/RFID绑定无点击样式.png", UriKind.RelativeOrAbsolute));
                    RFIDBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BookmarkPIC = new BitmapImage(new Uri("../images/书标1.png", UriKind.RelativeOrAbsolute));
                    BookmarkBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    BarCodePIC = new BitmapImage(new Uri("../images/条码1.png", UriKind.RelativeOrAbsolute));
                    BarCodeBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    QueryPIC = new BitmapImage(new Uri("../images/查询1.png", UriKind.RelativeOrAbsolute));
                    QueryBK = new BitmapImage(new Uri("BKWDJ.png", UriKind.RelativeOrAbsolute));
                    break;
            }
        }
        /// <summary>
        /// RFID点击样式修改
        /// </summary>
        private BitmapImage RFIDpic { get; set; }
        public BitmapImage RFIDPIC
        {
            get { return RFIDpic; }
            set
            {
                RFIDpic = value;
                this.RaisePropertyChanged(() => RFIDPIC);
            }
        }
        private BitmapImage RFIDbk { get; set; }
        public BitmapImage RFIDBK
        {
            get { return RFIDbk; }
            set
            {
                RFIDbk = value;
                this.RaisePropertyChanged(() => RFIDBK);
            }
        }
        /// <summary>
        /// 书标打印
        /// </summary>
        private ICommand BookMark { get; set; }
        public ICommand Bookmark
        {
            get
            {
                return BookMark ?? (BookMark = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    if (controlClass == controlClass.bookmark)
                        return;
                    controlShow(controlClass.bookmark);
                    PrintSetControl printSetControl = new PrintSetControl(null);
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(printSetControl);
                }));
            }
        }
        /// <summary>
        /// 书标打印点击样式修改
        /// </summary>
        private BitmapImage BookMarkpic { get; set; }
        public BitmapImage BookmarkPIC
        {
            get { return BookMarkpic; }
            set
            {
                BookMarkpic = value;
                this.RaisePropertyChanged(() => BookmarkPIC);
            }
        }
        private BitmapImage BookMarkbk { get; set; }
        public BitmapImage BookmarkBK
        {
            get { return BookMarkbk; }
            set
            {
                BookMarkbk = value;
                this.RaisePropertyChanged(() => BookmarkBK);
            }
        }
        /// <summary>
        /// 条码打印
        /// </summary>
        private ICommand barcode { get; set; }
        public ICommand BarCode
        {
            get
            {
                return barcode ?? (barcode = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    if (controlClass == controlClass.barcode)
                        return;
                    controlShow(controlClass.barcode);
                    MarCodePrintControl marCodePrintControl = new MarCodePrintControl();
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(marCodePrintControl);
                }));
            }
        }
        /// <summary>
        /// 条码打印点击样式修改
        /// </summary>
        private BitmapImage barcodepic { get; set; }
        public BitmapImage BarCodePIC
        {
            get { return barcodepic; }
            set
            {
                barcodepic = value;
                this.RaisePropertyChanged(() => BarCodePIC);
            }
        }
        private BitmapImage barcodebk { get; set; }
        public BitmapImage BarCodeBK
        {
            get { return barcodebk; }
            set
            {
                barcodebk = value;
                this.RaisePropertyChanged(() => BarCodeBK);
            }
        }
        /// <summary>
        /// 图书查询
        /// </summary>
        private ICommand query { get; set; }
        public ICommand Query
        {
            get
            {
                return query ?? (query = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    if (controlClass == controlClass.query)
                        return;
                    controlShow(controlClass.query);
                    ServerSetting.info = null;
                    QueryControl queryControl = new QueryControl(mainControl);
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(queryControl);
                }));
            }
        }
        private ICommand centerCommand { get; set; }
        public ICommand CenterCommand
        {
            get
            {
                return centerCommand ?? (centerCommand = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    ShowPIC = "../images/三角形1.png";
                    Show = Visibility.Hidden;
                    if (controlClass == controlClass.user)
                        return;
                    controlShow(controlClass.user);
                    mainControl.Grid.Children.Clear();
                    PersonalCenterControl personalCenterControl = new PersonalCenterControl(mainControl);
                    mainControl.Grid.Children.Add(personalCenterControl);
                }));
            }
        }
        /// <summary>
        /// 图书查询点击样式修改
        /// </summary>
        private BitmapImage querypic { get; set; }
        public BitmapImage QueryPIC
        {
            get { return querypic; }
            set
            {
                querypic = value;
                this.RaisePropertyChanged(() => QueryPIC);
            }
        }
        private BitmapImage querybk { get; set; }
        public BitmapImage QueryBK
        {
            get { return querybk; }
            set
            {
                querybk = value;
                this.RaisePropertyChanged(() => QueryBK);
            }
        }
        /// <summary>
        /// 设置点击
        /// </summary>
        private ICommand set { get; set; }
        public ICommand Set
        {
            get
            {
                return set ?? (set = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    if (controlClass == controlClass.set)
                        return;
                    controlShow(controlClass.set);
                    ServerSetting.info = null;
                    PeriodicalControl bindingCorrectionControl = new PeriodicalControl(mainControl);
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(bindingCorrectionControl);
                }));
            }
        }
        /// <summary>
        /// 设置点击样式修改
        /// </summary>
        private BitmapImage setpic { get; set; }
        public BitmapImage SetPIC
        {
            get { return setpic; }
            set
            {
                setpic = value;
                this.RaisePropertyChanged(() => SetPIC);
            }
        }

        private BitmapImage setbk { get; set; }
        public BitmapImage SetBK
        {
            get { return setbk; }
            set
            {
                setbk = value;
                this.RaisePropertyChanged(() => SetBK);
            }
        }
        /// <summary>
        /// 刊期合订
        /// </summary>
        private ICommand quit { get; set; }
        public ICommand Quit
        {
            get
            {
                return quit ?? (quit = new DelegateCommand(() =>
                {
                    mainControl.thread.Abort();
                    if (controlClass == controlClass.periodical)
                        return;
                    controlShow(controlClass.periodical);
                    BIssueSubscription_Control loginControl = new BIssueSubscription_Control(mainControl,null);
                    mainControl.Grid.Children.Clear();
                    mainControl.Grid.Children.Add(loginControl);
                }));
            }
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        private ICommand closeCommond { get; set; }
        public ICommand CloseCommond
        {
            get
            {
                return closeCommond ?? (closeCommond=new DelegateCommand(()=> {
                    Load load = new Load();
                    DialogHelper.ShowDialog(load);
                    if (load.IsQuit)
                    {
                        mainControl.thread.Abort();
                        LoginControl loginControl = new LoginControl(mainControl.mainWindow);
                        mainControl.mainWindow.gridControl.Children.Clear();
                        mainControl.mainWindow.gridControl.Children.Add(loginControl);
                    }
                }));
            }
        }
        /// <summary>
        /// 退出点击样式修改
        /// </summary>
        private BitmapImage quitpic { get; set; }
        public BitmapImage QuitPIC
        {
            get { return quitpic; }
            set
            {
                quitpic = value;
                this.RaisePropertyChanged(() => QuitPIC);
            }
        }

        private BitmapImage quitbk { get; set; }
        public BitmapImage QuitBK
        {
            get { return quitbk; }
            set
            {
                quitbk = value;
                this.RaisePropertyChanged(() => QuitBK);
            }
        }

        #region RFID
      
        /// <summary>
        /// 三角路径
        /// </summary>
        private string ShowPic { get; set; }
        public string ShowPIC
        {
            get { return ShowPic; }
            set
            {
                ShowPic = value;
                this.RaisePropertyChanged(() => ShowPIC);
            }
        }
        /// <summary>
        /// 个人中心显示
        /// </summary>
        private Visibility show { get; set; }
        public Visibility Show
        {
            get { return show; }
            set
            {
                show = value;
                this.RaisePropertyChanged(() => Show);
            }
        }
        private ICommand djsj { get; set; }
        public ICommand DJSJ
        {
            get
            {
                return djsj ?? (djsj = new DelegateCommand(() =>
                {
                   // mainControl.thread.Abort();
                    if (ShowPIC.Contains("1"))
                    {
                        ShowPIC = "../images/三角形2.png";
                        Show = Visibility.Visible;
                    }
                    else
                    {
                        ShowPIC = "../images/三角形1.png";
                        Show = Visibility.Hidden;
                    }
                }));
            }
        }
        private string connState { get; set; }
        public string ConnState
        {
            get { return connState; }
            set
            {
                connState = value;
                this.RaisePropertyChanged(() => ConnState);
            }
        }
        #endregion
        #region 个人中心
        /// <summary>
        /// 用户名
        /// </summary>
        private string user { get; set; }
        public string User
        {
            get { return user; }
            set
            {
                user = value;
                this.RaisePropertyChanged(() => User);
            }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        private string email { get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                this.RaisePropertyChanged(() => Email);
            }
        }
        /// <summary>
        /// 电话
        /// </summary>
        private string phone { get; set; }
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                this.RaisePropertyChanged(() => Phone);
            }
        }
        /// <summary>
        /// 身份证
        /// </summary>
        private string IDcard { get; set; }
        public string IDCard
        {
            get { return IDcard; }
            set
            {
                IDcard = value;
                this.RaisePropertyChanged(() => IDCard);
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        private string passWord { get; set; }
        public string PassWord
        {
            get { return passWord; }
            set
            {
                passWord = value;
                this.RaisePropertyChanged(() => PassWord);
            }
        }
        /// <summary>
        /// 角色等级
        /// </summary>
        private string roleLevel { get; set; }
        public string RoleLevel
        {
            get { return roleLevel; }
            set
            {
                roleLevel = value;
                this.RaisePropertyChanged(() => RoleLevel);
            }
        }
        /// <summary>
        /// 编辑密码
        /// </summary>
        private ICommand editCommand { get; set; }
        public ICommand EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new DelegateCommand(() =>
                {
                    EditControl editControl = new EditControl(mainControl.mainWindow);
                    editControl.passWord = PassWord;
                    //editControl.ShowDialog();
                    DialogHelper.ShowDialog(editControl);
                }));
            }
        }
        private Visibility callNumberState { get; set; }
        public Visibility CallNumberState
        {
            get { return callNumberState; }
            set
            {
                callNumberState = value;
                this.RaisePropertyChanged(() => CallNumberState);
            }
        }
        #endregion
    }
    public enum controlClass
    {
        rfid = 0,
        bookmark = 1,
        barcode = 2,
        query = 3,
        set = 4,
        user = 5,
        periodical=6
    }
}
