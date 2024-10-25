using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.upgrade_screen;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

public partial class UpgradeScreen : CenterContainer
{
	private Label _vorpexCount, _partyCoinCount, _keyCount;
	private ItemList _partyList;
	private Button _backButton, _convertButton, _convertButtonB, _convertButton2, _convertButton2B;
	private int _selected;
	private UpgradeItem _item;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vorpexCount = this.GetNode<Label>("%VorpexCount");
		_partyCoinCount = GetNode<Label>("%OwnedTalismans");
		_keyCount = GetNode<Label>("%KeyCount");
        _partyList = this.GetNode<ItemList>("%ItemList");
        _backButton = this.GetNode<Button>("%BackButton");
        _convertButton = this.GetNode<Button>("%ConvertButton");
        _convertButton2 = this.GetNode<Button>("%ConvertButton2");
        _convertButtonB = this.GetNode<Button>("%Convert2XVCButton");
        _convertButton2B = this.GetNode<Button>("%Convert2XPCButton");
		_backButton.Pressed += _OnBackButtonPressed;
		_item = GetNode<UpgradeItem>("%UpgradeItem");
		_selected = 0;

		_convertButton.Pressed += _OnConvertPressed;
		_convertButtonB.Pressed += _OnConvertPressedB;
		_convertButton2.Pressed += _OnConvertPressed2;
		_convertButton2B.Pressed += _OnConvertPressed2B;

        _item.UpdatePartyDisplay += _OnUpdatePartyDisplay;
		_item.SetVisibility += _OnVisibleChange;

		_partyList.ItemSelected += _OnItemSelected;

		RefreshReserveList();

        var conversionCheckBox = GetNode<CheckBox>("%PCtoVCCheckbox");
        conversionCheckBox.Pressed += () =>
        {
            if (conversionCheckBox.ButtonPressed)
            {
                _convertButton.Visible = false;
                _convertButton2.Visible = false;

                _convertButtonB.Visible = true;
                _convertButton2B.Visible = true;
                conversionCheckBox.Text = "x100";
            }
            else
            {
                _convertButton.Visible = true;
                _convertButton2.Visible = true;

                _convertButtonB.Visible = false;
                _convertButton2B.Visible = false;
                conversionCheckBox.Text = "x1000";
            }
        };
    }

    private void RefreshReserveList()
    {
        _partyList.Clear();

        var go = PersistentGameObjects.GameObjectInstance();
        var mainPlayer = go.MainPlayer;
        var reserves = mainPlayer.ReserveMembers;
        var currency = mainPlayer.Wallet.Currency;

        foreach (var reserve in reserves)
        {
            string name = reserve.DisplayName;
            if (reserve.IsInParty)
                name += " (Party)";

            _partyList.AddItem(name, CharacterImageAssets.GetTextureForItemList(reserve.Image));
        }

        if (_selected >= reserves.Count)
            _selected = reserves.Count - 1;

        _partyList.Select(_selected);
        _item.Initialize(reserves[_selected]);
        _vorpexCount.Text = $"{currency[SkillAssets.VORPEX_ICON].Amount:n0}";
        _partyCoinCount.Text = $"{currency[SkillAssets.PARTY_COIN_ICON].Amount:n0}";

        if (currency.ContainsKey(SkillAssets.BOUNTY_KEY))
            _keyCount.Text = $"{currency[SkillAssets.BOUNTY_KEY].Amount:n0}";

        if (go.ProgressFlagObject.EndgameUnlocked)
            GetNode<HBoxContainer>("%KeyInfo").Visible = true;

    }

	private void _OnConvertPressed()
	{
        ConvertPCToVC(100, 200);
    }

    private void _OnConvertPressedB()
    {
        ConvertPCToVC(1000, 2000);
    }

    private void _OnConvertPressed2()
    {
        ConvertVCToPC(100, 200);
    }

    private void _OnConvertPressed2B()
    {
        ConvertVCToPC(1000, 2000);
    }

    private void ConvertPCToVC(int pcAmount, int vcAmount)
    {
        var mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
        var currency = mainPlayer.Wallet.Currency;
        var pc = currency[SkillAssets.PARTY_COIN_ICON];
        var vc = currency[SkillAssets.VORPEX_ICON];

        if (pc.Amount - pcAmount >= 0)
        {
            pc.Amount -= pcAmount;
            vc.Amount += vcAmount;

            _vorpexCount.Text = $"{currency[SkillAssets.VORPEX_ICON].Amount:n0}";
            _partyCoinCount.Text = $"{currency[SkillAssets.PARTY_COIN_ICON].Amount:n0}";
            PersistentGameObjects.Save();
        }
    }

    private void ConvertVCToPC(int pcAmount, int vcAmount)
    {
        var mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
        var currency = mainPlayer.Wallet.Currency;
        var pc = currency[SkillAssets.PARTY_COIN_ICON];
        var vc = currency[SkillAssets.VORPEX_ICON];

        if (vc.Amount - vcAmount >= 0)
        {
            pc.Amount += pcAmount;
            vc.Amount -= vcAmount;

            _vorpexCount.Text = $"{currency[SkillAssets.VORPEX_ICON].Amount:n0}";
            _partyCoinCount.Text = $"{currency[SkillAssets.PARTY_COIN_ICON].Amount:n0}";
            PersistentGameObjects.Save();
        }
    }

    private void _OnItemSelected(long index)
	{
		_selected = (int)index;
        var reserves = PersistentGameObjects.GameObjectInstance().MainPlayer.ReserveMembers;
		_item.Initialize(reserves[(int)index]);
	}

    private void _OnUpdatePartyDisplay(object sender, EventArgs e)
    {
		RefreshReserveList();
    }

    private void _OnVisibleChange(object sender, bool visible)
    {
        Visible = visible;
    }

    private void _OnBackButtonPressed()
	{
		this.QueueFree();
	}
}
