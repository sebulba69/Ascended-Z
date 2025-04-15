using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

namespace AscendedZ.battle
{
    public enum TurnState { PLAYER, ENEMY }

    /// <summary>
    /// An object that houses all the important stuff we want
    /// to keep track of in our Battle Scene.
    /// </summary>
    public class BattleSceneObject
    {
        // When the player selects a skill
        public EventHandler StartEnemyDoTurn;
        public EventHandler MakeEnemyDoTurn;

        public EventHandler<BattleUIUpdate> UpdateUI;
        public EventHandler<PlayerTargetSelectedEventArgs> SkillSelected;

        private const int PLAYER_STATE = 0;
        private const int ENEMY_STATE = 1;
        private IBattleState[] _states = new IBattleState[] { new PlayerTurnState(), new EnemyTurnState() }; 
        private IBattleState _currentState;
        private TurnState _turnState;
        private int _tier;

        public List<BattlePlayer> Players { get; set; } = new();
        public List<Enemy> Enemies { get; set; } = new();
        public PressTurn PressTurn { get; set; } = new();
        public TurnState TurnState { get => _turnState; }
        public BattlePlayer ActivePlayer { get => Players.Find(p => p.IsActiveEntity); }
        public List<BattlePlayer> AlivePlayers { get => Players.FindAll(p => p.HP > 0); }
        public List<BattlePlayer> DeadPlayers { get => Players.FindAll(p => p.HP == 0); }
        public List<Enemy> AliveEnemies { get => Enemies.FindAll(e => e.HP > 0); }

        public BattleSceneObject(int tier)
        {
            _tier = tier;
            _currentState = _states[PLAYER_STATE];
            _turnState = TurnState.PLAYER;
        }

        /// <summary>
        /// This function is only called once at the start of the battle.
        /// It kicks off our state machine.
        /// </summary>
        public void InitializePartyMembers(List<BattlePlayer> players)
        {
            this.Players = players;
            this.SetPartyMemberTurns();
        }

        public void InitializeEnemies(int tier, bool dungeonCrawlEncounter, bool random = false, bool isRandomBoss = false, string tikki="")
        {
            if(random)
                Enemies = EntityDatabase.MakeRandomEnemyEncounter(tier, isRandomBoss);
            else if (tikki != "")
                Enemies = EntityDatabase.MakeTikkiMiniboss(tikki);
            else
            {
                if (PersistentGameObjects.GameObjectInstance().ProgressFlagObject.EndgameUnlocked && !dungeonCrawlEncounter && tier >= 251)
                {
                    tier += 60;
                }

                Enemies = EntityDatabase.MakeBattleEncounter(tier, dungeonCrawlEncounter);
            }

                
        }

        public void InitializeFullscreenBoss(int tier, string name, bool isLabrybuceBoss)
        {
            Enemies = EntityDatabase.MakeFullBodyBoss(tier, name, isLabrybuceBoss); // initialize a full string boy
        }

        /// <summary>
        /// A function to start the current state.
        /// Can be called at the start of battle if you pass in isBattleStart as true.
        /// </summary>
        public void StartBattle()
        {
            _currentState.StartState(this);

            foreach(var player in Players)
            {
                player.StatusHandler.AddStatus(player, SkillDatabase.AtkBuff.Status.Clone());
                player.StatusHandler.AddStatus(player, SkillDatabase.DefBuff.Status.Clone());
            }    
            
            foreach(var enemy in Enemies)
            {
                enemy.StatusHandler.AddStatus(enemy, SkillDatabase.AtkBuff.Status.Clone());
                enemy.StatusHandler.AddStatus(enemy, SkillDatabase.DefBuff.Status.Clone());
            }

            // we don't change the turn state at the start of a state change
            // we only enable user Input if turnState == turnstate.player
            this.PostUIUpdate();
        }

        public void PostUIUpdate(BattleResult result = null)
        {
            UpdateUI?.Invoke(this, new BattleUIUpdate()
            {
                UserCanInput = (_turnState == TurnState.PLAYER),
                Result = result
            });
        }

        public void SetPartyMemberTurns()
        {
            // it looks stupid, but C# doesn't natively recognize that a list of Players/Enemies are Battle Entities.
            SetEntityTurns(
                new List<BattleEntity>(this.Players),
                new List<BattleEntity>(this.Enemies));

            this.PostUIUpdate();
        }

        public void SetupEnemyTurns()
        {
            // it looks stupid, but C# doesn't natively recognize that a list of Players/Enemies are Battle Entities.
            SetEntityTurns(
                new List<BattleEntity>(this.Enemies),
                new List<BattleEntity>(this.Players));
        }

        private void SetEntityTurns(List<BattleEntity> turnEntities, List<BattleEntity> previousEntities)
        {
            int turns = 0;
            foreach(var entity in turnEntities)
            {
                if(entity.HP > 0 && entity.CanAttack)
                {
                    turns += entity.Turns;
                }
            }

            // we want to update the turn count on the people who last acted
            foreach (var entity in previousEntities)
                entity.StatusHandler.UpdateStatusTurns(entity, false);

            foreach (var entity in turnEntities)
                entity.StatusHandler.UpdateStatusTurns(entity, true);

            this.PressTurn.SetTurns(turns);
        }

        public void HandlePostTurnProcessing(BattleResult result)
        {
            if (result.Target?.HP == 0)
            {
                result.Target.StatusHandler.Clear();
            }

            result.User?.StatusHandler.ApplyBattleResult(result);
            result.Target?.StatusHandler.ApplyBattleResult(result);

            this.PressTurn.HandleTurns(result.ResultType);

            this.PostUIUpdate(result);
        }

        public void DoEnemyMove()
        {
            if(!DidEnemiesWin() && !DidPartyMembersWin())
            {
                this.MakeEnemyDoTurn?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.PostUIUpdate();
            }
        }

        public void ChangeActiveEntity()
        {
            _currentState.ChangeActiveEntity(this);
        }

        /// <summary>
        /// Make sure all our party members are still alive.
        /// </summary>
        public bool DidEnemiesWin()
        {
            return this.AlivePlayers.Count == 0;
        }

        /// <summary>
        /// Make sure all our enemies are still alive.
        /// </summary>
        public bool DidPartyMembersWin()
        {
            return this.Enemies.FindAll(enemy => enemy.HP > 0).Count == 0;
        }

        public void ChangeTurnState()
        {
            _currentState.EndState(this);
            if (_turnState == TurnState.PLAYER)
            {
                _turnState = TurnState.ENEMY;
                _currentState = _states[ENEMY_STATE];
            }
            else
            {
                _turnState = TurnState.PLAYER;
                _currentState = _states[PLAYER_STATE];
            }
            _currentState.StartState(this);
        }
    }
}
