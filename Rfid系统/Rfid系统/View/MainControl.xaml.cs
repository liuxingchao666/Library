using Rfid系统.Model;
using Rfid系统.ViewModel;
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

namespace Rfid系统.View
{
    /// <summary>
    /// MainControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            DataContext = null;
            DataContext = new MainViewModel(this);
           
        }
        public MainWindow mainWindow;
        public ISBNbookListInfo info=new ISBNbookListInfo();
        public Thread thread;
        public RFIDBindingControl rFIDBindingControl;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rFIDBindingControl = new RFIDBindingControl(this);
            this.Grid.Children.Clear();
            this.Grid.Children.Add(rFIDBindingControl);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            user.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF344561"));
            user.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            user.Background= new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            user.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF344561")); 
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Quit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            Quit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF344561"));
        }

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Quit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            Quit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF344561"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
