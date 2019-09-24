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
    public  class GetCallNumberByIdDAL
    {
        public bool GetCallNumberById(ref object errorMsg)
        {
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "data/cata/book/bookcollection/currency/getBopSearchNumber?id=" + errorMsg.ToString());
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));
                RetrunInfo retrunInfo = new RetrunInfo();
                if (result["state"].ToString().ToLower().Equals("true"))
                    retrunInfo.TrueOrFalse = true;
                else
                    retrunInfo.TrueOrFalse = false;
                if (retrunInfo.TrueOrFalse)
                {
                    CallNumberInfo info = new CallNumberInfo()
                    {
                        searchNumberAuthorNum = result["row"]["searchNumberAuthorNum"].ToString(),
                        searchNumberOrderNum = result["row"]["searchNumberOrderNum"].ToString()
                    };
                    retrunInfo.result = info;
                }
                else
                    retrunInfo.result = result["msg"].ToString();
                errorMsg = retrunInfo;
                return true;
            }
            catch
            {
                errorMsg = "未连接服务器";
                return false;
            }
        }
    }
}
