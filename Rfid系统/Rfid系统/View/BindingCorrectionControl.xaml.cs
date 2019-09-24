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
    /// BindingCorrectionControl.xaml 的交互逻辑
    /// </summary>
    public partial class BindingCorrectionControl : UserControl
    {
        public BindingCorrectionControl(MainControl control)
        {
            InitializeComponent();
            DataContext = null;
            this.DataContext = new BingListViewModel(control);
        }
        private void Query_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            msg.Visibility = Visibility.Hidden;
        }

        private void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.V)
            {
                if (query.Text.Length <= 1)
                {
                    msg.Visibility = Visibility.Visible;
                }
                else
                {
                    msg.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Msg_MouseEnter(object sender, MouseEventArgs e)
        {
            query.Focus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            query.Focus();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Query_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(query.Text))
                msg.Visibility = Visibility.Hidden;
            else
                msg.Visibility = Visibility.Visible;
        }
        
        private void Msg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            query.Focus();
        }
    }
}
