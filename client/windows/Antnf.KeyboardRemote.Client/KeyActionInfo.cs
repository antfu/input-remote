using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antnf.KeyboardRemote.Client
{
    public enum KeyActionType
    {
        KeyDown,
        KeyUp
    }
    public class KeyActionInfo
    {
        public int KeyCode { get; set; }
        public string Key { get; set; }
        public KeyActionType ActionType { get; set; }
        public bool IsShiftDown { get; set; }
        public bool IsCtrlDown { get; set; }
        public bool IsAltDown { get; set; }
    }
}
