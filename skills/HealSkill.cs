using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.skills
{
    public class HealSkill : ISkill
    {
        public int Cap
        {
            get
            {
                var go = PersistentGameObjects.GameObjectInstance();

                int cap = 300;
                if(go.MaxTier < 250)
                    cap = MiscGlobals.GetSoftcap() - 1;

                if (go.ProgressFlagObject.EndgameUnlocked)
                    cap = 450;

                return cap;
            }
        }

        public SkillId Id => SkillId.Healing;
        private int _level = 0;
        private string _baseName, _description;
        private int _tier = 1;
        public int TransferLevel { get; set; }
        public string Name
        {
            get
            {
                if (_level == 0)
                    return _baseName;
                else
                    return $"{_baseName} +{_level}";
            }
        }

        public string BaseName { get => _baseName; set => _baseName = value; }
        public int Level { get => _level; set => _level = value; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get ; set; }
        public int HealAmount { get; set; }
        public int Tier { get => _tier; set => _tier = value; }

        public List<Status> RemoveStatusAilments { get; set; }

        public string Description
        {
            get
            {
                string description;
                if (TargetType == TargetTypes.SINGLE_TEAM_DEAD)
                    description = $"Revives a dead player for {HealAmount:n0}.";
                else if (TargetType == TargetTypes.SINGLE_TEAM)
                    description = $"Heals a single player for {HealAmount:n0}.";
                else if (TargetType == TargetTypes.TEAM_ALL_DEAD)
                    description = $"Revives all dead players for {HealAmount:n0}.";
                else
                    description = $"Heals all players for {HealAmount:n0}";

                if(RemoveStatusAilments.Count > 0)
                {
                    var ailments = new List<string>();

                    foreach(var ailment in RemoveStatusAilments)
                    {
                        if (ailment.Name.Contains("Weak"))
                        {
                            ailments.Add("Weak- Skills");
                            break;
                        }
                            
                        ailments.Add(ailment.Name);
                    }

                    description += $"\nRemoves {string.Join(", ", ailments)}";
                }

                return description;
            }
        }
        public HealSkill()
        {
            RemoveStatusAilments = new List<Status>();
        }

        public string GetBattleDisplayString()
        {
            return $"{this.Name} ({this.HealAmount:n0} HP)";
        }

        public void LevelUp()
        {
            int boost = (Level + 1) * 2;
            
            try
            {
                _level++;
            }
            catch (Exception)
            {
                _level = int.MaxValue - 1;
            }

            try
            {
                this.HealAmount += boost;
            }
            catch (Exception)
            {
                this.HealAmount = int.MaxValue - 1;
            }
        }

        public string GetUpgradeString()
        {
            if (_level == Cap)
                return GetBattleDisplayString();
            else
                return $"{GetBattleDisplayString()} → {this.HealAmount + (Level + 1) * 2:n0}";
        }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            foreach (var status in RemoveStatusAilments)
                target.StatusHandler.RemoveStatus(target, status.Id);

            return target.ApplyHealingSkill(this);
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            BattleResult all = ProcessSkill(user, targets[0]);
            all.Target = null;
            all.Results.Add(all.ResultType);
            all.AllHPChanged.Add(all.HPChanged);
            all.Targets.Add(targets[0]);

            for (int i = 1; i < targets.Count; i++)
            {
                BattleResult result = ProcessSkill(user, targets[i]);
                all.AllHPChanged.Add(result.HPChanged);
                all.Targets.Add(targets[i]);
                all.Results.Add(result.ResultType);
            }

            return all;
        }

        public override string ToString()
        {
            return this.GetBattleDisplayString();
        }

        public ISkill Clone()
        {
            var hs = new HealSkill()
            {
                BaseName = this.BaseName,
                Level = Level,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                HealAmount = this.HealAmount,
                Tier = this.Tier,
                TransferLevel = this.TransferLevel,
            };

            foreach (var st in RemoveStatusAilments)
                hs.RemoveStatusAilments.Add(st.Clone());

            return hs;
        }
    }
}
