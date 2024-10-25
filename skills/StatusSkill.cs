using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.skills
{
    public class StatusSkill : ISkill
    {
        private bool _isRemoveStatusSkill = false;
        private string _description;
        public SkillId Id => SkillId.Status;
        public string BaseName { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public Status Status { get; set; }
        public List<Status> Statuses { get; set; }
        public string Name => BaseName;
        public bool IsCounterDecreaseStatus { get; set; } // for when we just want to decrease the counter on a stat
        public bool IsRemoveStatusSkill { get => _isRemoveStatusSkill; set => _isRemoveStatusSkill = value; }
        public int TransferLevel { get; set; }
        public string Description
        {
            get
            {
                return _description;
            }
        }

        public StatusSkill()
        {
            Level = 1;
            Statuses = new List<Status>();
        }

        public void SetDescription(string description)
        {
            _description = description;
        }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            if(BaseName == "Ancient Choir")
            {
                // if same type as user, apply benefits
                if(target.BaseName == user.BaseName)
                {
                    var holyGrail = SkillDatabase.HolyGrail;
                    holyGrail.ProcessSkill(user, target);
                }
                else
                {
                    // apply debuffs to opposite
                    var torpefy = SkillDatabase.Torpefy;
                    torpefy.ProcessSkill(user, target);
                }
            }

            BattleResult result = new BattleResult()
            {
                SkillUsed = this,
                Target = target,
                ResultType = (!IsRemoveStatusSkill) ? BattleResultType.StatusApplied : BattleResultType.StatusRemoved
            };

            if (BaseName != "Ancient Choir")
            {
                if (Statuses.Count == 0)
                    ProcessStatus(target, Status);
                else
                    foreach (var status in Statuses)
                        ProcessStatus(target, status);
            }
            return result;
        }

        private void ProcessStatus(BattleEntity target, Status status)
        {
            if (IsRemoveStatusSkill)
            {
                target.StatusHandler.RemoveStatus(target, status.Id);
            }
            else if (IsCounterDecreaseStatus)
            {
                // if they don't have the status, then apply it
                target.StatusHandler.AddStatus(target, status, true);
            }
            else
            {
                target.StatusHandler.AddStatus(target, status);
            }
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            BattleResult all = ProcessSkill(user, targets[0]);
            
            all.Target = null;
            all.Results.Add(all.ResultType);

            for(int i = 1; i < targets.Count; i++)
            {
                BattleResult r = ProcessSkill(user, targets[i]);
                all.Results.Add(r.ResultType);
            }

            all.Targets.AddRange(targets);
            return all;
        }

        public string GetBattleDisplayString()
        {
            return $"{this.BaseName}";
        }

        public void LevelUp()
        {
        }

        public override string ToString()
        {
            return GetBattleDisplayString();
        }

        public string GetUpgradeString()
        {
            return $"{GetBattleDisplayString()} [NO UPGRADE]";
        }

        public string GetAscendedString(int ascendedLevel)
        {
            return GetBattleDisplayString();
        }

        public ISkill Clone()
        {
            var statusSkill = new StatusSkill()
            {
                BaseName = this.BaseName,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                IsRemoveStatusSkill = this.IsRemoveStatusSkill,
                IsCounterDecreaseStatus = IsCounterDecreaseStatus
            };

            if (Status == null)
                statusSkill.Statuses = new List<Status>(Statuses);
            else
                statusSkill.Status = Status.Clone();

            return statusSkill;
        }
    }
}
