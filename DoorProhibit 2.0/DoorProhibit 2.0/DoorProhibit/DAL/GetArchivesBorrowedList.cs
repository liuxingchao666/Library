using DoorProhibit.BLL;
using DoorProhibit.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.DAL
{
    public  class GetArchivesBorrowedList
    {
        /// <summary>
        /// 获取档案被借出的记录
        /// </summary>
        public string url =string.Format("http://{0}:{1}/doorguardmodule/doorStore/getAllowTakeArchives?currentPage=-1",PublicData.PublicData.serverIp,PublicData.PublicData.serverSite);
        public  List<BookMessage> GetArchivesBorroweds()
        {
            url = string.Format("http://{0}:{1}/doorguardmodule/doorStore/getAllowTakeArchives?currentPage=-1", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            List<BookMessage> list = new List<BookMessage>();
            try
            {
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                return GetBooksResult(jsonResult);
            }
            catch
            {
                PublicData.PublicData.state = "未连接服务器";
                return list;
            }
        }

        public List<BookMessage> GetBooksResult(object jsonResult)
        {
            List<BookMessage> list = new List<BookMessage>();
            var obj = JObject.Parse(jsonResult.ToString());
            foreach (JToken jToken in obj.Children())
            {
                var p = jToken as JProperty;

                if (p.Name == "rows")
                {
                    List<JToken> jTokens = p.Value.ToList();
                    foreach (JToken jT in jTokens)
                    {
                        BookMessage book = new BookMessage();
                        List<JToken> pp = jT.Children().ToList();
                        foreach (JToken jj in pp)
                        {
                            var js = jj as JProperty;
                            switch (js.Name.ToLower())
                            {
                                case "rfid":
                                    book.EPC = js.Value.ToString();
                                    break;
                                case "fileName":
                                    book.FileName = js.Value.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                        list.Add(book);
                    }
                }

            }
            return list;
        }
    }
}
