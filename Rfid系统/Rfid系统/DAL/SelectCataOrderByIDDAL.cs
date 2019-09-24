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
    public class SelectCataOrderByIDDAL
    {
        public bool selectCataOrderByID(ref object errorMsg)
        {
            string url = string.Format(@"{0}{1}?id={2}",ServerSetting.UrlPath, "rfidmodule/bing/selectCataOrderByID",errorMsg.ToString());
            Http http = new Http(url,null);
            errorMsg = http.HttpGet(url);
            return DealJson(ref errorMsg);
        }
        public bool DealJson(ref object errorMsg)
        {
            RetrunInfo retrunInfo = new RetrunInfo();
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
                foreach(JToken kens in p)
                {
                    var ken = kens as JProperty;
                    if (ken.Name.ToLower().Equals("row"))
                    {
                        ISBNbookListInfo iSBNbookListInfo = new ISBNbookListInfo();
                        foreach (JToken token in ken.Value.ToList())
                        {
                            var j = token as JProperty;
                            switch (j.Name.ToLower())
                            {
                                case "volumenum":
                                    iSBNbookListInfo.JCH = j.Value.ToString();
                                    break;
                                case "fktypecode":
                                    iSBNbookListInfo.fkTypeCode = j.Value.ToString();
                                    break;
                                case "ordernum":
                                    iSBNbookListInfo.OrderNum = j.Value.ToString();
                                    break;
                                case "setbooks":
                                    iSBNbookListInfo.SetBooks = j.Value.ToString();
                                    break;
                            }
                        }
                        retrunInfo.result = iSBNbookListInfo;
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
