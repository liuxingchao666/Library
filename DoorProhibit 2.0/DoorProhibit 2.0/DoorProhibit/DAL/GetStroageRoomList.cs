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
     public static class GetStroageRoomList
    {
        private static string url= string.Format("http://{0}:{1}/doorguardmodule/doorStore/getAllStoreInfo", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);

        public static List<StroageRoom> GetStroageRooms()
        {
            url = string.Format("http://{0}:{1}/doorguardmodule/doorStore/getAllStoreInfo", PublicData.PublicData.serverIp, PublicData.PublicData.serverSite);
            List<StroageRoom> stroageRooms = new List<StroageRoom>();
            Http http = new Http(url, null);
            object jsonResult = http.HttpGet(url);
            stroageRooms = GetList(jsonResult);
           
            return stroageRooms;
        }
        public static List<StroageRoom> GetList(object obj)
        {
            List<StroageRoom> list = new List<StroageRoom>();
            try
            {
                var result = JObject.Parse(obj.ToString());
                foreach (JToken jToken in result.Children())
                {
                    var p = jToken as JProperty;
                    if (p.Name.ToLower() == "rows")
                    {
                        List<JToken> tokens = p.ToList();
                        foreach (JToken Token in tokens)
                        {
                            List<JToken> jtokens = Token.ToList();
                            foreach (JToken token in jtokens)
                            {
                                List<JToken> ken = token.ToList();
                                StroageRoom room = new StroageRoom();
                                foreach (JToken jk in ken)
                                {
                                    var pp = jk as JProperty;

                                    switch (pp.Name.ToLower())
                                    {
                                        case "storename":
                                            room.StroageRoomName = pp.Value.ToString();
                                            break;
                                        case "id":
                                            room.id = pp.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }

                                }
                                list.Add(room);
                            }
                        }
                        break;
                    }
                }
                return list;
            }
            catch
            {
                return list;
            }
        }
    }
}
