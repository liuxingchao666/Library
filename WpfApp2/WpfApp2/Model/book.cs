using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    /// <summary>
    /// 书籍
    /// </summary>
    public class book
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 索书号
        /// </summary>
        public string searchNumber { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 定价
        /// </summary>
        public decimal? price { get; set; }
        /// <summary>
        /// 类型编码
        /// </summary>
        public string fkTypeCode { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string fkTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pageNumber { get; set; }
        /// <summary>
        /// 出版社编码
        /// </summary>
        public string fkPressCode { get; set; }
        /// <summary>
        /// 出版社名称
        /// </summary>
        public string fkPressName { get; set; }
        /// <summary>
        /// 出厂事件
        /// </summary>
        public DateTime? createTime { get; set; }
        /// <summary>
        /// 改制事件
        /// </summary>
        public DateTime? updateTime { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string introduction { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string coverPhotoUrl { get; set; }
    }
    /// <summary>
    /// 处理返回值得到list
    /// </summary>
    public class GetBook
    {
        public object result { get; set; }
        public GetBook(object Res)
        {
            result = Res;
        }
        public List<book> GetBooks()
        {
            List<book> list = new List<book>();
            if (result != null)
            {
                var obja = JObject.Parse(result.ToString());
                foreach (JToken key in obja.Children())
                {
                    var p = key as JProperty;
                    if (p.Name == "row")
                    {
                        foreach (JToken keys in p.Value.Children())
                        {
                            var ps = keys as JProperty;
                            if (ps.Name == "list")
                            {
                                foreach (JToken keyss in ps.Value.Children())
                                {
                                    book book = new book();
                                    List<JToken> bL = keyss.ToList();
                                    foreach (JToken token in bL)
                                    {
                                        var jProperty = token as JProperty;
                                        switch (jProperty.Name)
                                        {
                                            case "id":
                                                book.id = jProperty.Value.ToString();
                                                break;
                                            case "searchNumber":
                                                book.searchNumber = jProperty.Value.ToString();
                                                break;
                                            case "name":
                                                book.name = jProperty.Value.ToString();
                                                break;
                                            case "price":
                                                book.price = jProperty.Value.ToDecimal();
                                                break;
                                            case "fkTypeCode":
                                                book.fkTypeCode = jProperty.Value.ToString();
                                                break;
                                            case "fkTypeName":
                                                book.fkTypeName = jProperty.Value.ToString();
                                                break;
                                            case "pageNumber":
                                                book.pageNumber = jProperty.Value.ToString();
                                                break;
                                            case "fkPressCode":
                                                book.fkPressCode = jProperty.Value.ToString();
                                                break;
                                            case "fkPressName":
                                                book.fkPressName = jProperty.Value.ToString();
                                                break;
                                            case "creatTime":
                                                book.createTime = jProperty.Value.ToDate();
                                                break;
                                            case "updateTime":
                                                book.updateTime = jProperty.Value.ToDate();
                                                break;
                                            case "introduction":
                                                book.introduction = jProperty.Value.ToString();
                                                break;
                                            default:
                                                book.coverPhotoUrl = jProperty.Value.ToString();
                                                break;
                                        }   
                                    }
                                    list.Add(book);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
