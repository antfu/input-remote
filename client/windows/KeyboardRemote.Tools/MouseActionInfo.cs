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
        public MouseButton? Button { get; set; }


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
                case "buttondown":
                    return MouseActionType.ButtonDown;
                case "buttonup":
                    return MouseActionType.ButtonUp;
            }
            return MouseActionType.None;
        }
        public static MouseButton? GetButton(string button)
        {
            switch (button)
            {
                case "left":
                    return MouseButton.Left;
                case "right":
                    return MouseButton.Right;
                case "middle":
                    return MouseButton.Middle;
            }
            return null;
        }
        public static MouseActionInfo ParseFromJsonObject(dynamic data)
        {
            return new MouseActionInfo()
            {
                ActionType = GetActionType((string)data.action),
                X = data.x,
                Y = data.y,
                Button = GetButton((string)data.button)
            };
        }
    }
}
