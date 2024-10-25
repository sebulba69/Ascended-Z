using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class RetreatSkill : ISkill
    {
        public SkillId Id => SkillId.Retreat;

        public string BaseName { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public string Name => BaseName;
        public string Description { get; }
        public int TransferLevel { get; set; }
        public RetreatSkill()
        {
            Level = 1;
        }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            return new BattleResult() { ResultType = BattleResultType.Retreat, SkillUsed = this };
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            throw new NotImplementedException();
        }

        public string GetBattleDisplayString()
        {
            return this.BaseName;
        }

        public override string ToString()
        {
            return $"{this.BaseName}";
        }

        public void LevelUp()
        {
        }

        public string GetAscendedString(int ascendedLevel)
        {
            return GetUpgradeString();
        }

        public string GetUpgradeString()
        {
            return ToString();
        }

        public ISkill Clone()
        {
            return new RetreatSkill()
            {
                BaseName = this.BaseName,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon
            };
        }
    }
}
