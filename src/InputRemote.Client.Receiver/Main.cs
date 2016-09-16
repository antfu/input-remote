using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InputRemote.Tools;
using InputRemote.Server;
using InputRemote.Server.Http;

namespace InputRemote.Client.Receiver
{
    public partial class Main : Form
    {
        private WebsocketAgent agent;
        private NotifyIconAgent notifyAgent;
        private EmbeddedServer ws_server;
        private StaticHttpServer http_server;
        private SettingHelper settings;
        private string ws_url;

#if DEBUG
        private string http_dir = "../../../Controller";
#else
        private string http_dir = "controller";
#endif

        public bool EmbeddedServerEnabled { get; set; } = false;
        public Main()
        {
            settings = SettingHelper.GetSetting("setting.json");
            if (settings["ws_port"] == null)
                settings["ws_port"] = 81;
            if (settings["http_port"] == null)
                settings["http_port"] = 80;
            ws_url = "ws://localhost:" + settings["ws_port"].ToString() + "/ws/r";
            InitializeComponent();
        }

        private void Reconnect(bool input = false)
        {
            agent.Reconnect(ws_url, true);
        }

        private void EnableEmbeddedServer()
        {
            
            int ws_port = settings["ws_port"];
            int http_port = settings["http_port"];
            if (agent != null
                || agent.IsConnected != true
                || MessageBox.Show("Enabling embedded server will cause you lost existing connection. Are you sure?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EmbeddedServerEnabled = true;
                ws_server = new EmbeddedServer(ws_port);
                http_server = new StaticHttpServer(http_dir, http_port, ws_port);
                ws_server.OnError += (sender, e) =>
                {
                    if (e.Exception.SocketErrorCode == System.Net.Sockets.SocketError.AddressAlreadyInUse)
                        MessageBox.Show("Port " + ws_port.ToString() + " is already in use, can not start server. Exiting.");
                    else
                        MessageBox.Show("Unhandlered exception on http server: " + e.Exception.Message.ToString() + ". Exiting.");

                    Application.Exit();
                };
                http_server.OnError += (sender, e) =>
                {
                    if (e.Exception.SocketErrorCode == System.Net.Sockets.SocketError.AddressAlreadyInUse)
                        MessageBox.Show("Port " + http_port.ToString() + " is already in use, can not start server. Exiting.");
                    else
                        MessageBox.Show("Unhandlered exception on http server: " + e.Exception.Message.ToString() + ". Exiting.");

                    Application.Exit();
                };
                ws_server.Start();
                http_server.Start();
                useEmbeddedServerToolStripMenuItem.Checked = true;
                Reconnect(false);
            }
        }
        private void DisableEmbeddedServer()
        {
            if (MessageBox.Show("Are you sure to close embedded server?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EmbeddedServerEnabled = false;
                if (ws_server != null)
                    ws_server.Stop();
                if (http_server != null)
                    http_server.Stop();
                ws_server = null;
                http_server = null;
                useEmbeddedServerToolStripMenuItem.Checked = false;
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.Hide();
            //Control.CheckForIllegalCrossThreadCalls = false;

            agent = new WebsocketAgent(ws_url);
            agent.OnKeyDown += Receiver_OnKeyDown;
            agent.OnKeyUp += Receiver_OnKeyUp;
            agent.OnMouseMove += Agent_OnMouseMove;
            agent.OnMouseButton += Agent_OnMouseButton;
            agent.OnConnect += Agent_OnConnect;
            agent.OnClose += Agent_OnClose;
            agent.OnPeerStateChange += Receiver_OnPeerStateChange;

            TrayNotifyIcon.Icon = Properties.Resources.r_red;
            notifyAgent = new NotifyIconAgent(TrayNotifyIcon, agent)
            {
                DisconnectIcon = Properties.Resources.r_red,
                WaitingIcon = Properties.Resources.r_grey,
                OnlineIcon = Properties.Resources.r_green,
                KeyDownIcon = Properties.Resources.r_orange
            };

            EnableEmbeddedServer();

            System.Diagnostics.Process.Start("http://localhost:" + settings["http_port"].ToString() + "/about.html");
        }

        private void Agent_OnMouseButton(WebsocketAgent agent, MouseActionInfo info)
        {
            if (!enableToolStripMenuItem.Checked) return;
            
            switch(info.ActionType)
            {
                case MouseActionType.ButtonDown:
                    if (info.Button.HasValue)
                    MouseSimulator.MouseDown(info.Button.Value);
                    break;
                case MouseActionType.ButtonUp:
                    if (info.Button.HasValue)
                        MouseSimulator.MouseUp(info.Button.Value);
                    break;
            }
        }

        private Point? CusorStartPoint = null;
        private void Agent_OnMouseMove(WebsocketAgent agent, MouseActionInfo info)
        {
            if (!enableToolStripMenuItem.Checked) return;

            if (info.ActionType == MouseActionType.MoveStart)
                CusorStartPoint = MouseSimulator.Position;
            else if (info.ActionType == MouseActionType.MoveEnd)
                CusorStartPoint = null;
            else if (info.ActionType == MouseActionType.Move)
            {
                if (CusorStartPoint != null)
                {
                    MouseSimulator.Position = new MousePoint(new Point(CusorStartPoint.Value.X + (int)info.X, CusorStartPoint.Value.Y + (int)info.Y));
                }
            }
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
            return;
        }

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Opacity = 1;
            this.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrayNotifyIcon.Visible = false;
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

        private void useEmbeddedServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (useEmbeddedServerToolStripMenuItem.Checked)
                DisableEmbeddedServer();
            else
                EnableEmbeddedServer();
        }
    }
}
