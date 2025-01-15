using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;

public partial class RewardScreen : Control
{
	private ItemList _rewardsList;
	private Button _claimRewardsButton;
	private List<Currency> _rewards;
    private GameObject _gameObject;
    private int tier;
   
    private const int REWARD_MULTIPLIER = 7;

    private int Multiplier
    {
        get
        {
            if (_gameObject == null)
                return REWARD_MULTIPLIER;

            int t = _gameObject.Tier;
            int multiplier = REWARD_MULTIPLIER - 1;

            return multiplier + (int)((t * 0.05) + 1);
        }
    }

    private Random _rand;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_rewardsList = this.GetNode<ItemList>("%RewardList");
        _claimRewardsButton = this.GetNode<Button>("%ClaimButton");
        _claimRewardsButton.Pressed += _OnClaimRewardsPressed;
        _gameObject = PersistentGameObjects.GameObjectInstance();

        _rand = new Random();

        _claimRewardsButton.Text = $"[{Controls.GetControlString(Controls.CONFIRM)}] {_claimRewardsButton.Text}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _OnClaimRewardsPressed();
        }
    }

    public void InitializeSpecialEnemyRewards(string enemyName)
    {
        Dictionary<string, Currency> importantBosses = new() 
        {
            { EnemyNames.Nettala, new ProofOfAscension(){ Amount = 1} },
            { EnemyNames.Draco, new ProofOfBuce() { Amount = 1 } },
        };

        _rewards = [importantBosses[enemyName]];
        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeSMTRewards()
    {
        tier = _gameObject.Tier;
        int startingValue = 7;
        if(tier > TierRequirements.TIER6_STRONGER_ENEMIES)
            startingValue += tier/5;

        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * GetMultiplier(tier, startingValue) },
            new PartyCoin() { Amount = tier }
        };

        if(tier > TierRequirements.TIER8_STRONGER_ENEMIES)
        {
            _rewards.Add(new Dellencoin() { Amount = GetMultiplier(tier, REWARD_MULTIPLIER) * tier });
        }

        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterRewards()
    {
        tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * GetMultiplier(tier, REWARD_MULTIPLIER) },
            new PartyCoin() { Amount = (tier/2) + 1 },
            new Dellencoin() { Amount =  GetMultiplier(tier,REWARD_MULTIPLIER) * tier },
        };
        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeBountyRewards()
    {
        tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * GetMultiplier(tier, REWARD_MULTIPLIER + 2) },
            new PartyCoin() { Amount = tier },
            new Dellencoin() { Amount =  GetMultiplier(tier, REWARD_MULTIPLIER + 1) * tier },
            new ElderKey() { Amount =1 }
        };
        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterSpecialRewards()
    {
        tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * GetMultiplier(tier, (REWARD_MULTIPLIER - 1)) },
            new PartyCoin() { Amount = tier + 1 },
            new Dellencoin() { Amount = GetMultiplier(tier,REWARD_MULTIPLIER - 3) * tier },
        };
        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterSpecialBossRewards()
    {
        tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = (tier + 10) * GetMultiplier(tier, (REWARD_MULTIPLIER + 5)) },
            new PartyCoin() { Amount = (tier + 10) * GetMultiplier(tier, REWARD_MULTIPLIER) },
            new Dellencoin() { Amount = (tier + 10) * GetMultiplier(tier,REWARD_MULTIPLIER-2) },
        };
        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeDungeonCrawlTierRewards()
    {
        tier = _gameObject.TierDC;
        _rewards = RewardsCalculator.GetDungeonCrawlCompletionRewards(tier);
        LabrybuceTax();
        SetupRewards();
    }

    public void RandomizeDungeonCrawlRewards()
    {
        tier = _gameObject.TierDC;
        var rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * GetMultiplier(tier, (REWARD_MULTIPLIER - 2)) },
            new PartyCoin() { Amount = (tier / 2) + 1 },
            new Dellencoin() { Amount = tier * GetMultiplier(tier, REWARD_MULTIPLIER - 3) },
        };

        _rewards = new List<Currency>() { rewards[_rand.Next(0, rewards.Count)] };
        LabrybuceTax();
        SetupRewards();
    }

    public void InitializeDungeonCrawlSpecialItems()
    {
        tier = _gameObject.TierDC;

        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * GetMultiplier(tier, (REWARD_MULTIPLIER - 2)) },
            new PartyCoin() { Amount = (tier / 2) + 1 },
            new Dellencoin() { Amount = tier * GetMultiplier(tier, REWARD_MULTIPLIER - 3) },
        };

        LabrybuceTax();
        SetupRewards();
    }

    private void LabrybuceTax()
    {
        HashSet<string> currencies = new HashSet<string>() 
        {
            SkillAssets.KEY_SHARD, SkillAssets.BOUNTY_KEY, SkillAssets.ELDER_KEY_ICON,
            SkillAssets.PROOF_OF_ASCENSION_ICON, SkillAssets.PROOF_OF_BUCE_ICON
        };

        double percentage = 0.75 + (tier * 0.05);

        foreach (var reward in _rewards)
        {
            if (!currencies.Contains(reward.Name))
            {
                reward.Amount = (int)(reward.Amount * percentage);
                if (reward.Amount == 0)
                    reward.Amount = 1;
            }

        }
            
    }

    private int GetMultiplier(int tier, int rewardMult)
    {
        if(tier/100 > 0)
        {
            rewardMult *= (tier/100) + 1;
        }

        return rewardMult;
    }

    public void InitializePotOfGreedRewards()
    {
        var currency = _gameObject.MainPlayer.Wallet.Currency;
        var keyShard = new KeyShard() { Amount = 1 };

        if (!currency.ContainsKey(keyShard.Name))
            currency.Add(keyShard.Name, new KeyShard() { Amount = 0 });

        if (currency[keyShard.Name].Amount + 1 >= 4)
        {
            var bountyKey = new BountyKey() { Amount = 1 };

            if (!currency.ContainsKey(bountyKey.Name))
                currency.Add(bountyKey.Name, new BountyKey() { Amount = 0 });

            currency[keyShard.Name].Amount -= 3;

            _rewards = new List<Currency>() { bountyKey };
        }
        else
        {
            _rewards = new List<Currency>() { keyShard };
        }
        SetupRewards();
    }

    private void SetupRewards()
    {
        foreach (Currency reward in _rewards)
        {
            string rewardString = $"{reward.Name} x{reward.Amount:n0}";
            _rewardsList.AddItem(rewardString, SkillAssets.GenerateIcon(reward.Icon));
        }
    }

    private void _OnClaimRewardsPressed()
	{
        var currency = _gameObject.MainPlayer.Wallet.Currency;
        foreach (Currency reward in _rewards)
        {
            if (currency.ContainsKey(reward.Name))
            {
                currency[reward.Name].Amount += reward.Amount;
            }
            else
            {
                currency.Add(reward.Name, reward);
            }
        }

        this.QueueFree();
	}

}
