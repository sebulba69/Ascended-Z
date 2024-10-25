using AscendedZ.entities.sigils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class SigilDatabase
    {
        public static List<Sigil> MakeSigils()
        {
            return [
                    MakeSigil("HP", 0),
                    MakeSigil("Fire", 1),
                    MakeSigil("Ice", 2),
                    MakeSigil("Elec", 3),
                    MakeSigil("Wind", 4),
                    MakeSigil("Dark", 5),
                    MakeSigil("Light", 6)
                ];
        }

        private static Sigil MakeSigil(string name, int id)
        {
            var sigil = new Sigil
            {
                Name = $"{name} Sigil",
                Level = 1,
                Image = SigilAssets.SigilImages[id],
                StatIndex = id
            };

            return sigil;
        }
    }
}
