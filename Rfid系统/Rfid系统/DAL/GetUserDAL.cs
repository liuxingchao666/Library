using Newtonsoft.Json.Linq;
using Rfid系统.BLL;
using Rfid系统.DAL;
using System;
using System.Collections.Generic;

namespace Rfidϵͳ.DAL
{
    public static class GetUserDAL
    {
        public static void GetUser()
        {
            try
            {
                string url = string.Format("{0}{1}", ServerSetting.UrlPath, "authmodule/personalCore/getNowUser");
                Http http = new Http(url, null);
                GetUserDAL.DealJson(http.HttpGet(url));
            }
            catch
            {
            }
        }

        public static void DealJson(object errorMsg)
        {
            try
            {
                JToken p = JToken.Parse(errorMsg.ToString());
                foreach (JToken jToken in ((IEnumerable<JToken>)p))
                {
                    JProperty kens = jToken as JProperty;
                    bool flag = kens.Name.ToLower().Equals("row");
                    if (flag)
                    {
                        foreach (JToken ken in ((IEnumerable<JToken>)kens.Value))
                        {
                            JProperty Ken = ken as JProperty;
                            string a = Ken.Name.ToString().ToLower();
                            if (!(a == "username"))
                            {
                                if (!(a == "idcard"))
                                {
                                    if (!(a == "phone"))
                                    {
                                        if (!(a == "email"))
                                        {
                                            if (a == "fkrolenames")
                                            {
                                                ServerSetting.userInfo.RoleLevel = Ken.Value.ToString();
                                            }
                                        }
                                        else
                                        {
                                            ServerSetting.userInfo.Email = Ken.Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        ServerSetting.userInfo.Phone = Ken.Value.ToString();
                                    }
                                }
                                else
                                {
                                    ServerSetting.userInfo.IDCard = Ken.Value.ToString();
                                }
                            }
                            else
                            {
                                ServerSetting.userInfo.User = Ken.Value.ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
