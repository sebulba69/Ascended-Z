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
    [JsonDerivedType(typeof(TechnicalStatus), typeDiscriminator: nameof(TechnicalStatus))]
    public class TechnicalStatus : Status
    {
        private const int STACK_CAP = 1;
        private int _stacks;

        public TechnicalStatus() : base()
        {
            _id = StatusId.TechnicalStatus;
            _stacks = 0;
            this.Icon = SkillAssets.TECH_STATUS_ICON;
            Name = "Technical";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _stacks = 1;
            Active = true;
            base.ActivateStatus(owner); 
        }

        public override void IncreaseStatusCounter()
        {
        }

        public override void DecreaseStatusCounter()
        {
            _stacks--;
            if (_stacks < STACK_CAP)
                Active = false;

            if (_stacks == 0)
                RemoveStatus = true;
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
            wrapper.Counter = _stacks;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = $"Technical: At {STACK_CAP} stack, the next non-Null/Drained/Guarded attack will\ncount as a weakness. Stacks with weaknesses for more damage. Disappears after\nnext elemental skill is used.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new TechnicalStatus();
        }
    }
}
