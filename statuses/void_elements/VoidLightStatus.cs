using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    [JsonDerivedType(typeof(VoidLightStatus), typeDiscriminator: nameof(VoidLightStatus))]
    public class VoidLightStatus : ChangeElementStatus
    {
        public VoidLightStatus() : base()
        {
            _id = StatusId.VoidLightStatus;

            _elementToChange = skills.Elements.Light;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_LIGHT_ICON;

            Name = "Void Light";
        }

        public override Status Clone()
        {
            return new VoidLightStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
