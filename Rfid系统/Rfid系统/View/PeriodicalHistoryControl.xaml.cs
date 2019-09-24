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
    /// PeriodicalHistoryControl.xaml 的交互逻辑
    /// </summary>
    public partial class PeriodicalHistoryControl : UserControl
    {
        public PeriodicalHistoryControl(MainControl mainControl)
        {
            InitializeComponent();
            this.mainControl = mainControl;
            DataContext = new PeriodicalHistoryViewModel(mainControl);
        }
        public MainControl mainControl;
        private void Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combox.SelectedIndex == 3)
            {
                dateState.Visibility = Visibility.Visible;
                queryState.Visibility = Visibility.Hidden;
                msg.Visibility = Visibility.Hidden;
            }
            else if (combox.SelectedIndex == 1)
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

        private void Msg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            query.Focus();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            mainControl.thread.Abort();
            PeriodicalControl periodicalControl = new PeriodicalControl(mainControl);
            mainControl.Grid.Children.Clear();
            mainControl.Grid.Children.Add(periodicalControl);
        }
    }
}
