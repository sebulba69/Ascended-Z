using AscendedZ.currency;
using AscendedZ.currency.rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class RewardsCalculator
    {
        private static int REWARD_MULTIPLIER = 15;

        public static List<Currency> GetDungeonCrawlCompletionRewards(int tier)
        {
            return new List<Currency>()
            {
                new Vorpex() { Amount = tier * GetMultiplier(tier, (REWARD_MULTIPLIER + 2)) },
                new PartyCoin() { Amount = tier * GetMultiplier(tier, (REWARD_MULTIPLIER - 5)) },
                new Dellencoin() { Amount = tier * GetMultiplier(tier,REWARD_MULTIPLIER) },
            };
        }

        public static int GetMultiplier(int tier, int rewardMult)
        {
            if (tier / 100 > 0)
            {
                rewardMult *= (tier / 100) + 1;
            }

            return rewardMult;
        }
    }
}
