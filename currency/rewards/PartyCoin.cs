using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class PartyCoin : Currency
    {
        public PartyCoin()
        {
            Name = SkillAssets.PARTY_COIN_ICON;
            Icon = Name;
        }
    }
}
