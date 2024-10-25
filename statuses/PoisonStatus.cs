using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    [JsonDerivedType(typeof(PoisonStatus), typeDiscriminator: nameof(PoisonStatus))]
    public class PoisonStatus : Status
    {
        private int _activeTurns;

        public PoisonStatus() : base() 
        {
            _id = StatusId.PoisonStatus;
            _activeTurns = 0;
            this.Icon = SkillAssets.POISON_ICON;
            Name = "Poison";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result) { }

        /// <summary>
        /// Update the status after it's been applied at the start of the player/enemy (depends on owner) turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _activeTurns++;
            _statusOwner.HP -= (int)(_statusOwner.MaxHP * 0.25);
            if (_statusOwner.HP < 0) 
                _statusOwner.HP = 0;
            
            if (_activeTurns == 3 || _statusOwner.HP == 0)
                RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 4 - (_activeTurns + 1);
            wrapper.CounterColor = Colors.White;
            wrapper.Description = $"Poison: Reduce HP by 25% for 3 turns.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new PoisonStatus();
        }
    }
}
