using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class MusicAssets
    {
        public static readonly string TIKKI_BOSS = "res://music/dungeon_crawl_boss/dungeon_crawl_tikki_boss.ogg";
        public static readonly string OW_MUSIC_FOLDER = "res://music/overworld/";
        public static readonly string DR_MUSIC_FOLDER = "res://music/dungeons_tiers";
        public static readonly string DR_BOSSES_MUSIC_FOLDER = "res://music/dungeon_bosses";
        public static readonly string DC_MUSIC_FOLDER = "res://music/dungoen_crawl";
        private static readonly string DC_BOSS_RANDOM_FOLDER = "res://music/dungeon_crawl_boss/dungeon_crawl_random_bosses";

        public static readonly string DC_BOSS_PRE = "res://music/dungeon_crawl_pre/dungeon_crawl_boss_pre.ogg";
        public static readonly string DC_BOSS_PRE_BOUNTY = "res://music/dungeon_crawl_pre/dungeon_craw_bounty_pre.ogg";
        public static readonly string DC_BOSS = "res://music/dungeon_crawl_boss/dungeon_crawl_boss.ogg";
        public static readonly string DC_BOUNTY_BOSS = "res://music/dungeon_crawl_boss/dungeon_crawl_bounty_boss.ogg";
        public static readonly string BOSS_VICTORY = "res://music/boss_victory.ogg";
        public static readonly string FIRST_CUTSCENE = "res://music/cutscene.ogg";

        private static List<string> _overworldTracks, _dungeonTracksReal, _dungeonBossesReal, _dungeonCrawlTracks, _dungeonCrawlRandomBossTracks;

        private static List<string> OverworldTracks 
        {
            get 
            {
                if (_overworldTracks == null) 
                {
                    _overworldTracks = new List<string>();
                    AssetUtil.LoadAssets(OW_MUSIC_FOLDER, _overworldTracks);
                }

                return _overworldTracks;
            }
        }


        private static List<string> DungeonTracksReal 
        {
            get 
            {
                if(_dungeonTracksReal == null)
                {
                    _dungeonTracksReal = new List<string>();
                    AssetUtil.LoadAssets(DR_MUSIC_FOLDER, _dungeonTracksReal);
                }

                return _dungeonTracksReal;
            }
        }

        private static List<string> DungeonBossesReal
        {
            get
            {
                if(_dungeonBossesReal == null)
                {
                    _dungeonBossesReal = new List<string>();
                    AssetUtil.LoadAssets(DR_BOSSES_MUSIC_FOLDER, _dungeonBossesReal);
                }

                return _dungeonBossesReal;
            }
        }

        private static List<string> DungeonCrawlTracks
        {
            get
            {
                if(_dungeonCrawlTracks == null)
                {
                    _dungeonCrawlTracks = new List<string>();
                    AssetUtil.LoadAssets(DC_MUSIC_FOLDER, _dungeonCrawlTracks);
                }

                return _dungeonCrawlTracks;
            }
        }

        private static List<string> DungeonCrawlRandomBossTracks
        {
            get
            {
                if(_dungeonCrawlRandomBossTracks == null)
                {
                    _dungeonCrawlRandomBossTracks = new List<string>();
                    AssetUtil.LoadAssets(DC_BOSS_RANDOM_FOLDER, _dungeonCrawlRandomBossTracks);
                }

                return _dungeonCrawlRandomBossTracks;
            }
        }

        public static string GetOverworldTrackNormal()
        {
            var gameObject = PersistentGameObjects.GameObjectInstance();
            int tier = gameObject.MaxTier;
            int index = Equations.GetTierIndexBy10(tier);

            bool isEndGame = gameObject.ProgressFlagObject.EndgameUnlocked;

            if (isEndGame)
            {
                if (index >= OverworldTracks.Count)
                    index = OverworldTracks.Count - 1;
            }
            else
            {
                if (index >= OverworldTracks.Count - 1)
                    index = OverworldTracks.Count - 2;
            }

            return OverworldTracks[index];
        }

        public static List<string> GetOverworldTracks(int tier, bool isEndgame)
        {
            int adjustedTier = Equations.GetTierIndexBy10(tier);
            List<string> tracks = new List<string>();

            var overworld = OverworldTracks;
            for (int i = 0; i < adjustedTier + 1; i++)
            {
                if (isEndgame) 
                {
                    if (i >= overworld.Count)
                        break;
                }
                else
                {
                    if (i >= overworld.Count - 1)
                        break;
                }


                tracks.Add(overworld[i]);
            }

            return tracks;
        }

        private static readonly List<string> _endgameTracks = 
            [
                "res://music/dungeons_tiers/tier003.ogg",
                "res://music/dungeons_tiers/tier005.ogg",
                "res://music/dungeons_tiers/tier007.ogg",
                "res://music/dungeons_tiers/tier008.ogg",
                "res://music/dungeons_tiers/tier004.ogg"
            ];
        public static string GetDungeonTrack(int tier)
        {
            // tiers 5 - 10 have special tracks
            if (tier % 10 == 0)
            {
                int index = Equations.GetTierIndexBy10(tier);
                return DungeonBossesReal[index];
            }
            else
            {
                int index = ((tier - (tier % 10)) / 10);

                if (index >= DungeonTracksReal.Count)
                {
                    index -= DungeonTracksReal.Count;
                    return _endgameTracks[index];
                }
                else
                {
                    return DungeonTracksReal[index];
                }
            }
        }

        public static string GetDungeonTrackDC(int tier)
        {
            int index = Equations.GetTierIndexBy25(tier);
            return DungeonCrawlTracks[index];
        }

        public static string GetDungeonTrackRandomBoss(int tier)
        {
            int index = Equations.GetTierIndexBy100(tier);
            return DungeonCrawlRandomBossTracks[index];
        }
    }
}
