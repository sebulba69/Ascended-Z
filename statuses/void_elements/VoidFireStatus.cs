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
    [JsonDerivedType(typeof(VoidFireStatus), typeDiscriminator: nameof(VoidFireStatus))]
    public class VoidFireStatus : ChangeElementStatus
    {
        public VoidFireStatus() : base()
        {
            _id = StatusId.VoidFireStatus;

            _elementToChange = skills.Elements.Fire;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_FIRE_ICON;

            Name = "Void Fire";
        }

        public override Status Clone()
        {
            return new VoidFireStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
