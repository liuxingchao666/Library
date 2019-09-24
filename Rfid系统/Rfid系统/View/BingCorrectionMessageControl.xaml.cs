using Rfidϵͳ.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Rfid系统.View
{
    /// <summary>
    /// BingCorrectionMessageControl.xaml 的交互逻辑
    /// </summary>
    public partial class BingCorrectionMessageControl : UserControl
    {

        public BingCorrectionMessageControl(MainControl mainControl,QueryInfo info)
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new BingCorrectionMessageViewModel(mainControl,info);
            this.info = info;
            ServerSetting.OldEPClist.Clear();
            ServerSetting.EPClist.Clear();
          //  if (ServerSetting.RfidConnState)
              //  ServerSetting.rfid.Start();
        }
        public QueryInfo info;
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Code.SelectAll();
            Code.Focus();
        }

        private void Marcode_KeyDown(object sender, KeyEventArgs e)
        {
            marcode.SelectAll();
            marcode.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ServerSetting.rfid.IsOpen())
                ServerSetting.rfid.Start();
            rfid.Clear();
            if (ServerSetting.EPClist.Count > 0)
                rfid.Text = ServerSetting.EPClist.Dequeue();
            rfid.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Code.Clear();
            Code.Focus();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            marcode.Clear();
            marcode.Focus();
        }

        private void Rfid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(rfid.Text))
                rfid.Text = info.RFID;
        }

        private void Code_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Code.Text))
                Code.Text = info.CorrectionCode;
        }

        private void Marcode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(marcode.Text))
                marcode.Text = info.CallNumber;
        }
    }
}
