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
     public class SelectOneDAL
    {
        public bool SelectOne(ref object errorMsg)
        {
            try
            {
                RetrunInfo retrunInfo = new RetrunInfo();
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "rfidmodule/rFIDPeriodicalr/selectOne?id=" + errorMsg.ToString());
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));

                errorMsg = result["msg"].ToString();
                retrunInfo.ResultCode = result["code"].ToString();
                if (retrunInfo.ResultCode.Contains("300"))
                    ServerSetting.IsOverDue = true;
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    retrunInfo.TrueOrFalse = true;
                    PeriodicalsInfo info = new PeriodicalsInfo()
                    {
                        id = result["row"]["cataTbPeriodicalInfo"]["id"].ToString(),
                        issn = result["row"]["cataTbPeriodicalInfo"]["issn"].ToString(),
                       // issnPrice = result["row"]["cataTbPeriodicalInfo"]["issnPrice"].ToString(),
                        author = result["row"]["cataTbPeriodicalInfo"]["author"].ToString(),
                        name = result["row"]["cataTbPeriodicalInfo"]["name"].ToString(),
                        fkTypeName = result["row"]["cataTbPeriodicalInfo"]["fkTypeName"].ToString(),
                        fkPressName = result["row"]["cataTbPeriodicalInfo"]["fkPressName"].ToString(),
                        releaseCycle = result["row"]["cataTbPeriodicalInfo"]["releaseCycle"].ToString(),
                        unifyNum = result["row"]["cataTbPeriodicalInfo"]["unifyNum"].ToString(),
                        fkTypeCode = result["row"]["cataTbPeriodicalInfo"]["fkTypeCode"].ToString(),
                        postIssueNumber = result["row"]["cataTbPeriodicalInfo"]["postIssueNumber"].ToString(),
                        openBook = result["row"]["cataTbPeriodicalInfo"]["openBook"].ToString(),
                        remark = result["row"]["cataTbPeriodicalInfo"]["remark"].ToString(),
                        parallelTitle = result["row"]["cataTbPeriodicalInfo"]["parallelTitle"].ToString()

                    };
                    PeriodicalCollectionInfo periodicalCollectionInfo = new PeriodicalCollectionInfo()
                    {
                        id = result["row"]["id"].ToString(),
                        code = result["row"]["code"].ToString(),
                        callNumber = result["row"]["callNumber"].ToString(),
                        available = result["row"]["available"].ToString(),
                        lendingPermission = result["row"]["lendingPermission"].ToString(),
                        hkPrice = result["row"]["hkPrice"].ToString(),
                        hkRemark = result["row"]["hkRemark"].ToString(),
                        placeCode = result["row"]["placeCode"].ToString(),
                        RFID = result["row"]["rfid"].ToString()
                    };
                    info.collectionInfo = periodicalCollectionInfo;
                    PNInfo pNInfo = new PNInfo()
                    {
                        fkCataPeriodicalId=result["row"]["periodicalTbNumber"]["id"].ToString(),
                        aNumber = result["row"]["periodicalTbNumber"]["aNumber"].ToString(),
                        sNumber = result["row"]["periodicalTbNumber"]["sNumber"].ToString(),
                        price = result["row"]["periodicalTbNumber"]["price"].ToString(),
                        page = result["row"]["periodicalTbNumber"]["page"].ToString(),
                        publicationDateStr =result["row"]["periodicalTbNumber"]["publicationDate"].ToString(),
                        remark =result["row"]["periodicalTbNumber"]["remark"].ToString()
                    };
                    info.pNInfo = pNInfo;
                    retrunInfo.result = info;
                    errorMsg = retrunInfo;
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
