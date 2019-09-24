using DoorProhibit.BLL;
using DoorProhibit.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.DAL
{
    public  class GetNewestAlarmList
    {
        /// <summary>
        /// 获取报警档案记录
        /// </summary>
        public List<BookMessage> GetNewestMessage()
        {
            string HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            string url = string.Format(@"http://{0}:{1}/entranceGuard/log/getStoTbEntranceGuardAlarm?fkEntranceGuardName="+Dns.GetHostName()+ "&&fkEntranceGuardNum="+ HDid, PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            List<BookMessage> list = new List<BookMessage>();
            try
            {
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                var result = JToken.Parse(jsonResult.ToString());
                var row = result["row"].Children().ToList();
                List<BookMessage> books = new List<BookMessage>();
                int i = 1;
                foreach (var temp in row)
                {
                    BookMessage bookMessage = new BookMessage()
                    {
                        AlarmTime=Convert.ToDateTime(temp["createTime"].ToString()),
                        FileName = temp["fkFileName"].ToString(),
                        Number = i
                    };
                    books.Add(bookMessage);
                    i++;
                }
                return books;
            }
            catch
            {
                PublicData.PublicData.state = "未连接服务器";
                return list;
            }
        }
    }
}
