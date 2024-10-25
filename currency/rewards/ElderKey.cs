using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class ElderKey : Currency
    {
        public ElderKey() 
        {
            Name = SkillAssets.ELDER_KEY_ICON;
            Icon = Name;
        }
    }
}
