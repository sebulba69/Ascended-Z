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
    [JsonPolymorphic]
    [JsonDerivedType(typeof(TeleportToExitItem), typeDiscriminator: nameof(TeleportToExitItem))]
    [JsonDerivedType(typeof(HealPartyMemberFullItem), typeDiscriminator: nameof(HealPartyMemberFullItem))]
    [JsonDerivedType(typeof(HealPartyMemberReviveItem), typeDiscriminator: nameof(HealPartyMemberReviveItem))]
    [JsonDerivedType(typeof(TeleportToBossRoomItem), typeDiscriminator: nameof(TeleportToBossRoomItem))]
    public interface IDungeonItem
    {
        string Name { get; }
        string Icon { get; }
        int Amount { get; set; }
        int Cost { get; } // orb cost
        string Description { get; }
        TargetTypes TargetType { get; }

        void Use(DungeonScreen dungeonScreen);
        void Use(List<BattlePlayer> targets);
    }
}
