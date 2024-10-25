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
    [JsonDerivedType(typeof(MarkOfDeathStatus), typeDiscriminator: nameof(MarkOfDeathStatus))]
    public class MarkOfDeathStatus : Status
    {
        private int _activeTurns;

        public MarkOfDeathStatus() : base()
        {
            _id = StatusId.MarkOfDeathStatus;
            Icon = SkillAssets.MARK_OF_DEATH_STATUS;
            Name = "Mark of Death";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _activeTurns = 0;
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result)
        {
        }

        /// <summary>
        /// Update the status after it's been applied at the start of a turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _activeTurns++;
            if (_activeTurns == 2)
            {
                RemoveStatus = true;
                _statusOwner.HP = 0;
            }
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 2 - _activeTurns;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = "Mark of Death: Affected will die after 2 turns.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new MarkOfDeathStatus();
        }
    }
}
