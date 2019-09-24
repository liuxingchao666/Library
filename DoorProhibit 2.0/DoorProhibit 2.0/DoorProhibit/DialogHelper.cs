using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace DoorProhibit
{
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
