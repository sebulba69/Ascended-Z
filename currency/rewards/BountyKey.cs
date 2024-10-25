using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class BountyKey : Currency
    {
        public BountyKey() 
        {
            Name = SkillAssets.BOUNTY_KEY;
            Icon = Name;
        }
    }
}
