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
    [JsonDerivedType(typeof(ConfuseStatus), typeDiscriminator: nameof(ConfuseStatus))]
    public class ConfuseStatus : Status
    {
        private int _activeTurns;

        public ConfuseStatus() : base()
        {
            _id = StatusId.Confusion;
            _activeTurns = 0;
            Icon = SkillAssets.CONFUSION;
            Name = "Confusion";
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
            wrapper.CounterColor = Colors.White;
            wrapper.Description = "Confusion: Affected will miss (-2 turn icons) all attacks until the party's next turn.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new ConfuseStatus();
        }
    }
}
