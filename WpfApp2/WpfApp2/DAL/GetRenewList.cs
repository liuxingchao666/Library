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
     public  class GetRenewList
    {

        public GetRenewList(string cardNum,string bookName)
        {
           url= string.Format("http://{0}:{1}/{2}?cardnumber={3}&name={4}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/bookinfo/getBorrowBookInfoByCard", cardNum, bookName);
        }
        public static string url = "";

        public static bool GetRenews(ref object errorMsg)
        {
            try
            {
                Http http = new Http(url,null);
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
        public static bool DealJson(ref object errorMsg)
        {
            try
            {
                List<ArchivesInfo> archives = new List<ArchivesInfo>();
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = true;
                int max = 0;int size = 0;
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false")
                    {
                        result= false;
                    }
                    if (p.Name.ToLower() == "msg")
                    {
                        errorMsg = p.Value.ToString();
                    }
                    if (p.Name.ToLower() == "currentPage")
                    {
                        max = p.Value.toInt();
                    }
                    if (p.Name.ToLower() == "pagesize")
                    {
                        size = p.Value.toInt();
                    }
                }
                if (!result)
                {
                    return false;
                }
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "rows")
                    {
                        List<JToken> jTokens = token.ToList();
                        foreach (JToken kens in jTokens)
                        {
                            int i = 1;
                            List<JToken> kenLists = kens.ToList();
                            foreach (JToken k in kenLists)
                            {
                                ArchivesInfo info = new ArchivesInfo();
                                info.num = (max ) * size + i;
                                List<JToken> kenList = k.ToList();
                                foreach (JToken ks in kenList) { 
                                var jp = ks as JProperty;
                                    switch (jp.Name.ToLower())
                                    {
                                        case "id":
                                            info.Id = jp.Value.ToString();
                                            break;
                                        case "bookname":
                                            info.ArchivesName = jp.Value.ToString();
                                            break;
                                        case "renewcount":
                                            info.SurplusRenewableTimes = jp.Value.toInt();
                                            break;
                                        case "createtime":
                                            info.BSTime = jp.Value.ToString();
                                            break;
                                        case "planreturntime":
                                            info.EDTime = jp.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                info.source =@"..\ControlImages\灰色选择.png";
                                archives.Add(info);
                                i++;
                            }
                            
                        }
                        errorMsg = archives;
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
