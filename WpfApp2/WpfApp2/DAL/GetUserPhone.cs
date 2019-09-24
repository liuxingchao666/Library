using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.BLL;

namespace WpfApp2.DAL
{
    public class GetUserPhone
    {
        public bool GetPhone(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/selectUserInfoByCard");
                url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/selectUserInfoByCard");
                url += "?idCard=" + errorMsg.ToString();
                
                Http http = new Http(url, errorMsg.ToString());
                errorMsg = http.HttpGet(url);
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
                            foreach (JToken ken in token)
                            {
                                foreach (JToken j in ken)
                                {
                                    var value = j as JProperty;
                                    switch (value.Name.ToLower())
                                    {
                                        case "phone":
                                            errorMsg = value.Value.ToString();
                                            return true;
                                    }
                                }
                               
                            }
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
