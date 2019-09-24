using System;
using System.Collections.Generic;
using System.IO;
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
using WpfApp2.BLL;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// ReportLossIdentitionShowControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportLossIdentitionShowControl : UserControl
    {
        public ReportLossIdentitionShowControl(ReportLossIdentitionPage reportLossIdentitionPage,string actionName)
        {
            InitializeComponent();
            DataContext = new ShowControlViewModel(reportLossIdentitionPage);
            LoadData(reportLossIdentitionPage.user);
            this.ActionName = actionName;
        }
      
        delegate void UpdateTimer();
        public string ActionName;
       

        public void LoadData(CardData data)
        {
            this.userName.Text = data.Name.Trim();
            this.IdCard.Text = data.CardNo.Trim();
            this.PIC.Source = StringToBitmapImage(data.PIC);
        }
        public BitmapImage StringToBitmapImage(string PIC)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            byte[] ImgByte = Convert.FromBase64String(PIC);
            bitmap.StreamSource = new MemoryStream(ImgByte);
            bitmap.EndInit();
            return bitmap;
        }

   

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActionName.Equals("0"))
            {
                state.Visibility = Visibility.Visible;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }

        private void ReportLossIdentitionPage_GotFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
