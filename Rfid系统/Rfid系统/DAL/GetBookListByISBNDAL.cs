using Newtonsoft.Json.Linq;
using Rfid系统.BLL;
using Rfid系统.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.DAL
{
     public class GetBookListByISBNDAL
    {
        public bool GetBookListByISBN(ref object errorMsg)
        {
            string url = string.Format("{0}{1}{2}",ServerSetting.UrlPath, "rfidmodule/bing/selectCataTbByISBN?isbn=",errorMsg.ToString());
            Http http = new Http(url,null);
            errorMsg = http.HttpGet(url);
            return DealJosn(ref errorMsg);
        }
        public bool DealJosn(ref object errorMsg)
        {
            RetrunInfo retrunInfo = new RetrunInfo() { TrueOrFalse = false };
            try
            {
                var p = JToken.Parse(errorMsg.ToString());
                foreach (JToken kens in p)
                {
                    var ken = kens as JProperty;
                    if (ken.Name.ToLower().Equals("state") && ken.Value.ToString().ToLower().Equals("true"))
                    {
                        retrunInfo.TrueOrFalse = true;
                    }
                    if (ken.Name.ToLower().Equals("code"))
                        retrunInfo.ResultCode = ken.Value.ToString();
                    if (ken.Name.ToLower().Equals("msg"))
                        retrunInfo.result = ken.Value.ToString();
                    if (retrunInfo.ResultCode != null && retrunInfo.ResultCode.Contains("300"))
                        ServerSetting.IsOverDue = true;
                }
                if (!retrunInfo.TrueOrFalse)
                {
                    errorMsg = retrunInfo;
                    return false;
                }
                foreach (JToken kens in p)
                {
                    var ken = kens as JProperty;
                    if (ken.Name.ToLower().Equals("row"))
                    {
                        List<ISBNbookListInfo> list = new List<ISBNbookListInfo>();
                        foreach (JToken jToken in ken.Value.ToList())
                        {
                            ISBNbookListInfo info = new ISBNbookListInfo();
                            foreach (JToken token in jToken)
                            {
                                var j = token as JProperty;
                                switch (j.Name.ToLower())
                                {
                                    case "id":
                                        info.id = j.Value.ToString();
                                        break;
                                    case "isbn":
                                        info.ISBN = j.Value.ToString();
                                        break;
                                    case "name":
                                        info.BookName = j.Value.ToString();
                                        break;
                                    case "author":
                                        info.Author = j.Value.ToString();
                                        break;
                                    case "fkpressname":
                                        info.Press = j.Value.ToString();
                                        break;
                                    case "publishingtime":
                                        info.PressDate = j.Value.ToString();
                                        break;
                                    case "fktypename":
                                        info.Classification = j.Value.ToString();
                                        break;
                                    case "price":
                                        info.Price = j.Value.ToString();
                                        break;
                                    case "volumenum":
                                        info.JCH = j.Value.ToString();
                                        break;
                                    case "pagenumber":
                                        info.PageNumber = j.Value.ToString();
                                        break;
                                }
                            }
                            list.Add(info);
                        }
                        retrunInfo.result = list;
                        errorMsg = retrunInfo;
                    }
                }
                return true;
            }
            catch
            {
                retrunInfo.result = "未连接服务器";
                errorMsg = retrunInfo;
                return false;
            }
        }
    }
}
