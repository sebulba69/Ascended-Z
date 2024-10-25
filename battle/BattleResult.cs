using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AscendedZ.battle
{
    public enum BattleResultType { Wk, Rs, Nu, Dr, Normal, Guarded, Tech, TechWk, Evade, HPGain, StatusApplied, StatusRemoved, Pass, Retreat, BeastEye, DragonEye, Reinforcement }

    /// <summary>
    /// This is the class the UI is going to use to know what information needs to be shown on the screen
    /// after an interaction
    /// </summary>
    public class BattleResult
    {
        public BattleResultType ResultType { get; set; }
        public List<BattleResultType> Results { get; set; }
        /// <summary>
        /// Can represent Damage taken or HP gained
        /// </summary>
        public int HPChanged { get; set; }
        public List<int> AllHPChanged { get; set; }
        public BattleEntity User { get; set; }
        public BattleEntity Target { get; set; }
        public List<BattleEntity> Targets { get; set; }
        public List<string> EnemySummonNames { get; set; }
        public ISkill SkillUsed { get; set; }
        
        public BattleResult() 
        {
            AllHPChanged = new List<int>();
            Targets = new List<BattleEntity>();
            Results = new List<BattleResultType>();
            EnemySummonNames = new();
        }

        public string GetResultString()
        {
            string result;

            if (ResultType == BattleResultType.TechWk)
                result = "W.TECH";
            else if (ResultType == BattleResultType.Wk)
                result = "WEAK";
            else if (ResultType == BattleResultType.Tech)
                result = "TECH";
            else if (ResultType == BattleResultType.Guarded)
                result = "GUARD";
            else if (ResultType == BattleResultType.Rs)
                result = "RESIST";
            else if (ResultType == BattleResultType.Nu)
                result = "VOID";
            else if (ResultType == BattleResultType.Evade)
                result = "EVADED";
            else
                result = string.Empty;

            return result;
        }
    }
}
