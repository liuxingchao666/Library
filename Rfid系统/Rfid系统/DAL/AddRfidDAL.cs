using Rfid系统.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Management;
using System.Net;
using Rfid系统.BLL;
using Newtonsoft.Json.Linq;

namespace Rfid系统.DAL
{
    public class AddRfidDAL
    {
        public bool AddRfid(ref object errorMsg)
        {
            string url = string.Format("{0}{1}",ServerSetting.UrlPath,"rfidmodule/bing/addBookColloction");
            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["Model"].Value;
                break;
            }
            ISBNbookListInfo info = errorMsg as ISBNbookListInfo;
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add("fkCataBookId",info.id);
            keys.Add("code", info.BookCdoe);
            keys.Add("rfid", info.EPC.Replace(" ",""));
            keys.Add("available", 1);
            keys.Add("lendingPermission",0);
            keys.Add("place",ServerSetting.Place);
            keys.Add("dailyRent",1);
            keys.Add("callNumber",info.CallNumber);
            keys.Add("equipmentName",Dns.GetHostName());
            keys.Add("equipmentCode",HDid);
            

            Http http = new Http(url,keys);
            errorMsg = http.HttpPosts();
            return DealJson(ref errorMsg);
        }

        public bool DealJson(ref object errorMsg)
        {
            RetrunInfo retrunInfo = new RetrunInfo() {TrueOrFalse=false };
            try
            {
                var p = JToken.Parse(errorMsg.ToString());
                foreach (JToken jToken in p)
                {
                    var ken = jToken as JProperty;
                    if (ken.Name.ToLower() == "state" && ken.Value.ToString().ToLower() == "true")
                    {
                        retrunInfo.TrueOrFalse = true;
                    }
                    if (ken.Name.ToLower().Equals("msg"))
                        retrunInfo.result = ken.Value.ToString();
                    if (ken.Name.ToLower().Equals("code"))
                        retrunInfo.ResultCode = ken.Value.ToString();
                    if (retrunInfo.ResultCode != null && retrunInfo.ResultCode.Contains("300"))
                        ServerSetting.IsOverDue = true;

                }
                errorMsg = retrunInfo;
                return retrunInfo.TrueOrFalse;
            }
            catch 
            {
                retrunInfo.result = "未连接服务器";
                retrunInfo.TrueOrFalse = false;
                errorMsg = retrunInfo;
                return false;
            }
        }
    }
}
 