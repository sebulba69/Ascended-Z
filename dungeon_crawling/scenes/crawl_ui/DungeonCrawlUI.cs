using AscendedZ;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonCrawlUI : Control
{
	private GridContainer _container;
	private Label _tier, _coordinates, _encounters, _orbs, _morbis, _pickaxes;
	private HBoxContainer _specialTileContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tier = GetNode<Label>("%Tier");
        _coordinates = GetNode<Label>("%Coordinates");
		_container = GetNode<GridContainer>("%PartyContainer");
		_encounters = GetNode<Label>("%EncountersRemaining");
		_orbs = GetNode<Label>("%OrbAmount");
        _morbis = GetNode<Label>("%MorbisAmount");
        _pickaxes = GetNode<Label>("%PickAxeAmount");
		_specialTileContainer = GetNode<HBoxContainer>("%SpecialTileContainer");
	}

	public void SetParty(int tier, List<BattlePlayer> players, int orbs, int morbis, int pickaxes, Dungeon dungeon)
	{
		_tier.Text = $"TIER: {tier}";

		_orbs.Text = orbs.ToString();
		_pickaxes.Text = pickaxes.ToString();
        _morbis.Text = morbis.ToString();

        foreach (var child in _container.GetChildren())
			_container.RemoveChild(child);

		var pScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_CRAWL_PARTY_MEMBER);

        foreach (var p in players)
		{
			var scene = pScene.Instantiate<PartyMemberDCDisplay>();
			_container.AddChild(scene);
			scene.SetPartyMember(p);
		}

		_encounters.Text = dungeon.RequiredString;
    }

	public void MineToolTipVisible(bool visible)
	{
		GetNode<Label>("%MineToolTip").Visible = visible;
	}

	public void SetSpecialTileContainer(List<SpecialTile> tiles)
	{
		foreach(var child in _specialTileContainer.GetChildren())
			_specialTileContainer.RemoveChild(child);

		var scene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_SP_TILE);

		foreach(var tile in tiles)
		{
			var sp = scene.Instantiate<DCSpecialTileIcon>();
			_specialTileContainer.AddChild(sp);
			sp.SetSpecialTile(tile);
		}
	}

	public void SetCoordinates(int x, int y)
	{
        _coordinates.Text = $"COORDS: [{y},{x}]";
    }
}
