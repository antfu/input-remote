using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Antnf.KeyboardRemote.Tools
{
	public delegate void KeyActionEventHandler(WebsocketAgent agent, KeyActionInfo info);
	public delegate void PeerStateChangeEventHandler(WebsocketAgent agent, PeerState state);
	public delegate void JsonEventHandler(WebsocketAgent agent, dynamic data);

	/// <summary>
	/// 网络嵌套字代理。
	/// </summary>
	public class WebsocketAgent
	{
		/// <summary>获取地址</summary>
		/// <value>地址</value>
		public string URL { get; private set; }
		/// <summary>获取代理的嵌套字</summary>
		/// <value>代理的嵌套字</value>
		public WebSocket Socket { get; private set; }

        public bool IsConnected { get; private set; } = false;
        public bool IsOnline { get; private set; } = false;

        public event KeyActionEventHandler OnKeyDown;
		public event KeyActionEventHandler OnKeyUp;
		public event PeerStateChangeEventHandler OnPeerStateChange;
		public event JsonEventHandler OnRawMessage;
		public event EventHandler OnPing;

		public WebsocketAgent(string url)
		{
            Reconnect(url,false);
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

        public void Reconnect(string new_url = null, bool instant_connect = true)
        {
            // Set new url if exists
            if (new_url != null)
                this.URL = new_url;
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
            if (instant_connect)
                this.Connect();
        }

        public void SendKey(KeyActionInfo info)
        {
            string json_str = Convert.DumpKeyAction(info);
            this.Socket.Send(json_str);
        }
        
		private void Socket_OnOpen(object sender, EventArgs e)
		{
            this.IsConnected = true;
			//throw new NotImplementedException();
		}

		private void Socket_OnMessage(object sender, MessageEventArgs e)
		{
			if (e.IsPing)
			{
				if (OnPing != null)
					this.OnPing(this, null);
			}
			else if (e.IsText)
			{
				dynamic obj = JsonConvert.DeserializeObject(e.Data);
				if (OnRawMessage != null)
					this.OnRawMessage(this, obj);

				if (obj.action == "system")
				{
                    // PeerState Update
                    if (obj.subaction == "peerstate")
					{
                        this.IsOnline = obj.data.state;
                        if (OnPeerStateChange != null)
							this.OnPeerStateChange(this, obj.data.state == true ? PeerState.Online : PeerState.Offline);
					}
				}

				else if (obj.action == "key")
				{
					KeyActionInfo info = Convert.ParseKeyAction(obj.data);
                    // KeyUp Event
                    if (info.ActionType == KeyActionType.KeyUp)
					{
						if (OnKeyUp != null)
							this.OnKeyUp(this, info);
					}
                    // KeyDown Event
                    else if (info.ActionType == KeyActionType.KeyDown)
					{
						if (OnKeyDown != null)
							this.OnKeyDown(this, info);
					}
				}
			}
			else
			{
				//TODO: IsBinary
			}
		}

		private void Socket_OnError(object sender, ErrorEventArgs e)
		{
            //e.Message
            //throw new NotImplementedException();
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
		{
            this.IsConnected = false;
            //e.Code
            //e.Reason
            //throw new NotImplementedException();
        }

    }
}
