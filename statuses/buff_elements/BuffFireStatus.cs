using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    [JsonDerivedType(typeof(BuffFireStatus), typeDiscriminator: nameof(BuffFireStatus))]
    public class BuffFireStatus : ElementBuffStatus
    {
        public BuffFireStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Fire;
            BuffElement = skills.Elements.Fire;
            Name = "Buff Fire";
            Icon = SkillAssets.GetElementIconByElementEnum(BuffElement);
        }

        public override Status Clone()
        {
            return new BuffFireStatus
            {
                BuffElement = this.BuffElement,
                Amount = this.Amount,
                Stacks = _stacks,
                Icon = this.Icon
            };
        }
    }
}
