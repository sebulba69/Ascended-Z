using AscendedZ;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;

public partial class StatusIcon : Control
{
	private TextureRect _icon;
	private Label _counter;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_icon = this.GetNode<TextureRect>("%Icon");
		_counter = this.GetNode<Label>("%Counter");
	}

	public void SetIcon(StatusIconWrapper wrapper)
	{
		_icon.Texture = SkillAssets.GenerateIcon(wrapper.Icon);
		_counter.Text = wrapper.Counter.ToString();
		Visible = !wrapper.SetInvisible;
		_counter.AddThemeColorOverride("font_color", wrapper.CounterColor);
		GetNode<PanelContainer>("%PanelContainer").TooltipText = wrapper.Description;
    }
}
