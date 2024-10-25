using AscendedZ.battle;
using AscendedZ.entities;
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
    [JsonDerivedType(typeof(StunStatus), typeDiscriminator: nameof(StunStatus))]
    public class StunStatus : Status
    {
        /// <summary>
        /// The total amount of turns the Status is active for.
        /// </summary>
        private const int ACTIVE_TURNS = 1;
        private const int REQUIRED_STACKS_TO_BE_ACTIVE = 0;

        private int _stacks;
        private int _activeTurns;

        public StunStatus() : base()
        {
            _id = StatusId.StunStatus;
            this.Icon = SkillAssets.STUN_ICON;
            Name = "Stun";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);
            _stacks = 1;
            _statusOwner.CanAttack = false;
            _activeTurns = 0;
            Active = true;
        }

        public override void UpdateStatus(BattleResult result)
        {
        }

        public override void ClearStatus()
        {
            this.RemoveStatus = true;
            _statusOwner.CanAttack = true;
        }

        /// <summary>
        /// Update the status after it's been applied.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _activeTurns++;
            if (_activeTurns == ACTIVE_TURNS)
            {
                this.RemoveStatus = true;
                entity.CanAttack = true;
            }
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"Stun: Prevents attacks for 1 turn.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new StunStatus();
        }
    }
}
