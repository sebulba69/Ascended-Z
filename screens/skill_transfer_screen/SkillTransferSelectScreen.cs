using Godot;
using System;

public partial class SkillTransferSelectScreen : HBoxContainer
{
	private SkillSelectionScene _scene1, _scene2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_scene1 = GetNode<SkillSelectionScene>("%EntitySelectionScene");
		_scene2 = GetNode<SkillSelectionScene>("%EntitySelectionScene2");
	}

	public SkillSelectionScene[] GetSkillSelectionScenes()
	{
		return [_scene1, _scene2];
	}
}
