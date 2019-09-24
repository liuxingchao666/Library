using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rfid系统.DAL;
using Rfid系统.Model;
using Rfid系统.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DataGrid = System.Windows.Controls.DataGrid;

namespace Rfid系统.ViewModel
{
    public class PNChooseViewModel : NotificationObject
    {
        public PNChooseViewModel(List<PNInfo> info, PNChooseControl control)
        {
            List = info;
            this.control = control;
        }
        public PNChooseControl control;
        /// <summary>
        /// 数据源
        /// </summary>
        private List<PNInfo> list { get; set; } = new List<PNInfo>();
        public List<PNInfo> List
        {
            get { return list; }
            set
            {
                list = value;
                this.RaisePropertyChanged(() => List);
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
                return closeCommond ?? (closeCommond = new DelegateCommand(() =>
                {
                    control.Close();
                }));
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        private ICommand addCommond { get; set; }
        public ICommand AddCommond
        {
            get
            {
                return addCommond ?? (addCommond = new DelegateCommand<DataGrid>((data) =>
                {

                    Dictionary<string, object> keyValues = new Dictionary<string, object>();
                    keyValues.Add("aNumber", string.IsNullOrEmpty(aNumber) ? "" : aNumber);
                    keyValues.Add("fkCataPeriodicalId", control.fkCataPeriodicalId);
                    try
                    {
                        keyValues.Add("publicationDateStr", Convert.ToDateTime(publicationDateStr).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        keyValues.Add("publicationDateStr",null);
                    }
                    keyValues.Add("page", page.ToInt());
                    keyValues.Add("price", price.ToInt());
                    keyValues.Add("remark", string.IsNullOrEmpty(remark) ? "" : remark);
                    keyValues.Add("sNumber", string.IsNullOrEmpty(sNumber) ? "" : sNumber);
                    if (string.IsNullOrEmpty(control.EdeitId))
                    {
                        object errorMsg = keyValues;
                        PNAddDAL pNAddDAL = new PNAddDAL();
                        if (pNAddDAL.PNAdd(ref errorMsg))
                        {
                            RetrunInfo info = errorMsg as RetrunInfo;
                            if (info.TrueOrFalse)
                            {
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                                        cfa.AppSettings.Settings["price"].Value = price;
                                        cfa.AppSettings.Settings["page"].Value = page;
                                        cfa.AppSettings.Settings["remark"].Value = remark;
                                        cfa.AppSettings.Settings["sNumber"].Value = sNumber;
                                        cfa.AppSettings.Settings["aNumber"].Value = aNumber;
                                        cfa.AppSettings.Settings["publicationDateStr"].Value = publicationDateStr;
                                        cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                                        ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                                    }
                                    catch { }
                                });
                                GetPNDAL getPN = new GetPNDAL();
                                errorMsg = control.fkCataPeriodicalId;
                                if (getPN.GetPN(ref errorMsg))
                                {
                                    info = errorMsg as RetrunInfo;
                                    if (info.TrueOrFalse)
                                    {
                                        List<PNInfo> infos = info.result as List<PNInfo>;
                                        data.ItemsSource = null;
                                        data.ItemsSource = infos;
                                    }
                                    else
                                    {
                                        if (ServerSetting.IsOverDue)
                                        {
                                            ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainWindow);
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
                                    control.Close();
                                    ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainWindow);
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
                        keyValues.Add("id", control.EdeitId);
                        object errorMsg = keyValues;
                        PNEditDAL pNEditDAL = new PNEditDAL();
                        if (pNEditDAL.PNEdit(ref errorMsg))
                        {
                            RetrunInfo info = errorMsg as RetrunInfo;
                            if (info.TrueOrFalse)
                            {
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                                        cfa.AppSettings.Settings["price"].Value = price;
                                        cfa.AppSettings.Settings["page"].Value = page;
                                        cfa.AppSettings.Settings["remark"].Value = remark;
                                        cfa.AppSettings.Settings["sNumber"].Value = sNumber;
                                        cfa.AppSettings.Settings["aNumber"].Value = aNumber;
                                        cfa.AppSettings.Settings["publicationDateStr"].Value = publicationDateStr;
                                        cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                                        ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                                    }
                                    catch { }
                                });
                                GetPNDAL getPN = new GetPNDAL();
                                errorMsg = control.fkCataPeriodicalId;
                                if (getPN.GetPN(ref errorMsg))
                                {

                                    info = errorMsg as RetrunInfo;
                                    if (info.TrueOrFalse)
                                    {

                                        List<PNInfo> infos = info.result as List<PNInfo>;
                                        data.ItemsSource = null;
                                        data.ItemsSource = infos;
                                    }
                                    else
                                    {
                                        if (ServerSetting.IsOverDue)
                                        {
                                            ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainWindow);
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
                                    control.Close();
                                    ErrorPage errorPage = new ErrorPage(info.result.ToString(), control.mainWindow);
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
                    control.EdeitId = null;
                }));
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        private ICommand deleteCommond { get; set; }
        public ICommand DeleteCommond
        {
            get
            {
                return deleteCommond ?? (deleteCommond = new DelegateCommand<DataGrid>((data) =>
                {

                }));
            }
        }
        /// <summary>
        /// 选中数据
        /// </summary>
        private ICommand okCommond { get; set; }
        public ICommand OKCommond
        {
            get
            {
                return okCommond ?? (okCommond = new DelegateCommand<DataGrid>((data) =>
                {
                    control.info = data.SelectedItem as PNInfo;
                    control.Close();
                }));
            }
        }
        /// <summary>
        /// 选中修改
        /// </summary>
        private ICommand click { get; set; }
        public ICommand Click
        {
            get
            {
                return click ?? (click = new DelegateCommand<DataGrid>((data) =>
                {
                    try
                    {
                        PNInfo info = data.SelectedItem as PNInfo;
                        sNumber = info.sNumber;
                        aNumber = info.aNumber;
                        remark = info.remark;
                        page = info.page;
                        price = info.price;
                        publicationDateStr = info.publicationDateStr;
                        control.EdeitId = info.fkCataPeriodicalId;
                    }
                    catch { }
                }));
            }
        }
        /// <summary>
        /// 复制上一次录入数据
        /// </summary>
        private ICommand copyCommond { get; set; }
        public ICommand CopyCommond
        {
            get
            {
                return copyCommond ?? (copyCommond = new DelegateCommand(() =>
                {
                    price = ConfigurationManager.AppSettings["price"];
                    remark = ConfigurationManager.AppSettings["remark"];
                    page = ConfigurationManager.AppSettings["page"];
                    sNumber = ConfigurationManager.AppSettings["sNumber"];
                    aNumber = ConfigurationManager.AppSettings["aNumber"];
                    publicationDateStr = ConfigurationManager.AppSettings["publicationDateStr"];
                }));
            }
        }
        /// <summary>
        /// 出版日期
        /// </summary>
        private string PublicationDateStr { get; set; }
        public string publicationDateStr
        {
            get { return PublicationDateStr; }
            set
            {
                PublicationDateStr = value;
                this.RaisePropertyChanged(() => publicationDateStr);
            }
        }
        /// <summary>
        /// 页数
        /// </summary>
        private string Page { get; set; }
        public string page
        {
            get { return Page; }
            set
            {
                Page = value.ToInt().ToString();
                this.RaisePropertyChanged(() => page);
            }
        }
        /// <summary>
        /// 定价
        /// </summary>
        private string Price { get; set; }
        public string price
        {
            get { return Price; }
            set
            {
                Price = value.ToInt().ToString();
                this.RaisePropertyChanged(() => price);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string Remark { get; set; }
        public string remark
        {
            get { return Remark; }
            set
            {
                Remark = value;
                this.RaisePropertyChanged(() => remark);
            }
        }
        /// <summary>
        /// 总期号
        /// </summary>
        private string snumber { get; set; }
        public string sNumber
        {
            get { return snumber; }
            set
            {
                snumber = value;
                this.RaisePropertyChanged(() => sNumber);
            }
        }
        /// <summary>
        /// 刊期号
        /// </summary>
        private string anumber { get; set; }
        public string aNumber
        {
            get { return anumber; }
            set
            {
                anumber = value;
                this.RaisePropertyChanged(() => aNumber);
            }
        }
    }
}
