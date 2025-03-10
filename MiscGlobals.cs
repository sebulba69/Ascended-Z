﻿using AscendedZ.game_object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class MiscGlobals
    {
        public static int FUSION_GRADE_CAP = 6;

        public static int GetSoftcap()
        {
            int maxTier = PersistentGameObjects.GameObjectInstance().MaxTier;

            // level cap
            if (maxTier >= 150)
                return 151;

            if (maxTier <= 10)
                return 11;

            int remainder = maxTier % 10;

            if (remainder == 0)
            {
                return maxTier + 1;
            }
            else
            {
                int softCap = maxTier - remainder;

                return softCap + 11;
            }
        }
    }
}
