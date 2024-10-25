using AscendedZ;
using AscendedZ.screens.end_screen;
using Godot;
using System;
using System.Collections.Generic;

public partial class EndScreenOptions : ItemList
{
	public bool EmptyClick { get; set; }
    public bool CanInput { get; set; }
    private int _selectedIndex = 0;
    private EndScreenOptionsObject _endScreenObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        FocusExited += () => EmptyClick = false;
        EmptyClicked += (position, mouseButtonIndex) => { EmptyClick = true; };
        ItemClicked += _OnMenuItemClicked;
	}

    public override void _Input(InputEvent @event)
    {
        if (!CanInput) return;

        if (@event.IsActionPressed(Controls.UP) && !EmptyClick)
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = 0;
            Select(_selectedIndex);
            _endScreenObject.SelectedIndex = _selectedIndex;
        }

        if (@event.IsActionPressed(Controls.DOWN) && !EmptyClick)
        {
            _selectedIndex++;
            if (_selectedIndex >= ItemCount) _selectedIndex = ItemCount - 1;
            Select(_selectedIndex);
            _endScreenObject.SelectedIndex = _selectedIndex;
        }

        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _endScreenObject.InvokeSelected();
        }
    }

    /// <summary>
    /// Call this method before setting the screen to be visible.
    /// </summary>
    /// <param name="items"></param>
    public void SetItems(List<EndScreenItem> items) 
    {
        Clear();

        foreach (var item in items) 
        {
            AddItem(item.ItemText);
        }

        _selectedIndex = 0;

        _endScreenObject = new EndScreenOptionsObject()
        {
            SelectedIndex = _selectedIndex,
            Items = items
        };

        Select(_selectedIndex);
    }

    private void _OnMenuItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (CanInput && mouse_button_index == (long)MouseButton.Left)
        {
            // This guarantees that the user has to double click to choose an option with mouse.
            if (_selectedIndex != index)
            {
                _selectedIndex = (int)index;
                _endScreenObject.SelectedIndex = _selectedIndex;
            }
            else
            {
                _endScreenObject.InvokeSelected();
            }
        }
    }
}
