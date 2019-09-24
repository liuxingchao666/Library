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
     public class SelectLocalDAL
    {
        public bool SelectLoacl(ref object errorMsg)
        {
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/selectLocal?queryIssn=" + errorMsg.ToString());
                Http http = new Http(url,null);
                errorMsg = http.HttpGet(url);
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
            RetrunInfo retrunInfo = new RetrunInfo();
            try
            {
                var result = JToken.Parse(errorMsg.ToString());
                retrunInfo.ResultCode = result["code"].ToString();
                if (retrunInfo.ResultCode.Contains("300"))
                    ServerSetting.IsOverDue = true;
                if (result["state"].ToString().ToLower().Equals("false"))
                {
                    retrunInfo.TrueOrFalse = false;
                    retrunInfo.result = result["msg"].ToString();
                    errorMsg = retrunInfo;
                }
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    int count = result["row"].ToList().Count;
                    List<PeriodicalsInfo> infos = new List<PeriodicalsInfo>();
                    for (int i = 0; i < count; i++)
                    {
                        PeriodicalsInfo periodicalsInfo = new PeriodicalsInfo()
                        {
                            Number = i + 1,
                            id = result["row"][i]["id"].ToString(),
                            issn = result["row"][i]["issn"].ToString(),
                           // issnPrice = result["row"][i]["issnPrice"].ToString(),
                            author = result["row"][i]["author"].ToString(),
                            name =result["row"][i]["name"].ToString(),
                            fkTypeName=result["row"][i]["fkTypeName"].ToString(),
                            fkPressName=result["row"][i]["fkPressName"].ToString(),
                            releaseCycle =result["row"][i]["releaseCycle"].ToString(),
                            unifyNum = result["row"][i]["unifyNum"].ToString(),
                            fkTypeCode = result["row"][i]["fkTypeCode"].ToString(),
                            postIssueNumber=result["row"][i]["postIssueNumber"].ToString(),
                            openBook = result["row"][i]["openBook"].ToString(),
                            remark = result["row"][i]["remark"].ToString(),
                            parallelTitle = result["row"][i]["parallelTitle"].ToString()
                        };
                        infos.Add(periodicalsInfo);
                    }
                    retrunInfo.result = infos;
                    retrunInfo.TrueOrFalse = true;
                    errorMsg = retrunInfo;
                }
            }
            catch 
            {
                retrunInfo.TrueOrFalse = false;
                retrunInfo.result = "未连接服务器";
                errorMsg = retrunInfo;
            }
        }
    }
}
