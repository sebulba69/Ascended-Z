using AscendedZ;
using AscendedZ.currency;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;

public partial class SkillUpgradeUI : CenterContainer
{
	private Label _vorpexOwned;
	private VBoxContainer _skillDisplays;
	private Button _backButton;

	private Currency _vorpex;
	private PackedScene _unit = ResourceLoader.Load<PackedScene>(Scenes.SKILL_UPGRADE_UNIT);
	private OverworldEntity _entity;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vorpexOwned = GetNode<Label>("%VorpexOwned");
        _skillDisplays = GetNode<VBoxContainer>("%SkillDisplays");
		_backButton = GetNode<Button>("%BackButton");

        _vorpex = PersistentGameObjects.GameObjectInstance().MainPlayer.Wallet.Currency[SkillAssets.VORPEX_ICON];
        _backButton.Pressed += QueueFree;
    }

	public void SetPartyMember(OverworldEntity entity)
	{
        _entity = entity;
        _vorpexOwned.Text = $"{_vorpex.Amount:n0}";

        PostSkills();
    }


    private void PostSkills()
	{
        foreach (var skillDisplays in _skillDisplays.GetChildren())
		{
			var display = (SkillUpgradeUnit)skillDisplays;
            _skillDisplays.RemoveChild(display);

			display.QueueFree();
        }

		for(int i = 0; i < _entity.Skills.Count; i++)
		{
			if(_entity.Skills[i].Id == SkillId.Elemental
				|| _entity.Skills[i].Id == SkillId.Healing)
			{
                var sUnit = _unit.Instantiate<SkillUpgradeUnit>();
                sUnit.UpdateVorpex += _OnVorpexUpdated;

                _skillDisplays.AddChild(sUnit);

                sUnit.SetSkill(_entity, i);
            }
        }
	}

	private void _OnVorpexUpdated(object sender, EventArgs e)
	{
        _vorpexOwned.Text = $"{_vorpex.Amount:n0}";
    }
}
