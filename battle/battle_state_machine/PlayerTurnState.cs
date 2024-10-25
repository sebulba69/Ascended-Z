using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using static Godot.Projection;

namespace AscendedZ.battle.battle_state_machine
{
    /// <summary>
    /// This state handles all activities that take place during the Player's turn.
    /// </summary>
    public class PlayerTurnState : IBattleState
    {
        private int _activePlayer;

        public void StartState(BattleSceneObject battleSceneObject)
        {
            var players = battleSceneObject.Players;
            _activePlayer = players.FindIndex(p => p.HP > 0 && p.CanAttack);
            if(_activePlayer >= 0)
            {
                var active = players[_activePlayer];
                active.IsActiveEntity = true;
                battleSceneObject.SkillSelected += _OnSkillSelected;
            }
        }

        private void _OnSkillSelected(object sender, PlayerTargetSelectedEventArgs eventArgs)
        {
            BattleSceneObject battleSceneObject = sender as BattleSceneObject;
            
            var players = battleSceneObject.Players;
            var active = players[_activePlayer];

            BattleResult result = null;

            ISkill skill;
            if (!eventArgs.Guard)
                skill = active.Skills[eventArgs.SkillIndex];
            else
                skill = SkillDatabase.Guard;

            if(active.StatusHandler.HasStatus(statuses.StatusId.GuardStatus) && skill.Id != SkillId.Pass)
                active.StatusHandler.RemoveStatus(active, statuses.StatusId.GuardStatus);

            // result cannot be null at the end of this function
            switch (skill.TargetType)
            {
                case TargetTypes.SINGLE_OPP:
                    // our only available targets are alive enemies
                    var possibleTargets = battleSceneObject.Enemies.FindAll(e => e.HP > 0);
                    var enemy = possibleTargets[eventArgs.TargetIndex];
                    result = skill.ProcessSkill(active, enemy);
                    break;
                case TargetTypes.SINGLE_TEAM:
                    BattleEntity player = battleSceneObject.AlivePlayers[eventArgs.TargetIndex];
                    result = skill.ProcessSkill(active, player);
                    break;
                case TargetTypes.SELF:
                    result = skill.ProcessSkill(active, active);
                    break;
                case TargetTypes.SINGLE_TEAM_DEAD:
                    var deadPlayer = battleSceneObject.DeadPlayers[eventArgs.TargetIndex];
                    result = skill.ProcessSkill(active, deadPlayer);
                    break;
                case TargetTypes.TEAM_ALL:
                    var targetPlayers = battleSceneObject.AlivePlayers;
                    result = skill.ProcessSkill(active, new List<BattleEntity>(targetPlayers));
                    break;
                case TargetTypes.OPP_ALL:
                    var targetEnemies = battleSceneObject.AliveEnemies;
                    result = skill.ProcessSkill(active, new List<BattleEntity>(targetEnemies));
                    break;
                case TargetTypes.TEAM_ALL_DEAD:
                    var deadPlayers = battleSceneObject.DeadPlayers;
                    result = skill.ProcessSkill(active, new List<BattleEntity>(deadPlayers));
                    break;
            }

            result.User = active;
            
            battleSceneObject.HandlePostTurnProcessing(result);
        }

        public void ChangeActiveEntity(BattleSceneObject battleSceneObject)
        {
            var players = battleSceneObject.Players;
            battleSceneObject.ActivePlayer.IsActiveEntity = false;
            do
            {
                _activePlayer++;
                if (_activePlayer == players.Count)
                    _activePlayer = 0;
            } while (players[_activePlayer].HP == 0 || !players[_activePlayer].CanAttack);

            players[_activePlayer].IsActiveEntity = true;
        }

        public void EndState(BattleSceneObject battleSceneObject)
        {
            foreach (var player in battleSceneObject.Players)
                player.IsActiveEntity = false;

            battleSceneObject.SkillSelected -= _OnSkillSelected;
        }
    }
}
