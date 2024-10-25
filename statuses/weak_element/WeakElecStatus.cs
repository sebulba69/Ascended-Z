using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.weak_element
{
    [JsonDerivedType(typeof(WeakElecStatus), typeDiscriminator: nameof(WeakElecStatus))]
    public class WeakElecStatus : ChangeElementStatus
    {
        public WeakElecStatus() : base()
        {
            _id = StatusId.WexElecStatus;

            _elementToChange = skills.Elements.Elec;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_ELEC_ICON;

            _turnCount = 2;

            Name = "Weak Elec";
        }

        public override Status Clone()
        {
            return new WeakElecStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
