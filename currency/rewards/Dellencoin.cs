using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class Dellencoin : Currency
    {
        public Dellencoin()
        {
            this.Name = SkillAssets.DELLENCOIN;
            this.Icon = Name;
        }
    }
}
