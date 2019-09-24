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
    public  class GetReplacementCost
    {
        public bool Getdesosit(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/getReissueCost");
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
                ReturnInfo info = new ReturnInfo();
                if (result)
                {
                    info.SuccessOrFalse = true;
                    foreach (JToken jToken in jtoken.Children())
                    {
                        var p = jToken as JProperty;
                        if (p.Name.ToLower() == "row")
                        {
                            info.CostDeposit = p.Value.ToString().toInt();
                        }
                    }
                    errorMsg = info;
                    return result;
                }
                info.SuccessOrFalse = false;
                info.errorMsg = "服务器异常，请稍后再试";
                errorMsg = info;
                return false;
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
