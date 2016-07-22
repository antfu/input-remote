using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Antnf.KeyboardRemote.Client
{
    public partial class Main : Form
    {
        private Receiver receiver;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            receiver = new Receiver("ws://localhost/ws/r");
            receiver.Socket.OnOpen += Socket_OnOpen;
            receiver.Socket.OnClose += Socket_OnClose;
            receiver.OnRawMessage += Receiver_OnRawMessage;
            receiver.OnKeyDown += Receiver_OnKeyDown;
            receiver.OnKeyUp += Receiver_OnKeyUp;
            receiver.OnPeerStateChange += Receiver_OnPeerStateChange;

            receiver.Connect();
        }

        private void Socket_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            connectToolStripMenuItem.Checked = true;
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            connectToolStripMenuItem.Checked = true;
        }


        private void Receiver_OnRawMessage(Receiver receiver, dynamic data)
        {
            Log("Raw", data.ToString());
        }

        private void Receiver_OnPeerStateChange(Receiver receiver, PeerState state)
        {
            if(NotifyConnection.Checked)
                Notify(state.ToString(), "State");
            Log("State", state.ToString());
        }

        private void Receiver_OnKeyDown(Receiver receiver, KeyActionInfo info)
        {
            if (NotifyKeyDown.Checked)
                Notify(info.Key,"KeyDown");
            Log("KeyDown", info.Key);
            if (enableToolStripMenuItem.Checked)
                Actor.KeyDown((byte)info.KeyCode);
        }

        private void Receiver_OnKeyUp(Receiver receiver, KeyActionInfo info)
        {
            if (NotifyKeyUp.Checked)
                Notify(info.Key, "KeyUp");
            Log("KeyUp", info.Key);
            if (enableToolStripMenuItem.Checked)
                Actor.KeyUp((byte)info.KeyCode);

        }

        private void Log(string action, string msg = "")
        {
            OutputTextBox.Text += string.Format("[{0}]{1}", action, msg) + Environment.NewLine;
        }
        private void Notify(string text, string title = "")
        {
            TrayNotifyIcon.BalloonTipTitle = title;
            TrayNotifyIcon.BalloonTipText = text;
            TrayNotifyIcon.ShowBalloonTip(100);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            receiver.Connect();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            receiver.Close();
        }

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void DelaySendTest_Click(object sender, EventArgs e)
        {
            Actor.KeyDown(16);
            Actor.KeyPress(38);
            Actor.KeyUp(16);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NotifyConnection.Checked)
                receiver.Close();
            else
                receiver.Connect();
                
        }
        
    }
}
