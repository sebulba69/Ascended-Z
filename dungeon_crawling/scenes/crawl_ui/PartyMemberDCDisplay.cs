using AscendedZ;
using AscendedZ.entities.battle_entities;
using Godot;
using System;

public partial class PartyMemberDCDisplay : PanelContainer
{
	private TextureRect _picture;
	private TextureProgressBar _hp;
	private Label _name;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_picture = GetNode<TextureRect>("%Picture");
		_hp = GetNode<TextureProgressBar>("%HP");
		_name = GetNode<Label>("%NameLabel");
	}

	public void SetPartyMember(BattlePlayer member)
	{
		_picture.Texture = ResourceLoader.Load<Texture2D>(member.Image);
		_hp.MaxValue = member.MaxHP;
		_hp.Value = member.HP;
		_name.Text = member.Name;
	}
}
