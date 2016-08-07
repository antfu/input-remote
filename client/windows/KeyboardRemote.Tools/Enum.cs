using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardRemote.Tools
{
    public enum ClientType
    {
        Unknown,
        Sender,
        Receiver
    }
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

    public enum KeyActionType
    {
        KeyDown,
        KeyUp
    }

    public enum MouseActionType
    {
        None,
        MoveStart,
        MoveEnd,
        Move,
        ButtonDown,
        ButtonUp
    }

    /// <summary>
	/// Standard Keyboard Shortcuts used by most applications
	/// </summary>
	public enum StandardShortcut
    {
        Copy,
        Cut,
        Paste,
        SelectAll,
        Save,
        Open,
        New,
        Close,
        Print
    }
}
