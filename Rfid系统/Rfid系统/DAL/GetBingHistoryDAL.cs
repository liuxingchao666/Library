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
    public class GetBingHistoryDAL
    {
        public bool GetBingHistory(ref object errorMsg)
        {
            try
            {
                RetrunInfo retrunInfo = new RetrunInfo();
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/query/selectBookBingLog");
                Dictionary<string, object> keyValuePairs = errorMsg as Dictionary<string, object>;
                Http http = new Http(url, keyValuePairs);
                var result = JToken.Parse(http.HttpPosts());
                retrunInfo.result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("false"))
                    retrunInfo.TrueOrFalse = false;
                else
                {
                    retrunInfo.TrueOrFalse = true;
                    var value = JToken.Parse(result["row"].ToString());
                    List<BingHistoryInfo> infos = new List<BingHistoryInfo>();
                    foreach (var temp in value.Children())
                    {
                        BingHistoryInfo info = new BingHistoryInfo()
                        {
                            name = temp["name"].ToString(),
                            code = temp["code"].ToString(),
                            rfid = temp["rfid"].ToString(),
                            callNumber = temp["cellNumber"].ToString(),
                            person = temp["person"].ToString(),
                            createTime = temp["createTime"].ToString(),
                            state = temp["state"].ToString() == "0" ? "失败" : "成功"
                        };
                        infos.Add(info);
                    }
                    retrunInfo.result = infos;
                    retrunInfo.page = result["total"].ToString().ToInt() / 10;
                    if (result["total"].ToString().ToInt() % 10 != 0)
                        retrunInfo.page++;
                }
                
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
