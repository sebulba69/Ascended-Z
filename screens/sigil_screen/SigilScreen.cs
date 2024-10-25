using AscendedZ;
using AscendedZ.battle;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.entities.sigils;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using AscendedZ.screens.sigil_screen;
using Godot;
using System;
using System.Collections.Generic;

public partial class SigilScreen : CenterContainer
{
    private Label _dellencoinLabel;

	private PartyMemberDisplay _partyMemberDisplay;

    private OverworldEntity _entity;
    private List<SigilEquipMenuScene> _sigilScenes;

    private Currency _dellencoin;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _partyMemberDisplay = GetNode<PartyMemberDisplay>("%PartyMemberIcon");
        _dellencoinLabel = GetNode<Label>("%NameLabel");

        _dellencoin =
            PersistentGameObjects
                .GameObjectInstance()
                .MainPlayer
                .Wallet
                .Currency[SkillAssets.DELLENCOIN];

        _dellencoinLabel.Text = $"{_dellencoin.Amount:n0}";

        Button back = GetNode<Button>("%BackButton");
		back.Pressed += QueueFree;
	}

    public void SetEntity(OverworldEntity entity)
    {
        _entity = entity;

        _partyMemberDisplay.ShowRandomEntity(new EntityUIWrapper() { Entity = entity });

        _sigilScenes = 
        [ 
            GetNode<SigilEquipMenuScene>("%SigilScene"), 
            GetNode<SigilEquipMenuScene>("%SigilScene2"), 
            GetNode<SigilEquipMenuScene>("%SigilScene3")
        ];

        for (int i = 0; i < _sigilScenes.Count; i++) 
        {
            var sigilScene = _sigilScenes[i];
            var wrapper = new SigilWrapper() 
            {
                PartyMemberDisplay = _partyMemberDisplay,
                Entity = _entity,
                Dellencoin = _dellencoin,
                DellencoinLabel = _dellencoinLabel
            };

            sigilScene.SetSigils(wrapper, i);
            sigilScene.EquipChanged += _OnEquippedChanged;
        }
    }

    private void _OnEquippedChanged(object sender, int index) 
    {
        _partyMemberDisplay.ShowRandomEntity(new EntityUIWrapper() { Entity = _entity });

        for (int i = 0; i < _sigilScenes.Count; i++)
        {
            var sigilScene = _sigilScenes[i];
            sigilScene.UpdateSigils(i);
        }

        // save here
        PersistentGameObjects.Save();
    }
}
