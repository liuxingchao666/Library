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
    public class ReportLossDAL
    {
        public bool AddCard(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/cardReport");
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)

                {
                    HDid = (string)mo.Properties["ProcessorId"].Value;
                    break;
                }
                url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/cardReport");
                CardData data = errorMsg as CardData;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("fkReaderCard", data.CardNo);
                pairs.Add("equipmentCode", HDid);
                pairs.Add("equipmentName", Dns.GetHostName());
                Http http = new Http(url, pairs);
                errorMsg = http.HttpPut();
                return DealJson(ref errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;

                return false;
            }
        }

        public bool DealJson(ref object errorMsg)
        {
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = true;
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false")
                    {
                        result = false;
                    }
                    if (p.Name.ToLower() == "msg")
                    {
                        errorMsg += p.Value.ToString();
                    }
                    if (p.Name.ToLower() == "code" && p.Value.ToString().Trim() == "301")
                    {
                        errorMsg += p.Value.ToString();
                    }
                }
                if (!errorMsg.ToString().Contains("301"))
                {
                    foreach (JToken token in jtoken.Children())
                    {
                        var p = token as JProperty;
                        if (p.Name.ToLower() == "msg")
                        {
                            errorMsg = p.Value.ToString();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ServerSeting.connState = false;
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}
