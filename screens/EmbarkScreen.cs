using AscendedZ;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Godot.WebSocketPeer;

/// <summary>
/// This class is focused on managing Reserve Party members.
/// How the UI displays characters *in* the party is handled in InPartyMemberContainer.cs.
/// </summary>
public partial class EmbarkScreen : TextureRect
{
    private Label _tierLabel;
    private PartyEditScreen _partyEditScreen;
    private Button _endlessDungeonBtn, _endAscendedButton, _labrybuceBtn, _rightBtn, _right10Btn, _leftTier, _left10Btn;
    private GameObject _gameObject;
    private AudioStreamPlayer _itemSfx;

    public bool Embark { get; set; }

    public bool DungeonCrawling { get => _partyEditScreen.DungeonCrawling; }

    public override void _Ready()
    {
        AddUserSignal("CloseEmbarkScreen");

        _gameObject = PersistentGameObjects.GameObjectInstance();

        _tierLabel = this.GetNode<Label>("%TierLabel");

        Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetBackground(_gameObject.MaxTier, _gameObject.ProgressFlagObject.EndgameUnlocked));

        _itemSfx = GetNode<AudioStreamPlayer>("%ItemSfx");

        _leftTier = this.GetNode<Button>("%LeftTierBtn");
        _rightBtn = this.GetNode<Button>("%RightTierBtn");
        _left10Btn = this.GetNode<Button>("%LeftTier10Btn");
        _right10Btn = this.GetNode<Button>("%RightTier10Btn");
        _endlessDungeonBtn = this.GetNode<Button>("%EndlessDungeonBtn");
        _labrybuceBtn = this.GetNode<Button>("%LabribuceBtn");
        _endAscendedButton = GetNode<Button>("%EndAscendedButton");
        _labrybuceBtn.Visible = (_gameObject.MaxTier >= 10);
        _gameObject.EndAscended = false;

        // on click events
        _left10Btn.Pressed += () => 
        {
            for (int i = 0; i < 10; i++)
                _OnLeftBtnPressed();
        };

        _right10Btn.Pressed += () =>
        {
            for (int i = 0; i < 10; i++)
                _OnRightBtnPressed();
        };

        _leftTier.Pressed += _OnLeftBtnPressed;
        _rightBtn.Pressed += _OnRightBtnPressed;
        
        _labrybuceBtn.Pressed += _OnLabrybuceButtonPressed;
        _endlessDungeonBtn.Pressed += _OnEndlessDungeonButtonPressed;
        _endAscendedButton.Pressed+= _OnEndAscendedButtonPressed;
        _partyEditScreen = this.GetNode<PartyEditScreen>("%PartyEditScreen");
        _partyEditScreen.DoEmbark += _OnEmbarkPressed;

        _gameObject.Tier = _gameObject.MaxTier;
        _gameObject.TierDC = _gameObject.MaxTierDC;
        SetTierText(_gameObject.Tier);

        if(_gameObject.MaxTier == 301)
        {
            _endAscendedButton.Visible = true;
        }

        SetArrowVisibility();
    }

    private void _OnEmbarkPressed(object sender, bool embarkPressed)
    {
        Embark = embarkPressed;
        Visible = false;
        EmitSignal("CloseEmbarkScreen");
    }

    private void _OnLabrybuceButtonPressed()
    {
        _endlessDungeonBtn.Disabled = false;
        _labrybuceBtn.Disabled = true;
        _endAscendedButton.Disabled = false;
        _gameObject.EndAscended = false;
        SetArrowVisibilityDC();

        SetTierText(_gameObject.TierDC);
        _partyEditScreen.DungeonCrawling = true;
    }

    private void _OnEndlessDungeonButtonPressed()
    {
        _endlessDungeonBtn.Disabled = true;
        _labrybuceBtn.Disabled = false;
        _endAscendedButton.Disabled = false;
        _gameObject.EndAscended = false;
        SetArrowVisibility();

        SetTierText(_gameObject.Tier);
        _partyEditScreen.DungeonCrawling = false;
    }

    private void _OnEndAscendedButtonPressed()
    {
        _endlessDungeonBtn.Disabled = false;
        _labrybuceBtn.Disabled = false;
        _endAscendedButton.Disabled = true;
        _gameObject.EndAscended = true;

        _right10Btn.Visible = false;
        _rightBtn.Visible = false;
        _leftTier.Visible = false;
        _left10Btn.Visible = false;

        _tierLabel.Text = "Complete Your Journey";
        _partyEditScreen.DungeonCrawling = false;
    }

    private void _OnLeftBtnPressed()
    {
        if (_labrybuceBtn.Disabled) 
        {
            _gameObject.TierDC--;
            SetTierText(_gameObject.TierDC);

            SetArrowVisibilityDC();
        }
        else 
        {
            _gameObject.Tier--;
            SetTierText(_gameObject.Tier);

            SetArrowVisibility();
        }
    }

    private void _OnRightBtnPressed() 
    {
        if (_labrybuceBtn.Disabled)
        {
            _gameObject.TierDC++;
            SetTierText(_gameObject.TierDC);

            SetArrowVisibilityDC();
        }
        else
        {
            _gameObject.Tier++;
            SetTierText(_gameObject.Tier);

            SetArrowVisibility();
        }
    }

    private void SetArrowVisibility()
    {
        if (_gameObject.Tier == _gameObject.MaxTier)
        {
            if (_gameObject.Tier + 10 <= _gameObject.MaxTier)
            {
                _right10Btn.Visible = true;
            }
            else
            {
                _right10Btn.Visible = false;
            }

            _rightBtn.Visible = false;
        }
        else
        {
            _right10Btn.Visible = true;
            _rightBtn.Visible = true;
        }
        
        if(_gameObject.Tier == 1)
        {
            _leftTier.Visible = false;
            _left10Btn.Visible = false;
        }
        else
        {
            _leftTier.Visible = true;
            if (_gameObject.Tier - 10 > 0)
            {
                _left10Btn.Visible = true;
            }
        }
    }

    private void SetArrowVisibilityDC()
    {
        if (_gameObject.TierDC == _gameObject.MaxTierDC)
        {
            if (_gameObject.TierDC + 10 <= _gameObject.MaxTierDC)
            {
                _right10Btn.Visible = true;
            }
            else
            {
                _right10Btn.Visible = false;
            }

            _rightBtn.Visible = false;
        }
        else
        {
            _right10Btn.Visible = true;
            _rightBtn.Visible = true;
        }

        if (_gameObject.TierDC == 1)
        {
            _leftTier.Visible = false;
            _left10Btn.Visible = false;
        }
        else
        {
            _leftTier.Visible = true;
            if(_gameObject.TierDC - 10 > 0)
            {
                _left10Btn.Visible = true;
            }
        }
    }

    private void SetTierText(int tier)
    {
        string tierText;
        string tierNumber = tier.ToString();

        if (!_labrybuceBtn.Disabled)
        {
            tierText =  "Dungeon Floor:";
            if(tier >= _gameObject.TierCap && !_gameObject.ProgressFlagObject.EndgameUnlocked)
            {
                tierNumber = "[MAX]";
                _partyEditScreen.EmbarkButton.Text = "[E] Ascend";
            }
            else
            {
                if (tier >= _gameObject.TierCap)
                    tierNumber = "[CAPPED]";

                _partyEditScreen.EmbarkButton.Text = "[E] Embark";
            }
        }
        else
        {
            tierText = "Labrybuce Sector:";
            if (tier >= _gameObject.TierDCCap && !_gameObject.ProgressFlagObject.EndgameUnlocked)
            {
                tierNumber = "[MAX]";
                _partyEditScreen.EmbarkButton.Text = "[E] Ascend";
            }
            else
            {
                if (tier >= _gameObject.TierDCCap)
                    tierNumber = "[CAPPED]";

                _partyEditScreen.EmbarkButton.Text = "[E] Embark";
            }
        }
        
        _tierLabel.Text = $"{tierText} {tierNumber}";
    }
}
