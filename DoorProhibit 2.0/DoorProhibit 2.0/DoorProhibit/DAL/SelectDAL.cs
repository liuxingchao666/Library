using DoorProhibit.BLL;
using DoorProhibit.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.DAL
{
    public class SelectDAL
    {
        public ReturnInfo Select(string rfid)
        {
            string HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            string url = string.Format("http://{0}:{1}/entranceGuard/log/select?rfid="+rfid+ "&&fkEntranceGuardName=" + Dns.GetHostName() + "&&fkEntranceGuardNum=" + HDid, PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            Http http = new Http(url, null);
            var result = JToken.Parse(http.HttpGet(url));
            ReturnInfo returnInfo = new ReturnInfo() {stateCode=result["code"].ToString() };
            return returnInfo;
        }
    }
}
