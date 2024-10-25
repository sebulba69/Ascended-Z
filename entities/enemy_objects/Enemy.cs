using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AscendedZ.entities.enemy_objects.enemy_ais;
using System.Text.Json.Serialization;
using System.ComponentModel.Design;
using AscendedZ.game_object;
using Godot;

namespace AscendedZ.entities.enemy_objects
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(BossHellAI), typeDiscriminator: nameof(BossHellAI))]
    public class Enemy : BattleEntity
    {
        protected bool _isBoss = false;
        protected bool _isAgroOverride = false;
        public bool IsBoss { get => _isBoss; set => _isBoss = value; }
        public bool RandomEnemy { get; set; }

        protected Random _rng;

        public string Description { get; set; }

        /// <summary>
        /// EventHandler for special bosses only
        /// </summary>
        public EventHandler<string[]> PlayDialog;

        public Enemy() 
        {
            Type = EntityType.Enemy;
            _rng = new Random();
        }

        public void Boost(int tier, bool quickBoost = false)
        {
            int boost = (tier+1);
            if(boost == 0)
                boost = 1;

            if(!quickBoost)
                MaxHP *= (int)(boost * 0.75);
                
            double scalar = 2.0;

            if (tier > 30)
                scalar = 2.5;

            int levelUps = (int)((boost / scalar) + 1);
            for (int i = 0; i < levelUps; i++)
            {
                foreach (ISkill skill in Skills)
                {
                    skill.LevelUp();
                }
            }
        }

        public virtual List<ISkill> GetDisplaySkillList()
        {
            return new List<ISkill>(Skills);
        }

        public virtual void ResetEnemyState()
        {
            throw new NotImplementedException();
        }

        public virtual void HardReset() { }

        /// <summary>
        /// Get a Target + a Skill to be used during the next battle.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            throw new NotImplementedException();
        }

        protected BattleEntity GetRandomAlivePlayer(BattleSceneObject battleSceneObject)
        {
            var player = battleSceneObject.AlivePlayers[_rng.Next(battleSceneObject.AlivePlayers.Count)];
            return player;
        }

        protected BattleEntity GetTargetAffectedByAgro(BattleSceneObject battleSceneObject)
        {
            var agro = battleSceneObject.AlivePlayers.Find(p => p.StatusHandler.HasStatus(statuses.StatusId.AgroStatus));

            _isAgroOverride = (agro != null);

            return agro;
        }

        /// <summary>
        /// Utility function for a feature common to most enemy AI.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected List<BattlePlayer> FindPlayersWithWeaknessToElement(BattleSceneObject battleSceneObject, Elements element)
        {
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;

            return players.FindAll(player => player.Resistances.IsWeakToElement(element));
        }

        protected List<BattlePlayer> FindPlayersUnaffectedByStatus(BattleSceneObject battleSceneObject, Status status)
        {
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;

            return players.FindAll(player => player != null && !player.StatusHandler.HasStatus(status.Id));
        }

        protected BattleEntity FindElementSkillTarget(ElementSkill skill, BattleSceneObject battleSceneObject)
        {
            var wex = FindPlayersWithWeaknessToElement(battleSceneObject, skill.Element);

            if (wex.Count > 0)
                return wex[_rng.Next(wex.Count)];

            List<BattlePlayer> targets = battleSceneObject.AlivePlayers.FindAll(
                p => p != null && 
                !p.Resistances.IsNullElement(skill.Element) && 
                !p.Resistances.IsDrainElement(skill.Element));

            if (targets.Count == 0 || skill.Piercing)
            {
                return GetRandomAlivePlayer(battleSceneObject);
            }
            else
            {
                return targets[_rng.Next(targets.Count)];
            }
        }

        protected BattleEntity FindTargetForStatus(StatusSkill status, BattleSceneObject battleSceneObject)
        {   
            if(status.TargetType == TargetTypes.SINGLE_OPP)
            {
                List<BattlePlayer> players = FindPlayersUnaffectedByStatus(battleSceneObject, status.Status);

                // no reason not to apply buffs/debuffs
                if (players.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (status.Status.Id == StatusId.StunStatus && players.Count == 1)
                    {
                        return null;
                    }
                    else
                    {
                        return players[_rng.Next(_rng.Next(players.Count))];
                    }
                }
            }
            else
            {
                return this;
            }
        }
    }
}
