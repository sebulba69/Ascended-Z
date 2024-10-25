using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.statuses.weak_element
{
    [JsonDerivedType(typeof(WeakDarkStatus), typeDiscriminator: nameof(WeakDarkStatus))]
    public class WeakDarkStatus : ChangeElementStatus
    {
        public WeakDarkStatus() : base()
        {
            _id = StatusId.WexDarkStatus;

            _elementToChange = skills.Elements.Dark;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_DARK_ICON;

            _turnCount = 2;

            Name = "Weak Dark";
        }

        public override Status Clone()
        {
            return new WeakDarkStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
