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
    public class GetHDDCQKDAL
    {
        public bool GetHDDCQK(ref object errorMsg)
        {
            try
            {
                RetrunInfo info = new RetrunInfo();
                double pageSize = Math.Pow(9,10);
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/getHDDCQK?cataPeriodicalId=" + errorMsg.ToString());
                
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url).ToString());
                info.ResultCode = result["code"].ToString();
                if (info.ResultCode.Contains("300"))
                    ServerSetting.IsOverDue = true;
                info.result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("false"))
                    info.TrueOrFalse = false;
                else
                {
                    info.TrueOrFalse = true;
                    List<HDDCQKInfo> list = new List<HDDCQKInfo>();
                    int count = result["row"].ToList().Count;
                    for (int i = 0; i < count; i++)
                    {
                        HDDCQKInfo hDDCQKInfo = new HDDCQKInfo() {
                            IsCheck =false,
                            number =i+1,
                            id = result["row"][i]["id"].ToString(),
                            code = result["row"][i]["code"].ToString(),
                            lendState = result["row"][i]["lendState"].ToString(),
                            callNumber =result["row"][i]["callNumber"].ToString(),
                            price = result["row"][i]["price"].ToString(),
                            anumber = result["row"][i]["anumber"].ToString(),
                            snumber =result["row"][i]["snumber"].ToString()
                        };
                        list.Add(hDDCQKInfo);
                    }
                    info.result = list;
                }
                errorMsg = info;
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
