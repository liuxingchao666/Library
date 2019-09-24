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
     public class GetCsDAL
    {
        public bool GetCs(ref object errorMsg)
        {
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/getcs?cataPeriodicalId="+errorMsg.ToString());
                Http http = new Http(url, null);
                errorMsg = http.HttpGet(url);
                var result = JToken.Parse(errorMsg.ToString());
                errorMsg = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    result = JToken.Parse(result["row"].ToString());
                    ISBNbookListInfo listInfo = new ISBNbookListInfo()
                    {
                        fkTypeCode =result["typeCode"].ToString(),
                        OrderNum =(result["orderNum"].ToString().ToInt()+1).ToString()
                    };
                    errorMsg = listInfo;
                    return true;
                }
                else
                    return false;
            }
            catch 
            {
                errorMsg = "未连接服务器";
                return false;
            }
        }
    }
}
