using Rfid系统.DAL;
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
    /// PNChooseControl.xaml 的交互逻辑
    /// </summary>
    public partial class PNChooseControl : Window
    {
        public PNChooseControl(List<PNInfo> info,MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            DataContext = new PNChooseViewModel(info,this);
        }
        public MainWindow mainWindow;
        public string fkCataPeriodicalId;
        public PNInfo info;
        public string EdeitId;

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            anumber.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ///删除
            PNInfo pNInfo = grid.SelectedItem as PNInfo;
            List<string> list = new List<string>() { pNInfo.fkCataPeriodicalId};
            DeletePNDAL pNDAL = new DeletePNDAL();
            object errorMsg = list;
            EdeitId = null;
            if (pNDAL.DeletePN(ref errorMsg))
            {
                RetrunInfo info = errorMsg as RetrunInfo;
                if (info.TrueOrFalse)
                {
                    GetPNDAL getPN = new GetPNDAL();
                    errorMsg = fkCataPeriodicalId;
                    if (getPN.GetPN(ref errorMsg))
                    {
                        info = errorMsg as RetrunInfo;
                        if (info.TrueOrFalse)
                        {
                            List<PNInfo> infos = info.result as List<PNInfo>;
                            grid.ItemsSource = null;
                            grid.ItemsSource = infos;
                        }
                        else
                        {
                            if (ServerSetting.IsOverDue)
                            {
                                ErrorPage errorPage = new ErrorPage(info.result.ToString(),mainWindow);
                                DialogHelper.ShowDialog(errorPage);
                            }
                            else
                            {
                                MessageBox.Show("失败提示：" + info.result);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(errorMsg.ToString());
                    }
                }
                else
                {
                    if (ServerSetting.IsOverDue)
                    {
                        this.Close();
                        ErrorPage errorPage = new ErrorPage(info.result.ToString(), mainWindow);
                        DialogHelper.ShowDialog(errorPage);
                    }
                    else
                    {
                        MessageBox.Show("失败提示：" + info.result);
                    }
                }
            }
            else
            {
                MessageBox.Show("失败提示："+errorMsg.ToString());
            }
        }
    }
}
