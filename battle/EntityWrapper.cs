using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    /// <summary>
    /// Wrapper for Battle Entities so we can send them over to Godot Objects
    /// </summary>
    public class EntityWrapper
    {
        private bool _isBoss = false;
        public bool IsBoss { get => _isBoss; set => _isBoss = value; }
        public BattleEntity BattleEntity { get; set; }
    }
}
