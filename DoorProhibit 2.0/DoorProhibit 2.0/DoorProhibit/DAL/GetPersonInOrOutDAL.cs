using DoorProhibit.BLL;
using DoorProhibit.Model;
using DoorProhibit.PublicData;
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
     public class GetPersonInOrOutDAL
    {
        public bool GetPersonInOrOut(ref object errorMsg)
        {
            try
            {
                string HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["ProcessorId"].Value;
                    break;
                }

                string url = string.Format(@"http://{0}:{1}/{2}", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite, "entranceGuard/log/getStoTbEntranceGuardInout?fkEntranceGuardNum=" + HDid + "&&fkEntranceGuardName=" + Dns.GetHostName());
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));
                ReturnInfo returnInfo = new ReturnInfo();
                if (result["state"].ToString().ToLower().Equals("true"))
                    returnInfo.trueOrFlase = true;
                else
                    returnInfo.trueOrFlase = false;
                returnInfo.stateCode = result["code"].ToString();
                if (returnInfo.trueOrFlase)
                {
                    EnterOutInfo enterOutInfo = new EnterOutInfo()
                    {
                        enterNumber = result["row"]["enterNumber"].ToInt(),
                        leaveNumber = result["row"]["leaveNumber"].ToInt()
                    };
                    returnInfo.result = enterOutInfo;
                }
                else
                {
                    returnInfo.result = result["msg"].ToString();
                }
                errorMsg = returnInfo;
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}
