using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.Projection;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// An enemy that only attacks players with a specific status.
    /// Status skill must be at slot 0.
    /// Attack skill must be at slot 1.
    /// </summary>
    public class StatusAttackEnemy : Enemy
    {
        protected Status _status;
        private bool _alternateStatus;
        private const int STATUS_SKILL = 0;
        private const int ATTACK_SKILL = 1;
        public Status Status { set => _status = value; }

        public StatusAttackEnemy()
        {
            _alternateStatus = true;
            Turns = 1;
            Description = $"Randomly applies a status to a player without one unless there is either only one player alive or one player without the status, then it uses elemental skills.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;
            ISkill skill;
            BattleEntity target;

            BattlePlayer playerAffectedByAgro = players.Find((player) => { return player.StatusHandler.HasStatus(StatusId.AgroStatus); });

            // agro overrides this enemy's programming
            if(playerAffectedByAgro == null)
            {
                // find a player who does not have the status
                List<BattlePlayer> playersUnaffectedByStatus = players.FindAll((player) => { return !player.StatusHandler.HasStatus(_status.Id); });

                if(_status.Id == StatusId.StunStatus)
                {
                    _alternateStatus = !_alternateStatus;
                }

                // do not apply a status if there is only one player left
                if (playersUnaffectedByStatus.Count > 1 && players.Count > 1)
                {
                    if (_status.Id == StatusId.StunStatus)
                    {
                        if (!_alternateStatus)
                        {
                            skill = Skills[STATUS_SKILL];
                            target = playersUnaffectedByStatus[_rng.Next(playersUnaffectedByStatus.Count)];
                        }
                        else
                        {
                            skill = Skills[ATTACK_SKILL];
                            target = players[_rng.Next(players.Count)];
                        }
                    }
                    else
                    {
                        skill = Skills[STATUS_SKILL];
                        target = playersUnaffectedByStatus[_rng.Next(playersUnaffectedByStatus.Count)];
                    }
                }
                else
                {
                    skill = Skills[ATTACK_SKILL];
                    target = players[_rng.Next(players.Count)];
                }
            }
            else
            {
                skill = Skills[ATTACK_SKILL];
                target = playerAffectedByAgro;
            }


            return new EnemyAction
            {
                Skill = skill,
                Target = target
            };
        }

        public override void ResetEnemyState()
        {
            // Do nothing.
        }
    }
}
