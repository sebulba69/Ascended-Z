using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.entities.sigils;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using AscendedZ.screens.upgrade_screen;
using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class UpgradeItem : VBoxContainer
{
	private readonly PackedScene SIGIL_ICON = ResourceLoader.Load<PackedScene>(Scenes.SIGIL_ICON);
	private GameObject _go;
	private TextureRect _image;
	private Label _name, _upgradeCost, _refundRewardPC, _refundRewardVC;
	private RichTextLabel _description;
	private Button _upgradeBtn, _refundBtn, _reorderBtn, _sigilButton, _skillBoostButton;
	private bool _mouseOver;
	private HBoxContainer _sigilDisplay, _sigilBucetMenu;
	private PanelContainer _lockDisplay;
	
	private UpgradeItemObject _upgradeItemObject;

	public EventHandler UpdatePartyDisplay;
	public EventHandler<bool> SetVisibility;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_image = GetNode<TextureRect>("%Picture");
		_name = GetNode<Label>("%Name");
		_upgradeBtn = GetNode<Button>("%UpgradeButton");
		_refundBtn = GetNode<Button>("%RefundButton");
		_reorderBtn = GetNode<Button>("%ReorderSkillButton");
		_sigilButton = GetNode<Button>("%SigilButton");
        _skillBoostButton = GetNode<Button>("%SkillUpgradeButton");
        _upgradeCost = GetNode<Label>("%VCCost");
		_refundRewardPC = GetNode<Label>("%PCCost");
        _refundRewardVC = GetNode<Label>("%VPGain");
		_description = GetNode<RichTextLabel>("%Description");

		_sigilDisplay = GetNode<HBoxContainer>("%SigilDisplay");
		_lockDisplay = GetNode<PanelContainer>("%LockedNotification");
		_sigilBucetMenu = GetNode<HBoxContainer>("%SigilMenu");

        _go = PersistentGameObjects.GameObjectInstance();
        _upgradeItemObject = new UpgradeItemObject(_go);

        _upgradeBtn.Pressed += _OnUpgradeButtonPressed;
		_refundBtn.Pressed += _OnRefundButtonPressed;
		_reorderBtn.Pressed += _OnReorderBtnPressed;
		_sigilButton.Pressed += _OnSigilButtonPressed;
		_skillBoostButton.Pressed += _OnIndividualSkillUpgradeButton;
    }

	public void Initialize(OverworldEntity entity)
	{
		_upgradeItemObject.Initialize(entity);
        _image.Texture = ResourceLoader.Load<Texture2D>(entity.Image);
		UpdateDisplay();
    }

	public void UpdateDisplay()
	{
		var entity = _upgradeItemObject.Entity;

		_name.Text = entity.DisplayName;
		_upgradeCost.Text = $"{entity.VorpexValue:n0}";
		_refundRewardPC.Text = $"{entity.RefundReward:n0}";
		_refundRewardVC.Text = $"{entity.RefundRewardVC:n0}";
        _description.Text = entity.GetUpgradeString();
        _upgradeBtn.Text = "Upgrade";

        if (entity.IsLevelCapHit)
		{
			_upgradeBtn.Disabled = true;
			if (_go.ProgressFlagObject.EndgameUnlocked)
			{
                _upgradeBtn.Disabled = false;
				_upgradeBtn.Text = "Boost HP";
                _upgradeCost.Text = "1";
				GetNode<PanelContainer>("%VorpexCost").Visible = false;
				GetNode<PanelContainer>("%KeyCost").Visible = true;
				_upgradeBtn.Disabled = !entity.CanBoostHP();
            }
        }
		else
		{
            int softCap = MiscGlobals.GetSoftcap();
            if (entity.Level + 1 >= softCap)
            {
                _upgradeBtn.Disabled = true;
				_upgradeBtn.Text = $"Capped: {softCap}";
			}
			else
			{
                _upgradeBtn.Disabled = false;
            }
        }
		
		bool endgameVisible = (_upgradeItemObject.Entity.Level == 150 && _upgradeItemObject.Entity.FusionGrade == MiscGlobals.FUSION_GRADE_CAP);
        _lockDisplay.Visible = !endgameVisible;
        _sigilBucetMenu.Visible = endgameVisible;
        _refundBtn.Disabled = !_upgradeItemObject.CanRefund();

		foreach(var child in _sigilDisplay.GetChildren())
		{
			_sigilDisplay.RemoveChild(child);
		}

		foreach(var sigil in _upgradeItemObject.Entity.Sigils)
		{
			if(sigil.Index > -1)
			{
                var icon = SIGIL_ICON.Instantiate<SigilItemIcon>();
                _sigilDisplay.AddChild(icon);
                icon.SetPic(sigil.Image);
            }
        }
    }

	private async void _OnReorderBtnPressed()
	{
		var reorder = ResourceLoader.Load<PackedScene>(Scenes.REORDER).Instantiate<SkillReorderControl>();
		GetTree().Root.AddChild(reorder);
        SetVisibility?.Invoke(null, false);

        reorder.SetStartList(_upgradeItemObject.Entity);

		await ToSignal(reorder, "tree_exited");

        UpdateDisplay();

        SetVisibility?.Invoke(null, true);
    }

    private async void _OnSigilButtonPressed()
    {
        var sigilButton = ResourceLoader.Load<PackedScene>(Scenes.SIGIL_SCREEN).Instantiate<SigilScreen>();
        GetTree().Root.AddChild(sigilButton);

		sigilButton.SetEntity(_upgradeItemObject.Entity);

        SetVisibility?.Invoke(null, false);
        await ToSignal(sigilButton, "tree_exited");
        UpdateDisplay();
        SetVisibility?.Invoke(null, true);
    }

    private async void _OnIndividualSkillUpgradeButton()
    {
        var individualSkillUpgradeButton = ResourceLoader.Load<PackedScene>(Scenes.INDIVIDUAL_UPGRADE_SCREEN).Instantiate<SkillUpgradeUI>();
        GetTree().Root.AddChild(individualSkillUpgradeButton);

        individualSkillUpgradeButton.SetPartyMember(_upgradeItemObject.Entity);

        SetVisibility?.Invoke(null, false);
        await ToSignal(individualSkillUpgradeButton, "tree_exited");
        UpdateDisplay();
        UpdatePartyDisplay?.Invoke(null, EventArgs.Empty);
        SetVisibility?.Invoke(null, true);
    }

    private void _OnUpgradeButtonPressed()
	{
		if (_go.ProgressFlagObject.EndgameUnlocked)
		{
            _upgradeItemObject.UpgradeHP();
        }
		else
		{
            _upgradeItemObject.Upgrade();
        }

        UpdateDisplay();
        UpdatePartyDisplay?.Invoke(null, EventArgs.Empty);
	}

    private void _OnRefundButtonPressed()
    {
		_upgradeItemObject.Refund();
        UpdatePartyDisplay?.Invoke(null, EventArgs.Empty);
    }
}
