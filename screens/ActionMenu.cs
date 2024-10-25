using AscendedZ;
using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities;
using AscendedZ.skills;
using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Reflection;

public enum ActionMenuState
{
    Menu,
    SkillSelect,
    TargetSelect
}

public partial class ActionMenu : Control
{
	private ItemList _actionList;
	private Label _menu;
    private bool _canInput;
    private int _selectedIndex;
    private ActionMenuState _state;
    private TextureRect _icon;
    private Label _toolTip;
    private readonly string MENU_STR = $"(←/X) Menu, ({Controls.GetControlString(Controls.CONFIRM)}) Select";
    private readonly string SKILL_STR = $"(→) Skills, ({Controls.GetControlString(Controls.CONFIRM)}) Select";
    private readonly string SKILL_TOOLTIP = "Choose a skill!";
    private readonly string TARGET_STR = $"(←/X) Skills, ({Controls.GetControlString(Controls.CONFIRM)}) Select";

    public bool CanInput { get => _canInput; set => _canInput = value; }
    public bool EmptyClick { get; set; }
    private BattleSceneObject _battleSceneObject;

    private PlayerTargetSelectedEventArgs _playerTargetSelectedEventArgs;

    public BattleSceneObject BattleSceneObject 
    {
        get => _battleSceneObject;
        set => _battleSceneObject = value;
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        base._Ready();

        _state = ActionMenuState.Menu;

        _icon = this.GetNode<TextureRect>("%ActionMenuIcon");
        _toolTip = this.GetNode<Label>("%ActionMenuSkill");

		_actionList = this.GetNode<ItemList>("%ActionList");
		_menu = this.GetNode<Label>("%MenuLabel");
        _canInput = true;
        _selectedIndex = 0;

        // in the menu is Skill, Retreat
        _menu.Text = MENU_STR;
        _toolTip.Text = SKILL_TOOLTIP;
        _icon.Visible = false;

        // item_clicked
        _actionList.ItemSelected += (long selected) => { _selectedIndex = (int)selected; };
        _actionList.ItemClicked += _OnMenuItemClicked;
        _actionList.FocusExited += () => EmptyClick = false;
        _actionList.EmptyClicked += (vectorPosition, mouseButtonIndex) => { EmptyClick = true; };
    }

    public override void _Input(InputEvent @event)
    {
        if (!_canInput)
            return;

        if (@event.IsActionPressed(Controls.UP) && !EmptyClick)
        {
            _selectedIndex--;
            if (_selectedIndex <= 0)
                _selectedIndex = 0;

            DoActionIndexSelectionChange(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.DOWN) && !EmptyClick)
        {
            _selectedIndex++;
            if (_selectedIndex >= _actionList.ItemCount)
                _selectedIndex = _actionList.ItemCount - 1;

            DoActionIndexSelectionChange(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.LEFT) || @event.IsActionPressed(Controls.BACK))
        {
            if (_state == ActionMenuState.SkillSelect)
            {
                LoadMenu();
            }
            else if(_state == ActionMenuState.TargetSelect)
            {
                LoadActiveSkillList();
            }
        }

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            if (_state == ActionMenuState.Menu)
            {
                LoadActiveSkillList();
            }
        }

        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            DoSelection(_selectedIndex);
        }
    }

    private void DoActionIndexSelectionChange(int index)
    {
        if(index >= 0 && index < _actionList.ItemCount)
            _actionList.Select(_selectedIndex);
    }

    private void LoadMenu()
    {
        _actionList.Clear();
        _actionList.AddItem("Magic", SkillAssets.GenerateIcon(SkillAssets.MAGIC_ICON));

        var retreat = SkillDatabase.Retreat;

        _actionList.AddItem(retreat.GetBattleDisplayString(), SkillAssets.GenerateIcon(retreat.Icon));

        _menu.Text = SKILL_STR;
        _state = ActionMenuState.Menu;
        _toolTip.Text = string.Empty;
        _icon.Visible = false;

        _selectedIndex = 0;
        _actionList.Select(0);
    }

    public void LoadActiveSkillList()
    {
        var active = BattleSceneObject.ActivePlayer;

        if (active == null)
            return;

        _actionList.Clear();

        foreach (ISkill skill in active.Skills)
        {
            _actionList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
        }

        _selectedIndex = 0;
        _actionList.Select(0);

        _menu.Text = MENU_STR;
        _toolTip.Text = SKILL_TOOLTIP;
        _icon.Visible = false;
        _state = ActionMenuState.SkillSelect;
    }

    private void LoadTargetList()
    {
        _actionList.Clear();

        TargetTypes skillTargetType = _battleSceneObject.ActivePlayer.Skills[_playerTargetSelectedEventArgs.SkillIndex].TargetType;
        
        // only show valid targets for the skill we have selected
        if (skillTargetType == TargetTypes.SINGLE_OPP)
        {
            foreach (var enemy in _battleSceneObject.Enemies.FindAll(enemy => enemy.HP > 0))
                _actionList.AddItem($"{enemy.HP:n0}/{enemy.MaxHP:n0} HP", CharacterImageAssets.GetTextureForItemList(enemy.Image));
        }
        else
        {
            var playerTargetList = _battleSceneObject.AlivePlayers;

            if (skillTargetType == TargetTypes.SINGLE_TEAM_DEAD)
                playerTargetList = _battleSceneObject.DeadPlayers;

            foreach (var player in playerTargetList)
                _actionList.AddItem($"{player.HP:n0}/{player.MaxHP:n0} HP", CharacterImageAssets.GetTextureForItemList(player.Image));
        }

        _state = ActionMenuState.TargetSelect;

        _menu.Text = TARGET_STR;
        
        _icon.Visible = true;
        
        ISkill skill = _battleSceneObject.ActivePlayer.Skills[_playerTargetSelectedEventArgs.SkillIndex];

        ChangeSkillIconRegion(SkillAssets.GetIcon(skill.Icon));
        _toolTip.Text = skill.GetBattleDisplayString();
        
        _selectedIndex = 0;
        _actionList.Select(0);
    }

    private void ChangeSkillIconRegion(KeyValuePair<int, int> coords)
    {
        AtlasTexture atlas = _icon.Texture as AtlasTexture;
        atlas.Region = new Rect2(coords.Key, coords.Value, 32, 32);
    }

    private void _OnMenuItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if(_canInput && mouse_button_index == (long)MouseButton.Left)
        {
            EmptyClick = true;
            DoSelection(index);
        }
    }

    private void DoSelection(long index)
    {
        _selectedIndex = (int)index;
        switch (_state)
        {
            case ActionMenuState.Menu:
                if (_selectedIndex == 0)
                {
                    LoadActiveSkillList();
                }
                else if (_selectedIndex == 1)
                {
                    _battleSceneObject.HandlePostTurnProcessing(new BattleResult
                    {
                        SkillUsed = SkillDatabase.Retreat,
                        ResultType = BattleResultType.Retreat
                    });
                }
                else
                {
                    throw new NotImplementedException();
                }
                break;
            case ActionMenuState.SkillSelect:
                _playerTargetSelectedEventArgs = new PlayerTargetSelectedEventArgs
                {
                    SkillIndex = _selectedIndex
                };

                var tType = _battleSceneObject.ActivePlayer.Skills[_playerTargetSelectedEventArgs.SkillIndex].TargetType;
                if (tType == TargetTypes.TEAM_ALL 
                    || tType == TargetTypes.OPP_ALL 
                    || tType == TargetTypes.SELF 
                    || tType == TargetTypes.TEAM_ALL_DEAD)
                {
                    if(tType == TargetTypes.TEAM_ALL_DEAD && _battleSceneObject.DeadPlayers.Count == 0)
                        return;
                    
                    _battleSceneObject.SkillSelected?.Invoke(_battleSceneObject, _playerTargetSelectedEventArgs);
                    _canInput = false;
                }
                else
                {
                    LoadTargetList();
                }
                break;
            case ActionMenuState.TargetSelect:
                _playerTargetSelectedEventArgs.TargetIndex = _selectedIndex;
                _battleSceneObject.SkillSelected?.Invoke(_battleSceneObject, _playerTargetSelectedEventArgs);
                _canInput = false;
                break;
        }
    }
}
