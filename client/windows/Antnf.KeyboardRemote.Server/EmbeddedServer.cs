using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using Newtonsoft.Json;
using Antnf.KeyboardRemote.Tools;

namespace Antnf.KeyboardRemote.Server
{
    public class BaseBehavior : WebSocketBehavior
    {
        private static SenderService _sender;
        private static RecevierService _receiver;

        public void SendObject(dynamic obj)
        {
            this.Send(JsonConvert.SerializeObject(obj));
        }
        public void SendPeerState(PeerState state)
        {
            this.SendObject(new
            {
                action = "system",
                subaction = "peerstate",
                data = new
                {
                    state = state == PeerState.Online ? true : false
                }
            });
        }
        public void SendString(string str)
        {
            this.Send(str);
        }

        public BaseBehavior Other
        {
            get
            {
                if (this is SenderService)
                    return _receiver;
                else if (this is RecevierService)
                    return _sender;
                return null;
            }
        }
        public BaseBehavior Self
        {
            get
            {
                if (this is SenderService)
                    return _sender;
                else if (this is RecevierService)
                    return _receiver;
                return null;
            }
            set
            {
                if (this is SenderService)
                    _sender = value as SenderService;
                else if (this is RecevierService)
                    _receiver = value as RecevierService;
            }
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            if (this.Other != null)
                this.Other.SendString(e.Data);
            this.Self = null;
        }
        protected override void OnClose(CloseEventArgs e)
        {
            if (this.Other != null)
                this.Other.SendPeerState(PeerState.Offline);
            
        }
        protected override void OnOpen()
        {
            if (this.Self != null)
            {
                this.Context.WebSocket.Close(CloseStatusCode.InvalidData, "already_connected");
                return;
            }
            else
            {
                this.Self = this;
                if (this.Other != null)
                {
                    this.SendPeerState(PeerState.Online);
                    this.Other.SendPeerState(PeerState.Online);
                }
            }
        }
    }
    public class SenderService : BaseBehavior
    {
    }
    public class RecevierService : BaseBehavior
    {
    }

    public class EmbeddedServer
    {
        public void Run(int port = 80)
        {
            var wssv = new WebSocketServer(port);
            wssv.AddWebSocketService<SenderService>("/ws/s");
            wssv.AddWebSocketService<RecevierService>("/ws/r");
            wssv.Start();
            Console.WriteLine("WebSocket server started on " + wssv.Address.ToString() + ":" + wssv.Port);
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
