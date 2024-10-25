using AscendedZ;
using AscendedZ.dungeon_crawling.scenes.crawl_ui;
using Godot;
using System;
using static Godot.WebSocketPeer;

public partial class ShopkeeperUI : CenterContainer
{
    private bool _emptyClicked = false;
	private ShopkeeperUIObject _shopkeeperUIObject;
    private ItemList _list;
    private Label _description, _orbCount;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _shopkeeperUIObject = new ShopkeeperUIObject();

        _list = GetNode<ItemList>("%ItemList");
        _description = GetNode<Label>("%ItemDescription");
        _orbCount = GetNode<Label>("%OrbAmount");

        Button buyButton = GetNode<Button>("%BuyButton");
        Button backButton = GetNode<Button>("%BackButton");

        foreach(var item in _shopkeeperUIObject.Items)
        {
            _list.AddItem($"{item.Name} [{item.Cost} Orbs]", SkillAssets.GenerateIcon(item.Icon));
        }

        _list.ItemSelected += (long selected) => 
        {
            _shopkeeperUIObject.Selected = (int)selected;
        };

        _list.ItemClicked += _OnItemClicked;
        _list.EmptyClicked += (vectorPosition, mouseButtonIndex) => _emptyClicked = true;
        _list.FocusExited += () => _emptyClicked = false;
        _list.Select(0);

        _description.Text = _shopkeeperUIObject.SelectedItem.Description;
        _orbCount.Text = _shopkeeperUIObject.OrbCount.ToString();

        buyButton.Pressed += () =>
        {
            _shopkeeperUIObject.Buy();
            _orbCount.Text = _shopkeeperUIObject.OrbCount.ToString();
        };

        backButton.Pressed += () => QueueFree();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.UP) && !_emptyClicked)
        {
            _shopkeeperUIObject.Selected--;
            if (_shopkeeperUIObject.Selected < 0)
                _shopkeeperUIObject.Selected = 0;

            _list.Select(_shopkeeperUIObject.Selected);
            _description.Text = _shopkeeperUIObject.SelectedItem.Description;
            _orbCount.Text = _shopkeeperUIObject.OrbCount.ToString();
        }

        if (@event.IsActionPressed(Controls.DOWN) && !_emptyClicked)
        {
            _shopkeeperUIObject.Selected++;
            if (_shopkeeperUIObject.Selected >= _list.ItemCount)
                _shopkeeperUIObject.Selected = _list.ItemCount - 1;

            _list.Select(_shopkeeperUIObject.Selected);
            _description.Text = _shopkeeperUIObject.SelectedItem.Description;
        }

        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _shopkeeperUIObject.Buy();
            _orbCount.Text = _shopkeeperUIObject.OrbCount.ToString();
        }

        if (@event.IsActionPressed(Controls.BACK))
        {
            QueueFree();
        }
    }

    private void _OnItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Left)
        {
            _emptyClicked = true;
            _shopkeeperUIObject.Selected = (int)index;
            _description.Text = _shopkeeperUIObject.SelectedItem.Description;
        }
    }
}
