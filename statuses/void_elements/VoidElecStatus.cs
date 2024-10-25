using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    [JsonDerivedType(typeof(VoidElecStatus), typeDiscriminator: nameof(VoidElecStatus))]
    public class VoidElecStatus : ChangeElementStatus
    {
        public VoidElecStatus() : base()
        {
            _id = StatusId.VoidElecStatus;

            _elementToChange = skills.Elements.Elec;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_ELEC_ICON;

            Name = "Void Elec";
        }

        public override Status Clone()
        {
            return new VoidElecStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
