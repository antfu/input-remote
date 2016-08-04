using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeyboardRemote.Tools;
using KeyboardRemote.Server;
using KeyboardRemote.Client.Components;

namespace KeyboardRemote.Client.Receiver
{
    public partial class Main : Form
    {
        private WebsocketAgent agent;
        private NotifyIconAgent notifyAgent;
        private SettingHelper setting = SettingHelper.GetSetting("setting.json");
        private EmbeddedServer server;
        private bool _EmbeddedServerEnabled = false;

        public bool EmbeddedServerEnabled
        {
            get
            {
                return _EmbeddedServerEnabled;
            }
            private set
            {
                _EmbeddedServerEnabled = value;
                setting["embeddedserver"] = value;
                useEmbeddedServerToolStripMenuItem.Checked = value;
                changeAddrToolStripMenuItem.Enabled = !value;
            }
        }

        public Main()
        {
            InitializeComponent();
        }

        private void Reconnect(bool input = false)
        {
            string new_ws_address = AddressHelper.GetWsAddress(input);
            if (!string.IsNullOrEmpty(new_ws_address))
                agent.Reconnect(new_ws_address, true);
        }

        private void EnableEmbeddedServer(int port = 2333)
        {
            if (agent != null
                || agent.IsConnected != true
                || MessageBox.Show("Enabling embedded server will cause you lost existing connection. Are you sure?","Are you sure?",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EmbeddedServerEnabled = true;
                server = new EmbeddedServer(port);
                setting["ws_url"] = server.RecevierWsAddress;
                server.Start();
                Reconnect(false);
            }
        }
        private void DisableEmbeddedServer()
        {
            if (MessageBox.Show("Are you sure to close embedded server?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EmbeddedServerEnabled = false;
                if (server != null)
                    server.Stop();
                server = null;
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            
            agent = new WebsocketAgent(AddressHelper.GetWsAddress());
            agent.OnKeyDown += Receiver_OnKeyDown;
            agent.OnKeyUp += Receiver_OnKeyUp;
            agent.OnConnect += Agent_OnConnect;
            agent.OnClose += Agent_OnClose;
            agent.OnPeerStateChange += Receiver_OnPeerStateChange;

            if (setting["embeddedserver"] == true)
                EnableEmbeddedServer();

            TrayNotifyIcon.Icon = Properties.Resources.r_red;
             notifyAgent = new NotifyIconAgent(TrayNotifyIcon, agent)
            {
                DisconnectIcon = Properties.Resources.r_red,
                WaitingIcon = Properties.Resources.r_grey,
                OnlineIcon = Properties.Resources.r_green,
                KeyDownIcon = Properties.Resources.r_orange
            };

            agent.Connect();
        }

        private void Agent_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            connectToolStripMenuItem.Checked = false;
            TrayNotifyIcon.Text = "Receiver [Disconnect]";
        }

        private void Agent_OnConnect(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Checked = true;
            TrayNotifyIcon.Text = "Receiver [Offline]";
        }

        private void Receiver_OnPeerStateChange(WebsocketAgent sender, PeerState state)
        {
            TrayNotifyIcon.Text = "Receiver [" + state.ToString() + "]";
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
		
        private void changeAddrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reconnect(true);
        }

        private void notifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyAgent.NotifyEnabled = notifyToolStripMenuItem.Selected;
        }

        private void useEmbeddedServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (useEmbeddedServerToolStripMenuItem.Checked)
                DisableEmbeddedServer();
            else
                EnableEmbeddedServer();
        }
    }
}
