using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
using WpfApp2.BLL;
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;
using Action = System.Action;
using Image = System.Windows.Controls.Image;

namespace WpfApp2.Controls
{
    /// <summary>
    /// RenewListControl.xaml 的交互逻辑
    /// </summary>
    public partial class RenewListControl : UserControl
    {
       
        public RenewListControl(MainPage mainPage)
        {
            InitializeComponent();
           
            DataContext = new ShowControlViewModel(mainPage);
            Task.Run(() => {
         
            LoadPage = 1;
                this.Dispatcher.BeginInvoke((Action)delegate {
                this.GRID.CanUserAddRows = false;
                this.tet.Visibility = Visibility.Visible;
            this.txt.Visibility = Visibility.Hidden;
                datagrid.Visibility = Visibility.Hidden;
                });
                // GetDataList();
            page = mainPage;
          
          
            thread = new Thread(new ThreadStart(operation));
            thread.IsBackground = true;
            });
        }
       public Thread thread;
        public MainPage page;
        /// <summary>
        /// 查询数据源
        /// </summary>
        public List<message> DataList;
        /// <summary>
        /// 展示数据源
        /// </summary>
        public List<message> LoadList;
        /// <summary>
        /// 最大页数
        /// </summary>
        public int MaxPage;
        /// <summary>
        /// 当前页
        /// </summary>
        public int LoadPage;
        /// <summary>
        /// 跳转页
        /// </summary>
        public int NewPage;
        /// <summary>
        /// 人员信息
        /// </summary>
        public CardData user = new CardData();
        /// <summary>
        /// 查询数据源
        /// </summary>
        public int i = 60;
        public PrintShow PrintShow;

        delegate void operationDeletege();
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
                    i = 0;
                    thread.Abort();
                    if (PrintShow != null)
                    {
                        PrintShow.thread.Abort();
                        PrintShow.Close();
                    }
                    page.Grid.Children.Clear();
                    MainControl mainControl = new MainControl(page);
                    page.Grid.Children.Add(mainControl);
                    ServerSeting.ISAdd = true;
                    ServerSeting.SYSleepTimes = 60;
                }
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public void LoadPageData()
        {
            int i = 1;
            LoadList = new List<message>();
            if (DataList == null || DataList.Count == 0)
            {
                return;
            }
            if (NewPage <= MaxPage)
            {
                int SIndex = (NewPage - 1) * 9 + 1;
                int EIndex = NewPage * 9;

                foreach (message m in DataList)
                {
                    if (i >= SIndex && SIndex <= EIndex)
                    {
                        LoadList.Add(m);
                        SIndex++;
                    }
                    i++;
                }
                LoadPage = NewPage;
            }
            else
            {
                int SIndex = (MaxPage - 1) * 9 + 1;
                int EIndex = MaxPage * 9;
                foreach (message m in DataList)
                {
                    if (i >= SIndex && SIndex <= EIndex)
                    {
                        LoadList.Add(m);
                        SIndex++;
                    }
                    i++;
                }
                LoadPage = MaxPage;
                NewPage = MaxPage;
            }
            this.GRID.ItemsSource = LoadList;
        }
      
        /// <summary>
        /// 全选图标切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_MouseEnter(object sender, MouseEventArgs e)
        {
            txt.Clear();
            txt.Visibility = Visibility.Hidden;
            tet.Visibility = Visibility.Visible;
            tet.Focus();
        }
        /// <summary>
        /// 控件tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Label button = (Label)sender;
            switch (int.Parse(button.Tag.ToString()))
            {
                case 1:
                    this.Tag = 1;
                    break;
                case 2:
                    this.Tag = 2;
                    break;
                case 3:
                    this.Tag = 3;
                    break;
                default:
                    this.Tag = 4;
                    break;
            }
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            text.Focus();
            MR.Visibility = Visibility.Hidden;
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (text.Text != "")
            {
                MR.Visibility = Visibility.Hidden;
            }
            else
            {
                MR.Visibility = Visibility.Visible;
            }
            tet.Focus();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
           thread.Start();
        }
        private void Img_MouseEnter(object sender, MouseEventArgs e)
        {
            //BitmapImage imgSource = null;
            //Image button = (Image)sender;
            //switch (int.Parse(button.Tag.ToString()))
            //{
            //    case 0:
            //        imgSource = new BitmapImage(new Uri(@"..\ControlImages\灰色选择.png", UriKind.Relative));
            //        this.img.Source = imgSource;
            //        this.img.Tag = 0;
            //        this.GRID.Tag = 0;
            //        break;
            //    default:
            //        imgSource = new BitmapImage(new Uri(@"..\ControlImages\未选择.png", UriKind.Relative));
            //        this.img.Source = imgSource;
            //        this.img.Tag = 1;
            //        this.GRID.Tag = 1;
            //        break;
            //}
        }

        private void GRID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
