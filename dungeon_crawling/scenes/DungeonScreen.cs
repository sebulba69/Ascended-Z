using AscendedZ;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.dungeon_crawling.backend.dungeon_items;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.end_screen;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UITile
{
    public int X { get; set; }
    public int Y { get; set; }
    public TileScene Scene { get; set; }
}

public partial class DungeonScreen : Transitionable2DScene
{
    private readonly PackedScene _uiTileScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_TILE_SCENE);
    private readonly PackedScene _minerScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_MINER);
    private readonly PackedScene _dungeonDialogScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_DIALOG_SCENE);
    private readonly PackedScene _yesNoPopup = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP);
    private readonly PackedScene _battleScene = ResourceLoader.Load<PackedScene>(Scenes.BATTLE_SCENE);
    private readonly PackedScene _transitionScene = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION);
    private readonly PackedScene _fountainScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_FOUNTAIN);
    private readonly PackedScene _rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS);
    private readonly PackedScene _inventoryScene = ResourceLoader.Load<PackedScene>(Scenes.INVENTORY_SCENE);
    private readonly PackedScene _shopScene = ResourceLoader.Load<PackedScene>(Scenes.SHOPKEEPER_SCENE);

    private InventoryUI _inventoryUI;

    private bool _prematurelyLeave;
    private Marker2D _tiles;
    private CanvasLayer _popup;
    private TextureRect _background, _endBackground;
    private DungeonCrawlUI _crawlUI;
    private FloorExitScene _floorExitScene;
    private Camera2D _camera;
    private AudioStreamPlayer _audioStreamPlayer, _encounterSfxPlayer, _healSfxPlayer, _itemSfxPlayer;
    private Button _retreat, _inventory;
    private DungeonEntity _player;
    private UITile _currentScene;

    private bool _processingEvent;
    private bool _endingScene;
    private Dungeon _dungeon;
    private GameObject _gameObject;
    private List<BattlePlayer> _battlePlayers;

    private UITile[,] _uiTiles;
    private List<EndScreenItem> _endScreenItems;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameObject = PersistentGameObjects.GameObjectInstance();

        _tiles = this.GetNode<Marker2D>("%Tiles");
        _player = this.GetNode<DungeonEntity>("%Player");
        _camera = this.GetNode<Camera2D>("%Camera2D");
        _background = this.GetNode<TextureRect>("%Background");
        _endBackground = this.GetNode<TextureRect>("%EndBackground");
        _audioStreamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _encounterSfxPlayer = this.GetNode<AudioStreamPlayer>("%EncounterSfxPlayer");
        _healSfxPlayer = this.GetNode<AudioStreamPlayer>("%HealSfxPlayer");
        _itemSfxPlayer = this.GetNode<AudioStreamPlayer>("%ItemSfxPlayer");
        _crawlUI = this.GetNode<DungeonCrawlUI>("%DungeonCrawlUi");
        _popup = this.GetNode<CanvasLayer>("%Popups");
        _floorExitScene = this.GetNode<FloorExitScene>("%FloorExitScene");
        _endScreenItems = new List<EndScreenItem>();

        var optionsNode = _floorExitScene.EndScreenOptions;

        var continueOption = new EndScreenItem() { ItemText = "Proceed" };
        var stayOption = new EndScreenItem() { ItemText = "Stay" };
        var backOption = new EndScreenItem() { ItemText = "Retreat" };

        continueOption.ItemSelected += _OnContinueToNextFloor;
        stayOption.ItemSelected += _OnStayButtonPressed;
        backOption.ItemSelected += _OnRetreatButtonPressed;

        if(_gameObject.TierDC + 1 < _gameObject.TierDCCap)
        {
            _endScreenItems.Add(continueOption);
        }

        if(_gameObject.TierDC+1 == 301)
        {
            _endScreenItems.Remove(continueOption);
        }

        _endScreenItems.Add(stayOption);
        _endScreenItems.Add(backOption);

        _retreat = this.GetNode<Button>("%RetreatBtn");
        _inventory = this.GetNode<Button>("%InventoryBtn");
        _retreat.Pressed += () =>
        {
            _prematurelyLeave = true;
            _OnRetreatButtonPressed(null, EventArgs.Empty);
        };

        _inventory.Pressed += _OnInventoryPressed;

        _player.Up.Pressed += _OnMineUp;
        _player.Down.Pressed += _OnMineDown;
        _player.Left.Pressed += _OnMineLeft;
        _player.Right.Pressed += _OnMineRight;

        _gameObject.MusicPlayer.SetStreamPlayer(_audioStreamPlayer);
        _battlePlayers = _gameObject.MakeBattlePlayerListFromParty();
        _player.SetGraphic(_gameObject.MainPlayer.Image);

        SetEncounterVisibility(false);
    }

    private void _OnMineDown()
    {
        if (_gameObject.Pickaxes != 0)
        {
            MineDirection(_currentScene.X + 1, _currentScene.Y);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        }
    }

    private void _OnMineUp()
    {
        if (_gameObject.Pickaxes != 0)
        {
            MineDirection(_currentScene.X - 1, _currentScene.Y);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        }
    }

    private void _OnMineLeft()
    {
        if (_gameObject.Pickaxes != 0)
        {
            MineDirection(_currentScene.X, _currentScene.Y - 1);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        }
    }

    private void _OnMineRight()
    {
        if (_gameObject.Pickaxes != 0)
        {
            MineDirection(_currentScene.X, _currentScene.Y + 1);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        }
    }

    private async void _OnInventoryPressed()
    {
        _processingEvent = true;
        _crawlUI.Visible = false;
        _inventoryUI = _inventoryScene.Instantiate<InventoryUI>();

        _inventoryUI.Players = _battlePlayers;
        _inventoryUI.UseItemDScreen += _OnItemUse;

        _popup.AddChild(_inventoryUI);

        await ToSignal(_inventoryUI, "tree_exited");
        SetCrawlValues();
        _crawlUI.Visible = true;
        _processingEvent = false;
    }

    private void _OnItemUse(object sender, IDungeonItem item)
    {
        if (item != null)
        {
            if (item.Icon == SkillAssets.TP_TO_BOSS_ROOM)
            {
                if (_dungeon.Boss == null)
                    return;
            }
            _gameObject.LabrybuceInventoryObject.UseItem(item);
            _inventoryUI.QueueFree();
            item.Use(this);
        }

        SetCrawlValues();
        PersistentGameObjects.Save();
    }

    private void MineDirection(int x, int y)
    {
        GetNode<AudioStreamPlayer>("%MinePlayer").Play();
        _dungeon.Mine(x, y);
        var tiles = _dungeon.Tiles;
        DrawDoors(_uiTiles[x, y], tiles[x, y], tiles);
        _uiTiles[x, y].Scene.Visible = true;
        FillInDoors(x, y);
        _gameObject.Pickaxes--;

        SetCrawlValues();

        PersistentGameObjects.Save();
    }

    private void FillInDoors(int x, int y)
    {
        var tiles = _dungeon.Tiles;
        if (y - 1 >= 0 && _uiTiles[x, y - 1].Scene.Visible)
        {
            DrawDoors(_uiTiles[x, y - 1], tiles[x, y - 1], tiles);
        }

        if (x - 1 >= 0 && _uiTiles[x - 1, y].Scene.Visible)
        {
            DrawDoors(_uiTiles[x - 1, y], tiles[x - 1, y], tiles);
        }

        if (x + 1 < tiles.GetLength(0) && _uiTiles[x + 1, y].Scene.Visible)
        {
            DrawDoors(_uiTiles[x + 1, y], tiles[x + 1, y], tiles);
        }

        if (y + 1 < tiles.GetLength(0) && _uiTiles[x, y + 1].Scene.Visible)
        {
            DrawDoors(_uiTiles[x, y + 1], tiles[x, y + 1], tiles);
        }
    }

    private bool _mine = false;

    public override void _Input(InputEvent @event)
    {
        if (_endingScene) return;
        if (_processingEvent) return;
        if (_currentScene == null) return;

        int x = _currentScene.X;
        int y = _currentScene.Y;

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            MoveDirection(x, y + 1);
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            MoveDirection(x, y - 1);
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            MoveDirection(x + 1, y);
        }

        if (@event.IsActionPressed(Controls.UP))
        {
            MoveDirection(x - 1, y);
        }

        if (@event.IsActionPressed(Controls.RETREAT))
        {
            _prematurelyLeave = true;
            _OnRetreatButtonPressed(null, EventArgs.Empty);
        }

        if (@event.IsActionPressed(Controls.INVENTORY))
        {
            _OnInventoryPressed();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        bool mine = Input.IsActionPressed(Controls.MINE);
        if (Input.IsActionPressed(Controls.UP) && mine && _player.Up.Visible == true)
        {
            _OnMineUp();
        }

        if (Input.IsActionPressed(Controls.DOWN) && mine && _player.Down.Visible == true)
        {
            _OnMineDown();
        }

        if (Input.IsActionPressed(Controls.LEFT) && mine && _player.Left.Visible == true)
        {
            _OnMineLeft();
        }

        if (Input.IsActionPressed(Controls.RIGHT) && mine && _player.Right.Visible == true)
        {
            _OnMineRight();
        }
    }

    public void TeleportToExit()
    {
        var exit = _dungeon.Exit;
        MoveDirection(exit.X, exit.Y);
    }

    public void TeleportToBoss()
    {
        var boss = _dungeon.Boss;
        MoveDirection(boss.X, boss.Y);
    }

    private void MoveDirection(int x, int y)
    {
        if (x >= 0 && x < _uiTiles.GetLength(0) && y >= 0 && y < _uiTiles.GetLength(1))
        {
            var tile = _uiTiles[x, y];
            if (tile.Scene.Visible)
            {
                _player.SetArrows(false, false, false, false);
                _currentScene = tile;
                _processingEvent = true;
                var tween = CreateTween();
                tween.TweenProperty(_player, "position", _currentScene.Scene.Position, 0.25);
                tween.Finished += () =>
                {
                    _processingEvent = false;
                    _dungeon.MoveDirection(x, y);
                    _crawlUI.SetCoordinates(x, y);
                };

                SetPlayerDirections(x, y);
            }
        }
    }

    private void SetPlayerDirections(int x, int y)
    {
        if (_gameObject.Pickaxes > 0)
        {
            bool up = (x - 1 >= 0 && !_uiTiles[x - 1, y].Scene.Visible);
            bool right = (y + 1 < _uiTiles.GetLength(0) && !_uiTiles[x, y + 1].Scene.Visible);
            bool down = (x + 1 < _uiTiles.GetLength(0) && !_uiTiles[x + 1, y].Scene.Visible);
            bool left = (y - 1 >= 0 && !_uiTiles[x, y - 1].Scene.Visible);

            _player.SetArrows(up, down, left, right);
        }
    }

    private void SetCrawlValues()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        var tier = gameObject.TierDC;
        int morbis = 0;
        if (gameObject.MainPlayer.Wallet.Currency.ContainsKey(SkillAssets.MORBIS))
            morbis = gameObject.MainPlayer.Wallet.Currency[SkillAssets.MORBIS].Amount;

        _crawlUI.SetParty(tier, _battlePlayers, _gameObject.Orbs,
                          morbis, _gameObject.Pickaxes, _dungeon);

        var spTiles = _dungeon.SpecialTiles;

        _crawlUI.SetSpecialTileContainer(spTiles);

        _crawlUI.MineToolTipVisible(_gameObject.Pickaxes > 0);
    }

    public void StartDungeon()
    {
        _processingEvent = true;
        _background.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetCombatDCBackground(_gameObject.TierDC));
        _endBackground.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetCombatDCBackground(_gameObject.TierDC));
        _player.SetArrows(false, false, false, false);
        int tier = _gameObject.TierDC;

        _currentScene = null;
        _dungeon = new Dungeon(_gameObject.TierDC);
        _dungeon.Generate();
        _dungeon.TileEventTriggered += OnTileEventTriggeredAsync;

        foreach (var child in _tiles.GetChildren())
            _tiles.RemoveChild(child);

        var tiles = _dungeon.Tiles;
        int rows = tiles.GetLength(0);
        int columns = rows;

        _uiTiles = new UITile[rows, columns];
        var position = new Vector2(0, 0);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                _uiTiles[r, c] = MakeNewUITile(r, c);
                _uiTiles[r, c].Scene.Position = position;
                if (tiles[r, c].IsPartOfMaze)
                {
                    DrawDoors(_uiTiles[r, c], tiles[r, c], tiles);
                    _uiTiles[r, c].Scene.SetGraphic(tiles[r, c].Graphic);
                    if (tiles[r,c].TileEventId == TileEventId.Portal)
                    {
                        // drawn 0,0 at top left, which is not how the tiles are drawn on the map :^)
                        var tpArr = tiles[r, c].TPLocation;
                        int[] correctedLocation = [tpArr[1], tpArr[0]];
                        string tpLocation = string.Join(',', correctedLocation);
                        _uiTiles[r, c].Scene.SetTeleporterCoords($"[{tpLocation}]");
                    }
                }
                else
                {
                    _uiTiles[r, c].Scene.Visible = false;
                }

                if (c < columns - 1)
                    position = _uiTiles[r, c].Scene.GetGlobalPosition(Direction.Right);
            }

            if (r < rows - 1)
                position = _uiTiles[r, 0].Scene.GetGlobalPosition(Direction.Down);
        }

        var start = _dungeon.Current;
        _currentScene = _uiTiles[start.X, start.Y];
        _player.Position = _currentScene.Scene.Position;
        _crawlUI.SetCoordinates(_currentScene.X, _currentScene.Y);

        if (tier % 50 != 0)
        {
            SetPlayerDirections(start.X, start.Y);
        }

        if (tier > 250 && tier % 10 == 0)
        {
            _gameObject.MusicPlayer.PlayMusic(MusicAssets.DC_BOSS_PRE_BOUNTY);
        }
        else if (tier % 50 == 0)
        {
            _gameObject.MusicPlayer.PlayMusic(MusicAssets.DC_BOSS_PRE);
        }
        else
        {
            _gameObject.MusicPlayer.PlayMusic(MusicAssets.GetDungeonTrackDC(_gameObject.TierDC));
        }

        SetCrawlValues();
        SetEncounterVisibility(true);
        _processingEvent = false;
    }

    private void DrawDoors(UITile uiTile, Tile tile, Tile[,] tiles)
    {
        int x = tile.X;
        int y = tile.Y;
        int length = tiles.GetLength(0);

        // look left
        if (y - 1 >= 0)
            if (tiles[x, y - 1].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Left);

        // look right
        if (y + 1 < length)
            if (tiles[x, y + 1].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Right);

        // look up
        if (x - 1 >= 0)
            if (tiles[x - 1, y].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Up);

        // look down
        if (x + 1 < length)
            if (tiles[x + 1, y].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Down);
    }

    private UITile MakeNewUITile(int x, int y, int attemptCount = 0)
    {
        if (attemptCount > 5)
            throw new Exception("MakeNewUITile failed due to unloaded assets.");

        try
        {
            TileScene tileScene = _uiTileScene.Instantiate<TileScene>();
            UITile uiTile = new UITile() { Scene = tileScene, X = x, Y = y };
            _tiles.AddChild(uiTile.Scene);
            var template = BackgroundAssets.GetCombatDCTileTemplate(_gameObject.TierDC);
            uiTile.Scene.ChangeBackgroundColor(template);
            return uiTile;
        }
        catch (NullReferenceException)
        {
            return MakeNewUITile(x, y, attemptCount + 1);
        }

    }

    private async void OnTileEventTriggeredAsync(object sender, TileEventId id)
    {
        // start of the event, prevent further inputs
        _processingEvent = true;
        _player.SetArrows(false, false, false, false);

        switch (id)
        {
            case TileEventId.Miner:
                _crawlUI.Visible = false;
                var miner = _minerScene.Instantiate<MinerUI>();
                _popup.AddChild(miner);
                miner.SetUIValues();
                await ToSignal(miner, "tree_exited");
                SetCrawlValues();
                _dungeon.Current.EventTriggered = false;
                _crawlUI.Visible = true;
                break;
            case TileEventId.Shopkeeper:
                _crawlUI.Visible = false;
                var shop = _shopScene.Instantiate<ShopkeeperUI>();
                _popup.AddChild(shop);
                await ToSignal(shop, "tree_exited");
                SetCrawlValues();
                _dungeon.Current.EventTriggered = false;
                _crawlUI.Visible = true;
                break;
            case TileEventId.Item:
            case TileEventId.SpecialItem:
            case TileEventId.PotOfGreed:
                await ShowRewardScreen(id);
                ResetUIAfterItem();
                break;
            case TileEventId.Gem:
                _itemSfxPlayer.Play();
                _dungeon.ProcessAction(id);
                if(_dungeon.CanExit)
                    _player.DisplayPopup("+1 Orb");
                else
                    _player.DisplayPopup("+1 Gem");

                ResetUIAfterItem();
                break;

            case TileEventId.BossDialog:
                var dScene = _dungeonDialogScene.Instantiate<DC_BOSS_CUTSCENE>();
                _popup.AddChild(dScene);
                dScene.Start(_dungeon.Current.Entity, _dungeon.Current.EntityImage);

                await ToSignal(dScene, "tree_exited");

                var pFlags = _gameObject.ProgressFlagObject;
                if(!pFlags.ViewedDCCutscenes.Contains(_gameObject.TierDC))
                    pFlags.ViewedDCCutscenes.Add(_gameObject.TierDC);
                _currentScene.Scene.TurnOffGraphic();
                PersistentGameObjects.Save();
                break;
            case TileEventId.SpecialBossEncounter:
            case TileEventId.SpecialEncounter:
            case TileEventId.Encounter:
            case TileEventId.Tikki:
                bool doNotDoEncounter = false;

                if (id == TileEventId.SpecialBossEncounter)
                {
                    _dungeon.Current.EventTriggered = false;

                    var bossPrompt = _yesNoPopup.Instantiate<AscendedYesNoWindow>();
                    _popup.AddChild(bossPrompt);
                    bossPrompt.SetDialogMessage("You sense a dangerous presence beyond the door. Do you proceed?");
                    bossPrompt.AnswerSelected += (sender, args) =>
                    {
                        doNotDoEncounter = !args;
                    };

                    await ToSignal(bossPrompt, "tree_exited");
                }

                if (doNotDoEncounter)
                    break;

                var combatScene = _battleScene.Instantiate<BattleEnemyScene>();
                var transition = _transitionScene.Instantiate<SceneTransition>();

                this.AddChild(transition);
                float oldPosition = _audioStreamPlayer.GetPlaybackPosition();

                _encounterSfxPlayer.Play();

                transition.PlayFadeIn();
                await ToSignal(transition.Player, "animation_finished");

                SetEncounterVisibility(false);

                this.AddChild(combatScene);
                bool retreat = false;
                combatScene.BackToHome += (sender, args) =>
                {
                    retreat = true;
                };
                await Task.Delay(100);
                bool isSPBossEncounter = (id == TileEventId.SpecialBossEncounter);
                if (id != TileEventId.Tikki)
                {
                    combatScene.SetupForDungeonCrawlEncounter(_battlePlayers, (id == TileEventId.SpecialEncounter) || isSPBossEncounter, isSPBossEncounter);
                }
                else
                {
                    combatScene.SetupForDungeonCrawlEncounter(_battlePlayers, false, false, _dungeon.Current.Entity);
                }

                transition.PlayFadeOut();
                await ToSignal(transition.Player, "animation_finished");
                try { transition.QueueFree(); } catch (Exception) { }
                await ToSignal(combatScene, "tree_exited");

                if (retreat)
                {
                    _prematurelyLeave = true;
                    _OnRetreatButtonPressed(null, EventArgs.Empty);
                }
                else
                {
                    int tier = _gameObject.TierDC;
                    if (tier % 50 != 0)
                    {
                        _gameObject.MusicPlayer.PlayMusic(MusicAssets.GetDungeonTrackDC(_gameObject.TierDC), oldPosition);
                    }

                    if (id == TileEventId.SpecialBossEncounter)
                    {
                        _gameObject.RandomizedBossEncounters.Remove(tier);
                    }

                    _dungeon.ProcessAction(id);

                    if(id == TileEventId.Tikki)
                    {
                        _gameObject.DefeatedTikkis[_gameObject.TierDC].Add(_dungeon.Current.Entity);
                        if (_dungeon.CanExit)
                        {
                            _gameObject.TikkiBosses.Remove(_gameObject.TierDC);
                        }
                    }

                    SetEncounterVisibility(true);
                    SetCrawlValues();
                    _currentScene.Scene.TurnOffGraphic(); // <-- turnoff when finished
                    PersistentGameObjects.Save();
                }
                _dungeon.Current.EventTriggered = true;
                break;

            case TileEventId.Heal:
                // heal some amount of HP/MP
                foreach (var player in _battlePlayers)
                {
                    player.HP = player.MaxHP;
                }

                _healSfxPlayer.Play();
                SetCrawlValues();
                _currentScene.Scene.TurnOffGraphic(); // <-- turnoff when finished
                PersistentGameObjects.Save();
                break;

            case TileEventId.Orb:
                _gameObject.Orbs++;
                _itemSfxPlayer.Play();
                _player.DisplayPopup("+1 Orb");
                _currentScene.Scene.TurnOffGraphic();
                SetCrawlValues();
                break;

            case TileEventId.Portal:
                _dungeon.Current.EventTriggered = false;

                var popupWindow = _yesNoPopup.Instantiate<AscendedYesNoWindow>();
                _popup.AddChild(popupWindow);
                popupWindow.SetDialogMessage("Teleport?");
                popupWindow.AnswerSelected += (sender, args) =>
                {
                    if (args)
                    {
                        // spawn yes/no box
                        var tile = _dungeon.Current;
                        int x = tile.TPLocation[0];
                        int y = tile.TPLocation[1];

                        _currentScene = _uiTiles[x, y];

                        var tween = CreateTween();
                        tween.TweenProperty(_player, "position", _currentScene.Scene.Position, 0.25);
                        tween.Finished += () =>
                        {
                            _processingEvent = false;
                            _dungeon.MoveDirection(x, y, true);
                            _crawlUI.SetCoordinates(x, y);
                        };

                        _dungeon.Current.EventTriggered = false;
                    }
                };
                await ToSignal(popupWindow, "tree_exited");
                SetCrawlValues();
                break;

            case TileEventId.Fountain:
                _crawlUI.Visible = false;
                var fountain = _fountainScene.Instantiate<FountainOfBuce>();
                _popup.AddChild(fountain);
                await ToSignal(fountain, "tree_exited");
                SetCrawlValues();
                _dungeon.Current.EventTriggered = false;
                _crawlUI.Visible = true;
                break;

            case TileEventId.Exit:
                // handle dungeon end stuff (do yes/no box)
                if (_dungeon.CanExit)
                {
                    _endingScene = true;
                    _camera.Enabled = false;
                    _floorExitScene.Visible = true;
                    // _endBackground.Visible = true;
                    SetEncounterVisibility(false, true);
                    List<EndScreenItem> viewable = new List<EndScreenItem>();

                    int startIndex = 0;
                    if (!(_gameObject.TierDC + 1 < _gameObject.TierDCCap) && !_gameObject.ProgressFlagObject.EndgameUnlocked)
                    {
                        // the continue button
                        _endScreenItems[0].ItemText = "Ascend";
                        _endScreenItems[0].ItemSelected -= _OnContinueToNextFloor;
                        _endScreenItems[0].ItemSelected += _OnContinueFinalBoss;
                    }

                    for (int i = startIndex; i < _endScreenItems.Count; i++)
                    {
                        viewable.Add(_endScreenItems[i]);
                    }

                    _floorExitScene.EndScreenOptions.SetItems(viewable);
                    _floorExitScene.EndScreenOptions.CanInput = true;
                }
                _dungeon.Current.EventTriggered = false;
                break;
        }


        if (_dungeon.Current.EventTriggered)
        {
            var spTiles = _dungeon.SpecialTiles;
            int removed = 0;
            if (id == TileEventId.Tikki)
            {
                removed = spTiles.RemoveAll(sp => sp.Entity == _dungeon.Current.Entity);
            }
            else
            {
                removed = spTiles.RemoveAll(sp => sp.Id == id);
            }
            
            if (removed > 0)
                _crawlUI.SetSpecialTileContainer(spTiles);
        }

        // on completion of the event
        SetPlayerDirections(_currentScene.X, _currentScene.Y);
        _processingEvent = false;
    }

    private async void _OnContinueFinalBoss(object sender, EventArgs e)
    {
        // _endBackground.Visible = false;
        _floorExitScene.Visible = false;
        _crawlUI.Visible = false;
        _retreat.Visible = false;
        _inventory.Visible = false;

        var gameObject = _gameObject;
        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        GetTree().Root.AddChild(transition);
        transition.PlayFadeIn();
        await ToSignal(transition.Player, "animation_finished");
        var cutsceneObject = gameObject.CutsceneObject;
        
        IncrementMaxTier();

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

        transition.PlayFadeOut();
        QueueFree();
    }

    private async Task ShowRewardScreen(TileEventId id)
    {
        var reward = _rewardScene.Instantiate<RewardScreen>();
        _itemSfxPlayer.Play();
        _crawlUI.Visible = false;
        _popup.AddChild(reward);
        if (id == TileEventId.PotOfGreed)
            reward.InitializePotOfGreedRewards();
        else if (id == TileEventId.SpecialItem)
            reward.InitializeDungeonCrawlSpecialItems();
        else if (id == TileEventId.Item)
            reward.RandomizeDungeonCrawlRewards();
        else
            reward.InitializeDungeonCrawlTierRewards();

        await ToSignal(reward, "tree_exited");
    }

    private void ResetUIAfterItem()
    {
        SetCrawlValues();
        _crawlUI.Visible = true;
        PersistentGameObjects.Save();
        _currentScene.Scene.TurnOffGraphic();
    }

    private async void _OnContinueToNextFloor(object sender, EventArgs e)
    {
        _floorExitScene.Visible = false;
        _floorExitScene.EndScreenOptions.CanInput = false;
        await ShowRewardScreen(TileEventId.Exit);

        var transition = _transitionScene.Instantiate<SceneTransition>();
        AddChild(transition);
        transition.PlayFadeIn();
        
        await ToSignal(transition.Player, "animation_finished");
        // _endBackground.Visible = false;
        _camera.Enabled = true;
        if (_dungeon.FloorType == FloorTypes.Tikki)
        {
            _gameObject.DefeatedTikkis.Remove(_gameObject.TierDC);
        }

        IncrementMaxTier();

        if (CutsceneAssets.StartLabrybuceFloor.ContainsKey(_gameObject.TierDC) && !_gameObject.CutsceneObject.StartOfLabrybuceWatched.Contains(_gameObject.TierDC))
        {
            _gameObject.CutsceneObject.StartOfLabrybuceWatched.Add(_gameObject.TierDC);
            var cutscene = ResourceLoader.Load<PackedScene>(CutsceneAssets.StartLabrybuceFloor[_gameObject.TierDC]);
            this.GetTree().Root.AddChild(cutscene.Instantiate());
            PersistentGameObjects.Save();

            transition.PlayFadeOut();
            await ToSignal(transition.Player, "animation_finished");
            try { transition.QueueFree(); } catch (Exception) { }
            QueueFree();
        }
        else
        {
            SetEncounterVisibility(true, true);

            foreach (var member in _battlePlayers)
                member.HP = member.MaxHP;

            StartDungeon();
            _crawlUI.Visible = true;
            transition.PlayFadeOut();
            await ToSignal(transition.Player, "animation_finished");
            try { transition.QueueFree(); } catch (Exception) { }

            _endingScene = false;

            PersistentGameObjects.Save();
        }


    }

    private void _OnStayButtonPressed(object sender, EventArgs e)
    {
        _endingScene = false;
        _floorExitScene.Visible = false;
        _camera.Enabled = true;
        _floorExitScene.EndScreenOptions.CanInput = false;
        SetEncounterVisibility(true, true);
    }

    private async void _OnRetreatButtonPressed(object sender, EventArgs e)
    {
        _floorExitScene.EndScreenOptions.CanInput = false;
        _crawlUI.Visible = false;
        _floorExitScene.Visible = false;
        _retreat.Visible = false;
        _inventory.Visible = false;
        if (!_prematurelyLeave)
        {
            // _endBackground.Visible = false;
            IncrementMaxTier();
            var rewards = _rewardScene.Instantiate<RewardScreen>();
            _popup.AddChild(rewards);
            rewards.InitializeDungeonCrawlTierRewards();
            _itemSfxPlayer.Play();
            await ToSignal(rewards, "tree_exited");
        }

        PersistentGameObjects.Save();
        TransitionScenes(Scenes.MAIN, _audioStreamPlayer);
    }

    private void IncrementMaxTier()
    {
        int tier = _gameObject.TierDC;
        int maxTier = _gameObject.MaxTierDC;

        if (tier == maxTier && tier + 1 <= _gameObject.TierDCCap)
        {
            _gameObject.MaxTierDC++;
            _gameObject.TierDC++;
        }
        else
        {
            _gameObject.TierDC++;
        }
    }

    private void SetEncounterVisibility(bool visible, bool keepCamera = false)
    {
        _tiles.Visible = visible;
        _camera.Enabled = visible;
        _player.Visible = visible;
        _retreat.Visible = visible;
        _inventory.Visible = visible;
        if (!keepCamera)
        {
            _crawlUI.Visible = visible;
            _popup.Visible = visible;
        }
    }
}
