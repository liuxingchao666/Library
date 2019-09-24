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
    public class GetNowUserDAL
    {
        public string pic;
          
        public  bool GetPushDAL(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/readerTbCardInfo/getNowUser");
                UserMessage userMessage = errorMsg as UserMessage;
                pic = userMessage.PIC;
                url += "?idCard=" + userMessage.IdentificationCode;
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                return DealJson(ref errorMsg, jsonResult);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;
                return false;
            }
        }
        public  bool DealJson(ref object errorMsg, object json)
        {
            try
            {
                var result = JToken.Parse(json.ToString());
                bool bl = true;
                foreach (JToken jToken in result.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false")
                    {
                        bl = false;
                    }
                }
                if (!bl)
                {
                    return false;
                }
                foreach (JToken jToken in result.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "row")
                    {
                        foreach (JToken jtoken in jToken)
                        {
                            UserMessage user = new UserMessage();
                            foreach (JToken j in jtoken)
                            {
                                var value = j as JProperty;
                                switch (value.Name.ToLower())
                                {
                                    case "readername":
                                        user.Name = value.Value.ToString();
                                        break;
                                    case "readercard":
                                        user.IdentificationCode = value.Value.ToString();
                                        break;
                                    case "phone":
                                        user.Phone = value.Value.ToString();
                                        break;
                                    case "cardnumber":
                                        user.UserCard = value.Value.ToString();
                                        break;
                                    case "name":
                                        user.Grade = value.Value.ToString();
                                        break;
                                    case "cardstatename":
                                        user.constate = value.Value.ToString();
                                        break;
                                    case "logintime":
                                        user.ZCDate = Convert.ToDateTime(value.Value.ToString()).ToString("yyyy-MM-dd");
                                        break;
                                    case "overduename":
                                        user.overdueName = value.Value.ToString();
                                        break;
                                    case "cardstate":
                                        user.CardState = value.Value.ToString();
                                        break;
                                    case "overdue":
                                        user.OverDueState = value.Value.ToString();
                                        break;
                                    case "state":
                                        user.State = value.Value.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            user.PIC = pic;
                            errorMsg = user;
                        }
                    
                        return true;
                    }
                }
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
