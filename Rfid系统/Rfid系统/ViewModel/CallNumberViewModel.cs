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
     public class CallNumberViewModel : NotificationObject
    {
        public CallNumberViewModel(CallNumberControl control)
        {
            this.Callcontrol = control;
            LoadCallNumber = "当前对应分类号是：" +control.info.fkTypeCode;
            LoadMCallNumber = "当前分类号“"+control.info.fkTypeCode+"”的最大种次顺序号如下所显示，请为其设置一个新的最大种次号，以便和分类号一起组成图书排架号：";
            CallNumber = control.info.OrderNum.ToInt().ToString();
            CallNumber1 = control.info.fkTypeCode + "/" + (control.info.OrderNum.ToInt()+0).ToString();
            CallNumber2 = control.info.fkTypeCode + "/" + (control.info.OrderNum.ToInt()+1).ToString();
            CallNumber3 = control.info.fkTypeCode + "/" + (control.info.OrderNum.ToInt()+2).ToString();
          
        }
        public CallNumberControl Callcontrol;
        /// <summary>
        /// 页眉
        /// </summary>
        private string loadCallNumber { get; set; }
        public string LoadCallNumber
        {
            get { return loadCallNumber; }
            set
            {
                loadCallNumber = value;
                this.RaisePropertyChanged(()=> LoadCallNumber);
            }
        }
        /// <summary>
        /// 提示
        /// </summary>
        private string loadMCallNumber { get; set; }
        public string LoadMCallNumber
        {
            get { return loadMCallNumber; }
            set
            {
                loadMCallNumber = value;
                this.RaisePropertyChanged(() => LoadMCallNumber);
            }
        }
        /// <summary>
        /// 输入值
        /// </summary>
        private string callNumber { get; set; }
        public string CallNumber
        {
            get { return callNumber; }
            set
            {
                callNumber = value;
                this.RaisePropertyChanged(()=> CallNumber);
            }
        }
        /// <summary>
        /// 输入值1
        /// </summary>
        private string callNumber1 { get; set; }
        public string CallNumber1
        {
            get { return callNumber1; }
            set
            {
                callNumber1 = value;
                this.RaisePropertyChanged(() => CallNumber1);
            }
        }
        /// <summary>
        /// 输入值2
        /// </summary>
        private string callNumber2 { get; set; }
        public string CallNumber2
        {
            get { return callNumber2; }
            set
            {
                callNumber2 = value;
                this.RaisePropertyChanged(() => CallNumber2);
            }
        }
        /// <summary>
        /// 输入值3
        /// </summary>
        private string callNumber3 { get; set; }
        public string CallNumber3
        {
            get { return callNumber3; }
            set
            {
                callNumber3 = value;
                this.RaisePropertyChanged(() => CallNumber3);
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        private ICommand closeCommond { get; set; }
        public ICommand CloseCommond
        {
            get
            {
                return closeCommond ?? (closeCommond=new DelegateCommand(()=> {
                    Callcontrol.Close();
                }));
            }
        }
        private ICommand okCommond { get; set; }
        public ICommand OkCommond
        {
            get
            {
                return okCommond ?? (okCommond=new DelegateCommand(()=> {
                    Callcontrol.info.SetBooks =CallNumber;
                    Callcontrol.info.OrderNum = CallNumber;
                    Callcontrol.Close();
                }));
            }
        }
    }
}
