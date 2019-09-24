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
    public  class ChangePassWordDAL
    {
        public bool ChangePassWord(ref object errorMsg)
        {
            List<string> list = errorMsg as List<string>;
            string url = string.Format(@"{0}{1}",ServerSetting.UrlPath, "authmodule/personalCore/editPassword");
            Dictionary<object, string> valuePairs = new Dictionary<object, string>();
            valuePairs.Add("password",list[0]);
            valuePairs.Add("newPassword", list[1]);
            Http http = new Http(url,valuePairs);
            errorMsg = http.HttpPut();
            return DealJson(ref errorMsg);
        }
        private bool DealJson(ref object errorMsg)
        {
            RetrunInfo retrunInfo = new RetrunInfo();
            try
            {
                var p = JToken.Parse(errorMsg.ToString());
                foreach (JToken jToken in p)
                {
                    var ken = jToken as JProperty;
                    if (ken.Name.ToLower() == "state" && ken.Value.ToString().ToLower() == "true")
                    {
                        retrunInfo.TrueOrFalse = true;
                    }
                    else if (ken.Name.ToLower() == "state" && ken.Value.ToString().ToLower() == "false")
                    {
                        retrunInfo.TrueOrFalse = false;
                    }
                    if (ken.Name.ToLower().Equals("msg"))
                        retrunInfo.result = ken.Value.ToString();
                    if (ken.Name.ToLower().Equals("code"))
                        retrunInfo.ResultCode = ken.Value.ToString();
                    if (ken.Name.ToLower().Equals("msg"))
                        retrunInfo.result = ken.Value.ToString();
                    if (retrunInfo.ResultCode != null && retrunInfo.ResultCode.Contains("300"))
                        ServerSetting.IsOverDue = true;
                }
                errorMsg = retrunInfo;
                return retrunInfo.TrueOrFalse;
            }
            catch 
            {
                retrunInfo.TrueOrFalse = false;
                retrunInfo.result = "未连接服务器";
                errorMsg = retrunInfo;
                return false;
            }
        }
    }
}
