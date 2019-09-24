using DoorProhibit.BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.DAL
{
     public class floorEditDAL
    {
        public bool floorEdit(ref object errorMsg)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/entranceGuard/log/edit", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
                Dictionary<string, object> keyValues = errorMsg as Dictionary<string, object>;
                Http http = new Http(url, keyValues);
                var result = JToken.Parse(http.HttpPosts());
                if (result["state"].ToString().ToLower().Equals("true"))
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                PublicData.PublicData.state = "未连接服务器";
                return false;
            }
        }
    }
}
