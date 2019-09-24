using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using WpfApp2.BLL;

namespace WpfApp2.DAL
{
    public class MakeUpDAL
    {   
        public bool MakeUpCard(ref object errorMsg)
        {
            bool result;
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/cardReissue");
                string HDid = "";
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = moc.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        ManagementObject mo = (ManagementObject)enumerator.Current;
                        HDid = (string)mo.Properties["ProcessorId"].Value;
                    }
                }
                CardData data = errorMsg as CardData;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("fkReaderCard", data.CardNo);
                pairs.Add("newCardNumber", data.cardNum);
                pairs.Add("reissueCost", data.cost);
                pairs.Add("equipmentCode", HDid);
                pairs.Add("equipmentName", Dns.GetHostName());
                Http http = new Http(url, pairs);
                errorMsg = http.HttpPut();
                result = this.DealJson(ref errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;
                result = false;
            }
            return result;
        }

        public bool DealJson(ref object errorMsg)
        {
            bool result2;
            try
            {
                JToken jtoken = JToken.Parse(errorMsg.ToString());
                bool result = true;
                foreach (JToken token in jtoken.Children())
                {
                    JProperty p = token as JProperty;
                    bool flag = p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false";
                    if (flag)
                    {
                        result = false;
                    }
                    bool flag2 = p.Name.ToLower() == "msg";
                    if (flag2)
                    {
                        errorMsg = p.Value.ToString();
                    }
                }
                bool flag3 = result;
                if (flag3)
                {
                    foreach (JToken token2 in jtoken.Children())
                    {
                        JProperty p2 = token2 as JProperty;
                        bool flag4 = p2.Name.ToLower() == "row";
                        if (flag4)
                        {
                            errorMsg = p2.Value.ToString();
                            result2 = true;
                            return result2;
                        }
                    }
                }
                result2 = result;
            }
            catch (Exception ex)
            {
                ServerSeting.connState = false;
                errorMsg = ex.Message;
                result2 = false;
            }
            return result2;
        }
    }
}
