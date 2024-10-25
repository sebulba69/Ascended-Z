using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class ProgressFlagObject
    {
        public bool AddFusionTutorial { get; set; }
        public bool CustomPartyMembersViewed { get; set; }
        public bool PrimaryWeaponEquippedForFirstTime { get; set; }
        public HashSet<int> ViewedDCCutscenes { get; set; }
        public bool FinalBossDefeated { get; set; }
        public bool PostFinalBossCutsceneWatched { get; set; }
        public bool EndgameUnlocked { get; set; }
        public bool FinalCutsceneWatched { get; set; }
        public HashSet<int> ViewedSkillProgressTiers { get; set; }
        public HashSet<int> ElderKeysCollected { get; set; }


        public readonly List<int> SKILL_PROGRESS_TIERS =
        [
            TierRequirements.TIER2_STRONGER_ENEMIES,
            TierRequirements.TIER5_STRONGER_ENEMIES,
            TierRequirements.TIER6_STRONGER_ENEMIES,
            TierRequirements.TIER7_STRONGER_ENEMIES,
            TierRequirements.TIER8_STRONGER_ENEMIES,
            TierRequirements.TIER9_STRONGER_ENEMIES,
            TierRequirements.TIER10_STRONGER_ENEMIES,
            TierRequirements.TIER15_STRONGER_ENEMIES,
            260
        ];

        public ProgressFlagObject()
        {
            ViewedDCCutscenes = new();
            ViewedSkillProgressTiers = new();
            ElderKeysCollected = new();
        }
    }
}
