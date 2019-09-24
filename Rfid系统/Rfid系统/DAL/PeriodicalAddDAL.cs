using Newtonsoft.Json.Linq;
using Rfid系统.BLL;
using Rfid系统.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.DAL
{
/// <summary>
/// 典藏期刊添加
/// </summary>
    public class PeriodicalAddDAL
    {
        public bool PeriodicalAdd(ref object errorMsg)
        {
            try
            {
                PeriodicalInfo info = errorMsg as PeriodicalInfo;
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/rfidAdd");
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                keyValues.Add("available", info.available);
                keyValues.Add("lendingPermission", info.lendingPermission);
                keyValues.Add("callNumber", info.callNumber);
                keyValues.Add("code", info.code);
                keyValues.Add("placeCode", info.placeId);
                keyValues.Add("fkCataPeriodicalId", info.fkCataPeriodicalId);
                keyValues.Add("pNumberId", info.pNumberId);
                keyValues.Add("rfid", info.rfid);
                Http http = new Http(url, keyValues);
                errorMsg = http.HttpPosts();
                DealJson(ref errorMsg);
                return true;
            }
            catch 
            {
                errorMsg = "未连接服务器";
                return false;
            }
        }
        public void DealJson(ref object errorMsg)
        {
            RetrunInfo info = new RetrunInfo();
            try
            {
                var result = JToken.Parse(errorMsg.ToString());
                if (result["state"].ToString().ToLower().Equals("false"))
                    info.TrueOrFalse = false;
                else
                    info.TrueOrFalse = true;
                info.result = result["msg"].ToString();
            }
            catch (Exception ex)
            {
                info.TrueOrFalse = false;
                info.result = ex.Message;
            }
            errorMsg = info;
        }
    }
}
