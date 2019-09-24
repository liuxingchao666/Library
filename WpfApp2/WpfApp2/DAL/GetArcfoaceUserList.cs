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
    public  class GetArcfoaceUserList
    {
        public  string url;
        public  bool getBookClass(ref object errorMsg)
        {
            try
            {
                url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/faceRecognition/select");

                Http http = new Http(url, null);
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

        public  bool DealJson(ref object errorMsg)
        {
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = true;
#pragma warning disable CS0219 // 变量“size”已被赋值，但从未使用过它的值
#pragma warning disable CS0219 // 变量“max”已被赋值，但从未使用过它的值
                int max = 0; int size = 0;
#pragma warning restore CS0219 // 变量“max”已被赋值，但从未使用过它的值
#pragma warning restore CS0219 // 变量“size”已被赋值，但从未使用过它的值
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false")
                    {
                        result = false;
                    }
                }
                if (!result)
                {
                    return false;
                }
                List<UserMessage> usersList = new List<UserMessage>();
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "row")
                    {
                        List<JToken> jTokens = token.ToList();
                        foreach (JToken kens in jTokens)
                        {
#pragma warning disable CS0219 // 变量“i”已被赋值，但从未使用过它的值
                            int i = 1;
#pragma warning restore CS0219 // 变量“i”已被赋值，但从未使用过它的值
                            List<JToken> kenLists = kens.ToList();
                            foreach (JToken k in kenLists)
                            {
                                List<JToken> kenList = k.ToList();
                                UserMessage user = new UserMessage();
                                foreach (JToken ks in kenList)
                                {
                                    var jp = ks as JProperty;
                                    switch (jp.Name.ToLower())
                                    {
                                        case "name":
                                            user.Name = jp.Value.ToString();
                                            break;
                                        case "img":
                                            user.PIC = jp.Value.ToString();
                                            break;
                                        case "age":
                                            user.age = jp.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                usersList.Add(user);
                            }

                        }
                        errorMsg = usersList;
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
