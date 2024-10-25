using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class Pakorag : Enemy
    {
        private List<ISkill> _skills1, _skills2, _current;
        private int _currentSkill;
        private ISkill _buff, _buffBoost, _choir, _beastEye;
        private bool _buffBoosted;

        public Pakorag() : base()
        {
            Name = EnemyNames.Pakorag;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHPDC(Name);
            Turns = 4;
            _isBoss = true;
            _currentSkill = 0;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            var iceP1 = SkillDatabase.IceMadGod;
            var windP1 = SkillDatabase.WindMadGod;
            var icePA = SkillDatabase.IceAllP;
            var windPA = SkillDatabase.WindAllP;
            var almighty = SkillDatabase.Almighty;

            _beastEye = SkillDatabase.BeastEye;
            _buff = SkillDatabase.HolyGrail;
            _buffBoost = SkillDatabase.BuffBoost;
            _choir = SkillDatabase.AncientChoir;

            Skills.AddRange([iceP1, windP1, icePA, windPA, almighty]);

            _skills1 = [iceP1, windP1, _beastEye, _buff, _buff];
            _skills2 = [windPA, icePA, almighty, _choir];
            _current = _skills1;

            Description = $"{Name}: Can increase its buff stacks from 2 to 4. Alternates between two scripts:\n" +
                $"1. Uses single-hit Ice and Wind moves combined with buffs.\n\n" +
                $"2. Uses single-hit Piercing Ice and Wind moves combined with debuffs.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.Add(SkillDatabase.BeastEye);
            list.Add(SkillDatabase.HolyGrail);
            list.Add(SkillDatabase.AncientChoir);
            list.Add(SkillDatabase.BuffBoost);

            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            if(_currentSkill == 0 && !StatusHandler.HasStatus(StatusId.BuffBoost))
            {
                action.Skill = _buffBoost;
                action.Target = this;
            }
            else
            {
                action.Skill = _current[_currentSkill];
                if(action.Skill.Id == SkillId.Elemental)
                {
                    action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
                }
                else
                {
                    action.Target = this;
                }
            }

            _currentSkill++;
            if (_currentSkill >= _current.Count)
                _currentSkill = 0;

            return action;
        }

        public override void ResetEnemyState()
        {
            if(_current == _skills1)
            {
                _current = _skills2;
            }
            else
            {
                _current = _skills1;
            }

            _currentSkill = 0;
        }
    }
}