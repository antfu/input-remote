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

        public Main()
        {
            InitializeComponent();
        }

        private void Reconnect(bool renew = false)
        {
            string new_ws_address = InputHelper.GetWsAddress(renew);
            if (!string.IsNullOrEmpty(new_ws_address))
                agent.Reconnect(new_ws_address, true);
        }
        

        private void Main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            Control.CheckForIllegalCrossThreadCalls = false;

            agent = new WebsocketAgent(InputHelper.GetWsAddress());
            agent.OnKeyDown += Receiver_OnKeyDown;
            agent.OnKeyUp += Receiver_OnKeyUp;
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

        }
    }
}
