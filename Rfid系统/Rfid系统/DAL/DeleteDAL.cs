using Newtonsoft.Json.Linq;
using Rfid系统.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.DAL
{
    public class DeleteDAL
    {
        public bool Delete(ref object errorMsg)
        {
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "periodicalmodule/rFIDPeriodicalr/delete");
                Dictionary<string, object> keyValues = new Dictionary<string, object>() { { "codes ", errorMsg } };
                Http http = new Http(url, keyValues);
                errorMsg = http.HttpPut();
                var result = JToken.Parse(errorMsg.ToString());
                errorMsg = result["msg"].ToString();
                if (result["state"].ToString().Equals("true"))
                    return true;
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
