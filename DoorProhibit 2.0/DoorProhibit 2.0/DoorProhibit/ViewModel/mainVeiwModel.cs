using DoorProhibit.Controls;
using DoorProhibit.DAL;
using DoorProhibit.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using DoorProhibit.PublicData;

namespace DoorProhibit.ViewModel
{
    public class mainVeiwModel : NotificationObject
    {
        public MainWindow mainWindow;
        public Thread thread;
        public Thread thread1;
        public MainControl control;
        public mainVeiwModel(MainWindow main,MainControl control)
        {
            mainWindow = main;
            this.control = control;
            thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {

                    if (PublicData.PublicData.state != "未连接服务器")
                    {
                        GetArchivesAccessList getArchivesAccessList = new GetArchivesAccessList();
                        GetArchivesListByList(getArchivesAccessList.GetNewestMessage());
                        ////报警记录
                        GetNewestAlarmList getNewestAlarmList = new GetNewestAlarmList();
                        GetAlarmListByList(getNewestAlarmList.GetNewestMessage());
                        break;
                        //////可借出列
                    }
                    Thread.Sleep(500);
                }
            }));
            thread1 = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    NowDate = DateTime.Now.ToString("yyyy/MM/dd");
                    NowTime = DateTime.Now.ToString("HH:mm:ss");
                    WeekNow = GetWeek(DateTime.Now.DayOfWeek.ToString());
                    state = PublicData.PublicData.state;
                    Color = PublicData.PublicData.color;
                    PIC = PublicData.PublicData.pic;
                    Thread.Sleep(50);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
            thread1.IsBackground = true;
            thread1.Start();
            InNum= PublicData.PublicData.IntoNum.ToInt() + "人";
            OutNum = PublicData.PublicData.OutNum.ToInt() + "人";
        }
        /// <summary>
        /// 进入人次
        /// </summary>
        private string inNum { get; set; }
        public string InNum
        {
            get { return string.IsNullOrEmpty(inNum) ? "0人" : inNum; }
            set
            {
                inNum = PublicData.PublicData.IntoNum.ToString()+"人";
                this.RaisePropertyChanged(() => InNum);
            }
        }
        private string pic { get; set; }
        public string PIC
        {
            get { return pic; }
            set
            {
                pic = value;
                this.RaisePropertyChanged(()=>PIC);
            }
        }
        private string color { get; set; }
        public string Color
        {
            get { return color; }
            set
            {
                color = string.IsNullOrEmpty(value) ? "#FF7E00" : value;
                this.RaisePropertyChanged(()=>Color);
            }
        }
        /// <summary>
        /// 进出人次
        /// </summary>
        private string outNum { get; set; }
        public string OutNum
        {
            get { return string.IsNullOrEmpty(outNum) ? "0人" : outNum; }
            set
            {
                outNum = PublicData.PublicData.OutNum.ToString()+"人";
                this.RaisePropertyChanged(() => OutNum);
            }
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        private string nowTime { get; set; }
        public string NowTime
        {
            get { return string.IsNullOrEmpty(nowTime) ? DateTime.Now.ToString("HH:mm:ss") : nowTime; }
            set
            {
                nowTime = value;
                this.RaisePropertyChanged(() => NowTime);
            }
        }
        /// <summary>
        /// 当前日期
        /// </summary>
        private string nowDate { get; set; }
        public string NowDate
        {
            get { return string.IsNullOrEmpty(nowDate) ? DateTime.Now.ToString("yyyy/MM/dd") : nowDate; }
            set
            {
                nowDate = value;
                this.RaisePropertyChanged(() => NowDate);
            }
        }
        /// <summary>
        /// 设备通信状态
        /// </summary>
        private string State { get; set; }
        public string state
        {
            get { return string.IsNullOrEmpty(State) ? "正常" : State; }
            set
            {
                State = value;
                this.RaisePropertyChanged(() => state);
            }
        }
        /// <summary>
        /// 当前星期数
        /// </summary>
        private string weekNow { get; set; }
        public string WeekNow
        {
            get { return string.IsNullOrEmpty(weekNow) ? GetWeek(DateTime.Now.DayOfWeek.ToString()) : weekNow; }
            set
            {
                weekNow = value;
                this.RaisePropertyChanged(() => WeekNow);
            }
        }
        /// <summary>
        /// 设置
        /// </summary>
        private ICommand SetUpCommond { get; set; }
        public ICommand setUpCommond
        {
            get
            {
                return SetUpCommond ?? (SetUpCommond = new DelegateCommand(setUp));
            }
        }
        /// <summary>
        /// 档案出入表
        /// </summary>
        public List<Archives> messages = new List<Archives>();
        public List<Archives> message
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                this.RaisePropertyChanged(() => message);
            }
        }
        /// <summary>
        /// 报警
        /// </summary>
        public List<Alarm> AlarmLists = new List<Alarm>();
        public List<Alarm> AlarmList
        {
            get
            {
                return AlarmLists;
            }
            set
            {
                AlarmLists = value;
                this.RaisePropertyChanged(() => AlarmList);
            }
        }

        /// <summary>
        /// 获取星期几---英转中
        /// </summary>
        /// <param name="week">sunday</param>
        /// <returns></returns>
        public string GetWeek(string week)
        {
            switch (week)
            {
                case "Monday":
                    return "星期一";
                case "Tuesday":
                    return "星期二";
                case "Wednesday":
                    return "星期三";
                case "Thursday":
                    return "星期四";
                case "Friday":
                    return "星期五";
                case "Saturday":
                    return "星期六";
                default:
                    return "星期天";
            }
        }


        /// <summary>
        /// 获取档案出入记录
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        public List<Archives> GetArchivesListByList(List<BookMessage> books)
        {
            List<Archives> list = new List<Archives>();
            int index = 1;

            foreach (BookMessage temp in books)
            {
                Archives archives = new Archives()
                {
                    num = index + "、",
                    FileName = temp.FileName,
                    date=temp.AlarmTime
                };
                index++;
                list.Add(archives);
            }

            message = list;
            return list;
        }
        /// <summary>
        /// 获取报警记录
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        public List<Alarm> GetAlarmListByList(List<BookMessage> books)
        {
            List<Alarm> list = new List<Alarm>();
            int index = 1;
            foreach (BookMessage temp in books)
            {
                Alarm archives = new Alarm()
                {
                    num = index + "、",
                    FileName = temp.FileName,
                    AlarmTime = temp.AlarmTime
                };
                index++;
                list.Add(archives);
            }
            AlarmList = list;
            return list;
        }
        /// <summary>
        /// 设置
        /// </summary>
        public void setUp()
        {
            Loading loading = new Loading();
            DialogHelper.ShowDialog(loading);
            if (loading.isLogin)
            {
                control.mysp.Close();
                control.thread.Abort();
                mainWindow.Grid.Children.Clear();
                SetUpMain setUpMain = new SetUpMain(mainWindow);
                mainWindow.Grid.Children.Add(setUpMain);
            }
        }
    }
    /// <summary>
    /// 档案记录
    /// </summary>
    public class Archives
    {
        public string num { get; set; }
        public string FileName { get; set; }
        public DateTime date { get; set; }
    }
    /// <summary>
    /// 报警记录
    /// </summary>
    public class Alarm
    {
        public string num { get; set; }
        public string FileName { get; set; }
        public DateTime AlarmTime { get; set; }
    }
}
