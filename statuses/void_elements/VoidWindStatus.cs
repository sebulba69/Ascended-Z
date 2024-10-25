using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    [JsonDerivedType(typeof(VoidWindStatus), typeDiscriminator: nameof(VoidWindStatus))]
    public class VoidWindStatus : ChangeElementStatus
    {
        public VoidWindStatus() : base()
        {
            _id = StatusId.VoidWindStatus;

            _elementToChange = skills.Elements.Wind;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_WIND_ICON;

            Name = "Void Wind";
        }

        public override Status Clone()
        {
            return new VoidWindStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
