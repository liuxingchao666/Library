using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统.Model
{
     public class PeriodicalsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 合订期刊id
        /// </summary>
       public string id { get; set; }
        /// <summary>
        /// 编目期刊id
        /// </summary>
        public string fkCataPeriodicalId { get; set; }       
        /// <summary>
                                           
            /// isbn
                                           
            /// </summary>
        public string issn { get; set; }
        /// <summary>
        /// 期刊名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 主编
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 参考单价
        /// 
        /// </summary>
        public string issnPrice { get; set; }
        /// <summary>
        /// 发行单位
        /// </summary>
        public string fkPressName { get; set; }
        /// <summary>
        /// 分类号
        /// </summary>
        public string fkTypeCode { get; set; }
        /// <summary>
        /// 分类号名
        /// </summary>
        public string fkTypeName { get; set; }
        /// <summary>
        /// 发行周期
        /// </summary>
        public string releaseCycle { get; set; }
        /// <summary>
        /// 同一刊号 
        /// </summary>
        public string unifyNum { get; set; }
        /// <summary>
        /// 邮发代号
        /// </summary>
        public string postIssueNumber { get; set; }
        /// <summary>
        /// 参考开本
        /// </summary>
        public string openBook { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 并列题名
        /// </summary>
        public string parallelTitle { get; set; }
        /// <summary>
        /// 刊期列表
        /// </summary>
        public List<HDDCQKInfo> pNInfos { get; set; }
        /// <summary>
        /// 典藏信息
        /// </summary>
        public PeriodicalCollectionInfo collectionInfo { get; set; }
        /// <summary>
        /// 刊期号
        /// </summary>
        public PNInfo pNInfo { get; set; }
    }
}
