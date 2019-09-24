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
    public  class SelectListDAL
    {
        public bool SelectList(SelectClass @class, ref object errorMsg)
        {
            ServerSetting.@class = @class;
         
            string url = string.Format("{0}{1}?", ServerSetting.UrlPath, "rfidmodule/query/select");
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("pageSize",10);
            keyValuePairs.Add("currentPage", ServerSetting.loadPage.ToInt());
            switch (@class)
            {
                case SelectClass.isbn:
                    keyValuePairs.Add("isbn", errorMsg.ToString());
                    break;
                case SelectClass.RFID:
                    List<string> list = errorMsg as List<string>;
                    keyValuePairs.Add("rfids", list.ToArray());
                    break;
                case SelectClass.callNumber:
                    keyValuePairs.Add("callNumber",errorMsg.ToString());
                    break;
                default:
                    keyValuePairs.Add("code", errorMsg.ToString());
                    break;
            }
          
            Http http = new Http(url, keyValuePairs);
            errorMsg = http.HttpPosts();
            return DealJson(ref errorMsg);
        }
        public bool DealJson(ref object errorMsg)
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
                    if (ken.Name.ToLower().Equals("pages"))
                        ServerSetting.totalPages = ken.Value.ToInt();
                    if (ken.Name.ToLower().Equals("row"))
                    {
                        int index = 1+(ServerSetting.loadPage-1)*10 ;
                        List<QueryInfo> list = new List<QueryInfo>();
                        foreach (JToken jToken in ken.Value.ToList())
                        {
                            QueryInfo info = new QueryInfo() { num = index };
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
                                    case "callnumber":
                                        info.CallNumber = j.Value.ToString();
                                        break;
                                    case "price":
                                        info.Price = j.Value.ToString();
                                        break;
                                    case "fktypename":
                                        info.ClassificationName = j.Value.ToString();
                                        break;
                                    case "pagenumber":
                                        info.PageNumber = j.Value.ToString();
                                        break;
                                    case "code":
                                        info.CorrectionCode = j.Value.ToString();
                                        break;
                                    case "rfid":
                                        info.RFID = j.Value.ToString();
                                        break;
                                    case "place":
                                        info.Place = j.Value.ToString();
                                        break;
                                    case "bop":
                                        info.bop = j.Value.ToString();
                                        switch (info.bop)
                                        {
                                            case "0":
                                                info.bop = "图书";
                                                break;
                                            default:
                                                info.bop = "期刊";
                                                break;
                                        }
                                        break;
                                    case "merge":
                                        info.merge = j.Value.ToString();
                                        break;
                                }
                            }
                            index++;
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
    public enum SelectClass
    {
        isbn = 0,
        RFID = 1,
        CorrectionCode=2,
        callNumber = 3
    }
}
