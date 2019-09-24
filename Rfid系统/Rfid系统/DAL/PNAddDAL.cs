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
    public class PNAddDAL
    {
        public bool PNAdd(ref object errorMsg)
        {
            try
            {
                PNInfo info = errorMsg as PNInfo;
                string url = string.Format("{0}{1}",ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/addPN");
                Dictionary<string, object> keyValues = errorMsg as Dictionary<string, object>;
               
                Http http = new Http(url,keyValues);
                errorMsg = http.HttpPosts();
                DealJson(ref errorMsg);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        public void DealJson(ref object errorMsg)
        {
            RetrunInfo info = new RetrunInfo();
            try
            {
                var result = JToken.Parse(errorMsg.ToString());
                if (result["state"].ToString().Equals("false"))
                    info.TrueOrFalse = false;
                else
                    info.TrueOrFalse = true;
                info.result = result["msg"].ToString();
            }
            catch 
            {
                info.TrueOrFalse = false;
                info.result = "未连接服务器";
            }
            errorMsg = info;
        }
    }
}
