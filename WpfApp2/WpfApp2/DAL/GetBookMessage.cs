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
    public static class GetBookMessage
    {
        public static string url;
        public static bool getBook(ref object errorMsg, ref int currentPage, ref int total)
        {
            try
            {
                url = string.Format("http://{0}:{1}/{2}{3}&pageSize={4}&currentPage={5}", ServerSeting.ServerIP, ServerSeting.ServerSite, "borrowmodule/InAndOutEquipment/getScondPage?fkCataBookId=", errorMsg.ToString(), 9, currentPage);

                Http http = new Http(url, null);
                errorMsg = http.HttpGet(url);
                return DealJson(ref errorMsg, ref total,currentPage);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;

                return false;
            }
        }

        public static bool DealJson(ref object errorMsg, ref int total,int currentPage)
        {
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = false;
                foreach (JToken token in jtoken.Children())
                {
                    var p = token as JProperty;
                    if (p.Name.ToLower() == "state" && p.Value.ToString().ToLower() == "true")
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
                    foreach (JToken j in jtoken.Children())
                    {
                        var p = j as JProperty;
                        if (p.Name.ToLower() == "total")
                        {
                            total = p.Value.toInt();
                        }
                    }
                    List<ArchivesInfo> listInfo = new List<ArchivesInfo>();
                    int i = (currentPage.ToInt() - 1) * 9 + 1;
                    foreach (JToken j in jtoken.Children())
                    {
                        var p = j as JProperty;
                        if (p.Name.ToLower() == "row")
                        {
                            foreach (JToken kens in p.ToList())
                            {
                                foreach (JToken tokens in kens.Children())
                                {
                                    ArchivesInfo info = new ArchivesInfo() { num=i};
                                    List<JToken> list = tokens.ToList();
                                    foreach (JToken token in list)
                                    {
                                        var pp = token as JProperty;
                                        switch (pp.Name.ToLower())
                                        {
                                            case "searchnumber":
                                                info.serchNumber = pp.Value.ToString();
                                                break;
                                            case "code":
                                                info.barcode = pp.Value.ToString();
                                                break;
                                            case "lendstate":
                                                info.RackState = pp.Value.toInt();
                                                switch (info.RackState)
                                                {
                                                    case 1:
                                                        info.IsOpen = System.Windows.Visibility.Visible;
                                                        info.IsSend = System.Windows.Visibility.Hidden;
                                                        info.IsClose = System.Windows.Visibility.Hidden;
                                                        break;
                                                    case 2:
                                                        info.IsOpen = System.Windows.Visibility.Hidden;
                                                        info.IsSend = System.Windows.Visibility.Visible;
                                                        info.IsClose = System.Windows.Visibility.Hidden;
                                                        break;
                                                    default:
                                                        info.IsOpen = System.Windows.Visibility.Hidden;
                                                        info.IsClose = System.Windows.Visibility.Visible;
                                                        info.IsSend = System.Windows.Visibility.Hidden;
                                                        break;
                                                }
                                                break;
                                            case "name":
                                                info.ArchivesName = pp.Value.ToString();
                                                break;
                                            case "locationnam":
                                                info.WZ = pp.Value.ToString();
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    listInfo.Add(info);
                                    i++;
                                }
                            }
                            errorMsg = listInfo;
                            return true;
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
                return false; ;
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
