using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;

public partial class RecruitScreen : CenterContainer
{
    private int _cost = 1;
    private ItemList _availableRecruits;
    private Label _partyCoinCost;
    private List<OverworldEntity> _availablePartyMembers;

    private PartyMemberDisplay _partyMemberDisplay;
    private Currency _partyCoins;
    private int _selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _availableRecruits = this.GetNode<ItemList>("%PartyMemberList");
        _partyCoinCost = this.GetNode<Label>("%OwnedTalismans");
        _partyMemberDisplay = this.GetNode<PartyMemberDisplay>("%PartyMemberDisplay");

        Button buyButton = this.GetNode<Button>("%BuyButton");

        buyButton.Pressed += _OnBuyButtonPressed;
        _availableRecruits.ItemSelected += _OnItemSelected;
        _partyCoins = PersistentGameObjects.GameObjectInstance().MainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON];
        SetShopVendorWares();
    }

    public void SetOwnedPartyCoin()
    {
        _partyCoinCost.Text = $"{_partyCoins.Amount:n0} PC";
    }

    public void SetShopVendorWares()
    {
        _availablePartyMembers = EntityDatabase.MakeShopVendorWares(PersistentGameObjects.GameObjectInstance());
        _availablePartyMembers.Reverse();

        int shopLevel = PersistentGameObjects.GameObjectInstance().ShopLevel;
        for (int i = 0; i < shopLevel; i++)
        {
            foreach (var member in _availablePartyMembers) 
            {
                member.LevelUp();
            }
        }

        _cost = (int)(shopLevel * 1.5) + 1;

        RefreshVendorWares(0);
    }

    private void _OnItemSelected(long index)
    {
        _selected = (int)index;
        DisplayPartyMemberOnScreen(_selected);
    }

    private void _OnBuyButtonPressed()
    {
        if (_availablePartyMembers.Count == 0)
            return;

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
        MainPlayer mainPlayer = gameObject.MainPlayer;

        OverworldEntity partyMember = _availablePartyMembers[_selected];

        if (_partyCoins.Amount >= _cost)
        {
            if (mainPlayer.IsPartyMemberOwned(partyMember.Name))
                return;

            _partyCoins.Amount -= _cost;

            mainPlayer.ReserveMembers.Add(partyMember);

            RefreshVendorWares(_selected);

            PersistentGameObjects.Save();
        }
    }

    private void RefreshVendorWares(int lastSelected)
    {
        _availableRecruits.Clear();

        MainPlayer mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
        foreach(OverworldEntity availablePartyMember in _availablePartyMembers)
        {
            string owned = string.Empty;
            if (mainPlayer.IsPartyMemberOwned(availablePartyMember.Name))
                owned = " [OWNED]";

            _availableRecruits.AddItem($"{availablePartyMember.DisplayName} - {_cost} PC{owned}", CharacterImageAssets.GetTextureForItemList(availablePartyMember.Image));
        }

        if(_availablePartyMembers.Count == 0)
        {
            _partyMemberDisplay.Clear();
        }
        else
        {
            DisplayPartyMemberOnScreen(lastSelected);
        }

        _partyCoinCost.Text = $"{_partyCoins.Amount:n0} PC";
    }

    private void DisplayPartyMemberOnScreen(int index)
    {
        OverworldEntity member = _availablePartyMembers[index];
        _partyMemberDisplay.ShowRandomEntity(new AscendedZ.screens.back_end_screen_scripts.EntityUIWrapper() { Entity = member });
    }
}
