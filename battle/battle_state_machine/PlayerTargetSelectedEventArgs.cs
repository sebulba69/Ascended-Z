using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle.battle_state_machine
{
    public class PlayerTargetSelectedEventArgs : EventArgs
    {
        public int SkillIndex { get; set; }
        public int TargetIndex { get; set; }
        public bool Guard { get; set; }
    }
}
