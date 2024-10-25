using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.dungeon_items
{
    public class ItemWrapper
    {
        public IDungeonItem Item { get; set; }
        public List<BattlePlayer> Targets { get; set; }
    }
}
