using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses.void_elements;
using AscendedZ.statuses.weak_element;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    public class ChangeElementStatus : Status
    {
        private ResistanceType _oldResType;
        protected ResistanceType _newResType;
        protected int _turnCount;
        private int _turns;
        protected Elements _elementToChange;

        /// <summary>
        /// Icon must be set from initializer.
        /// </summary>
        public ChangeElementStatus() : base()
        {
            this.UpdateDuringOwnersTurn = true;
            _turns = 0;
            _turnCount = 1;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            if (!owner.StatusHandler.HasStatus(_id)) 
            {
                base.ActivateStatus(owner);

                _oldResType = owner.Resistances.GetResistance(_elementToChange);
                owner.Resistances.SetResistance(_newResType, _elementToChange);
            }
        }

        public override void UpdateStatus(BattleResult result) {}

        /// <summary>
        /// Update the status after it's been applied at the start of the player/enemy (depends on owner) turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _turns++;
            if(_turns >= _turnCount)
            {
                _statusOwner.Resistances.SetResistance(_oldResType, _elementToChange);
                this.RemoveStatus = true;
            }

        }

        public override void ClearStatus()
        {
            _statusOwner?.Resistances.SetResistance(_oldResType, _elementToChange);
            this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _turnCount;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"{Name}: Sets the target's resistance to a specific element to {_newResType} for 1 turn.";

            return wrapper;
        }
    }
}
