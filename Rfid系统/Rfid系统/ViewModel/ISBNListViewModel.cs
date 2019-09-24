using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rfid系统.ViewModel
{
    public class ISBNListViewModel : NotificationObject
    {
        public ISBNListViewModel(ISBNListControl control,RetrunInfo info)
        {
            this.control = control;
            PICState = Visibility.Hidden;

            if (info.TrueOrFalse)
                List = info.result as List<ISBNbookListInfo>;
            else
            {
                control.grid.Visibility = Visibility.Hidden;
                PICState = Visibility.Visible;
                if (string.IsNullOrEmpty(info.ResultCode))
                    ErrorPIC = new BitmapImage(new Uri("../images/未连接.png", UriKind.RelativeOrAbsolute));
                else if (info.ResultCode.Equals("201"))
                    ErrorPIC = new BitmapImage(new Uri("../images/无数据.jpg", UriKind.RelativeOrAbsolute));
            }
        }
        public ISBNListControl control;

        private BitmapImage errorPIC { get; set; }
        public BitmapImage ErrorPIC
        {
            get { return errorPIC; }
            set
            {
                errorPIC = value;
                this.RaisePropertyChanged(() => ErrorPIC);
            }
        }
        private Visibility PICstate { get; set; }
        public Visibility PICState
        {
            get { return PICstate; }
            set
            {
                PICstate = value;
                this.RaisePropertyChanged(() => PICState);
            }
        }
        /// <summary>
        /// 数据集
        /// </summary>
        private List<ISBNbookListInfo> list { get; set; } = new List<ISBNbookListInfo>();
        public List<ISBNbookListInfo> List
        {
            get { return list; }
            set
            {
                list = value;
                this.RaisePropertyChanged(() => List);
            }
        }
        /// <summary>
        ///选中
        /// </summary>
        private ICommand click { get; set; }
        public ICommand Click
        {
            get
            {
                return click ?? (click = new DelegateCommand<DataGrid>((data) =>
                {
                    ISBNbookListInfo info = data.SelectedItem as ISBNbookListInfo;
                    control.info = info;
                    control.Close();
                }));
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        private ICommand back { get; set; }
        public ICommand Back
        {
            get
            {
                return back ?? (back = new DelegateCommand(() => { control.Close(); }));
            }
        }
    }
}
