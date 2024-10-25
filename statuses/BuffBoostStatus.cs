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
    [JsonDerivedType(typeof(BuffBoostStatus), typeDiscriminator: nameof(BuffBoostStatus))]
    public class BuffBoostStatus : Status
    {
        public int Stacks { get; set; }

        public BuffBoostStatus()
        {
            _id = StatusId.BuffBoost;
            Icon = SkillAssets.BUFF_BOOST_ICON;
            Name = "Buff Boost";
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
        }

        public override void UpdateStatusTurns(BattleEntity entity)
        {
        }

        public override int GetStacks()
        {
            return Stacks;
        }

        public override void ClearStatus()
        {
        }

        public override Status Clone()
        {
            return new BuffBoostStatus() 
            { 
                Name = Name,
                Icon = Icon,
                Stacks = Stacks 
            };
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = $"Buff Boost: Increases buffs from capping at x2 stacks to x{Stacks}.";

            return wrapper;
        }
    }
}
