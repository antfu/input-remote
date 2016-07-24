using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Antnf.KeyboardRemote.Tools
{
    public class KeyActionInfo
    {
        public int KeyCode { get; set; }
        public string Key { get; set; }
        public KeyActionType ActionType { get; set; }
        public bool IsShiftDown { get; set; }
        public bool IsCtrlDown { get; set; }
        public bool IsAltDown { get; set; }


        /// <summary>
        /// 动态解析键盘行为信息。
        /// </summary>
        /// <param name="data">运行时决定的键盘行为数据。</param>
        /// <returns>键盘行为信息。</returns>
        public static KeyActionInfo ParseFromJsonObject(dynamic data)
        {
            return new KeyActionInfo()
            {
                ActionType = data.keyaction == "keydown" ? KeyActionType.KeyDown : KeyActionType.KeyUp,
                Key = data.key,
                KeyCode = data.keycode,
                IsAltDown = data.is_alt_down,
                IsShiftDown = data.is_shift_down,
                IsCtrlDown = data.is_ctrl_down
            };
        }
        /// <summary>
        /// Dumps KeyActionInfo into object
        /// </summary>
        /// <param name="info">KeyActionInfo object</param>
        /// <returns>Dynamic Object</returns>
        public static dynamic DumpToObject(KeyActionInfo info)
        {
            return new
            {
                keyaction = info.ActionType == KeyActionType.KeyDown ? "keydown" : "keyup",
                key = info.Key,
                keycode = info.KeyCode,
                is_alt_down = info.IsAltDown,
                is_shift_down = info.IsShiftDown,
                is_ctrl_down = info.IsCtrlDown
            };
        }
        /// <summary>
        /// Dumps KeyActionInfo into json
        /// </summary>
        /// <param name="info">KeyActionInfo object</param>
        /// <returns>Json string</returns>
        public static string DumpToJson(KeyActionInfo info)
        {
            return JsonConvert.SerializeObject(DumpToObject(info));
        }

        public static KeyActionInfo ParseFromKeyEventArgs(KeyActionType action_type, KeyEventArgs args)
        {
            return new KeyActionInfo()
            {
                ActionType = action_type,
                Key = args.KeyCode.ToString(),
                KeyCode = args.KeyValue,
                IsAltDown = args.Alt,
                IsShiftDown = args.Shift,
                IsCtrlDown = args.Control
            };
        }
    }
}
