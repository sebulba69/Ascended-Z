using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class CopyCatEnemyFactory : EnemyFactory
    {
        public CopyCatEnemyFactory()
        {
            _functionDictionary[EnemyNames.Naldbear] = MakeNaldbear;
            _functionDictionary[EnemyNames.Stroma_Hele] = MakeStromaHele;
            _functionDictionary[EnemyNames.Sylla] = MakeSylla;
            _functionDictionary[EnemyNames.Venforth] = MakeVenforth;
        }
        public Enemy MakeNaldbear()
        {
            var naldbear = MakeCopyCatEnemy(EnemyNames.Naldbear, 12);

            naldbear.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            naldbear.Skills.Add(SkillDatabase.Elec1.Clone());

            return naldbear;
        }

        public Enemy MakeStromaHele()
        {
            var stroma = MakeCopyCatEnemy(EnemyNames.Stroma_Hele, 12);

            stroma.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);

            stroma.Skills.Add(SkillDatabase.Fire1.Clone());

            return stroma;
        }

        public Enemy MakeSylla()
        {
            var sylla = MakeCopyCatEnemy(EnemyNames.Sylla, 12);

            sylla.Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            sylla.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            sylla.Skills.Add(SkillDatabase.IceAll.Clone());

            return sylla;
        }

        public Enemy MakeVenforth()
        {
            var venforth = MakeCopyCatEnemy(EnemyNames.Venforth, 12);

            venforth.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            venforth.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            venforth.Skills.Add(SkillDatabase.ElecAll.Clone());

            return venforth;
        }

        protected Enemy MakeCopyCatEnemy(string name, int hp)
        {
            return new CopyCatEnemy
            {
                Name = $"[CC] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
