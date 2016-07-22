using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WebSocketSharp;

namespace Antnf.KeyboardRemote.Client
{
    enum PeerSatate
    {
        Offline,
        Online
    }
    delegate void KeyActionEvent(Receiver receiver, KeyActionInfo info);
    delegate void PeerStateChangeEvent(Receiver receiver, PeerSatate state);
    class Receiver
    {
        public string URL {get;}
        public WebSocket Socket { get;}
        public event KeyActionEvent OnKeyDown;
        public event KeyActionEvent OnKeyUp;
        public event PeerStateChangeEvent OnPeerStateChange;

        public Receiver(string url)
        {
            this.URL = url;
            this.Socket = new WebSocket(this.URL);
            this.Socket.OnOpen += Socket_OnOpen;
            this.Socket.OnMessage += Socket_OnMessage;
            this.Socket.OnClose += Socket_OnClose;
            this.Socket.OnError += Socket_OnError;
#if DEBUG
            this.Socket.Log.Level = LogLevel.Trace;
            this.Socket.WaitTime = TimeSpan.FromSeconds(10);
            this.Socket.EmitOnPing = true;
#endif
        }

        public void Connect()
        {
            if (!this.Socket.IsAlive)
                this.Socket.Connect();
        }

        public void Close()
        {
            if (this.Socket.IsAlive)
                this.Socket.Close();
        }


        private void Socket_OnOpen(object sender, EventArgs e)
        {
            MessageBox.Show("Open");
            //throw new NotImplementedException();
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Data);
            //e.IsPing
            //e.Data
            //throw new NotImplementedException();
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
            //e.Message
            //throw new NotImplementedException();
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            MessageBox.Show(e.Reason);
            //e.Code
            //e.Reason
            //throw new NotImplementedException();
        }

    }
}
