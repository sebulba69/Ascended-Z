using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class Controls
    {
        public static readonly string UP = "up";
        public static readonly string DOWN = "down";
        public static readonly string LEFT = "left";
        public static readonly string RIGHT = "right";
        public static readonly string INVENTORY = "inventory";

        public static readonly string CONFIRM = "menuConfirm";
        public static readonly string EMBARK = "embark";
        public static readonly string BACK = "menuBack";
        public static readonly string SKIP = "retryEncounter";
        public static readonly string RETREAT = "retreat";
        public static readonly string MINE = "mine";

        private static Dictionary<string, string> _controlMap = new Dictionary<string, string>
        {
            { CONFIRM, "Z/Enter" },
            { BACK, "X" },
            { SKIP, "C" },
            { EMBARK, "E" },
            { RETREAT, "ESC" },
            { MINE, "Q" }
        };

        public static string GetControlString(string control)
        {
            return _controlMap[control];
        }
    }
}
