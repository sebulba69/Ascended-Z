using AscendedZ.entities.partymember_objects;
using Godot;
using System;

public partial class PartyMemberTransferSelectScreen : HBoxContainer
{
	private EntitySelectionScene _scene1, _scene2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_scene1 = GetNode<EntitySelectionScene>("%EntitySelectionScene");
		_scene2 = GetNode<EntitySelectionScene>("%EntitySelectionScene2");
	}

	public void ReDisplayEntities()
	{
		_scene1.ReDisplayEntity();
		_scene2.ReDisplayEntity();
    }

	public OverworldEntity[] GetSelectedEntities()
	{
		return [_scene1.Selected, _scene2.Selected];
	}
}
