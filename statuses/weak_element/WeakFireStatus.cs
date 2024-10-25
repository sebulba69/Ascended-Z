using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.weak_element
{
    [JsonDerivedType(typeof(WeakFireStatus), typeDiscriminator: nameof(WeakFireStatus))]
    public class WeakFireStatus : ChangeElementStatus
    {
        public WeakFireStatus() : base()
        {
            _id = StatusId.WexFireStatus;

            _elementToChange = skills.Elements.Fire;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_FIRE_ICON;

            _turnCount = 2;

            Name = "Weak Fire";
        }

        public override Status Clone()
        {
            return new WeakFireStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }

    }
}
