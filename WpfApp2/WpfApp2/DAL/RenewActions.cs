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
    public class RenewActions
    {
        public string cardNum { get; set; }
        public bool GetActionState(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/bookinfo/renewBooks");
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                ActionErrormsg msg = errorMsg as ActionErrormsg;
                int[] a = new int[] { 1};
                pairs.Add("cardnumber", msg.ICCardNum);
                pairs.Add("logids",msg.logids);
                cardNum = msg.ICCardNum;
                Http http = new Http(url, pairs);
                object jsonResult = http.HttpPosts();
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
                List<ActionErrormsg> list = new List<ActionErrormsg>();
                var ken = JToken.Parse(errorMsg.ToString());
                bool result = false;
                foreach (JToken token in ken.Children())
                {
                    var p = token as JProperty;
                    switch (p.Name.ToLower())
                    {
                        case "state":
                            if (p.Value.ToString().ToLower() == "true")
                            {
                                result = true;
                            }
                            break;
                        case "msg":
                            errorMsg = p.Value.ToString1();
                            break;
                        default:
                            break;
                    }
                }
                if (!result)
                {
                    return result;
                }
                foreach (JToken token in ken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower()=="row")
                    {
                        List<JToken> jTokens = token.ToList();
                        foreach (JToken kens in jTokens)
                        {
                            List<JToken> jken = kens.ToList();
                            foreach (JToken pp in jken)
                            {
                                ActionErrormsg msg = new ActionErrormsg();
                                msg.ICCardNum = cardNum;
                                foreach (JToken ps in pp.Children())
                                {
                                    var jp = ps as JProperty;
                                    switch (jp.Name.ToLower())
                                    {
                                        case "id":
                                            msg.id = jp.Value.ToString();
                                            break;
                                        case "state":
                                            msg.state = Convert.ToBoolean(jp.Value.ToString());
                                            break;
                                        case "message":
                                            msg.errorMsg = jp.Value.ToString();
                                            break;
                                        case "planreturntime":
                                            msg.planReturnTime = jp.Value.ToString();
                                            break;
                                        case "fkcardnumber":
                                            msg.ICCardNum = jp.Value.ToString();
                                            break;
                                        case "librarybookcode":
                                            msg.BardCode = jp.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    };
                                }
                                list.Add(msg);
                            }
                        }
                        errorMsg = null;
                        errorMsg = list;
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




