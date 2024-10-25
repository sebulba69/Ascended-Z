using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class ResistanceChangerEnemy : WeaknessHunterEnemy
    {
        private int _resistNum = 1;

        public Elements Resist1 { get; set; }
        public Elements Resist2 { get; set; }
        
        public ResistanceChangerEnemy() : base()
        {
            Description = $"[RCE] - Resistance Changer Enemy: Will alternate its resistances + skills every move.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            // this is the default action that'll be used if no one has a weakness to anything this enemy has
            EnemyAction action = new EnemyAction();
            action = base.GetNextAction(battleSceneObject);
            if (!_isAgroOverride)
            {
                List<BattlePlayer> players = battleSceneObject.AlivePlayers;
                Elements resistance = (_resistNum == 1) ? Resist1 : Resist2;

                foreach (var skill in Skills)
                {
                    if (skill.Id == SkillId.Elemental)
                    {
                        ElementSkill elementSkill = (ElementSkill)skill;
                        if (elementSkill.Element == resistance)
                        {
                            action.Skill = elementSkill;
                            action.Target = FindElementSkillTarget(elementSkill, battleSceneObject);
                        }
                    }
                }
            }
            ChangeResistances();
            return action;
        }

        private void ChangeResistances()
        {
            Elements resist1;
            Elements resist2;

            if (_resistNum == 1)
            {
                resist1 = Resist2;
                resist2 = Resist1;
            }
            else
            {
                resist1 = Resist1;
                resist2 = Resist2;
            }

            ResistanceType rtype1 = this.Resistances.GetResistance(resist1);
            ResistanceType rtype2 = this.Resistances.GetResistance(resist2);

            Resistances.ClearResistances();

            Resistances.SetResistance(rtype1, Resist1);
            Resistances.SetResistance(rtype2, Resist2);

            if (_resistNum == 1)
                _resistNum = 2;
            else
                _resistNum = 1;
        }
    }
}
