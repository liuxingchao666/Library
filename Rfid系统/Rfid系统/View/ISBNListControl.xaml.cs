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
using System.Windows.Shapes;

namespace Rfid系统.View
{
    /// <summary>
    /// ISBNListControl.xaml 的交互逻辑
    /// </summary>
    public partial class ISBNListControl : Window
    {
        public ISBNListControl(string ISBN,RetrunInfo info)
        {
            InitializeComponent();
            this.ISBN = ISBN;
            DataContext = null;
            DataContext = new ISBNListViewModel(this,info);
            info = null;
        }
        public string ISBN;
        public ISBNbookListInfo info;
    }
}
