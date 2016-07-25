using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Antnf.KeyboardRemote.Server
{
    public class Echo : WebSocketBehavior
    {
        private string _name;
        protected override void OnOpen()
        {
            _name = Context.QueryString["name"] ?? "Nobody";
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Send(_name + ": " + e.Data);
        }
    }

    public class DemoServer
    {
        public void Run(int port = 80)
        {
            var wssv = new WebSocketServer(port);
            wssv.AddWebSocketService<Echo>("/echo");
            wssv.Start();
            Console.WriteLine("WebSocket server started on " + wssv.Address.ToString() + ":" + wssv.Port);
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
