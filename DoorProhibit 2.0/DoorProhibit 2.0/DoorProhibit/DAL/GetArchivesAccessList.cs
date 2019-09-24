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
    public class GetArchivesAccessList
    {
        /// <summary>
        /// 获取书籍档案进出记录
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
            string url = string.Format(@"http://{0}:{1}/entranceGuard/log/getStoTbEntranceGuard?fkEntranceGuardName=" + Dns.GetHostName() + "&&fkEntranceGuardNum=" + HDid, PublicData.PublicData.serverIp, PublicData.PublicData.serverSite, PublicData.PublicData.EquipmentNume);
            List<BookMessage> list = new List<BookMessage>();
            try
            {
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                return GetBooksResult(jsonResult);
            }
            catch (Exception ex)
            {
                PublicData.PublicData.state = "未连接服务器";
                return list;
            }
        }

        public List<BookMessage> GetBooksResult(object jsonResult)
        {
            List<BookMessage> list = new List<BookMessage>();
            try
            {
                var obj = JObject.Parse(jsonResult.ToString());
                var result = obj["row"].Children().ToList();
                int i = 1;
                foreach (var temp in result)
                {
                    BookMessage message = new BookMessage()
                    {
                        Number = i,
                        FileName = temp["fileName"].ToString(),
                        AlarmTime = Convert.ToDateTime(temp["createTime"].ToString())
                    };
                    list.Add(message);
                    i++;
                }
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }
    }
}
