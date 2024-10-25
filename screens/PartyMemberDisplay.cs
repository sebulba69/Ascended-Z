using AscendedZ;
using AscendedZ.battle;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using AscendedZ.screens.upgrade_screen;
using Godot;
using System;
using System.Text;

public partial class PartyMemberDisplay : HBoxContainer
{
    private readonly PackedScene SIGIL_ICON = ResourceLoader.Load<PackedScene>(Scenes.SIGIL_ICON);

    private TextureRect _playerPicture;
    private TextEdit _description;
    private HBoxContainer _sigilDisplay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playerPicture = this.GetNode<TextureRect>("%PlayerProfile");
        _description = this.GetNode<TextEdit>("%DescriptionBox");
        _sigilDisplay = GetNode<HBoxContainer>("%SigilDisplay");
    }

    /// <summary>
    /// Sets up all the values and the descriptions of the party member associated with this UI.
    /// </summary>
    /// <param name="partyMember"></param>
    public void DisplayPartyMember(int index, bool isReserve)
    {
        OverworldEntity partyMember;
        var main = PersistentGameObjects.GameObjectInstance().MainPlayer;

        if (isReserve)
            partyMember = main.ReserveMembers[index];
        else
            partyMember = main.Party.Party[index];

        if (partyMember != null)
        {
            ShowRandomEntity(new EntityUIWrapper { Entity = partyMember });
        }
    }

    public void ShowRandomEntity(EntityUIWrapper wrapper)
    {
        var partyMember = wrapper.Entity;
        _playerPicture.Texture = ResourceLoader.Load<Texture2D>(partyMember.Image);

        StringBuilder description = new StringBuilder();
        description.AppendLine(partyMember.DisplayName + " ● Fusion Grade " + partyMember.FusionGrade);
        description.Append(partyMember.ToString().TrimEnd('r', '\n'));

        _description.Text = description.ToString();

        foreach (var child in _sigilDisplay.GetChildren())
        {
            _sigilDisplay.RemoveChild(child);
        }

        foreach (var sigil in partyMember.Sigils)
        {
            if (sigil.Index > -1)
            {
                var icon = SIGIL_ICON.Instantiate<SigilItemIcon>();
                _sigilDisplay.AddChild(icon);
                icon.SetPic(sigil.Image);
            }
        }
    }

    public void ShowFusionEntity(EntityUIWrapper wrapper) 
    {
        var partyMember = wrapper.Entity;
        _playerPicture.Texture = ResourceLoader.Load<Texture2D>(partyMember.Image);

        StringBuilder description = new StringBuilder();
        description.AppendLine(partyMember.DisplayName);
        description.Append(partyMember.GetFusionString().TrimEnd('r', '\n'));

        _description.Text = description.ToString();
    }

    public void Clear()
    {
        _playerPicture.Texture = null;
        _description.Text = string.Empty;

        foreach (var child in _sigilDisplay.GetChildren())
        {
            _sigilDisplay.RemoveChild(child);
        }
    }
}
