using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.BLL
{
    public static  class NewHairpin
    {
        /// <summary>
        /// 以相对波特率打开串口
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        [DllImport("D1801_DLL.dll", EntryPoint = "D1801_CommOpenWithBaud")]
        public static extern IntPtr D1801_CommOpenWithBaud(int index, int route);
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("D1801_DLL.dll", EntryPoint = "D1801_CommClose")]
        public static extern int D1801_CommClose(IntPtr ComHandle);
        /// <summary>
        /// 连接获取机械版本型号信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("D1801_DLL.dll", EntryPoint = "D1801_GetSysVersion")]
        public static extern int D1801_GetSysVersion(IntPtr ComHandle, int macAddr, out string version);
        /// <summary>
        /// 获取机械状态码
        /// </summary>
        /// <param name="index"></param>
        /// <param name="macAddr"></param>
        /// <param name="queryinfo"></param>
        /// <returns></returns>
        [DllImport("D1801_DLL.dll", EntryPoint = "D1801_Query")]
        public static extern int D1801_Query(IntPtr ComHandle, int macAddr, out byte[] infos);
        /// <summary>
        /// 操作命令
        /// </summary>
        /// <param name="index"></param>
        /// <param name="macAddr"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        [DllImport("D1801_DLL.dll", EntryPoint = "D1801_SendCmd")]
        public static extern int D1801_SendCmd(IntPtr ComHandle, int macAddr, string code, int codeLenght);
    }
}
