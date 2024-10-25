using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class GriffenHart : Enemy
    {
        private ISkill _almighty;
        private int _phaseIndex;
        private int _subPhaseIndex;

        private List<List<int>> _phases =
        [
            [ 0, 1, 2 ],
            [ 3, 4, 5 ],
            [ 1, 4, 2 ]
        ];

        public GriffenHart()
        {
            Name = EnemyNames.Griffen_Hart;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 2;
            _isBoss = true;

            _phaseIndex = 0;
            _subPhaseIndex = 0;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            _almighty = SkillDatabase.Almighty1;

            Skills.Add(SkillDatabase.Hellfire);
            Skills.Add(SkillDatabase.Dark1);
            Skills.Add(_almighty);
            Skills.Add(SkillDatabase.VoidIce);
            Skills.Add(SkillDatabase.Fire1);
            Skills.Add(SkillDatabase.Curse);

            Description = $"{Name} attacks in 3 phases depending on its health percentage:\n" +
                            $"60%+ HP: it will focus on debuffing and using single-hit Dark attacks, all-hit Fire attacks, and all-hit Almighty attacks.\n\n" +
                            $"30-60% HP: it will focus on covering its weakness, using single-hit Fire attacks, and all-hit Dark attacks.\n\n" +
                            $"30% and below: it will focus on single-hit Fire and Dark attacks.";
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
