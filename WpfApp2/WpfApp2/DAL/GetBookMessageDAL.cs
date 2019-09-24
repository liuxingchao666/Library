
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
    public class GetBookMessageDAL
    {
        public GetBookMessageDAL(ArchivesInfo info)
        {
            paramter = info.barcode;
            url = string.Format("http://{0}:{1}/{2}?barcode={3}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/bookinfo/getBookInfo", "321321"); 
        }
        private  string url = "";
        public  string paramter = "";
        public bool GetMessage(ref object  errorMsg,ArchivesInfo info)
        {
            try
            {
              
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                errorMsg = jsonResult;
                if (DealJson(ref errorMsg))
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

        public bool DealJson(ref object errorMsg)
        {
            try
            {
                ArchivesInfo info = new ArchivesInfo();
                var jToken = JToken.Parse(errorMsg.ToString());
                bool state = false;
                foreach (JToken j in jToken.Children())
                {
                    var p = j as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "true")
                    {
                        state = true;
                    }
                }
                foreach (JToken j in jToken.Children())
                {
                    if (state)
                    {
                        var p = j as JProperty;
                        if (p.Name.ToLower() == "row")
                        {
                            foreach (JToken kens in j.Children())
                            {
                                foreach (JToken tokens in kens.Children())
                                {
                                    List<JToken> list = tokens.ToList();
                                    foreach (JToken token in list)
                                    {
                                        var pp = token as JProperty;
                                        switch (pp.Name.ToLower())
                                        {
                                            case "coverphotourl":
                                                info.PIC = pp.Value.ToString();
                                                break;
                                            case "pagenumber":
                                                info.PageNum = pp.Value.ToString();
                                                break;
                                            case "author":
                                                info.Author = pp.Value.ToString();
                                                break;
                                            case "fkpressname":
                                                info.Press = pp.Value.ToString();
                                                break;
                                            case "publishingtime":
                                                info.PressDate = pp.Value.ToString();
                                                break;
                                            case "isbn":
                                                info.ISBN= pp.Value.ToString();
                                                break;
                                            case "name":
                                                info.ArchivesName = pp.Value.ToString();
                                                break;
                                            case "introduction":
                                                info.DetailedMessage = pp.Value.ToString();
                                                break;
                                            case "":
                                               
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                errorMsg = info;
                                return true;
                            }
                        }
                        else
                        {
                             p = j as JProperty;
                            if (p.Name.ToLower() == "msg")
                            {
                                errorMsg = p.Value.ToString();
                            }
                        }
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
