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
     public class PNEditDAL
    {
        public bool PNEdit(ref object errorMsg)
        {
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/editPN");
                Dictionary<string, object> keyValuePairs = errorMsg as Dictionary<string, object>;
                Http http = new Http(url, keyValuePairs);
                var result = JToken.Parse(http.HttpPut());
                RetrunInfo retrunInfo = new RetrunInfo();
                retrunInfo.result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("false"))
                    retrunInfo.TrueOrFalse = false;
                else
                    retrunInfo.TrueOrFalse = true;
                retrunInfo.ResultCode = result["code"].ToString();
                if (retrunInfo.ResultCode.Contains("300"))
                    ServerSetting.IsOverDue = true;
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
