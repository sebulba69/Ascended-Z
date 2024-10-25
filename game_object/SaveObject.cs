using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    /// <summary>
    /// Separate object for handling all forms of save data.
    /// </summary>
    public class SaveObject
    {
        /// <summary>
        /// Path for saving the current instance of the game.
        /// </summary>
        public string SavePathForCurrentGame { get; set; }

        public List<SaveEntry> SaveCache { get; set; }

        public SaveObject() {}

        /// <summary>
        /// Load the save cache from your save file, or creat it if none exist.
        /// </summary>
        public void Initialize(string saveCachePath)
        {
            SaveCache = JsonUtil.LoadObject<List<SaveEntry>>(saveCachePath);

            // if we didn't load anything, then initialize a new savecache
            if (SaveCache == null)
            {
                SaveCache = new List<SaveEntry>();
            }
        }

        public void SaveSaveCache(string saveCachePath)
        {
            JsonUtil.SaveObject(SaveCache, saveCachePath);
        }
    }
}
