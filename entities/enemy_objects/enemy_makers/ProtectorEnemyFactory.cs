using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class ProtectorEnemyFactory : EnemyFactory
    {
        public ProtectorEnemyFactory()
        {
            _functionDictionary[EnemyNames.Ed] = MakeEd;
            _functionDictionary[EnemyNames.Otem] = MakeOtem;
            _functionDictionary[EnemyNames.Hesret] = MakeHesret;
            _functionDictionary[EnemyNames.LaChris] = MakeLachris;
        }

        public Enemy MakeEd()
        {
            Elements elementToVoid = Elements.Fire;

            var ed = MakeProtectorEnemy(EnemyNames.Ed, 8, elementToVoid);

            ed.Skills.Add(SkillDatabase.VoidFire.Clone());
            ed.Skills.Add(SkillDatabase.Fire1.Clone());
            ed.Skills.Add(SkillDatabase.Light1.Clone());
            ed.Skills.Add(SkillDatabase.Elec1.Clone());

            return ed;
        }

        public Enemy MakeOtem()
        {
            Elements elementToVoid = Elements.Ice;

            var otem = MakeProtectorEnemy(EnemyNames.Otem, 8, elementToVoid);

            otem.Skills.Add(SkillDatabase.VoidIce.Clone());
            otem.Skills.Add(SkillDatabase.Ice1.Clone());
            otem.Skills.Add(SkillDatabase.Dark1.Clone());
            otem.Skills.Add(SkillDatabase.Wind1.Clone());

            return otem;
        }

        public Enemy MakeHesret()
        {
            Elements elementToVoid = Elements.Wind;

            var hesret = MakeProtectorEnemy(EnemyNames.Hesret, 8, elementToVoid);

            hesret.Skills.Add(SkillDatabase.VoidWind.Clone());
            hesret.Skills.Add(SkillDatabase.Wind1.Clone());
            hesret.Skills.Add(SkillDatabase.Ice1.Clone());
            hesret.Skills.Add(SkillDatabase.Fire1.Clone());

            return hesret;
        }

        public Enemy MakeLachris()
        {
            Elements elementToVoid = Elements.Light;

            var isumforth = MakeProtectorEnemy(EnemyNames.LaChris, 11, elementToVoid);

            isumforth.Skills.Add(SkillDatabase.VoidLight.Clone());
            isumforth.Skills.Add(SkillDatabase.WindAll.Clone());
            isumforth.Skills.Add(SkillDatabase.ElecAll.Clone());
            isumforth.Skills.Add(SkillDatabase.IceAll.Clone());

            return isumforth;
        }

        private Enemy MakeProtectorEnemy(string name, int hp, Elements elementToVoid)
        {
            var protector = new ProtectorEnemy()
            {
                Name = $"[PRCT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                ElementToVoid = elementToVoid
            };

            protector.Resistances.SetResistance(ResistanceType.Wk, elementToVoid);

            return protector;
        }
    }
}
