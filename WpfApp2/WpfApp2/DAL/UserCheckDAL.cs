using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.BLL;

namespace WpfApp2.DAL
{
    public class UserCheckDAL
    {
        public bool UserCheck(ref object errorMsg)
        {
            string url = string.Format("http://{0}:{1}/{2}{3}", ServerSeting.ServerIP, ServerSeting.ServerSite, "borrowmodule/InAndOutEquipment/check?cardNum=", errorMsg.ToString());
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            url += "&&equipmentCode=" + HDid + "&&equipmentName=" + Dns.GetHostName();
            Http http = new Http(url, null);
            errorMsg = http.HttpGet(url);
            return DealJson(ref errorMsg);
        }
        public bool DealJson(ref object errorMsg)
        {
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = false;
                errorMsg = jtoken["msg"].ToString();
                if (jtoken["state"].ToString().ToLower().Equals("true"))
                    result = true;

                return result;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}