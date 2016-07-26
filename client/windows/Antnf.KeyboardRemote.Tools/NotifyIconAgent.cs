using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace Antnf.KeyboardRemote.Tools
{
    public class NotifyIconAgent
    {
        public NotifyIcon Instance { get; private set; }
        public WebsocketAgent Agent { get; private set; }
        public Icon DefaultIcon { get; set; }
        public Icon DisconnectIcon { get; set; }
        public Icon WaitingIcon { get; set; }
        public Icon OnlineIcon { get; set; }
        public Icon KeyDownIcon { get; set; }
        public bool NotifyEnabled { get; set; }

        private bool _DisplayMessageIcon = false;
        private Icon _StateIcon;
        private Icon _MessageIcon;
        
        
        /// <summary>
        /// State icon
        /// </summary>
        private Icon StateIcon
        {
            get
            {
                return _StateIcon;
            }
            set
            {
                _StateIcon = value;
                if (!DisplayMessageIcon)
                    Instance.Icon = _StateIcon;
            }
        }
        /// <summary>
        /// State icon
        /// </summary>
        private Icon MessageIcon
        {
            get
            {
                return _MessageIcon;
            }
            set
            {
                _MessageIcon = value;
                if (DisplayMessageIcon)
                    Instance.Icon = _MessageIcon;
            }
        }
        /// <summary>
        /// Use message icon to override state icon or not
        /// </summary>
        private bool DisplayMessageIcon
        {
            get
            {
                return _DisplayMessageIcon;
            }
            set
            {
                _DisplayMessageIcon = value;
                if (DisplayMessageIcon)
                    Instance.Icon = MessageIcon;
                else
                    Instance.Icon = StateIcon;
            }
        }

        public NotifyIconAgent(NotifyIcon icon, WebsocketAgent agent)
        {
            Agent = agent;
            Instance = icon;
            agent.OnConnect += Agent_OnConnect;
            agent.OnClose += Agent_OnClose;
            agent.OnError += Agent_OnError;
            agent.OnPeerStateChange += Agent_OnPeerStateChange;
            agent.OnKeyDown += Agent_OnKeyDown;
            agent.OnKeyUp += Agent_OnKeyUp;
        }
    
        public void Notify(string text, string title = "")
        {
            if (NotifyEnabled)
            {
                Instance.BalloonTipTitle = string.IsNullOrEmpty(title) ? " " : title;
                Instance.BalloonTipText = string.IsNullOrEmpty(text) ? " " : text;
                Instance.ShowBalloonTip(100);
            }
        }

        private void Agent_OnConnect(object sender, EventArgs e)
        {
            UpdateIcon();
        }

        private void Agent_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            UpdateIcon();
            Notify(e.Reason, "Socket Closed");
        }

        private void Agent_OnPeerStateChange(WebsocketAgent agent, PeerState state)
        {
            UpdateIcon();
            Notify("Peer " + state.ToString());
        }

        private void Agent_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Notify(e.Message, "Socket Error");
        }

        private void Agent_OnKeyUp(WebsocketAgent agent, KeyActionInfo info)
        {
            DisplayMessageIcon = false;
        }

        private void Agent_OnKeyDown(WebsocketAgent agent, KeyActionInfo info)
        {
            MessageIcon = KeyDownIcon;
            DisplayMessageIcon = true;
        }

        private void UpdateIcon()
        {
            if (!Agent.IsConnected)
                StateIcon = DisconnectIcon;
            else if (!Agent.IsOnline)
                StateIcon = WaitingIcon;
            else
                StateIcon = OnlineIcon;
        }
    }
}
