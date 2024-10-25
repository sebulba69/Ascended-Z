using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    [JsonDerivedType(typeof(BuffElecStatus), typeDiscriminator: nameof(BuffElecStatus))]
    public class BuffElecStatus : ElementBuffStatus
    {
        public BuffElecStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Elec;
            BuffElement = skills.Elements.Elec;
            Name = "Buff Elec";
            Icon = SkillAssets.GetElementIconByElementEnum(BuffElement);
        }

        public override Status Clone()
        {
            return new BuffElecStatus
            {
                BuffElement = this.BuffElement,
                Amount = this.Amount,
                Stacks = _stacks,
                Icon = this.Icon
            };
        }
    }
}
