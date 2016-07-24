using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Antnf.KeyboardRemote.Tools;

namespace Antnf.KeyboardRemote.Client.Receiver
{
    public partial class Main : Form
    {
        private WebsocketAgent agent;
        private string ws_url = "";
        private string http_url = "http://localhost/receiver/"; //default url
        private SettingHelper settings = new SettingHelper("setting.json");

        public Main()
        {
            InitializeComponent();
        }
        private void UpdateNotifyIcon()
        {
            if (!agent.IsConnected)
                TrayNotifyIcon.Icon = Properties.Resources.red;
            else if (!agent.IsOnline)
                TrayNotifyIcon.Icon = Properties.Resources.grey;
            else
                TrayNotifyIcon.Icon = Properties.Resources.green;
        }

        private string url_to_ws(string url)
        {
            url = url.Replace("https", "wss").Replace("http", "ws").Replace("receiver", "ws") + "&t=receiver";
            if (url.IndexOf('?') == -1)
                url = url.Replace('&', '?');
            return url;
        }
        
        private void Reconnect()
        {
            agent.Reconnect(ws_url,true);
        }

        private void AddressInput()
        {
            var url_input = new AddressInput();
            url_input.url = this.http_url;
            url_input.ShowDialog();
            this.http_url = url_input.url;
            this.ws_url = url_to_ws(this.http_url);
            settings["http_url"] = this.http_url;
            settings["ws_url"] = this.ws_url;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.http_url = settings["http_url"];
            this.ws_url = settings["ws_url"];
            if (string.IsNullOrEmpty(this.http_url) || string.IsNullOrEmpty(this.ws_url))
                AddressInput();

            agent = new WebsocketAgent(ws_url);
            agent.OnRawMessage += Receiver_OnRawMessage;
            agent.OnKeyDown += Receiver_OnKeyDown;
            agent.OnKeyUp += Receiver_OnKeyUp;
            agent.OnConnect += Agent_OnConnect;
            agent.OnError += Agent_OnError;
            agent.OnClose += Agent_OnClose;
            agent.OnPeerStateChange += Receiver_OnPeerStateChange;

            agent.Connect();
        }

        private void Agent_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            connectToolStripMenuItem.Checked = false;
            UpdateNotifyIcon();
            Notify("Reason: " + e.Reason, "Socket Closed");
        }

        private void Agent_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Notify("Message: " + e.Message, "Socket Error");
        }

        private void Agent_OnConnect(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Checked = true;
            UpdateNotifyIcon();
        }

        private void Receiver_OnRawMessage(WebsocketAgent sender, dynamic data)
        {
            Log("Raw", data.ToString());
        }

        private void Receiver_OnPeerStateChange(WebsocketAgent sender, PeerState state)
        {
            stateToolStripMenuItem.Text = state.ToString();

            if (NotifyConnection.Checked)
                Notify(state.ToString(), "State");
            UpdateNotifyIcon();
            Log("State", state.ToString());

        }

        private void Receiver_OnKeyDown(WebsocketAgent sender, KeyActionInfo info)
        {
            if (NotifyKeyDown.Checked)
                Notify(info.Key,"KeyDown");
            Log("KeyDown", info.Key);
            if (enableToolStripMenuItem.Checked)
                KeyboardSimulator.KeyDown((Keys)info.KeyCode);
        }

        private void Receiver_OnKeyUp(WebsocketAgent sender, KeyActionInfo info)
        {
            if (NotifyKeyUp.Checked)
                Notify(info.Key, "KeyUp");
            Log("KeyUp", info.Key);
            if (enableToolStripMenuItem.Checked)
                KeyboardSimulator.KeyUp((Keys)info.KeyCode);
        }

        private void Log(string action, string msg = "")
        {
            OutputTextBox.Text += string.Format("[{0}]{1}", action, msg) + Environment.NewLine;
        }
        private void Notify(string text, string title = "")
        {
            TrayNotifyIcon.BalloonTipTitle = string.IsNullOrEmpty(title) ? " " : title;
            TrayNotifyIcon.BalloonTipText = string.IsNullOrEmpty(text) ? " " : text;
            TrayNotifyIcon.ShowBalloonTip(100);
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

        private void changeAddrToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddressInput();
            Reconnect();
        }
    }
}
