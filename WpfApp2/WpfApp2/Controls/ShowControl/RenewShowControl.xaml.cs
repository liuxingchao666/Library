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
using Action = WpfApp2.BLL.Action;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// RenewShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class RenewShowControl : Window
    {
        public MainPage page;
        public RenewShowControl(MainPage renewPage)
        {

            InitializeComponent();
            try
            {
                timer = new System.Threading.Thread(new ThreadStart(EditTimes));
                page = renewPage;
                ServerSeting.GetMessage.user = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public Thread thread;
        System.Threading.Thread timer;
        delegate void UpdateTimer();
        public int Times = 0;
        /// <summary>
        /// 读取的IC卡号
        /// </summary>
        public string ICCardNum { get; set; }
        public void EditTimes()
        {
            try
            {
                int index = 0;
                int i = 1;
                while (Times < 59)
                {
                   
                    Times++;
                    this.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        this.times.Content = (60 - Times).ToString() + "秒后返回主页";
                    });
                    Thread.Sleep(1000);
                }
                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
                this.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    this.Close();
                    timer.Abort();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            thread = new Thread(new ThreadStart(() =>
            {
                while (ServerSeting.GetMessage.user == null)
                {
                    lock (this)
                    {
                        ServerSeting.GetMessage.GO();
                    }
                    Thread.Sleep(50);
                }
                Task.Run(() => { 
                if (ServerSeting.GetMessage.user != null)
                {
                    CardData data = new CardData()
                    {
                        CardNo = ServerSeting.GetMessage.user.IdentificationCode
                    };
                    GetCardBumByIDCard getCardBumByIDCard = new GetCardBumByIDCard();
                    object errorMsg = data;
                   
                    if (getCardBumByIDCard.GetCardnum(ref errorMsg))
                    {
                        object error = errorMsg.ToString();
                        if (string.IsNullOrEmpty(errorMsg.ToString()))
                        {
                            Times = 60;
                            this.Dispatcher.BeginInvoke((System.Action)delegate
                            {
                                this.Close();
                                RenewFalse renewFalse = new RenewFalse("用户未办理读者卡");
                                renewFalse.ShowDialog();
                            });
                        }
                        else
                        {
                            UserCheckDAL checkDAL = new UserCheckDAL();
                            if (!checkDAL.UserCheck(ref error))
                            {
                                Times = 60;
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    this.Close();
                                });
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    RenewFalse renewFalse = new RenewFalse(error);
                                    renewFalse.ShowDialog();
                                });
                            }
                            else
                            {
                                Times = 60;
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    this.Close();
                                });
                                string CardNum = errorMsg.ToString();
                                data.cardNum = CardNum;
                                page.CardNum = CardNum;
                                this.Dispatcher.BeginInvoke((System.Action)delegate
                                {
                                    RenewListControl control1 = new RenewListControl(page);
                                    control1.user = data;
                                    page.Grid.Children.Clear();
                                    page.Grid.Children.Add(control1);
                                   // MessageBox.Show(DateTime.Now.Second - tt + "");
                                });
                                
                            }
                        }
                    }
                    else
                    {
                        ServerSeting.GetMessage.user = null;
                        Times = 60;
                        this.Dispatcher.BeginInvoke((System.Action)delegate
                        {
                            this.Close();
                        });
                        this.Dispatcher.BeginInvoke((System.Action)delegate
                        {
                           
                            RenewFalse renewFalse = new RenewFalse(errorMsg);
                            renewFalse.ShowDialog();

                        });
                    }
                }
                });
                thread.Abort();
            }));
            thread.IsBackground = true;
            thread.Start();
            try
            {
                timer.IsBackground = true;
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Card_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Times = 60;
                thread.Abort();
                ServerSeting.ISAdd = true;
                ServerSeting.SYSleepTimes = 60;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }
    }
}
