using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.BLL
{
    public class NewPrint
    {
        /// <summary>
        /// 打印类型及机器
        /// </summary>
        /// <param name="type">连接打印机类型</param>
        /// <param name="index">第几个打印机</param>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll")]
        public static extern bool TxOpenPrinter(int type,int index);
        /// <summary>
        /// 获取打印机状态
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll")]
        public static extern int TxGetStatus();
        /// <summary>
        /// 关闭打印机
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll")]
        public static extern void TxClosePrinter();
        /// <summary>
        /// 初始化打印机
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll")]
        public static extern void TxInit();
        /// <summary>
        /// 打印内容
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll")]
        public static extern void TxOutputString(string context);
    }
    public class Print
    {
        public Print()
        {
            if (NewPrint.TxOpenPrinter(1, 3))
            {
                NewPrint.TxInit();
               
            }
        }
    }
}
