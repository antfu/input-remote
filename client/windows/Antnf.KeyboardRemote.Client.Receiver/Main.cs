using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Antnf.KeyboardRemote.Tools;
using Antnf.KeyboardRemote.Client.Components;

namespace Antnf.KeyboardRemote.Client.Receiver
{
    public partial class Main : Form
    {
        private WebsocketAgent agent;
        private NotifyIconAgent notifyAgent;
        private string ws_url = "";
        private string http_url = "http://localhost/receiver/"; //default url
        private SettingHelper settings = new SettingHelper("setting.json");

        public Main()
        {
            InitializeComponent();
        }
        
        private void Reconnect()
        {
            agent.Reconnect(ws_url,true);
        }

		#warning 修改了一下这个方法以及调用这个方法的其他方法的代码逻辑。整理起来差不多是这样的——当TryInputAddress方法返回true，意味着url发生了改变，此时必定要做出相应的动作；当返回false，则url没有发生改变，或者改变没有被认可或提交，此时不需要响应。 -> Sam Lu
		/// <summary>
		/// 通过弹出 <see cref="AddressInput"/> 对话框，试图让用户输入一个URL。
		/// </summary>
		/// <param name="allowCancel">指示是否允许用户取消对话框操作。</param>
		/// <returns>用户输入了新的合法的URL，并提交了更改。</returns>
		private bool TryInputAddress(bool allowCancel = false)
        {
            var url_input = new AddressInput();
            url_input.Url = this.http_url;
			url_input.AllowCancel = allowCancel;
			
			if (url_input.ShowDialog() == true)
			{
				this.http_url = url_input.Url;
				this.ws_url = WebsocketAgent.HttpToWsUrl(this.http_url);
				settings["http_url"] = this.http_url;
				settings["ws_url"] = this.ws_url;

				return true;
			}
			else return false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.http_url = settings["http_url"];
            this.ws_url = settings["ws_url"];
            if (string.IsNullOrEmpty(this.http_url) || string.IsNullOrEmpty(this.ws_url))
                TryInputAddress();

            agent = new WebsocketAgent(ws_url);
            agent.OnKeyDown += Receiver_OnKeyDown;
            agent.OnKeyUp += Receiver_OnKeyUp;
            agent.OnConnect += Agent_OnConnect;
            agent.OnClose += Agent_OnClose;
            agent.OnPeerStateChange += Receiver_OnPeerStateChange;

            notifyAgent = new NotifyIconAgent(TrayNotifyIcon, agent)
            {
                DisconnectIcon = Properties.Resources.red,
                WaitingIcon = Properties.Resources.grey,
                OnlineIcon = Properties.Resources.green,
                KeyDownIcon = Properties.Resources.orange
            };

            agent.Connect();
        }

        private void Agent_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            connectToolStripMenuItem.Checked = false;
        }
        
        private void Agent_OnConnect(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Checked = true;
        }
        
        private void Receiver_OnPeerStateChange(WebsocketAgent sender, PeerState state)
        {
            stateToolStripMenuItem.Text = state.ToString();
            Log("State", state.ToString());
        }

        private void Receiver_OnKeyDown(WebsocketAgent sender, KeyActionInfo info)
        {
            if (enableToolStripMenuItem.Checked)
                KeyboardSimulator.KeyDown((Keys)info.KeyCode);
            Log("KeyDown", info.Key);
        }

        private void Receiver_OnKeyUp(WebsocketAgent sender, KeyActionInfo info)
        {
            Log("KeyUp", info.Key);
            if (enableToolStripMenuItem.Checked)
                KeyboardSimulator.KeyUp((Keys)info.KeyCode);
        }

        private void Log(string action, string msg = "")
        {
            OutputTextBox.Text += string.Format("[{0}]{1}", action, msg) + Environment.NewLine;
        }

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Opacity = 1;
            this.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connectToolStripMenuItem.Checked)
                agent.Close();
            else
                Reconnect();
        }

		#warning 这里就是关键的位置，我发现，如果按照原先的代码逻辑，不管url有没有改变，都会重新连接一次，错误就发生在（本来已经处于“Connected”状态，再次尝试连接将导致“Socket Closed”）。
        private void changeAddrToolStripMenuItem_Click(object sender, EventArgs e)
        {
			#warning	我建议这里再添加是否已经处于“Connected”状态的判断。
            if (TryInputAddress(true))
			{
				Reconnect();
			}
        }
    }
}
