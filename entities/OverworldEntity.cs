using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.sigils;
using AscendedZ.json_interface_converters;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.entities.partymember_objects
{
    public class OverworldEntity : Entity
    {
        private int _maxLevelCap = 150;
        private int _level = 0;
        private int _grade = 0;
        private int _vorpexCost = 1;
        private int _shopCost = 1;
        private bool _isInParty = false;
        private int _fusionGrade = 0;
        private int _skillCap = 2;
        private List<Sigil> _sigils;

        public bool IsLevelCapHit => Level == _maxLevelCap;
        public bool IsInParty { get => _isInParty; set => _isInParty = value; }
        public int Level { get => _level; set => _level = value; }
        public int VorpexValue { get => _vorpexCost; set => _vorpexCost = value; }

        public int RefundRewardVC
        {
            get
            {
                int refundYield = VorpexValue;

                if (FusionGrade > 0)
                    refundYield *= FusionGrade;

                refundYield = (int)(refundYield * 0.25) + 1;

                return refundYield;
            }
        }

        public int RefundReward 
        { 
            get 
            {
                int refund = (int)(VorpexValue * 0.03);
                refund += (FusionGrade * 2);

                return refund;
            } 
        }

        public int MaxHP { get; set; }
        public int SkillCap { get => _skillCap; set => _skillCap = value; }
        public string DisplayName 
        { 
            get
            {
                string retString;
                string prefix = "";
                if(Level > 0)
                {
                    prefix = $"[L.{Level}] ";
                    if (Level == _maxLevelCap)
                    {
                        prefix = "[MAX] ";
                    }
                }

                retString = $"{prefix}{Name}";

                return retString;
            } 
        }
        public List<ISkill> Skills { get; set; } = new();
        public ResistanceArray Resistances { get; set; } = new();
        public int FusionGrade { get => _fusionGrade; set => _fusionGrade = value; }
        public List<Sigil> Sigils 
        {
            get 
            {
                if(_sigils == null)
                {
                    _sigils = SigilDatabase.MakeSigils();
                }

                return _sigils;
            } 
            set 
            { 
                _sigils = value; 
            } 
        }

        public BattlePlayer MakeBattlePlayer()
        {
            var player = MakeBattlePlayerBase();

            player.Skills.Add(SkillDatabase.Pass);
            player.Skills.Add(SkillDatabase.Guard);

            return player;
        }

        public BattlePlayer MakeBattlePlayerBase()
        {
            var player = new BattlePlayer()
            {
                Name = DisplayName,
                Image = Image,
                HP = MaxHP,
                MaxHP = MaxHP,
                Resistances = new(),
                BaseName = Name
            };

            for(int i = 0; i < Resistances.RArray.Length; i++)
                player.Resistances.SetResistance((ResistanceType)Resistances.RArray[i], (Elements)i);

            foreach (var skill in Skills)
                player.Skills.Add(skill.Clone());

            foreach (var sigil in Sigils)
            {
                if(sigil.Index != -1)
                    player.ApplySigil(sigil);
            }
                

            return player;
        }

        private const int TIER_CAP = 5;

        public void SetLevel(int level)
        {
            Level = level;
        }

        public void LevelUp()
        {
            if (Level + 1 > _maxLevelCap)
                return;

            Level++;

            VorpexValue = Equations.GetVorpexLevelValue(VorpexValue, Level);
            MaxHP = Equations.GetOWMaxHPUpgrade(MaxHP, Level);

            foreach (ISkill skill in Skills)
            {
                if(Level >= skill.TransferLevel)
                {
                    skill.TransferLevel = 0;
                    skill.LevelUp();
                }
            }
                
        }

        private const int HP_CAP = 999999;

        public bool CanBoostHP()
        {
            return MaxHP <= HP_CAP;
        }

        public void HPBoost()
        {
            MaxHP = Equations.GetOWMaxHPUpgrade(MaxHP, Level);
            if (MaxHP >= HP_CAP)
                MaxHP = HP_CAP;
        }

        public string GetHPLevelUpPreview(int hp)
        {
            return $"{Equations.GetOWMaxHPUpgrade(hp, Level):n0} HP";
        }

        public string GetUpgradeString()
        {
            var bp = MakeBattlePlayerBase();

            StringBuilder skills = new StringBuilder();

            foreach (ISkill skill in bp.Skills)
            {
                if(Level < skill.TransferLevel)
                {
                    skills.Append($"{skill.GetBattleDisplayString()} → 🔒 L.{skill.TransferLevel}\n");
                }
                else
                {
                    skills.Append(skill.GetUpgradeString() + "\n");
                }
            }
                

            string final = $"{bp.MaxHP:n0} HP → {GetHPLevelUpPreview(bp.MaxHP):n0}\n{Resistances.GetResistanceString()}\n{skills.ToString()}";

            if (FusionGrade > 0)
                final = $"Fusion Grade {FusionGrade}\n{final}";

            return final;
        }

        public string GetFusionString()
        {
            StringBuilder skills = new StringBuilder();

            skills.AppendLine(GetSkills(Skills, true));

            string maxHP = GetHPString(MaxHP);

            maxHP += $" ● (Skills: {SkillCap})";

            return $"{maxHP:n0}\n{Resistances.GetResistanceString()}\n{skills.ToString()}";
        }

        public override string ToString()
        {
            var bp = MakeBattlePlayerBase();

            StringBuilder skills = new StringBuilder();

            skills.AppendLine(GetSkills(bp.Skills, false));

            return $"{bp.MaxHP:n0} HP ● {Resistances.GetResistanceString()}\n{skills.ToString()}";
        }

        private string GetHPString(int hpString)
        {
            string maxHP = $"{hpString} HP";
            if (FusionGrade > 0)
                maxHP = $"{maxHP:n0} ● Fusion {FusionGrade}";

            return maxHP;
        }

        private string GetSkills(List<ISkill> skillList, bool fusion)
        {
            StringBuilder skills = new StringBuilder();

            if (Skills.Count > 0)
            {
                foreach (ISkill skill in skillList)
                {
                    if (fusion)
                    {
                        var clone = skill.Clone();
                        for(int i = 0; i < FusionGrade; i++)
                            clone.LevelUp();

                        skills.AppendLine(clone.ToString());
                    }
                    else
                    {
                        skills.AppendLine(skill.ToString());
                    }
                    
                }
                    
            }
            else
            {
                skills.AppendLine("[NONE]");
            }

            return skills.ToString();
        }
    }
}
