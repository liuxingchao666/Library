using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.BLL
{
    public class Http
    {
        /// <summary>
        /// 路劲
        /// </summary>
        public  string Url { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public  object Parameter { get; set; }
        public Http(string url, object parameter)
        {
            Url = url;
            Parameter = parameter;
        }

        public static HttpWebRequest request = null;
        public static HttpWebResponse response = null;
        public string HttpPosts()
        {
            try
            {
                //获取请求对象
                request = GreateHttpResquest();
             
                //获取响应对象
                response = (HttpWebResponse)request.GetResponse();

                //获得响应流
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                //获取响应流信息
                string responseMsg = readStream.ReadToEnd();
                response.Close();
                receiveStream.Close();
                readStream.Close();

                return responseMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private  HttpWebRequest GreateHttpResquest()
        {
         
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Timeout = 500;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Parameter);
            //data
            Encoding utf8 = Encoding.UTF8;
            byte[] data = utf8.GetBytes(json);
            request.ContentLength = data.Length;

            //把请求数据 写入请求流中
            try
            {
                
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                return request;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string HttpGet(string url)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Timeout = 500;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
