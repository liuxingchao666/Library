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
    public class BingCorrectionEditDAL
    {
        public bool BingCorrectionEdit(ref object errorMsg)
        {
            Dictionary<string, object> keyValues = errorMsg as Dictionary<string, object>;
            string url = string.Format("{0}{1}",ServerSetting.UrlPath, "rfidmodule/query/edit");
            Http http = new Http(url,keyValues);
            errorMsg = http.HttpPosts();
            return DealJson(ref errorMsg);
        }
        public bool DealJson(ref object errorMg)
        {
            RetrunInfo retrunInfo = new RetrunInfo() { TrueOrFalse = false };
            try
            {
                var p = JToken.Parse(errorMg.ToString());
                if (p["state"].ToString().ToLower().Equals("true"))
                    retrunInfo.TrueOrFalse = true;
                retrunInfo.result = p["msg"].ToString();
                retrunInfo.ResultCode = p["code"].ToString();
                if (retrunInfo.ResultCode != null && retrunInfo.ResultCode.Contains("300"))
                    ServerSetting.IsOverDue = true;
                errorMg = retrunInfo;
                return true;
                
            }
            catch 
            {
                retrunInfo.result = "未连接服务器";
                retrunInfo.TrueOrFalse = false;
                errorMg = retrunInfo;
                return false;
            }
        }
    }
}
