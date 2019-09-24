using DoorProhibit.BLL;
using DoorProhibit.Controls;
using DoorProhibit.DAL;
using DoorProhibit.Model;
using DoorProhibit.PublicData;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;


namespace DoorProhibit.ViewModel
{
    public class SetUpViewModel : NotificationObject
    {
        public MainWindow main;
        public SetUpViewModel(MainWindow mainWindow)
        {
            main = mainWindow;
            GetFloorDAL getFloorDAL = new GetFloorDAL();
            Task.Run(() =>
            {
                if (getFloorDAL.GetFloor(out object errorMsg))
                {
                    ReturnInfo returnInfo = errorMsg as ReturnInfo;
                    List<floorInfo> floors = returnInfo.result as List<floorInfo>;
                    kfitems = floors;
                    foreach (var temp in floors)
                    {
                        if (temp.IsCheck)
                            skfItems = temp;
                    }
                }
            });
            show(0);
        }
        public string serverip;
        public string serversite;

        public void show(int index)
        {
            switch (index)
            {
                case 0:
                    setColor = "#00BEF6";
                    setColor1 = "#0CE59B";
                    passColor = "#EAEAEA";
                    passColor1 = "#EAEAEA";
                    SetVisibile = Visibility.Visible;
                    PassVisibile = Visibility.Hidden;
                    successVisibile = Visibility.Hidden;
                    break;
                case 1:
                    passColor = "#00BEF6";
                    passColor1 = "#0CE59B";
                    setColor = "#EAEAEA";
                    setColor1 = "#EAEAEA";
                    PassVisibile = Visibility.Visible;
                    SetVisibile = Visibility.Hidden;
                    successVisibile = Visibility.Hidden;
                    break;
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        private ICommand BackCommond { get; set; }
        public ICommand backCommond
        {
            get
            {
                return BackCommond ?? (BackCommond = new DelegateCommand(back));
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        private ICommand SignOutCommond { get; set; }
        public ICommand signOutCommond
        {
            get
            {
                return SignOutCommond ?? (SignOutCommond = new DelegateCommand(signOut));
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        private ICommand OKCommond { get; set; }
        public ICommand okCommond
        {
            get
            {
                return OKCommond ?? (OKCommond = new DelegateCommand(() =>
                {
                    try
                    {
                        Task.Run(() =>
                        {
                            if (skfItems != null && !string.IsNullOrEmpty(skfItems.id))
                            {
                                if (!string.IsNullOrEmpty(serverip))
                                {
                                    PublicData.PublicData.serverIp = serverip;
                                    PublicData.PublicData.serverSite = serversite;
                                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件
                                    cfa.AppSettings.Settings["ServerIP"].Value = serverip;//修改配置文件中的某一个项的value
                                    cfa.AppSettings.Settings["ServerPort"].Value =serversite;
                                    cfa.Save(ConfigurationSaveMode.Modified);  //保存配置文件
                                    ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件
                                }
                                string HDid = "";
                                ManagementClass mc = new ManagementClass("Win32_Processor");
                                ManagementObjectCollection moc = mc.GetInstances();
                                foreach (ManagementObject mo in moc)
                                {
                                    HDid = (string)mo.Properties["ProcessorId"].Value;
                                    break;
                                }
                                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                                keyValues.Add("equipmentCode", HDid);
                                keyValues.Add("equipmentName", Dns.GetHostName());
                                keyValues.Add("fkStoreId", skfItems.id);
                                object errorMsg = keyValues;
                                floorEditDAL floorEditDAL = new floorEditDAL();
                                floorEditDAL.floorEdit(ref errorMsg);
                            }
                        });
                    }
                    catch { }
                    finally
                    {
                        main.Grid.Children.Clear();
                        MainControl mainControl = new MainControl(main);
                        main.Grid.Children.Add(mainControl);
                    }
                }));
            }
        }
        #region
        /// <summary>
        /// 设置
        /// </summary>
        private ICommand SetCommond { get; set; }
        public ICommand setCommond
        {
            get
            {
                return SetCommond ?? (SetCommond = new DelegateCommand(() =>
                {
                    show(0);
                }));
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        private string SetColor { get; set; }
        public string setColor
        {
            get { return SetColor; }
            set
            {
                SetColor = value;
                this.RaisePropertyChanged(() => setColor);
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        private string SetColor1 { get; set; }
        public string setColor1
        {
            get { return SetColor1; }
            set
            {
                SetColor1 = value;
                this.RaisePropertyChanged(() => setColor1);
            }
        }
        private ICommand PassWordCommond { get; set; }
        public ICommand passWordCommond
        {
            get
            {
                return PassWordCommond ?? (PassWordCommond = new DelegateCommand(() =>
                {
                    show(1);
                }));
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        private string PassColor { get; set; }
        public string passColor
        {
            get { return PassColor; }
            set
            {
                PassColor = value;
                this.RaisePropertyChanged(() => passColor);
            }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        private string PassColor1 { get; set; }
        public string passColor1
        {
            get { return PassColor1; }
            set
            {
                PassColor1 = value;
                this.RaisePropertyChanged(() => passColor1);
            }
        }
        private string connPIC { get; set; }
        public string ConnPIC
        {
            get { return connPIC; }
            set
            {
                connPIC = value;
                this.RaisePropertyChanged(() => ConnPIC);
            }
        }
        private string connError { get; set; }
        public string ConnError
        {
            get { return connError; }
            set
            {
                connError = value;
                this.RaisePropertyChanged(() => ConnError);
            }
        }
        private string connColor { get; set; }
        public string ConnColor
        {
            get { return connColor; }
            set
            {
                connColor = value;
                this.RaisePropertyChanged(() => ConnColor);
            }
        }
        private ICommand TryCommond { get; set; }
        public ICommand tryCommond
        {
            get
            {
                return TryCommond ?? (TryCommond = new DelegateCommand(() =>
                {
                    if (string.IsNullOrEmpty(ServerIP))
                    {
                        ConnColor = "#FF2E00";
                        ConnPIC = "../images/测试连接失败.png";
                        ConnError = "IP地址不能为空！！";
                        return;
                    }
                    if (string.IsNullOrEmpty(ServerSite))
                    {
                        ConnColor = "#FF2E00";
                        ConnPIC = "../images/测试连接失败.png";
                        ConnError = "端口不能为空！！";
                        return;
                    }
                    Regex regex = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
                    if (!regex.IsMatch(ServerIP))
                    {
                        ConnColor = "#FF2E00";
                        ConnPIC = "../images/测试连接失败.png";
                        ConnError = "ip地址格式不对！！";
                        return;
                    }
                    if (ServerSite.Length != 4)
                    {
                        ConnColor = "#FF2E00";
                        ConnPIC = "../images/测试连接失败.png";
                        ConnError = "端口格式不对！！";
                        return;
                    }
                    try
                    {
                        string url = string.Format("http://{0}:{1}/entranceGuard/log/currency/check", ServerIP, ServerSite);
                        Http http = new Http(url, null);
                        object jsonResult = http.HttpGet(url);
                        var result = JObject.Parse(jsonResult.ToString());
                        if (result["state"].ToString().ToLower().Equals("true"))
                        {
                            serverip = ServerIP;
                            serversite = ServerSite;
                            ConnColor = "#07D6BF";
                            ConnPIC = "../images/测试连接成功.png";
                            ConnError = "测试成功";
                            string HDid = "";
                            ManagementClass mc = new ManagementClass("Win32_Processor");
                            ManagementObjectCollection moc = mc.GetInstances();
                            foreach (ManagementObject mo in moc)
                            {
                                HDid = (string)mo.Properties["ProcessorId"].Value;
                                break;
                            }
                            url = string.Format("http://{0}:{1}/entranceGuard/log/currency/storeselect?equipmentName=" + Dns.GetHostName() + "&&equipmentCode=" + HDid, ServerIP, ServerSite);
                            http = new Http(url, null);
                            var results = JToken.Parse(http.HttpGet(url));
                            List<floorInfo> infos = new List<floorInfo>();
                            foreach (var temp in results["row"].Children())
                            {
                                floorInfo info = new floorInfo()
                                {
                                    id = temp["id"].ToString(),
                                    Name = temp["name"].ToString(),
                                    IsCheck = (Boolean)temp["check"]
                                };
                                infos.Add(info);
                            }
                            kfitems = infos;
                            foreach (var temp in infos)
                            {
                                if (temp.IsCheck)
                                    skfItems = temp;
                            }
                        }
                        else
                        {
                            ConnColor = "#FF2E00";
                            ConnPIC = "../images/测试连接失败.png";
                            ConnError = "测试连接失败！！";
                        }
                    }
                    catch
                    {
                        ConnColor = "#FF2E00";
                        ConnPIC = "../images/测试连接失败.png";
                        ConnError = "测试连接失败！！";
                    }
                }));
            }
        }

        private Visibility passVisibile { get; set; }
        public Visibility PassVisibile
        {
            get { return passVisibile; }
            set
            {
                passVisibile = value;
                this.RaisePropertyChanged(() => PassVisibile);
            }
        }
        private Visibility SuccessVisibile { get; set; }
        public Visibility successVisibile
        {
            get { return SuccessVisibile; }
            set
            {
                SuccessVisibile = value;
                this.RaisePropertyChanged(() => successVisibile);
            }
        }
        private Visibility setVisibile { get; set; }
        public Visibility SetVisibile
        {
            get { return setVisibile; }
            set
            {
                setVisibile = value;
                this.RaisePropertyChanged(() => SetVisibile);
            }
        }
        #endregion
        /// <summary>
        /// 取消
        /// </summary>
        private ICommand CancelCommond { get; set; }
        public ICommand cancelCommond
        {
            get
            {
                return CancelCommond ?? (CancelCommond = new DelegateCommand(cancel));
            }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        private string SBItems { get; set; }
        public string SBitems
        {
            get { return string.IsNullOrEmpty(SBItems) == true ? PublicData.PublicData.EquipmentNume : SBItems; }
            set
            {
                SBItems = value;
                this.RaisePropertyChanged(() => SBitems);
            }
        }
        /// <summary>
        /// 库房数据源
        /// </summary>
        private List<floorInfo> KFItems { get; set; }
        public List<floorInfo> kfitems
        {
            get { return KFItems; }
            set
            {
                KFItems = value;
                this.RaisePropertyChanged(() => kfitems);
            }
        }
        private floorInfo SKFItems { get; set; }
        public floorInfo skfItems
        {
            get { return SKFItems; }
            set
            {
                SKFItems = value;
                this.RaisePropertyChanged(() => skfItems);
            }
        }
        /// <summary>
        /// 本地ip
        /// </summary>
        private string LoadIP { get; set; }
        public string loadIP
        {
            get { return GetLoadIp(LoadIP); }
            set
            {
                LoadIP = GetLoadIp(value);
                this.RaisePropertyChanged(() => loadIP);
            }
        }
        /// <summary>
        /// 服务器IP
        /// </summary>
        private string serverIP { get; set; }
        public string ServerIP
        {
            get { return string.IsNullOrEmpty(serverIP) == true ? PublicData.PublicData.serverIp : serverIP; }
            set
            {
                serverIP = value;
                this.RaisePropertyChanged(() => ServerIP);
            }
        }
        /// <summary>
        /// 服务器端口
        /// </summary>
        private string serverSite { get; set; }
        public string ServerSite
        {
            get { return string.IsNullOrEmpty(serverSite) == true ? PublicData.PublicData.serverSite : serverSite; }
            set
            {
                serverSite = value;
                this.RaisePropertyChanged(() => ServerSite);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        public void back()
        {
            main.Grid.Children.Clear();
            MainControl mainControl = new MainControl(main);
            main.Grid.Children.Add(mainControl);
        }
        /// <summary>
        /// 退出
        /// </summary>
        public void signOut()
        {
            IsQuit isQuit = new IsQuit();
            DialogHelper.ShowDialog(isQuit);
        }
        /// <summary>
        /// 取消
        /// </summary>
        public void cancel()
        {
            main.Grid.Children.Clear();
            MainControl mainControl = new MainControl(main);
            main.Grid.Children.Add(mainControl);
        }
        /// <summary>
        /// 获取本地ip
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetLoadIp(string value)
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
        /// <summary>
        /// 连接验证
        /// </summary>
        public string DealServerIP(string value)
        {
            string OldServerIP = PublicData.PublicData.serverIp;
            PublicData.PublicData.serverIp = value;
            Thread.Sleep(500);
            lock (this)
            {
                if (!Login.login())
                {
                    System.Windows.MessageBox.Show("服务器ip设置错误");
                    PublicData.PublicData.serverIp = OldServerIP;
                    return OldServerIP;
                }
            }
            return value;
        }
        /// <summary>
        /// 连接验证
        /// </summary>
        public string DealServerSite(string value)
        {
            string OldServerSite = PublicData.PublicData.serverSite;
            PublicData.PublicData.serverSite = value;
            Thread.Sleep(500);
            lock (this)
            {
                if (!Login.login())
                {
                    System.Windows.MessageBox.Show("服务器端口设置错误");
                    PublicData.PublicData.serverSite = OldServerSite;
                    return OldServerSite;
                }
            }
            return value;
        }
    }
}
