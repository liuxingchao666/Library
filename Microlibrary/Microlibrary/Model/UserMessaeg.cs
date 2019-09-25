using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microlibrary.Model
{
    public class UserMessage
    {
        /// <summary>
        /// 图片
        /// </summary>
        public string PIC { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string age { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string NationName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdentificationCode { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirdTh { get; set; }
        /// <summary>
        /// 办理日期
        /// </summary>
        public DateTime DealDate { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime MaxDate { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 签发部门
        /// </summary>
        public string DealDepartment { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string ZCDate { get; set; }
        /// <summary>
        /// 读书卡卡号
        /// </summary>
        public string UserCard { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 对比相似度
        /// </summary>
        public float Similarity { get; set; } = 0;
        /// <summary>
        /// 用户状态
        /// </summary>
        public string constate { get; set; }
        /// <summary>
        /// 是否逾期
        /// </summary>
        public string overdueName { get; set; }
        /// <summary>
        /// 用户状态码
        /// </summary>
        public string CardState { get; set; }
        /// <summary>
        /// 逾期状态码
        /// </summary>
        public string OverDueState { get; set; }
        /// <summary>
        /// 用户是否失信
        /// </summary>
        public string State { get; set; }
    }
    public class ImageInfo
    {
        /// <summary>
        /// 图片的像素数据
        /// </summary>
        public IntPtr imgData { get; set; }

        /// <summary>
        /// 图片像素宽
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 图片像素高
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        public int format { get; set; }
    }
}
