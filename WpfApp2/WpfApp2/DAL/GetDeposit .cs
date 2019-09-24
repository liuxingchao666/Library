using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.BLL;
using WpfApp2.Model;

namespace WpfApp2.DAL
{
    public class GetDeposit
    {
        public bool Getdesosit(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/reissueCost");
                CardData cardData = errorMsg as CardData;
                url += "?idCard=" + cardData.CardNo;
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                errorMsg = jsonResult;
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
            ReturnInfo info = new ReturnInfo();
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = true;
                foreach (JToken jToken in jtoken.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false")
                    {
                        result = false;
                        break;
                    }
                    else if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "true")
                    {
                        result = true;
                        break;
                    }
                }

                info.SuccessOrFalse = true;
                foreach (JToken jToken in jtoken.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "row")
                    {
                        info.Deposit = p.Value.ToString().toInt();
                    }
                    if (p.Name.ToLower() == "msg")
                    {
                        info.errorMsg = p.Value.ToString();
                    }
                }
                info.SuccessOrFalse = result;
                errorMsg = info;
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
