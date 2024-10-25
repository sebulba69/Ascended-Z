using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class Tier190Boss : Enemy
    {
        private int _turnCount;
        public int TurnsForSkillSwap { get; set; }
        public ISkill ReplacementSkill { get; set; }
        public List<ISkill> Script { get; set; }
        private int _move;

        public Tier190Boss() : base()
        {
            _isBoss = true;
            _move = 0;
            _turnCount = 0;
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = new List<ISkill>();
            list.AddRange(Script);
            list.Add(ReplacementSkill);
            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            if (_turnCount % TurnsForSkillSwap == 0 && _move == 0) 
            {
                action.Skill = ReplacementSkill;
            }
            else
            {
                action.Skill = Script[_move];
            }

            if (action.Skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else if (action.Skill.Id == SkillId.Healing)
            {
                var enemies = battleSceneObject.AliveEnemies;
                action.Target = enemies[0];

                for(int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].HP < action.Target.HP)
                    {
                        action.Target = enemies[i];
                    }
                }
            }
            else
            {
                action.Target = this;
            }

            _move++;
            if (_move >= Script.Count)
            {
                _move = 0;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _move = 0;
            _turnCount++;
        }
    }
}
