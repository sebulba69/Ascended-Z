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
    public class BohumirCibulka : Enemy
    {
        private ISkill _almighty;
        private int _phaseIndex;
        private int _subPhaseIndex;

        private List<List<int>> _phases =
        [
            [ 2, 0, 1 ],
            [ 3, 4, 5 ],
            [ 2, 1, 4 ]
        ];

        public BohumirCibulka()
        {
            Name = EnemyNames.Bohumir_Cibulka;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 2;
            _isBoss = true;

            _phaseIndex = 0;
            _subPhaseIndex = 0;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);
            _almighty = SkillDatabase.Almighty1;

            Skills.Add(SkillDatabase.DefDebuff);
            Skills.Add(SkillDatabase.Ice1);
            Skills.Add(SkillDatabase.BeastEye);
            Skills.Add(SkillDatabase.VoidFire);
            Skills.Add(SkillDatabase.Light1);
            Skills.Add(SkillDatabase.TechBuffAll);
            Skills.Add(_almighty);

            Description = $"{Name} attacks in 3 phases depending on its health percentage:\n" +
                $"60%+ HP, it will focus on debuffing and using Ice attacks.\n\n" +
                $"30-60% HP, it will focus on covering its weakness, using Light attacks, applying the Technical status to its team.\n\n" +
                $"30% and below, it will focus on both Ice and Light attacks.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            var phase = _phases[_phaseIndex];

            if (_subPhaseIndex < phase.Count)
            {
                int skillIndex = phase[_subPhaseIndex];

                action.Skill = Skills[skillIndex];
            }
            else
            {
                action.Skill = _almighty;
            }

            if (action.Skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else
            {
                action.Target = this;
            }

            _subPhaseIndex++;

            return action;
        }

        public override void ResetEnemyState()
        {
            int percentage = (int)(((double)HP / MaxHP) * 100);

            if (percentage <= 60 && percentage >= 30)
            {
                _phaseIndex = 1;
            }
            else if (percentage < 30)
            {
                _phaseIndex = 2;
            }
            else
            {
                _phaseIndex = 0;
            }


            _subPhaseIndex = 0;
        }
    }
}
