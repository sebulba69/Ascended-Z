using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    [JsonDerivedType(typeof(GuardStatus), typeDiscriminator: nameof(GuardStatus))]
    public class GuardStatus : Status
    {
        private int _activeTurns = 0;

        public GuardStatus() : base()
        {
            _id = StatusId.GuardStatus;
            this.Icon = SkillAssets.GUARD_ICON;
            Name = "Guard";

            UpdateDuringOwnersTurn = true;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);
            _activeTurns = 0;
            Active = true;
        }

        public override void UpdateStatus(BattleResult result)
        {
        }

        public override void ClearStatus()
        {
            this.RemoveStatus = true;
        }

        /// <summary>
        /// Update the status after it's been applied.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _activeTurns++;
            if (_activeTurns == 1)
            {
                RemoveStatus = true;
            }
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"Guard: Covers weaknesses for 1 turn. Status is removed if the guarding entity attacks.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new GuardStatus();
        }
    }
}
