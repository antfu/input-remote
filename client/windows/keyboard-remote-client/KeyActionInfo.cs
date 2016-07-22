using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antnf.KeyboardRemote.Client
{
    enum KeyActionType
    {
        KeyDown,
        KeyUp
    }
    class KeyActionInfo
    {
        public int KeyCode { get; }
        public string Key { get; }
        public KeyActionType ActionType { get; }
        public bool IsShiftDown { get; }
        public bool IsCtrlDown { get; }
        public bool IsAltDown { get; }
    }
}
