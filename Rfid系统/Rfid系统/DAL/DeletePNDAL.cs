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
     public class DeletePNDAL
    {
        public bool DeletePN(ref object errorMsg)
        {
            try
            {
                RetrunInfo retrunInfo = new RetrunInfo();
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/deletePN");
                List<string> list = errorMsg as List<string>;
                Dictionary<string, object> valuePairs = new Dictionary<string, object>();
                valuePairs.Add("ids", list.ToArray());
                Http http = new Http(url, valuePairs);
                var result = JToken.Parse(http.HttpDelete());
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
