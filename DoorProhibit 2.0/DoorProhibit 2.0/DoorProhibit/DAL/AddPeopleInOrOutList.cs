using DoorProhibit.BLL;
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
    public class AddPeopleInOrOutList
    {
        /// <summary>
        /// 添加人员进出记录
        /// </summary>
     
        public void AddPeopleInOrOut(PublicData.InOrOut inOrOut)
        {
            string enterOrOut = "";
            string url = string.Format("http://{0}:{1}/entranceGuard/log/inAndOut", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            switch (inOrOut)
            {
                case PublicData.InOrOut.In:
                    enterOrOut = "in";
                    break;
                default:
                    enterOrOut = "out";
                    break;
            }
            string HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            Dictionary<string, string> matchKV = new Dictionary<string, string>();
            matchKV.Add("fkEntranceGuardNum", HDid);
            matchKV.Add("fkEntranceGuardName", Dns.GetHostName());
            matchKV.Add("inAndOut", enterOrOut);
            try
            {
                Http http = new Http(url, matchKV);
                object jsonResult = http.HttpPosts();
            }
            catch (Exception ex)
            {
                PublicData.PublicData.state = "未连接服务器";
            }
        }
    }
}
