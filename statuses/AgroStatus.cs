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
    [JsonDerivedType(typeof(AgroStatus), typeDiscriminator: nameof(AgroStatus))]
    public class AgroStatus : Status
    {
        private int _activeTurns;
        private const int ACTIVE_TURNS = 2;

        public AgroStatus() : base()
        {
            _id = StatusId.AgroStatus;
            _activeTurns = 0;
            this.Icon = SkillAssets.AGRO_ICON;
            Name = "Agro";
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

            if(_activeTurns == ACTIVE_TURNS)
                this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = ACTIVE_TURNS - _activeTurns;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"Agro: All enemies target the player with the agro status. Overrides enemies that inflict a status to focus on attacking only.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new AgroStatus();
        }
    }
}
