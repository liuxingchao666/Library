using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.Model
{
   public class StroageRoom
    {
        public string StroageRoomName { get; set; }
        public string id { get; set; }
    }
    public class floorInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否已绑定
        /// </summary>
        public bool IsCheck { get; set; }
    }
}
