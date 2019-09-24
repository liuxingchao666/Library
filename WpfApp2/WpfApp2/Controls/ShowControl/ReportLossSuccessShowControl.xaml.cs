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
using WpfApp2.View;
using WpfApp2.ViewModel;
using System.Drawing;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// ReportLossSuccessShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportLossSuccessShowControl : UserControl
    {
        public ReportLossSuccessShowControl(ReportLossSuccessPage reportLossSuccessPage,DealClass dealClass)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(reportLossSuccessPage);
            if (dealClass == DealClass.indirect)
            {
                this.index.Content = "您的读书卡已挂失";
                this.PIC.Source = new BitmapImage(new Uri(@"..\..\ControlImages\已挂失.png", UriKind.Relative));
            }
        }
        System.Threading.Thread timer;
        delegate void UpdateTimer();
        public int Times;
        public void EditTimes()
        {
            while (true)
            {
                Times++;
                this.Dispatcher.BeginInvoke(new UpdateTimer(Edit));
                Thread.Sleep(1000);
            }
        }

        public void Edit()
        {
            if (Times <= 59)
            {
                this.times.Content = (60 - Times).ToString()+ "秒后将返回主页";
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer.IsBackground = true;
            timer.Start();
        }
    }
    public enum DealClass
    {
        Direct=0,
        indirect=1
    }
}
