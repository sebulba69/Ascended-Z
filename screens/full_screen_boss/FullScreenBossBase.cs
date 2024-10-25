using AscendedZ.battle;
using AscendedZ.effects;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.game_object;
using AscendedZ.screens.end_screen;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using static Godot.Projection;

namespace AscendedZ.screens.full_screen_boss
{
    /// <summary>
    /// Generic base class for Full Screen Bosses that is meant
    /// to be used as a framework inherited by all bosses who take up
    /// the full scene
    /// </summary>
    public partial class FullScreenBossBase : Node2D
    {
        private readonly PackedScene _partyScreen = ResourceLoader.Load<PackedScene>(Scenes.PARTY_CHANGE);
        private readonly PackedScene _partyBox = ResourceLoader.Load<PackedScene>(Scenes.PARTY_BOX);
        private readonly PackedScene _enemyBox = ResourceLoader.Load<PackedScene>(Scenes.ENEMY_BOX);
        private readonly PackedScene _turnIcons = ResourceLoader.Load<PackedScene>(Scenes.TURN_ICONS);
        private readonly PackedScene _damageNumScene = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM);
        private readonly PackedScene _rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS);
        private readonly PackedScene _infoScreen = ResourceLoader.Load<PackedScene>(Scenes.INFO_SCREEN);

        // Camera
        protected ShakeyCam _camera;
        
        protected AscendedTextbox _dialogBox;

        protected FullScreenBossHUD _hud;
        protected HBoxContainer _turnIconContainer;
        protected EffectAnimation _effect;
        protected Node2D _enemySprite;

        // Player info
        protected ActionMenu _actionMenu;
        protected HBoxContainer _playerStuff, _partyMembers;
        protected CanvasLayer _playerUI, _enemyUI;

        // Skill info
        protected Label _skillName;
        protected TextureRect _skillIcon;
        protected PanelContainer _skillDisplayIcons;

        // end screen
        protected CenterContainer _endBox;
        protected EndScreenOptions _endScreenOptions;

        protected BattleSceneObject _battleSceneObject;
        protected CenterContainer _dmgMarker;

        private string _entityName;
        private bool _isLabrybuce;

        protected void InternalReady()
        {
            AddUserSignal("DialogGone");

            _skillName = this.GetNode<Label>("%SkillName");
            _skillIcon = this.GetNode<TextureRect>("%SkillIcon");
            _skillDisplayIcons = this.GetNode<PanelContainer>("%SkillDisplayIcons");

            _partyMembers = this.GetNode<HBoxContainer>("%PartyPortraits");
            _enemySprite = this.GetNode<Node2D>("%BattleSprite");
            _turnIconContainer = this.GetNode<HBoxContainer>("%TurnIconContainer");

            _endBox = this.GetNode<CenterContainer>("%EndBox");
            _endScreenOptions = GetNode<EndScreenOptions>("%EndScreenOptions");

            _actionMenu = this.GetNode<ActionMenu>("%ActionMenu");
            _dmgMarker = GetNode<CenterContainer>("%DamageMarker");
            _effect = GetNode<EffectAnimation>("%EffectSprite");

            _camera = GetNode<ShakeyCam>("%Camera");

            _playerUI = GetNode<CanvasLayer>("%PlayerUI");
            _enemyUI = GetNode<CanvasLayer>("%UIElements");

            _hud = GetNode<FullScreenBossHUD>("%FullScreenBossHud");

            _dialogBox.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialog"));

            var musicPlayer = PersistentGameObjects.GameObjectInstance().MusicPlayer;
            musicPlayer.SetStreamPlayer(GetNode<AudioStreamPlayer>("%MusicPlayer"));
            musicPlayer.ResetCurrentSong(); // do this to avoid conflicts w/ main screen
        }

        protected void InitializeBattleScene(string enemyName, bool isOpeningShot, bool isLabrybuceBoss)
        {
            GameObject gameObject = PersistentGameObjects.GameObjectInstance();
            int tier = gameObject.Tier;

            _entityName = enemyName;
            _isLabrybuce = isLabrybuceBoss;

            ClearChildrenFromNode(_partyMembers);

            _battleSceneObject = new BattleSceneObject(tier);
            _actionMenu.EmptyClick = false;
            _actionMenu.BattleSceneObject = _battleSceneObject;

            _battleSceneObject.InitializeFullscreenBoss(tier, enemyName, isLabrybuceBoss);
            _battleSceneObject.InitializePartyMembers(gameObject.MakeBattlePlayerListFromParty());
            _battleSceneObject.UpdateUI += _OnUIUpdate;

            _battleSceneObject.Enemies[0].PlayDialog += _OnDialogToDisplay;

            // add players to the scene
            foreach (var member in _battleSceneObject.Players)
            {
                HBoxContainer hBoxContainer = new HBoxContainer() { Alignment = BoxContainer.AlignmentMode.Center };
                hBoxContainer.AddThemeConstantOverride("theme_override_constants/separation", 0);
                var partyBox = _partyBox.Instantiate<EntityDisplayBox>();

                _partyMembers.AddChild(hBoxContainer);
                hBoxContainer.AddChild(partyBox);

                partyBox.InstanceEntity(new EntityWrapper() { BattleEntity = member });
                if (member.IsActiveEntity)
                {
                    _actionMenu.Reparent(hBoxContainer);
                    _actionMenu.EmptyClick = false;
                }
            }

            // setup hud!
            var enemy = _battleSceneObject.Enemies[0];
            _hud.Initialize(enemy);

            _hud.InfoButton.Pressed += () =>
            {
                if (!_actionMenu.CanInput)
                    return;

                _actionMenu.CanInput = false;

                var info = _infoScreen.Instantiate<BattleInfoBox>();

                _enemyUI.AddChild(info);

                info.SetEnemyInfo(enemy);

                info.TreeExited += () => _actionMenu.CanInput = true;
            };

            // set the turns and prep the b.s.o. for processing battle stuff
            _battleSceneObject.StartBattle();

            UpdateTurnsUsingTurnState(TurnState.PLAYER);

            if(!isOpeningShot)
                _actionMenu.CanInput = true;
        }

        private bool _skip = false;

        private async void _OnDialogToDisplay(object sender, string[] text)
        {
            _actionMenu.CanInput = false;
            _playerUI.Visible = false;
            _dialogBox.Visible = true;
            _skip = false;

            foreach(string t in text)
            {
                if (_skip)
                    break;

                _dialogBox.DisplayText(t);

                await ToSignal(_dialogBox, "ReadyForMoreDialogEventHandler");
            }

            _OnSkipDialog();
        }

        private void _OnSkipDialog()
        {
            _skip = true;

            EmitSignal("DialogGone");
            _playerUI.Visible = true;
            _partyMembers.Visible = true;
            _dialogBox.Visible = false;
        }

        protected void ClearChildrenFromNode(Node node)
        {
            foreach (var child in node.GetChildren())
            {
                node.RemoveChild(child);
                child.QueueFree();
            }
        }

        private async void _OnUIUpdate(object sender, BattleUIUpdate update)
        {
            if (_dialogBox.Visible)
            {
                await ToSignal(this, "DialogGone");
            }

            // handle battle results if any
            if (update.Result != null)
            {
                var result = update.Result;

                // check if we're running from this battle
                if (result.ResultType == BattleResultType.Retreat)
                {
                    // ask here
                    this.EndBattle(false, true);
                    return;
                }

                // display skill icon display
                if (result.SkillUsed != null)
                {
                    _skillDisplayIcons.SetIndexed("modulate:a", 1);
                    ChangeSkillIconRegion(SkillAssets.GetIcon(result.SkillUsed.Icon));
                    _skillName.Text = result.SkillUsed.Name;
                }

                if (result.User != null)
                {
                    if(result.User.Type == EntityType.Player)
                    {
                        EntityDisplayBox userNode = (EntityDisplayBox)FindBattleEntityNode(result.User);
                        BattleEffectWrapper userEffects = new BattleEffectWrapper()
                        {
                            IsEntitySkillUser = true,
                            Result = result
                        };

                        await userNode.UpdateBattleEffects(userEffects);
                    }
                    else
                    {
                        string startupAnimationString = result.SkillUsed.StartupAnimation;
                        bool isHPGainedFromMove = (result.ResultType == BattleResultType.HPGain || result.ResultType == BattleResultType.Dr);
                        if (!string.IsNullOrEmpty(startupAnimationString))
                        {
                            _effect.Visible = true;
                            _effect.PlayAnimation(startupAnimationString);
                            await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

                            _effect.Visible = false;
                        }
                    }

                }

                if (result.Targets.Count > 0)
                {
                    Task[] tasks = new Task[result.Targets.Count];
                    List<Node> targetNodes = new List<Node>();
                    foreach (var target in result.Targets)
                        targetNodes.Add(FindBattleEntityNode(target));

                    for (int t = 0; t < result.Targets.Count; t++)
                    {
                        if (result.Targets[t].Type == EntityType.Enemy)
                        {
                            Func<Task> task = async () =>
                            {
                                string endupAnimationString = result.SkillUsed.EndupAnimation;
                                bool isHPGainedFromMove = (result.ResultType == BattleResultType.HPGain || result.ResultType == BattleResultType.Dr);
                                if (!string.IsNullOrEmpty(endupAnimationString))
                                {
                                    _effect.Visible = true;
                                    _effect.PlayAnimation(endupAnimationString);
                                    await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

                                    _effect.Visible = false;
                                }

                                if ((int)(result.ResultType) < (int)BattleResultType.StatusApplied)
                                {
                                    // play damage sfx
                                    if (!isHPGainedFromMove && result.HPChanged > 0)
                                    {
                                        _camera.Shake();
                                    }

                                    // play damage number
                                    var dmgNumber = _damageNumScene.Instantiate<DamageNumber>();
                                    dmgNumber.SetDisplayInfo(result.HPChanged, isHPGainedFromMove, result.GetResultString());
                                    _dmgMarker.CallDeferred("add_child", dmgNumber);
                                }
                            };

                            tasks[t] = task.Invoke();
                        }
                        else
                        {
                            int index = t;
                            EntityDisplayBox targetNode = (EntityDisplayBox)targetNodes[index];
                            BattleResult subResult = new BattleResult();

                            subResult.ResultType = result.Results[index];
                            if (result.AllHPChanged.Count > 0)
                            {
                                subResult.HPChanged = result.AllHPChanged[index];
                            }
                            subResult.SkillUsed = result.SkillUsed;

                            BattleEffectWrapper targetNodeEffects = new BattleEffectWrapper() { Result = subResult };

                            tasks[t] = targetNode.UpdateBattleEffects(targetNodeEffects);
                        }
                        await Task.Delay(200);
                    }

                    await Task.WhenAll(tasks);
                }
                else if (result.Target != null)
                {
                    if(result.Target.Type == EntityType.Enemy)
                    {
                        string endupAnimationString = result.SkillUsed.EndupAnimation;
                        bool isHPGainedFromMove = (result.ResultType == BattleResultType.HPGain || result.ResultType == BattleResultType.Dr);
                        if (!string.IsNullOrEmpty(endupAnimationString))
                        {
                            _effect.Visible = true;
                            _effect.Call("PlayAnimation", endupAnimationString);
                            await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

                            _effect.Visible = false;
                        }

                        if ((int)(result.ResultType) < (int)BattleResultType.StatusApplied)
                        {
                            // play damage sfx
                            if (!isHPGainedFromMove && result.HPChanged > 0)
                            {
                                _camera.Shake();
                            }

                            // play damage number
                            var dmgNumber = _damageNumScene.Instantiate<DamageNumber>();
                            dmgNumber.SetDisplayInfo(result.HPChanged, isHPGainedFromMove, result.GetResultString());
                            _dmgMarker.CallDeferred("add_child", dmgNumber);
                        }
                    }
                    else
                    {
                        EntityDisplayBox targetNode = (EntityDisplayBox)FindBattleEntityNode(result.Target);
                        BattleEffectWrapper targetNodeEffects = new BattleEffectWrapper() { Result = result };

                        await targetNode.UpdateBattleEffects(targetNodeEffects);
                    }
                }

                // slight delay so the skill icon doesn't auto vanish
                await Task.Delay(350);
                ResetSkillIcon();

                _battleSceneObject.ChangeActiveEntity();
            }

            _hud.UpdateValues();

            UpdatePlayerDisplay(_battleSceneObject.Players);

            // check if win conditions were met
            if (_battleSceneObject.DidPartyMembersWin())
            {
                this.EndBattle(true);
                return;
            }

            if (_battleSceneObject.DidEnemiesWin())
            {
                this.EndBattle(false);
                return;
            }

            _actionMenu.LoadActiveSkillList();

            bool playerTurn = _battleSceneObject.TurnState == TurnState.PLAYER;
            SetNewTurns(playerTurn);

            _actionMenu.Visible = playerTurn;

            if (_battleSceneObject.PressTurn.TurnEnded)
            {
                _battleSceneObject.PressTurn.TurnEnded = false; // set turns
                _battleSceneObject.ChangeTurnState(); // change turn state
                UpdateTurnsUsingTurnState(_battleSceneObject.TurnState);
            }
            
            // after we fully display an animation and process a skill
            // then we want to use an enemy skill
            if (_battleSceneObject.TurnState == TurnState.ENEMY)
                _battleSceneObject.DoEnemyMove();
            else
                _actionMenu.CanInput = true;
        }

        private async void EndBattle(bool didPlayerWin, bool retreated = false)
        {
            _actionMenu.CanInput = false;
            SetEndScreenVisibility(true);
            _endBox.Visible = false;
            _endScreenOptions.Visible = false;
            _endScreenOptions.CanInput = false;
            Label endLabel = GetNode<Label>("%EndOfBattleLabel");
            var gameObject = PersistentGameObjects.GameObjectInstance();

            // heal everyone
            foreach (var member in _battleSceneObject.Players)
            {
                member.DefenseModifier = 0;
                for (int i = 0; i < member.ElementDamageModifiers.Length; i++)
                {
                    member.ElementDamageModifiers[i] = 0;
                }
                member.IsActiveEntity = false;
                member.StatusHandler.Clear();
            }

            List<EndScreenItem> options = new List<EndScreenItem>();

            if (didPlayerWin)
            {
                if (_entityName == EnemyNames.Drakalla)
                {
                    GetNode<AudioStreamPlayer>("%MusicPlayer").Stop();

                    PackedScene endingScene = ResourceLoader.Load<PackedScene>(Scenes.ENDING_B1);
                    var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
                    AddChild(transition);
                    transition.PlayFadeIn();
                    await ToSignal(transition.Player, "animation_finished");
                    _camera.Zoom = new Vector2(1, 1);
                    this.GetTree().Root.AddChild(endingScene.Instantiate());
                    transition.PlayFadeOut();
                    await ToSignal(transition.Player, "animation_finished");
                    try { transition.QueueFree(); } catch (Exception) { }
                    QueueFree();
                    return;
                }

                ChangeEndScreenVisibilityOnly(false);

                endLabel.Text = "Ascended God Vanquished!";

                gameObject.MusicPlayer.PlayMusic(MusicAssets.BOSS_VICTORY);
                gameObject.MusicPlayer.ResetAllTracksAfterBoss();

                if(_entityName == EnemyNames.Nettala)
                {
                    if (!gameObject.ProgressFlagObject.FinalBossDefeated)
                        gameObject.ProgressFlagObject.FinalBossDefeated = true;
                }

                if (!gameObject.ImportantFights.Contains(_entityName))
                {
                    gameObject.ImportantFights.Add(_entityName);
                    GetNode<AudioStreamPlayer>("%ItemSfxPlayer").Play();
                    var rewardScene = _rewardScene.Instantiate<RewardScreen>();
                    this.GetTree().Root.AddChild(rewardScene);

                    if(_entityName != EnemyNames.Draco && _entityName != EnemyNames.Drakalla)
                        gameObject.MaxTier++;

                    ChangeEndScreenVisibilityOnly(false);
                    if (_entityName == EnemyNames.Nettala || _entityName == EnemyNames.Draco)
                        rewardScene.InitializeSpecialEnemyRewards(_entityName);
                    else
                        rewardScene.InitializeDungeonCrawlEncounterSpecialBossRewards();

                    await ToSignal(rewardScene, "tree_exited");
                }

                ChangeEndScreenVisibilityOnly(true);

                AddBackToHomeButton(options);
            }
            else if (retreated)
            {
                endLabel.Text = "Retreated from battle.";

                AddBasicDungeonOptions(options);
            }
            else
            {
                endLabel.Text = "Your party died.";
                AddBasicDungeonOptions(options);
            }

            PersistentGameObjects.Save();

            var progressFlags = gameObject.ProgressFlagObject;
            if (didPlayerWin)
            {
                if(_entityName == EnemyNames.Nettala && !progressFlags.PostFinalBossCutsceneWatched)
                {
                    progressFlags.PostFinalBossCutsceneWatched = true;
                    PersistentGameObjects.Save();

                    PackedScene endingScene = ResourceLoader.Load<PackedScene>(Scenes.ENDING_A1);
                    var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
                    AddChild(transition);
                    transition.PlayFadeIn();
                    await ToSignal(transition.Player, "animation_finished");
                    _camera.Zoom = new Vector2(1, 1);
                    this.GetTree().Root.AddChild(endingScene.Instantiate());
                    transition.PlayFadeOut();
                    await ToSignal(transition.Player, "animation_finished");
                    try { transition.QueueFree(); } catch (Exception) { }
                    QueueFree();
                }
                else
                {
                    _endBox.Visible = true;
                    _endScreenOptions.SetItems(options);
                    _endScreenOptions.CanInput = true;
                    _endScreenOptions.Visible = true;
                }
            }
            else
            {
                _endBox.Visible = true;
                _endScreenOptions.SetItems(options);
                _endScreenOptions.CanInput = true;
                _endScreenOptions.Visible = true;
            }
        }

        private void AddBasicDungeonOptions(List<EndScreenItem> options)
        {
            var partyChangeItem = new EndScreenItem() { ItemText = "Party" };
            var retryItem = new EndScreenItem() { ItemText = "Retry" };

            partyChangeItem.ItemSelected += _OnChangePartyBtnPressed;
            retryItem.ItemSelected += _OnRetryFloorBtnPressed;

            options.Add(partyChangeItem);
            options.Add(retryItem);
            AddBackToHomeButton(options);
        }

        private void AddBackToHomeButton(List<EndScreenItem> options)
        {
            var backToHomeItem = new EndScreenItem() { ItemText = "Leave" };
            backToHomeItem.ItemSelected += _OnBackToHomeBtnPressed;
            options.Add(backToHomeItem);
        }

        private void _OnRetryFloorBtnPressed(object sender, EventArgs e)
        {
            _camera.Enabled = true;
            _endScreenOptions.CanInput = false;
            SetEndScreenVisibility(false);
            InitializeBattleScene(_entityName, false, _isLabrybuce);
        }

        private void _OnChangePartyBtnPressed(object sender, EventArgs e)
        {
            _endScreenOptions.CanInput = false;

            var vbox = this.GetNode<VBoxContainer>("%EndVBox");
            vbox.Visible = false;

            var partyChangeScene = _partyScreen.Instantiate<PartyEditScreen>();

            _endBox.AddChild(partyChangeScene);

            partyChangeScene.DisableEmbarkButton();

            partyChangeScene.DoEmbark += (sender, args) =>
            {
                partyChangeScene.QueueFree();
                vbox.Visible = true;
                _endScreenOptions.CanInput = true;
            };
        }

        private async void _OnBackToHomeBtnPressed(object sender, EventArgs e)
        {
            _endScreenOptions.CanInput = false;
            _endBox.Visible = false;
            PackedScene mainScreenScene = ResourceLoader.Load<PackedScene>(Scenes.MAIN);
            var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
            AddChild(transition);
            transition.PlayFadeIn();
            _camera.Enabled = false;
            await ToSignal(transition.Player, "animation_finished");
            GetTree().Root.AddChild(mainScreenScene.Instantiate());
            transition.PlayFadeOut();
            await ToSignal(transition.Player, "animation_finished");
            try { transition.QueueFree(); } catch (Exception) { }
            QueueFree();
        }

        /// <summary>
        /// We use this function multiple times.
        /// 1. After a move to show who the next player is.
        /// 2. At the end of a turn to remove the current player icon.
        /// </summary>
        private void UpdatePlayerDisplay(List<BattlePlayer> players)
        {
            for (int j = 0; j < players.Count; j++)
            {
                if (players[j].HP == 0)
                    players[j].StatusHandler.Clear();

                var vBoxContainer = _partyMembers.GetChild(j);
                var partyDisplay = (EntityDisplayBox)vBoxContainer.GetChild(0);

                var playerWrapper = new EntityWrapper() { BattleEntity = players[j] };
                partyDisplay.UpdateEntityDisplay(playerWrapper);

                if (players[j].IsActiveEntity)
                {
                    _actionMenu.Reparent(vBoxContainer);
                    _actionMenu.EmptyClick = false;
                }
            }
        }

        private Node FindBattleEntityNode(BattleEntity entity)
        {
            Node nodeToFind;

            if (entity.Type == EntityType.Player)
            {
                int pIndex = _battleSceneObject.Players.IndexOf((BattlePlayer)entity);
                var child = _partyMembers.GetChild(pIndex);
                nodeToFind = child.GetChild(0);
            }
            else
            {
                nodeToFind = _enemySprite;
            }

            return nodeToFind;
        }

        #region Skill Icon Updates
        private void ResetSkillIcon()
        {
            _skillDisplayIcons.SetIndexed("modulate:a", 0);
            _skillName.Text = String.Empty;
            ChangeSkillIconRegion(new KeyValuePair<int, int>(0, 0));
        }

        private void ChangeSkillIconRegion(KeyValuePair<int, int> coords)
        {
            AtlasTexture atlas = _skillIcon.Texture as AtlasTexture;
            atlas.Region = new Rect2(coords.Key, coords.Value, 32, 32);
        }
        #endregion

        #region Turn Functions
        protected void UpdateTurnsUsingTurnState(TurnState turnState)
        {
            if (turnState == TurnState.PLAYER)
            {
                _battleSceneObject.SetPartyMemberTurns();
                SetNewTurns(true);

                _actionMenu.LoadActiveSkillList();
            }
            else
            {
                _battleSceneObject.SetupEnemyTurns();
                SetNewTurns(false);
            }

            // change our active player display
            // if enemy, no active players
            // if player, 1 active player
            UpdatePlayerDisplay(_battleSceneObject.Players);
        }

        private void SetNewTurns(bool isPlayer)
        {
            // clear all icons to redraw them
            var children = _turnIconContainer.GetChildren();
            foreach (var child in children)
                _turnIconContainer.RemoveChild(child);

            List<int> turns = _battleSceneObject.PressTurn.TurnIcons;
            foreach (int turn in turns)
            {
                var turnIconScene = _turnIcons.Instantiate();
                _turnIconContainer.AddChild(turnIconScene);
                turnIconScene.Call("SetIconState", isPlayer, turn == 1);
            }
        }
        #endregion

        #region End Screen Functions
        private void SetEndScreenVisibility(bool visible)
        {
            ChangeEndScreenVisibilityOnly(visible);

            _playerUI.Visible = !visible;
            _enemyUI.Visible = !visible;
            GetNode<VBoxContainer>("%PlayerVBoxContainer").Visible = !visible;
            _skillDisplayIcons.SetIndexed("modulate:a", 0);
            _enemySprite.Visible = !visible;
        }

        private void ChangeEndScreenVisibilityOnly(bool visible)
        {
            _endBox.Visible = visible;
            _endScreenOptions.Visible = visible;
            _endScreenOptions.EmptyClick = false;
        }
        #endregion
    }
}
