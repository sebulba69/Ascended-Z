using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

public partial class MainScreen : Transitionable2DScene
{
    private CenterContainer _root;
    private VBoxContainer _mainUIContainer;
    private Label _tooltip;
    private AudioStreamPlayer _audioPlayer;
    private HBoxContainer _topRightContainer;
    private bool _checkBoxPressed;
    MainPlayerContainer _mainPlayerContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _root = this.GetNode<CenterContainer>("CenterContainer");
        _mainUIContainer = this.GetNode<VBoxContainer>("%MainContainer");
        _tooltip = this.GetNode<Label>("%Tooltip");
        _audioPlayer = this.GetNode<AudioStreamPlayer>("%MusicPlayer");
        _topRightContainer = this.GetNode<HBoxContainer>("%TopRightContainer");

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
        TextureRect background = this.GetNode<TextureRect>("%Background");
        background.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetBackground(gameObject.MaxTier, gameObject.ProgressFlagObject.EndgameUnlocked));
        _mainPlayerContainer = this.GetNode<MainPlayerContainer>("%MainPlayerContainer");

        if (!gameObject.Mail.HasMail(MailboxAssets.PRESS_TURN_TITLE))
        {
            gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.CONTROL_TITLE]);
            gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.PRESS_TURN_TITLE]);
            gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.PRESS_TURN_TITLE_2]);
            PersistentGameObjects.Save();
        }

        InitializeMusicButton(gameObject);
        InitializePlayerInformation(gameObject);
        InitializeButtons(gameObject);
    }

    #region Setup functions
    private void InitializeMusicButton(GameObject gameObject)
    {
        OptionButton musicOptionsButton = this.GetNode<OptionButton>("%MusicOptionsButton");
        CheckBox checkBox = this.GetNode<CheckBox>("%CheckBox");

        MusicObject musicPlayer = gameObject.MusicPlayer;
        List<string> overworldTracks = MusicAssets.GetOverworldTracks(gameObject.MaxTier, gameObject.ProgressFlagObject.EndgameUnlocked);

        musicPlayer.SetStreamPlayer(_audioPlayer);

        int indexOfSongToDisplay = 0;
        for (int i = 0; i < overworldTracks.Count; i++)
        {
            string track = overworldTracks[i];
            if (track.Equals(gameObject.MusicPlayer.OverworldThemeCustom))
                indexOfSongToDisplay = i;

            track = track.Replace(".ogg", "");
            track = track.Substring(MusicAssets.OW_MUSIC_FOLDER.Length);
            musicOptionsButton.AddItem(track);
        }

        musicOptionsButton.Select(indexOfSongToDisplay);

        musicOptionsButton.ItemSelected += (long index) =>
        {
            musicPlayer.OverworldThemeCustom = overworldTracks[(int)index];
            PersistentGameObjects.Save();

            musicPlayer.PlayMusic(musicPlayer.OverworldThemeCustom);
        };

        checkBox.Toggled += (bool state) => 
        {
            musicPlayer.ResetAllTracksAfterBoss();
            musicPlayer.IsMusicCustom = state;
            SwapOverworldTracks(musicPlayer);
        };

        SwapOverworldTracks(musicPlayer);
    }

    private void InitializePlayerInformation(GameObject gameObject)
    {
        _mainPlayerContainer.InitializePlayerInformation(gameObject);
        DoEmbarkButtonCheck(gameObject);
    }

    private void DoEmbarkButtonCheck(GameObject gameObject)
    {
        var embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button upgradeButton = this.GetNode<Button>("%UpgradePartyButton");

        if (gameObject.MainPlayer.ReserveMembers.Count > 0)
        {
            embarkButton.Visible = true;
            upgradeButton.Visible = true;
        }    
    }

    private void SwapOverworldTracks(MusicObject musicPlayer)
    {
        OptionButton musicOptionsButton = this.GetNode<OptionButton>("%MusicOptionsButton");
        CheckBox checkBox = this.GetNode<CheckBox>("%CheckBox");

        string track;
        if (musicPlayer.IsMusicCustom)
        {
            musicOptionsButton.Visible = true;
            checkBox.Text = "Normal";
            checkBox.ButtonPressed = true;
            track = musicPlayer.OverworldThemeCustom;
        }
        else
        {
            musicOptionsButton.Visible = false;
            checkBox.Text = "Custom";
            checkBox.ButtonPressed = false;
            track = musicPlayer.OverworldTheme;
        }

        musicPlayer.PlayMusic(track);
    }

    private void InitializeButtons(GameObject gameObject)
    {
        int tier = gameObject.MaxTier;

        Button menuButton = this.GetNode<Button>("%MenuButton");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button recruitButton = this.GetNode<Button>("%RecruitButton");
        Button upgradeButton = this.GetNode<Button>("%UpgradePartyButton");
        Button fuseButton = this.GetNode<Button>("%FuseButton");
        Button changeRoomButton = this.GetNode<Button>("%ChangeRoomButton");
        Button skillTransferButton = this.GetNode<Button>("%SkillTransferButton");
        Button mailBoxButton = this.GetNode<Button>("%MailboxButton");
        Button trueAscensionButton = this.GetNode<Button>("%TrueAscensionButton");

        Button bountyKey = GetNode<Button>("%BountyKeyButton");
        Button elderKey = GetNode<Button>("%ElderButton");

        DoRecruitButtonTextCheck();

        bool isRecruitCustomTier = tier > TierRequirements.TIER2_STRONGER_ENEMIES;
        if (isRecruitCustomTier && !gameObject.Mail.HasMail(MailboxAssets.RECRUITING))
        {
            gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.RECRUITING]);
            PersistentGameObjects.Save();
        }

        var progressFlagObject = gameObject.ProgressFlagObject;

        if (tier > TierRequirements.FUSE)
        {
            fuseButton.Visible = true;
            skillTransferButton.Visible = true;

            if (!gameObject.Mail.HasMail(MailboxAssets.FUSIONS))
            {
                gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.FUSIONS]);
                gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.FUSIONS_CONT]);
                gameObject.Mail.Mail.Add(MailboxAssets.GameMail[MailboxAssets.REC_FUSIONS]);
                PersistentGameObjects.Save();
            }
        }

        if (gameObject.MainPlayer.ReserveMembers.Count > 0)
        {
            upgradeButton.Visible = true;
        };

        if (gameObject.ProgressFlagObject.EndgameUnlocked)
        {
            GetNode<VBoxContainer>("%BountyKeyOption").Visible = true;
            GetNode<VBoxContainer>("%ElderKeyOption").Visible = true;
        }

        var wallet = gameObject.MainPlayer.Wallet.Currency;
        if(wallet.ContainsKey(SkillAssets.PROOF_OF_ASCENSION_ICON) && wallet.ContainsKey(SkillAssets.PROOF_OF_BUCE_ICON))
        {
            trueAscensionButton.Visible = true;
        }

        menuButton.Pressed += _OnMenuButtonPressed;
        embarkButton.Pressed += _OnEmbarkButtonPressed;
        recruitButton.Pressed += _OnRecruitButtonPressed;
        upgradeButton.Pressed += _OnUpgradeButtonPressed;
        fuseButton.Pressed += _OnFuseButtonPressed;
        changeRoomButton.Pressed += _OnChangeRoomPressed;
        skillTransferButton.Pressed += _OnSkillTransferPressed;
        mailBoxButton.Pressed += _OnMailboxPressed;
        trueAscensionButton.Pressed += _OnTrueAscensionButtonPressed;

        if(gameObject.TierDCCap == 301)
        {
            bountyKey.Text = "[LAB. MAXED]";
        }

        if(gameObject.TierCap == 301)
        {
            elderKey.Text = "[D. MAXED]";
        }

        bountyKey.Pressed += () => 
        {
            int keyCost = 10;
            var currency = gameObject.MainPlayer.Wallet.Currency;
            if (currency.ContainsKey(SkillAssets.BOUNTY_KEY))
            {
                var keys = currency[SkillAssets.BOUNTY_KEY];
                if (gameObject.TierDCCap + 10 <= 301 && keys.Amount - keyCost >= 0)
                {
                    keys.Amount -= keyCost;
                    gameObject.TierDCCap += 10;
                    InitializePlayerInformation(gameObject);

                    if (gameObject.TierDCCap == 301)
                    {
                        bountyKey.Text = "[LAB. MAXED]";
                    }

                    GetNode<AudioStreamPlayer>("%JingleyJingle").Play();
                    PersistentGameObjects.Save();
                }
                else
                {
                    GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
                }
            }
            else
            {
                GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
            }
        };

        elderKey.Pressed += () =>
        {
            var currency = gameObject.MainPlayer.Wallet.Currency;
            if (currency.ContainsKey(SkillAssets.ELDER_KEY_ICON))
            {
                var keys = currency[SkillAssets.ELDER_KEY_ICON];
                if (gameObject.TierCap + 10 <= 301 && keys.Amount - 1 >= 0)
                {
                    keys.Amount -= 1;
                    gameObject.TierCap += 10;
                    InitializePlayerInformation(gameObject);

                    if (gameObject.TierCap == 301)
                    {
                        elderKey.Text = "[D. MAXED]";
                    }

                    GetNode<AudioStreamPlayer>("%JingleyJingle").Play();

                    PersistentGameObjects.Save();
                }
                else
                {
                    GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
                }
            }
            else
            {
                GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
            }
        };

        menuButton.MouseEntered += () => { _tooltip.Text = "Save your game or quit to Title."; };
        embarkButton.MouseEntered += () => { _tooltip.Text = "Enter the Endless Dungeon with your party."; };
        recruitButton.MouseEntered += () => { _tooltip.Text = "Recruit Party Members to be used in battle."; };
        upgradeButton.MouseEntered += () => { _tooltip.Text = "Upgrade Party Members with Vorpex."; };
        fuseButton.MouseEntered += () => { _tooltip.Text = "Combine Party Members to create new ones and transfer skills."; };
        changeRoomButton.MouseEntered += () => { _tooltip.Text = "Change your Ascended's look!"; };
        skillTransferButton.MouseEntered += () => { _tooltip.Text = "Transfer skills between your Ascendeds."; };
        mailBoxButton.MouseEntered += () => { _tooltip.Text = "View tutorials on how to ASCEND!!!"; };
        trueAscensionButton.MouseEntered += () => { _tooltip.Text = "With your newfound power, you can transcend this mortal realm"; };
        
        if (gameObject.Mail.HasMailLeftUnread())
        {
            mailBoxButton.Text = "❗ Mailbox ❗";
        }
        else
        {
            mailBoxButton.Text = "Mailbox";
        }
    }
    #endregion
    
    private async void _OnTrueAscensionButtonPressed()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        var wallet = gameObject.MainPlayer.Wallet.Currency;
        wallet.Remove(SkillAssets.PROOF_OF_ASCENSION_ICON);
        wallet.Remove(SkillAssets.PROOF_OF_BUCE_ICON);
        wallet.Add(SkillAssets.ELDER_KEY_ICON, new ElderKey { Amount = 0 });
        gameObject.ProgressFlagObject.EndgameUnlocked = true;
        gameObject.SigilLevelCap = 300;
        PersistentGameObjects.Save();

        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        var oneHundredPercentCutscene = ResourceLoader.Load<PackedScene>(Scenes.ONE_HUNDRED_PERCENT_CUTSCENE).Instantiate();
        GetTree().Root.AddChild(transition);
        transition.PlayFadeIn();
        await ToSignal(transition.Player, "animation_finished");

        GetTree().Root.AddChild(oneHundredPercentCutscene);
        GetTree().Root.RemoveChild(this);
        QueueFree();

        CloseTransition(transition);
    }

    private void DoRecruitButtonTextCheck()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        int tier = gameObject.MaxTier;
        Button recruitButton = this.GetNode<Button>("%RecruitButton");
        var progressTiers = gameObject.ProgressFlagObject.SKILL_PROGRESS_TIERS;

        recruitButton.Text = "Recruit";

        foreach (int pT in progressTiers)
        {
            if (tier > pT && !gameObject.ProgressFlagObject.ViewedSkillProgressTiers.Contains(pT))
            {
                recruitButton.Text = "❗ Recruit ❗";
                break;
            }
        }
    }

    private void _OnSkillTransferPressed()
    {
        DisplayScene(Scenes.SKILL_TRANSFER);
    }

    private void _OnMailboxPressed()
    {
        DisplayScene(Scenes.MAIL_SCREEN);
    }

    private void _OnChangeRoomPressed()
    {
        DisplayScene(Scenes.MAIN_CHANGE_ROOM);
    }

    private void _OnMenuButtonPressed()
    {
        _mainUIContainer.Visible = false;
        _topRightContainer.Visible = false;
        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(Scenes.MENU).Instantiate();

        _root.AddChild(instanceOfPackedScene);
        instanceOfPackedScene.Connect("EndMenuScene", new Callable(this, "_OnMenuClosed"));
    }

    private async void _OnEmbarkButtonPressed()
    {
        _mainUIContainer.Visible = false;
        _topRightContainer.Visible = false;

        var embark = ResourceLoader.Load<PackedScene>(Scenes.MAIN_EMBARK).Instantiate<EmbarkScreen>();
        GetTree().Root.AddChild(embark);
        await ToSignal(embark, "CloseEmbarkScreen");

        if (embark.Embark)
        {
            var gameObject = PersistentGameObjects.GameObjectInstance();
            var isEndgame = gameObject.ProgressFlagObject.EndgameUnlocked;

            if (embark.DungeonCrawling && gameObject.TierDC == gameObject.TierDCCap && !isEndgame)
            {
                var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
                GetTree().Root.AddChild(transition);
                transition.PlayFadeIn();
                await ToSignal(transition.Player, "animation_finished");
                var cutsceneObject = gameObject.CutsceneObject;

                if (CutsceneAssets.StartTierCutscenesSpecialLab.ContainsKey(gameObject.TierDC) && !cutsceneObject.SpecialBossCutscenesWatchedLabrybuce.Contains(gameObject.TierDC))
                {
                    cutsceneObject.SpecialBossCutscenesWatchedLabrybuce.Add(gameObject.TierDC);
                    var cutscene = ResourceLoader.Load<PackedScene>(CutsceneAssets.StartTierCutscenesSpecialLab[gameObject.TierDC]);
                    this.GetTree().Root.AddChild(cutscene.Instantiate());
                    PersistentGameObjects.Save();
                }
                else
                {
                    var battleScene = ResourceLoader.Load<PackedScene>(Scenes.FINAL_BOSS_LAB).Instantiate<DracoBattleScene>();
                    this.GetTree().Root.AddChild(battleScene);
                    await Task.Delay(150);
                }
                Visible = false;
                embark.QueueFree();
                GetTree().Root.RemoveChild(this);
                QueueFree();
                CloseTransition(transition);
            }
            else if (!embark.DungeonCrawling
                && gameObject.Tier == gameObject.TierCap && !isEndgame)
            {
                var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
                GetTree().Root.AddChild(transition);
                transition.PlayFadeIn();
                await ToSignal(transition.Player, "animation_finished");
                var cutsceneObject = gameObject.CutsceneObject;

                if (CutsceneAssets.StartTierCutscenesSpecial.ContainsKey(gameObject.Tier) && !cutsceneObject.SpecialBossCutscenesWatchedEndless.Contains(gameObject.Tier))
                {
                    cutsceneObject.SpecialBossCutscenesWatchedEndless.Add(gameObject.Tier);
                    var cutscene = ResourceLoader.Load<PackedScene>(CutsceneAssets.StartTierCutscenesSpecial[gameObject.Tier]);
                    this.GetTree().Root.AddChild(cutscene.Instantiate());
                    PersistentGameObjects.Save();
                }
                else
                {
                    var battleScene = ResourceLoader.Load<PackedScene>(Scenes.FINAL_BOSS).Instantiate<NetallaBattleScene>();
                    this.GetTree().Root.AddChild(battleScene);
                    await Task.Delay(150);
                }

                Visible = false;
                embark.QueueFree();
                GetTree().Root.RemoveChild(this);
                QueueFree();
                CloseTransition(transition);
            }
            else if (gameObject.EndAscended)
            {
                var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
                GetTree().Root.AddChild(transition);
                transition.PlayFadeIn();
                await ToSignal(transition.Player, "animation_finished");
                gameObject.Tier = 301;
                if (!gameObject.ProgressFlagObject.FinalCutsceneWatched)
                {
                    gameObject.ProgressFlagObject.FinalCutsceneWatched = true;
                    var cutscene = ResourceLoader.Load<PackedScene>(Scenes.ONE_HUNDRED_PERCENT_BOSS);
                    this.GetTree().Root.AddChild(cutscene.Instantiate());
                    PersistentGameObjects.Save();
                }
                else
                {
                    var battleScene = ResourceLoader.Load<PackedScene>(Scenes.FINAL_BOSS_100_PERCENT).Instantiate();
                    this.GetTree().Root.AddChild(battleScene);
                    await Task.Delay(150);
                }
                Visible = false;
                embark.QueueFree();
                GetTree().Root.RemoveChild(this);
                QueueFree();
                CloseTransition(transition);
            }
            else
            {
                DoNormalEmbark(embark, gameObject);
            }
        }
        else
        {
            embark.QueueFree();

            _mainUIContainer.Visible = true;
            _topRightContainer.Visible = true;

            var pFlags = PersistentGameObjects.GameObjectInstance().ProgressFlagObject;

            if (pFlags.CustomPartyMembersViewed)
                this.GetNode<Button>("%RecruitButton").Text = "Recruit";

            _mainPlayerContainer.UpdateCurrencyDisplay();
            DoEmbarkButtonCheck(PersistentGameObjects.GameObjectInstance());
        }
    }

    private async void DoNormalEmbark(EmbarkScreen embark, GameObject gameObject)
    {
        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        GetTree().Root.AddChild(transition);
        transition.PlayFadeIn();
        await ToSignal(transition.Player, "animation_finished");
        var cutsceneObject = gameObject.CutsceneObject;

        if (embark.DungeonCrawling)
        {
            if (CutsceneAssets.StartLabrybuceFloor.ContainsKey(gameObject.TierDC)
                && !cutsceneObject.StartOfLabrybuceWatched.Contains(gameObject.TierDC))
            {
                cutsceneObject.StartOfLabrybuceWatched.Add(gameObject.TierDC);
                var cutscene = ResourceLoader.Load<PackedScene>(CutsceneAssets.StartLabrybuceFloor[gameObject.TierDC]);
                this.GetTree().Root.AddChild(cutscene.Instantiate());
                PersistentGameObjects.Save();
            }
            else
            {
                var dungeon = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_CRAWL).Instantiate<DungeonScreen>();
                GetTree().Root.AddChild(dungeon);
                await Task.Delay(10);
                dungeon.StartDungeon();
            }
        }
        else
        {
            if (CutsceneAssets.StartTierCutscenes.ContainsKey(gameObject.Tier) && !cutsceneObject.StartOfTierCutscenesWatched.Contains(gameObject.Tier))
            {
                cutsceneObject.StartOfTierCutscenesWatched.Add(gameObject.Tier);
                var cutscene = ResourceLoader.Load<PackedScene>(CutsceneAssets.StartTierCutscenes[gameObject.Tier]);
                this.GetTree().Root.AddChild(cutscene.Instantiate());
                PersistentGameObjects.Save();
            }
            else
            {
                if(gameObject.Tier > 250 && gameObject.Tier % 10 == 0)
                {
                    int tier = gameObject.Tier - 250;
                    tier /= 10;
                    tier--;
                    var elderScene = ResourceLoader.Load<PackedScene>(Scenes.ELDER_BATTLES[tier]).Instantiate();
                    this.GetTree().Root.AddChild(elderScene);
                }
                else
                {
                    var battleScene = ResourceLoader.Load<PackedScene>(Scenes.BATTLE_SCENE).Instantiate<BattleEnemyScene>();
                    this.GetTree().Root.AddChild(battleScene);
                    await Task.Delay(150);
                    battleScene.SetupForNormalEncounter();
                }

            }
        }

        Visible = false;
        embark.QueueFree();
        GetTree().Root.RemoveChild(this);
        QueueFree();
        CloseTransition(transition);
    }

    private async void CloseTransition(SceneTransition transition)
    {
        transition.PlayFadeOut();
        await ToSignal(transition.Player, "animation_finished");
        try { transition.QueueFree(); } catch (Exception) { }
    }

    private void _OnRecruitButtonPressed()
    {
        DisplayScene(Scenes.MAIN_RECRUIT);
    }

    private void _OnUpgradeButtonPressed()
    {
        DisplayScene(Scenes.UPGRADE);
    }

    private void _OnFuseButtonPressed()
    {
        DisplayScene(Scenes.FUSION);
    }

    private async void _OnMenuClosed(bool quitToStart)
    {
        if (quitToStart)
        {
            var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();

            this.AddChild(transition);

            transition.PlayFadeIn();
            await ToSignal(transition.Player, "animation_finished");
            _audioPlayer.Stop();
            this.GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(Scenes.START).Instantiate());

            transition.PlayFadeOut();
            await ToSignal(transition.Player, "animation_finished");

            try { transition.QueueFree(); } catch (Exception) { }

            QueueFree();
        }
        else
        {
            _mainUIContainer.Visible = true;
            _topRightContainer.Visible = true;
        }
    }

    private async void DisplayScene(string packedScenePath)
    {
        _mainUIContainer.Visible = false;
        _topRightContainer.Visible = false;
        Button mailBoxButton = this.GetNode<Button>("%MailboxButton");
        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(packedScenePath).Instantiate();
        _root.AddChild(instanceOfPackedScene);

        await ToSignal(instanceOfPackedScene, "tree_exited");

        _mainUIContainer.Visible = true;
        _topRightContainer.Visible = true;

        var go = PersistentGameObjects.GameObjectInstance();
        var pFlags = go.ProgressFlagObject;

        DoRecruitButtonTextCheck();

        if (go.Mail.HasMailLeftUnread())
        {
            mailBoxButton.Text = "❗ Mailbox ❗";
        }
        else
        {
            mailBoxButton.Text = "Mailbox";
        }

        _mainPlayerContainer.UpdatePlayerPic(go.MainPlayer.Image);
        _mainPlayerContainer.UpdateCurrencyDisplay();
        DoEmbarkButtonCheck(PersistentGameObjects.GameObjectInstance());
    }
}
