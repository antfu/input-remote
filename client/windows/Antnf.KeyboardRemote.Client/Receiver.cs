using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Antnf.KeyboardRemote.Tools
{
	/// <summary>
	/// 终端状态。
	/// </summary>
	public enum PeerState
	{
		/// <summary>处于离线状态。</summary>
		Offline,
		/// <summary>处于在线状态。</summary>
		Online
	}

	public delegate void KeyActionEventHandler(WebsocketAgent agent, KeyActionInfo info);
	public delegate void PeerStateChangeEventHandler(WebsocketAgent agent, PeerState state);
	public delegate void JsonEventHandler(WebsocketAgent agent, dynamic data);

	/// <summary>
	/// 网络嵌套字代理。
	/// </summary>
	public class WebsocketAgent
	{
		/// <summary>获取地址。</summary>
		/// <value>地址。</value>
		public string URL { get; }

		/// <summary>获取代理的嵌套字。</summary>
		/// <value>代理的嵌套字。</value>
		public WebSocket Socket { get; }

		public event KeyActionEventHandler OnKeyDown;
		public event KeyActionEventHandler OnKeyUp;
		public event PeerStateChangeEventHandler OnPeerStateChange;
		public event JsonEventHandler OnRawMessage;
		public event EventHandler OnPing;

		public WebsocketAgent(string url)
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

		/// <summary>
		/// 动态解析键盘行为信息。
		/// </summary>
		/// <param name="data">运行时决定的键盘行为数据。</param>
		/// <returns>键盘行为信息。</returns>
		private KeyActionInfo ParseKeyAction(dynamic data)
		{
			return new KeyActionInfo()
			{
				ActionType = data.keyaction == "keydown" ? KeyActionType.KeyDown : KeyActionType.KeyUp,
				Key = data.key,
				KeyCode = data.keycode,
				IsAltDown = data.is_alt_down,
				IsShiftDown = data.is_shift_down,
				IsCtrlDown = data.is_ctrl_down
			};
		}

		private void Socket_OnOpen(object sender, EventArgs e)
		{
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
					if (obj.subaction == "peerstate")
					{
						if (OnPeerStateChange != null)
							this.OnPeerStateChange(this, obj.data.state == true ? PeerState.Online : PeerState.Offline);
					}
				}

				else if (obj.action == "key")
				{
					KeyActionInfo info = ParseKeyAction(obj.data);
					if (info.ActionType == KeyActionType.KeyUp)
					{
						if (OnKeyUp != null)
							this.OnKeyUp(this, info);
					}
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
			//e.Code
			//e.Reason
			//throw new NotImplementedException();
		}

	}
}
