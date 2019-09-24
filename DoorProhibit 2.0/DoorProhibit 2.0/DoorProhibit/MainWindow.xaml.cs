
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Windows;
using System;
using System.Net;
using System.IO;
using DoorProhibit.BLL;
using DoorProhibit.DAL;
using DoorProhibit.Model;
using System.Collections.Generic;
using DoorProhibit.ViewModel;
using DoorProhibit.Controls;
using System.Management;
using System.Threading;

namespace DoorProhibit
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          
            DataContext = new mainVeiwModel(this,null);
         
        }
        MainControl mainControl;
        /// <summary>
        /// 打开红外扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PublicData.PublicData.state = "未连接服务器";
            }
            catch (Exception ex)
            {
                MessageBox.Show("串口连接失败");
                PublicData.PublicData.state = "串口连接失败";
            }
            finally
            {
                MainControl mainControl = new MainControl(this);
                this.Grid.Children.Add(mainControl);
                this.mainControl = mainControl;
            }
        }
    }
}
