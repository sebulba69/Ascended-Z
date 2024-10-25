using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.dungeon_items
{
    [JsonDerivedType(typeof(TeleportToExitItem), typeDiscriminator: nameof(TeleportToExitItem))]
    public class TeleportToExitItem : IDungeonItem
    {
        public string Name => "Buclidix Stick of Blixing";
        public string Icon => SkillAssets.TP_STICK_EXIT_ICON;
        public int Amount { get; set; }
        public int Cost { get => 10; }
        public string Description => "Teleport to the exit of the Labrybuce";
        public TargetTypes TargetType => TargetTypes.SELF;

        public void Use(DungeonScreen dungeonScreen)
        {
            dungeonScreen.TeleportToExit();
        }

        public void Use(List<BattlePlayer> targets)
        {
            throw new NotImplementedException();
        }
    }
}
