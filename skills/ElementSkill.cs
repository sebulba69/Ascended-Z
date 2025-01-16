using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.sigils;
using AscendedZ.game_object;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public partial class ElementSkill : ISkill 
    {
        public int Cap
        {
            get
            {
                var go = PersistentGameObjects.GameObjectInstance();

                int cap = 300;
                if (go.MaxTier < 250)
                    cap = MiscGlobals.GetSoftcap() - 1;

                if (go.ProgressFlagObject.EndgameUnlocked)
                    cap = 450;

                return cap;
            }
        }

        public SkillId Id => SkillId.Elemental;
        private int _damage;
        private int _damageModifier = 0;
        private int _tier = 1;
        private int _level = 0;
        private string _baseName;
        private bool _forceWex = false;
        private bool _piercing = false;
        public int TransferLevel { get; set; }
        public string Description 
        { 
            get 
            {
                string description = $"Deals {Damage:n0} {Element} damage to";

                if (TargetType == TargetTypes.SINGLE_OPP)
                    description = $"{description} a single enemy.";
                else
                    description = $"{description} multiple enemies.";

                if (_piercing)
                    description += "\nIgnores Rs/Nu resistances (but not Guard/Dr)";

                return description;
            }
        }

        public string Name
        {
            get
            {
                string name;

                if(_level == 0)
                {
                    name = _baseName;
                }
                else
                {
                    name = $"{_baseName} +{_level}";
                }

                if (_piercing)
                    name = "[P] " + name;

                return name;
            }
        }
        public string BaseName { get => _baseName; set => _baseName = value; }
        public TargetTypes TargetType { get; set; }
        public Elements Element { get; set; }
        public int Level { get => _level; set => _level = value; }
        public int Tier { get => _tier; set => _tier = value; }
        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }

        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }

        /// <summary>
        /// List of statuses that are applied in addition to the element skill
        /// </summary>
        public List<StatusSkill> Statuses { get; set; }

        public bool ForceWex { get => _forceWex; set => _forceWex = value; }
        public bool Piercing { get => _piercing; set => _piercing = value; }

        public ElementSkill()
        {
            Statuses = new List<StatusSkill>();
        }

        public void ApplySigil(Sigil sigil)
        {
            int index = sigil.StatIndex - 1;
            if((Elements)index == Element)
            {
                Damage = Equations.ApplyIntegerBoostPercentage(Damage, sigil.BoostPercentage);
            }
        }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            var result = target.ApplyElementSkill(user, this);
            if(TargetType == TargetTypes.SINGLE_OPP)
            {
                if (user.StatusHandler.HasStatus(StatusId.TechnicalStatus))
                    user.StatusHandler.RemoveStatus(user, StatusId.TechnicalStatus);

                if (user.StatusHandler.HasStatus(StatusId.FocusStatus))
                    user.StatusHandler.RemoveStatus(user, StatusId.FocusStatus);
            }

            // don't bother applying statuses if you drain or null the element
            if (Statuses.Count > 0 && target.HP > 0)
            {
                if(result.ResultType != BattleResultType.Nu && result.ResultType != BattleResultType.Dr)
                {
                    foreach (var status in Statuses)
                    {
                        // don't do anything w/ the result
                        status.ProcessSkill(user, target);
                    }
                }
            }

            return result;
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            BattleResult all = ProcessSkill(user, targets[0]);

            all.Target = null;
            all.AllHPChanged.Add(all.HPChanged);
            all.Results.Add(all.ResultType);
            all.Targets.Add(targets[0]);

            // Wk, Rs, Nu, Dr, Norm, Guard, Tech, TechWk, Evade
            int[] bResultRanking = { 2, 1, 3, 4, 0, 1, 2, 2, 3 };

            int compare = bResultRanking[(int)all.ResultType];
            for (int i = 1; i < targets.Count; i++)
            {
                BattleResult result = ProcessSkill(user, targets[i]);

                all.Results.Add(result.ResultType);

                int integerResult = bResultRanking[(int)result.ResultType];

                if(compare < integerResult)
                {
                    compare = integerResult;
                    all.ResultType = result.ResultType;
                }
                    

                all.AllHPChanged.Add(result.HPChanged);
                all.Targets.Add(targets[i]);
            }

            // remove technical at the end so it applies to all enemies/players
            if (user.StatusHandler.HasStatus(StatusId.TechnicalStatus))
                user.StatusHandler.RemoveStatus(user, StatusId.TechnicalStatus);

            if(user.StatusHandler.HasStatus(StatusId.FocusStatus))
                user.StatusHandler.RemoveStatus(user, StatusId.FocusStatus);

            return all;
        }


        public string GetBattleDisplayString()
        {
            return $"{this.Name} ({this.Damage:n0})";
        }

        public override string ToString()
        {
            return $"[{this.Element.ToString()}] {this.Name} ({this.Damage:n0})";
        }

        public void LevelUp()
        {
            int boost = GetBoostValue(Level);
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
                this.Damage += boost;
            }
            catch(Exception)
            {
                this.Damage = int.MaxValue - 1;
            }
        }

        public string GetUpgradeString()
        {
            if (_level == Cap)
                return ToString();
            else
                return $"{ToString()} → {(this.Damage + GetBoostValue(Level)):n0}";
        }

        private int GetBoostValue(int level)
        {
            int boost = (level + 1 / 2) + 1;
            if (boost == 0)
                boost = 1;

            return boost;
        }

        public ISkill Clone()
        {
            var elementSkill = new ElementSkill()
            {
                BaseName = this.BaseName,
                Damage = this.Damage,
                TargetType = this.TargetType,
                Element = this.Element,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                Level = this.Level,
                Tier = this.Tier,
                ForceWex = ForceWex,
                Piercing = Piercing,
                TransferLevel = TransferLevel,
            };

            foreach(var status in Statuses)
            {
                elementSkill.Statuses.Add((StatusSkill)status.Clone());
            }

            return elementSkill;
        }


    }
}
