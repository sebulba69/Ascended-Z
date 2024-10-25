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
    [JsonDerivedType(typeof(SealStatus), typeDiscriminator: nameof(SealStatus))]
    public class SealStatus : Status
    {
        private int _activeTurns;
        private const int ACTIVE_TURNS = 2;

        public SealStatus() : base()
        {
            _id = StatusId.SealStatus;
            _activeTurns = 0;
            this.Icon = SkillAssets.SEAL_ICON;
            Name = "Seal";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);

            var atk = owner.StatusHandler.GetStatus(StatusId.AtkChangeStatus);
            var def = owner.StatusHandler.GetStatus(StatusId.DefChangeStatus);

            if (atk.GetStacks() >= 0) 
            {
                atk.ClearStatus();
            }

            if(def.GetStacks() >= 0)
            {
                def.ClearStatus();
            }
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

            if (_activeTurns == ACTIVE_TURNS)
                this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = ACTIVE_TURNS - _activeTurns;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = $"Seal: Afflicted cannot be buffed for 3 turns.\nAlso all buffs are reset when the status is applied.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new SealStatus();
        }
    }
}
