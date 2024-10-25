using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

public partial class FusionScreen : CenterContainer
{
    private readonly string DEFAULT_SKILL_TRANSFER = "Skills Transferred: ???/???";
    private readonly string DEFAULT_TT = "Combine two party members with the same resistances to get a BUCE-T!";
	private readonly string WARNING_TT = "Both party members must be level";
	
	private PartyMemberDisplay _displayFusion, _material1, _material2;
	private Label _owned, _cost, _tooltip, _skillsTransfered;
	private ItemList _fusionSkillList;
	private Button _fuseButton;
	private FusionScreenObject _fSO;
	private int _selectedIndex = 0;

	private bool _isTransferState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isTransferState = false;

		_displayFusion = this.GetNode<PartyMemberDisplay>("%FusionResult");
        _material1 = this.GetNode<PartyMemberDisplay>("%Material1");
        _material2 = this.GetNode<PartyMemberDisplay>("%Material2");
		_fusionSkillList = this.GetNode<ItemList>("%FusionAndSkillList");
        _fuseButton = this.GetNode<Button>("%FuseButton");
        _owned = GetNode<Label>("%PartyCoinsOwned");
        _cost = GetNode<Label>("%PartyCoinCost");
        _skillsTransfered = GetNode<Label>("%SkillsTransferedLabel");
		_tooltip = GetNode<Label>("%Tooltip");

        Button backButton = this.GetNode<Button>("%BackButton");
		backButton.Pressed += () => 
		{
			if (_isTransferState)
			{
                ReturnToMainFusionScreen();
                _skillsTransfered.Text = DEFAULT_SKILL_TRANSFER;
            }
			else
			{
                this.QueueFree();
            }
		};

		_fSO = new FusionScreenObject();

        _fusionSkillList.ItemSelected += _OnItemSelectedFusion;
        _fuseButton.Pressed += _OnFuseButtonPressed;

        _selectedIndex = 0;
        _owned.Text = $"{_fSO.OwnedPartyCoins:n0}";
        PopulatePossibleFusionList();
    }

	/// <summary>
	/// For filling out the Fusion list if there were any changes to it.
	/// </summary>
	private void PopulatePossibleFusionList()
	{
        _fSO.PopulateMaterialFusionListReserves();

        _fusionSkillList.Clear();

        foreach (FusionObject fusion in _fSO.Fusions)
        {
            OverworldEntity member = fusion.Fusion;
            _fusionSkillList.AddItem(member.DisplayName, CharacterImageAssets.GetTextureForItemList(member.Image));
        }

        if (_fusionSkillList.ItemCount > 0)
        {
            SetFusionSkillListSelectedIndex();
        }

        DisplayFusion();
    }

	/// <summary>
	/// Add skills to be transfered to a fusion.
	/// </summary>
	private void PopulateSkillTransferList()
	{
		// fusion is already displayed
		if (_fSO.Fusions.Count == 0)
        {
            _skillsTransfered.Text = DEFAULT_SKILL_TRANSFER;
            return;
        }

        FusionObject fusion = _fSO.DisplayFusion;

		_fusionSkillList.Clear();

		List<ISkill> skills = _fSO.GetFusionMaterialSkills();

        foreach (var skill in skills)
		{
			string skillDisplayString = skill.GetBattleDisplayString();

			if (fusion.Fusion.Skills.Contains(skill))
				skillDisplayString += " [TRANSFERRED]";

            _fusionSkillList.AddItem(skillDisplayString, SkillAssets.GenerateIcon(skill.Icon));
        }

        
        _skillsTransfered.Text = $"Skills Transferred: {fusion.Fusion.Skills.Count}/{fusion.Fusion.SkillCap}";
    }

	/// <summary>
	/// For displaying a specific fusion on screen + its materials
	/// </summary>
	/// <param name="index"></param>
	private void DisplayFusion()
	{
		if (_fSO.Fusions.Count == 0)
		{
			_displayFusion.Clear();
			_material1.Clear();
			_material2.Clear();
            return;
        }

        FusionObject fusion = _fSO.DisplayFusion;

		var fusionHolder = new EntityUIWrapper { Entity = fusion.Fusion };
		var mat1Holder   = new EntityUIWrapper { Entity = fusion.Material1 };
		var mat2Holder   = new EntityUIWrapper { Entity = fusion.Material2 };

		_displayFusion.ShowFusionEntity(fusionHolder);
        _material1.ShowRandomEntity(mat1Holder);
		_material2.ShowRandomEntity(mat2Holder);
		if(_fSO.IsCorrectTier())
		{
            if (_fSO.IsCorrectFusionLevel())
            {
                _tooltip.Text = DEFAULT_TT;
                _fuseButton.Disabled = false;
            }
            else
            {
                _tooltip.Text = $"{WARNING_TT} {TierRequirements.GetFusionTierRequirement(fusion.Fusion.FusionGrade)}!";
                _fuseButton.Disabled = true;
            }
        }
        else
        {
			_tooltip.Text = $"Your Endless Dungeon Tier must be greater than {TierRequirements.GetFusionTierRequirement(fusion.Fusion.FusionGrade)} to fuse!";
            _fuseButton.Disabled = true;
        }


        UpdateCost();
    }

	private void UpdateCost()
    {
        int cost = _fSO.GetCost();
        string costStr = "Cost: ";
        if (cost == -1)
            costStr += "???";
        else
            costStr += $"{cost:n0}";

        _cost.Text = costStr;
        _owned.Text = $"{_fSO.OwnedPartyCoins:n0}";
    }

	private void SetFusionSkillListSelectedIndex()
	{
		if (_selectedIndex >= _fusionSkillList.ItemCount)
			_selectedIndex = _fusionSkillList.ItemCount - 1;

        _fusionSkillList.Select(_selectedIndex);
    }

	private void _OnItemSelectedFusion(long selected)
	{
        _selectedIndex = (int)selected;

        if (!_isTransferState)
		{
            _fSO.FusionIndex = _selectedIndex;
        }	
		else
		{
            _fSO.AddOrRemoveSkillFromFusion(_selectedIndex);
            PopulateSkillTransferList();
        }

        DisplayFusion();
    }

	private void _OnFuseButtonPressed()
	{
		if (_fSO.Fusions.Count == 0)
			return;

		if (!_isTransferState)
		{
            _isTransferState = true;

            PopulateSkillTransferList();

            _fuseButton.Text = "Confirm";
        }
		else
		{
			bool successfulFusion = _fSO.TryFuse();

			if (successfulFusion)
			{
                ReturnToMainFusionScreen();
                UpdateCost();
                _skillsTransfered.Text = DEFAULT_SKILL_TRANSFER;
            }
				
        }
    }

	private void ReturnToMainFusionScreen()
	{
        _isTransferState = false;

        PopulatePossibleFusionList();

        _fuseButton.Text = "Fuse";
    }
}
