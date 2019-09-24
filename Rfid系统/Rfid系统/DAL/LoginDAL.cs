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
    public class LoginDAL
    {
        public LoginDAL(string account, string passWord)
        {
            this.Account = account;
            this.PassWord = passWord;
        }
        /// <summary>
        /// 登陆账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string PassWord { get; set; }
        public bool GetLoginResult(ref object errorMsg)
        {
            string url = string.Format(@"{0}{1}", ServerSetting.UrlPath, "authmodule/index/rfidLogin");
            Dictionary<string, object> valuePairs = new Dictionary<string, object>();
            valuePairs.Add("account", Account);
            valuePairs.Add("password", PassWord);
            Http http = new Http(url, valuePairs);
            errorMsg = http.HttpPosts();
            return DealJson(ref errorMsg);
        }
        private bool DealJson(ref object errorMsg)
        {
            try
            {
                bool result = false;
                string code="";
                var p = JToken.Parse(errorMsg.ToString());
                RetrunInfo info = new RetrunInfo();
                string a = p["row"].ToString();
                foreach (JToken jToken in p)
                {
                    var ken = jToken as JProperty;
                    if (ken.Name.ToLower() == "state" && ken.Value.ToString().ToLower() == "true")
                    {
                        result = true;
                    }
                    else if (ken.Name.ToLower() == "state" && ken.Value.ToString().ToLower() == "false")
                    {
                        result = false;
                    }
                    else if (ken.Name.ToLower().Equals("code"))
                        code = ken.Value.ToString();
                    else if (ken.Name.ToLower().Equals("msg"))
                        info.result = ken.Value.ToString();
                }
                info.ResultCode = code;
                info.TrueOrFalse = result;
                if (!result)
                {
                    errorMsg = info;
                    return result;
                }
                foreach (JToken jToken in p)
                {
                    var kens = jToken as JProperty;
                    if (kens.Name.ToLower().Equals("row"))
                    {
                        foreach (JToken ken in kens.Value)
                        {
                            var Ken = ken as JProperty;
                            if (Ken.Name.ToLower().Equals("authorization"))
                                ServerSetting.Authorization = Ken.Value.ToString();
                            if (Ken.Name.ToLower().ToLower().Equals("authtbmanager"))
                            {
                                LoginInfo loginInfo = new LoginInfo();
                                loginInfo.UpdateDate = DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss");
                                foreach (JToken j in Ken.Value)
                                {
                                    var jp = j as JProperty;
                                    switch (jp.Name.ToLower())
                                    {
                                        case "username":
                                            loginInfo.UserName = jp.Value.ToString();
                                            ServerSetting.UserName = jp.Value.ToString();
                                            break;
                                        case "headeraddress":
                                            loginInfo.HeaderAddress = jp.Value.ToString();
                                            ServerSetting.HeaderAddress = jp.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                info.result = loginInfo;
                            }
                        }
                    }
                }
                errorMsg = info;
                return result;
            }
            catch 
            {
                errorMsg = "未连接服务器";
                return false;
            }
        }
    }
}
