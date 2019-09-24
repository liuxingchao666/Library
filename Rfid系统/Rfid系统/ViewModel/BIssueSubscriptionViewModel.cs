using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rfid系统.ViewModel
{
    public class BIssueSubscriptionViewModel : NotificationObject
    {
        public BIssueSubscriptionViewModel(BIssueSubscription_Control control)
        {
            this.control = control;
        }
        private int selectIndex { get; set; }
        public int SelectIndex
        {
            get { return selectIndex; }
            set
            {
                selectIndex = value.ToInt();
                this.RaisePropertyChanged(() => SelectIndex);
            }
        }
        public BIssueSubscription_Control control;
        /// <summary>
        /// 管理
        /// </summary>
        private ICommand manageCommond { get; set; }
        public ICommand ManageCommond
        {
            get
            {
                return manageCommond ?? (manageCommond = new DelegateCommand(()=> {

                }));
            }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        private List<HDDCQKInfo> list { get; set; } = new List<HDDCQKInfo>();
        public List<HDDCQKInfo> List
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
