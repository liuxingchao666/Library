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
using WpfApp2.BLL;
using WpfApp2.DAL;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// Transaction2ShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class Transaction2ShowControl : UserControl
    {
        public Transaction2ShowControl(PayMentPage payMentPage)
        {
            ServerSeting.HostNeedMoney = 0;
            ServerSeting.HostMoney = 0;
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(payMentPage);
            Cost.Content = payMentPage.info.CostDeposit.ToString();
            ServerSeting.HostNeedMoney = payMentPage.info.CostDeposit;
           
            
                second.Visibility = Visibility.Visible;
                GB.Content = payMentPage.info.Deposit.toInt().ToString();
                YJ.Content = (payMentPage.info.CostDeposit.toInt() - payMentPage.info.Deposit.toInt()).ToString();
            
          
        }
        public  System.Threading.Thread timer;
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
                this.Cost.Content=ServerSeting.HostNeedMoney.ToString();
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
