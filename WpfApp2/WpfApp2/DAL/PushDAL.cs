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
    public static class PushDAL
    {
        public static bool GetPushDAL(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/bookinfo/getOrderBook");
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                return DealJson(ref errorMsg,jsonResult);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;
                return false;
            }
        }
        public static bool DealJson(ref object errorMsg,object json)
        {
            try
            {
                var result = JToken.Parse(json.ToString());
                bool bl = true;
                foreach (JToken jToken in result.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower()=="false")
                    {
                        bl = false;
                    }
                    if (p.Name.ToLower() == "msg")
                    {
                        errorMsg = p.Value.ToString();
                    }
                }
                if (!bl)
                {
                    return false;
                }
                foreach (JToken jToken in result.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "rows")
                    {
                        List<ArchivesInfo> list = new List<ArchivesInfo>();
                        List<JToken> jTokens = p.ToList();
                        foreach (JToken jtoken in jTokens)
                        {
                            ArchivesInfo archivesInfo = new ArchivesInfo();
                            foreach (JToken j in jtoken.Children())
                            {
                                List<JToken> jT = j.ToList();
                                foreach (JToken a in jT)
                                {
                                    var jp = a as JProperty;
                                    switch (jp.Name.ToLower())
                                    {
                                        case "barcode":
                                            archivesInfo.barcode = jp.Value.ToString();
                                            break;
                                        case "bookname":
                                            archivesInfo.ArchivesName = jp.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            list.Add(archivesInfo);
                        }
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
