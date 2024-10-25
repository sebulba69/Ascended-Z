using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.sigils
{
    public class Sigil
    {
        private int _levelUpCost = 1;
        private double _boostPercentage = 0.01;

        public int Index { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        /// <summary>
        /// HP, Fire, Ice, Elec, Wind, Dark, Light
        /// </summary>
        public int StatIndex { get; set; }
        public double BoostPercentage { get => _boostPercentage; set => _boostPercentage = value; }
        public int LevelUpCost { get => _levelUpCost; set => _levelUpCost = value; }

        public bool CanLevelUp() => Level + 1 <= PersistentGameObjects.GameObjectInstance().SigilLevelCap;

        public Sigil()
        {
            Index = -1;
        }

        public void LevelUp()
        {
            Level++;

            LevelUpCost = Equations.GetVorpexLevelValue(LevelUpCost, Level);
            if (LevelUpCost > 3000)
                LevelUpCost = 3000;

            _boostPercentage += 0.01;
            _boostPercentage = Math.Round(_boostPercentage, 2); // avoid weird floating point math
        }

        public string GetDisplayString()
        {
            string level = $"[L.{Level}]";

            if (Level == PersistentGameObjects.GameObjectInstance().SigilLevelCap)
            {
                level = "[MAX]";
            }
            string name =  $"{level} {Name}";

            if (Index > -1)
                name = $"[{Name} EQUIPPED]";

            return name;
        }
    }
}
