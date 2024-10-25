using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    /// <summary>
    /// A collection of scene paths used throughout the game.
    /// </summary>
    public partial class Scenes
    {
        // CUTSCENES
        public static readonly string PROLOG_1 = "res://screens/cutscene/001. Prologue/Prologue.tscn";
        public static readonly string PROLOG_2 = "res://screens/cutscene/002. Prologue pt.2/Prologue 02.tscn";

        public static readonly string ENDING_A1 = "res://screens/cutscene/006. Ending A/NormalEnding.tscn";
        public static readonly string ENDING_A2 = "res://screens/cutscene/006. Ending A/EndingA2.tscn";
        public static readonly string ENDING_B1 = "res://screens/cutscene/007. Ending B/EndingB1.tscn";
        public static readonly string ONE_HUNDRED_PERCENT_CUTSCENE = "res://screens/cutscene/100PercentCutscene.tscn";
        public static readonly string ONE_HUNDRED_PERCENT_BOSS = "res://screens/cutscene/Final100PercentBossPreCutscene.tscn";
        public static readonly string CREDITS = "res://screens/CGCutsceneScreen.tscn";

        // new UI screens
        public static readonly string SETTINGS = "res://screens/settings_screen/SettingsScreen.tscn";
        public static readonly string START = "res://screens/StartScreen.tscn";
        public static readonly string MAIN = "res://screens/MainScreen.tscn";
        public static readonly string MAIN_RECRUIT = "res://screens/RecruitScreenTabs.tscn";
        public static readonly string MAIN_EMBARK = "res://screens/EmbarkScreen.tscn";
        public static readonly string MAIN_EMBARK_DISPLAY = "res://screens/PartyMemberDisplay.tscn";
        public static readonly string MAIN_CHANGE_ROOM = "res://screens/ChangeRoomScreen.tscn";
        public static readonly string BATTLE_SCENE = "res://screens/BattleEnemyScene.tscn";
        public static readonly string MENU = "res://screens/MenuScene.tscn";
        public static readonly string UPGRADE = "res://screens/upgrade_screen/UpgradeScreen.tscn";
        public static readonly string REORDER = "res://screens/upgrade_screen/skill_reorder/SkillReorderControl.tscn";
        public static readonly string PARTY_CHANGE = "res://screens/PartyEditScreen.tscn";
        public static readonly string FUSION = "res://screens/FusionScreen.tscn";
        public static readonly string SKILL_TRANSFER = "res://screens/skill_transfer_screen/SkillTransferScreen.tscn";
        public static readonly string SIGIL_SCREEN = "res://screens/sigil_screen/SigilScreen.tscn";
        public static readonly string MAIL_SCREEN = "res://screens/teleport/TutorialScreen.tscn";

        public static readonly string TRANSITION = "res://screens/SceneTransition.tscn";

        public static readonly string UPGRADE_ITEM = "res://screens/upgrade_screen/UpgradeItem.tscn";

        // icons
        public static readonly string ICON = "res://misc_icons/Icon.tscn";

        // currency
        public static readonly string CURRENCY_DISPLAY = "res://misc_icons/CurrencyDisplay.tscn";

        // popups
        public static readonly string YES_NO_POPUP = "res://screens/PopupWindow.tscn";

        // reward screen
        public static readonly string REWARDS = "res://screens/RewardScreen.tscn";

        // res
        public static readonly string RESISTANCE = "res://screens/ResistanceDisplay.tscn";

        // battle assets
        public static readonly string PARTY_BOX = "res://screens/PartyBattleDisplayBox.tscn";
        public static readonly string ENEMY_BOX = "res://screens/EnemyBattleDisplayBox.tscn";
        public static readonly string BOSS_BOX = "res://screens/BossBattleDisplayBox.tscn";

        // effects
        public static readonly string EFFECTS = "res://effects/EffectSprite.tscn";
        public static readonly string DAMAGE_NUM = "res://effects/DamageNumber.tscn";
        public static readonly string STATUS = "res://statuses/StatusIcon.tscn";

        // info screen
        public static readonly string INFO_SCREEN = "res://screens/battle_info/BattleInfoBox.tscn";

        // boss scenes
        public static readonly string FINAL_BOSS = "res://screens/full_screen_boss/NetallaBattleScene.tscn";
        public static readonly string FINAL_BOSS_LAB = "res://screens/full_screen_boss/DracoBattleScene.tscn";
        public static readonly string FINAL_BOSS_100_PERCENT = "res://screens/full_screen_boss/DrakallaBattleScene.tscn";

        // elders
        public static readonly string ELDER_1 = "res://screens/full_screen_boss/YacnacalbBattleScene.tscn";
        public static readonly string ELDER_2 = "res://screens/full_screen_boss/MhaarvoshBattleScene.tscn";
        public static readonly string ELDER_3 = "res://screens/full_screen_boss/BhotldrenBattleScene.tscn";
        public static readonly string ELDER_4 = "res://screens/full_screen_boss/AiucxaiobhloBattleScene.tscn";
        public static readonly string ELDER_5 = "res://screens/full_screen_boss/GhryztitralbhBattleScene.tscn";

        // icons
        public static readonly string TURN_ICONS = "res://turn_icons/TurnIcon.tscn";

        // dungeon crawling
        public static readonly string DUNGEON_CRAWL = "res://dungeon_crawling/scenes/DungeonScreen.tscn";
        public static readonly string DUNGEON_TILE_SCENE = "res://dungeon_crawling/scenes/TileScene.tscn";
        public static readonly string DUNGEON_CRAWL_PARTY_MEMBER = "res://dungeon_crawling/scenes/crawl_ui/PartyMemberDCDisplay.tscn";
        public static readonly string DUNGEON_FOUNTAIN = "res://dungeon_crawling/scenes/crawl_ui/FountainOfBuce.tscn";
        public static readonly string DUNGEON_MINER = "res://dungeon_crawling/scenes/crawl_ui/MinerUI.tscn";
        public static readonly string DUNGEON_SP_TILE = "res://dungeon_crawling/scenes/crawl_ui/DCSpecialTileIcon.tscn";
        public static readonly string DUNGEON_DIALOG_SCENE = "res://dungeon_crawling/scenes/crawl_ui/DC_BOSS_CUTSCENE.tscn";
        public static readonly string INVENTORY_SCENE = "res://dungeon_crawling/scenes/crawl_ui/InventoryUI.tscn";
        public static readonly string SHOPKEEPER_SCENE = "res://dungeon_crawling/scenes/crawl_ui/ShopkeeperUI.tscn";
        
        public static readonly string SKILL_UPGRADE_UNIT = "res://screens/upgrade_screen/skill_max_upgrade/skill_upgrade_unit.tscn";
        public static readonly string INDIVIDUAL_UPGRADE_SCREEN = "res://screens/upgrade_screen/skill_max_upgrade/SkillUpgradeUI.tscn";
        public static readonly string SIGIL_ICON = "res://screens/sigil_screen/SigilItemIcon.tscn";

        public static readonly string[] ELDER_BATTLES = 
            [
                "res://screens/full_screen_boss/YacnacalbBattleScene.tscn",
                "res://screens/full_screen_boss/MhaarvoshBattleScene.tscn",
                "res://screens/full_screen_boss/BhotldrenBattleScene.tscn",
                "res://screens/full_screen_boss/AiucxaiobhloBattleScene.tscn",
                "res://screens/full_screen_boss/GhryztitralbhBattleScene.tscn"
            ];
    }
}
