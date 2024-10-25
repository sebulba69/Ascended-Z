using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using AscendedZ.screens.skill_transfer_screen;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;

public partial class SkillSelectionScene : VBoxContainer
{
    private PartyMemberDisplay _display;
    private ItemList _itemList;

    private EntitySelectionSkillsObject _sceneObject;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _display = GetNode<PartyMemberDisplay>("%PartyMemberIcon");
        _itemList = GetNode<ItemList>("%ItemList");

        _itemList.SelectMode = ItemList.SelectModeEnum.Multi;
    }

    public override void _Input(InputEvent @event) 
    { 
        if (@event is InputEventMouseButton mbe) 
        {
            mbe.CtrlPressed = true;
        } 
    }

    public void SetSelected(OverworldEntity selected)
    {
        _itemList.Clear();
        _sceneObject = new EntitySelectionSkillsObject(selected);
        _display.ShowRandomEntity(new EntityUIWrapper() { Entity = selected });

        var bp = selected.MakeBattlePlayerBase();

        foreach (var skill in bp.Skills)
        {
            _itemList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
        }
    }

    public List<SkillIndexWrapper> GetSelectedSkills() 
    {
        return _sceneObject.GetSelectedSkills(_itemList.GetSelectedItems());
    }
}
