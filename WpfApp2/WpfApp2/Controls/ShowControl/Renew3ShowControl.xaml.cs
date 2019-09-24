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
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// Renew3ShowPage.xaml 的交互逻辑
    /// </summary>
    public partial class Renew3ShowControl : UserControl
    {
        public Renew3ShowControl(Renew3Page renew3Page)
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(EditTimes));
            DataContext = new ShowControlViewModel(renew3Page);

        }
        /// <summary>
        /// 计时器
        /// </summary>
        System.Threading.Timer timer;
        delegate void UpdateTimer();
        /// <summary>
        /// 技数
        /// </summary>
        public int Times;
        public void EditTimes(object sender)
        {
            Times++;
            this.Dispatcher.BeginInvoke(new UpdateTimer(Edit));
        }

        public void Edit()
        {
            if (Times <= 30)
            {
                this.times.Content = (30 - Times).ToString();
            }
        }
        
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            timer.Change(0, 1000);
        }
    }
}
