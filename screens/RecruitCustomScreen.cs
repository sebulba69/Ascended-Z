using AscendedZ;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class RecruitCustomScreen : CenterContainer
{
    private readonly string BUY_TEXT = "Buy";
    private readonly string SELECT_PARTY_MEMBER = "Select Member";
    private readonly string MAX_NUMBER_OF_SKILLS = "Max Number of Skills:";
	private Node _partyMemberDisplay;
	private ItemList _itemList;
	private Label _ownedPartyCoin;
	private Label _costLabel, _skillNumber, _description;
    private Button _buyButton, _backButton;
    private CheckBox _previewSkills;

    private int _selectedIndex = 0;

	private RecruitCustomObject _recruitCustomObject;
	private GameObject _gameObject;
    private List<CheckBox> _checkBoxes;
    public EventHandler BackOut;

    public Node2D SkillNotification { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_partyMemberDisplay = this.GetNode("%PartyMemberDisplay");
		_itemList = this.GetNode<ItemList>("%ItemList");
		_ownedPartyCoin = this.GetNode<Label>("%OwnedPartyCoin");
        _costLabel = this.GetNode<Label>("%CostLabel");
        _description = this.GetNode<Label>("%Description");
        _skillNumber = this.GetNode<Label>("%SkillNumber");
        _previewSkills = GetNode<CheckBox>("%PreviewSkills");

        _itemList.ItemSelected += _OnItemSelected;

        _gameObject = PersistentGameObjects.GameObjectInstance();

        _recruitCustomObject = new RecruitCustomObject();
		_recruitCustomObject.Initialize();
        _recruitCustomObject.SetPreviewPartyMember(_selectedIndex);

        ChangePotentialPartyMembers();

        _buyButton = this.GetNode<Button>("%BuyButton");
        _backButton = this.GetNode<Button>("%BackButton");

        _buyButton.Pressed += _OnBuyButtonPressed;
        _backButton.Pressed += _OnBackButtonPressed;
        _previewSkills.Pressed += _OnPreviewPressed;

        _buyButton.Text = SELECT_PARTY_MEMBER;

        _itemList.ItemClicked += _OnMenuItemClicked;

        _ownedPartyCoin.Text = $"{_gameObject.MainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON].Amount:n0} PC";

        List<Action> functions = [_OnF0Checked, _OnF1Checked, _OnF2Checked, _OnF3Checked, _OnF4Checked, _OnF5Checked, _OnF6Checked];
        _checkBoxes = new List<CheckBox>();

        for (int i = 0; i < _gameObject.Checkboxes.Count; i++)
        {
            _checkBoxes.Add(GetNode<CheckBox>($"%F{i}"));
            _checkBoxes[i].ButtonPressed = _gameObject.Checkboxes[i];
            _checkBoxes[i].Pressed += functions[i];
        }
    }

    public void Initialize()
    {
        DoPreviewCheck();
    }

    private void DoPreviewCheck()
    {
        int tier = _gameObject.MaxTier;
        var progressTiers = _gameObject.ProgressFlagObject.SKILL_PROGRESS_TIERS;
        _previewSkills.Text = "Preview Skills";
        SkillNotification.Visible = false;
        foreach (int pT in progressTiers)
        {
            if (tier > pT && !_gameObject.ProgressFlagObject.ViewedSkillProgressTiers.Contains(pT))
            {
                _previewSkills.Text = "❗ Preview Skills ❗";
                SkillNotification.Visible = true;
                break;
            }
        }
    }

    private void BuyPartyMember()
    {
        var mainPlayer = _gameObject.MainPlayer;
        var partyCoin = mainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON];
        var selected = _recruitCustomObject.SelectedEntity;

        bool canAfford = (partyCoin.Amount >= _recruitCustomObject.Cost);
        bool isOwnedByPlayer = (mainPlayer.IsPartyMemberOwned(selected.Name));
        bool hasSkills = _recruitCustomObject.SelectedEntity.Skills.Count > 0;
        bool isOfLevel = _recruitCustomObject.SelectedEntity.Level >= (_recruitCustomObject.SelectedEntity.FusionGrade * 10);

        if (canAfford && !isOwnedByPlayer && hasSkills && isOfLevel)
        {
            partyCoin.Amount -= _recruitCustomObject.Cost;

            // prevent any references in memory back to this screen
            var partyMember = _recruitCustomObject.BuyAndReturnSelected();
            mainPlayer.ReserveMembers.Add(partyMember);
            _OnBackButtonPressed();

            PersistentGameObjects.Save();
        }
        else
        {
            GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
        }

        _ownedPartyCoin.Text = $"{partyCoin.Amount:n0} PC";
    }

    public void SetShopVendorWares()
    {
		_recruitCustomObject.Initialize();

        _itemList.Clear();
        if(_buyButton.Text == SELECT_PARTY_MEMBER)
            ChangePotentialPartyMembers();
        else
            ChangePotentialSkills(_previewSkills.ButtonPressed);
    }

	public void SetOwnedPartyCoin()
	{
        _ownedPartyCoin.Text = $"{_gameObject.MainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON].Amount:n0} PC";
    }

    private void ChangePotentialPartyMembers()
	{
		_itemList.Clear();

		var mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;

        foreach (var availablePartyMember in _recruitCustomObject.DisplayMembers)
        {
            string owned = string.Empty;
            if (mainPlayer.IsPartyMemberOwned(availablePartyMember.Name))
                owned = " [OWNED]";

            _itemList.AddItem($"{availablePartyMember.DisplayName}{owned}", CharacterImageAssets.GetTextureForItemList(availablePartyMember.Image));
        }

		if (_selectedIndex >= _itemList.ItemCount)
			_selectedIndex = _itemList.ItemCount - 1;

        if (_itemList.ItemCount > 0)
        {
            if(_selectedIndex < 0)
                _selectedIndex = 0;

            _itemList.Select(_selectedIndex);
            _recruitCustomObject.SetPreviewPartyMember(_selectedIndex);
        }

        ShowPreviewPartyMember();
    }

    private void ChangePotentialSkills(bool preview)
    {
        var skills = (!preview) ? _recruitCustomObject.AvailableSkills : _recruitCustomObject.AllSkills;
        foreach (var skill in skills)
            _itemList.AddItem(skill.Name, SkillAssets.GenerateIcon(skill.Icon));

        if (_selectedIndex >= _itemList.ItemCount)
            _selectedIndex = _itemList.ItemCount - 1;

        _itemList.Select(_selectedIndex);
    }

    private void _OnPreviewPressed()
    {
        if (_previewSkills.ButtonPressed)
        {
            var gameObject = PersistentGameObjects.GameObjectInstance();
            int tier = gameObject.MaxTier;
            var progressTiers = gameObject.ProgressFlagObject.SKILL_PROGRESS_TIERS;
            foreach (int pT in progressTiers)
            {
                if (tier > pT && !gameObject.ProgressFlagObject.ViewedSkillProgressTiers.Contains(pT))
                    gameObject.ProgressFlagObject.ViewedSkillProgressTiers.Add(pT);
            }

            PersistentGameObjects.Save();
            _previewSkills.Text = "Preview Skills";
            SkillNotification.Visible = false;

            _itemList.Clear();
            _itemList.MaxColumns = 2;
            foreach (var cb in _checkBoxes)
                cb.Disabled = true;
            _OnItemSelected(0);
            _description.Visible = true;
            ChangePotentialSkills(true);
        }
        else 
        {
            _itemList.Clear();
            _itemList.MaxColumns = 1;
            ResetPartyMembers();
        }
    }

    private void _OnBuyButtonPressed()
    {
        if (_itemList.ItemCount == 0)
            return;

        var mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
        if (mainPlayer.IsPartyMemberOwned(_recruitCustomObject.SelectedEntity.Name)
            || _previewSkills.ButtonPressed)
        {
            GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
            return;
        }

        if (_buyButton.Text == SELECT_PARTY_MEMBER)
        {
            _previewSkills.Disabled = true;
            _itemList.Clear();

            foreach (var cb in _checkBoxes)
                cb.Disabled = true;

            _buyButton.Text = BUY_TEXT;
            ChangePotentialSkills(false);
            _description.Visible = true;
            _OnItemSelected(_selectedIndex);
            _itemList.MaxColumns = 2;
        }
        else
        {
            BuyPartyMember();
        }
    }

    private void _OnItemSelected(long index)
	{
        if (_buyButton.Text == SELECT_PARTY_MEMBER && !_previewSkills.ButtonPressed)
        {
            _selectedIndex = (int)index;
            _recruitCustomObject.SetPreviewPartyMember(_selectedIndex);

            ShowPreviewPartyMember();
        }
        else
        {
            _selectedIndex = (int)index;
            if (_previewSkills.ButtonPressed)
            {
                _description.Text = _recruitCustomObject.AllSkills[_selectedIndex].Description;
            }
            else
            {
                _description.Text = _recruitCustomObject.GetSkillDescription(_selectedIndex);
            }
        }
    }

    private void _OnBackButtonPressed()
    {
        if (_buyButton.Text != SELECT_PARTY_MEMBER)
        {
            ResetPartyMembers();
        }
        else
        {
            if(_previewSkills.ButtonPressed)
            {
                _previewSkills.ButtonPressed = false;
                _OnPreviewPressed();
            }
            else
            {
                BackOut?.Invoke(null, EventArgs.Empty);
            }
        }
    }

    private void ResetPartyMembers()
    {
        _previewSkills.Disabled = false;
        _itemList.MaxColumns = 1;
        _buyButton.Text = SELECT_PARTY_MEMBER;
        _description.Visible = false;
        _recruitCustomObject.SelectedEntity.Skills.Clear();
        ChangePotentialPartyMembers();

        foreach (var cb in _checkBoxes)
            cb.Disabled = false;
    }

    private void _OnF0Checked()
    {
        OnFGradeChecked(0);
    }

    private void _OnF1Checked()
    {
        OnFGradeChecked(1);
    }

    private void _OnF2Checked()
    {
        OnFGradeChecked(2);
    }

    private void _OnF3Checked()
    {
        OnFGradeChecked(3);
    }

    private void _OnF4Checked()
    {
        OnFGradeChecked(4);
    }

    private void _OnF5Checked()
    {
        OnFGradeChecked(5);
    }

    private void _OnF6Checked()
    {
        OnFGradeChecked(6);
    }

    private void OnFGradeChecked(int g)
    {
        var button = GetNode<CheckBox>($"%F{g}");
        _gameObject.Checkboxes[g] = button.ButtonPressed;
        _recruitCustomObject.FilterResults();

        ChangePotentialPartyMembers();
        PersistentGameObjects.Save();
    }

    private void _OnMenuItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (_buyButton.Text != SELECT_PARTY_MEMBER && mouse_button_index == (long)MouseButton.Right)
        {
            _OnItemSelected(index);
            _recruitCustomObject.SetSkill((int)index);
            ShowPreviewPartyMember();
            UpdateCost();
        }
    }

    private void ShowPreviewPartyMember()
    {
        if (_itemList.ItemCount > 0)
        {
            _skillNumber.Text = $"{MAX_NUMBER_OF_SKILLS} {_recruitCustomObject.SelectedEntity.SkillCap}";
            _partyMemberDisplay.Call("Clear");
            _partyMemberDisplay.Call("ShowRandomEntity", new EntityUIWrapper { Entity = _recruitCustomObject.SelectedEntity });
            UpdateCost();
        }
        else 
        {
            _partyMemberDisplay.Call("Clear");
        }
    }

	private void UpdateCost()
	{
		_costLabel.Text = $"Cost: {_recruitCustomObject.Cost:n0} PC";
	}
}
