using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class TierRequirements
    {
        // tiers 10, 20, 30, 40, 50, 150 = the point at which the grade of fusion is allowed
        private static readonly int[] FUSION_TIERS = { 10, 20, 30, 40, 50, 150 };

        /// <summary>
        /// 5
        /// </summary>
        public static int UPGRADE_SCREEN = 5;

        /// <summary>
        /// 10
        /// </summary>
        public static int TIER2_STRONGER_ENEMIES = 10;

        /// <summary>
        /// 15
        /// </summary>
        public static int TIER3_STRONGER_ENEMIES = 15;
        public static int FUSE = 15;

        /// <summary>
        /// 20
        /// </summary>
        public static int TIER4_STRONGER_ENEMIES = 20;
        public static int QUESTS_FUSION_MEMBERS = 20;

        /// <summary>
        /// 20
        /// </summary>
        public static int QUESTS_PARTY_MEMBERS_UPGRADE = 20;

        /// <summary>
        /// 30
        /// </summary>
        public static int QUESTS_ALL_FUSION_MEMBERS = 30;

        public static int SIGILS = 80;
        /// <summary>
        /// 40
        /// </summary>
        public static int TIER5_STRONGER_ENEMIES = 40;
        /// <summary>
        /// 50
        /// </summary>
        public static int TIER6_STRONGER_ENEMIES = 50;
        /// <summary>
        /// 70
        /// </summary>
        public static int TIER7_STRONGER_ENEMIES = 70;
        /// <summary>
        /// 100
        /// </summary>
        public static int TIER8_STRONGER_ENEMIES = 100;
        /// <summary>
        /// 130
        /// </summary>
        public static int TIER9_STRONGER_ENEMIES = 130;
        /// <summary>
        /// 150
        /// </summary>
        public static int TIER10_STRONGER_ENEMIES = 150;
        /// <summary>
        /// 160
        /// </summary>
        public static int TIER11_STRONGER_ENEMIES = 160;
        /// <summary>
        /// 170
        /// </summary>
        public static int TIER12_STRONGER_ENEMIES = 170;
        /// <summary>
        /// 190
        /// </summary>
        public static int TIER13_STRONGER_ENEMIES = 190;
        /// <summary>
        /// 210
        /// </summary>
        public static int TIER14_STRONGER_ENEMIES = 210;
        /// <summary>
        /// 230
        /// </summary>
        public static int TIER15_STRONGER_ENEMIES = 230;

        public static int GetFusionTierRequirement(int fusionGrade)
        {
            int index = fusionGrade - 1;
            if(index >= FUSION_TIERS.Length)
            {
                return -1;
            }
            else
            {
                return FUSION_TIERS[index];
            }
        }
    }
}
