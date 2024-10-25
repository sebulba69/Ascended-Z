using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class AlexChurch : Enemy
    {
        private int _skillsTaken, _stolenSkillIndex;
        private bool _resetSkills = false, _doCopySkills = false;
        private const int REMOVE_START_INDEX = 2;
        
        public AlexChurch() : base()
        {
            Name = EnemyNames.Alex_Church;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 2;
            _isBoss = true;

            _skillsTaken = 0;

            Skills.AddRange(
                [
                    SkillDatabase.SkillCopy.Clone(), // 0
                    SkillDatabase.Almighty.Clone(), // 1
                ]);

            Description = $"{Name}: Will attempt to steal the first elemental skill in each player's skill list. After taking a skill, {Name}'s resistances will change to be weak to the opposite element of the element tied to the skill. An elemental skill isn't found in a party member's first slot, it will be replaced with an Almighty skill instead.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(SkillDatabase.DefDebuff);
            list.Add(SkillDatabase.DragonEye);
            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            if (_skillsTaken == 0 && _doCopySkills)
            {
                action.Skill = Skills[0];
                action.Target = null;

                var alivePlayers = battleSceneObject.AlivePlayers;

                foreach (var player in alivePlayers)
                {
                    var firstSkill = player.Skills[0];
                    Skills.Add(firstSkill.Clone());

                    if (firstSkill.Id == SkillId.Elemental)
                    {
                        var element = (ElementSkill)firstSkill;
                        var opposite = SkillDatabase.ElementalOpposites[element.Element];
                        Resistances.SetResistance(ResistanceType.Nu, element.Element);

                        if(!Resistances.IsNullElement(opposite) && !Resistances.IsDrainElement(opposite))
                        {
                            Resistances.SetResistance(ResistanceType.Wk, opposite);
                        }
                    }
                    else
                    {
                        Resistances.SetResistance(ResistanceType.Dr, (Elements)_skillsTaken);
                    }

                    _skillsTaken++;
                }

                _stolenSkillIndex = (Skills.Count - _skillsTaken) - 1;
            }
            else if(_skillsTaken == 0 && !_doCopySkills)
            {
                action.Skill = SkillDatabase.DefDebuff;
                action.Target = null;
                _doCopySkills = true;
            }
            else
            {
                
                if(!_resetSkills)
                    _resetSkills = true;

                if(_stolenSkillIndex < Skills.Count)
                {
                    if(_stolenSkillIndex == 1)
                    {
                        action.Skill = SkillDatabase.DragonEye;
                        _stolenSkillIndex++;
                    }
                    else
                    {
                        action.Skill = Skills[_stolenSkillIndex++];

                        if (action.Skill.Id == SkillId.Elemental)
                        {
                            var elemental = (ElementSkill)action.Skill;
                            action.Target = FindElementSkillTarget(elemental, battleSceneObject);
                        }
                        else
                        {
                            action.Skill = Skills[1];
                            action.Target = null;
                        }
                    }
                }
                else
                {
                    action.Skill = Skills[1];
                    action.Target = null;
                }
            }
            return action;
        }

        public override void ResetEnemyState()
        {
            if (_resetSkills) 
            {
                Skills.RemoveRange(REMOVE_START_INDEX, Skills.Count - REMOVE_START_INDEX);
                _skillsTaken = 0;
                _resetSkills = false;
                _doCopySkills = false;
                Resistances.ClearResistances();
            }
        }
    }
}
