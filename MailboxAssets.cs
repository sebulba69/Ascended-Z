using AscendedZ.game_object.mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class MailboxAssets
    {
        public static string CONTROL_TITLE = "Controls";
        public static string PRESS_TURN_TITLE = "Combat";
        public static string PRESS_TURN_TITLE_2 = "Combat (Advanced)";
        public static string RECRUITING = "Custom Recruiting";
        public static string FUSIONS = "Fusion Basics";
        public static string FUSIONS_CONT = "Fusions (cont.)";
        public static string REC_FUSIONS = "Recruit Fusions";

        private static string CONTROL_TUTORIAL = "res://screens/teleport/Controls.tscn";
        private static string PRESS_TURN_TUTORIAL_1 = "res://screens/teleport/PressTurnTutorial.tscn";
        private static string PRESS_TURN_TUTORIAL_2 = "res://screens/teleport/PressTurnAdvanced.tscn";
        private static string CUSTOM_RECRUITING_SCREEN = "res://screens/teleport/CustomRecruiting.tscn";
        private static string FUSIONS_1 = "res://screens/teleport/FusionTutorial.tscn";
        private static string FUSIONS_2 = "res://screens/teleport/FusionTutorial2.tscn";
        private static string FUSIONS_3 = "res://screens/teleport/FusionTutorial3.tscn";

        // Title, Mail
        public static Dictionary<string, Mail> GameMail = new Dictionary<string, Mail>()
        {
            { CONTROL_TITLE, new Mail() { Title = CONTROL_TITLE, MailScene = CONTROL_TUTORIAL}},
            { PRESS_TURN_TITLE, new Mail() { Title = PRESS_TURN_TITLE, MailScene = PRESS_TURN_TUTORIAL_1}},
            { PRESS_TURN_TITLE_2, new Mail() { Title = PRESS_TURN_TITLE_2, MailScene = PRESS_TURN_TUTORIAL_2}},
            { RECRUITING, new Mail() { Title = RECRUITING, MailScene = CUSTOM_RECRUITING_SCREEN}},
            { FUSIONS, new Mail() { Title = FUSIONS, MailScene = FUSIONS_1}},
            { FUSIONS_CONT, new Mail() { Title = FUSIONS_CONT, MailScene = FUSIONS_2}},
            { REC_FUSIONS, new Mail() { Title = REC_FUSIONS, MailScene = FUSIONS_3}},
        };
    }
}
