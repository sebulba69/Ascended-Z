using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class KeyShard : Currency
    {
        public KeyShard()
        {
            Name = SkillAssets.KEY_SHARD;
            Icon = Name;
        }
    }
}
