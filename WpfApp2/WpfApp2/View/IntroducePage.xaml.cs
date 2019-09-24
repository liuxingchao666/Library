using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.BLL;
using WpfApp2.Controls;
using WpfApp2.ViewModel;
using Newtonsoft.Json.Linq;
using WpfApp2.Model;
using System.Threading;
using WpfApp2.DAL;

namespace WpfApp2.View
{
    /// <summary>
    /// IntroducePage.xaml 的交互逻辑
    /// </summary>
    public partial class IntroducePage : UserControl
    {
        public MainPage mainPage;
#pragma warning disable CS0108 // “IntroducePage.Tag”隐藏继承的成员“FrameworkElement.Tag”。如果是有意隐藏，请使用关键字 new。
        public object Tag;
#pragma warning restore CS0108 // “IntroducePage.Tag”隐藏继承的成员“FrameworkElement.Tag”。如果是有意隐藏，请使用关键字 new。
        public object TagBook;
        private Thread thread;
        public int i = 60;
        delegate void operationDeletege();
        #region BookList
        /// <summary>
        /// 操作数据源
        /// </summary>
        public List<ArchivesInfo> bookInfos;
        /// <summary>
        /// 操作页
        /// </summary>
        public int LoadPage;
        /// <summary>
        /// 查询书本总书
        /// </summary>
        public int Total;
        #endregion
        #region BookClassList 
        /// <summary>
        /// 操作数据源
        /// </summary>
        public List<ArchivesInfo> bookClassInfos;
        /// <summary>
        /// 操作页
        /// </summary>
        public int LoadClassPage;
        /// <summary>
        /// 查询册总数
        /// </summary>
        public int Totals;
        #endregion
        public IntroducePage(MainPage MainPage, object tag, GridClass gridClass)
        {
            InitializeComponent();
            mainPage = MainPage;
            contentControl = gridClass;
            Tag = tag;
            DataContext = new ControlViewModel(this, tag);
            JudgeId();
        }
        public void operation()
        {
            while (i > 0)
            {
                this.Dispatcher.BeginInvoke(new operationDeletege(Operation));
                Thread.Sleep(1000);
            }
        }

        public void Operation()
        {
            lock (thread)
            {
                if (i > 1)
                {
                    i--;
                    this.RemainingTime.Content = "操作时间: " + i + "s";
                }
                else
                {
                    thread.Abort();
                    operationDeletege eventHandler = new operationDeletege(ControlViewModel.close);
                    eventHandler();
                    mainPage.Grid.Children.Clear();

                    MainControl mainControl = new MainControl(mainPage);
                    mainPage.Grid.Children.Add(mainControl);
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;

                }
            }
        }
        /// <summary>
        /// 是否打开书籍介绍页
        /// </summary>
        public void JudgeId()
        {
            if (contentControl == GridClass.Introduce)
            {

                IntroduceControl control = new IntroduceControl(this, Tag);
                contentControl = GridClass.Introduce;
                this.datagrid.Children.Clear();
                this.datagrid.Children.Add(control);
            }
            else
            {
                SelectClassListControl1 control = new SelectClassListControl1(this, Tag);
                contentControl = GridClass.BookClassList;
                this.datagrid.Children.Clear();
                this.datagrid.Children.Add(control);
            }
        }

        public List<ArchivesInfo> infos = new List<ArchivesInfo>();
        public string paramter;
        /// <summary>
        /// 控件展示类型
        /// </summary>
        public GridClass contentControl = GridClass.Introduce;
        /// <summary>
        /// 点击返回执行的事件
        /// </summary>
        public void BackAction()
        {
            if (contentControl == GridClass.BookClassList)
            {
                thread.Abort();
                mainPage.QueryControl = new QueryControl(mainPage);
                mainPage.Grid.Children.Clear();
                mainPage.Grid.Children.Add(mainPage.QueryControl);
            }
            else if (contentControl == GridClass.Introduce)
            {
                SelectListControl selectListControl = new SelectListControl(this, TagBook);
                contentControl = GridClass.BookList;
                this.datagrid.Children.Clear();
                this.datagrid.Children.Add(selectListControl);
                i = 60;
            }
            else
            {
                SelectClassListControl1 selectClassListControl1 = new SelectClassListControl1(this, Tag);
                contentControl = GridClass.BookClassList;
                this.datagrid.Children.Clear();
                this.datagrid.Children.Add(selectClassListControl1);
                i = 60;
            }
        }

        public List<ArchivesInfo> archivesInfos = new List<ArchivesInfo>();

        private void Text_MouseEnter(object sender, MouseEventArgs e)
        {
            if (text.Text == "请输入查询所借的书籍名或作家")
            {
                text.Clear();
                text.Focus();
            }
        }

        private void Text_MouseLeave(object sender, MouseEventArgs e)
        {
            if (text.Text == "")
            {
                text.Text = "请输入查询所借的书籍名或作家";
            }
            YC.Focus();
        }

        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            i = 60;
            if (thread != null)
            {
                thread.Abort();
            }
            thread = new Thread(new ThreadStart(operation));
            thread.IsBackground = true;
            thread.Start();
        }
    }
    /// <summary>
    /// dataGrid 控件
    /// </summary>
    public enum GridClass
    {
        BookList = 0,
        Introduce = 1,
        BookClassList = 2
    }
    public class ValueInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
