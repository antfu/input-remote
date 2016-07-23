using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Antnf.KeyboardRemote.Client
{
    public class Actor
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);
        private const byte KEYEVENTF_KEYDOWN = 0x01;
        private const byte KEYEVENTF_KEYUP = 0x02;

        public static void KeyDown(byte keycode)
        {
            keybd_event(keycode, 0, KEYEVENTF_KEYDOWN, (IntPtr)0);
        }
        public static void KeyUp(byte keycode)
        {
            keybd_event(keycode, 0, KEYEVENTF_KEYUP, (IntPtr)0);
        }
        public static void KeyPress(byte keycode)
        {
            KeyDown(keycode);
            KeyUp(keycode);
        }

    }
}
