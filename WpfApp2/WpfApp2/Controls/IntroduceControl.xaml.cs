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
    /// IntroduceControl.xaml 的交互逻辑
    /// </summary>
    public partial class IntroduceControl : UserControl
    {
#pragma warning disable CS0108 // “IntroduceControl.Tag”隐藏继承的成员“FrameworkElement.Tag”。如果是有意隐藏，请使用关键字 new。
        private object Tag;
#pragma warning restore CS0108 // “IntroduceControl.Tag”隐藏继承的成员“FrameworkElement.Tag”。如果是有意隐藏，请使用关键字 new。
        public IntroduceControl(IntroducePage introducePage,object tag)
        {
            InitializeComponent();
            Tag = tag;
            DataContext = new ControlViewModel(introducePage,tag);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public void getData()
        {
        }
    }
}
