using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.BLL
{
    /// <summary>
    /// 内部类
    /// </summary>
    public  class ErrorLog
    {
        /// <summary>
        /// 文件夹路径
        /// </summary>
        private readonly string FilePath = AppDomain.CurrentDomain.BaseDirectory+"/log";
        private readonly string NowFilePath = AppDomain.CurrentDomain.BaseDirectory + "/log/"+DateTime.Now.ToShortDateString()+"";
        /// <summary>
        /// 日志路径及命名
        /// </summary>
        private string logPath=string.Empty;
        public ErrorLog()
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            if (!Directory.Exists(NowFilePath))
            {
                Directory.CreateDirectory(NowFilePath);
            }
        }
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="url"></param>
        public void AddFile(Exception exception,string url)
        {
            GetLogPath(ref logPath, 0);
            using(FileStream fileStream = new FileStream(logPath,FileMode.CreateNew))
            {
                StreamWriter writer = new StreamWriter(fileStream);
                writer.WriteLine(DateTime.Now);
                writer.WriteLine("出错位置："+url);
                writer.Write(exception.GetType()+":");
                writer.Write(exception.Message + "/r/n");
                fileStream.Unlock(0, fileStream.Length);
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }
        /// <summary>
        /// 递归获取文件路径及名
        /// </summary>
        /// <param name="logpath"></param>
        /// <param name="i"></param>
        public void GetLogPath(ref string logpath,int i)
        {

            logpath = AppDomain.CurrentDomain.BaseDirectory + "/log/" + DateTime.Now.ToShortDateString() + "/"  + i + ".txt";
            if (File.Exists(logpath))
            {
                GetLogPath(ref logpath, ++i);
            }
        }
    }
}
