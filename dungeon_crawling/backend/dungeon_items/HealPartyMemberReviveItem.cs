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
    [JsonDerivedType(typeof(HealPartyMemberReviveItem), typeDiscriminator: nameof(HealPartyMemberReviveItem))]
    public class HealPartyMemberReviveItem : IDungeonItem
    {
        public string Name => "Buclidix Regoop of Gribble";
        public string Icon => SkillAssets.HPR_ITEM_ICON;
        public int Amount { get; set; }
        public int Cost { get => 5; }
        public string Description => "Revive 1 party member with full health";
        public TargetTypes TargetType => TargetTypes.SINGLE_TEAM_DEAD;

        public void Use(DungeonScreen dungeonScreen)
        {
            throw new NotImplementedException();
        }

        public void Use(List<BattlePlayer> targets)
        {
            foreach (var target in targets)
                target.HP = target.MaxHP;
        }
    }
}
