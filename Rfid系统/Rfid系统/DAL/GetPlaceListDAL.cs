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
    public class GetPlaceListDAL
    {
        public bool GetPlaceList(ref object errorMsg)
        {
            string url = string.Format("{0}{1}", ServerSetting.UrlPath, "data/cata/book/bookcollection/currency/getLibName");
            Http http = new Http(url, null);
            errorMsg = http.HttpGet(url);
            return DealJson(ref errorMsg);
        }
        private bool DealJson(ref object errorMsg)
        {
            RetrunInfo retrunInfo = new RetrunInfo() { TrueOrFalse = false };
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
                    if (ken.Name.ToLower().Equals("msg"))
                        retrunInfo.result = ken.Value.ToString();
                    if (ken.Name.ToLower().Equals("code"))
                        retrunInfo.ResultCode = ken.Value.ToString();
                    if (ken.Name.ToLower().Equals("msg"))
                        retrunInfo.result = ken.Value.ToString();
                    if (retrunInfo.ResultCode != null && retrunInfo.ResultCode.Contains("300"))
                        ServerSetting.IsOverDue = true;
                }
                if (!retrunInfo.TrueOrFalse)
                {
                    errorMsg = retrunInfo;
                    return false;
                }
                foreach (JToken jToken in p)
                {
                    var jProperty = jToken as JProperty;
                    if (jProperty.Name.ToLower() == "row")
                    {
                        List<PlaceInfo> placeInfos = new List<PlaceInfo>();
                        foreach (JToken kens in jProperty.Value)
                        {
                            PlaceInfo placeInfo = new PlaceInfo();
                            foreach (JToken ken in kens.Children())
                            {
                                var k = ken as JProperty;
                                switch (k.Name.ToLower())
                                {
                                    case "name":
                                        placeInfo.PlaceName = k.Value.ToString();
                                        break;
                                    case "code":
                                        placeInfo.id = k.Value.ToString();
                                        break;
                                }
                            }
                            placeInfos.Add(placeInfo);
                        }
                        retrunInfo.result = placeInfos;
                        errorMsg = retrunInfo;
                        return true;
                    }
                }
                return true;
            }
            catch 
            {
                retrunInfo.result = "未连接服务器";
                retrunInfo.TrueOrFalse = false;
                errorMsg = retrunInfo;
                return false;
            }
        }
    }
}
