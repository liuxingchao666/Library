using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using WpfApp2.BLL;
using WpfApp2.Model;

namespace WpfApp2.DAL
{
    public static class VerificationConn
    {
        public static bool GetVerification()
        {
            try
            {
                string url = string.Format("http://{0}:{1}/{2}", ServerSeting.ServerIP, ServerSeting.ServerSite, "equipmentmodule/verifyConnection/getConnection");
                Http http = new Http(url, null);
                object jsonResult = http.HttpGet(url);
                if (!jsonResult.ToString().ToLower().Contains("row"))
                {
                    ServerSeting.connState = false;
                    return false;
                }
                if (ServerSeting.isGit)
                {
                    ServerSeting.isGit = false;
                    Task.Run(() =>
                    {
                        try
                        {
                            DirectoryInfo directory = new DirectoryInfo("错误日志");
                            DirectoryInfo[] files = directory.GetDirectories();
                            if (files.Length == 0)
                            {

                            }
                            else
                            {
                                string filePath = "";
                                foreach (DirectoryInfo file in files.ToList())
                                {
                                    if (file.Name.ToInt() > filePath.ToInt())
                                        filePath = file.Name;
                                }
                                filePath = "错误日志/" + filePath;
                                directory = new DirectoryInfo(filePath);
                                files = directory.GetDirectories();
                                string temp = "";
                                foreach (DirectoryInfo file in files.ToList())
                                {
                                    if (file.Name.ToInt() > temp.ToInt())
                                        temp = file.Name;
                                }
                                filePath = filePath + "/" + temp;
                                string[] lists = Directory.GetFiles(filePath);
                                string path = "";
                                string tempPath = "";
                                DateTime date = DateTime.Now;
                                FileInfo fileInfo;
                                foreach (string file in lists.ToList())
                                {
                                    tempPath = filePath + "/" + file;
                                    if (string.IsNullOrEmpty(path))
                                    {
                                        fileInfo = new FileInfo(tempPath);
                                        date = fileInfo.CreationTime;
                                        path = filePath + "/" + fileInfo.Name;
                                    }
                                    else
                                    {
                                        fileInfo = new FileInfo(tempPath);
                                        if (fileInfo.CreationTime > date)
                                        {
                                            path = filePath + "/" + fileInfo.Name; ;
                                            date = fileInfo.CreationTime;
                                        }
                                    }
                                }
                                try
                                {
                                    XmlDocument document = new XmlDocument();
                                    document.Load(path);

                                    //遍历Name
                                    XmlNode xml = document.SelectSingleNode("人员档案");
                                    XmlNodeList list = xml.ChildNodes;
                                    List<GitInfo> gitInfos = new List<GitInfo>();
                                    foreach (XmlNode node in list)
                                    {
                                        if (node.Attributes[0].Name == "orderNumber" && !string.IsNullOrEmpty(node.Attributes[0].Value))
                                        {

                                            XmlNodeList nodeList = node.ChildNodes;
                                            GitInfo info = new GitInfo() { orderNumber = node.Attributes[0].Value };
                                            node.Attributes[0].Value = "";
                                            foreach (XmlNode xmlNode in nodeList)
                                            {
                                                switch (xmlNode.Name)
                                                {
                                                    case "设备名称":
                                                        info.equipmentName = xmlNode.InnerText;
                                                        break;
                                                    case "设备编码":
                                                        info.equipmentCode = xmlNode.InnerText;
                                                        break;
                                                    case "交易时间":
                                                        info.createTime = xmlNode.InnerText;
                                                        break;
                                                    case "收取金额":
                                                        info.money = xmlNode.InnerText.ToInt();
                                                        break;
                                                    case "失败原因":
                                                        info.reason = xmlNode.InnerText;
                                                        break;
                                                }
                                            }
                                            gitInfos.Add(info);
                                        }
                                    }
                                    document.Save(path);
                                     url = string.Format("{0}/equipmentmodule/readerTbCardInfo/currency/addEquipmentTbOrders", ServerSeting.urlPath);
                                    Http http_s = new Http(url, gitInfos);
                                    var result = JToken.Parse(http_s.HttpPosts());
                                    var row = result["row"].Children();
                                    List<GitInfo> infos = new List<GitInfo>();
                                    foreach (JToken token in row)
                                    {
                                        foreach (GitInfo info in gitInfos)
                                        {
                                            if (info.orderNumber == token["orderNumber"].ToString())
                                            {
                                                info.Gitreason = token["reason"].ToString();
                                                info.GitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                infos.Add(info);
                                            }
                                        }
                                    }
                                    CreateLog(infos);
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch { }
                    });

                    return true;
                }
                return true;
            }
            catch
            {
                ServerSeting.connState = false;
                return false;
            }
        }
        public static void CreateLog(List<GitInfo> infos)
        {
            if (!Directory.Exists("上传错误日志/" + DateTime.Now.Year))
                Directory.CreateDirectory("上传错误日志/" + DateTime.Now.Year);
            if (!Directory.Exists("上传错误日志/" + DateTime.Now.Year + "/" + DateTime.Now.Month))
                Directory.CreateDirectory("上传错误日志/" + DateTime.Now.Year + "/" + DateTime.Now.Month);
            string path = "上传错误日志/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
            XmlDocument documentt = new XmlDocument();
            if (!File.Exists(path))
            {
                XmlDeclaration header = documentt.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement elementt = documentt.CreateElement("人员档案");
                documentt.AppendChild(elementt);
                documentt.Save(path);
            }
          XmlDocument  document = new XmlDocument();
            document.Load(path);
            foreach (GitInfo info in infos)
            {
                XmlNode xml = document.SelectSingleNode("人员档案");
                XmlElement elements = document.CreateElement("订单号");
                elements.SetAttribute("orderNumber", info.orderNumber);
                xml.AppendChild(elements);

                XmlElement element2 = document.CreateElement("设备编码");
                element2.InnerText = info.equipmentCode;
                elements.AppendChild(element2);

                XmlElement element3 = document.CreateElement("设备名称");
                element3.InnerText = info.equipmentName;
                elements.AppendChild(element3);

                XmlElement element4 = document.CreateElement("交易时间");
                element4.InnerText = info.createTime;
                elements.AppendChild(element4);

                XmlElement element1 = document.CreateElement("收取金额");
                element1.InnerText = info.money.ToString();
                elements.AppendChild(element1);

                XmlElement element5 = document.CreateElement("失败原因");
                element5.InnerText = info.reason;
                elements.AppendChild(element5);

                XmlElement element6 = document.CreateElement("上传时间");
                element6.InnerText = info.GitTime;
                elements.AppendChild(element6);

                XmlElement element7 = document.CreateElement("上传失败原因");
                element7.InnerText = info.Gitreason;
                elements.AppendChild(element7);
            }
            document.Save(path);
            
        }
    }
}

