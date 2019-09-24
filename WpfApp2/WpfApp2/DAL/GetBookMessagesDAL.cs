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
    public class GetBookMessagesDAL
    {
        public GetBookMessagesDAL(string bookName,int currPage,int currSize)
        {
            url = string.Format("http://{0}:{1}/{2}?Name={3}&currentpage={4}&pagesize={5}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/bookinfo/getBookInfos", bookName,currPage,currSize); 
        }
        public string url = "";
        public bool GetMessages(ref object errorMsg,ref int max,ref int size)
        {
            try
            {
                Http http = new Http(url, null);
                errorMsg = http.HttpGet(url);
                if (DealJson(ref errorMsg,ref max,ref size))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;
                return false;
            }
        }
        public bool DealJson(ref object errorMsg, ref int max, ref int size)
        {
            try
            {
                var jToken= JToken.Parse(errorMsg.ToString());
                bool result = false;
                foreach (JToken jTokens in jToken.Children())
                {
                    var jp = jTokens as JProperty;
                    if (jp.Name.ToLower() == "state" && jp.Value.ToString().ToLower() == "true")
                    {
                        result= true;
                    }
                    if (jp.Name.ToLower() == "msg")
                    {
                        errorMsg = jp.Value;
                    }
                    if (jp.Name.ToLower() == "currentPage")
                    {
                        max = jp.Value.toInt();
                    }
                    if (jp.Name.ToLower() == "pagesize")
                    {
                        size = jp.Value.toInt();
                    }
                }
                if (!result)
                {
                    return false;
                }
                List<ArchivesInfo> list = new List<ArchivesInfo>();
                foreach (JToken jTokens in jToken.Children())
                {

                    var jp = jTokens as JProperty;
                    if (jp.Name.ToLower() == "rows")
                    {
                        List<JToken> kens = jTokens.ToList();
                        int i = 1;
                        foreach (JToken ken in kens)
                        {
                           
                            foreach (JToken k in ken.Children())
                            {
                                ArchivesInfo info = new ArchivesInfo();
                                info.num = (max) * size+i;
                                foreach (JToken kk in k)
                                {
                                    var p = kk as JProperty;
                                    switch (p.Name.ToLower())
                                    {
                                        case "searchnumber":
                                            info.serchNumber = p.Value.ToString();
                                            break;
                                        case "barcode":
                                            info.barcode = p.Value.ToString();
                                            break;
                                        case "state":
                                            info.RackState = p.Value.toInt();
                                            switch (info.RackState)
                                            {
                                                case 0:
                                                    info.IsOpen = System.Windows.Visibility.Visible;
                                                    info.IsClose = System.Windows.Visibility.Hidden;
                                                    break;
                                                default:
                                                    info.IsOpen = System.Windows.Visibility.Hidden;
                                                    info.IsClose = System.Windows.Visibility.Visible;
                                                    break;
                                            }
                                            break;
                                        case "name":
                                            info.ArchivesName = p.Value.ToString();
                                            break;
                                        case "location":
                                            info.WZ = p.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                             
                                list.Add(info);
                                i++;
                            }
                           
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
