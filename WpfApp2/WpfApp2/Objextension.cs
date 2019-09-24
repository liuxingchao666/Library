using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WpfApp2.BLL;

namespace WpfApp2
{
     public static class Objextension
    {

        public static string PBEPC { get; set; }
        public static int ToInt(this object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (int.TryParse(obj.ToString(), out int result))
            {
                if (result == 0)
                {
                    result = 1;
                }
                return result;
            }
            return 1;
        }
      
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            if (decimal.TryParse(obj.ToString(), out decimal result))
            {
                return result;
            }
            return 0;
        }
        public static DateTime? ToDate(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (DateTime.TryParse(obj.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }
        public static string ToString1(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            return obj.ToString();
        }
        public static ErrorClass ToError(this object obj)
        {
            ErrorClass error = ErrorClass.commond;
            switch (obj.ToString())
            {
                case "0x4000":
                    error = ErrorClass.await;
                    break;
                case "0x0080":
                    error = ErrorClass.Busy;
                    break;
                case "0x1000":
                    error = ErrorClass.cutterError;
                    break;
                case "0x8000":
                    error = ErrorClass.fastNoPage;
                    break;
                case "0x0020":
                    error = ErrorClass.losePage;
                    break;
                case "0x2000":
                    error = ErrorClass.NonRecoverable;
                    break;
                case "0x0010":
                    error = ErrorClass.onlineState;
                    break;
                case "0x0400":
                    error = ErrorClass.PrintError;
                    break;
                case "0x0800":
                    error = ErrorClass.smallError;
                    break;
                default:
                    break;
            }
            return error;
        }
    }
    public static class DialogHelper
    {
        static Window GetWindowFromHwnd(IntPtr hwnd)
        {
            return (Window)HwndSource.FromHwnd(hwnd).RootVisual;
        }
        //GetForegroundWindow API

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        //调用GetForegroundWindow然后调用GetWindowFromHwnd
        static Window GetTopWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == null)
                return null;
            return GetWindowFromHwnd(hwnd);
        }
        //显示对话框并自动设置Owner，可以理解为子窗体添加父窗体
        public static void ShowDialog(Window win)
        {
            win.Owner = GetTopWindow();
            win.ShowInTaskbar = false;       //false表示不显示新的窗口，默认当前打开窗口为一个子窗口（不会显示两个窗口）
            win.ShowDialog();
        }
    }
}
