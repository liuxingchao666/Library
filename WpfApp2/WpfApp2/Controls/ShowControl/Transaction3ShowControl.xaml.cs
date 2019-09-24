using System;
using System.Collections.Generic;
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
using WpfApp2.DAL;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// Transaction3ShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class Transaction3ShowControl : UserControl
    {
        public Transaction3ShowControl(PayMentPage payMentPage)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(payMentPage);
            this.Cost.Content = payMentPage.info.CostDeposit.ToString();
            page = payMentPage;
        }
        private PayMentPage page;
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
            if (Times <= 60)
            {
                this.Cost.Content =ServerSeting.HostNeedMoney.ToString();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer.IsBackground = true;
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Abort();
        }
    }
}
