using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.statuses.weak_element
{
    [JsonDerivedType(typeof(WeakIceStatus), typeDiscriminator: nameof(WeakIceStatus))]
    public class WeakIceStatus : ChangeElementStatus
    {
        public WeakIceStatus() : base()
        {
            _id = StatusId.WexIceStatus;

            _elementToChange = skills.Elements.Ice;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_ICE_ICON;

            _turnCount = 2;

            Name = "Weak Ice";
        }

        public override Status Clone()
        {
            return new WeakIceStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
