using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeyboardRemote.Tools;
using KeyboardRemote.Client.Components;

namespace KeyboardRemote.Client.Sender
{
    public partial class Main : Form
    {
        WebsocketAgent agent;
        private NotifyIconAgent notifyAgent;
        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        public Main()
        {
            InitializeComponent();
        }

        private void Reconnect(bool renew = false)
        {
            agent.Reconnect(AddressHelper.GetWsAddress(renew), true);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            agent = new WebsocketAgent(AddressHelper.GetWsAddress());
            TrayNotifyIcon.Icon = Properties.Resources.s_red;
            notifyAgent = new NotifyIconAgent(TrayNotifyIcon, agent)
            {
                DisconnectIcon = Properties.Resources.s_red,
                WaitingIcon = Properties.Resources.s_grey,
                OnlineIcon = Properties.Resources.s_green,
                KeyDownIcon = Properties.Resources.s_orange
            };
            agent.Connect();

            mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);
            mouseHook.MouseWheel += new MouseEventHandler(mouseHook_MouseWheel);
            keyboardHook.KeyDown += new KeyEventHandler(keyboardHook_KeyDown);
            keyboardHook.KeyUp += new KeyEventHandler(keyboardHook_KeyUp);
            keyboardHook.KeyPress += new KeyPressEventHandler(keyboardHook_KeyPress);
            mouseHook.Start();
            keyboardHook.Start();
            SetXYLabel(MouseSimulator.X, MouseSimulator.Y);
        }
        void keyboardHook_KeyPress(object sender, KeyPressEventArgs e)
        {
            AddKeyboardEvent(
              "KeyPress",
              "",
              e.KeyChar.ToString(),
              "",
              "",
              ""
              );
        }
        void keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            AddKeyboardEvent(
              "KeyUp",
              e.KeyCode.ToString(),
              "",
              e.Shift.ToString(),
              e.Alt.ToString(),
              e.Control.ToString()
              );
            agent.SendKey(KeyActionInfo.ParseFromKeyEventArgs(KeyActionType.KeyUp, e));
            // Block System Key Input
            //e.Handled = true; 
        }
        void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            AddKeyboardEvent(
              "KeyDown",
              e.KeyCode.ToString(),
              "",
              e.Shift.ToString(),
              e.Alt.ToString(),
              e.Control.ToString()
              );
            agent.SendKey(KeyActionInfo.ParseFromKeyEventArgs(KeyActionType.KeyDown, e));
            // Block System Key Input
            //e.Handled = true;
        }
        void mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            AddMouseEvent(
              "MouseWheel",
              "",
              "",
              "",
              e.Delta.ToString()
              );
        }
        void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            AddMouseEvent(
              "MouseUp",
              e.Button.ToString(),
              e.X.ToString(),
              e.Y.ToString(),
              ""
              );
        }
        void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            AddMouseEvent(
              "MouseDown",
              e.Button.ToString(),
              e.X.ToString(),
              e.Y.ToString(),
              ""
              );
        }
        void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            SetXYLabel(e.X, e.Y);
        }
        void SetXYLabel(int x, int y)
        {
            curXYLabel.Text = String.Format("Current Mouse Point: X={0}, y={1}", x, y);
        }
        void AddMouseEvent(string eventType, string button, string x, string y, string delta)
        {
            listView1.Items.Insert(0,
              new ListViewItem(
                new string[]{
            eventType,
            button,
            x,
            y,
            delta
                }));
        }
        void AddKeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
        {
            listView2.Items.Insert(0,
               new ListViewItem(
                 new string[]{
            eventType,
            keyCode,
            keyChar,
            shift,
            alt,
            control
              }));
        }
        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Not necessary anymore, will stop when application exits
            //mouseHook.Stop();
            //keyboardHook.Stop();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connectToolStripMenuItem.Checked)
            {
                agent.Close();
                connectToolStripMenuItem.Checked = false;
            }
            else
                Reconnect();
        }

        private void changeAddrToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Reconnect(true);
        }
    }
}
