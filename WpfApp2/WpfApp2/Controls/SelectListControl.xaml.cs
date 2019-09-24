using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
    /// SelectListControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectListControl : UserControl
    {
        public SelectListControl(IntroducePage page,object Tag)
        {
            InitializeComponent();
            DataContext = new ControlViewModel(page, Tag);
            this.datagrid.CanUserAddRows = false;
            this.datagrid.ItemsSource = page.infos;
        }
   
    }
    public class message
    {
        public string author { get; set; }
        public string num { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string WZ { get; set; }
        public string isOpen { get; set; }
        public string isClose { get; set; }
        public string action { get; set; }
        public bool Ischeck { get; set; }
        public DateTime? stratDate { get; set; }
        public DateTime? endDate { get; set; }
        public string source { get; set; }
    }
}
