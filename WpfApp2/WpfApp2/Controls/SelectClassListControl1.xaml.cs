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
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls
{
    /// <summary>
    /// SelectClassListControl1.xaml 的交互逻辑
    /// </summary>
    public partial class SelectClassListControl1 : UserControl
    {
        public SelectClassListControl1(IntroducePage page, object Tag)
        {
            InitializeComponent();
            DataContext = new ControlViewModel(page, Tag);
        }
    }
}
