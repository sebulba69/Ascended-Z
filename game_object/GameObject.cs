using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.sigils;
using AscendedZ.game_object.mail;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class GameObject
    {
        private int _tierCap = 251;
        private int _tierCapDC = 251;

        private int _tier = 1;
        private int _tierDC = 1;
        private int _maxTier = 1;
        private int _maxTierDC = 1;
        private int _shopLevel = 0;
        private bool _partyMemberObtained = false;


        public int TierCap { get => _tierCap; set => _tierCap = value; }

        public int TierDCCap { get => _tierCapDC; set => _tierCapDC = value; }

        /// <summary>
        /// The current floor you're on as displayed to you by the UI.
        /// </summary>
        public int Tier
        {
            get => _tier;
            set
            {
                _tier = value;
                if (_tier >= MaxTier)
                    _tier = MaxTier;

                if (_tier < 1)
                    _tier = 1;
            }
        }

        /// <summary>
        /// The current floor you're on as displayed to you by the UI.
        /// </summary>
        public int TierDC
        {
            get => _tierDC;
            set
            {
                _tierDC = value;
                if (_tierDC >= MaxTierDC)
                    _tierDC = MaxTierDC;

                if (_tierDC < 1)
                    _tierDC = 1;
            }
        }

        /// <summary>
        /// The highest possible tier you can get to at your current point in the game.
        /// </summary>
        public int MaxTier
        {
            get => _maxTier;
            set
            {
                _maxTier = value;
            }
        }

        /// <summary>
        /// The highest possible dungeon crawling tier you can get to at your current point in the game.
        /// </summary>
        public int MaxTierDC
        {
            get => _maxTierDC;
            set
            {
                _maxTierDC = value;
            }
        }

        public int RandomBossIndex { get; set; }

        public int ShopLevel 
        {
            get
            {
                if (_shopLevel > PersistentGameObjects.SHOP_CAP)
                    return PersistentGameObjects.SHOP_CAP;
                else
                    return _shopLevel;
            }
            set
            { 
                _shopLevel = value; 
                if(_shopLevel > PersistentGameObjects.SHOP_CAP)
                    _shopLevel = PersistentGameObjects.SHOP_CAP;
            } 
        }

        private int _startLevelCap = 100;
        public int SigilLevelCap { get => _startLevelCap; set => _startLevelCap = value; }

        public MainPlayer MainPlayer { get; set; }
        public int Orbs { get; set; }
        public int Pickaxes { get; set; }
        public MusicObject MusicPlayer { get; set; }
        public LabrybuceInventoryObject LabrybuceInventoryObject { get; set; }
        public ProgressFlagObject ProgressFlagObject { get; set; }
        public CutsceneObject CutsceneObject { get; set; }
        public Dictionary<int, List<string>> TierEnemyEncounters { get; set; }
        public Dictionary<int, Enemy> RandomizedBossEncounters { get; set; }
        // Load by tier
        public Dictionary<int, Dictionary<string, Enemy>> TikkiBosses { get; set; }
        /// <summary>
        /// Tier, List of names
        /// </summary>
        public Dictionary<int, HashSet<string>> DefeatedTikkis { get; set; }
        public HashSet<int> DefeatedGoldBoss { get; set; }
        public Dictionary<int, HashSet<string>> DiscoveredFusions { get; set; }
        public HashSet<string> ImportantFights { get; set; }
        public List<bool> Checkboxes { get; set; }
        public Mailbox Mail { get; set; }
        public bool EndAscended { get; set; } = false;
        public GameObject()
        {
            MusicPlayer = new MusicObject();
            LabrybuceInventoryObject = new LabrybuceInventoryObject();
            ProgressFlagObject = new ProgressFlagObject();
            CutsceneObject = new CutsceneObject();
            TierEnemyEncounters = new Dictionary<int, List<string>>();
            RandomizedBossEncounters = new Dictionary<int, Enemy>();
            TikkiBosses = new();
            DefeatedTikkis = new();
            DefeatedGoldBoss = new();
            ImportantFights = new();
            Mail = new Mailbox();

            if(DiscoveredFusions == null)
            {
                DiscoveredFusions = new Dictionary<int, HashSet<string>>();
                for (int fg = 1; fg <= MiscGlobals.FUSION_GRADE_CAP; fg++)
                {
                    DiscoveredFusions.Add(fg, new HashSet<string>());
                }
            }

            Checkboxes = new List<bool>();

            for(int i = 0; i <= MiscGlobals.FUSION_GRADE_CAP; i++)
            {
                Checkboxes.Add(true); 
            }

            ShopLevel = 0;
        }

        public List<BattlePlayer> MakeBattlePlayerListFromParty()
        {
            List<BattlePlayer> players = new List<BattlePlayer>();
            foreach (var member in MainPlayer.Party.Party)
            {
                if (member != null)
                {
                    players.Add(member.MakeBattlePlayer());
                }
            }
            return players;
        }
    }
}
