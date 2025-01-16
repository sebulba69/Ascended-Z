using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public partial class SkillUpgradeUnit : HBoxContainer
{
	private TextureRect _skillIcon;
	private Label _skillDisplayName, _costLabel;
	private Button _upgradeButton;

	private Currency _vorpex;
	private OverworldEntity _entity;
	private BattlePlayer _bp;
	private ISkill _skill;
	private int _skillIndex, _cost, _cap;

	public EventHandler UpdateVorpex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _skillIcon = GetNode<TextureRect>("%SkillIcon");
		_skillDisplayName = GetNode<Label>("%SkillDisplayName");
		_costLabel = GetNode<Label>("%VorpexCost");
        _upgradeButton = GetNode<Button>("%UpgradeButton");

		_upgradeButton.Pressed += _OnUpgradePressed;

		_vorpex = PersistentGameObjects.GameObjectInstance().MainPlayer.Wallet.Currency[SkillAssets.VORPEX_ICON];
	}

	public void SetSkill(OverworldEntity entity, int skillIndex)
	{
		_entity = entity;
        _skillIndex = skillIndex;
        _skill = entity.Skills[_skillIndex];

        ChangeSkillIconRegion(SkillAssets.GetIcon(_skill.Icon));
        SetSkillDisplay();
    }

	private void SetSkillDisplay()
	{
        _bp = _entity.MakeBattlePlayerBase();

		_cap = 999;
		int modifier = 0;
		if(_bp.Skills[_skillIndex].Id == SkillId.Elemental)
		{
			var element = (ElementSkill)_bp.Skills[_skillIndex];
			modifier = element.Level;

            _cap = element.Cap;
        }
		else if (_bp.Skills[_skillIndex].Id == SkillId.Healing)
		{
            var heal = (HealSkill)_bp.Skills[_skillIndex];
			modifier = heal.Level;

			_cap = heal.Cap;
        }

		_cost = Equations.GetVorpexLevelValue(_entity.VorpexValue + modifier, modifier);
        _costLabel.Text = _cost.ToString();
        if (_skill.Level >= _cap)
		{
			_skillDisplayName.Text = _bp.Skills[_skillIndex].GetBattleDisplayString();
			_upgradeButton.Disabled = true;
			if (PersistentGameObjects.GameObjectInstance().MaxTier < 250)
				_upgradeButton.Text = "[SOFT CAP]";
			else
				_upgradeButton.Text = "[MAX]";
		}
		else
		{
            _skillDisplayName.Text = _bp.Skills[_skillIndex].GetUpgradeString();
        }
    }

    private void ChangeSkillIconRegion(KeyValuePair<int, int> coords)
    {
        AtlasTexture atlas = _skillIcon.Texture as AtlasTexture;
        atlas.Region = new Rect2(coords.Key, coords.Value, 32, 32);
    }

	private void _OnUpgradePressed()
	{
		if(_vorpex.Amount - _cost >= 0 && _skill.Level + 1 <= _cap)
		{
			_vorpex.Amount -= _cost;
			_skill.LevelUp();
            SetSkillDisplay();
            UpdateVorpex?.Invoke(null, EventArgs.Empty);

            PersistentGameObjects.Save();
        }
	}
}
