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
            receiver.OnRawMessage += Receiver_OnRawMessage;
            receiver.OnKeyDown += Receiver_OnKeyDown;
            receiver.OnKeyUp += Receiver_OnKeyUp;
            receiver.OnPeerStateChange += Receiver_OnPeerStateChange;

            receiver.Connect();
        }

        private void Receiver_OnKeyUp(Receiver receiver, KeyActionInfo info)
        {
            Notify(info.Key, "KeyUp");
            Log("KeyUp", info.Key);
        }

        private void Receiver_OnRawMessage(Receiver receiver, dynamic data)
        {
            Log("Raw", data.ToString());
        }

        private void Receiver_OnPeerStateChange(Receiver receiver, PeerState state)
        {
            Notify(state.ToString(), "State");
            Log("State", state.ToString());
        }

        private void Receiver_OnKeyDown(Receiver receiver, KeyActionInfo info)
        {
            Notify(info.Key,"KeyDown");
            Log("KeyDown", info.Key);
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
    }
}
