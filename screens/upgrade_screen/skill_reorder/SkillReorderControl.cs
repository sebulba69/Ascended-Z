using AscendedZ;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.upgrade_screen.skill_reorder;
using AscendedZ.skills;
using Godot;
using System;

public partial class SkillReorderControl : CenterContainer
{
	private ItemList _startList, _endList;
	private Button _reorder, _back;
	private SkillReorderControlObject _object;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startList = GetNode<ItemList>("%StartList");
		_endList = GetNode<ItemList>("%EndList");
		_reorder = GetNode<Button>("%ReorderButton");
		_back = GetNode<Button>("%BackButton");

        _reorder.Pressed += _OnReorderPressed;
        _startList.ItemClicked += _OnItemClicked;
		_back.Pressed += _OnBackButtonPressed;
	}

	public void SetStartList(OverworldEntity entity)
	{
        _object = new SkillReorderControlObject(entity);

        var bp = entity.MakeBattlePlayerBase();

        foreach (ISkill skill in bp.Skills)
            _startList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
    }

    private void _OnItemClicked(long selected, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Left)
        {
            int index = (int)selected;

            if (_object.IsItemAlreadyAdded(index))
            {
                _object.RemoveSkill(index);
            }
            else
            {
                _object.AddSkill(index);
            }

            _endList.Clear();
            foreach (ISkill skill in _object.DisplaySkills)
            {
                if (skill != null)
                {
                    _endList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
                }
            }
        }
    }

    private void _OnReorderPressed()
	{
        if (_object.CanReorder())
        {
            _object.ReorderSkills();
            PersistentGameObjects.Save();

            _startList.Clear();
            _endList.Clear();

            foreach (ISkill skill in _object.EntitySkills)
                _startList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
        }
	}

	private void _OnBackButtonPressed()
	{
		QueueFree();
	}
}
