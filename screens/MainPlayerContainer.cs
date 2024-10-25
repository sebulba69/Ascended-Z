using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;

public partial class MainPlayerContainer : VBoxContainer
{
    private PackedScene _currencyDisplay = ResourceLoader.Load<PackedScene>(Scenes.CURRENCY_DISPLAY);

    public void InitializePlayerInformation(GameObject gameObject, bool isDungeonCrawling = false)
	{
        TextureRect playerPicture = this.GetNode<TextureRect>("%PlayerPicture");
        Label playerName = this.GetNode<Label>("%PlayerNameLabel");

        MainPlayer player = gameObject.MainPlayer;
        playerPicture.Texture = ResourceLoader.Load<Texture2D>(player.Image);
        int tier = gameObject.MaxTier;

        if(tier == gameObject.TierCap)
        {
            playerName.Text = $"[MAX] {player.Name}";
        }
        else
        {
            playerName.Text = $"[T. {tier}] {player.Name}";
        }
        

        UpdateCurrencyDisplay();
    }

    public void UpdatePlayerPic(string pic)
    {
        GetNode<TextureRect>("%PlayerPicture").Texture = ResourceLoader.Load<Texture2D>(pic);
    }

    public void UpdateCurrencyDisplay()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        var currencyDisplay = this.GetNode("%Currency");
        var wallet = gameObject.MainPlayer.Wallet;

        foreach (var child in currencyDisplay.GetChildren())
        {
            currencyDisplay.RemoveChild(child);
        }

        var currencyDict = wallet.Currency;

        List<Currency> currencyList = [currencyDict[SkillAssets.VORPEX_ICON], currencyDict[SkillAssets.PARTY_COIN_ICON], currencyDict[SkillAssets.DELLENCOIN]];

        if (currencyDict.ContainsKey(SkillAssets.BOUNTY_KEY))
            currencyList.Add(currencyDict[SkillAssets.BOUNTY_KEY]);

        if (currencyDict.ContainsKey(SkillAssets.ELDER_KEY_ICON))
            currencyList.Add(currencyDict[SkillAssets.ELDER_KEY_ICON]);

        if (currencyDict.ContainsKey(SkillAssets.PROOF_OF_ASCENSION_ICON))
            currencyList.Add(currencyDict[SkillAssets.PROOF_OF_ASCENSION_ICON]);

        if (currencyDict.ContainsKey(SkillAssets.PROOF_OF_BUCE_ICON))
            currencyList.Add(currencyDict[SkillAssets.PROOF_OF_BUCE_ICON]);

        foreach (var currency in currencyList)
        {
            var display = _currencyDisplay.Instantiate<CurrencyDisplay>();
            currencyDisplay.AddChild(display);
            display.SetCurrencyToDisplay(currency.Icon, currency);
        }
    }
}
