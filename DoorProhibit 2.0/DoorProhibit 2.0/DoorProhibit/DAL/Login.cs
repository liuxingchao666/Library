using DoorProhibit.BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.DAL
{
    public static class Login
    {
        public static bool login()
        {
            try
            {
               string url = string.Format("http://{0}:{1}/entranceGuard/log/currency/check", PublicData.PublicData.
                   serverIp, PublicData.PublicData.serverSite);
             
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                var result = JObject.Parse(jsonResult.ToString());
                if (result["state"].ToString().ToLower().Equals("true"))
                    return true;
                return false; 
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
