using DoorProhibit.BLL;
using DoorProhibit.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoorProhibit.DAL
{
    public class AddArchivesInOrOutList
    {
        /// <summary>
        /// 添加书籍档案进出记录
        /// </summary>
        public BookMessage book = new BookMessage();
        public AddArchivesInOrOutList(BookMessage message)
        {
            book = message;
        }
        public void GetArchivesInOrOut()
        {
            string url = string.Format("http://{0}:{1}/entranceGuard/log/arc", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            string HDid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["ProcessorId"].Value;
                break;
            }
            Dictionary<string, string> matchKV = new Dictionary<string, string>();
            matchKV.Add("rfid", book.EPC);
            matchKV.Add("fkEntranceGuardNum", HDid);
            matchKV.Add("fkEntranceGuardName", Dns.GetHostName());
            try
            {
                Http http = new Http(url, matchKV);
                object jsonResult = http.HttpPosts();
            }
            catch
            {
                PublicData.PublicData.state = "未连接服务器";
            }
        }
    }
}
