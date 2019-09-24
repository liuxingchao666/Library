using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp2.Controls;
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class ControlViewModel : NotificationObject
    {
        public IntroducePage page;
        public object Tag;
        public dealInfo info;
        #region classList
        /// <summary>
        /// 当前页
        /// </summary>
        public int currentPage = 1;
        /// <summary>
        /// 总条数
        /// </summary>
        public int total;
        /// <summary>
        /// 册  数据源
        /// </summary>
        public List<ArchivesInfo> classData;
        #endregion
        #region list
        /// <summary>
        /// 当前页
        /// </summary>
        public int currentPages = 1;
        /// <summary>
        /// 总条数
        /// </summary>
        public int totals;
        /// <summary>
        /// 书 数据源
        /// </summary>
        public List<ArchivesInfo> cData;
        #endregion
        public ControlViewModel(IntroducePage introducePage, object tag)
        {
            InputPage = "1";
            page = introducePage;
            Tag = tag;
            if (ServerSeting.connState)
            {
                dealInfo dealInfo = tag as dealInfo;
                info = dealInfo;
                PICstate = Visibility.Hidden;
                object errorMsg;
                if (dealInfo.Class == ListClass.classList)
                {
                    if (page.LoadClassPage > 0)
                    {
                        currentPage = page.LoadClassPage.ToInt();
                        InputPage = page.LoadClassPage.ToInt().ToString();
                    }
                    errorMsg = dealInfo.paramter;
                    if (GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total))
                    {
                        int count = total / 9;
                        if (total % 9 != 0)
                            count++;
                        CountPage = count;
                        message = errorMsg as List<ArchivesInfo>;
                        if (message.Count == 0)
                        {
                            GRIDState = Visibility.Hidden;
                            GRIDstate = Visibility.Hidden;
                            PICSJstate = Visibility.Visible;
                            PICSJState = Visibility.Visible;
                        }
                        else
                        {
                            GRIDState = Visibility.Visible;
                            GRIDstate = Visibility.Visible;
                            PICSJstate = Visibility.Hidden;
                            PICSJState = Visibility.Hidden;
                        }
                        page.infos = message;
                    }
                }
                else if (dealInfo.Class == ListClass.introduce)
                {
                    ArchivesInfo archivesInfo = dealInfo.paramter as ArchivesInfo;
                    errorMsg = archivesInfo.barcode;
                    if (GetBookIntroduceDal.getBookClass(ref errorMsg))
                    {
                        archivesInfo = errorMsg as ArchivesInfo;
                        if (string.IsNullOrEmpty(archivesInfo.PIC))
                        {
                            PIC = new BitmapImage(new Uri("../ControlImages/无图.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            try
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(archivesInfo.PIC);
                                WebResponse response = request.GetResponse();
                                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                                PIC = ImageToBitmapImage(img);
                            }
                            catch
                            {
                                PIC = new BitmapImage(new Uri("../ControlImages/无图.png", UriKind.RelativeOrAbsolute));
                            }
                        }
                        Author = archivesInfo.Author;
                        Press = archivesInfo.Press;
                        PressDate = archivesInfo.PressDate;
                        ISBN = archivesInfo.ISBN;
                        LayOut = archivesInfo.LayOut;
                        OpenBook = archivesInfo.openBook;
                        PageNumber = archivesInfo.PageNum;
                        Name = "《" + archivesInfo.ArchivesName + "》";
                        detailedmessage = archivesInfo.DetailedMessage;
                    }
                }
                else
                {
                    if (page.LoadPage > 0)
                    {
                        currentPage = page.LoadPage.ToInt();
                        InputPage = page.LoadPage.ToInt().ToString();
                    }
                    errorMsg = dealInfo.paramter;
                    if (GetBookMessage.getBook(ref errorMsg, ref currentPage, ref totals))
                    {
                        int count = total / 9;
                        if (total % 9 != 0)
                            count++;
                        CountPage = count;
                        Message = new List<ArchivesInfo>();
                        Message = errorMsg as List<ArchivesInfo>;
                        page.infos = Message;
                        if (Message.Count <= 0)
                        {
                            GRIDState = Visibility.Hidden;
                            GRIDstate = Visibility.Hidden;
                            PICSJstate = Visibility.Visible;
                            PICSJState = Visibility.Visible;
                        }
                        else
                        {
                            GRIDState = Visibility.Visible;
                            GRIDstate = Visibility.Visible;
                            PICSJstate = Visibility.Hidden;
                            PICSJState = Visibility.Hidden;
                        }
                    }
                }
            }
            else
            {
                PICstate = Visibility.Visible;
                GRIDstate = Visibility.Hidden;
                PICSJstate = Visibility.Hidden;
            }

        }
        public int maxPage;
        public int pageSize;

        private string inputPage { get; set; }
        public string InputPage
        {
            get { return string.IsNullOrEmpty(inputPage) ? "1" : inputPage; }
            set
            {
                inputPage = value;
                RaisePropertyChanged("InputPage");
            }
        }
        private int countPage { get; set; }
        public int CountPage
        {
            get { return countPage.ToInt(); }
            set
            {
                countPage = value;
                RaisePropertyChanged(()=> CountPage);
            }
        }

        private Visibility GRIDstate { get; set; }
        public Visibility GRIDState
        {
            get { return GRIDstate; }
            set
            {
                value = GRIDstate;
                this.RaisePropertyChanged(() => GRIDState);
            }
        }
        private Visibility PICstate { get; set; }
        public Visibility PICState
        {
            get { return PICstate; }
            set
            {
                value = PICstate;
                this.RaisePropertyChanged(() => PICState);
            }
        }

        private Visibility PICSJstate { get; set; }
        public Visibility PICSJState
        {
            get { return PICSJstate; }
            set
            {
                value = PICSJstate;
                this.RaisePropertyChanged(() => PICSJState);
            }
        }
        private ICommand PICClickcomand { get; set; }
        public ICommand PICClickComand
        {
            get
            {
                return PICClickcomand ?? (PICClickcomand = new DelegateCommand(getconn));
            }
        }
        public void getconn()
        {
            if (VerificationConn.GetVerification())
            {
                PICstate = Visibility.Hidden;
                GRIDstate = Visibility.Visible;
                if (string.IsNullOrEmpty(TestStr) || TestStr == "请输入查询所借的书籍名或作家")
                {
                    GetBookMessagesDAL messagesDAL = new GetBookMessagesDAL(null, 1, 9);

                    if (messagesDAL.GetMessages(ref Tag, ref maxPage, ref pageSize))
                    {
                        Message = Tag as List<ArchivesInfo>;
                        page.infos = Message;
                    }
                }
            }
            else
            {
                PICstate = Visibility.Visible;
                GRIDstate = Visibility.Hidden;
            }
        }
        private BitmapImage pic { get; set; }

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
        public BitmapImage PIC
        {
            get { return pic; }
            set
            {
                pic = value;
                this.RaisePropertyChanged(() => PIC);
            }
        }

        private string author { get; set; }
        public string Author
        {
            get { return author; }
            set
            {
                author ="作者 : "+ value;
                this.RaisePropertyChanged(() => Author);
            }
        }

        private string name { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged(() => Name);
            }
        }

        private string pagenumber { get; set; }
        public string PageNumber
        {
            get { return pagenumber; }
            set
            {
                pagenumber = "页码 : "+value;
                this.RaisePropertyChanged(() => PageNumber);
            }
        }

        private string press { get; set; }
        public string Press
        {
            get { return press; }
            set
            {
                press = "出版社 : "+value;
                this.RaisePropertyChanged(() => Press);
            }
        }
        private string isbn { get; set; }
        public string ISBN
        {
            get { return isbn; }
            set
            {
                isbn ="ISBN : "+ value;
                this.RaisePropertyChanged(() => ISBN);
            }
        }

        private string openBook { get; set; }
        public string OpenBook
        {
            get { return openBook; }
            set
            {
                openBook ="开本 : "+ value;
                this.RaisePropertyChanged(() => OpenBook);
            }
        }

        private string layOut { get; set; }
        public string LayOut
        {
            get { return layOut; }
            set
            {
                layOut ="类型 : "+value;
                this.RaisePropertyChanged(() => LayOut);
            }
        }


        private string pressDate { get; set; }
        public string PressDate
        {
            get { return pressDate; }
            set
            {
                pressDate ="出版日期 : "+ value;
                this.RaisePropertyChanged(() => PressDate);
            }
        }

        private string RemainingTime { get; set; }
        public string RemainingTimes
        {
            get { return RemainingTime; }
            set
            {
                RemainingTime = value;
                this.RaisePropertyChanged(() => RemainingTimes);
            }
        }

        private string Detailedmessage { get; set; }
        public string detailedmessage
        {
            get { return Detailedmessage; }
            set
            {
                Detailedmessage = value;
                this.RaisePropertyChanged(() => detailedmessage);
            }
        }

        public List<ArchivesInfo> messages = new List<ArchivesInfo>();
        public List<ArchivesInfo> message
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                this.RaisePropertyChanged(() => message);
            }
        }

        /// <summary>
        /// 第二程
        /// </summary>
        public List<ArchivesInfo> Messages = new List<ArchivesInfo>();
        public List<ArchivesInfo> Message
        {
            get
            {
                return Messages;
            }
            set
            {
                Messages = value;
                this.RaisePropertyChanged(() => Message);
            }
        }


        private ICommand _SelectAllCommand;
        public ICommand SelectAllCommand
        {
            get

            {
                return _SelectAllCommand ?? (_SelectAllCommand = new DelegateCommand<Grid>((data) =>
                {
                    Load(data);
                }));
            }
        }


        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return _BackCommand ?? (_BackCommand = new DelegateCommand<Grid>((data) =>
                {
                    page.BackAction();
                }));
            }
        }
        private ICommand checkComands;
        public ICommand checkComand
        {
            get
            {
                return checkComands ?? (checkComands = new DelegateCommand<DataGrid>((data) =>
                {
                    LoadPage(data);
                }));
            }
        }
        private ICommand _ClikCommand;
        public ICommand ClikCommand
        {
            get
            {
                return _ClikCommand ?? (_ClikCommand = new DelegateCommand<DataGrid>((data) =>
                {
                    UseCard();
                }));
            }
        }

        private ICommand _MouseClickCommand;
        public ICommand MouseClickCommand
        {
            get
            {
                return _MouseClickCommand ?? (_MouseClickCommand = new DelegateCommand<Label>((data) =>
                {
                    GetDataList(data);
                }));
            }
        }
        private ICommand firstCommand { get; set; }
        public ICommand FirstCommand
        {
            get
            {
                return firstCommand ?? (firstCommand = new DelegateCommand<DataGrid>((data) =>
                FistSetp(data)
                ));
            }
        }

        private ICommand lastCommand { get; set; }
        public ICommand LastCommand
        {
            get
            {
                return lastCommand ?? (lastCommand = new DelegateCommand<DataGrid>((data) =>
               LastSetp(data)
                ));
            }
        }

        private ICommand nextCommand { get; set; }
        public ICommand NextCommand
        {
            get
            {
                return nextCommand ?? (nextCommand = new DelegateCommand<DataGrid>((data) =>
               NextSetp(data)
                ));
            }
        }
        private string teststr { get; set; }
        public string TestStr
        {
            get { return string.IsNullOrEmpty(teststr) ? "请输入查询所借的书籍名或作家" : teststr; }
            set
            {
                teststr = value;
                RaisePropertyChanged("TestStr");
            }
        }
        public void Load(Grid data)
        {
            SelectListControl listControl = new SelectListControl(page, TestStr);
            page.datagrid.Children.Clear();
            page.datagrid.Children.Add(listControl);
            page.contentControl = GridClass.BookClassList;
        }

        public List<message> M;
        /// <summary>
        /// 查询详情--介绍
        /// </summary>
        /// <param name="data"></param>
        public void LoadPage(DataGrid data)
        {
            if (data == null || data.SelectedIndex < 0)
                return;
            ArchivesInfo m = data.SelectedItem as ArchivesInfo;
            if (!data.Name.Contains("ss"))
            {
                dealInfo dealInfo = new dealInfo() { paramter = m, Class = ListClass.introduce };
                page.bookInfos = Message;
                page.Totals = totals;
                page.LoadPage = InputPage.ToInt();
                page.TagBook = info;
                IntroduceControl introduceControl = new IntroduceControl(page, dealInfo);
                page.datagrid.Children.Add(introduceControl);
                page.contentControl = GridClass.Introduce;
                page.i = 60;
            }
            else
            {
                page.bookClassInfos = Message;
                page.LoadClassPage = InputPage.ToInt();
                page.Total = total;
                dealInfo dealInfo = new dealInfo() { paramter = m.Id, Class = ListClass.list };
                SelectListControl control = new SelectListControl(page, dealInfo);
                page.datagrid.Children.Add(control);
                page.contentControl = GridClass.BookList;
                page.i = 60;
            }
        }

        public static RenewPage renewPage;
        public static void close()
        {
            if (renewPage != null)
            {
                renewPage.Close();
            }
        }
        /// <summary>
        /// 办卡弹出框
        /// </summary>
        public void UseCard()
        {
           // renewPage = new RenewPage();
            //handleShowControl control = new handleShowControl(renewPage);
            //renewPage.GRID.Children.Add(control);
            //renewPage.ShowDialog();
        }

        public void GetDataList(Label button)
        {
            if (!ServerSeting.connState)
                return;
#pragma warning disable CS0219 // 变量“messagesDAL”已被赋值，但从未使用过它的值
            GetBookMessagesDAL messagesDAL = null;
#pragma warning restore CS0219 // 变量“messagesDAL”已被赋值，但从未使用过它的值
            object errorMsg = info.paramter;
            switch (button.Tag.ToInt())
            {
                #region 跳转
                //case 0:
                //     messagesDAL = new GetBookMessagesDAL(null, InputPage.ToInt(), 9);
                //    if (messagesDAL.GetMessages(ref Tag, ref maxPage, ref pageSize))
                //    {
                //        message = Tag as List<ArchivesInfo>;
                //        page.infos = message;
                //    }
                //    break;
                //case 4:
                //    if (total / 9 == 0)
                //    {
                //        return;
                //    }
                //    if (InputPage.ToInt() == currentPage)
                //    {
                //        return;
                //    }
                //    if ((InputPage.ToInt() - 1) * 9 <= 0)
                //    {
                //        currentPage = 1;
                //        GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                //        message = errorMsg as List<ArchivesInfo>;
                //        classData = message;
                //        InputPage = "1";
                //    }
                //    else if (total % 9 == 0)
                //    {
                //        if (InputPage.ToInt() >= total / 9)
                //        {
                //            currentPage = total / 9;
                //            GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                //            message = errorMsg as List<ArchivesInfo>;
                //            classData = message;
                //            InputPage = (total / 9).ToString();
                //        }
                //        else
                //        {
                //            currentPage = InputPage.ToInt();
                //            GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                //            message = errorMsg as List<ArchivesInfo>;
                //            classData = message;
                //        }
                //    }
                //    else
                //    {
                //        if (InputPage.ToInt() >= (total / 9) + 1)
                //        {
                //            currentPage = (total / 9) + 1;
                //            GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                //            message = errorMsg as List<ArchivesInfo>;
                //            classData = message;
                //            InputPage = ((total / 9) + 1).ToString();
                //        }
                //        else
                //        {
                //            currentPage = InputPage.ToInt();
                //            GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                //            message = errorMsg as List<ArchivesInfo>;
                //            classData = message;
                //        }
                //    } 

                //    break;
                #endregion
                case 1:
                    if (totals / 9 == 0)
                    {
                        return;
                    }
                    currentPages = 1;
                    if (GetBookMessage.getBook(ref errorMsg, ref currentPages, ref totals))
                    {
                        Message = null;
                        Message = errorMsg as List<ArchivesInfo>;
                        cData = Message;
                        InputPage = "1";
                        break;
                    }
                    else
                    {

                        break;
                    }
                case 5:
                    if (total / 9 == 0)
                    {
                        return;
                    }
                    currentPage = 1;
                    GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                    message = errorMsg as List<ArchivesInfo>;
                    classData = message;
                    InputPage = "1";
                    break;
                case 2:
                    if (totals / 9 == 0)
                    {
                        return;
                    }
                    if (InputPage.ToInt() == 1)
                    {
                        return;
                    }
                    int i = InputPage.ToInt() - 1;
                    GetBookMessage.getBook(ref errorMsg, ref i, ref totals);
                    Message = null;
                    Message = errorMsg as List<ArchivesInfo>;
                    cData = Message;
                    InputPage = (InputPage.ToInt() - 1).ToString();
                    break;
                case 6:
                    if (total / 9 == 0)
                    {
                        return;
                    }
                    if (InputPage.ToInt() == 1)
                    {
                        return;
                    }
                    i = InputPage.ToInt() - 1;
                    GetBookClassMessage.getBookClass(ref errorMsg, ref i, ref total);
                    message = errorMsg as List<ArchivesInfo>;
                    classData = message;
                    InputPage = (InputPage.ToInt() - 1).ToString();
                    break;
                case 3:

                    break;
                case 7:
                    if (total / 9 == 0)
                    {
                        return;
                    }
                    if (InputPage.ToInt() >= total / 9)
                    {
                        if (total % 9 == 0)
                        {
                            return;
                        }
                        else if (InputPage.ToInt() + 1 == (total / 9) + 1)
                        {
                            currentPage = (total / 9) + 1;
                            GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                            message = errorMsg as List<ArchivesInfo>;
                            classData = message;
                            InputPage = (InputPage.ToInt() + 1).ToString();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        currentPage = (InputPage.ToInt() + 1);
                        GetBookClassMessage.getBookClass(ref errorMsg, ref currentPage, ref total);
                        message = errorMsg as List<ArchivesInfo>;
                        classData = message;
                        InputPage = (InputPage.ToInt() + 1).ToString();
                    }
                    break;

            }

        }

        public void FistSetp(DataGrid dadta)
        {
            if (!ServerSeting.connState)
                return;
            object errorMsg = info.paramter;
            if (totals / 9 == 0)
            {
                return;
            }
            currentPages = 1;
            GetBookMessage.getBook(ref errorMsg, ref currentPages, ref totals);
            Message = null;
            Message = errorMsg as List<ArchivesInfo>;
            cData = Message;
            InputPage = "1";
            dadta.Dispatcher.BeginInvoke((Action)delegate ()
            {
                dadta.ItemsSource = Message;
            });
        }
        public void LastSetp(DataGrid dadta)
        {
            if (!ServerSeting.connState)
                return;
            object errorMsg = info.paramter;
            if (totals / 9 == 0)
            {
                return;
            }
            if (InputPage.ToInt() == 1)
            {
                return;
            }
            int i = InputPage.ToInt() - 1;
            GetBookMessage.getBook(ref errorMsg, ref i, ref totals);
            Message = null;
            Message = errorMsg as List<ArchivesInfo>;
            cData = Message;
            InputPage = (InputPage.ToInt() - 1).ToString();
            dadta.Dispatcher.BeginInvoke((Action)delegate ()
            {
                dadta.ItemsSource = Message;
            });

        }
        public void NextSetp(DataGrid dadta)
        {
            if (!ServerSeting.connState)
                return;
            object errorMsg = info.paramter;
            if (totals / 9 == 0)
            {
                return;
            }
            if (InputPage.ToInt() >= totals / 9)
            {
                if (totals % 9 == 0)
                {
                    return;
                }
                else if (InputPage.ToInt() + 1 == (totals / 9) + 1)
                {
                    currentPages = (totals / 9) + 1;
                    GetBookMessage.getBook(ref errorMsg, ref currentPages, ref totals);
                    Message = null;
                    Message = errorMsg as List<ArchivesInfo>;
                    cData = message;
                    InputPage = (InputPage.ToInt() + 1).ToString();
                    dadta.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        dadta.ItemsSource = Message;
                    });
                }
                else
                {
                    return;
                }
            }
            else
            {
                currentPages = (InputPage.ToInt() + 1);
                GetBookMessage.getBook(ref errorMsg, ref currentPages, ref totals);
                Message = null;
                Message = errorMsg as List<ArchivesInfo>;
                cData = message;
                InputPage = (InputPage.ToInt() + 1).ToString();
                dadta.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dadta.ItemsSource = Message;
                });
            }
        }
    }
}
