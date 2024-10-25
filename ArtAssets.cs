
using AscendedZ.battle;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.entities;
using AscendedZ.skills;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class SigilAssets
    {
        private static List<string> _sigilImages;
        private static string SIGIL_IMAGES = "res://misc_icons/sigil_images/";
        public static List<string> SigilImages
        {
            get 
            {
                if(_sigilImages == null)
                {
                    _sigilImages = new List<string>();
                    AssetUtil.LoadAssets(SIGIL_IMAGES, _sigilImages);
                }

                return _sigilImages;
            }
        }
    }

    public static class BackgroundAssets
    {
        private static List<string> _overworldBgs, _combatBgs, _combatBgsDC;
        private static readonly string CG_BGs = "res://cg_backgrounds/";

        public static List<string> OverworldBgs
        {
            get
            {
                if (_overworldBgs == null)
                {
                    _overworldBgs = new List<string>();
                    AssetUtil.LoadAssets(Path.Combine(CG_BGs, "overworld"), _overworldBgs);
                }

                return _overworldBgs;
            }
        }

        public static List<string> CombatBgs 
        {
            get 
            {
                if(_combatBgs == null)
                {
                    _combatBgs = new List<string>();
                    AssetUtil.LoadAssets(Path.Combine(CG_BGs, "dungeon"), _combatBgs);
                }

                return _combatBgs;
            }
        }

        public static List<string> CombatBgsDC 
        {
            get 
            {
                if(_combatBgsDC == null)
                {
                    _combatBgsDC = new List<string>();
                    AssetUtil.LoadAssets(Path.Combine(CG_BGs, "dungeon_crawling"), _combatBgsDC);
                }

                return _combatBgsDC;
            }
        }

        private static readonly List<string> _templates =
        [ 
            "ffffff", "0ed684",
            "8f5ad4", "9f85ff",
            "01ffbc", "24fff8",
            "ffb2ff", "d08aff",
            "00f5a0", "ffff76", "c9c9c9b4", "ff4646c1"
        ];

        public static string GetBackground(int tier, bool isEndgame)
        {
            int index = Equations.GetTierIndexBy10(tier);
            if (index >= OverworldBgs.Count - 1 && !isEndgame)
                index = OverworldBgs.Count - 2;
            else if (index >= OverworldBgs.Count && isEndgame)
                index = OverworldBgs.Count - 1;

            return OverworldBgs[index];
        }

        public static string GetCombatBackground(int tier)
        {
            int index = Equations.GetTierIndexBy10(tier);
            return CombatBgs[index];
        }

        public static string GetCombatDCBackground(int tier)
        {
            int index = Equations.GetTierIndexBy50(tier);

            if (index >= CombatBgsDC.Count)
                index = CombatBgsDC.Count - 1;

            return CombatBgsDC[index];
        }

        public static string GetCombatDCTileTemplate(int tier)
        {
            int index = Equations.GetTierIndexBy25(tier);

            if (index >= _templates.Count)
                index = _templates.Count - 1;

            return _templates[index];
        }
    }

    /// <summary>
    /// Quick access to art assets using res paths
    /// </summary>
    public static class CharacterImageAssets
    {

        private static List<string> _playerPics;

        public static List<string> PlayerPics
        {
            get
            {
                if(_playerPics == null)
                {
                    _playerPics = new List<string>();

                    AssetUtil.LoadAssets("res://player_pics/", _playerPics);
                }

                return _playerPics;
            }
        }

        public static string GetImagePath(string name)
        {
            string nameToLower = name.ToLower();
            return $"res://entity_pics/{nameToLower}.png";
        }

        public static Texture2D GetTextureForItemList(string imagePath)
        {
            Texture2D texture = ResourceLoader.Load<Texture2D>(imagePath);

            Image image = texture.GetImage();
            image.Resize(32, 32);

            return ImageTexture.CreateFromImage(image);
        }
    }

    public static class SkillAssets
    {
        // Startups
        public static readonly string STARTUP1_MG = "startup_anim";

        // Effect Animations
        public static readonly string WIND_T1 = "wind1";
        public static readonly string FIRE_T1 = "fire1";
        public static readonly string ELEC_T1 = "elec1";
        public static readonly string ICE_T1 = "ice1";
        public static readonly string LIGHT_T1 = "light1";
        public static readonly string DARK_T1 = "dark1";

        public static readonly string WIND_T2 = "wind2";
        public static readonly string FIRE_T2 = "fire2";
        public static readonly string ELEC_T2 = "elec2";
        public static readonly string ICE_T2 = "ice2";
        public static readonly string LIGHT_T2 = "light2";
        public static readonly string DARK_T2 = "dark2";

        public static readonly string ICE_BUFF = "ice_buff";
        public static readonly string ELEC_BUFF = "elec_buff";
        public static readonly string FIRE_BUFF = "fire_buff";
        public static readonly string WIND_BUFF = "wind_buff";
        public static readonly string LIGHT_BUFF = "light_buff";
        public static readonly string DARK_BUFF = "dark_buff";

        public static readonly string FLAT_BUFF = "flat_buff";
        public static readonly string FLAT_DEBUFF = "flat_debuff";
        public static readonly string ATK_BUFF = "atk_buff";
        public static readonly string TECH_BUFF = "tech_buff";
        public static readonly string ALMIGHTY = "almighty_all";

        public static readonly string HEAL_T1 = "heal1";
        public static readonly string REVIVE = "revive";
        public static readonly string STUN_T1 = "stun1";
        public static readonly string AGRO = "agro";
        public static readonly string POISON = "poison";
        public static readonly string BIND = "bind";
        public static readonly string SEAL = "seal";

        public static readonly string EYESKILLANIM = "eyeskill";
        public static readonly string STATUS_RECOVER = "status_recovery";
        public static readonly string VOID_SHIELD = "void_shield";
        public static readonly string POISON_ICON = "poison";

        // ICONS + ICON_STRINGS
        public static readonly string ICON_ATLAS = "res://misc_icons/IconSet.png";

        // Skill Icons - Elements
        public static readonly string FIRE_ICON = "Fire";
        public static readonly string ICE_ICON = "Ice";
        public static readonly string ELEC_ICON = "Elec";
        public static readonly string WIND_ICON = "Wind";
        public static readonly string LIGHT_ICON = "Light";
        public static readonly string DARK_ICON = "Dark";

        // Skill Icons - Voids
        public static readonly string VOID_FIRE_ICON = "VoidFire";
        public static readonly string VOID_ICE_ICON = "VoidIce";
        public static readonly string VOID_WIND_ICON = "VoidWind";
        public static readonly string VOID_DARK_ICON = "VoidDark";
        public static readonly string VOID_LIGHT_ICON = "VoidLight";
        public static readonly string VOID_ELEC_ICON = "VoidElec";

        public static readonly string BEAST_EYE = "BeastEye";

        // Skill Icons - Weaks
        public static readonly string WEAK_FIRE_ICON = "WeakFire";
        public static readonly string WEAK_ELEC_ICON = "WeakElec";
        public static readonly string WEAK_ICE_ICON = "WeakIce";
        public static readonly string WEAK_DARK_ICON = "WeakDark";
        public static readonly string WEAK_WIND_ICON = "WeakWind";

        // Skill Icons - Other
        public static readonly string HEAL_ICON = "Heal";
        public static readonly string STUN_ICON = "Stun";
        public static readonly string AGRO_ICON = "Agro";
        public static readonly string BIND_ICON = "Bind";
        public static readonly string SEAL_ICON = "Seal";
        public static readonly string PASS_ICON = "Pass";
        public static readonly string GUARD_ICON = "Guard";
        public static readonly string RETREAT_ICON = "Retreat";

        public static readonly string MAGIC_ICON = "Magic";

        public static readonly string QUEST_ICON = "Quest";

        // Currency Icons
        public static readonly string VORPEX_ICON = "Vorpex";
        public static readonly string PARTY_COIN_ICON = "Party Coins";
        public static readonly string DELLENCOIN = "Dellencoin";
        public static readonly string PROOF_OF_ASCENSION_ICON = "Proof of Ascension";
        public static readonly string PROOF_OF_BUCE_ICON = "Proof of Buce";
        public static readonly string KEY_SHARD = "Key Shard";
        public static readonly string BOUNTY_KEY = "Bounty Key";
        public static readonly string MORBIS = "Morbis";
        public static readonly string ELDER_KEY_ICON = "Elder Key";

        // Buff Skills
        public static readonly string TECH_PLUS_ICON = "Tech+";
        public static readonly string TECH_MINUS_ICON = "Tech-";
        public static readonly string EVADE_PLUS_ICON = "Evade+";
        public static readonly string EVADE_MINUS_ICON = "Evade-";
        public static readonly string ATK_PLUS_ICON = "ATK+";
        public static readonly string ATK_MINUS_ICON = "ATK-";
        public static readonly string DEF_PLUS_ICON = "DEF+";
        public static readonly string DEF_MINUS_ICON = "DEF-";
        public static readonly string BUFF_BOOST_ICON = "BUFF_BOOST";
        public static readonly string VICTORY_CRY_ICON = "VICTORY_CRY";
        public static readonly string DEBUFF_BOOST_ICON = "DEBUFF_BOOST";
        public static readonly string LUSTER_CANDY = "LUSTER_CANDY";
        public static readonly string DEBILITATE = "DEBILITATE";

        // Buff Statuses
        public static readonly string TECH_STATUS_ICON = "Technicals";
        public static readonly string CONFUSION = "Evasions";
        public static readonly string FOCUS_ICON = "Focus";
        public static readonly string MARK_OF_DEATH_STATUS = "MarkOfDeath";
        public static readonly string ATK_STATUS_ICON = "Attack";
        public static readonly string DEF_STATUS_ICON = "Defense";
        public static readonly string ALMIGHT_ICON = "Almighty";

        // items
        public static readonly string TP_STICK_EXIT_ICON = "Staff";
        public static readonly string TP_TO_BOSS_ROOM = "Boss";
        public static readonly string HP_ITEM_ICON = "HPItem";
        public static readonly string HPR_ITEM_ICON = "HPRItem";
        // Silent Prayer
        public static readonly string SILENT_PRAYER = "SilentPrayer";

        // GB Statuses
        public static readonly string LIFETRACK = "LifeTrack";

        public static readonly System.Collections.Generic.Dictionary<string, KeyValuePair<int, int>> ICONS = new System.Collections.Generic.Dictionary<string, KeyValuePair<int, int>>()
        {
            [FIRE_ICON] = new KeyValuePair<int, int>(0, 128),
            [VOID_FIRE_ICON] = new KeyValuePair<int, int>(32, 2144),
            [VOID_ICE_ICON] = new KeyValuePair<int, int>(224, 2144),
            [VOID_WIND_ICON] = new KeyValuePair<int, int>(128, 2144),
            [VOID_LIGHT_ICON] = new KeyValuePair<int, int>(416, 2144),
            [VOID_DARK_ICON] = new KeyValuePair<int, int>(448, 2144),
            [VOID_ELEC_ICON] = new KeyValuePair<int, int>(96, 2144),
            [POISON_ICON] = new KeyValuePair<int, int>(64, 0),
            [ICE_ICON] = new KeyValuePair<int, int>(32, 128),
            [ELEC_ICON] = new KeyValuePair<int, int>(64, 128),
            [WIND_ICON] = new KeyValuePair<int, int>(160, 128),
            [LIGHT_ICON] = new KeyValuePair<int, int>(192, 128),
            [DARK_ICON] = new KeyValuePair<int, int>(224, 128),
            [HEAL_ICON] = new KeyValuePair<int, int>(256, 128),
            [STUN_ICON] = new KeyValuePair<int, int>(288, 0),
            [AGRO_ICON] = new KeyValuePair<int, int>(480, 0),
            [PASS_ICON] = new KeyValuePair<int, int>(352, 128),
            [GUARD_ICON] = new KeyValuePair<int, int>(32, 160),
            [RETREAT_ICON] = new KeyValuePair<int, int>(288, 576),
            [VORPEX_ICON] = new KeyValuePair<int, int>(448, 1376),
            [PARTY_COIN_ICON] = new KeyValuePair<int, int>(32, 288),
            [DELLENCOIN] = new KeyValuePair<int, int>(320, 608),
            [KEY_SHARD] = new KeyValuePair<int, int>(160, 576),
            [BOUNTY_KEY] = new KeyValuePair<int, int>(96, 384),
            [MORBIS] = new KeyValuePair<int, int>(416, 1632),
            [WEAK_FIRE_ICON] = new KeyValuePair<int, int>(32, 1600),
            [WEAK_ELEC_ICON] = new KeyValuePair<int, int>(96, 1600),
            [WEAK_ICE_ICON] = new KeyValuePair<int, int>(192, 1600),
            [WEAK_DARK_ICON] = new KeyValuePair<int, int>(448, 1600),
            [WEAK_WIND_ICON] = new KeyValuePair<int, int>(128, 1600),
            [BEAST_EYE] = new KeyValuePair<int, int>(192, 224),
            [QUEST_ICON] = new KeyValuePair<int, int>(480, 352),
            [MAGIC_ICON] = new KeyValuePair<int, int>(384, 480),
            [TECH_STATUS_ICON] = new KeyValuePair<int, int>(416, 480),
            [CONFUSION] = new KeyValuePair<int, int>(192, 0),
            [TECH_PLUS_ICON] = new KeyValuePair<int, int>(160, 64),
            [TECH_MINUS_ICON] = new KeyValuePair<int, int>(160, 96),
            [EVADE_PLUS_ICON] = new KeyValuePair<int, int>(192, 64),
            [EVADE_MINUS_ICON] = new KeyValuePair<int, int>(192, 96),
            [ATK_PLUS_ICON] = new KeyValuePair<int, int>(64, 64),
            [ATK_MINUS_ICON] = new KeyValuePair<int, int>(64, 96),
            [DEF_PLUS_ICON] = new KeyValuePair<int, int>(96, 64),
            [DEF_MINUS_ICON] = new KeyValuePair<int, int>(96, 96),
            [ATK_STATUS_ICON] = new KeyValuePair<int, int>(320, 480),
            [DEF_STATUS_ICON] = new KeyValuePair<int, int>(352, 480),
            [ALMIGHT_ICON] = new KeyValuePair<int, int>(0, 160),
            [TP_STICK_EXIT_ICON] = new KeyValuePair<int, int>(288, 352),
            [TP_TO_BOSS_ROOM] = new KeyValuePair<int, int>(160, 192),
            [HP_ITEM_ICON] = new KeyValuePair<int, int>(0, 704),
            [HPR_ITEM_ICON] = new KeyValuePair<int, int>(128, 800),
            [LUSTER_CANDY] = new KeyValuePair<int, int>(32, 64),
            [DEBILITATE] = new KeyValuePair<int, int>(32, 96),
            [BIND_ICON] = new KeyValuePair<int, int>(416, 0),
            [SEAL_ICON] = new KeyValuePair<int, int>(128, 0),
            [LIFETRACK] = new KeyValuePair<int, int>(128, 160),
            [SILENT_PRAYER] = new KeyValuePair<int, int>(480, 128),
            [PROOF_OF_ASCENSION_ICON] = new KeyValuePair<int, int>(64, 1952),
            [PROOF_OF_BUCE_ICON] = new KeyValuePair<int, int>(256, 1952),
            [BUFF_BOOST_ICON] = new KeyValuePair<int, int>(288, 64),
            [DEBUFF_BOOST_ICON] = new KeyValuePair<int, int>(288, 96),
            [FOCUS_ICON] = new KeyValuePair<int, int>(480, 224),
            [MARK_OF_DEATH_STATUS] = new KeyValuePair<int, int>(320, 0),
            [ELDER_KEY_ICON] = new KeyValuePair<int, int>(32, 448),
            [VICTORY_CRY_ICON] = new KeyValuePair<int, int>(416, 128),
        };

        public static KeyValuePair<int, int> GetIcon(string key)
        {
            if (string.IsNullOrEmpty(key) || !ICONS.ContainsKey(key))
                throw new Exception($"{key} not present in dictionary.");

            return ICONS[key];
        }

        public static string GetElementIconByElementEnum(Elements element)
        {
            switch (element)
            {
                case Elements.Fire:
                    return FIRE_ICON;
                case Elements.Ice:
                    return ICE_ICON;
                case Elements.Elec:
                    return ELEC_ICON;
                case Elements.Wind:
                    return WIND_ICON;
                case Elements.Light:
                    return LIGHT_ICON;
                case Elements.Dark:
                    return DARK_ICON;
                case Elements.Almighty:
                    return ALMIGHT_ICON;
                default:
                    throw new Exception($"Element, {element.ToString()}, does not have icon.");
            }
        }

        public static string GetAnimationByElementAndTier(int tier, Elements element)
        {
            var tier1Animations = new System.Collections.Generic.Dictionary<Elements, string> 
            { 
                { Elements.Fire, FIRE_T1 },
                { Elements.Ice, ICE_T1 },
                { Elements.Wind, WIND_T1 },
                { Elements.Elec, ELEC_T1 },
                { Elements.Dark, DARK_T1 },
                { Elements.Light, LIGHT_T1 }, 
                { Elements.Almighty, ALMIGHTY } 
            };

            var tier2Animations = new System.Collections.Generic.Dictionary<Elements, string> 
            {
                { Elements.Fire, FIRE_T2 },
                { Elements.Ice, ICE_T2 },
                { Elements.Wind, WIND_T2 },
                { Elements.Elec, ELEC_T2 },
                { Elements.Dark, DARK_T2 },
                { Elements.Light, LIGHT_T2 },
                { Elements.Almighty, ALMIGHTY }
            };

            string animation = string.Empty;

            switch (tier)
            {
                case 1:
                    animation = tier1Animations[element];
                    break;
                case 2:
                    animation = tier2Animations[element];
                    break;
                default:
                    animation = tier1Animations[element];
                    break;
            }

            return animation;
        }

        public static string GetBuffAnimationByElement(Elements element)
        {
            var buffAnimations = new System.Collections.Generic.Dictionary<Elements, string>
            {
                { Elements.Fire, FIRE_BUFF },
                { Elements.Ice, ICE_BUFF },
                { Elements.Wind, WIND_BUFF },
                { Elements.Elec, ELEC_BUFF },
                { Elements.Dark, DARK_BUFF },
                { Elements.Light, LIGHT_BUFF }
            };

            return buffAnimations[element];
        }

        private static readonly Texture2D _atlas = ResourceLoader.Load<Texture2D>("res://misc_icons/IconSet.png");

        public static AtlasTexture GenerateIcon(string iconKey)
        {
            KeyValuePair<int, int> coords = GetIcon(iconKey);

            AtlasTexture icon = new AtlasTexture();
            icon.Atlas = _atlas;
            icon.Region = new Rect2(coords.Key, coords.Value, 32, 32);
            return icon;
        }
    }
}
