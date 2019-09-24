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
using System.Windows.Shapes;

namespace Rfid系统.View
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.ColumnDefinitions[2].Width =new GridLength( grid.ColumnDefinitions[0].Width.ToInt() + grid.ColumnDefinitions[2].Width.ToInt());
            grid.ColumnDefinitions[0].Width = new GridLength(0);
        }
    }
}
