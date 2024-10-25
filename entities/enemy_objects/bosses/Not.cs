using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class Not : Enemy
    {
        private int _move;
        private List<ISkill> _script;

        public Not() : base()
        {
            Name = EnemyNames.Not;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 2;
            _isBoss = true;

            var beye = SkillDatabase.BeastEye;
            var allWind = SkillDatabase.WindAll;
            var almighty = SkillDatabase.Almighty;
            var allFire = SkillDatabase.FireAll;
            var heal = SkillDatabase.Heal1;
            var debilitate = SkillDatabase.Debilitate;
            var lusterCandy = SkillDatabase.LusterCandy;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Light);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            Skills.AddRange([
                beye, allWind, almighty, allFire, heal, debilitate, lusterCandy
                ]);

            _script = [beye, allWind, lusterCandy, beye, beye, debilitate, beye, allFire, lusterCandy, beye, debilitate, almighty, heal];
            _move = 0;

            Description = $"{Name}: Will cast its skills in this order (picks up where it last left off every turn): {string.Join(", ", _script)}";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.AddRange(
                [
                    SkillDatabase.BeastEye,
                    SkillDatabase.LusterCandy,
                    SkillDatabase.Debilitate
                ]);

            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();
            
            action.Skill = _script[_move];

            if (action.Skill.Id == SkillId.Healing)
            {
                action.Target = this;
            }
            else if (action.Skill.Id == SkillId.Status) 
            {
                action.Target = null;
            }
            else if(action.Skill.Id == SkillId.Eye)
            {
                action.Target = this;
            }
            else
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }

            _move++;
            if (_move >= _script.Count)
                _move = 0;

            return action;
        }

        public override void ResetEnemyState()
        {
        }
    }
}
