using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.statuses
{
    [JsonDerivedType(typeof(BindStatus), typeDiscriminator: nameof(BindStatus))]
    public class BindStatus : Status
    {
        private int _activeTurns;
        private const int ACTIVE_TURNS = 2;

        public BindStatus() : base()
        {
            _id = StatusId.BindStatus;
            _activeTurns = 0;
            this.Icon = SkillAssets.BIND_ICON;
            Name = "Bind";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
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

            if (_activeTurns == ACTIVE_TURNS)
                this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = ACTIVE_TURNS - _activeTurns;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = $"Bind: Afflicted cannot be healed past 25% HP for 3 turns.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new BindStatus();
        }
    }
}
