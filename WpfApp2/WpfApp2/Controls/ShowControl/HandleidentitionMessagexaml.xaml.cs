using Emgu.CV;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace WpfApp2.Controls.ShowControl
{
    /// <summary>
    /// HandleidentitionMessagexaml.xaml 的交互逻辑
    /// </summary>
    public partial class HandleidentitionMessagexaml : UserControl
    {
        public HandleidentitionMessagexaml(HandleidentitionPage handleidentitionPage,UserMessage userMessage)
        {
            InitializeComponent();
            DataContext = new ShowControlViewModel(handleidentitionPage);
            Task.Run(() => {
                this.Dispatcher.BeginInvoke((Action)delegate {
                    this.PIC.ImageSource = StringToBitmapImage(userMessage.PIC);
                    this.userName.Text = userMessage.Name;
                    this.IdCard.Text =   userMessage.IdentificationCode;
                    this.Phone.Content = userMessage.Phone;
                    this.ZCDate.Content= userMessage.ZCDate;
                    this.cardNum.Content=userMessage.UserCard;
                    this.Grde.Content = userMessage.Grade;
                    this.CardState.Content = userMessage.constate;
                    this.IsOverDue.Content = userMessage.overdueName;
                    
                });
            });
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

    }
}
