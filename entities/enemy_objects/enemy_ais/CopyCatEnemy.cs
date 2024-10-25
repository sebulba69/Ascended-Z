using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class CopyCatEnemy : AlternatingEnemy
    {
        private BattleResult _lastUsedSkill;

        public CopyCatEnemy() : base()
        {
            Description = "[CC] - Copy Cat Enemy: Will always copy the last elemental skill used against it and will target the player who last attacked it. Targeting is overwritten by Agro.";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            _lastUsedSkill = result;

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = base.GetNextAction(battleSceneObject);

            if(_lastUsedSkill != null)
            {
                action.Skill = _lastUsedSkill.SkillUsed.Clone();
                if (!_isAgroOverride && _lastUsedSkill.User.HP > 0)
                {
                    action.Target = _lastUsedSkill.User;
                }   
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _lastUsedSkill = null;

            base.ResetEnemyState();
        }
    }
}
