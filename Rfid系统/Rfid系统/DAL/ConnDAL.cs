using Rfid系统.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.DAL
{
    public static class VerificationConn
    {
        

        public static bool GetVerification()
        {
            try
            {
                string url = string.Format("{0}{1}", string.Format(@"http://{0}:{1}/",ServerSetting.ServerIP,ServerSetting.ServerPort), "equipmentmodule/verifyConnection/getConnection");
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                if (!jsonResult.ToString().ToLower().Contains("row"))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
