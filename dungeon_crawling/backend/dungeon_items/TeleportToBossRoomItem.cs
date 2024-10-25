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
    [JsonDerivedType(typeof(TeleportToBossRoomItem), typeDiscriminator: nameof(TeleportToBossRoomItem))]
    public class TeleportToBossRoomItem : IDungeonItem
    {
        public string Name => "Buclidix Stick of Bossing";
        public string Icon => SkillAssets.TP_TO_BOSS_ROOM;
        public int Amount { get; set; }
        public int Cost { get => 10; }
        public string Description => "Teleport to the golden boss room if available";
        public TargetTypes TargetType => TargetTypes.SELF;

        public void Use(DungeonScreen dungeonScreen)
        {
            dungeonScreen.TeleportToBoss();
        }

        public void Use(List<BattlePlayer> targets)
        {
            throw new NotImplementedException();
        }
    }
}
