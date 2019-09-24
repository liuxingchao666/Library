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
    /// TransactionSucess2ShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class TransactionSucess2ShowControl : UserControl
    {
        public TransactionSucess2ShowControl(TransactionSucessPage transactionSucessPage)
        {
            InitializeComponent();
            timer = new System.Threading.Thread(new ThreadStart(EditTimes));
            DataContext = new ShowControlViewModel(transactionSucessPage);
            this.page = transactionSucessPage;
        }
        public TransactionSucessPage page;
        System.Threading.Thread timer;
        public PrintParamtClass printParamt;
        delegate void UpdateTimer();
        public int Times;
        public void EditTimes()
        {
            while (true)
            {
                Times++;
                this.Dispatcher.BeginInvoke(new UpdateTimer(Edit));
                ServerSeting.ISAdd = false;
                Thread.Sleep(1000);
            }
        }

        public void Edit()
        {
            if (Times <= 59)
            {
                this.times.Content = (60 - Times).ToString() + "秒后将返回主页";
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => {
            if (ServerSeting.hairpin.serialPort.IsOpen)
            {
                byte[] send = new byte[6];
                send[0] = 0x02;
                send[1] = 0x45;
                send[2] = 0x53;
                send[3] = 0x03;
                send[4] = 0x17;
                send[5] = 0x05;
                ServerSeting.hairpin.serialPort.Write(send, 0, send.Length);
            }
        });
            timer.IsBackground = true;
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          //  timer.Abort();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            page.timer.Abort();
            page.Close();
            ServerSeting.ISAdd = false;
            UseM5 useM5 = new UseM5(PrintClass.Reissue, printParamt);
            object errorMsg = "";
            if (!useM5.ConnState(ref errorMsg))
            {
                errorPage errorPage = new errorPage(errorMsg.ToString());
                errorPage.ShowDialog();
                return;
            }
            UserMessage userMessage = new UserMessage() { IdentificationCode = page.cardData.CardNo };
            HandleidentitionPage handleidentitionPage = new HandleidentitionPage(page.mainPage);
             errorMsg = userMessage;
            GetNowUserDAL getNowUserDAL = new GetNowUserDAL();
            if (getNowUserDAL.GetPushDAL(ref errorMsg))
            {
                handleidentitionPage.ActionName = "个人信息";
                userMessage = errorMsg as UserMessage;
                userMessage.PIC = page.cardData.PIC;
                HandleidentitionMessagexaml messagexaml = new HandleidentitionMessagexaml(handleidentitionPage, userMessage);
                handleidentitionPage.GRID.Children.Add(messagexaml);
                page.mainPage.Grid.Children.Add(handleidentitionPage);
            }
           
        }

        private void TransactionSucessPage_LostFocus(object sender, RoutedEventArgs e)
        {
            timer.Abort();
        }
    }
}
