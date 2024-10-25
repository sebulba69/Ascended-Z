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
    [JsonDerivedType(typeof(Jyndesdarth), typeDiscriminator: nameof(Jyndesdarth))]
    public class Jyndesdarth : Status
    {
        private int _life;

        public Jyndesdarth() : base()
        {
            _id = StatusId.Jyndesdarth;
            _life = 0;
            this.Icon = SkillAssets.LIFETRACK;
            Name = "Jyndesdarth";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner); 
        }

        public override void IncreaseStatusCounter()
        {
        }

        public override void DecreaseStatusCounter()
        {
        }

        public override void UpdateStatus(BattleResult result)
        {
            int rtypeInt = (int)result.ResultType;
            if (rtypeInt < (int)BattleResultType.HPGain && result.ResultType != BattleResultType.Dr) 
            {
                if(result.Target == _statusOwner)
                {
                    _life += result.HPChanged;
                    if(_life > 50000)
                    {
                        Active = true;
                    }
                }
            }
        }

        /// <summary>
        /// Update the status after it's been applied at the start of a turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
        }

        public override void ClearStatus()
        {
            _life = 0;
            Active = false;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _life/1000;
            wrapper.CounterColor = Colors.White;

            if (Active)
                wrapper.CounterColor = Colors.Green;

            wrapper.Description = $"Jyndesdarth: Activates every 50,000 damage received.\nConverts enemy single-hit attacks to multi-hit.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new Jyndesdarth();
        }
    }
}
