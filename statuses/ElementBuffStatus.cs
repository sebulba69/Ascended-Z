using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using AscendedZ.statuses.buff_elements;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.statuses
{
    public class ElementBuffStatus : Status
    {
        protected int _stacks = 1;
        private int _turnCount;
        protected double _baseAmount;
        public Elements BuffElement { get; set; }
        public double Amount { get; set; }
        public int Stacks { get => _stacks; set => _stacks = value; }

        public ElementBuffStatus() : base()
        {
            _baseAmount = 0.15;
            Amount = _baseAmount;
            _turnCount = 0;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);

            ApplyBuff();
        }

        public override void IncreaseStatusCounter()
        {
            _stacks++;
            _turnCount = 0;
            Amount = _baseAmount * _stacks;

            ApplyBuff();
        }

        private void ApplyBuff()
        {
            if (_statusOwner == null)
                return;

            _statusOwner.ElementDamageModifiers[(int)BuffElement] = Amount;
        }

        public override void UpdateStatus(BattleResult result)
        {
        }

        public override void ClearStatus()
        {
            _statusOwner.ElementDamageModifiers[(int)BuffElement] = 0;
            base.ClearStatus();
        }

        /// <summary>
        /// Update the status after it's been applied at the start of a turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _turnCount++;

            if (_turnCount == 2)
                RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _stacks;
            wrapper.CounterColor = Colors.White;

            wrapper.Description = $"{Name}: Increase damage for {BuffElement} by {Math.Round(Amount*100,1)}%.";

            return wrapper;
        }
    }
}
