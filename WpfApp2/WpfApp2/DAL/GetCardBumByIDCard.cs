using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.BLL;

namespace WpfApp2.DAL
{
    public class GetCardBumByIDCard
    {
         
        public bool GetCardnum(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "borrowmodule/InAndOutEquipment/getCardNumByIDCard");
                CardData cardData = errorMsg as CardData;
                url += "?idCard=" + cardData.CardNo;
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                errorMsg = jsonResult;
                return DealJson(ref errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                ServerSeting.connState = false;
                return false;
            }
        }
        public bool DealJson(ref object errorMsg)
        {
            try
            {
                var jtoken = JToken.Parse(errorMsg.ToString());
                bool result = true;
                if (jtoken["state"].ToString().ToLower().Equals("false"))
                {
                    result = false;
                }
                if (result)
                {
                    errorMsg = jtoken["row"].ToString();
                    return result;
                }
                else
                {
                    errorMsg = jtoken["msg"].ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                ServerSeting.connState = false;
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}
