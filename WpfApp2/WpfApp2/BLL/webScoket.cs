
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Fleck;
using WebSocketServer = Fleck.WebSocketServer;
using System.Threading;
using System.Windows;

namespace WpfApp2.BLL
{
    public class service : WebSocketBehavior
    {
        /// <summary>
        /// 打开
        /// </summary>
        protected override void OnOpen()
        {
            BookRfid book = new BookRfid(Action.moreTimes,null);
        }
        /// <summary>
        /// 连接中
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMessage(MessageEventArgs e)
        {
            object data = e as object;
            Send("sdfsdf");
        }
        /// <summary>
        /// 出错
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }
    }

    public class webScoket
    {
        public Thread thread;
        public int i = 0;
        public List<string> EPCList = new List<string>();
        public bool puResult = true;
        public webScoket()
        {
            
            thread = new Thread(new ThreadStart(add));
            thread.IsBackground = true;
            WebSocketServer server = new WebSocketServer("ws://127.0.0.1:7181");
            BookRfid bookRfid = new BookRfid(Action.moreTimes, null);
            server.Start(socket => {
            socket.OnOpen = () =>   //连接建立事件 
            {                    //获取客户端网页的url       
                string clientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
            };                    
           
                socket.OnMessage = message =>  //接受客户端网页消息事件              
                {
                    EPCList.Clear();
                    
                    if (thread.ThreadState.ToString() == "Background, Unstarted")
                    {
                        thread.Start();
                    }
                    puResult = true;
                    i = 0;
                    while (puResult)
                    {
                        
                        string epc = Objextension.PBEPC;
                        bool result = false;
                        foreach (var temp in EPCList)
                        {
                            if (temp.Trim() == epc.Trim())
                            {
                                result = true;
                            }
                        }
                        if (!result && !string.IsNullOrEmpty(epc))
                        {
                            socket.Send(epc + "");
                            EPCList.Add(epc);
                        }
                    }
                };
            });
        }
        public void add()
        {
            while (true)
            {
                i++;

                if (i >= 10)
                {
                    puResult = false;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
