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
    [JsonDerivedType(typeof(HealPartyMemberFullItem), typeDiscriminator: nameof(HealPartyMemberFullItem))]
    public class HealPartyMemberFullItem : IDungeonItem
    {
        public string Name => "Buclidix Goop of Gribble";
        public string Icon => SkillAssets.HP_ITEM_ICON;
        public int Amount { get; set; }
        public int Cost { get => 5; }
        public string Description => "Fully heal 1 party member";
        public TargetTypes TargetType => TargetTypes.SINGLE_TEAM;

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
