using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle.battle_state_machine
{
    public class EnemyAction
    {
        public ISkill Skill { get; set; }
        public BattleEntity Target { get; set; }
    }
}
