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
    public class GetFloorDAL
    {
        public bool GetFloor(out object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { trueOrFlase=false};
                string HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["ProcessorId"].Value;
                    break;
                }
                string url = string.Format("http://{0}:{1}/entranceGuard/log/currency/storeselect?equipmentName=" + Dns.GetHostName() + "&&equipmentCode=" + HDid, PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));
                returnInfo.result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                    returnInfo.trueOrFlase = true;
                errorMsg = returnInfo;
                if (!returnInfo.trueOrFlase)
                    return true;
                List<floorInfo> infos = new List<floorInfo>();
                foreach (var temp in result["row"].Children())
                {
                    floorInfo info = new floorInfo() {
                        id=temp["id"].ToString(),
                        Name =temp["name"].ToString(),
                        IsCheck=(Boolean)temp["check"]
                    };
                    infos.Add(info);
                }
                returnInfo.result = infos;
                return true;
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}
