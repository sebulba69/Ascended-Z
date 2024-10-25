using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    [JsonDerivedType(typeof(BuffDarkStatus), typeDiscriminator: nameof(BuffDarkStatus))]
    public class BuffDarkStatus : ElementBuffStatus
    {
        public BuffDarkStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Dark;
            BuffElement = skills.Elements.Dark;
            Name = "Buff Dark";
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
