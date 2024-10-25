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
    [JsonDerivedType(typeof(DebuffBoostStatus), typeDiscriminator: nameof(DebuffBoostStatus))]
    public class DebuffBoostStatus : Status
    {
        public DebuffBoostStatus()
        {
            _id = StatusId.DebuffBoost;
            Icon = SkillAssets.DEBUFF_BOOST_ICON;
            Name = "Debuff Boost";
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
            return 0;
        }

        public override void ClearStatus()
        {
        }

        public override Status Clone()
        {
            return new DebuffBoostStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = "Debuff Boost: Increases debuffs from capping at x2 stacks to x4.";

            return wrapper;
        }
    }
}
