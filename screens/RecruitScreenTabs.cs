using AscendedZ;
using AscendedZ.game_object;
using Godot;
using System;

public partial class RecruitScreenTabs : CenterContainer
{
	private const int MEMBER_REQUEST_FORUM = 1;
	private Button _shopLevelButton;
	private Label _ownedDellencoin;

	private RecruitScreen _recruitShop;
	private RecruitCustomScreen _memberRequestForum;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TabContainer tabContainer = this.GetNode<TabContainer>("%TabContainer");
		tabContainer.SetTabHidden(MEMBER_REQUEST_FORUM, true);

		_shopLevelButton = this.GetNode<Button>("%UpgradeButton");
		_shopLevelButton.Pressed += _OnShopUpgradePressed;

		_ownedDellencoin = this.GetNode<Label>("%OwnedDellencoin");

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _recruitShop = tabContainer.GetNode<RecruitScreen>("%Recruit Shop");
		_memberRequestForum = tabContainer.GetNode<RecruitCustomScreen>("%Member Request Forum");
		_memberRequestForum.SkillNotification = GetNode<Node2D>("%SkillNotification");
		_memberRequestForum.Initialize();

        var backButtonRecruitShop = _recruitShop.GetNode<Button>("%BackButton");
		backButtonRecruitShop.Pressed += QueueFree;

		tabContainer.TabChanged += (newTab) => 
		{ 
			if(newTab > 0 && !gameObject.ProgressFlagObject.CustomPartyMembersViewed)
			{
				gameObject.ProgressFlagObject.CustomPartyMembersViewed = true;
				PersistentGameObjects.Save();
            }

			_recruitShop.SetOwnedPartyCoin();
			_memberRequestForum.SetOwnedPartyCoin();
        };

		if(gameObject.MaxTier > TierRequirements.TIER2_STRONGER_ENEMIES)
		{
			tabContainer.SetTabHidden(MEMBER_REQUEST_FORUM, false);
            _memberRequestForum.BackOut += _OnBackButtonPressed;
        }

		if(gameObject.ShopLevel >= PersistentGameObjects.SHOP_CAP)
		{
			_shopLevelButton.Disabled = true;
            _shopLevelButton.Text = "MAX LEVEL";
        }

		_ownedDellencoin.Text = $"{gameObject.MainPlayer.Wallet.Currency[SkillAssets.DELLENCOIN].Amount:n0} D$";
    }

	private void _OnShopUpgradePressed()
	{
		int cost = 2000;
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
		var wallet = gameObject.MainPlayer.Wallet;
		if (wallet.Currency[SkillAssets.DELLENCOIN].Amount - cost >= 0) 
		{
			wallet.Currency[SkillAssets.DELLENCOIN].Amount -= cost;
			gameObject.ShopLevel++;

			_recruitShop.SetShopVendorWares();
			_memberRequestForum.SetShopVendorWares();

            _ownedDellencoin.Text = $"{gameObject.MainPlayer.Wallet.Currency[SkillAssets.DELLENCOIN].Amount:n0} D$";

			if(gameObject.ShopLevel >= PersistentGameObjects.SHOP_CAP)
			{
				_shopLevelButton.Disabled = true;
				_shopLevelButton.Text = "MAX LEVEL";
			}
        }
    }

    private void _OnBackButtonPressed(object sender, EventArgs e)
    {
        this.QueueFree();
    }
}
