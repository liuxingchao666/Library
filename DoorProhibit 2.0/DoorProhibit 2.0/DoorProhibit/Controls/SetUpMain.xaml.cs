using DoorProhibit.ViewModel;
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

namespace DoorProhibit.Controls
{
    /// <summary>
    /// SetUpMain.xaml 的交互逻辑
    /// </summary>
    public partial class SetUpMain : UserControl
    {
        public MainWindow Main;
        public SetUpMain(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            DataContext = new SetUpViewModel(main);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetUp2 setUp2 = new SetUp2(Main);
            this.canvas.Children.Add(setUp2);
        }
    }
}
