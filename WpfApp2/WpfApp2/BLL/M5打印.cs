 using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.DAL;
using WpfApp2.Model;

namespace WpfApp2.BLL
{
    public static class M5打印
    {
        /// <summary>
        /// 确认打印机打开方式并打开
        /// </summary>
        /// <param name="type">连接方式</param>
        /// <param name="idx">运行次数</param>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxOpenPrinter")]
        public static extern bool TxOpenPrinter(int type, int idx);
        /// <summary>
        /// 关闭打印机连接
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxClosePrinter")]
        public static extern void TxClosePrinter();
        /// <summary>
        /// 打印机状态获取
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxGetStatus")]
        public static extern int TxGetStatus2();
        /// <summary>
        /// 初始化打印机
        /// </summary>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxInit")]
        public static extern void TxInit();
        /// <summary>
        /// 打印内容（自动换行）
        /// </summary>
        /// <param name="content"></param>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxOutputStringLn")]
        public static extern void TxOutputStringLn(string content);
        /// <summary>
        /// 打印内容
        /// </summary>
        /// <param name="content"></param>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxOutputString")]
        public static extern void TxOutputString(string content);
        /// <summary>
        /// 跳行指令
        /// </summary>
        /// <param name="content"></param>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxNewline")]
        public static extern void TxNewline();
        /// <summary>
        /// 特殊操作
        /// </summary>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxDoFunction")]
        public static extern int TxDoFunction(int func, int param1, int param2);

        [DllImport("TxPrnMod.dll", EntryPoint = "TxWritePrinter")]
        public static extern bool TxWritePrinter(char[] buf, int len);
        /// <summary>
        /// 字体样式恢复
        /// </summary>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxResetFont")]
        public static extern void TxResetFont();
        /// <summary>
        /// 打印条码
        /// </summary>
        [DllImport("TxPrnMod.dll", EntryPoint = "TxPrintBarcode")]
        public static extern bool TxPrintBarcode(int type, string content);
        /// <summary>
        /// 二维码打印
        /// </summary>
        /// <param name="url"></param>
        /// <param name="len"></param>

        [DllImport("TxPrnMod.dll", EntryPoint = "TxPrintQRcode")]
        public static extern void TxPrintQRcode(string url, int len);
        [DllImport("TxPrnMod.dll", EntryPoint = "TxPrintImage")]
        public static extern bool TxPrintImage(string url);
    }
    public class UseM5
    {
        public UseM5(PrintClass @class,PrintParamtClass paramtClass)
        {
            printClass = @class;
            this.paramtClass = paramtClass;
        }
        /// <summary>
        /// 打印类型
        /// </summary>
        public PrintClass printClass;
        public PrintParamtClass paramtClass;
        public string Path { get; set; }
        public List<ArchivesInfo> renewInfos = new List<ArchivesInfo>();
        public bool ConnState(ref object errorMsg)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                if (M5打印.TxOpenPrinter(1, 0))
                {
                        //初始化
                        M5打印.TxInit();
                    //执行的内容
                    ///查询质量
                    int code = M5打印.TxGetStatus2();
                    if (code == 56)
                    {
                        errorMsg = "打印机打印纸张不够，请加纸";
                        return false;
                    }
                    //字体大小
                    //条码打印
                    try
                        {
                        switch (printClass)
                        {
                            case PrintClass.Add:
                                M5打印.TxDoFunction(1, 0, 1);
                                //粗体
                                M5打印.TxDoFunction(3, 1, 0);
                                //中对齐
                                M5打印.TxDoFunction(6, 1, 0);
                                M5打印.TxOutputStringLn("重庆夔牛科技图书馆(办卡)");
                                M5打印.TxDoFunction(10, 40, 0);
                                //设置shang
                                M5打印.TxDoFunction(14, 15, 0);
                                M5打印.TxResetFont();
                                M5打印.TxDoFunction(6, 0, 0);
                                if (!string.IsNullOrEmpty(ServerSeting.OrderNumber))
                                {
                                    M5打印.TxDoFunction(10, 20, 0);
                                    M5打印.TxOutputStringLn("订单号：" + ServerSeting.OrderNumber);
                                }
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("用  户：" + paramtClass.name);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("读者卡号：" + paramtClass.cardNo);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("时  间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                M5打印.TxDoFunction(10, 20, 0);
                                if (paramtClass.costDespoit.toInt() > 0)
                                {
                                    M5打印.TxOutputStringLn("交易类型：现金支付");
                                    M5打印.TxDoFunction(10, 20, 0);
                                    if (paramtClass.SecondDespoit.toInt() == 0)
                                    {
                                        M5打印.TxOutputStringLn("交易金额：" + paramtClass.costDespoit + "  CNY");
                                        M5打印.TxDoFunction(10, 20, 0);
                                    }
                                    else
                                    {
                                        M5打印.TxOutputStringLn("押金金额：" + (paramtClass.costDespoit - paramtClass.SecondDespoit) + "  CNY");
                                        M5打印.TxDoFunction(10, 20, 0);
                                        M5打印.TxOutputStringLn("二次办卡金额：" + paramtClass.SecondDespoit + "  CNY");
                                        M5打印.TxDoFunction(10, 20, 0);
                                    }
                                }
                                M5打印.TxOutputStringLn("联系电话："+paramtClass.phone);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("- - - - - - - - - - - - - - - - - - ");
                                M5打印.TxOutputStringLn(" 此凭条为您的办卡凭证，请妥善保管！");
                                break;
                            case PrintClass.errorLog:
                                M5打印.TxDoFunction(1, 0, 1);
                                //粗体
                                M5打印.TxDoFunction(3, 1, 0);
                                //中对齐
                                M5打印.TxDoFunction(6, 1, 0);
                                M5打印.TxOutputStringLn("重庆夔牛科技图书馆");
                                M5打印.TxDoFunction(10, 40, 0);
                                //设置shang
                                M5打印.TxDoFunction(14, 15, 0);
                                M5打印.TxResetFont();
                              
                                M5打印.TxDoFunction(6, 1, 0);
                                M5打印.TxOutputStringLn("办理读书卡失败退款凭条");
                                M5打印.TxDoFunction(10, 30, 0);
                                M5打印.TxDoFunction(6, 0, 0);
                                M5打印.TxOutputStringLn("订 单 号：" + paramtClass.orderNumber);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("时    间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("用    户：" + paramtClass.name);
                                if (!string.IsNullOrEmpty(paramtClass.cardNo))
                                {
                                    M5打印.TxDoFunction(10, 20, 0);
                                    M5打印.TxOutputStringLn("卡   号：" +paramtClass.cardNo);
                                }
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("交易类型：现金支付");
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("交易详情记录如下：");
                                int count = 0;
                                foreach (TransactionRecordInfo info in ServerSeting.transactionRecordInfos)
                                {
                                    M5打印.TxDoFunction(10, 20, 0);
                                    M5打印.TxOutputStringLn("缴费时间：" + info.DenominationTime.ToString());
                                    M5打印.TxDoFunction(10, 20, 0);
                                    count += info.Denomination;
                                    M5打印.TxOutputStringLn("面    额：" + info.Denomination+" CNY");
                                }
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("缴费总计："+count+" CNY");
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("热线电话： 010-65222882");
                                
                                M5打印.TxOutputStringLn("------------------------------------------");
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("请于"+DateTime.Now.ToShortDateString()+"至"+DateTime.Now.AddDays(3).ToShortDateString()+"前找相关负责人处理订单");
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn(" 此凭条为您的操作凭证，请妥善保管！");
                                break;
                            case PrintClass.Reissue:
                                M5打印.TxDoFunction(1, 0, 1);
                                //粗体
                                M5打印.TxDoFunction(3, 1, 0);
                                //中对齐
                                M5打印.TxDoFunction(6, 1, 0);
                                M5打印.TxOutputStringLn("重庆夔牛科技图书馆（补办）");
                                M5打印.TxDoFunction(10, 40, 0);
                                //设置shang
                                M5打印.TxDoFunction(14, 15, 0);
                                M5打印.TxResetFont();
                                M5打印.TxDoFunction(6, 0, 0);
                                if (!string.IsNullOrEmpty(ServerSeting.OrderNumber))
                                {
                                    M5打印.TxDoFunction(10, 20, 0);
                                    M5打印.TxOutputStringLn("订单号：" + ServerSeting.OrderNumber);
                                }
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("时    间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("用    户：" + paramtClass.name);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("新 卡 号：" + paramtClass.cardNo);
                                M5打印.TxDoFunction(10, 20, 0);
                                if (paramtClass.costDespoit.toInt() > 0)
                                {
                                    M5打印.TxOutputStringLn("交易类型：现金支付");
                                    M5打印.TxDoFunction(10, 20, 0);
                                    M5打印.TxOutputStringLn("补办金额：" + paramtClass.costDespoit + "  CNY");
                                }
                                M5打印.TxOutputStringLn("------------------------------------------");
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn(" 此凭条为您的补办凭证，请妥善保管！");
                                break;
                            case PrintClass.Loss:
                                M5打印.TxOutputStringLn("时    间（DATE TIME）：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                M5打印.TxOutputStringLn("用    户（USER NAME）：" + paramtClass.name);
                                M5打印.TxOutputStringLn("您的读书卡已挂失！！！");
                                M5打印.TxOutputStringLn("联系电话（TELEPHONE）：" + paramtClass.phone);
                                break;
                            case PrintClass.Renew:
                                M5打印.TxDoFunction(1, 0, 1);
                                //粗体
                                M5打印.TxDoFunction(3, 1, 0);
                                //中对齐
                                M5打印.TxDoFunction(6, 1, 0);
                                M5打印.TxOutputStringLn("重庆夔牛科技图书馆（续借）");
                                M5打印.TxDoFunction(10, 40, 0);
                                //设置shang
                                M5打印.TxDoFunction(14, 15, 0);
                                M5打印.TxResetFont();
                                M5打印.TxDoFunction(6, 0, 0);
                                M5打印.TxOutputStringLn("用  户：" + paramtClass.name+"   读者卡号：" +paramtClass.cardNo);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("时  间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("------------------------------------------");
                                M5打印.TxDoFunction(10, 20, 0);
                                int Snum = 0;
                                int Fnum = 0;
                                foreach (ArchivesInfo info in renewInfos)
                                {
                                    M5打印.TxOutputStringLn("条码号：" + info.barcode);
                                    M5打印.TxDoFunction(10, 20, 0);
                                    M5打印.TxOutputStringLn("书  名："+info.ArchivesName);
                                    M5打印.TxDoFunction(10, 20, 0);
                                    if (info.errorMsg != null && info.errorMsg.Contains("成功"))
                                    {
                                        M5打印.TxOutputStringLn("借阅开始时间：" + info.BSTime );
                                        M5打印.TxDoFunction(10, 20, 0);
                                        M5打印.TxOutputStringLn("预计归还时间：" + info.EDTime);
                                        M5打印.TxDoFunction(10, 20, 0);
                                        M5打印.TxOutputStringLn("备  注：" + info.errorMsg);
                                        M5打印.TxDoFunction(10, 20, 0);
                                      
                                        Snum++;
                                    }
                                    else
                                    {
                                        M5打印.TxOutputStringLn("借阅开始时间：" + info.BSTime);
                                        M5打印.TxDoFunction(10, 20, 0);
                                        M5打印.TxOutputStringLn("预计归还时间：" + info.EDTime);
                                        M5打印.TxDoFunction(10, 20, 0);
                                        M5打印.TxOutputStringLn("备  注：" + info.errorMsg);
                                        M5打印.TxDoFunction(10, 20, 0);
                                        Fnum++;
                                    }
                                    M5打印.TxOutputStringLn("------------------------------------------");
                                }
                                M5打印.TxOutputStringLn("数  量：" + renewInfos.Count);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("成  功：" + Snum);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("失  败：" + Fnum);
                                M5打印.TxDoFunction(10, 20, 0);
                                M5打印.TxOutputStringLn("------------------------------------------");
                                M5打印.TxOutputStringLn(" 此凭条为您的续借凭证，请妥善保管！");
                                break;
                        }
                        ServerSeting.transactionRecordInfos.Clear();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    //切纸
                    M5打印.TxDoFunction(10, 30, 30);
                    M5打印.TxDoFunction(12, 0, 30);
                        //关闭连接
                        M5打印.TxClosePrinter();
                }
                else
                {
                    errorMsg="连接打印机失败";
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        public string getErrorMessage(ErrorClass errorClass)
        {
            switch (errorClass)
            {
                case ErrorClass.await:
                    return "请稍等，正在恢复";
                case ErrorClass.Busy:
                    return "请稍等";
                case ErrorClass.cutterError:
                    return "切刀出问题，请检查";
                case ErrorClass.fastNoPage:
                    return "打印纸快用完了";
                case ErrorClass.losePage:
                    return "打印纸不足";
                case ErrorClass.NonRecoverable:
                    return "设备出问题，请更替";
                case ErrorClass.onlineState:
                    return "联机状态";
                case ErrorClass.PrintError:
                    return "打印出问题";
                case ErrorClass.smallError:
                    return "设备出问题，请安排人员检查";
                default:
                    return "正常";
            }
        }
    }
    public enum ErrorClass
    {
        onlineState = 0x0010,
        losePage = 0x0020,
        Busy = 0x0080,
        PrintError = 0x0400,
        smallError = 0x0800,
        cutterError = 0x1000,
        NonRecoverable = 0x2000,
        await = 0x4000,
        fastNoPage = 0x8000,
        commond = 0x0008
    }
    public enum PrintClass
    {
        Renew=0,
        Add=1,
        Loss=2,
        errorLog=3,
        Reissue=4
    }
}
