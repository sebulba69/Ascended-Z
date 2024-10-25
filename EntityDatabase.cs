using AscendedZ.entities;
using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.entities.enemy_objects.bosses;
using AscendedZ.entities.enemy_objects.enemy_makers;
using AscendedZ.entities.partymember_objects;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using Godot.Collections;

namespace AscendedZ
{
    /// <summary>
    /// This class is to be used when we want to access Enemies, Party Members, Bosses, etc.
    /// </summary>
    public static class EntityDatabase
    {
        private static readonly Random RANDOM = new Random();
        private static readonly bool DEBUG = false;
        /// <summary>
        /// The max tier where we start generating enemies randomly.
        /// </summary>
        private static readonly int RANDOM_TIER = 5;

        private static EnemyMaker _enemyMaker = new EnemyMaker();

        private static readonly List<string>[] TUTORIAL_ENCOUNTERS = new List<string>[]
        {
            new List<string>(){ EnemyNames.Conlen },
            new List<string>(){ EnemyNames.Liamlas, EnemyNames.Orachar },
            new List<string>(){ EnemyNames.Fastrobren, EnemyNames.Conlen, EnemyNames.Liamlas },
            new List<string>(){ EnemyNames.CattuTDroni, EnemyNames.Orachar, EnemyNames.Conlen  },
        };

        /// <summary>
        /// Boss encounters for every 10 floors
        /// </summary>
        private static readonly List<string>[] BOSS_ENCOUNTERS =
        [
            [EnemyNames.Harbinger],
            [EnemyNames.Elliot_Onyx],
            [EnemyNames.Sable_Vonner],
            [EnemyNames.Cloven_Umbra],
            [EnemyNames.Ashen_Ash],
            [EnemyNames.Ethel_Aura],
            [EnemyNames.Kellam_Von_Stein],
            [EnemyNames.Drace_Skinner],
            [EnemyNames.Jude_Stone],
            [EnemyNames.Drace_Razor],
            [EnemyNames.Everit_Pickerin],
            [EnemyNames.Alex_Church],
            [EnemyNames.Bohumir_Cibulka, EnemyNames.Griffen_Hart],
            [EnemyNames.Zell_Grimsbane],
            [EnemyNames.Soren_Winter],
            [EnemyNames.Requiem_Heliot],
            [EnemyNames.Not],
            [EnemyNames.Sable_Craft],
            [EnemyNames.Cinder_Morgan, EnemyNames.Granger_Barlow],
            [EnemyNames.Thorne_Lovelace],
            [EnemyNames.Morden_Brack],
            [EnemyNames.Law_Vossen],
            [EnemyNames.Arc_Hunt],
            [EnemyNames.Buceala],
            [EnemyNames.Storm_Vossen]
        ];

        private static readonly List<string>[] DUNGEON_BOSS_ENCOUNTERS =
        {
            [EnemyNames.Ocura],
            [EnemyNames.Emush],
            [EnemyNames.Hrothstyr_Zarmor, EnemyNames.Logvat, EnemyNames.Theodstin_Glove],
            [EnemyNames.Pleromyr],
            [EnemyNames.Kodek],
            [EnemyNames.Ancient_Nodys],
            [EnemyNames.Pakorag],
            [EnemyNames.Iminth],
            [EnemyNames.Sentinal],
            [EnemyNames.Floor_Architect]
        };

        /// <summary>
        /// Based on tier. Each one is cumulative based on the tier.
        /// Entry [1] would be included with entry [0] and so on.
        /// </summary>
        private static readonly List<string>[] VENDOR_WARES = new List<string>[]
        {
            new List<string>(){ PartyNames.Locphiedon },
            new List<string>(){ PartyNames.Gagar},
            new List<string>(){ PartyNames.Maxwald },
            new List<string>(){ PartyNames.Yuudam },
            new List<string>(){ PartyNames.Pecheal, PartyNames.Toke },
            new List<string>(){ PartyNames.Halvia }
        };

        private static readonly List<string> CUSTOM_WARES = new List<string>
        {
            EnemyNames.Conlen, PartyNames.Andmond, PartyNames.Joan, PartyNames.Tyhere, PartyNames.Paria, EnemyNames.Isenald
        };

        /// <summary>
        /// A list of indexes that the current tier must be equal to or greater than
        /// to become available in the shop.
        /// </summary>
        private static readonly int[] SHOP_INDEXES = new int[] { 1, 2, 3, 5, 11, 13 };

        /// <summary>
        /// List based off the element each character is strong to.
        /// </summary>
        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION1_RESULTS = new() 
        {
            { Elements.Fire, PartyNames.Ancrow },
            { Elements.Ice, PartyNames.Candun},
            { Elements.Wind, PartyNames.Samlin},
            { Elements.Elec, PartyNames.Ciavid },
            { Elements.Light, PartyNames.Conson },
            { Elements.Dark, PartyNames.Cermas }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION2_RESULTS = new()
        {
            { Elements.Fire, PartyNames.Marchris },
            { Elements.Ice, PartyNames.Thryth },
            { Elements.Wind, PartyNames.Everever },
            { Elements.Elec, PartyNames.Eri },
            { Elements.Light, PartyNames.Winegeful },
            { Elements.Dark, PartyNames.Fledron }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION3_RESULTS = new()
        {
            { Elements.Fire, PartyNames.Ride},
            { Elements.Ice,  PartyNames.Shacy},
            { Elements.Wind, PartyNames.Lesdan},
            { Elements.Elec,  PartyNames.Tinedo},
            { Elements.Light,  PartyNames.Earic},
            { Elements.Dark,  PartyNames.Baring }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION4_RESULTS = new()
        {
            { Elements.Fire, PartyNames.Muelwise},
            { Elements.Ice, PartyNames.Swithwil},
            { Elements.Wind,  PartyNames.Ronboard},
            { Elements.Elec, PartyNames.Xtrasu },
            { Elements.Light, PartyNames.LatauVHurquij},
            { Elements.Dark, PartyNames.Tami }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION5_RESULTSA = new()
        {
            { Elements.Fire, PartyNames.Pher },
            { Elements.Ice,  PartyNames.Isenann},
            { Elements.Wind, PartyNames.Dosam },
            { Elements.Elec, PartyNames.Laanard },
            { Elements.Light, PartyNames.Hallou },
            { Elements.Dark, PartyNames.Dinowaru }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION5_RESULTSB = new()
        {
            { Elements.Fire, PartyNames.Onay },
            { Elements.Ice,  PartyNames.Burckhard},
            { Elements.Wind, PartyNames.Aydinc },
            { Elements.Elec, PartyNames.Kory },
            { Elements.Light, PartyNames.Claude },
            { Elements.Dark, PartyNames.Groskopf }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION6_RESULTSA = new()
        {
            { Elements.Fire, PartyNames.Lozinul },
            { Elements.Ice,  PartyNames.Uvale},
            { Elements.Wind, PartyNames.Dasinevu },
            { Elements.Elec, PartyNames.Dome },
            { Elements.Light, PartyNames.Famber },
            { Elements.Dark, PartyNames.Snans }
        };

        private static readonly System.Collections.Generic.Dictionary<Elements, string> FUSION6_RESULTSB = new()
        {
            { Elements.Fire, PartyNames.Sacha_Kegul },
            { Elements.Ice,  PartyNames.Qrok_Emut},
            { Elements.Wind, PartyNames.Towane },
            { Elements.Elec, PartyNames.Reeclacwu },
            { Elements.Light, PartyNames.Valuvari },
            { Elements.Dark, PartyNames.ShadowNinja }
        };


        public static List<Enemy> MakeBattleEncounter(int tier, bool dungeonCrawlEncounter)
        {
            List<Enemy> encounter = new List<Enemy>();
            if ((tier - RANDOM_TIER) < 0)
            {
                foreach (string enemyName in TUTORIAL_ENCOUNTERS[tier - 1])
                {
                    var enemy = _enemyMaker.MakeEnemy(enemyName, tier);
                    enemy.Boost(tier);
                    encounter.Add(enemy);
                }
            }
            else
            {
                GameObject gameObject = PersistentGameObjects.GameObjectInstance();
                int encounterIndex = tier - RANDOM_TIER;

                List<string> encounterNames = new List<string>();

                // If we have already stored an encounter in this list, we want to re-use it.
                if (gameObject.TierEnemyEncounters.ContainsKey(encounterIndex) && !dungeonCrawlEncounter)
                {
                    encounterNames = gameObject.TierEnemyEncounters[encounterIndex];
                }
                else
                {
                    if (tier % 10 == 0 && !dungeonCrawlEncounter)
                    {
                        int bossIndex = (tier / 10) - 1;
                        encounterNames.AddRange(BOSS_ENCOUNTERS[bossIndex]);
                    }
                    else if ((tier - 5) % 50 == 0  && tier <= 250 && dungeonCrawlEncounter)
                    {
                        int bossIndex = (tier/50) - 1;
                        encounterNames.AddRange(DUNGEON_BOSS_ENCOUNTERS[bossIndex]);
                    }
                    else if ((tier - 5) % 10 == 0 && tier > 250 && dungeonCrawlEncounter)
                    {
                        int bossIndex = (tier / 50) - 1;
                        bossIndex += (tier - 250) / 10;
                        if (bossIndex >= DUNGEON_BOSS_ENCOUNTERS.Length)
                            bossIndex = DUNGEON_BOSS_ENCOUNTERS.Length - 1;
                        encounterNames.AddRange(DUNGEON_BOSS_ENCOUNTERS[bossIndex]);
                    }
                    else
                    {
                        // use RANDOM_ENEMIES as a base
                        List<string> possibleEncounters = new List<string>();

                        string[] tier1RandomEncounters = [EnemyNames.Liamlas, EnemyNames.Fastrobren, EnemyNames.Thylaf, EnemyNames.Arwig, EnemyNames.Riccman, EnemyNames.Gardmuel, EnemyNames.Sachael, EnemyNames.Isenald, EnemyNames.CattuTDroni];
                        string[] tier2RandomEncounters = [EnemyNames.Ed, EnemyNames.Otem, EnemyNames.Hesret];
                        string[] tier3RandomEncounters = [EnemyNames.Nanfrea, EnemyNames.Ferza, EnemyNames.Anrol, EnemyNames.David];
                        string[] tier4RandomEncounters = [EnemyNames.Fledan, EnemyNames.Walds, EnemyNames.Naldbear, EnemyNames.Stroma_Hele,EnemyNames.Thony, EnemyNames.Conson];
                        string[] tier5RandomEncounters = [EnemyNames.Pebrand, EnemyNames.Leofuwil, EnemyNames.Gormacwen, EnemyNames.Vidwerd, EnemyNames.Sylla, EnemyNames.Venforth];
                        string[] tier6RandomEncountersA = [ EnemyNames.Aldmas, EnemyNames.Fridan, EnemyNames.Bue, EnemyNames.Bued, EnemyNames.Bureen, EnemyNames.Wennald, EnemyNames.Garcar, EnemyNames.LaChris,
                                                                        EnemyNames.Isumforth, EnemyNames.Ingesc, EnemyNames.Rahfortin ];
                        string[] tier6RandomEncountersB = [ EnemyNames.Leswith, EnemyNames.Paca, EnemyNames.Wigfred, EnemyNames.Lyley, EnemyNames.Acardeb,
                                                                        EnemyNames.Darol, EnemyNames.Hesbet, EnemyNames.Olu, EnemyNames.Iaviol, EnemyNames.Zalth, EnemyNames.Bernasbeorth];

                        string[] randomEnemies = [ EnemyNames.Ardeb, EnemyNames.DrigaBoli, EnemyNames.FoameShorti,
                                                                EnemyNames.ReeshiDeeme, EnemyNames.Tily, EnemyNames.Hahere, EnemyNames.Brast ];
                        string[] tier7RandomEncounters = [EnemyNames.Yodigrin, EnemyNames.Vustuma, EnemyNames.Gupmoth, EnemyNames.Maltamos, EnemyNames.Rusnopi, EnemyNames.Uptali, EnemyNames.Sufnod, EnemyNames.Ket, EnemyNames.Khasterat,
                        EnemyNames.Palmonu, EnemyNames.Baos, EnemyNames.Cendros, EnemyNames.Rigratos, EnemyNames.Zorliros, EnemyNames.Zervos, EnemyNames.Vaphos, EnemyNames.Leos,
                        EnemyNames.Camnonos, EnemyNames.Ridravos, EnemyNames.Raos];

                        string[] tier8RandomEncounters = [EnemyNames.Iji, EnemyNames.Sezuzo, EnemyNames.Tilaza_Fado, EnemyNames.Enu, EnemyNames.Juye,
                        EnemyNames.Dahone_Zude, EnemyNames.Ouadia, EnemyNames.Jiochroudice, EnemyNames.Hugline, EnemyNames.Danyll];

                        string[] tier9RandomEncounters = [EnemyNames.Kuo_toa, EnemyNames.Nolat, EnemyNames.Aboleth, EnemyNames.Hollyshimmer, EnemyNames.Albedo, EnemyNames.Grizzleboink_the_Noodle_Snatcher];
                        string[] tier10RandomEncounters = [EnemyNames.Buight, EnemyNames.Builectric, EnemyNames.Burk,EnemyNames.Tuidonak, EnemyNames.Soredr, EnemyNames.Elmaulgikr,EnemyNames.Vualdr, EnemyNames.Zadzek, EnemyNames.Skotzag,EnemyNames.Gostviltirst,EnemyNames.Phorna,];
                        string[] tier11RandomEncounters = [EnemyNames.Bazzaelth, EnemyNames.Culdra, EnemyNames.Lord, EnemyNames.Dimnain, EnemyNames.Green_Reaper, EnemyNames.Black_Getter,
                        EnemyNames.Piazeor, EnemyNames.Kodose, EnemyNames.Omre, EnemyNames.Uri, EnemyNames.Zaalki];

                        int minEnemies = 2;
                        int maxEnemies = 4;

                        int dungeonCrawlSub = (dungeonCrawlEncounter) ? 5 : 0;
                        int adjustedTier = tier - dungeonCrawlSub;

                        List<string[]> tList = new List<string[]>();

                        if (adjustedTier >= TierRequirements.TIER2_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER3_STRONGER_ENEMIES) // new encounters available past certain tiers
                        {
                            minEnemies = 3;
                            tList = [tier1RandomEncounters, tier2RandomEncounters];
                        }
                        else if(adjustedTier >= TierRequirements.TIER3_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER4_STRONGER_ENEMIES)
                        {
                            minEnemies = 3;
                            tList = [tier1RandomEncounters, tier2RandomEncounters, tier3RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER4_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER5_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier2RandomEncounters, tier3RandomEncounters, tier4RandomEncounters];
                        }
                        else if(adjustedTier >= TierRequirements.TIER5_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER6_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier3RandomEncounters, tier4RandomEncounters, tier5RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER6_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER8_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier5RandomEncounters, tier6RandomEncountersA, tier6RandomEncountersB];
                        }
                        else if (adjustedTier >= TierRequirements.TIER8_STRONGER_ENEMIES &&
                            adjustedTier < TierRequirements.TIER9_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier4RandomEncounters, tier6RandomEncountersB, tier7RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER9_STRONGER_ENEMIES &&
                            adjustedTier < TierRequirements.TIER10_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier5RandomEncounters, tier1RandomEncounters, tier7RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER10_STRONGER_ENEMIES &&
                            adjustedTier < TierRequirements.TIER11_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier7RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER11_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER12_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier7RandomEncounters, tier8RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER12_STRONGER_ENEMIES && adjustedTier < TierRequirements.TIER13_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier7RandomEncounters, tier8RandomEncounters, tier9RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER13_STRONGER_ENEMIES
                            && adjustedTier < TierRequirements.TIER14_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [
                                tier7RandomEncounters,
                                tier8RandomEncounters,
                                tier9RandomEncounters,
                                tier5RandomEncounters
                                ];
                        }
                        else if (adjustedTier >= TierRequirements.TIER13_STRONGER_ENEMIES
                            && adjustedTier < TierRequirements.TIER14_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier7RandomEncounters, tier8RandomEncounters, tier9RandomEncounters, tier5RandomEncounters];
                        }
                        else if (adjustedTier >= TierRequirements.TIER14_STRONGER_ENEMIES
                            && adjustedTier < TierRequirements.TIER15_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier7RandomEncounters, tier8RandomEncounters, tier9RandomEncounters, tier6RandomEncountersA];
                        }
                        else if (adjustedTier >= TierRequirements.TIER15_STRONGER_ENEMIES && adjustedTier <= 250)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier7RandomEncounters, 
                                tier8RandomEncounters, tier9RandomEncounters, 
                                tier6RandomEncountersA, tier10RandomEncounters];
                        }
                        else if (adjustedTier > 250)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;

                            tList = [tier9RandomEncounters,tier10RandomEncounters, tier11RandomEncounters];
                        }
                        else
                        {
                            tList = [tier1RandomEncounters];
                        }

                        foreach (var t1 in tList)
                            possibleEncounters.AddRange(t1);


                        int numEnemies = RANDOM.Next(minEnemies, maxEnemies + 1);
                        for (int i = 0; i < numEnemies; i++)
                        {
                            int randomEnemyIndex = RANDOM.Next(possibleEncounters.Count);
                            string enemyName = possibleEncounters[randomEnemyIndex];
                            encounterNames.Add(enemyName);
                            possibleEncounters.Remove(enemyName);
                        }
                    }

                    if (!dungeonCrawlEncounter)
                    {
                        gameObject.TierEnemyEncounters.Add(encounterIndex, new List<string>(encounterNames));
                    }
                    PersistentGameObjects.Save();
                }

                int boost = GetTierBoost(tier);
                tier += boost;

                foreach (string name in encounterNames)
                {
                    Enemy enemy = _enemyMaker.MakeEnemy(name, tier);
                    enemy.Boost(tier);
                    encounter.Add(enemy);
                }


            }

            return encounter;
        }

        public static List<Enemy> MakeEnemyFromList(int tier, List<string> names)
        {
            int boost = GetTierBoost(tier);
            tier += boost;

            List<Enemy> encounter = new List<Enemy>();
            foreach(string enemyName in names)
            {
                var enemy = _enemyMaker.MakeEnemy(enemyName, tier);
                enemy.Boost(tier);
                encounter.Add(enemy);
            }
            return encounter;
        }

        public static List<Enemy> MakeRandomEnemyEncounter(int tier, bool isBoss)
        {
            List<Enemy> encounter = new List<Enemy>();

            int min = 3;

            if (tier > TierRequirements.TIER4_STRONGER_ENEMIES)
                min = 4;

            int max = 6;

            int encounterNumber = RANDOM.Next(min, max);
            var random = new RandomEnemyFactory();
            
            int boost = GetTierBoost(tier);
            random.SetTier(tier);
            tier += boost;

            if (!isBoss)
            {
                for (int e = 0; e < encounterNumber; e++)
                {
                    var enemy = random.GenerateEnemy();
                    enemy.MaxHP *= 2;
                    enemy.Boost(tier);
                    encounter.Add(enemy);
                }
            }
            else
            {
                var gameObject = PersistentGameObjects.GameObjectInstance();
                var dictionary = gameObject.RandomizedBossEncounters;
                int realTier = gameObject.TierDC;

                Enemy boss;

                bool successFullyFoundBoss = dictionary.TryGetValue(realTier, out boss);
                if (!successFullyFoundBoss)
                {
                    boss = random.GenerateBoss(realTier);
                    boss.Boost(tier);
                    dictionary.Add(realTier, boss);
                    PersistentGameObjects.Save();
                }
                else
                {
                    boss.HardReset();
                }

                encounter.Add(boss);
            }


            return encounter;
        }

        public static List<Enemy> MakeTikkiMiniboss(string tikkiName)
        {
            var gameObject = PersistentGameObjects.GameObjectInstance();
            int realTier = gameObject.TierDC;
            int tier = gameObject.TierDC;

            if (!gameObject.TikkiBosses.ContainsKey(tier))
            {
                gameObject.TikkiBosses.Add(tier, new System.Collections.Generic.Dictionary<string, Enemy>());
            }

            var dictionary = gameObject.TikkiBosses[tier];

            var random = new RandomEnemyFactory();

            int boost = GetTierBoost(tier);
            tier += boost;
            random.SetTier(tier);

            Enemy boss;

            bool successFullyFoundBoss = dictionary.TryGetValue(tikkiName, out boss);
            if (!successFullyFoundBoss)
            {
                boss = random.GenerateTikkiBoss(tikkiName, realTier);
                boss.Boost(tier);
                dictionary.Add(tikkiName, boss);
                PersistentGameObjects.Save();
            }
            else
            {
                boss.HardReset();
            }

            return new List<Enemy> { boss };
        }

        public static List<Enemy> MakeFullBodyBoss(int tier, string name, bool isLabrybuce)
        {
            List<Enemy> encounter = new List<Enemy>();
            var unique = new UniqueEnemyFactory();

            int boost = GetTierBoost(tier);
            tier += boost;
            unique.SetTier(tier);

            var gameObject = PersistentGameObjects.GameObjectInstance();
            int realTier = (isLabrybuce) ? gameObject.TierDC : gameObject.Tier;

            Enemy boss = unique.GetEnemyByName(name);
            boss.Boost(tier);
            PersistentGameObjects.Save();

            encounter.Add(boss);

            return encounter;
        }

        public static int GetTierBoost(int tier)
        {
            double boostBase = 0.2;

            int numBoosts = tier / 5;

            for (int i = 0; i < numBoosts; i++)
                boostBase += 0.05;

            return (int)(tier * boostBase);
        }

        public static List<OverworldEntity> MakeShopVendorWares(GameObject gameObject, bool isCustom = false)
        {
            int tier = gameObject.MaxTier;
            List<OverworldEntity> partyMembers = new List<OverworldEntity>();

            // Get vendor wares based on the tier we're on
            if (!isCustom)
            {
                int vendorWaresIndex = 0;
                foreach (int index in SHOP_INDEXES)
                {
                    if (tier >= index)
                    {
                        foreach (string member in VENDOR_WARES[vendorWaresIndex])
                            partyMembers.Add(PartyMemberGenerator.MakePartyMember(member));

                        vendorWaresIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                // grades N -> 1
                for(int fg = MiscGlobals.FUSION_GRADE_CAP; fg > 0; fg--)
                {
                    var set = gameObject.DiscoveredFusions[fg];
                    foreach (string name in set)
                        partyMembers.Add(PartyMemberGenerator.MakePartyMember(name));
                }

                // grade 0
                foreach(string name in CUSTOM_WARES)
                    partyMembers.Add(PartyMemberGenerator.MakePartyMember(name));
            }

            return partyMembers;
        }
    
        public static List<FusionObject> MakeFusionEntities(OverworldEntity material1, OverworldEntity material2)
        {
            int gradeDifference = Math.Abs(material2.FusionGrade - material1.FusionGrade);
            int gradeSum = material2.FusionGrade + material1.FusionGrade;
            if (gradeDifference > 1 || (gradeDifference == 0 && gradeSum > 0))
            {
                return new List<FusionObject>();
            }

            int fusionGrade = Math.Max(material1.FusionGrade, material2.FusionGrade) + 1;

            List<FusionObject> possibleFusions = new List<FusionObject>();

            List<List<System.Collections.Generic.Dictionary<Elements, string>>> results = 
                [
                    [FUSION1_RESULTS],
                    [FUSION2_RESULTS],
                    [FUSION3_RESULTS],
                    [FUSION4_RESULTS],
                    [FUSION5_RESULTSA, FUSION5_RESULTSB],
                    [FUSION6_RESULTSA, FUSION6_RESULTSB]
                ];

            List<Elements> primaryElements = GetPrimaryElements(material1, material2);

            if (fusionGrade - 1 < results.Count && primaryElements.Count > 0)
            {
                var dictionaryList = results[fusionGrade - 1];
                foreach(var dictionary in dictionaryList)
                {
                    PopulatePossibleFusions(dictionary, primaryElements, possibleFusions, material1, material2);
                } 
            }

            return possibleFusions;
        }
        
        private static void PopulatePossibleFusions(System.Collections.Generic.Dictionary<Elements, string> fusionResults, List<Elements> elements, 
                                                    List<FusionObject> possibleFusions, OverworldEntity material1, 
                                                    OverworldEntity material2)
        {
            foreach (var element in elements)
            {
                if (fusionResults.ContainsKey(element))
                {
                    var fusionObject = new FusionObject
                    {
                        Material1 = material1,
                        Material2 = material2
                    };

                    var fusion = PartyMemberGenerator.MakePartyMember(fusionResults[element]);
                    fusion.MaxHP = (int)((material1.MaxHP + material2.MaxHP) / 1.5);
                    fusion.SetLevel((material1.Level + material2.Level) / 2);

                    double discount = 0.05 * (fusion.FusionGrade + 1);
                    fusion.VorpexValue = (material1.VorpexValue + material2.VorpexValue) / 2;
                    fusion.VorpexValue -= (int)(fusion.VorpexValue * discount);
                    
                    fusionObject.Fusion = fusion;

                    possibleFusions.Add(fusionObject);
                }

            }
        }

        private static List<Elements> GetPrimaryElements(OverworldEntity material1, OverworldEntity material2)
        {
            List<Elements> elements = new List<Elements>();

            var mat1Elements = material1.Resistances.GetPrimaryElements();
            var mat2Elements = material2.Resistances.GetPrimaryElements();

            elements.AddRange(MatchElements(mat1Elements, mat2Elements));

            return elements;
        }

        private static List<Elements> GetPrimaryWeaknessElements(OverworldEntity material1, OverworldEntity material2)
        {
            List<Elements> elements = new List<Elements>();

            var mat1Elements =  material1.Resistances.GetPrimaryWeaknessElements();
            var mat2Elements =  material2.Resistances.GetPrimaryWeaknessElements();

            elements.AddRange(MatchElements(mat1Elements, mat2Elements));

            return elements;
        }

        private static List<Elements> GetNoneResistances(OverworldEntity material1, OverworldEntity material2)
        {
            List<Elements> primaryElements = new List<Elements>();

            if(material1.Resistances.HasNoResistances() && material2.Resistances.HasNoResistances())
            {
                Elements[] elements = Enum.GetValues<Elements>();

                for (int i = 0; i < 2; i++)
                    primaryElements.Add(elements[RANDOM.Next(0, elements.Length)]);
            }

            return primaryElements;
        }

        private static List<Elements> MatchElements(List<Elements> mat1Elements, List<Elements> mat2Elements)
        {
            List<Elements> elements = new List<Elements>();

            foreach (Elements mat1Element in mat1Elements)
            {
                foreach (Elements mat2Element in mat2Elements)
                {
                    if (mat1Element == mat2Element)
                    {
                        elements.Add(mat1Element);
                    }
                }
            }

            return elements;
        }
    
        /// <summary>
        /// Make party members for a battle quest
        /// </summary>
        public static List<string> GetAllPartyNamesForBattleQuest(int tier)
        {
            List<string> names = new List<string>();

            PopulatePartyNameList(tier, names);

            return names;
        }

        private static void PopulatePartyNameList(int tier, List<string> names)
        {
            int vendorWaresLength = VENDOR_WARES.Length;

            foreach (var list in VENDOR_WARES)
                names.AddRange(list);

            if (tier >= TierRequirements.FUSE)
                names.AddRange(CUSTOM_WARES);

            if (tier > TierRequirements.QUESTS_FUSION_MEMBERS)
                names.AddRange(FUSION1_RESULTS.Values);

            if (tier > TierRequirements.QUESTS_ALL_FUSION_MEMBERS)
            {
                names.AddRange(FUSION2_RESULTS.Values);
            }
        }

        public static int GetBossHP(string bossName)
        {
            int index = -1;
            
            for(int b = 0; b < BOSS_ENCOUNTERS.Length; b++)
            {
                var bosses = BOSS_ENCOUNTERS[b];
                foreach(var boss in bosses)
                {
                    if(boss == bossName)
                    {
                        index = b;
                        break;
                    }
                }
            }
            
            if (index == -1)
            {
                throw new Exception("Boss not defined in encounter list");
            }

            // get the boss number
            index++;

            int startingHP = 15 * (index * 5);
            return startingHP;
        }

        private static int LABRYBUCE_CONSTANT = 25;

        public static int GetBossHPDC(string bossName)
        {
            int index = -1;

            for (int b = 0; b < DUNGEON_BOSS_ENCOUNTERS.Length; b++)
            {
                var bosses = DUNGEON_BOSS_ENCOUNTERS[b];
                foreach (var boss in bosses)
                {
                    if (boss == bossName)
                    {
                        index = b;
                        break;
                    }
                }
            }

            if (index == -1)
            {
                throw new Exception("Boss not defined in encounter list");
            }

            // get the boss number
            index++;
            index *= 50; // get the actual tier
            index /= 10;

            int startingHP = LABRYBUCE_CONSTANT * (index * 5);

            if (startingHP >= 2500)
                startingHP = 2500;

            return startingHP;
        }

        public static int GetBossHPRandom(int tier)
        {
            // get random image
            return LABRYBUCE_CONSTANT * ((tier/10) * 5);
        }
    }
}
