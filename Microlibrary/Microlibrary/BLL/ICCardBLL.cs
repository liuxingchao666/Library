using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microlibrary.BLL
{
    public static class ICCardBLL
    {
        /// <summary>
        /// 自动获取串口
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_OpenRU")]
        public static extern int RF610_USB_OpenRU(out IntPtr HidHandle);
        /// <summary>
        /// 关闭当前模块
        /// </summary>
        /// <param name="HidHandle"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_CloseRU")]
        public static extern int RF610_USB_CloseRU(out IntPtr HidHandle);
        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <param name="HidHandle"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_GetHardVersion")]
        public static extern int RF610_USB_GetHardVersion(IntPtr HidHandle, out byte[] VerCode);
        /// <summary>
        /// 读卡号
        /// </summary>
        /// <param name="HidHandle"></param>
        /// <returns></returns>
        [DllImport("RF610_DLL.dll", EntryPoint = "RF610_USB_15693GetUid")]
        public static extern int RF610_USB_15693GetUid(IntPtr HidHandle, out byte[] ICcard);
    }
}
