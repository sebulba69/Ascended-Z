using AscendedZ.screens.skill_transfer_screen;
using AscendedZ.screens.back_end_screen_scripts;
using Godot;
using System;
using AscendedZ.game_object;
using AscendedZ;
using AscendedZ.entities.partymember_objects;
using System.Collections.Generic;
using AscendedZ.skills;

public partial class EntitySelectionScene : VBoxContainer
{
	private PartyMemberDisplay _display;
	private ItemList _itemList;

	private EntitySelectionSceneObject _sceneObject;

	public OverworldEntity Selected { get => _sceneObject.Selected; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_display = GetNode<PartyMemberDisplay>("%PartyMemberIcon");
		_itemList = GetNode<ItemList>("%ItemList");
        _sceneObject = new EntitySelectionSceneObject(PersistentGameObjects.GameObjectInstance().MainPlayer.ReserveMembers);
		_itemList.ItemSelected += _OnSelectedIndexChanged;
        _display.ShowRandomEntity(new EntityUIWrapper() { Entity = _sceneObject.Selected });

        var available = _sceneObject.Available;

        foreach (var reserve in available)
        {
            string name = reserve.DisplayName;
            if (reserve.IsInParty)
                name += " (Party)";

            _itemList.AddItem(name, CharacterImageAssets.GetTextureForItemList(reserve.Image));
        }
    }

	public void ReDisplayEntity()
	{
        _display.ShowRandomEntity(new EntityUIWrapper() { Entity = _sceneObject.Selected });
    }

	private void _OnSelectedIndexChanged(long index)
	{
		_sceneObject.ChangeSelected((int)index);

        _display.ShowRandomEntity(new EntityUIWrapper() { Entity = _sceneObject.Selected });
    }
}
