using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.game_object;
using Godot;
using System;
using System.Globalization;

public partial class MinerUI : CenterContainer
{
	private Button _buyButton, _backButton;

	private Label _morbis, _pickaxes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_buyButton = GetNode<Button>("%BuyPickaxes");
		_backButton = GetNode<Button>("%BackBtn");
		_morbis = GetNode<Label>("%Morbis");
        _pickaxes = GetNode<Label>("%Pickaxes");

		_buyButton.Text = $"[{Controls.GetControlString(Controls.CONFIRM)}] {_buyButton.Text}";
		_backButton.Text = $"[{Controls.GetControlString(Controls.BACK)}] {_backButton.Text}";
        _buyButton.Pressed += _OnBuyButtonPressed;
        _backButton.Pressed += () => QueueFree();
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _OnBuyButtonPressed();
        }

        if (@event.IsActionPressed(Controls.BACK))
        {
			QueueFree();
        }
    }

    public void SetUIValues()
	{
		var go = PersistentGameObjects.GameObjectInstance();

		var wallet = go.MainPlayer.Wallet;

		int morbis = 0;
		int pickaxes = go.Pickaxes;

		if (wallet.Currency.ContainsKey(SkillAssets.MORBIS))
			morbis = wallet.Currency[SkillAssets.MORBIS].Amount;

		_morbis.Text = morbis.ToString();
		_pickaxes.Text = pickaxes.ToString();
	}

	private void _OnBuyButtonPressed()
	{
        var go = PersistentGameObjects.GameObjectInstance();
        var wallet = go.MainPlayer.Wallet;

        if (wallet.Currency.ContainsKey(SkillAssets.MORBIS))
		{
			var morbis = wallet.Currency[SkillAssets.MORBIS];

			if(morbis.Amount - 1 >= 0)
			{
				morbis.Amount--;
				go.Pickaxes += 3;
				PersistentGameObjects.Save();
			}
        }

		SetUIValues();
    }
}
