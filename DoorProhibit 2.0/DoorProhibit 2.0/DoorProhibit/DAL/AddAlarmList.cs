using DoorProhibit.BLL;
using DoorProhibit.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.DAL
{
    /// <summary>
    /// 添加报警记录
    /// </summary>
    public class AddAlarmList
    {
        /// <summary>
        /// 需添加的报警记录信息
        /// </summary>
        public BookMessage parament =new BookMessage();
        public AddAlarmList(BookMessage message)
        {
            parament = message;
        }
        public void AddAlarm()
        {
            string HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            string url = string.Format("http://{0}:{1}/entranceGuard/log/alarm", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            Dictionary<string, string> matchKV = new Dictionary<string, string>();
            matchKV.Add("fkEntranceGuardNum", HDid);
            matchKV.Add("fkEntranceGuardName", Dns.GetHostName());
            matchKV.Add("rfid", parament.EPC);
            matchKV.Add("inAndOut", "out");
            try
            {
                Http http = new Http(url, matchKV);
                object jsonResult = http.HttpPosts();
                PublicData.PublicData.BJEPC = parament.EPC;
            }
            catch (Exception ex)
            {
                PublicData.PublicData.state = "未连接服务器";
            }
        }
    }
}
