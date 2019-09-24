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
    /// SetControl.xaml 的交互逻辑
    /// </summary>
    public partial class SetControl : UserControl
    {
        public SetControl(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new SetViewModel(mainWindow);
        }
    }
}
