using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class ProofOfAscension : Currency
    {
        public ProofOfAscension()
        {
            this.Name = SkillAssets.PROOF_OF_ASCENSION_ICON;
            this.Icon = Name;
        }
    }

    public class ProofOfBuce : Currency
    {
        public ProofOfBuce()
        {
            this.Name = SkillAssets.PROOF_OF_BUCE_ICON;
            this.Icon = Name;
        }
    }
}
