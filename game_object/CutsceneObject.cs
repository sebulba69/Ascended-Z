using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class CutsceneObject
    {
        public HashSet<int> EndOfTierCutscenesWatched { get; set; }
        public HashSet<int> StartOfTierCutscenesWatched { get; set; }
        public HashSet<int> StartOfLabrybuceWatched { get; set; }
        public HashSet<int> SpecialBossCutscenesWatchedEndless { get; set; }
        public HashSet<int> SpecialBossCutscenesWatchedLabrybuce { get; set; }

        public CutsceneObject()
        {
            if(EndOfTierCutscenesWatched == null)
            {
                EndOfTierCutscenesWatched = new();
                StartOfTierCutscenesWatched = new();
                StartOfLabrybuceWatched = new();
                SpecialBossCutscenesWatchedEndless = new();
                SpecialBossCutscenesWatchedLabrybuce = new();
            }
        }
    }
}
