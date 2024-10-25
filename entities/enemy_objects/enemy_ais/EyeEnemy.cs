using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using AscendedZ.entities.battle_entities;
using System.Collections.Generic;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class EyeEnemy : AlternatingEnemy
    {
        private bool _useEye;
        public ISkill EyeSkill { get; set; }

        public EyeEnemy()
        {
            Turns = 1;
            Description = "[EYE] - Eye Enemy: Will increase enemy turns if its weakness is hit. Otherwise, it attacks randomly.";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            _useEye = (result.ResultType == BattleResultType.Wk);

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var action = base.GetNextAction(battleSceneObject);

            if (_useEye) 
            {
                action.Target = this;
                action.Skill = EyeSkill;
                _useEye = false;
            }

            return action;
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(EyeSkill);
            return list;
        }

        public override void ResetEnemyState()
        {
            base.ResetEnemyState();
            _useEye = false;
        }
    }
}
