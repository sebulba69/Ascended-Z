using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using static Godot.WebSocketPeer;
using System.Text.Json.Serialization;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// Generic AI for boss battles so I don't have to hard script every single fight.
    /// </summary>
    [JsonDerivedType(typeof(BossHellAI), typeDiscriminator:nameof(BossHellAI))]
    public class BossHellAI : Enemy
    {
        private int _move;
        private int _wexHitCount;
        private bool _useVoid = true;

        public BossHellAI() : base()
        {
            _move = 0;
            _wexHitCount = 0;
            _isBoss = true;

        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk)
            {
                _wexHitCount++;
                _useVoid = _wexHitCount >= 3;
            }
                

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            ISkill skill = Skills[_move];

            EnemyAction action = new EnemyAction() { Skill = skill };

            if (skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)skill, battleSceneObject);
            }
            else if (skill.Id == SkillId.Status) 
            {
                if (!skill.BaseName.Contains("Void"))
                {
                    if (skill.TargetType == TargetTypes.OPP_ALL)
                        action.Target = battleSceneObject.AlivePlayers[0];
                    else
                        action.Target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
                else
                {
                    action.Target = FindVoidStatusTarget((StatusSkill)skill, battleSceneObject);
                }
            }
            else if (skill.Id == SkillId.Eye)
            {
                if(_wexHitCount == 0)
                {
                    action.Target = null;
                }
                else
                {
                    _wexHitCount--;
                    action.Target = this;
                }
            }

            if(action.Target == null)
            {
                // find the nearest elemental skill
                IncrementMove();
                action = GetNextAction(battleSceneObject);
            }
            else
            {
                IncrementMove();
            }

            return action;
        }



        private BattleEntity FindVoidStatusTarget(StatusSkill skill, BattleSceneObject battleSceneObject)
        {
            var status = skill.Status;

            if(StatusHandler.HasStatus(status.Id) || !_useVoid)
            {
                return null;
            }
            else
            {
                _useVoid = false;
                return this;
            }
        }

        private void IncrementMove()
        {
            _move++;
            if (_move >= Skills.Count)
                _move = 0;
        }

        public override void ResetEnemyState()
        {
            _wexHitCount = 0;
        }

        public override void HardReset()
        {
            ResetEnemyState();
            HP = MaxHP;
            StatusHandler.Clear();
            DefenseModifier = 0;
            for (int i = 0; i < ElementDamageModifiers.Length; i++)
            {
               ElementDamageModifiers[i] = 0;
            }
            _move = 0;
        }
    }
}
