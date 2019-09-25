using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace Microlibrary.BLL
{
    public class HttpBLL
    {
        public static HttpWebRequest request = null;

        public static HttpWebResponse response = null;

        public static string Url
        {
            get;
            set;
        }

        public static object Parameter
        {
            get;
            set;
        }

        public HttpBLL(string url, object parameter)
        {
            HttpBLL.Url = url;
            HttpBLL.Parameter = parameter;
        }

        public string HttpPosts()
        {
            string result;
            try
            {
                HttpBLL.request = HttpBLL.GreateHttpResquest();
                HttpBLL.response = (HttpWebResponse)HttpBLL.request.GetResponse();
                Stream receiveStream = HttpBLL.response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string responseMsg = readStream.ReadToEnd();
                HttpBLL.response.Close();
                receiveStream.Close();
                readStream.Close();
                result = responseMsg;
            }
            catch
            {
                result = "操作超时";
            }
            return result;
        }

        public string HttpPut()
        {
            string result;
            try
            {
                HttpBLL.request = HttpBLL.GreateHttpResquests();
                HttpBLL.response = (HttpWebResponse)HttpBLL.request.GetResponse();
                Stream receiveStream = HttpBLL.response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string responseMsg = readStream.ReadToEnd();
                HttpBLL.response.Close();
                receiveStream.Close();
                readStream.Close();
                result = responseMsg;
            }
            catch
            {
                result = "操作超时";
            }
            return result;
        }
        public string HttpDelete()
        {
            string result;
            try
            {
                HttpBLL.request = HttpBLL.GreateHttpResquestDel();
                HttpBLL.response = (HttpWebResponse)HttpBLL.request.GetResponse();
                Stream receiveStream = HttpBLL.response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string responseMsg = readStream.ReadToEnd();
                HttpBLL.response.Close();
                receiveStream.Close();
                readStream.Close();
                result = responseMsg;
            }
            catch
            {
                result = "操作超时";
            }
            return result;
        }

        private static HttpWebRequest GreateHttpResquests()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HttpBLL.Url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Timeout = 800;
            string json = JsonConvert.SerializeObject(HttpBLL.Parameter);
            Encoding utf8 = Encoding.UTF8;
            byte[] data = utf8.GetBytes(json);
            request.ContentLength = (long)data.Length;
            try
            {
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
            }
            catch
            {
            }
            return request;
        }
        private static HttpWebRequest GreateHttpResquestDel()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HttpBLL.Url);
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Timeout = 800;

            string json = JsonConvert.SerializeObject(HttpBLL.Parameter);
            Encoding utf8 = Encoding.UTF8;
            byte[] data = utf8.GetBytes(json);
            request.ContentLength = (long)data.Length;
            try
            {
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
            }
            catch
            {
            }
            return request;
        }

        private static HttpWebRequest GreateHttpResquest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HttpBLL.Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Timeout = 800;

            string json = JsonConvert.SerializeObject(HttpBLL.Parameter);
            Encoding utf8 = Encoding.UTF8;
            byte[] data = utf8.GetBytes(json);
            request.ContentLength = (long)data.Length;
            try
            {
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
            }
            catch
            {
            }
            return request;
        }

        public void AppenRequest(ref HttpWebRequest request)
        {
        }

        public string HttpGet(string url)
        {
            string result;
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Timeout = 800;
                this.AppenRequest(ref request);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        public string HttpUp(string filePath, string fileName)
        {
            string ip = "";
            string port = "";
            if (!System.IO.File.Exists(filePath))
                return "文件路径格式有误";
            byte[] bytes;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                bytes = new byte[(int)fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
            }
            WebClient webClient = new WebClient
            {
                Credentials = CredentialCache.DefaultCredentials
            };
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.QueryString["fileName"] = fileName;
            byte[] fileb = webClient.UploadData(new Uri(@"http://" + ip + ":" + port + "/" + fileName + ""), "POST", bytes);
            return Encoding.UTF8.GetString(fileb);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string HttpDown(string url)
        {
            string path = GetPath();
            WebClient webClient = new WebClient();
            if (System.IO.File.Exists(path + "/" + url.Split('/')[3]))
                System.IO.File.Delete(path + "/" + url.Split('/')[3]);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.DownloadFile(new Uri(url), path + "/" + url.Split('/')[3]);
            //或者
            webClient.DownloadFileCompleted += (s, es) =>
            {
                MessageBox.Show("下载成功!");
            };
            webClient.DownloadFileAsync(new Uri(url), path);
            return "下载成功";
        }
        [STAThread]
        static string GetPath()
        {
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog
            {
                Description = "请选择您的文件保存路径"
            };
            saveFileDialog.ShowDialog();
            return saveFileDialog.SelectedPath;
        }
    }
}
