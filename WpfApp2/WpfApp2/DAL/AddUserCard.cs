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
    public  class AddUserCard
    {
        public bool AddCard(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/add");
                CardData data = errorMsg as CardData;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["ProcessorId"].Value;
                    break;
                }
                pairs.Add("reissueCost",data.Despoit);
                pairs.Add("phone",data.phone);
                pairs.Add("cardNumber",data.cardNum);
                pairs.Add("balance", data.cost);
                pairs.Add("fkReaderCard",data.CardNo);
                pairs.Add("fkReaderName",data.Name);
                pairs.Add("readerAddress",data.Address);
                pairs.Add("equipmentCode",HDid);
                pairs.Add("equipmentName", Dns.GetHostName());

                Http http = new Http(url, pairs);
                errorMsg = http.HttpPosts();
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
                        errorMsg = p.Value.ToString();
                    }
                }
                if (result)
                {
                    foreach (JToken token in jtoken.Children())
                    {
                        var p = token as JProperty;
                        if (p.Name.ToLower() == "row")
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
