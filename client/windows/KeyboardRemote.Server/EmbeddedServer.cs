using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using Newtonsoft.Json;
using KeyboardRemote.Tools;

namespace KeyboardRemote.Server
{
    public class BaseBehavior : WebSocketBehavior
    {
        private static SenderService _sender;
        private static RecevierService _receiver;

        public static void Clear()
        {
            _sender = null;
            _receiver = null;
        }

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
        public ClientType Type
        {
            get
            {
                if (this is SenderService)
                    return ClientType.Sender;
                else if (this is RecevierService)
                    return ClientType.Receiver;
                return ClientType.Unknown;
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
            Console.WriteLine("Client "+Type.ToString()+" lost.");
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
                Console.WriteLine("Client " + Type.ToString() + " connected.");
            }
        }
    }
    public class SenderService : BaseBehavior { }
    public class RecevierService : BaseBehavior { }

    public class EmbeddedServer
    {
        public string SenderWsAddress { get; private set; }
        public string RecevierWsAddress { get; private set; }
        public int Port { get; private set; }
        public string Address
        {
            get
            {
                return "ws://localhost:" + server.Port;
            }
        }

        private WebSocketServer server;

        public EmbeddedServer(int port = 80)
        {
            Port = port;
            server = new WebSocketServer(Port);
            server.AddWebSocketService<SenderService>("/ws/s");
            server.AddWebSocketService<RecevierService>("/ws/r");
            SenderWsAddress = Address + "/ws/s";
            RecevierWsAddress = Address + "/ws/r";
        }
        public void Start()
        {
            try
            {
                server.Start();
                Console.WriteLine("WebSocket server started on " + Address);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
        }
        public void Stop()
        {
            server.Stop();
            BaseBehavior.Clear();
            Console.WriteLine("WebSocket server stopped");
        }
    }
}
