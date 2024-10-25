using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class Equations
    {
        private static int FLOOR_CAP = 60;
        public static int GetVorpexLevelValue(int initialVCValue, int level)
        {
            int vorpex;
            try
            {
                vorpex = initialVCValue + level;
            }
            catch (Exception)
            {
                vorpex = int.MaxValue - 1;
            }
            return vorpex;
        }

        public static int GetOWMaxHPUpgrade(int hp, int level)
        {
            try
            {
                int boost = (int)(level / 1.5);
                if (boost == 0)
                    boost = 1;
                hp += ((level + 1) + boost);
            }
            catch (Exception)
            {
                hp = int.MaxValue - 1;
            }

            return hp;
        }

        public static int GetTierIndexBy10(int tier)
        {
            return GetTierIndexByN(tier, 10);
        }

        public static int GetTierIndexBy50(int tier)
        {
            return GetTierIndexByN(tier, 50);
        }

        public static int GetTierIndexBy25(int tier)
        {
            return GetTierIndexByN(tier, 25);
        }

        public static int GetTierIndexBy100(int tier)
        {
            return GetTierIndexByN(tier, 100);
        }

        public static int ApplyIntegerBoostPercentage(int integerValue, double percentage)
        {
            double intermediate = (integerValue * percentage) + 1;
            return (int)(integerValue + intermediate);
        }

        private static int GetTierIndexByN(int tier, int n)
        {
            int index = 0;
            if (tier >= n + 1)
            {
                tier--;
                index = (tier - (tier % n)) / n;
            }
            return index;
        }

        public static int GetDungeonCrawlEvents(int tier)
        {
            if (tier > FLOOR_CAP)
                tier = FLOOR_CAP;

            return (tier / 10) + 2;
        }
    }
}
