using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardRemote.Tools
{
    public class MouseActionInfo
    {
        public MouseActionType ActionType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        
        public static MouseActionType GetActionType(string action)
        {
            switch(action)
            {
                case "start":
                    return MouseActionType.MoveStart;
                case "end":
                    return MouseActionType.MoveEnd;
                case "move":
                    return MouseActionType.Move;
                case "left_mouse_down":
                    return MouseActionType.LeftMouseDown;
                case "left_mouse_up":
                    return MouseActionType.LeftMouseUp;
                case "right_mouse_down":
                    return MouseActionType.RightMouseDown;
                case "right_mouse_up":
                    return MouseActionType.RightMouseDown;
            }
            return MouseActionType.None;
        }
        public static MouseActionInfo ParseFromJsonObject(dynamic data)
        {
            return new MouseActionInfo()
            {
                ActionType = GetActionType((string)data.action),
                X = data.x,
                Y = data.y
            };
        }
    }
}
