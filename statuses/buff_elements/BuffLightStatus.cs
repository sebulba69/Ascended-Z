using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.statuses.buff_elements
{
    [JsonDerivedType(typeof(BuffLightStatus), typeDiscriminator: nameof(BuffLightStatus))]
    public class BuffLightStatus : ElementBuffStatus
    {
        public BuffLightStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Light;
            BuffElement = skills.Elements.Light;
            Name = "Buff Light";
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
