using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class FlatBuffDebuffEnemy : WeaknessHunterEnemy
    {
        private bool _usedBuff;
        public ISkill FlatBuffDebuff { get; set; }

        public FlatBuffDebuffEnemy()
        {
            Turns = 1;
            Description = "[FBDF] - Flat Buff/Debuff Enemy: Alternates between applying a buff/debuff and hitting weaknesses.";
        }


        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var action = base.GetNextAction(battleSceneObject);

            if (!_usedBuff)
            {
                action.Target = null;
                action.Skill = FlatBuffDebuff;
            }

            _usedBuff = !_usedBuff;
            return action;
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(FlatBuffDebuff);
            return list;
        }

        public override void ResetEnemyState()
        {
            base.ResetEnemyState();
        }
    }
}
