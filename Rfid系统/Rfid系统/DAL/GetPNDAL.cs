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
     public class GetPNDAL
    {
        public bool GetPN(ref object errorMsg)
        {
            RetrunInfo retrunInfo = new RetrunInfo();
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/getPN?cataPeriodicalId=" + errorMsg.ToString());
                Http http = new Http(url, null);
                errorMsg = http.HttpGet(url);
                var result = JToken.Parse(errorMsg.ToString());
                errorMsg = result["msg"].ToString();
                retrunInfo.ResultCode= result["code"].ToString();
                if (retrunInfo.ResultCode.Contains("300"))
                    ServerSetting.IsOverDue = true;
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    retrunInfo.TrueOrFalse = true;
                    List<PNInfo> infos = new List<PNInfo>();
                    int count = result["row"].Children().ToList().Count;
                    for (int i = 0; i < count; i++)
                    {
                        PNInfo info = new PNInfo()
                        {
                            aNumber=result["row"][i]["aNumber"].ToString(),
                            sNumber = result["row"][i]["sNumber"].ToString(),
                            fkCataPeriodicalId =result["row"][i]["id"].ToString(),
                            page = result["row"][i]["page"].ToString(),
                            price = result["row"][i]["price"].ToString(),
                            publicationDateStr = result["row"][i]["publicationDateStr"].ToString(),
                            remark = result["row"][i]["remark"].ToString()
                        };
                        infos.Add(info);
                    }
                    retrunInfo.result = infos;
                }
                if (result["state"].ToString().ToLower().Equals("false"))
                {
                    retrunInfo.TrueOrFalse = false;
                    retrunInfo.result = result["msg"].ToString();
                }
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
