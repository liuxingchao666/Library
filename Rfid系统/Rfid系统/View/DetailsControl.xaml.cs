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
    /// DetailsControl.xaml 的交互逻辑
    /// </summary>
    public partial class DetailsControl : UserControl
    {
        public DetailsControl(MainControl control,QueryInfo info)
        {
            InitializeComponent();
            DataContext = null;
            this.DataContext = new DetailsViewModel(control,info);
            this.info = info;
        }
        public QueryInfo info;
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
            callNumber.Clear();
            callNumber.Focus();
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

        private void CallNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(callNumber.Text))
                callNumber.Text = info.CallNumber;
        }
    }
}
