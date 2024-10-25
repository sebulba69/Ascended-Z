using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities.partymember_objects;
using AscendedZ.entities.sigils;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using AscendedZ.screens.sigil_screen;
using Godot;
using System;

public partial class SigilScene : Control
{
	private TextureRect _image;
	private Label _upgradeCost, _percentageBoost, _nameLabel, _level;
	private Button _boost;
	private SigilWrapper _wrapper;
    private Sigil _sigil;

	public EventHandler UnequipSigil;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_image = GetNode<TextureRect>("%SigilIcon");
        _nameLabel = GetNode<Label>("%NameLabel");
        _level = GetNode<Label>("%Level");
        _upgradeCost = GetNode<Label>("%OwnedDellencoin");
		_percentageBoost = GetNode<Label>("%PercentageBoost");
		_boost = GetNode<Button>("%UpgradeButton");

		_boost.Pressed += _OnUpgradeButtonPressed;

		Button unequipButton = GetNode<Button>("%UnequipButton");
		unequipButton.Pressed += () => 
		{
			_sigil.Index = -1;
            _wrapper.PartyMemberDisplay.ShowRandomEntity(new EntityUIWrapper() { Entity = _wrapper.Entity });
			UnequipSigil?.Invoke(null, EventArgs.Empty);
        };
    }

	public void Initialize(SigilWrapper wrapper, Sigil sigil)
    {
		_sigil = sigil;
		_wrapper = wrapper;
        _nameLabel.Text = _sigil.Name;
        _image.Texture = ResourceLoader.Load<Texture2D>(sigil.Image);
		SetValuesThatChange();
	}

	private void SetValuesThatChange()
	{
		_level.Text = $"[L.{_sigil.Level}]";
		if(_sigil.Level == PersistentGameObjects.GameObjectInstance().SigilLevelCap)
            _level.Text = $"[MAX]";

        _upgradeCost.Text = $"{_sigil.LevelUpCost:n0}";
		_percentageBoost.Text = $"+{Math.Round(_sigil.BoostPercentage * 100)}%";
    }

	private void _OnUpgradeButtonPressed()
	{
		if(_wrapper.Dellencoin.Amount - _sigil.LevelUpCost >= 0)
		{
			if (_sigil.CanLevelUp())
			{
				_wrapper.Dellencoin.Amount -= _sigil.LevelUpCost;
				_sigil.LevelUp();
				SetValuesThatChange();
				_wrapper.DellencoinLabel.Text = $"{_wrapper.Dellencoin.Amount:n0}";
				_wrapper.PartyMemberDisplay.ShowRandomEntity(new EntityUIWrapper() { Entity = _wrapper.Entity });
            }
		}
	}
}
