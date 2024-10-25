using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class SfxAssets
    {
        private static string STARTUP = "res://effects/effect_sounds/Push.ogg";
        private static string AGRO = "res://effects/effect_sounds/Magic1.ogg";

        private static string DARKNESS1 = "res://effects/effect_sounds/Darkness3.ogg";
        private static string DARKNESS2 = "res://effects/effect_sounds/Darkness1.ogg";
        private static string DARKNESS3 = "res://effects/effect_sounds/Darkness4.ogg";

        private static string ELEC1 = "res://effects/effect_sounds/Thunder1.ogg";
        private static string ELEC2 = "res://effects/effect_sounds/Thunder2.ogg";
        private static string ELECBUFF = "res://effects/effect_sounds/Thunder3.ogg";

        private static string FIRE1 = "res://effects/effect_sounds/Fire1.ogg";
        private static string FIRE2 = "res://effects/effect_sounds/Fire2.ogg";
        private static string FIRE_BUFF = "res://effects/effect_sounds/Fire3.ogg";

        private static string ICE1 = "res://effects/effect_sounds/Ice4.ogg";
        private static string ICE2 = "res://effects/effect_sounds/Ice3.ogg";
        private static string ICEBUFF = "res://effects/effect_sounds/Ice2.ogg";

        private static string LIGHT1 = "res://effects/effect_sounds/Magic4.ogg";
        private static string LIGHT2 = "res://effects/effect_sounds/Magic2.ogg";
        private static string LIGHT3 = "res://effects/effect_sounds/Skill3.ogg";

        private static string WIND1 = "res://effects/effect_sounds/wind_1.ogg";
        private static string WIND2 = "res://effects/effect_sounds/Wind2.ogg";
        private static string WIND3 = "res://effects/effect_sounds/Wind5.ogg";

        private static string HEAL1 = "res://effects/effect_sounds/Heal3.ogg";
        private static string RECOVERY = "res://effects/effect_sounds/Recovery.ogg";
        private static string REVIVE = "res://effects/effect_sounds/Raise2.ogg";
        private static string POISON = "res://effects/effect_sounds/Poison.ogg";
        private static string STUN = "res://effects/effect_sounds/Paralyze3.ogg";
        private static string VOID = "res://effects/effect_sounds/Magic4.ogg";
        private static string EYE_SKILL = "res://effects/effect_sounds/Monster1.ogg";

        private static string FLAT_BUFF = "res://effects/effect_sounds/Skill1.ogg";
        private static string FLAT_DEBUFF = "res://effects/effect_sounds/Down2.ogg";
        private static string ALMIGHTY = "res://effects/effect_sounds/Saint4.ogg";

        /// <summary>
        /// Animation + Sfx
        /// </summary>
        public static readonly Dictionary<string, string> EFFECT_SOUNDS = new Dictionary<string, string>()
        {
            { SkillAssets.STARTUP1_MG, STARTUP },
            { SkillAssets.AGRO, AGRO },
            { SkillAssets.FIRE_T1, FIRE1 },
            { SkillAssets.FIRE_T2, FIRE2 },
            { SkillAssets.FIRE_BUFF, FIRE_BUFF },
            { SkillAssets.ICE_T1, ICE1 },
            { SkillAssets.ICE_T2, ICE2 },
            { SkillAssets.ICE_BUFF, ICEBUFF },
            { SkillAssets.WIND_T1, WIND1 },
            { SkillAssets.WIND_T2, WIND2 },
            { SkillAssets.WIND_BUFF, WIND3 },
            { SkillAssets.ELEC_T1, ELEC1 },
            { SkillAssets.ELEC_T2, ELEC2 },
            { SkillAssets.ELEC_BUFF, ELECBUFF },
            { SkillAssets.LIGHT_T1, LIGHT1 },
            { SkillAssets.LIGHT_T2, LIGHT2 },
            { SkillAssets.LIGHT_BUFF, LIGHT3 },
            { SkillAssets.DARK_T1, DARKNESS1 },
            { SkillAssets.DARK_T2, DARKNESS2 },
            { SkillAssets.DARK_BUFF, DARKNESS3 },
            { SkillAssets.HEAL_T1, HEAL1 },
            { SkillAssets.REVIVE, REVIVE },
            { SkillAssets.STATUS_RECOVER, RECOVERY },
            { SkillAssets.POISON, POISON },
            { SkillAssets.EYESKILLANIM, EYE_SKILL },
            { SkillAssets.STUN_T1, STUN },
            { SkillAssets.VOID_SHIELD, VOID },
            { SkillAssets.FLAT_BUFF, FLAT_BUFF },
            { SkillAssets.ATK_BUFF, LIGHT3 },
            { SkillAssets.TECH_BUFF, DARKNESS3 },
            { SkillAssets.FLAT_DEBUFF, FLAT_DEBUFF },
            { SkillAssets.ALMIGHTY, ALMIGHTY },
            { SkillAssets.BIND, AGRO},
            { SkillAssets.SEAL, AGRO},
        };
    }
}
