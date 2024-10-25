using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class BuffEnemy : AlternatingEnemy
    {
        private bool _useBuff = true;

        public BuffEnemy() : base()
        {
            Description = $"[BUFF] - Buff Enemy: Always buffs its element of choice before attacking randomly.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = base.GetNextAction(battleSceneObject);

            if (_useBuff)
            {
                action.Target = this;
                action.Skill = Skills[0];
            }
            else
            {
                action.Skill = Skills[_rng.Next(1, Skills.Count)];
            }

            _useBuff = !_useBuff;
            return action;
        }
    }
}
