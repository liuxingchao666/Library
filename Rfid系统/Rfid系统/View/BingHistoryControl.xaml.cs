using Rfid系统.DAL;
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
    /// BingHistoryControl.xaml 的交互逻辑
    /// </summary>
    public partial class BingHistoryControl : UserControl
    {
        public BingHistoryControl(MainControl mainControl)
        {
            InitializeComponent();
            this.mainControl = mainControl;
            ServerSetting.OldEPClist.Clear();
            DataContext = new BingHistoryViewModel(mainControl);
        }
        public MainControl mainControl;
        public int isDate = 0;
        private void Msg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            query.Focus();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combox.SelectedIndex == 3)
            {
                dateState.Visibility = Visibility.Visible;
                queryState.Visibility = Visibility.Hidden;
                msg.Visibility = Visibility.Hidden;
            }  else if(combox.SelectedIndex == 1)
            {
                dateState.Visibility = Visibility.Hidden;
                queryState.Visibility = Visibility.Visible;
                query.IsReadOnly = true;
                msg.Visibility = Visibility.Hidden;
               
            }
            else
            {
                dateState.Visibility = Visibility.Hidden;
                queryState.Visibility = Visibility.Visible;
                query.IsReadOnly = false;
                if (string.IsNullOrEmpty(query.Text))
                    msg.Visibility = Visibility.Visible;
                else
                    msg.Visibility = Visibility.Hidden;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            mainControl.thread.Abort();
            RFIDBindingControl control = new RFIDBindingControl(mainControl);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(control);
        }

        private void Query_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(query.Text))
            {
                msg.Visibility = Visibility.Hidden;
            }
            else
            {
                msg.Visibility = Visibility.Visible;
            }
        }
    }
}
