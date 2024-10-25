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
    [JsonDerivedType(typeof(FocusStatus), typeDiscriminator: nameof(FocusStatus))]
    public class FocusStatus : Status
    {
        private int _activeTurns;

        public FocusStatus() : base()
        {
            _id = StatusId.FocusStatus;
            _activeTurns = 0;
            Icon = SkillAssets.FOCUS_ICON;
            Name = "Focus";
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
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = "Focus: Next attack will do 1.5x damage, status\nis removed immediately after.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new FocusStatus();
        }
    }
}
