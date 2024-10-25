using AscendedZ.currency;
using AscendedZ.entities.partymember_objects;
using Godot;

namespace AscendedZ.screens.sigil_screen
{
    public class SigilWrapper
    {
        public PartyMemberDisplay PartyMemberDisplay { get; set; }
        public OverworldEntity Entity { get; set; }
        public Currency Dellencoin { get; set; }
        public Label DellencoinLabel { get; set; }
    }
}
