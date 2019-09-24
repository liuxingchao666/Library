using Rfid系统.Model;
using Rfid系统.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// CallNumberControl.xaml 的交互逻辑
    /// </summary>
    public partial class CallNumberControl : Window
    {
        public CallNumberControl(ISBNbookListInfo info)
        {
            InitializeComponent();
            this.info = info;
            DataContext = null;
            DataContext = new CallNumberViewModel(this);
        }

        public ISBNbookListInfo info;

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (CallNumer.Text.Length == 0 || CallNumer.Text.Length == 1)
                {
                 //   CallNumer.Text = info.SetBooks;
                }
            }
        }

        private void CallNumer_TextChanged(object sender, TextChangedEventArgs e)
        {
            CallNumber3.Content = info.fkTypeCode + "/" + (CallNumer.Text.ToInt() + 2).ToString();
            CallNumber2.Content = info.fkTypeCode + "/" + (CallNumer.Text.ToInt() + 1).ToString();
            CallNumber1.Content = info.fkTypeCode + "/" + (CallNumer.Text.ToInt() + 0).ToString();
        }
    }
}
