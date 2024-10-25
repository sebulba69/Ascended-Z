using AscendedZ;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.dungeon_crawling.backend.dungeon_items;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using AscendedZ.entities.battle_entities;

public enum InventoryMenuState
{
    ItemSelect,
    TargetSelect
}

public partial class InventoryUI : CenterContainer
{
    private PanelContainer _panel;
    private TextureRect _icon;
    private Label _toolTip, _menu, _description;
    private ItemList _itemList;
    private AudioStreamPlayer _audioStreamPlayer;

    private int _selectedIndex, _itemSelectIndex;
    private bool _emptyClick;
    private InventoryMenuState _state = InventoryMenuState.ItemSelect;
    private List<IDungeonItem> _items;
    List<BattlePlayer> _playerTargetList;

    private readonly string SKILL_TOOLTIP = "Choose an item!";
    private readonly string TARGET_STR = $"(X) Items, ({Controls.GetControlString(Controls.CONFIRM)}) Select";
    private readonly string BACK_STR = "(X) Back";

    public List<BattlePlayer> Players { get; set; }

    public EventHandler<IDungeonItem> UseItemDScreen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _panel = GetNode<PanelContainer>("%DescriptionPanel");
        _icon = GetNode<TextureRect>("%ActionMenuIcon");
        _toolTip = GetNode<Label>("%ActionMenuSkill");
        _description = GetNode<Label>("%ItemDescription");
        _itemList = GetNode<ItemList>("%ItemList");
        _menu = GetNode<Label>("%MenuLabel");
        _audioStreamPlayer = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        var inv = PersistentGameObjects.GameObjectInstance().LabrybuceInventoryObject;
        _toolTip.Text = SKILL_TOOLTIP;
        _items = inv.GetItems();
        _selectedIndex = 0;

        _itemList.EmptyClicked += (vectorPosition, mouseButtonIndex) => _emptyClick = true;
        _itemList.FocusExited += () => _emptyClick = false;
        _playerTargetList = new List<BattlePlayer>();
        LoadItems();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.UP) && !_emptyClick)
        {
            _selectedIndex--;
            if (_selectedIndex <= 0)
                _selectedIndex = 0;

            _itemList.Select(_selectedIndex);
            SelectItem(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.DOWN) && !_emptyClick)
        {
            _selectedIndex++;
            if (_selectedIndex >= _itemList.ItemCount)
                _selectedIndex = _itemList.ItemCount - 1;

            _itemList.Select(_selectedIndex);
            SelectItem(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.BACK))
        {
            if (_state == InventoryMenuState.TargetSelect)
            {
                LoadItems();
            }
            else
            {
                QueueFree();
            }
        }

        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            DoSelection(_selectedIndex);
        }
    }

    private void LoadItems()
    {
        _state = InventoryMenuState.ItemSelect;
        _itemSelectIndex = 0;
        _itemList.Clear();

        foreach (var item in _items)
        {
            _itemList.AddItem($"{item.Name} x{item.Amount}", SkillAssets.GenerateIcon(item.Icon));
        }

        if (_selectedIndex >= _itemList.ItemCount)
            _selectedIndex = _itemList.ItemCount - 1;


        if (_itemList.ItemCount > 0)
        {
            _itemList.Select(_selectedIndex);
            _panel.Visible = true;
            SelectItem(_selectedIndex);
        }
        else
        {
            _icon.Visible = false;
            _panel.Visible = false;
            _toolTip.Text = SKILL_TOOLTIP;
        }

        _menu.Text = BACK_STR;
    }

    private void LoadTargetList()
    {
        _itemList.Clear();

        TargetTypes itemTargetType = _items[_itemSelectIndex].TargetType;
        _playerTargetList.Clear();

        // only show valid targets for the skill we have selected
        if (itemTargetType == TargetTypes.SINGLE_TEAM)
        {
            _playerTargetList.AddRange(Players.FindAll(p => p.HP > 0 && p.HP < p.MaxHP));
        }
        else
        {
            _playerTargetList.AddRange(Players.FindAll(p => p.HP == 0));
        }

        foreach (var player in _playerTargetList)
            _itemList.AddItem($"{player.HP}/{player.MaxHP} HP", CharacterImageAssets.GetTextureForItemList(player.Image));

        _state = InventoryMenuState.TargetSelect;
        _menu.Text = TARGET_STR;
        _icon.Visible = true;

        _selectedIndex = 0;

        if(_itemList.ItemCount > 0)
            _itemList.Select(_selectedIndex);
    }

    private void SelectItem(int index)
    {
        if(index >= _items.Count)
            index = _items.Count - 1;

        if (index < 0)
            return;

        var item = _items[index];

        _description.Text = item.Description;
        _toolTip.Text = $"{item.Name} x{item.Amount}";
        _icon.Visible = true;
        ChangeItemIconRegion(SkillAssets.GetIcon(item.Icon));
    }

    private void ChangeItemIconRegion(KeyValuePair<int, int> coords)
    {
        AtlasTexture atlas = _icon.Texture as AtlasTexture;
        atlas.Region = new Rect2(coords.Key, coords.Value, 32, 32);
    }

    private void _OnMenuItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Left)
        {
            _emptyClick = true;
            DoSelection(index);
        }
    }

    private void DoSelection(long index)
    {
        _selectedIndex = (int)index;

        switch (_state)
        {
            case InventoryMenuState.ItemSelect:
                if (_itemList.ItemCount > 0)
                {
                    TargetTypes targetType = _items[_selectedIndex].TargetType;
                    // if self, it's the dungeon screen
                    if (targetType == TargetTypes.SELF)
                    {
                        UseItemDScreen?.Invoke(this, _items[_selectedIndex]);
                    }
                    else
                    {
                        _itemSelectIndex = _selectedIndex;
                        _selectedIndex = 0;
                        LoadTargetList();
                    }
                }
                break;

            case InventoryMenuState.TargetSelect:
                if (_itemList.ItemCount == 0)
                    return;

                var item = _items[_itemSelectIndex];
                item.Use([_playerTargetList[_selectedIndex]]);
                _audioStreamPlayer.Play();
                UseItemDScreen?.Invoke(this, null);

                PersistentGameObjects.GameObjectInstance().LabrybuceInventoryObject.UseItem(item);

                if (item.Amount > 0)
                {
                    _toolTip.Text = $"{item.Name} x{item.Amount}";
                    LoadTargetList();
                } 
                else
                    LoadItems();

                break;
        }
    }
}
