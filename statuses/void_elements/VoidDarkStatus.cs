using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    [JsonDerivedType(typeof(VoidDarkStatus), typeDiscriminator: nameof(VoidDarkStatus))]
    public class VoidDarkStatus : ChangeElementStatus
    {
        public VoidDarkStatus() : base()
        {
            _id = StatusId.VoidDarkStatus;

            _elementToChange = skills.Elements.Dark;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_DARK_ICON;

            Name = "Void Dark";
        }

        public override Status Clone()
        {
            return new VoidDarkStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
