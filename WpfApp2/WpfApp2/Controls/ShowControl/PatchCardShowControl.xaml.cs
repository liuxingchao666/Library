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
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// PatchCardShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class PatchCardShowControl : UserControl
    {
        public bool results;
        public PatchCardShowControl(RenewPage renewPage)
        {
            InitializeComponent();
            renew = renewPage;
            judge = true;
            ServerSeting.GetMessage.user = null;
            timer = new Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(renewPage);
        }
        public RenewPage renew;
        Thread timer;
        delegate void UpdateTimer();
        public int Times;
        public GetMessage getmessage;
        public CardData userMessage;
        public bool judge;
        public void EditTimes()
        {
            while (judge)
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
                this.times.Content = (60 - Times).ToString()+"秒后返回主页";
                if (ServerSeting.GetMessage.user == null)
                {
                    ServerSeting.GetMessage.GO();
                    if (ServerSeting.GetMessage.user != null && !string.IsNullOrEmpty(ServerSeting.GetMessage.user.Name))
                    {
                        judge = false;
                        timer.Abort();
                        CardData data = new CardData()
                        {
                            Name = ServerSeting.GetMessage.user.Name,
                            PIC = ServerSeting.GetMessage.user.PIC,
                            Sex = ServerSeting.GetMessage.user.Sex,
                            Nation = ServerSeting.GetMessage.user.NationName,
                            CardNo = ServerSeting.GetMessage.user.IdentificationCode,
                            Born = ServerSeting.GetMessage.user.BirdTh,
                            Address = ServerSeting.GetMessage.user.Address
                        };
                        ServerSeting.GetMessage.user = null;
                        renew.PatchCarduser = data;
                    }
                }
            }
            else
            {
                timer.Abort();
                renew.Close();
            }
        }
       
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Abort();
        }
    }
}
