using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    [JsonDerivedType(typeof(BuffWindStatus), typeDiscriminator: nameof(BuffWindStatus))]
    public class BuffWindStatus : ElementBuffStatus
    {
        public BuffWindStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Wind;
            BuffElement = Elements.Wind;
            Name = "Buff Wind";
            Icon = SkillAssets.GetElementIconByElementEnum(BuffElement);
        }

        public override Status Clone()
        {
            return new BuffWindStatus
            {
                BuffElement = this.BuffElement,
                Amount = this.Amount,
                Stacks = _stacks,
                Icon = this.Icon
            };
        }
    }
}
