using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rfid系统.ViewModel
{
    public class PeriodicalChooseViewModel : NotificationObject
    {
        public PeriodicalChooseViewModel(List<PeriodicalsInfo> info,PeriodicalChooseControl control)
        {
            this.control = control;
            List = info;
        }
        public PeriodicalChooseControl control;
        /// <summary>
        /// 关闭
        /// </summary>
        private ICommand closeCommond { get; set; }
        public ICommand CloseCommond
        {
            get
            {
                return closeCommond ?? (closeCommond=new DelegateCommand(()=> {
                    control.Close();
                }));
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        private ICommand okCommond { get; set; }
        public ICommand OKCommond
        {
            get
            {
                return okCommond ?? (okCommond = new DelegateCommand<DataGrid>((data) => {
                    control.info = data.SelectedItem as PeriodicalsInfo;
                    control.Close();
                }));
            }
        }
        /// <summary>
        /// 选中
        /// </summary>
        private ICommand click { get; set; }
        public ICommand Click
        {
            get
            {
                return click ?? (click = new DelegateCommand<DataGrid>((data) => {
                    PeriodicalsInfo periodicalsInfo = data.SelectedItem as PeriodicalsInfo;
                    control.info = periodicalsInfo;
                    control.Close();
                }));
            }
        }
        /// <summary>
        /// 数据原
        /// </summary>
        private List<PeriodicalsInfo> list { get; set; } = new List<PeriodicalsInfo>();
        public List<PeriodicalsInfo> List
        {
            get { return list; }
            set
            {
                list = value;
                this.RaisePropertyChanged(()=>List);
            }
        }
    }
}
