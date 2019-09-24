using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.BLL;
using WpfApp2.Model;
using static System.Net.WebRequestMethods;
using Http = WpfApp2.BLL.Http;

namespace WpfApp2.DAL
{
    public static class GetBookIntroduceDal
    {
        public static string url;
        public static bool getBookClass(ref object errorMsg)
        {
            try
            {
                url = string.Format("http://{0}:{1}/{2}{3}", ServerSeting.ServerIP, ServerSeting.ServerSite, "borrowmodule/InAndOutEquipment/getThirdPage?barcode=", errorMsg.ToString());

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

        public static bool DealJson(ref object errorMsg)
        {
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = false;
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "false")
                    {
                        result = true;
                    }
                    if (p.Name.ToLower() == "msg")
                    {
                        errorMsg = p.Value.ToString();
                    }
                }
                if (result)
                {
                    return false;
                }
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "row")
                    {
                        foreach (JToken kens in p.ToList())
                        {
                            foreach (JToken tokens in kens.Children())
                            {
                                ArchivesInfo info = new ArchivesInfo();
                                List<JToken> list = tokens.ToList();
                                foreach (JToken ken in list)
                                {
                                    var pp = ken as JProperty;
                                    switch (pp.Name.ToLower())
                                    {
                                        case "author":
                                            info.Author = pp.Value.ToString();
                                            break;
                                        case "name":
                                            info.ArchivesName = pp.Value.ToString();
                                            break;
                                        case "introduction":
                                            info.DetailedMessage = pp.Value.ToString();
                                            break;
                                        case "coverphotourl":
                                            info.PIC = pp.Value.ToString();
                                            break;
                                        case "pagenumber":
                                            info.PageNum = pp.Value.ToString();
                                            break;
                                        case "publishingtime":
                                            info.PressDate = pp.Value.ToString();
                                            break;
                                        case "fkpressname":
                                            info.Press = pp.Value.ToString();
                                            break;
                                        case "queryisbn":
                                            info.ISBN = pp.Value.ToString();
                                            break;
                                        case "fktypename":
                                            info.LayOut = pp.Value.ToString();
                                            break;
                                    }
                                }
                                errorMsg = info;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServerSeting.connState = false;
                errorMsg = ex.Message;
                return false;
            }
            return false;
        }
    }
}
